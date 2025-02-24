﻿using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace ViewModelsSamples.StepLines.Zoom;

public class ViewModel
{
    public ViewModel()
    {
        var values = new int[100];
        var r = new Random();
        var t = 0;

        for (var i = 0; i < 100; i++)
        {
            t += r.Next(-90, 100);
            values[i] = t;
        }

        SeriesCollection = new ISeries[] { new StepLineSeries<int> { Values = values } };
    }

    public IEnumerable<ISeries> SeriesCollection { get; set; }
}
