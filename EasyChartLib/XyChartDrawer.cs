using EasyChartLib.PercentageGraphics;
using System;
using System.Drawing;

namespace EasyChartLib
{
    internal class XyChartDrawer
    {
        public enum EDirection
        {
            LeftToRight,
            RightToLeft,
        }

        private readonly PercentGraphics _drawingArea;
        private readonly Axis _lookupAxis;
        private readonly Axis _valuesAxis;

        public XyChartDrawer(PercentGraphics drawingArea, Axis lookupAxis, Axis valuesAxis)
        {
            _drawingArea = drawingArea;
            _lookupAxis = lookupAxis;
            _valuesAxis = valuesAxis;
        }

        public void DrawPoint(Brush brush, float lookup, float value)
        {
            var lookupPercent = _lookupAxis.GetValueInPecentage(lookup);
            var valuePercent = _valuesAxis.GetValueInPecentage(value);

            _drawingArea.DrawPoint(new Pen(brush, 3), lookupPercent, valuePercent);
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
            _drawingArea.DrawLine(pen, fromPoint, toPoint);
        }



    }
}
