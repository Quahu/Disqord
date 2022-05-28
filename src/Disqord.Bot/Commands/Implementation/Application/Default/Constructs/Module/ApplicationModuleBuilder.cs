using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

public class ApplicationModuleBuilder : IModuleBuilder
{
    public ApplicationModuleBuilder? Parent { get; }

    public IList<ApplicationModuleBuilder> Submodules => _submodules;

    public IList<ApplicationCommandBuilder> Commands => _commands;

    public string? Alias { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public IList<ICheck> Checks { get; } = new List<ICheck>();

    public IList<Attribute> CustomAttributes { get; } = new List<Attribute>();

    public TypeInfo? TypeInfo { get; }

    IModuleBuilder? IModuleBuilder.Parent => Parent;

    IList<ICommandBuilder> IModuleBuilder.Commands => _commands;

    IList<IModuleBuilder> IModuleBuilder.Submodules => _submodules;

    private readonly UpcastingList<ApplicationModuleBuilder, IModuleBuilder> _submodules = new();
    private readonly UpcastingList<ApplicationCommandBuilder, ICommandBuilder> _commands = new();

    public ApplicationModuleBuilder()
        : this(null)
    { }

    public ApplicationModuleBuilder(ApplicationModuleBuilder? parent)
    {
        Parent = parent;
    }

    public ApplicationModuleBuilder(ApplicationModuleBuilder? parent, TypeInfo typeInfo)
        : this(parent)
    {
        TypeInfo = typeInfo;
        Name = typeInfo.Name;
    }

    public virtual ApplicationModule Build(ApplicationModule? parent = null)
    {
        // TODO: maybe throw when Alias is set for when the module contains context menu commands

        return new ApplicationModule(parent, this);
    }

    IModule IModuleBuilder.Build(IModule? parent)
        => Build((ApplicationModule?) parent);
}
