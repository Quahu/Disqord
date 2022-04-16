using System;

namespace Disqord.Extensions.Interactivity.Menus
{
    public abstract class ViewComponent
    {
        public ViewBase View { get; internal set; }

        /// <summary>
        ///     Gets or sets the zero-indexed row the component should appear on.
        ///     If <see langword="null"/>, the component's row will be determined automatically.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when the value is negative or higher than 4. </exception>
        public int? Row
        {
            get => _row;
            set
            {
                if (value != null && (value < 0 || value > 4))
                    throw new ArgumentException("A component's row must be a positive value not higher than 4.");

                var view = View;
                if (view != null)
                {
                    view.RemoveComponent(this);
                    _row = value;
                    view.AddComponent(this);
                }
                else
                {
                    _row = value;
                }
            }
        }
        private int? _row;

        /// <summary>
        ///     Gets or sets the zero-indexed position the component should appear in on the row.
        ///     If <see langword="null"/>, the component's position will be determined automatically.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when the value is negative or higher than 4. </exception>
        public int? Position
        {
            get => _position;
            set
            {
                if (value != null && (value < 0 || value > 4))
                    throw new ArgumentException("A component's position must be a positive value not higher than 4.");

                var view = View;
                if (view != null)
                {
                    view.RemoveComponent(this);
                    _position = value;
                    view.AddComponent(this);
                }
                else
                {
                    _position = value;
                }
            }
        }
        private int? _position;

        public abstract int Width { get; }

        protected ViewComponent()
        { }

        protected ViewComponent(ComponentAttribute attribute)
        {
            _row = attribute.Row != -1
                ? attribute.Row
                : null;

            _position = attribute.Position != -1
                ? attribute.Position
                : null;
        }

        protected void ReportChanges()
        {
            var view = View;
            if (view != null)
                view.HasChanges = true;
        }

        protected internal abstract LocalComponent ToLocalComponent();
    }
}
