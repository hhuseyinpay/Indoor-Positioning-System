using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;


namespace IndoorPositioning
{
    public enum Positions
    {
        Near,
        Far,
        Unknown
    }

    public enum Motion
    {
        Near,
        Away,
        Unknown
    }

    public class BleModel
    {
        public string Name { get; }

        private readonly List<int> _values;

        private int _lastCount;
        private double _lastAverage;
        private Positions _lastPosition;

        public BleModel(string name)
        {
            Name = name;
            _lastCount = 0;
            _lastPosition = 0;
            _lastAverage = 0;
            _values = new List<int>();
        }

        public void AddValue(int value)
        {
            _values.Insert(0, value);
        }


        public Positions GetCurrentPosition()
        {
            var diff = _values.Count - _lastCount;
            if (diff == 0)
                return _lastPosition;
            _lastAverage = _values.GetRange(0, diff).Average();
            _lastCount = _values.Count;

            if (_lastAverage < -90)
                _lastPosition = Positions.Unknown;

            if (_lastCount > -90 && _lastCount < -75)
                _lastPosition = Positions.Far;

            if (_lastCount > -75)
                _lastPosition = Positions.Near;

            return _lastPosition;
        }

        public bool IsComeNear()
        {
            /*
             *son ortalama genel ortalamadan düşükse yaklaşıyor demektir ????
             */
            return _lastAverage > _values.GetRange(0, 10).Average();
        }
    }
}