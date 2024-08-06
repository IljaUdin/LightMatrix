using ReactiveUI;
using System;

namespace LEDPanel_Avalonia.ViewModels
{
    public class RoutableViewModel : ViewModelBase, IRoutableViewModel
    {
        readonly Func<IScreen> _hostScreenFactory;
        // Reference to IScreen that owns the routable view model.
        //public IScreen HostScreen { get; }
        public IScreen HostScreen => _hostScreenFactory();

        // Unique identifier for the routable view model.
        public virtual string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public RoutableViewModel(Func<IScreen> hostScreenFactory)
        {
            _hostScreenFactory = hostScreenFactory;
        }
    }
}
