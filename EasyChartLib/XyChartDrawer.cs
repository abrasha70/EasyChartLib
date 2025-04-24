﻿using EasyChartLib.PercentageGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;

namespace EasyChartLib
{
    internal class XyChartDrawer
    {
        private readonly Bitmap _bmp;
        private readonly PercentGraphics _imageArea;
        private readonly PercentGraphics _chartArea;
        private readonly Axis _lookupAxis;
        private readonly Axis _valuesAxis;
        private readonly AxisDrawer _lookupAxisDrawer;
        private readonly AxisDrawer _valuesAxisDrawer;

        public XyChartDrawer(ChartSettings settings, Axis lookupAxis, Axis valuesAxis)
        {
            _bmp = new Bitmap(settings.Width, settings.Height);
            _imageArea = CreateImageArea(_bmp);

            //_drawingArea = drawingArea;
            _lookupAxis = lookupAxis;
            _valuesAxis = valuesAxis;


            if (settings.ShowAxis)
            {
                var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
                var digitSizeInPercentage = _imageArea.MeasureString("0", font);

                //var maxDigits = relevantValues.Max(value => AutoRound(value).ToString().Length);
                var maxDigits = 4;
                var spaceNeeded = digitSizeInPercentage.Width * maxDigits;
                var sideAxisWidth = spaceNeeded;
                var bottomAxisHeight = digitSizeInPercentage.Height * 1.5f;

                var areas = _imageArea.MatrixSplit(100 - bottomAxisHeight, sideAxisWidth);

                var chartArea = areas[0, 1];
                var valuesAxisArea = areas[0, 0];
                var lookupAxisArea = areas[1, 1];

                _chartArea = chartArea;
                _lookupAxisDrawer = new AxisDrawer(lookupAxisArea, lookupAxis, digitSizeInPercentage);
                _valuesAxisDrawer = new AxisDrawer(valuesAxisArea, valuesAxis, digitSizeInPercentage);

                _lookupAxisDrawer.DrawAxis(Pens.Black, font, Brushes.Black);
                _valuesAxisDrawer.DrawAxis(Pens.Black, font, Brushes.Black);
            }
            else
            {
                _chartArea = _imageArea;
            }

            _chartArea.DrawBorder(Pens.Black);
        }

        public void DrawPoint(Pen pen, float lookup, float value)
        {
            var lookupPercent = _lookupAxis.GetValueInPecentage(lookup);
            var valuePercent = _valuesAxis.GetValueInPecentage(value);

            if (lookupPercent < 0 || lookupPercent > 100) return;
            if (valuePercent < 0 || valuePercent > 100) return;

            _chartArea.DrawPoint(pen, lookupPercent, valuePercent);
        }

        public void DrawLine(Pen pen, float fromLookup, float fromValue, float toLookup, float toValue)
        {
            var fromPoint = new PointF(
                _lookupAxis.GetValueInPecentage(fromLookup),
                _valuesAxis.GetValueInPecentage(fromValue));

            var toPoint = new PointF(
                _lookupAxis.GetValueInPecentage(toLookup),
                _valuesAxis.GetValueInPecentage(toValue));

            DrawLineInPercentages(pen, fromPoint, toPoint);
        }

        public void DrawLine(Pen pen, IEnumerable<PointF> points)
        {
            GetPointsInPercentage(points);
        }

        public void DrawAreaGraph(Brush brush, IEnumerable<PointF> points)
        {
            var firstPoint = points.First();
            var lastPoint = points.Last();

            var areaPoints = new List<PointF>();
            areaPoints.Add(new PointF(firstPoint.X, 0));
            areaPoints.AddRange(points);
            areaPoints.Add(new PointF(lastPoint.X, 0));

            var areaPercPoints = GetPointsInPercentage(areaPoints);

            _chartArea.FillPolygon(brush, areaPercPoints);
        }

        public void DrawLineGraph(Pen pen, IEnumerable<PointF> points)
        {
            var areaPercPoints = GetPointsInPercentage(points);

            _chartArea.DrawLines(pen, areaPercPoints);
        }



        private void DrawLineInPercentages(Pen pen, PointF fromPoint, PointF toPoint)
        {
            if (fromPoint.X < 0 || fromPoint.X > 100) return;
            if (fromPoint.Y < 0 || fromPoint.Y > 100) return;

            if (toPoint.X < 0 || toPoint.X > 100) return;
            if (toPoint.Y < 0 || toPoint.Y > 100) return;

            _chartArea.DrawLine(pen, fromPoint, toPoint);
        }


        private IEnumerable<PointF> GetPointsInPercentage(IEnumerable<PointF> points)
        {
            var percPoints = points.Select(p =>
            {
                var percX = _lookupAxis.GetValueInPecentage(p.X);
                var percY = _valuesAxis.GetValueInPecentage(p.Y);
                var percPoint = new PointF(percX, percY);
                return percPoint;
            }
            );
            return percPoints;
        }

        private static PercentGraphics CreateImageArea(Bitmap bmp)
        {
            var margin = new ActualMargin(0, 0, 1, 1);
            var imageArea = new PercentGraphics(bmp, margin);
            imageArea.FillRectange(Brushes.White, 0, 0, 100, 100);
            return imageArea;
        }

        internal Bitmap GetBmp()
        {
            return _bmp;
        }
    }
}
