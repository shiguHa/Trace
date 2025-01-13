

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trace.Interfaces;

namespace Trace.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public MainWindowVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void Navigate(object obj)
        {
            switch (obj as string)
            {
                case "Home":
                    NavigateTo(typeof(HomeVM));
                    break;
                case "Idea1":
                    NavigateTo(typeof(Idea1VM));
                    break;
                    //case "Setting":
                    //    NavigateTo(typeof(SettingViewModel));
                    //    break;
            }
        }


        private void NavigateTo(Type targetViewModel)
        {
            if (targetViewModel != null)
            {
                _navigationService.NavigateTo(targetViewModel.FullName);
            }
        }
    }
}
