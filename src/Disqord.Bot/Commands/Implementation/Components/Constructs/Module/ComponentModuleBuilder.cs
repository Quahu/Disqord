using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;

namespace Disqord.Bot.Commands.Components;

public class ComponentModuleBuilder : IModuleBuilder
{
    public ComponentModuleBuilder? Parent { get; }

    public IList<ComponentModuleBuilder> Submodules => _submodules;

    public IList<ComponentCommandBuilder> Commands => _commands;

    public string? Alias { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public IList<ICheck> Checks { get; } = new List<ICheck>();

    public IList<Attribute> CustomAttributes { get; } = new List<Attribute>();

    public TypeInfo? TypeInfo { get; }

    IModuleBuilder? IModuleBuilder.Parent => Parent;

    IList<ICommandBuilder> IModuleBuilder.Commands => _commands;

    IList<IModuleBuilder> IModuleBuilder.Submodules => _submodules;

    private readonly UpcastingList<ComponentModuleBuilder, IModuleBuilder> _submodules = new();
    private readonly UpcastingList<ComponentCommandBuilder, ICommandBuilder> _commands = new();

    public ComponentModuleBuilder()
        : this(null)
    { }

    public ComponentModuleBuilder(ComponentModuleBuilder? parent)
    {
        Parent = parent;
    }

    public ComponentModuleBuilder(ComponentModuleBuilder? parent, TypeInfo typeInfo)
        : this(parent)
    {
        TypeInfo = typeInfo;
        Name = typeInfo.Name;
    }

    public virtual ComponentModule Build(ComponentModule? parent = null)
    {
        return new ComponentModule(parent, this);
    }

    IModule IModuleBuilder.Build(IModule? parent)
        => Build((ComponentModule?) parent);
}
