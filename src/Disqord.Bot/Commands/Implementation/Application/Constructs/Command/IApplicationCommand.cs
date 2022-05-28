// using System.Collections.Generic;
// using Qmmands;
//
// namespace Disqord.Bot.Commands.Application;
//
// public interface IApplicationCommand : ICommand
// {
//     new IApplicationModule Module { get; }
//
//     IModule ICommand.Module => Module;
//
//     new IReadOnlyList<IApplicationParameter> Parameters { get; }
//
//     IReadOnlyList<IParameter> ICommand.Parameters => Parameters;
//
//     string Alias { get; }
//
//     ApplicationCommandType Type { get; set; }
// }
