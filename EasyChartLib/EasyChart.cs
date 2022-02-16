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
        public Image GenerateMultiRankChart(ChartSettings settings, List<SingleCategoryData> categories)
        {
            var bmp = new Bitmap(settings.Width, settings.Height);

            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);

            var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
            var digitSize = imageArea.MeasureString("0", font);
            var textHeight = digitSize.Height;

            var axis = new Axis(settings, categories, digitSize, true);

            var axiesLength = axis.SpaceNeeded;
            var areas = imageArea.HorizontalSplit(axiesLength);
            var axisArea = areas[0];
            var chartsArea = areas[1];

            var categoryHeight = textHeight * 1.5f;

            if (settings.ShowAxis)
            {
                axisArea = axisArea.CreateSubArea(0, 0, 100, 100 - categoryHeight);

                var axisDrawer = new ChartDrawer(axisArea, axis, ChartDrawer.EDirection.BottomToTop);
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

                DrawCategoryGraphs(settings, graphArea, categoryData, axis, ChartDrawer.EDirection.BottomToTop);

                DrawCategoryLabels(settings, labelsArea, categoryData);
            }

            return bmp;
        }


        public Image GenerateSingleRankChart(ChartSettings settings, SingleCategoryData chartData)
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

            var axis = new Axis(settings, chartData, digitSize, false);
            if (settings.ShowAxis)
            {
                var axisDrawer = new ChartDrawer(axisArea, axis, ChartDrawer.EDirection.LeftToRight);
                axisDrawer.DrawAxis(Pens.Black, font, Brushes.Black);
            }
            else
            {
                chartsArea = imageArea;
            }

            DrawCategoryGraphs(settings, chartsArea, chartData, axis, ChartDrawer.EDirection.LeftToRight);

            chartsArea.DrawRectangle(Pens.Black, 0, 0, 100, 100);

            return bmp;
        }










        private void DrawCategoryGraphs(ChartSettings settings, PercentGraphics graphArea, SingleCategoryData categoryData, Axis axis, ChartDrawer.EDirection direction)
        {
            var drawer = new ChartDrawer(graphArea, axis, direction);

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



        private void DrawCategoryLabels(ChartSettings settings, PercentGraphics labelsArea, SingleCategoryData categoryData)
        {
            var alignment = new Alignment { Horizontal = HorizontalAlignment.CenteredToPoint, Vertical = VerticalAlignment.CenteredToPoint };
            var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
            labelsArea.DrawString(categoryData.Name, font, Brushes.Black, new PointF(50, 50), alignment);
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
