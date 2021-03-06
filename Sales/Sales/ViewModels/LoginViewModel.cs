﻿using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Properties
        public string Email { get; set; }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public bool IsRemembered { get; set; }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public string Password { get; set; }

        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.isEnabled = true;
            this.IsRemembered = true;
        }
        #endregion
        
        #region Commands
        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }

        public ICommand LoginFacebookComand
        {
            get
            {
                return new RelayCommand(LoginFacebook);
            }
        }

        public ICommand LoginInstagramComand
        {
            get
            {
                return new RelayCommand(LoginInstagram);
            }
        }

        public ICommand LoginTwitterComand
        {
            get
            {
                return new RelayCommand(LoginTwitter);
            }
        }

        public ICommand RegisterCommand
        {
            get { return new RelayCommand(Register); }
        }

        #endregion

        #region Methods
        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.isEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.isEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var token = await this.apiService.GetToken(url, this.Email, this.Password);
            if(token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.isEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);
                return;
            }

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
            Settings.IsRemembered = this.IsRemembered;

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await this.apiService.GetUser(url, prefix, $"{controller}/GetUser", this.Email, token.TokenType, token.AccessToken);
            if (response.IsSuccess)
            {
                var userASP = (MyUserASP)response.Result;
                MainViewModel.GetInstance().UserASP = userASP;
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
            }


            MainViewModel.GetInstance().Categories = new CategoriesViewModel();
            Application.Current.MainPage = new MasterPage();
            this.IsRunning = false;
            this.isEnabled = true;
        }

        private async void LoginFacebook()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginFacebookPage());
        }

        private async void LoginInstagram()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginInstagramPage());
        }

        private async void LoginTwitter()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginTwitterPage());
        }

        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
        #endregion
    }
}
