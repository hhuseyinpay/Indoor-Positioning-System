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

        #endregion

        private readonly Grid MapGrid;
        private BleModel Ble1 { get; set; }
        private BleModel Ble2 { get; set; }
        private BleModel Ble3 { get; set; }
        private readonly Algorithm _algorithm;

        public MapPage()
        {
            InitializeComponent();
            BindingContext = this;

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

            InitAsync();
        }

        private async void InitAsync()
        {
            try
            {
                CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                {
                    Debug.Write(scanResult.Device.Name + " ->");
                    Debug.WriteLine(scanResult.Rssi);

                    if (scanResult.Device.Name == Ble1.Name)
                        Ble1.AddValue(scanResult.Rssi);
                    else if (scanResult.Device.Name == Ble2.Name)
                        Ble2.AddValue(scanResult.Rssi);
                    else if (scanResult.Device.Name == Ble3.Name)
                        Ble3.AddValue(scanResult.Rssi);
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Task.Delay(500); // wait until page preparation finish
                await Application.Current.MainPage.DisplayAlert("CRITICAL ERROR",
                    "Error occurred while configuring bluetooth. Check the bluetooth status!", "Close the App");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }


            for (var i = 0; i < 52; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
                if (i % 2 == 0)
                    MapGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            }

            MapGrid.Children.Add(Dot, 10, 50);
            MainGrid.Children.Add(MapGrid);

            AddAnimation();

            await _algorithm.Run();
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
    }
}