// using System.Collections.Generic;
// using Qmmands;
//
// namespace Disqord.Bot.Commands.Application;
//
// public interface IApplicationModule : IModule
// {
//     new IApplicationModule? Parent { get; }
//
//     IModule? IModule.Parent => Parent;
//
//     new IReadOnlyList<IApplicationModule> Submodules { get; }
//
//     IReadOnlyList<IModule> IModule.Submodules => Submodules;
//
//     new IReadOnlyList<IApplicationCommand> Commands { get; }
//
//     IReadOnlyList<ICommand> IModule.Commands => Commands;
//
//     string? Alias { get; }
// }
