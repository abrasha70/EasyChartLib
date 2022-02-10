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

            var categoryHeight = chartsArea.MeasureString("Category", settings.Font).Height * 1.5f;

            axisArea = axisArea.CreateSubArea(0, 0, 100, 100 - categoryHeight);

            var axis = new Axis
            {
                MinValue = 0f,
                MaxValue = 30f,
                TickSize = 5f,
            };

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


        private void DrawCategoryLabels(ChartSettings settings, PercentGraphics labelsArea, Category categoryData)
        {
            var alignment = new Alignment { Horizontal = HorizontalAlignment.CenteredToPoint, Vertical = VerticalAlignment.CenteredToPoint };
            labelsArea.DrawString(categoryData.Name, settings.Font, Brushes.Black, new PointF(50, 50), alignment);
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
