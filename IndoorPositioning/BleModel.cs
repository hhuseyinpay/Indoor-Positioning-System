using System;
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

    public class BleModel : BindableObject
    {
        public string Name { get; }

        public ObservableCollection<double> Values
        {
            get => (ObservableCollection<double>) GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }

        public static readonly BindableProperty ValuesProperty =
            BindableProperty.Create(nameof(Values), typeof(ObservableCollection<double>), typeof(BleModel),
                default(ObservableCollection<double>));

        private readonly List<double> _values;
        private readonly List<double> _averageValues;
        private readonly List<double> _interPolationValues;

        private int _lastCount;
        private double _lastAverage;
        private Positions _currentPosition;

        public BleModel(string name)
        {
            Name = name;
            _lastCount = 0;
            _currentPosition = Positions.Unknown;
            _lastAverage = 0;
            _values = new List<double>();
            _averageValues = new List<double>();
            _interPolationValues = new List<double>();
            Values = new ObservableCollection<double>();
        }

        public void AddValue(int value)
        {
            _values.Insert(0, value);
            Values.Insert(0, value);
        }

        public void AddAverage(double avg)
        {
            _averageValues.Insert(0, avg);
        }

        public void CalculateCurrentPosition()
        {
            var diff = _values.Count - _lastCount;
            if (diff == 0)
                return; //_lastPosition;

            _lastAverage = _values.GetRange(0, diff).Average();
            AddAverage(_lastAverage);
            //double stdDev = _values.GetRange(0, diff).StandardDeviation();

            _lastCount = _values.Count;

            if (_lastAverage < -85)
                _currentPosition = Positions.Unknown;

            if (_lastAverage > -85 && _lastAverage < -65)
                _currentPosition = Positions.Far;

            if (_lastAverage >= -65)
                _currentPosition = Positions.Near;

            //return _lastPosition;
        }

        public bool IsNear => _currentPosition == Positions.Near;


        public bool IsFar => _currentPosition == Positions.Far;

        public bool IsUnknown => _currentPosition == Positions.Unknown;


        public bool IsFarUnknown()
        {
            return _currentPosition == Positions.Unknown || _currentPosition == Positions.Far;
        }

        public double GetCurrentAverage()
        {
            if (_averageValues.Count == 0)
                return 0;
            return _averageValues.First();
        }

        public double GetPreviousAverage()
        {
            if (_averageValues.Count == 0)
                return 0;
            return _averageValues.Count == 1 ? _averageValues.First() : _averageValues[1];
        }

        public string CurrentPositon()
        {
            switch (_currentPosition)
            {
                case Positions.Far:
                    return "Far";
                case Positions.Near:
                    return "Near";
                case Positions.Unknown:
                    return "Unknown";
                default:
                    return "Unknown";
            }
        }

        public bool IsComeNear()
        {
            /*
             *son ortalama genel ortalamadan düşükse yaklaşıyor demektir ????
             */
            return _lastAverage > _values.GetRange(0, 10).Average();
        }
    }

    public static class Extend
    {
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
    }
}