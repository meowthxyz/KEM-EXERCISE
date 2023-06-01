using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KEM_WPF.Models;
using KEM_WPF.Notifications;
using KEM_WPF.Views;

namespace KEM_WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Constructors
        public MainWindowViewModel(Window mainwindow)
        {   
            MainWindow = mainwindow;

            try
            {
                // New user.
                if (UserManager.IsEmptyUserDatabase())
                {
                    NewUserWindow NUW = new NewUserWindow();
                    NUW.ShowDialog();
                    if (UserManager.IsEmptyUserDatabase())
                    {
                        CloseWindow();
                        return;
                    }
                }

                // Login.
                LoginWindow LW = new LoginWindow();
                LW.ShowDialog();
                if (UserManager._LoggedUser.user_name == "" || UserManager._LoggedUser.user_name == null)
                {
                    CloseWindow();
                    return;
                }
            }
            catch (Exception ex)
            {
                CloseWindow();
                return;
            }
        }
        #endregion
        #region Menus and Views
        private string _greetings = "";
        private string[] _mainMenu = new string[] { };
        private string[] _mainMenuSuperAdmin = new string[] { "Users", "Equipments", "Sites", "Logout" };
        private string[] _mainMenuAdmin = new string[] { "Equipments", "Sites", "UpdateProfile", "Logout" };

        private SitesViewModel _site = new SitesViewModel();
        private EquipmentsViewModel _equipment = new EquipmentsViewModel();
        private UsersViewModel _users = new UsersViewModel();

        #endregion
        #region Change Menu
        public string Greetings
        {
            get
            {
                return (UserManager._LoggedUser.user_name != "" ? $"Hello, {UserManager._LoggedUser.user_name}!" : "");
            }
            set { SetProperty(ref _greetings, value); }
        }
        public string[] MainMenu 
        { 
            get 
            {
                if (UserManager._LoggedUser.user_type != "SuperAdmin")
                {
                    return _mainMenuAdmin;
                }
                return _mainMenuSuperAdmin; 
            }
            set { SetProperty(ref _mainMenu, value); }
        }

        private string[] _currentMenu;
        public string[] CurrentMenu
        {
            get { return _currentMenu; }
            set { SetProperty(ref _currentMenu, value); }
        }

        private ICommand _switchMenuCommand;
        public ICommand SwitchMenuCommand
        {
            get
            {
                //if (_switchMenuCommand == null) _switchMenuCommand = new RelayCommand(new Action<object>(SwitchMenu));
                if (_switchMenuCommand == null) _switchMenuCommand = new RelayCommand(new Action<object>(Navigate));
                return _switchMenuCommand;
            }
            set { SetProperty(ref _switchMenuCommand, value); }
        }
        #endregion

        #region Change View
        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set { SetProperty(ref _currentViewModel, value); }
        }
        private ICommand _switchViewCommand;
        public ICommand SwitchViewCommand
        {
            get
            {
                if (_switchViewCommand == null) _switchViewCommand = new RelayCommand(new Action<object>(Navigate));
                return _switchViewCommand;
            }
            set { SetProperty(ref _switchViewCommand, value); }
        }
        private void Navigate(object parameter)
        {
            string destination = (string)parameter;

            switch (destination)
            {
                case "Users":
                    CurrentViewModel = _users;
                    ((RelayCommand)_users.RefreshListCommand).CheckAndExecute(_users);
                    break;
                case "Equipments":
                    CurrentViewModel = _equipment;
                    ((RelayCommand)_equipment.RefreshListCommand).CheckAndExecute(_equipment);
                    break;
                case "Sites":
                    CurrentViewModel = _site;
                    ((RelayCommand)_site.RefreshListCommand).CheckAndExecute(_site);
                    break;
                case "UpdateProfile":

                    EditUserViewModel EUVM = new EditUserViewModel(UserManager._LoggedUser.user_name, UserManager._LoggedUser.first_name, UserManager._LoggedUser.last_name, UserManager._LoggedUser.email_address, UserManager._LoggedUser.user_type);
                    EditUserWindow EUV = new EditUserWindow() { DataContext = EUVM };
                    EUVM.EditWindow = EUV;
                    EUV.ShowDialog();

                    break;
                case "Logout":
                    //reset views/menus
                    CurrentViewModel = null;
                    MainMenu = _mainMenu;
                    Greetings = _greetings;
                    NotificationProvider.Info("Logout info", "Successfully logout.");

                    //end session
                    UserManager.ClearLoggedUser();
                    MainWindow?.Hide();

                    // Login.
                    LoginWindow LW = new LoginWindow();
                    LW.ShowDialog();
                    

                    if (UserManager._LoggedUser.user_name == "" || UserManager._LoggedUser.user_name == null)
                    {
                        CloseWindow();
                        return;
                    }
                    else
                    {
                        MainWindow?.Show();
                    }

                    //set menu
                    if (UserManager._LoggedUser.user_type == "SuperAdmin")
                    {
                        MainMenu = _mainMenuSuperAdmin;
                    }
                    else
                    {
                        MainMenu = _mainMenuAdmin;
                    }
                    Greetings = (UserManager._LoggedUser.user_name != "" ? $"Hello, {UserManager._LoggedUser.user_name}!" : "");

                    break;
                default:
                    CurrentViewModel = null;
                    break;
            }
        }
        #endregion
        #region Close Main window
        private Window _mainWindow;
        public Window MainWindow
        {
            get { return _mainWindow; }
            set { SetProperty(ref _mainWindow, value); }
        }
        private ICommand _closeMainWindowCommand;
        public ICommand CloseMainWindowCommand
        {
            get
            {
                if (_closeMainWindowCommand == null) _closeMainWindowCommand = new RelayCommand(CloseWindow);
                return _closeMainWindowCommand;
            }
            set { SetProperty(ref _closeMainWindowCommand, value); }
        }
        private void CloseWindow(object parameter = null)
        {
            MainWindow?.Close();
        }
        #endregion
    }
}
