﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UWP_FilRouge.Database;
using UWP_FilRouge.Views.ViewModelLight.Views.ViewModel.UcAccessors;
using Windows.UI.Xaml.Controls;

namespace UWP_FilRouge.Views.ViewModel
{
    public class SellerMainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        public RelayCommand MoveToRegisterPage { get; private set; }
        public RelayCommand MoveToLoginPage { get; private set; }
        public RelayCommand MoveToSellerPage { get; private set; }
        public RelayCommand MoveToCustomerPage { get; private set; }
        public RelayCommand MoveToOrderPage { get; private set; }
        private bool _isLoading = false;
        private bool _addingNewSeller = false;

        public SellerPageAccessor DataSeller { get; set; }
        private DatabaseService databaseService;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool AddingNewSeller
        {
            get => _addingNewSeller;
            set
            {
                if (_addingNewSeller != value)
                {
                    _addingNewSeller = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");

            }
        }
        private string _title;
        public string Title
        {

            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        public SellerMainPageViewModel(INavigationService navigationService, DatabaseService databaseService)
        {
            this.navigationService = navigationService;
            this.databaseService = databaseService;
            SetupSellerDatas();
            Title = "Seller Main Page";
            //MoveToRegisterPage = new RelayCommand(ToRegisterPage);
            MoveToLoginPage = new RelayCommand(ToLoginPage);
            MoveToOrderPage = new RelayCommand(ToOrderPage);
            //MoveToSellerPage = new RelayCommand(ToSellerPage);
            MoveToCustomerPage = new RelayCommand(ToCustomerPage);
        }

        private void SetupSellerDatas()
        {
            DataSeller = new SellerPageAccessor();
            SetupSellerEdit();
            SetupSellerList();
            SetupSellerShow();
        }

        private void SetupSellerEdit()
        {
            DataSeller.sellerEdit.validateButton.Content = "Valider";
            DataSeller.sellerEdit.validateButton.Action = new RelayCommand(SellerEditCommand);
            DataSeller.sellerEdit.cancelButton.Content = "Cancel";
            DataSeller.sellerEdit.cancelButton.Action = new RelayCommand(SellerEditCancel);
            DataSeller.sellerEdit.seller = new Seller();
        }

        private void SellerEditCancel()
        {
            navigationService.GoBack();
        }

        private void SellerEditCommand()
        {
            Seller seller = new Seller();
            seller.CopyFrom(DataSeller.sellerEdit.seller);

            try
            {
                databaseService.SqliteConnection.Insert(seller);
                DataSeller.sellerList.sellers.Add(seller);
            }
            catch (Exception e)
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Error";
                contentDialog.Content = e.Message;
                contentDialog.IsSecondaryButtonEnabled = false;
                contentDialog.PrimaryButtonText = "ok";
                contentDialog.ShowAsync();
            }
        }

        private void SellerRemoveCommand()
        {
            Seller seller = new Seller();
            
            seller.CopyFrom(DataSeller.sellerList.listView.SelectedItem);

            System.Diagnostics.Debug.WriteLine("{0}", DataSeller.sellerList.listView.SelectedItem.Id);

            try
            {
                databaseService.SqliteConnection.Delete(seller);
                DataSeller.sellerList.sellers.Remove(seller);
            }
            catch (Exception e)
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Error";
                contentDialog.Content = e.Message;
                contentDialog.IsSecondaryButtonEnabled = false;
                contentDialog.PrimaryButtonText = "ok";
                contentDialog.ShowAsync();
            }
        }

        private void SetupSellerShow()
        {
            DataSeller.sellerShow.seller = new Seller();
        }

        private void SetupSellerList()
        {
            DataSeller.sellerList.sellers = new ObservableCollection<Seller>();
            DataSeller.sellerList.deleteButton.Content = "Delete";
            DataSeller.sellerList.deleteButton.Action = new RelayCommand(SellerRemoveCommand);
            DataSeller.sellerList.updateButton.Content = "Update";
            //DataSeller.sellerList.updateButton.Action = new RelayCommand(SellerUpdateCommand);
            DataSeller.sellerList.updateButton.Content = "Nimp";
            DataSeller.sellerList.updateButton.Action = new RelayCommand(SellerUpdateCommand);
            DataSeller.sellerList.cancelButton.Content = "portequoi";
            DataSeller.sellerList.cancelButton.Action = new RelayCommand(SellerEditCancel);

            foreach (var item in databaseService.Sellers)
            {
                DataSeller.sellerList.sellers.Add(item);
            }
            DataSeller.sellerList.listView.SelectedItem = new Seller();
            DataSeller.sellerList.listView.SelectionChanged = new RelayCommand(SellerListSelectionChanged);

        }

        private void SellerUpdateCommand()
        {
            
            Seller seller = new Seller();
            
            seller.CopyFrom(DataSeller.sellerList.listView.SelectedItem);
            DataSeller.sellerShow.seller.CopyFrom(seller);
            Debug.WriteLine("{0}", seller.FirstName);

            try
            {
                databaseService.SqliteConnection.Update(seller);
                //DataSeller.sellerList.sellers.(seller);
            }
            catch (Exception e)
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Error";
                contentDialog.Content = e.Message;
                contentDialog.IsSecondaryButtonEnabled = false;
                contentDialog.PrimaryButtonText = "ok";
                contentDialog.ShowAsync();
            }
        }

        private void SellerListSelectionChanged()
        {
            Seller seller = DataSeller.sellerList.listView.SelectedItem;
            if (seller != null)
            {
                DataSeller.sellerShow.seller.CopyFrom(seller);
            }
        }

        private void ToLoginPage()
        {
            // Do Something
            navigationService.NavigateTo("Login Page");
        }

        private void ToCustomerPage()
        {
            // Do Something
            navigationService.NavigateTo("Customer Main Page");
        }

        private void ToOrderPage()
        {
            // Do Something
            navigationService.NavigateTo("Order Main Page");
        }
    }
}