using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructor
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commands
        public ICommand DeleteProductCommand
        {  
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }
        
        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        #endregion

        #region Methods
        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);
            if(!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.isSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller, this.ProductId);
            if (!response.isSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.message, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var deleteProduct = productsViewModel.MyProducts.Where(p => ProductId == this.ProductId).FirstOrDefault();
            if (deleteProduct != null)
            {
                productsViewModel.MyProducts.Remove(deleteProduct);
            }
            productsViewModel.RefreshList();
        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }
        #endregion
    }
}
