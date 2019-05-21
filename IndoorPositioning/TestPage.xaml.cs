using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.Collections;
using Plugin.BluetoothLE;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoorPositioning
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage
    {
        public ObservableCollection<string> BLE1ValueList
        {
            get => (ObservableCollection<string>) GetValue(BLE1ValueListProperty);
            set => SetValue(BLE1ValueListProperty, value);
        }

        public static readonly BindableProperty BLE1ValueListProperty =
            BindableProperty.Create(nameof(BLE1ValueList), typeof(ObservableCollection<string>), typeof(TestPage),
                default(ObservableCollection<string>));

        public ObservableCollection<string> BLE2ValueList
        {
            get => (ObservableCollection<string>) GetValue(BLE2ValueListProperty);
            set => SetValue(BLE2ValueListProperty, value);
        }

        public static readonly BindableProperty BLE2ValueListProperty =
            BindableProperty.Create(nameof(BLE2ValueList), typeof(ObservableCollection<string>), typeof(TestPage),
                default(ObservableCollection<string>));

        public ObservableCollection<string> BLE3ValueList
        {
            get => (ObservableCollection<string>) GetValue(BLE3ValueListProperty);
            set => SetValue(BLE3ValueListProperty, value);
        }

        public static readonly BindableProperty BLE3ValueListProperty =
            BindableProperty.Create(nameof(BLE3ValueList), typeof(ObservableCollection<string>), typeof(TestPage),
                default(ObservableCollection<string>));

        public TestPage()
        {
            InitializeComponent();
            BindingContext = this;
            BLE1ValueList = new ObservableCollection<string>();
            BLE2ValueList = new ObservableCollection<string>();
            BLE3ValueList = new ObservableCollection<string>();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //CrossBleAdapter.Current.ScanInterval(TimeSpan.FromMilliseconds(1000) , TimeSpan.FromMilliseconds(500))
            /*
            CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
            {
                Debug.Write(scanResult.Device.Name + " ->");
                Debug.WriteLine(scanResult.Rssi);
                switch (scanResult.Device.Name)
                {
                    case "BLE1":
                        BLE1ValueList.Insert(0, Convert.ToString(scanResult.Rssi));
                        break;
                    case "BLE2":
                        //BLEKValueList.Add(Convert.ToString(scanResult.Rssi));
                        BLE2ValueList.Insert(0, Convert.ToString(scanResult.Rssi));
                        break;
                    case "BLE3":
                        BLE3ValueList.Insert(0, Convert.ToString(scanResult.Rssi));
                        break;
                }
            });
            */
        }
    }
}