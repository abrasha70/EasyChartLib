using EasyChartLib.PercentageGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasyChartLib.RankChartRequest;

namespace EasyChartLib
{
    public class EasyChart
    {
        public Image GenerateMultiRankChart(ChartSettings settings, List<Category> categories)
        {
            var bmp = new Bitmap(settings.Width, settings.Height);

            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);


            var axisWidth = imageArea.MeasureString("999", settings.Font).Width;
            var areas = imageArea.HorizontalSplit(axisWidth * 1.5f);

            var axisArea = areas[0];
            var chartsArea = areas[1];

            var textSize = chartsArea.MeasureString("Text", settings.Font);
            var textHeight = chartsArea.MeasureString("Text", settings.Font).Height;


            var categoryHeight = textHeight * 1.5f;
            axisArea = axisArea.CreateSubArea(0, 0, 100, 100 - categoryHeight);

            var axis = GetAxis(categories, textHeight, settings.AxisMode);

            var axisDrawer = new ChartDrawer(axisArea, axis, ChartDrawer.EDirection.BottomToTop);
            axisDrawer.DrawAxis(Pens.Black, settings.Font, Brushes.Black);


            var categoryAreas = chartsArea.HorizontalMultiSplit(categories.Count);
            for (int index = 0; index < categories.Count; index++)
            {
                var categoryData = categories[index];
                var categoryArea = categoryAreas[index];

                var periodParts = categoryArea.VerticalSplit(100 - categoryHeight);
                var graphArea = periodParts[0];
                var labelsArea = periodParts[1];

                DrawCategoryGraphs(settings, graphArea, categoryData, axis, ChartDrawer.EDirection.BottomToTop);

                DrawCategoryLabels(settings, labelsArea, categoryData);

            }

            return bmp;
        }



        private void DrawCategoryGraphs(ChartSettings settings, PercentGraphics graphArea, Category categoryData, Axis axis, ChartDrawer.EDirection direction)
        {
            var drawer = new ChartDrawer(graphArea, axis, direction);

            //Ranks:
            foreach (var rank in categoryData.Ranks)
            {
                var colorHex = settings.RankDefs[rank.Key].ColorHex;
                var brush = HexToBrush(colorHex, settings.RanksAlpha);
                drawer.FillChartRange(brush, rank.MinValue, rank.MaxValue);
            }

            //Value:
            if (categoryData.Measured.HasValue)
            {
                drawer.FillChartColumn(Brushes.Navy, categoryData.Measured.Value, 25);
            }

            //Target:
            if (categoryData.Target.HasValue)
            {
                drawer.DrawLevelLine(new Pen(Color.Red, 2), categoryData.Target.Value, 50);
            }

            //Axis 0:
            drawer.DrawLevelLine(Pens.Black, 0);
        }


        private void DrawCategoryLabels(ChartSettings settings, PercentGraphics labelsArea, Category categoryData)
        {
            var alignment = new Alignment { Horizontal = HorizontalAlignment.CenteredToPoint, Vertical = VerticalAlignment.CenteredToPoint };
            labelsArea.DrawString(categoryData.Name, settings.Font, Brushes.Black, new PointF(50, 50), alignment);
        }



        private Axis GetAxis(Category category, float textLengthPercentage, EAxisMode axisMode)
        {
            var categories = new List<Category> { category };
            return GetAxis(categories, textLengthPercentage, axisMode);
        }

        private Axis GetAxis(List<Category> categories, float textLengthPercentage, EAxisMode axisMode)
        {
            var axis = new Axis();
            var allValues = GetRelevantValues(categories, axisMode);
            var minValue = allValues.Min();
            var maxValue = allValues.Max();
            var gap = maxValue - minValue;
            axis.MinValue = minValue - gap * 0.1f;
            axis.MaxValue = maxValue + gap * 0.1f;

            if (axis.MinValue < 0 && minValue >= 0) axis.MinValue = 0;

            axis.TickSize = CalcTickSize(axis.MinValue, axis.MaxValue, textLengthPercentage);
            return axis;
        }

        private float CalcTickSize(float minValue, float maxValue, float textLengthPercentage)
        {
            var maxAmountOfTicks = (float)Math.Floor(100f / (textLengthPercentage * 2f));
            var gap = maxValue - minValue;
            var tickSize = gap / maxAmountOfTicks;
            tickSize = AutoRound(tickSize);

            return tickSize;
        }

        private float AutoRound(float value)
        {
            var power = Math.Round(Math.Log10(value));
            var round1 = 1f * (float)Math.Pow(10, power);
            var round2 = 2f * (float)Math.Pow(10, power);
            var round5 = 5f * (float)Math.Pow(10, power);
            var round10 = 10f * (float)Math.Pow(10, power);
            if (value < round1) return round1;
            if (value < round2) return round2;
            if (value < round5) return round5;
            return round10;
        }



        private List<float> GetRelevantValues(List<Category> categories, EAxisMode axisMode)
        {
            switch (axisMode)
            {
                case EAxisMode.All:
                    return GetAllValues(categories);
                case EAxisMode.Focused:
                    return GetFocusedValues(categories);
                case EAxisMode.FocusedAndAround:
                    return GetFocusedAndAroundValues(categories);
                case EAxisMode.FocusedAndNearby:
                    return GetFocusedAndNearbyValues(categories);
                default:
                    return null;
            }
        }

        private List<float> GetAllValues(List<Category> categories)
        {
            var measured = categories.Select(category => category.Measured);
            var targets = categories.Select(category => category.Target);
            var rankMins = categories.SelectMany(category => category.Ranks.Select(rank => rank.MinValue));
            var rankMaxs = categories.SelectMany(category => category.Ranks.Select(rank => rank.MaxValue));

            var unified = measured.Union(targets).Union(rankMins).Union(rankMaxs);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private List<float> GetFocusedValues(List<Category> categories)
        {
            var measuredValues = categories.Select(category => category.Measured);
            var targetValues = categories.Select(category => category.Target);

            var unified = measuredValues.Union(targetValues);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private List<float> GetFocusedAndAroundValues(List<Category> categories)
        {
            var measured = categories.Select(category => category.Measured);
            var targets = categories.Select(category => category.Target);
            var surroundRanks = categories.SelectMany(
                category => category.Ranks.Where(
                    rank =>
                    IsBetween(category.Measured, rank.MinValue, rank.MaxValue) ||
                    IsBetween(category.Target, rank.MinValue, rank.MaxValue)
                    )
                );
            var rankMins = surroundRanks.Select(rank => rank.MinValue);
            var rankMaxs = surroundRanks.Select(rank => rank.MaxValue);

            var unified = measured.Union(targets).Union(rankMins).Union(rankMaxs);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private List<float> GetFocusedAndNearbyValues(List<Category> categories)
        {
            var allValues = new List<float?>();

            foreach (var category in categories)
            {
                allValues.Add(category.Measured);
                allValues.Add(category.Target);
                var ranks = category.Ranks;
                for (int rankIndex = 0; rankIndex < ranks.Count; rankIndex++)
                {
                    var rank = ranks[rankIndex];
                    var prev = rankIndex > 0 ? ranks[rankIndex - 1] : null;
                    var next = rankIndex < ranks.Count - 1 ? ranks[rankIndex + 1] : null;

                    var overlapCurrent = IsBetween(category.Measured, rank.MinValue, rank.MaxValue) || IsBetween(category.Target, rank.MinValue, rank.MaxValue);
                    var overlapPrev = prev != null && (IsBetween(category.Measured, prev.MinValue, prev.MaxValue) || IsBetween(category.Target, prev.MinValue, prev.MaxValue));
                    var overlapNext = next != null && (IsBetween(category.Measured, next.MinValue, next.MaxValue) || IsBetween(category.Target, next.MinValue, next.MaxValue));

                    if (overlapCurrent || overlapPrev || overlapNext)
                    {
                        allValues.Add(rank.MinValue);
                        allValues.Add(rank.MaxValue);
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

        private Color HexToColor(string colorHex, int alpha = 255)
        {
            int argb = Int32.Parse(colorHex, NumberStyles.HexNumber);
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
    }
}
