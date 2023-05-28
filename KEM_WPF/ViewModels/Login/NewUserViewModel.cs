using System;
using System.Windows;
using System.Windows.Input;
using KEM_WPF.Notifications;
using KEM_WPF.Models;
using System.Collections.Generic;

namespace KEM_WPF.ViewModels
{
    class NewUserViewModel : BindableBase
    {
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _userType = "Admin";
        private string _userID;
        private string _password;
        private string _confirm;
        public Window NewUserWindow { get; set; }
        public string FirstName
        {
            get
            {
                if (_firstName == null) _firstName = "";
                return _firstName;
            }
            set { SetProperty(ref _firstName, value); }
        }
        public string LastName
        {
            get
            {
                if (_lastName == null) _lastName = "";
                return _lastName;
            }
            set { SetProperty(ref _lastName, value); }
        }
        public string EmailAddress
        {
            get
            {
                if (_emailAddress == null) _emailAddress = "";
                return _emailAddress;
            }
            set { SetProperty(ref _emailAddress, value); }
        }
        public string UserType
        {
            get
            {
                if (_userType == null) _userType = "Admin";
                return _userType;
            }
            set { SetProperty(ref _userType, value); }
        }
        public string UserID
        {
            get
            {
                if (_userID == null) _userID = "";
                return _userID;
            }
            set { SetProperty(ref _userID, value); }
        }
        public string Password
        {
            get
            {
                if (_password == null) _password = "";
                return _password;
            }
            set { SetProperty(ref _password, value); }
        }
        public string Confirm
        {
            get
            {
                if (_confirm == null) _confirm = "";
                return _confirm;
            }
            set { SetProperty(ref _confirm, value); }
        }
        private List<string> _categoryList;
        public List<string> CategoryList
        {
            get
            {
                if (_categoryList == null)
                {
                    _categoryList = new List<string>();
                    _categoryList.Add("Admin");
                    _categoryList.Add("SuperAdmin");
                }
                return _categoryList;
            }
            set { SetProperty(ref _categoryList, value); }
        }

        private ICommand _click_AddUserCommand;

        public ICommand Click_AddUserCommand
        {
            get
            {
                if (_click_AddUserCommand == null) _click_AddUserCommand = new RelayCommand(new Action<object>(AddUser));
                return _click_AddUserCommand;
            }
            set { SetProperty(ref _click_AddUserCommand, value); }
        }
        private void AddUser(object parameter)
        {
            if (Password != Confirm)
            {
                NotificationProvider.Error("New user error", "Password does not match the confirm password.");
            }
            else
            {
                try
                {
                    if (UserManager.AddUser(UserID, Password, FirstName, LastName, EmailAddress, UserType))
                    {
                        NotificationProvider.Info("New user added", String.Format("Username: {0}", UserID));
                        NewUserWindow.Close();
                    }
                    else
                    {
                        NotificationProvider.Error("New user error", "Username already exist.");
                    }
                }
                catch
                {
                    NotificationProvider.Error("New user error", "Please fill-up all field(s).");
                }
            }
        }
    }
}
