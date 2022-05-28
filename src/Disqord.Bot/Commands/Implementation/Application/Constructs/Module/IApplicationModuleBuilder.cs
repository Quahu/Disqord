// using System.Collections.Generic;
// using Qmmands;
//
// namespace Disqord.Bot.Commands.Application;
//
// public interface IApplicationModuleBuilder : IModuleBuilder
// {
//     new IApplicationModuleBuilder? Parent { get; }
//
//     IModuleBuilder? IModuleBuilder.Parent => Parent;
//
//     new IList<IApplicationModuleBuilder> Submodules { get; }
//
//     new IList<IApplicationCommandBuilder> Commands { get; }
//
//     string? Alias { get; set; }
//
//     IApplicationModule Build(IApplicationModule? parent = null);
//
//     IModule IModuleBuilder.Build(IModule? parent)
//         => Build((IApplicationModule?) parent);
// }
