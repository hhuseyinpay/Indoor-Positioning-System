using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IndoorPositioning
{
    public enum Direction
    {
        Up,
        Down
    }

    public class Algorithm
    {
        private readonly int INVERVAL = 1000; //ms
        private readonly int BLE1_POSITION = 13;
        private readonly int BLE2_POSITION = 26;
        private readonly int BLE3_POSITION = 39;
        private readonly int END_POSITION = 51;


        private readonly Grid _grid;
        private readonly BoxView _dot;
        private readonly BleModel _ble1;
        private readonly BleModel _ble2;
        private readonly BleModel _ble3;

        private readonly int _maxRow;
        private readonly int _maxColumn;


        private Direction Direction { get; set; }

        public Algorithm(Grid grid, BoxView dot, BleModel ble1, BleModel ble2, BleModel ble3)
        {
            _grid = grid;
            _dot = dot;
            _ble1 = ble1;
            _ble2 = ble2;
            _ble3 = ble3;
            _maxRow = _grid.RowDefinitions.Count;
            _maxColumn = _grid.ColumnDefinitions.Count;
        }


        public async void Run()
        {
            await Task.Delay(100);
             InitialPosition();
        }

        private void CulculateCurrentPositions()
        {
            _ble1.CalculateCurrentPosition();
            _ble2.CalculateCurrentPosition();
            _ble3.CalculateCurrentPosition();

            Debug.WriteLine("ble1 -> .. | " +
                            _ble1.CurrentPositon() + " | " +
                            _ble2.CurrentPositon() + " | " +
                            _ble3.CurrentPositon());
        }

        private void GoNext(int until)
        {
            var row = Grid.GetRow(_dot);

            if (Direction == Direction.Up)
                until = Math.Abs(_grid.RowDefinitions.Count - until);

            if (row == _maxRow || until == row)
                return;


            switch (Direction)
            {
                case Direction.Down:
                    Grid.SetRow(_dot, row + 1);
                    break;
                case Direction.Up:
                    Grid.SetRow(_dot, row - 1);
                    break;
            }
        }


        private void GoToRow(int row)
        {
            /*
             * Bu fonksiyon herzaman INVERVAL kadar delay yapar.
             */
            if (Direction == Direction.Up)
                row = Math.Abs(_grid.RowDefinitions.Count - row);

            var currentRow = Grid.GetRow(_dot);
            var diff = Math.Abs(row - currentRow);

            if (diff == 0)
                diff = 1;

            var time = 4 * INVERVAL / diff;
            for (var i = 0; i < diff; i++)
            {
                GoNext(row);
                Task.Delay(time);
            }
        }


        private async void InitialPosition()
        {
            Debug.WriteLine("InitialPosition");
            while (true)
            {
                await Task.Delay(500);
                CulculateCurrentPositions();

                if (_ble1.IsFar && _ble3.IsUnknown)
                {
                    _grid.Children.Add(_dot, 11, 1);
                    Direction = Direction.Down;
                     EdgeInsideLoop(_ble1, _ble2, _ble3);
                     return;
                }

                if (_ble1.IsUnknown && _ble3.IsFar)
                {
                    _grid.Children.Add(_dot, 11, 51);
                    Direction = Direction.Up;

                     EdgeInsideLoop(_ble3, _ble2, _ble1);
                     return;
                }
            }
        }


        private async void EdgeInsideLoop(BleModel b1, BleModel b2, BleModel b3)
        {
            /*
             *  yaklaşılmakta olan  ble'yi b1 varsıyorum. ortadaki ble b2 en uzaktaki b3
             */
            DateTime start = DateTime.Now;
// Do some work
            Debug.WriteLine("EdgeInsideLoop");
            while (true)
            {
                TimeSpan timeDiff = DateTime.Now - start;
                CulculateCurrentPositions();

                if (b1.IsNear && b2.IsFar && timeDiff.Seconds > 8)
                {
                    GoToRow(BLE1_POSITION);
                     CenterInsideLoop(b2, b1, b3);
                     return;
                }

                GoNext(BLE1_POSITION);

                await Task.Delay(INVERVAL);
            }
        }

        private async void CenterInsideLoop(BleModel b1, BleModel b2, BleModel b3)
        {
            /*
             *  yaklaşılmakta olan  ble'yi b1 varsıyorum. arkadaki ble b2 en ilerideki b3
             */
            DateTime start = DateTime.Now;
            Debug.WriteLine("CenterInsideLoop");
            while (true)
            {
                CulculateCurrentPositions();
                TimeSpan timeDiff = DateTime.Now - start;
                if (b1.IsNear && b3.IsFar && timeDiff.Seconds > 8)
                {
                    GoToRow(BLE2_POSITION);
                     CenterOutLoop(b3, b2);
                     return;
                }

                GoNext(BLE2_POSITION);

                await Task.Delay(INVERVAL);
            }
        }

        private async void CenterOutLoop(BleModel b1, BleModel b2)
        {
            /*
             *  yaklaşılmakta olan  ble'yi b1 varsıyorum. bir arkadaki ble b2 
             */
            DateTime start = DateTime.Now;
            Debug.WriteLine("CenterOutLoop");
            while (true)
            {
                CulculateCurrentPositions();
                TimeSpan timeDiff = DateTime.Now - start;
                if (b1.IsNear && timeDiff.Seconds > 8)
                {
                    GoToRow(BLE3_POSITION);
                      EdgeOutLoop(b1);
                      return;
                }

                GoNext(BLE3_POSITION);

                await Task.Delay(INVERVAL);
            }
        }

        private async void EdgeOutLoop(BleModel b1)
        {
            DateTime start = DateTime.Now;
            Debug.WriteLine("EdgeOutLoop");
            while (true)
            {
                b1.CalculateCurrentPosition();
                TimeSpan timeDiff = DateTime.Now - start;
                if (b1.IsUnknown && timeDiff.Seconds > 8)
                {
                    GoToRow(END_POSITION);
                    return;
                }

                GoNext(END_POSITION);

                await Task.Delay(INVERVAL);
            }
        }
    }
}