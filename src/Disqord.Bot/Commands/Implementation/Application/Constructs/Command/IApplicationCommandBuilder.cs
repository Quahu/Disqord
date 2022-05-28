// using System.Collections.Generic;
// using Qmmands;
//
// namespace Disqord.Bot.Commands.Application;
//
// public interface IApplicationCommandBuilder : ICommandBuilder
// {
//     new IApplicationModuleBuilder Module { get; }
//
//     IModuleBuilder ICommandBuilder.Module => Module;
//
//     new IList<IApplicationParameterBuilder> Parameters { get; }
//
//     string? Alias { get; set; }
//
//     ApplicationCommandType Type { get; set; }
//
//     IApplicationCommand Build(IApplicationModule module);
//
//     ICommand ICommandBuilder.Build(IModule module)
//         => Build((IApplicationModule) module);
// }
