using EasyChartLib.PercentageGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasyChartLib.RankChartParameters;

namespace EasyChartLib
{
    public class EasyChart
    {
        public Image GenerateMultiRankChart(RanksChartSettings settings, List<SingleCategoryData> categories)
        {
            var bmp = new Bitmap(settings.Width, settings.Height);

            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);

            var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
            var digitSizeInPercentage = imageArea.MeasureString("0", font);
            var textHeight = digitSizeInPercentage.Height;

            var relevantValues = GetRelevantValues(categories, settings.ZoomMode);
            var axis = new Axis(relevantValues);

            var maxDigits = relevantValues.Max(value => AutoRound(value).ToString().Length);
            var spaceNeeded = digitSizeInPercentage.Width * maxDigits;
            var axiesLength = spaceNeeded;
            var areas = imageArea.HorizontalSplit(axiesLength);
            var axisArea = areas[0];
            var chartsArea = areas[1];

            var categoryHeight = textHeight * 1.5f;

            if (settings.ShowAxis)
            {
                axisArea = axisArea.CreateSubArea(0, 0, 100, 100 - categoryHeight);

                var axisDrawer = new AxisDrawer(axisArea, axis, EDirection.BottomToTop, digitSizeInPercentage);
                axisDrawer.DrawAxis(Pens.Black, font, Brushes.Black);
            }
            else
            {
                chartsArea = imageArea;
            }


            var categoryAreas = chartsArea.HorizontalMultiSplit(categories.Count);
            for (int index = 0; index < categories.Count; index++)
            {
                var categoryData = categories[index];
                var categoryArea = categoryAreas[index];

                var periodParts = categoryArea.VerticalSplit(100 - categoryHeight);
                var graphArea = periodParts[0];
                var labelsArea = periodParts[1];

                DrawCategoryGraphs(settings, graphArea, categoryData, axis, RankChartDrawer.EDirection.BottomToTop);

                DrawCategoryLabels(settings, labelsArea, categoryData);
            }

            return bmp;
        }


        public Image GenerateSingleRankChart(RanksChartSettings settings, SingleCategoryData chartData)
        {
            var bmp = new Bitmap(settings.Width, settings.Height);

            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);

            var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
            var digitSizeInPercentage = imageArea.MeasureString("0", font);
            var axisTextHeight = digitSizeInPercentage.Height;

            var areas = imageArea.VerticalSplit(100 - axisTextHeight * 1.5f);
            var chartsArea = areas[0];
            var axisArea = areas[1];

            var relevantValues = GetRelevantValues(chartData, settings.ZoomMode);

            var axis = new Axis(relevantValues);
            if (settings.ShowAxis)
            {
                var axisDrawer = new AxisDrawer(axisArea, axis, EDirection.LeftToRight, digitSizeInPercentage);
                axisDrawer.DrawAxis(Pens.Black, font, Brushes.Black);
            }
            else
            {
                chartsArea = imageArea;
            }

            DrawCategoryGraphs(settings, chartsArea, chartData, axis, RankChartDrawer.EDirection.LeftToRight);

            chartsArea.DrawBorder(Pens.Black);

            return bmp;
        }



        public Image GenerateLmsChart(LmsChartSettings settings, LmsMeasurement measurement)
        {
            var bmp = new Bitmap(settings.Width, settings.Height);

            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);

            var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
            var digitSize = imageArea.MeasureString("0", font);
            var axisTextHeight = digitSize.Height;

            var areas = imageArea.VerticalSplit(100 - axisTextHeight * 1.5f);
            var chartsArea = areas[0];
            var axisArea = areas[1];

            var lookupAxis = new Axis(measurement.Lookup);
            var valuesAxis = new Axis(measurement.Value);

            if (settings.ShowAxis)
            {
                var axisDrawer = new AxisDrawer(axisArea, lookupAxis, EDirection.LeftToRight, digitSize);
                axisDrawer.DrawAxis(Pens.Black, font, Brushes.Black);
            }
            else
            {
                chartsArea = imageArea;
            }

            var xyChart = new XyChartDrawer(chartsArea, lookupAxis, valuesAxis);
            xyChart.DrawPoint(measurement.Lookup, measurement.Value, Brushes.Navy);

            //DrawCategoryGraphs(settings, chartsArea, chartData, lookupAxis, ChartDrawer.EDirection.LeftToRight);

            //TODO:
            //GetLMSFile
            //DrawLmsGraphs
            //DrawLmsMeasurements

            chartsArea.DrawBorder(Pens.Black);

            return bmp;
        }





        private float AutoRound(float value)
        {
            var power10 = Math.Floor(Math.Log10(value));
            var round1 = 1f * (float)Math.Pow(10, power10);
            var round2 = 2f * (float)Math.Pow(10, power10);
            var round5 = 5f * (float)Math.Pow(10, power10);
            var round10 = 10f * (float)Math.Pow(10, power10);
            if (value < round1) return round1;
            if (value < round2) return round2;
            if (value < round5) return round5;
            return round10;
        }



        private void DrawCategoryGraphs(RanksChartSettings settings, PercentGraphics graphArea, SingleCategoryData categoryData, Axis axis, RankChartDrawer.EDirection direction)
        {
            var drawer = new RankChartDrawer(graphArea, axis, direction);

            //Ranks:
            if (settings.RankColors != null)
            {
                var rankRanges = categoryData.GetRanksAsRanges();
                foreach (var rank in rankRanges)
                {
                    var colorHex = settings.RankColors[rank.Index];
                    var brush = HexToBrush(colorHex, settings.RanksAlpha);
                    drawer.FillChartRange(brush, rank.FromValue, rank.ToValue);
                }
            }


            //Measured Value:
            if (categoryData.Measured.HasValue)
            {
                drawer.FillChartColumn(Brushes.Navy, categoryData.Measured.Value, 25);
            }

            //Target Value:
            if (categoryData.Target.HasValue)
            {
                drawer.DrawLevelLine(new Pen(Color.Red, 2), categoryData.Target.Value, 50);
            }

            //Axis 0:
            drawer.DrawLevelLine(Pens.Black, 0);
        }



        private void DrawCategoryLabels(RanksChartSettings settings, PercentGraphics labelsArea, SingleCategoryData categoryData)
        {
            var alignment = new Alignment { Horizontal = HorizontalAlignment.CenteredToPoint, Vertical = VerticalAlignment.CenteredToPoint };
            var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
            labelsArea.DrawString(categoryData.Name, font, Brushes.Black, new PointF(50, 50), alignment);
        }









        private Color HexToColor(string colorHex, int alpha = 255)
        {
            int argb = Int32.Parse(colorHex.Replace("#", ""), NumberStyles.HexNumber);
            var color = Color.FromArgb(argb);
            color = Color.FromArgb(alpha, color);
            return color;
        }

        private Brush HexToBrush(string colorHex, int alpha = 255)
        {
            var color = HexToColor(colorHex, alpha);
            var brush = new SolidBrush(color);
            return brush;
        }




        private IEnumerable<float> GetRelevantValues(SingleCategoryData category, EZoomMode zoomMode)
        {
            return GetRelevantValues(new [] { category }, zoomMode);
        }

        private IEnumerable<float> GetRelevantValues(IEnumerable<SingleCategoryData> categories, EZoomMode zoomMode)
        {
            switch (zoomMode)
            {
                case EZoomMode.All:
                    return GetAllValues(categories);
                case EZoomMode.Focused:
                    return GetFocusedValues(categories);
                case EZoomMode.FocusedAndAround:
                    return GetFocusedAndAroundValues(categories);
                case EZoomMode.FocusedAndNearby:
                    return GetFocusedAndNearbyValues(categories);
                default:
                    return null;
            }
        }

        private IEnumerable<float> GetAllValues(IEnumerable<SingleCategoryData> categories)
        {
            var measured = categories.Select(category => category.Measured);
            var targets = categories.Select(category => category.Target);
            var rankMins = categories.SelectMany(category => category.GetRanksAsRanges().Select(rank => rank.FromValue));
            var rankMaxs = categories.SelectMany(category => category.GetRanksAsRanges().Select(rank => rank.ToValue));

            var unified = measured.Union(targets).Union(rankMins).Union(rankMaxs);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private IEnumerable<float> GetFocusedValues(IEnumerable<SingleCategoryData> categories)
        {
            var measuredValues = categories.Select(category => category.Measured);
            var targetValues = categories.Select(category => category.Target);

            var unified = measuredValues.Union(targetValues);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private IEnumerable<float> GetFocusedAndAroundValues(IEnumerable<SingleCategoryData> categories)
        {
            var measured = categories.Select(category => category.Measured);
            var targets = categories.Select(category => category.Target);
            var surroundRanks = categories.SelectMany(
                category => category.GetRanksAsRanges().Where(
                    rank =>
                    IsBetween(category.Measured, rank.FromValue, rank.ToValue) ||
                    IsBetween(category.Target, rank.FromValue, rank.ToValue)
                    )
                );
            var rankMins = surroundRanks.Select(rank => rank.FromValue);
            var rankMaxs = surroundRanks.Select(rank => rank.ToValue);

            var unified = measured.Union(targets).Union(rankMins).Union(rankMaxs);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private IEnumerable<float> GetFocusedAndNearbyValues(IEnumerable<SingleCategoryData> categories)
        {
            var allValues = new List<float?>();

            foreach (var category in categories)
            {
                allValues.Add(category.Measured);
                allValues.Add(category.Target);
                var ranks = category.GetRanksAsRanges();
                for (int rankIndex = 0; rankIndex < ranks.Count; rankIndex++)
                {
                    var rank = ranks[rankIndex];
                    var prev = rankIndex > 0 ? ranks[rankIndex - 1] : null;
                    var next = rankIndex < ranks.Count - 1 ? ranks[rankIndex + 1] : null;

                    var overlapCurrent = IsBetween(category.Measured, rank.FromValue, rank.ToValue) || IsBetween(category.Target, rank.FromValue, rank.ToValue);
                    var overlapPrev = prev != null && (IsBetween(category.Measured, prev.FromValue, prev.ToValue) || IsBetween(category.Target, prev.FromValue, prev.ToValue));
                    var overlapNext = next != null && (IsBetween(category.Measured, next.FromValue, next.ToValue) || IsBetween(category.Target, next.FromValue, next.ToValue));

                    if (overlapCurrent || overlapPrev || overlapNext)
                    {
                        allValues.Add(rank.FromValue);
                        allValues.Add(rank.ToValue);
                    }
                }
            }
            var result = allValues.Where(item => item.HasValue).Select(item => item.Value).Distinct();

            return result.ToList();
        }


        private bool IsBetween(float? testedValue, float? minValue, float? maxValue)
        {
            if (!testedValue.HasValue) return false;
            if (maxValue.HasValue && testedValue > maxValue) return false;
            if (minValue.HasValue && testedValue < minValue) return false;
            return true;
        }
    }
}
