// The MIT License(MIT)
//
// Copyright(c) 2021 Alberto Rodriguez Orozco & LiveCharts Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ViewModelsSamples.Test.HighPerformanceTest;

public class HighPerformanceViewModel : INotifyPropertyChanged
{
    #region Private Fields
    private Random m_Random = new Random();
    private LineSeries<PhysicalValue> m_TempSeries1;
    private LineSeries<PhysicalValue> m_TempSeries2;
    private LineSeries<PhysicalValue> m_TempSeries3;
    private LineSeries<PhysicalValue> m_TempSeries4;
    private LineSeries<PhysicalValue> m_TempSeries5;
    private Timer m_FastTimer;
    private Timer m_SlowTimer;
    private double m_BaseTemp = 25.0;
    private double m_TrendFactor = 1.0;
    private bool m_IsInitialized;
    #endregion

    #region Private Methods
    private void StartLiveValues(object o)
    {
        if (!m_IsInitialized)
            Initialize();

        m_FastTimer = new Timer(AddFastValue, null, 0, 1);   //(TimeSpan.FromMilliseconds(1), DispatcherPriority.Input, AddFastValue, Application.Current.Dispatcher);
        m_SlowTimer = new Timer(AddSlowValue, null, 0, 50);
    }

    private void Initialize()
    {
        LiveCharts.Configure(settings => settings.HasMap<PhysicalValue>((a, b) =>
        {
            b.PrimaryValue = a.Value;
            b.SecondaryValue = a.TimeStamp.Ticks;
        }));

        m_TempSeries1 = new LineSeries<PhysicalValue>
        {
            LineSmoothness = 0,
            Stroke = new SolidColorPaint(SKColors.Blue, 1),
            Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(100)),
            GeometryStroke = new SolidColorPaint(SKColors.Blue),
            GeometrySize = 8,
            GeometryFill = new SolidColorPaint(SKColors.White),
            Values = new ObservableCollection<PhysicalValue>(),
            Name = "Temperature1",
            AnimationsSpeed = TimeSpan.Zero,
        };
        m_TempSeries2 = new LineSeries<PhysicalValue>
        {
            LineSmoothness = 0,
            Stroke = new SolidColorPaint(SKColors.Red, 1),
            Fill = new SolidColorPaint(SKColors.Red.WithAlpha(100)),
            GeometryStroke = new SolidColorPaint(SKColors.Red),
            GeometrySize = 8,
            GeometryFill = new SolidColorPaint(SKColors.White),
            Values = new ObservableCollection<PhysicalValue>(),
            Name = "Temperature2",
            AnimationsSpeed = TimeSpan.Zero,
        };
        m_TempSeries3 = new LineSeries<PhysicalValue>
        {
            LineSmoothness = 0,
            Stroke = new SolidColorPaint(SKColors.DarkGreen, 1),
            Fill = new SolidColorPaint(SKColors.DarkGreen.WithAlpha(100)),
            GeometryStroke = new SolidColorPaint(SKColors.DarkGreen),
            GeometrySize = 8,
            GeometryFill = new SolidColorPaint(SKColors.White),
            Values = new ObservableCollection<PhysicalValue>(),
            Name = "Temperature3",
            AnimationsSpeed = TimeSpan.Zero,
        };
        m_TempSeries4 = new LineSeries<PhysicalValue>
        {
            LineSmoothness = 0,
            Stroke = new SolidColorPaint(SKColors.Violet, 1),
            Fill = new SolidColorPaint(SKColors.Violet.WithAlpha(100)),
            GeometryStroke = new SolidColorPaint(SKColors.Violet),
            GeometrySize = 8,
            GeometryFill = new SolidColorPaint(SKColors.White),
            Values = new ObservableCollection<PhysicalValue>(),
            Name = "Temperature4",
            AnimationsSpeed = TimeSpan.Zero,
        };
        m_TempSeries5 = new LineSeries<PhysicalValue>
        {
            LineSmoothness = 0,
            Stroke = new SolidColorPaint(SKColors.LimeGreen, 1),
            Fill = new SolidColorPaint(SKColors.LimeGreen.WithAlpha(100)),
            GeometryStroke = new SolidColorPaint(SKColors.LimeGreen),
            GeometrySize = 8,
            GeometryFill = new SolidColorPaint(SKColors.White),
            Values = new ObservableCollection<PhysicalValue>(),
            Name = "Temperature5",
            AnimationsSpeed = TimeSpan.Zero,
        };

        SeriesCollection.Add(m_TempSeries1);
        SeriesCollection.Add(m_TempSeries2);
        SeriesCollection.Add(m_TempSeries3);
        SeriesCollection.Add(m_TempSeries4);
        SeriesCollection.Add(m_TempSeries5);
    }

    private void AddFastValue(object? state)
    {
        var f = m_Random.Next(0, 1000) >= 400 ? 0.01 : -0.0125;
        m_TrendFactor += f;

        //TODO: Using this code with unmodified DataFactory will not lead to any exception
        //var newTemp1Values = new ObservableCollection<PhysicalValue>(m_TempSeries1.Values);
        //newTemp1Values.Add(new PhysicalValue
        //{
        //    TimeStamp = DateTime.Now,
        //    Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
        //    Unit = "°C"
        //});
        //m_TempSeries1.Values = newTemp1Values;
        //
        //var newTemp2Values = new ObservableCollection<PhysicalValue>(m_TempSeries2.Values);
        //newTemp2Values.Add(new PhysicalValue
        //{
        //    TimeStamp = DateTime.Now,
        //    Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
        //    Unit = "°C"
        //});
        //m_TempSeries2.Values = newTemp1Values;
        //
        //var newTemp3Values = new ObservableCollection<PhysicalValue>(m_TempSeries3.Values);
        //newTemp3Values.Add(new PhysicalValue
        //{
        //    TimeStamp = DateTime.Now,
        //    Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
        //    Unit = "°C"
        //});
        //m_TempSeries3.Values = newTemp1Values;


        //TODO: Using this code with unmodified DataFactory will lead to an InvalidOperationException
        ((ObservableCollection<PhysicalValue>)m_TempSeries1.Values).Add(new PhysicalValue
        {
            TimeStamp = DateTime.Now,
            Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
            Unit = "°C"
        });
        ((ObservableCollection<PhysicalValue>)m_TempSeries2.Values).Add(new PhysicalValue
        {
            TimeStamp = DateTime.Now,
            Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
            Unit = "°C"
        });
        ((ObservableCollection<PhysicalValue>)m_TempSeries3.Values).Add(new PhysicalValue
        {
            TimeStamp = DateTime.Now,
            Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
            Unit = "°C"
        });
    }

    private IEnumerable<PhysicalValue> AddArrayValue(PhysicalValue[] values, PhysicalValue newValue)
    {
        var newItems = new PhysicalValue[values.Length + 1];
        for (var i = 0; i < values.Length; i++)
            newItems[i] = values[i];

        newItems[values.Length] = newValue;

        return newItems;
    }

    private void AddSlowValue(object? state)
    {
        //var f = m_Random.Next(0, 10) >= 6 ? 0.1 : -0.1;
        //m_TrendFactor += f;
        //
        //((ObservableCollection<PhysicalValue>)m_TempSeries4.Values).Add(new PhysicalValue
        //{
        //    TimeStamp = DateTime.Now,
        //    Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
        //    Unit = "°C"
        //});
        //((ObservableCollection<PhysicalValue>)m_TempSeries5.Values).Add(new PhysicalValue
        //{
        //    TimeStamp = DateTime.Now,
        //    Value = m_BaseTemp * m_TrendFactor + m_Random.NextDouble() / 10,
        //    Unit = "°C"
        //});
    }

    private void StopLiveValues(object o)
    {
        if (m_FastTimer != null)
            _ = m_FastTimer.Change(Timeout.Infinite, Timeout.Infinite);

        if (m_SlowTimer != null)
            _ = m_SlowTimer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    private void ClearValues(object o)
    {
        ((ObservableCollection<PhysicalValue>)m_TempSeries1.Values).Clear();
        ((ObservableCollection<PhysicalValue>)m_TempSeries2.Values).Clear();
        ((ObservableCollection<PhysicalValue>)m_TempSeries3.Values).Clear();
        ((ObservableCollection<PhysicalValue>)m_TempSeries4.Values).Clear();
        ((ObservableCollection<PhysicalValue>)m_TempSeries5.Values).Clear();
    }

    #endregion

    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Constructors

    public HighPerformanceViewModel()
    {
        
    }
    #endregion

    #region Commands
    public ICommand CmdStartLiveValues => new Command(StartLiveValues);

    public ICommand CmdStopLiveValues => new Command(StopLiveValues);

    public ICommand CmdClearValues => new Command(ClearValues);
    #endregion

    #region Public Properties
    public ObservableCollection<ISeries> SeriesCollection { get; } = new ObservableCollection<ISeries>();

    public IEnumerable<ICartesianAxis> XAxes => new ObservableCollection<ICartesianAxis>
    {
        new Axis
        {
            Name = "Time",
            TextSize = 16,
            UnitWidth = 0.0000005
        }
    };

    public IEnumerable<ICartesianAxis> YAxes => new ObservableCollection<ICartesianAxis>
    {
        new Axis
        {
            Name = "Temperature",
            TextSize = 16
        }
    };
    #endregion
}

public class PhysicalValue : INotifyPropertyChanged
{
    #region Private Fields
    private DateTime m_TimeStamp;
    private double m_Value;
    private string m_Name;
    private string m_Unit;
    #endregion

    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Public Properties
    public DateTime TimeStamp
    {
        get => m_TimeStamp;
        set
        {
            if (m_TimeStamp != value)
            {
                m_TimeStamp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeStamp)));
            }

        }
    }

    public double Value
    {
        get => m_Value;
        set
        {
            if (m_Value != value)
            {
                m_Value = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(nameof(Value)));
            }

        }
    }

    public string Name
    {
        get => m_Name;
        set
        {
            if (m_Name != value)
            {
                m_Name = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
    }

    public string Unit
    {
        get => m_Unit;
        set
        {
            if (m_Unit != value)
            {
                m_Unit = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(nameof(Unit)));
            }
        }
    }

  
    #endregion

}
