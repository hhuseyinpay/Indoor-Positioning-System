using System.Threading.Tasks;
using Xamarin.Forms;

namespace IndoorPositioning
{
    public class Algorithm
    {
        private readonly Grid _grid;
        private readonly BoxView _dot;
        private readonly BleModel _ble1;
        private readonly BleModel _ble2;
        private readonly BleModel _ble3;
        private bool IsDirectionDown { get; set; } = false;

        public Algorithm(Grid grid, BoxView dot, BleModel ble1, BleModel ble2, BleModel ble3)
        {
            _grid = grid;
            _dot = dot;
            _ble1 = ble1;
            _ble2 = ble2;
            _ble3 = ble3;
            _ble1.GetCurrentPosition();
        }

        public async Task Run()
        {
            await Task.Delay(1000); // wait some time for bluetooth initialization
            while (true)
            {
                if(_ble1.GetCurrentPosition() == Positions.Far && 
                   _ble2.GetCurrentPosition() == Positions.Far  )
                
                
                
                
                GoNext();
                await Task.Delay(250);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private void GoNext()
        {
            var row = Grid.GetRow(_dot);
            if (IsDirectionDown)
                Grid.SetRow(_dot, row + 1);
            else
                Grid.SetRow(_dot, row - 1);
        }

        private void InitialPosition()
        {
            if (_ble1.GetCurrentPosition() == Positions.Far &&
                _ble2.GetCurrentPosition() == Positions.Far &&
                _ble3.GetCurrentPosition() == Positions.Unknown)
            {
                
            }
        }
    }
}