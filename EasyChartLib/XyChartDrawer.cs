﻿using EasyChartLib.PercentageGraphics;
using System;
using System.Drawing;
using System.Runtime;

namespace EasyChartLib
{
    internal class XyChartDrawer
    {
        public enum EDirection
        {
            LeftToRight,
            RightToLeft,
        }

        private readonly PercentGraphics _chartArea;
        private readonly Axis _lookupAxis;
        private readonly Axis _valuesAxis;
        private readonly AxisDrawer _lookupAxisDrawer;
        private readonly AxisDrawer _valuesAxisDrawer;

        public XyChartDrawer(PercentGraphics drawingArea, Axis lookupAxis, Axis valuesAxis, ChartSettings settings)
        {
            //_drawingArea = drawingArea;
            _lookupAxis = lookupAxis;
            _valuesAxis = valuesAxis;


            if (settings.ShowAxis)
            {
                var font = new Font(SystemFonts.DefaultFont.FontFamily, settings.FontSize);
                var digitSizeInPercentage = drawingArea.MeasureString("0", font);
                var axisTextHeight = digitSizeInPercentage.Height;

                //var maxDigits = relevantValues.Max(value => AutoRound(value).ToString().Length);
                var maxDigits = 4;
                var spaceNeeded = digitSizeInPercentage.Width * maxDigits;
                var sideAxisWidth = spaceNeeded;
                var bottomAxisHeight = axisTextHeight * 1.5f;

                var areas = drawingArea.MatrixSplit(100 - bottomAxisHeight, sideAxisWidth);

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
                _chartArea = drawingArea;
            }

            _chartArea.DrawBorder(Pens.Black);
        }

        public void DrawPoint(Brush brush, float lookup, float value)
        {
            var lookupPercent = _lookupAxis.GetValueInPecentage(lookup);
            var valuePercent = _valuesAxis.GetValueInPecentage(value);

            _chartArea.DrawPoint(new Pen(brush, 3), lookupPercent, valuePercent);
        }

        public void DrawLine(Pen pen, float fromLookup, float fromValue, float toLookup, float toValue)
        {
            var fromPoint = new PointF(
                _lookupAxis.GetValueInPecentage(fromLookup),
                _valuesAxis.GetValueInPecentage(fromValue));

            var toPoint = new PointF(
                _lookupAxis.GetValueInPecentage(toLookup),
                _valuesAxis.GetValueInPecentage(toValue));

            DrawLine(pen, fromPoint, toPoint);
        }

        public void DrawLine(Pen pen, PointF fromPoint, PointF toPoint)
        {
            _chartArea.DrawLine(pen, fromPoint, toPoint);
        }



    }
}
