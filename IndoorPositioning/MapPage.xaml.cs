using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoorPositioning
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage
    {
        #region Bindings

        public BoxView Dot
        {
            get => (BoxView) GetValue(dotProperty);
            set => SetValue(dotProperty, value);
        }

        public static readonly BindableProperty dotProperty =
            BindableProperty.Create(nameof(Dot), typeof(BoxView), typeof(MapPage), default(BoxView));

        public BleModel Ble1
        {
            get => (BleModel) GetValue(Ble1Property);
            set => SetValue(Ble1Property, value);
        }

        public static readonly BindableProperty Ble1Property =
            BindableProperty.Create(nameof(Ble1), typeof(BleModel), typeof(MapPage), default(BleModel));

        public BleModel Ble2
        {
            get => (BleModel) GetValue(Ble2Property);
            set => SetValue(Ble2Property, value);
        }

        public static readonly BindableProperty Ble2Property =
            BindableProperty.Create(nameof(Ble2), typeof(BleModel), typeof(MapPage), default(BleModel));

        public BleModel Ble3
        {
            get => (BleModel) GetValue(Ble3Property);
            set => SetValue(Ble3Property, value);
        }

        public static readonly BindableProperty Ble3Property =
            BindableProperty.Create(nameof(Ble3), typeof(BleModel), typeof(MapPage), default(BleModel));

        #endregion

        private Grid MapGrid;

        private Algorithm _algorithm;

        public MapPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //ConfigureBluetooth();
            InitAsync();

            Device.BeginInvokeOnMainThread( () => {  _algorithm.Run(); });
        }


        private void ConfigureBluetooth()
        {
            try
            {
                Debug.WriteLine("bluetooth config");
                CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                {
                    //Debug.Write(scanResult.Device.Name + " ->");
                    //Debug.WriteLine(scanResult.Rssi);
                    switch (scanResult.Device.Name)
                    {
                        case "BLE1":
                            Ble1.AddValue(scanResult.Rssi);
                            break;
                        case "BLE2":
                            //BLEKValueList.Add(Convert.ToString(scanResult.Rssi));
                            Ble2.AddValue(scanResult.Rssi);
                            break;
                        case "BLE3":
                            Ble3.AddValue(scanResult.Rssi);
                            break;
                    }
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Task.Delay(500); // wait until page preparation finish
                Application.Current.MainPage.DisplayAlert("CRITICAL ERROR",
                    "Error occurred while configuring bluetooth. Check the bluetooth status!", "Close the App");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }


        private void InitAsync()
        {
            MapGrid = new Grid() {Margin = new Thickness(0, 10)};
            Dot = new BoxView
            {
                Color = Color.Red,
                CornerRadius = 6,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Ble1 = new BleModel("BLE1");
            Ble2 = new BleModel("BLE2");
            Ble3 = new BleModel("BLE3");
            _algorithm = new Algorithm(MapGrid, Dot, Ble1, Ble2, Ble3);

            for (var i = 0; i < 52; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
                if (i % 2 == 0)
                    MapGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            }

            var b1 = new BoxView
            {
                Color = Color.Blue,
                CornerRadius = 6,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            var b2 = new BoxView
            {
                Color = Color.Blue,
                CornerRadius = 6,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            var b3 = new BoxView
            {
                Color = Color.Blue,
                CornerRadius = 6,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            
            MapGrid.Children.Add(b1, 14, 13);
            MapGrid.Children.Add(b2, 14, 26);
            MapGrid.Children.Add(b3, 14, 39);
            
            MapGrid.Children.Add(Dot, 11, 51);
            MainGrid.Children.Add(MapGrid);
            Grid.SetColumnSpan(MapGrid, 5);

            AddAnimation();
        }


        private void AddAnimation()
        {
            var pulseAnimation = new Animation();

            pulseAnimation.Add(
                0, 0.2,
                new Animation(alpha => Dot.Opacity = alpha, 1, 0, Easing.CubicOut, () => Dot.FadeTo(1))
            );
            pulseAnimation.Commit(this, "dotAnimation", 32, 400, null, null, () => true);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            InitAsync();
        }
    }
}