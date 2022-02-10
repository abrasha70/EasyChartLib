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
        public Image GenerateMultiRankChart(ChartSettings settings, List<Category> periods)
        {
            var bmp = new Bitmap(settings.Width, settings.Height);

            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);


            var axisWidth = imageArea.MeasureString("999", settings.Font).Width;
            var lengthSizes = new List<float?> { axisWidth * 1.5f, null };
            var areas = imageArea.HorizontalSplit(lengthSizes);

            var axisArea = areas[0];
            var chartsArea = areas[1];

            axisArea.DrawString("999", settings.Font, Brushes.Black, new PointF(0, 0));


            var periodAreas = chartsArea.HorizontalSplit(periods.Count);
            for (int index = 0; index < periods.Count; index++)
            {
                var periodData = periods[index];
                var periodArea = periodAreas[index];

                DrawCategoryChart(settings, periodData, periodArea);
            }
            //chartsArea.DrawRectangle(Pens.Black, 0, 0, 100, 100);

            return bmp;
        }



        private void DrawCategoryChart(ChartSettings settings, Category categoryData, PercentGraphics chartArea)
        {
            var categoryHeight = chartArea.MeasureString("Category", settings.Font).Height;

            var lengthSizes = new List<float?> { null, categoryHeight * 1.5f };
            var areas = chartArea.VerticalSplit(lengthSizes);

            var graphArea = areas[0];
            var categoryArea = areas[1];

            var alignment = new Alignment { Horizontal = HorizontalAlignment.CenteredToPoint, Vertical = VerticalAlignment.CenteredToPoint };
            categoryArea.DrawString(categoryData.Name, settings.Font, Brushes.Black, new PointF(50, 50), alignment);

            var axis = new Axis
            {
                MinValue = 0f,
                MaxValue = 30f,
                TickSize = 5f,
            };

            DrawCategoryGraphs(settings, axis, categoryData, graphArea);
        }

        private void DrawCategoryGraphs(ChartSettings settings, Axis axis, Category categoryData, PercentGraphics graphArea)
        {
            var drawer = new ChartDrawer(graphArea, axis, ChartDrawer.EDirection.BottomToTop);

            //Ranks:
            foreach (var rank in categoryData.Ranks)
            {
                var colorHex = settings.RankDefs[rank.Key].ColorHex;
                var brush = HexToBrush(colorHex, settings.RanksAlpha);
                drawer.FillChartRange(brush, rank.MinValue, rank.MaxValue);
            }

            //Value:
            if (categoryData.Value.HasValue)
            {
                drawer.FillChartColumn(Brushes.Navy, categoryData.Value.Value, 25);
            }

            //Target:
            if (categoryData.Target.HasValue)
            {
                drawer.DrawLevelLine(new Pen(Color.Red, 2), categoryData.Target.Value, 50);
            }

            //Axis 0:
            drawer.DrawLevelLine(Pens.Black, 0);
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
