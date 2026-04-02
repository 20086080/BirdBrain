using BirdBrain.Models;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Services
{
    public class ChartService
    {
        private SKColor GetTextColor()
        {
            var color = (Color)Application.Current!.Resources["TextPrimary"];       // Create Colour palette of Charts
            return new SKColor(
                (byte)(color.Red * 255),
                (byte)(color.Green * 255),
                (byte)(color.Blue * 255),
                (byte)(color.Alpha * 255));
        }

        // Build chart for Location Sightings Page - Line Chart
        public (ISeries[] Series, List<string> Labels, Axis[] XAxes, Axis[] YAxes) 
            BuildLocationChart(List<LocationDailyObs> data)
        {
            if (data == null || !data.Any())
            {
                return (Array.Empty<ISeries>(), new List<string>(), Array.Empty<Axis>(), Array.Empty<Axis>());

            }

            var skColor = GetTextColor();
            var values = data.Select(x => x.Sightings).ToList();
            var labels = data
                .Select(x =>
                {
                    if (DateTime.TryParse(x.ObsDt, out var dt))
                        return dt.ToString("dd/M");
                        return string.Empty; // or fallback
                })
                .ToList();

            var maxSightings = values.Max();

            var series = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = values,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(skColor) { StrokeThickness = 3 },
                    Fill = null
                }
            };

            var xAxes = new Axis[]
            {
                new Axis
                {
                    Labels = labels,
                    LabelsRotation = 20,
                    MinStep = 1,
                    SeparatorsPaint = null,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor)
                }
            };

            var yAxes = new Axis[]
            {
                new Axis
                {
                    SeparatorsPaint = null,
                    MinStep = 5,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor),
                    MaxLimit = maxSightings + 5
                }
            };

            return (series, labels, xAxes, yAxes);
        }

        // Build chart for Bird Sightings Page - Line Chart
        public (ISeries[] Series, List<string> Labels, Axis[] XAxes, Axis[] YAxes)
            BuildBirdChart(List<BirdDailyObs> data)
        {
            if (data == null || !data.Any())
            {
                return (Array.Empty<ISeries>(), new List<string>(), Array.Empty<Axis>(), Array.Empty<Axis>());
            }

            var skColor = GetTextColor();

            var values = data.Select(x => x.Sightings).ToList();

            var labels = data
                .Select(x => DateTime.Parse(x.ObsDt).ToString("dd/M"))
                .ToList();

            var maxSightings = values.Max();

            var series = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = values,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(skColor) { StrokeThickness = 3 },
                    Fill = null
                }
            };

            var xAxes = new Axis[]
            {
                new Axis
                {
                    Labels = labels,
                    LabelsRotation = 20,
                    MinStep = 1,
                    SeparatorsPaint = null,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor)
                }
            };

            var yAxes = new Axis[]
            {
                new Axis
                {
                    SeparatorsPaint = null,
                    MinStep = 5,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(skColor),
                    MaxLimit = maxSightings + 5
                }
            };

            return (series, labels, xAxes, yAxes);
        }
    }
}
