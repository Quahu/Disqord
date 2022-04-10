using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public class LocalRowComponent : LocalComponent
    {
        public IList<LocalComponent> Components
        {
            get => _components;
            set => this.WithComponents(value);
        }
        internal readonly List<LocalComponent> _components;

        public LocalRowComponent()
        {
            _components = new List<LocalComponent>();
        }

        private LocalRowComponent(LocalRowComponent other)
        {
            _components = other._components.ToList();
        }

        public override LocalRowComponent Clone()
            => new(this);
    }
}
