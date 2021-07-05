using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalRowComponent : LocalComponent
    {
        public IList<LocalNestedComponent> Components
        {
            get => _components;
            set => this.WithComponents(value);
        }
        internal readonly List<LocalNestedComponent> _components;

        public LocalRowComponent()
        {
            _components = new List<LocalNestedComponent>();
        }

        private LocalRowComponent(LocalRowComponent other)
        {
            _components = other._components.ToList();
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
