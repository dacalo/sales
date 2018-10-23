using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Views;
using System.Windows.Input;

namespace Sales.ViewModels
{
    public class CategoryItemViewModel : Category
    {
        #region Commands
        public ICommand GotoCategoryCommand
        {
            get
            {
                return new RelayCommand(GotoCategory);
            }
        }

        private async void GotoCategory()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await App.Navigator.PushAsync(new ProductsPage());
        }
        #endregion
    }

}
