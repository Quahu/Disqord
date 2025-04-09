﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disqord.Http;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;

namespace Disqord.Rest;

public class RestApiException : Exception
{
    /// <summary>
    ///     Gets the full API route that failed to execute.
    /// </summary>
    /// <remarks>
    ///     This route may include sensitive information such as webhook tokens and should not be displayed or exposed in public logs.
    ///     <para/>
    ///     <see cref="IFormattedRoute.BaseRoute"/> can be utilized instead for public-facing route logging.
    /// </remarks>
    public IFormattedRoute Route { get; }
    
    /// <summary>
    ///     Gets the HTTP failure response.
    /// </summary>
    public IHttpResponse HttpResponse { get; set; }

    /// <summary>
    ///     Gets the status code sent by the API.
    /// </summary>
    public HttpResponseStatusCode StatusCode => HttpResponse.StatusCode;

    /// <summary>
    ///     Gets the reason phrase describing the failure.
    /// </summary>
    public string? ReasonPhrase { get; }

    /// <summary>
    ///     Gets the error model sent by the API describing the error.
    /// </summary>
    /// <returns>
    ///     The JSON error model or <see langword="null"/> for <c>5xx</c> status codes.
    /// </returns>
    public RestApiErrorJsonModel? ErrorModel { get; }

    /// <summary>
    ///     Gets whether Discord failed to fulfill the request, i.e. Discord is the one at fault and returned a <c>5xx</c> status code..
    /// </summary>
    public bool IsServerError
    {
        get
        {
            var statusCode = (int) StatusCode;
            return statusCode > 499 && statusCode < 600;
        }
    }

    public RestApiException(IFormattedRoute route, IHttpResponse httpResponse, string? reasonPhrase, RestApiErrorJsonModel? errorModel)
        : base(GetErrorMessage(route.BaseRoute, httpResponse.StatusCode, reasonPhrase, errorModel))
    {
        Route = route;
        HttpResponse = httpResponse;
        ReasonPhrase = reasonPhrase;
        ErrorModel = errorModel;
    }

    /// <summary>
    ///     Checks if this exception represents the specified <see cref="RestApiErrorCode"/>.
    /// </summary>
    /// <param name="code"> The code to check for. </param>
    /// <returns>
    ///     <see langword="true"/> if this exception represents the code.
    /// </returns>
    public bool IsError(RestApiErrorCode code)
    {
        var errorModel = ErrorModel;
        return errorModel != null && errorModel.Code == code;
    }

    private static string GetErrorMessage(IFormattableRoute route, HttpResponseStatusCode statusCode, string? reasonPhrase, RestApiErrorJsonModel? errorModel)
    {
        var messageBuilder = new StringBuilder($"Failed to execute route {route.Method}|{route.Path}.\n");

        messageBuilder.Append($"HTTP: {(Enum.IsDefined(statusCode) ? $"{(int)statusCode} {statusCode}" : statusCode)}. ");

        if (errorModel == null)
            return messageBuilder.Append($"Reason phrase: {reasonPhrase}").ToString();

        // HTTP: 400 BadRequest. Error message: Invalid Form Body
        messageBuilder.Append($"Error message: {errorModel.Message}");

        // We check if Discord provided more detailed error messages.
        if (errorModel.ExtensionData.TryGetValue("errors", out var errors) && errors is IJsonObject errorsObject)
        {
            // We extract the errors from the structure shown below.
            var extracted = ExtractErrors(errorsObject);

            // We append the errors the message created above, example:
            // embed.fields[0].name: "Must be 256 or fewer in length."
            // embed.fields[1].value: "This field is required"
            messageBuilder.AppendJoin('\n', extracted.Select(x => $"{x.Key}: {x.Value}"));
            //message += $"\n{string.Join('\n', extracted.Select(x => $"{x.Key}: {x.Value ?? "unknown error"}"))}";
        }

        /*
        *  Example error model structure:
        *  {
        *      "code": 50035,
        *      "message": "Invalid Form Body",
        *      "errors": {
        *          "embed": {
        *              "fields": {
        *                  "0": {
        *                      "_errors": [
        *                          {
        *                              "code": "LIST_ITEM_VALUE_REQUIRED",
        *                              "message": "List item values of ModelType are required"
        *                          }
        */
        static IEnumerable<KeyValuePair<string, string>> ExtractErrors(IJsonObject jsonObject, string? key = null)
        {
            // We enumerate the fields in the `errors` JSON object.
            foreach (var (name, value) in jsonObject)
            {
                // We create the key that will be used in the output and in recursive calls.
                // If the key is an integer it means it's an index in an array instead, so we format it differently.
                var newKey = key != null
                    ? int.TryParse(name, out var index)
                        ? $"{key}[{index}]"
                        : $"{key}.{name}"
                    : name;

                // If the value is not a JSON object, just ToString whatever it is.
                if (value is not IJsonObject valueObject)
                {
                    yield return KeyValuePair.Create(newKey, value?.ToString() ?? "");
                    continue;
                }

                // If the value has no `_errors` field it means there's more nested data, recurse.
                if (!valueObject.TryGetValue("_errors", out var errors))
                {
                    foreach (var error in ExtractErrors(valueObject, newKey))
                    {
                        yield return error;
                    }
                    
                    continue;
                }

                // Skip non-array errors, just in case.
                if (errors is not IJsonArray errorsArray)
                    continue;

                // Add the key and the message/code for it.
                var messages = errorsArray.OfType<IJsonObject>()
                    .Select(static x => (x.GetValueOrDefault("message") ?? x.GetValueOrDefault("code"))?.ToString());

                yield return KeyValuePair.Create(newKey, string.Join("; ", messages));
            }
        }

        return messageBuilder.ToString();
    }
}
