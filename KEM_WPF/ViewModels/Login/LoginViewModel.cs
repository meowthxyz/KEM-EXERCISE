﻿using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using KEM_WPF.Views;
using KEM_WPF.Models;
using KEM_WPF.Notifications;

namespace KEM_WPF.ViewModels
{
    class LoginViewModel : BindableBase
    {
        private string _userID;
        private string _password;
        public Window LoginWindow { get; set; }
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

        private ICommand _click_LoginCommand;
        public ICommand Click_LoginCommand
        {
            get
            {
                if (_click_LoginCommand == null) _click_LoginCommand = new RelayCommand(new Action<object>(Login));
                return _click_LoginCommand;
            }
            set { SetProperty(ref _click_LoginCommand, value); }
        }

        private void Login(object parameter)
        {
           
            PasswordBox pwBox = (PasswordBox)parameter;
            Password = pwBox.Password;

            try
            {
                if(UserManager.AuthenticateUser(UserID, Password))
                {
                    NotificationProvider.Info(String.Format("Welcome, {0}!", UserID), "You have succesfully logged in.");
                    LoginWindow.Close();
                }
                else
                {
                    NotificationProvider.Error("Login error", "Invalid username/password.");
                }
            }
            catch (ArgumentException e)
            {
                NotificationProvider.Error("Login error", e.Message);
            }
            
        }

        private ICommand _click_SetupCommand;
        public ICommand Click_SetupCommand
        {
            get
            {
                if (_click_SetupCommand == null) _click_SetupCommand = new RelayCommand(new Action<object>(Setup));
                return _click_SetupCommand;
            }
            set { SetProperty(ref _click_SetupCommand, value); }
        }

        private void Setup(object parameter)
        {
            NewUserWindow NUW = new NewUserWindow();
            NUW.ShowDialog();
            //Views.SetupConnectionWindow setupWindow = new SetupConnectionWindow();
            //setupWindow.ShowDialog();
        }
    }
}
