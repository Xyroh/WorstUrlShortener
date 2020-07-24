using System;
using System.Collections.Generic;
using System.ComponentModel;
using com.xyroh.lib;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorstUrlShortener.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string propTitle = string.Empty;
        private bool isBusy;
        private bool isOnline;

        private string lastError = string.Empty;

        public BaseViewModel()
        {
            if (Device.RuntimePlatform != "macOS" && Device.RuntimePlatform != "WPF")
            {
                var connectivity = Connectivity.NetworkAccess;

                if (connectivity == NetworkAccess.Internet)
                {
                    // Connection to internet is available
                    this.IsOnline = true;
                }
                else
                {
                    this.IsOnline = false;
                }

                XyrohLib.Log("ONLINE: " + this.IsOnline);

                Connectivity.ConnectivityChanged += this.ConnectivityChanged;
            }

        }

        public string Title
        {
            get => this.propTitle;
            set => this.SetProperty(ref this.propTitle, value, "Title");
        }

        public string LastError
        {
            get => this.lastError;
            set => this.SetProperty(ref this.lastError, value, "LastError");
        }

        public bool IsBusy
        {
            get => this.isBusy;
            set => this.SetProperty(ref this.isBusy, value, "IsBusy");
        }

        public bool IsOnline
        {
            get
            {
                if(Device.RuntimePlatform != "macOS" && Device.RuntimePlatform != "WPF")
                {
                    return this.isOnline;
                }
                else
                {
                    // TODO Online connectivity check for MacOS
                    return true;
                }
            }
            set => this.SetProperty(ref this.isOnline, value, "IsOnline");
        }

        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
                return;
            store = value;
            if (onChanged != null)
                onChanged();
            this.OnPropertyChanged(propName);
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var connectivity = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;

            if (connectivity == NetworkAccess.Internet)

                // Connection to internet is available
                this.IsOnline = true;
            else
                this.IsOnline = false;

            XyrohLib.Log("ONLINE CHANGE - NOW: " + this.IsOnline);
        }
    }
}
