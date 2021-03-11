using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using com.xyroh.lib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// https://github.com/coolc0ders/XamarinForms_SnackBar

namespace WorstUrlShortener.Views.Shared
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSnackBarView : TemplatedView
    {
        public static readonly BindableProperty ButtonTextColorProperty = BindableProperty.Create("ButtonTextColor", typeof(Color), typeof(CustomSnackBarView), default(Color));
        public Color ButtonTextColor
        {
            get { return (Color)GetValue(ButtonTextColorProperty); }
            set { SetValue(ButtonTextColorProperty, value); }
        }

        public static readonly BindableProperty MessageProperty = BindableProperty.Create("Message", typeof(string), typeof(CustomSnackBarView), default(string));
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly BindableProperty CloseButtonTextProperty = BindableProperty.Create("CloseButtonText", typeof(string), typeof(CustomSnackBarView), "Close");
        public string CloseButtonText
        {
            get { return (string)GetValue(CloseButtonTextProperty); }
            set { SetValue(CloseButtonTextProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(float), typeof(CustomSnackBarView), default(float));
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(CustomSnackBarView), Color.White);
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty CloseButtonBackGroundColorProperty = BindableProperty.Create("CloseButtonBackGroundColor", typeof(Color), typeof(CustomSnackBarView), Color.Transparent);
        public Color CloseButtonBackGroundColor
        {
            get { return (Color)GetValue(CloseButtonBackGroundColorProperty); }
            set { SetValue(CloseButtonBackGroundColorProperty, value); }
        }

        public uint AnimationDuration { get; set; }

        #region IsOpen
        public static readonly BindableProperty IsOpenProperty = BindableProperty.Create("IsOpen", typeof(bool), typeof(CustomSnackBarView), false, propertyChanged: IsOpenChanged);
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private static void IsOpenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            bool isOpen;

            if (bindable != null && newValue != null)
            {
                var control = (CustomSnackBarView)bindable;
                isOpen = (bool)newValue;

                if (control.IsOpen == false)
                {
                    control.Close();
                }
                else
                {
                    control.Open(control.Message);

                }
            }
        }

        #endregion

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(CustomSnackBarView), default(string));
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public CustomSnackBarView()
        {
            IsVisible = false;
            AnimationDuration = 150;
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            Close();
        }

        public async void Close()
        {
            await this.TranslateTo(0, 50, AnimationDuration);
            Message = string.Empty;
            IsOpen = IsVisible = false;
        }

        public async void Open(string message)
        {
            IsVisible = true;
            Message = message;
            await this.TranslateTo(0, 0, AnimationDuration);
        }
    }
}
