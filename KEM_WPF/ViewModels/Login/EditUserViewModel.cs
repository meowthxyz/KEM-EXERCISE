﻿using System;
using KEM_WPF.Models;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using KEM_WPF.Notifications;

namespace KEM_WPF.ViewModels
{
    class EditUserViewModel : BindableBase
    {
        private EditUserViewModel() { }
        public EditUserViewModel(string oldUserID, string firstName, string lastName, string emailAddress, string userType) 
        { 
            this._oldUserID = oldUserID;
            this._firstName = firstName;
            this._lastName = lastName;
            this._emailAddress = emailAddress;
            this._userType = userType;
        }

        private Window _editWindow;
        public Window EditWindow
        {
            get { return _editWindow; }
            set { SetProperty(ref _editWindow, value); }
        }
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _userType;
        private string _oldUserID;
        private string _newUserID;
        private string _oldPassword;
        private string _password;
        private string _confirm;

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
                if (_userType == null) _userType = "";
                return _userType;
            }
            set { SetProperty(ref _userType, value); }
        }
        public string UserID
        {
            get
            {
                if (_newUserID == null) _newUserID = _oldUserID;
                return _newUserID;
            }
            set { SetProperty(ref _newUserID, value); }
        }
        public string OldPassword
        {
            get
            {
                if (_oldPassword == null) _oldPassword = "";
                return _oldPassword;
            }
            set { SetProperty(ref _oldPassword, value); }
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

        private ICommand _modifyUserCommand;
        public ICommand ModifyUserCommand
        {
            get
            {
                if (_modifyUserCommand == null) _modifyUserCommand = new RelayCommand(new Action<object>(ModifyUser));
                return _modifyUserCommand;
            }
            set { SetProperty(ref _modifyUserCommand, value); }
        }
        private void ModifyUser(object parameter)
        {
            //PasswordBox pwBox = (PasswordBox)parameter;
            //string OldPassword = pwBox.Password;

            //if (string.IsNullOrWhiteSpace(_oldUserID) ||
            //    string.IsNullOrWhiteSpace(OldPassword) ||
            //    string.IsNullOrWhiteSpace(UserID) ||
            //    string.IsNullOrWhiteSpace(Password) ||
            //    string.IsNullOrWhiteSpace(Confirm))
            //{
            //    NotificationProvider.Error("Edit user error", "Please fill the Username and Password fields.");
            //}
            //else
            //{
            try
            {
                if (UserManager.ModifyUser(_oldUserID, OldPassword, UserID, Password, Confirm, FirstName, LastName, EmailAddress, UserType))
                {
                    NotificationProvider.Info(String.Format("User modified: {0}", _oldUserID), String.Format("New username: {0}", UserID));
                    EditWindow?.Close();
                }
                else
                {
                    NotificationProvider.Error("Edit user error", "Database error");
                }
            }
            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "oldUserID":
                        NotificationProvider.Error("Edit user error", "The original username is missing from the database.");
                        break;
                    case "oldPassword":
                        NotificationProvider.Error("Edit user error", "The old password is wrong.");
                        break;
                    case "newUserId":
                        NotificationProvider.Error("Edit user error", "The new username already exist.");
                        break;
                    case "password":
                        NotificationProvider.Error("Edit user error", "Please fill the password field.");
                        break;
                    case "confirm":
                        NotificationProvider.Error("Edit user error", "Password does not match the confirm password.");
                        break;
                    case "noRecord":
                        NotificationProvider.Error("Edit user error", "User record not found.");
                        break;
                    default:
                        NotificationProvider.Error("Edit user error", "UserLogin error");
                        break;
                }
            }

            //}
        }
    }
}
