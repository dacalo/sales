using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel: BaseViewModel
    {
        private ApiService apiService;
        private bool isRefreshing;
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.isSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.message, "Accept");
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetList<Product>(url, "/api", "/Products");
            if (!response.isSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.message, "Accept");
                return;
            }

            var lst = (List<Product>)response.result;
            this.Products = new ObservableCollection<Product>(lst);
            this.IsRefreshing = false;
        }

        public ICommand RefreshCommang
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }
    }
}
