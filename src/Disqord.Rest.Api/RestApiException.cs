using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Http;
using Disqord.Rest.Api;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    public class RestApiException : Exception
    {
        public HttpResponseStatusCode StatusCode { get; }

        public RestApiErrorJsonModel ErrorModel { get; }

        public RestApiException(HttpResponseStatusCode statusCode, RestApiErrorJsonModel errorModel)
            : base(GetMessage(statusCode, errorModel))
        {
            StatusCode = statusCode;
            ErrorModel = errorModel;
        }

        private static string GetMessage(HttpResponseStatusCode statusCode, RestApiErrorJsonModel errorModel)
        {
            // We create an easily readable exception message, example:
            // HTTP: 400 BadRequest. Error message: Invalid Form Body
            var message = $"HTTP: {(Enum.IsDefined(statusCode) ? $"{(int) statusCode} {statusCode}" : statusCode)}. Error message: {errorModel.Message}";

            // We check if Discord provided more detailed error messages.
            if (errorModel.ExtensionData.TryGetValue("errors", out var errors))
            {
                // We extract the errors from the structure shown below.
                var extracted = ExtractErrors(errors as IJsonObject);

                // We append the errors the message created above, example:
                // embed.fields[0].name: "Must be 256 or fewer in length."
                // embed.fields[1].value: "This field is required"
                message += $"\n{string.Join('\n', extracted.Select(x => $"{x.Key}: {x.Value ?? "unknown error"}"))}";
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
            static IEnumerable<KeyValuePair<string, string>> ExtractErrors(IJsonObject jsonObject, string key = null)
            {
                var extracted = new List<KeyValuePair<string, string>>();
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
                        extracted.Add(KeyValuePair.Create(newKey, value.ToString()));
                        continue;
                    }

                    // If the value has no `_errors` field it means there's more nested data, recurse.
                    if (!valueObject.TryGetValue("_errors", out var errors))
                    {
                        extracted.AddRange(ExtractErrors(valueObject, newKey));
                        continue;
                    }

                    // Skip non-array errors, just in case.
                    if (errors is not IJsonArray errorsArray)
                        continue;

                    // Add the key and the message/code for it.
                    extracted.Add(KeyValuePair.Create(newKey, string.Join(' ', errorsArray.Select(x =>
                    {
                        if (x is not IJsonObject jsonObject)
                            return null;

                        return (jsonObject.GetValueOrDefault("message") ?? jsonObject.GetValueOrDefault("code"))?.ToString();
                    }))));
                }

                return extracted;
            }

            return message;
        }
    }
}
