using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalRowComponent : LocalComponent
    {
        public IList<LocalComponent> Components
        {
            get => _components;
            set => WithComponents(value);
        }
        private readonly IList<LocalComponent> _components;

        public LocalRowComponent()
        {
            _components = new List<LocalComponent>();
        }

        private LocalRowComponent(LocalRowComponent other)
        {
            _components = other._components.ToList();
        }

        public LocalRowComponent WithComponents(IEnumerable<LocalComponent> components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            _components.Clear();
            foreach (var component in components)
                _components.Add(component);

            return this;
        }

        public LocalRowComponent AddComponent(LocalComponent component)
        {
            Components.Add(component);
            return this;
        }

        public override LocalRowComponent Clone()
            => new(this);

        public override void Validate()
        {
            for (var i = 0; i < _components.Count; i++)
                _components[i].Validate();
        }
    }
}
