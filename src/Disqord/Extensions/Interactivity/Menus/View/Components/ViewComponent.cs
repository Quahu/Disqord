namespace Disqord.Extensions.Interactivity.Menus
{
    public abstract class ViewComponent
    {
        public ViewBase View { get; internal set; }

        public int Row
        {
            get => _row;
            set
            {
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
        private int _row;
        
        public int Position
        {
            get => _position;
            set
            {
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
        private int _position;

        public abstract int Width { get; }

        protected ViewComponent()
        { }

        protected ViewComponent(ComponentAttribute attribute)
        {
            _row = attribute.Row;
            _position = attribute.Position;
        }

        protected void ReportChanges()
        {
            if (View != null)
                View.HasChanges = true;
        }

        protected internal abstract LocalComponent ToLocalComponent();
    }
}
