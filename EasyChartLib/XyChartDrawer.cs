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
        private readonly EDirection _direction;

        public XyChartDrawer(PercentGraphics drawingArea, Axis lookupAxis, Axis valuesAxis, EDirection direction = EDirection.LeftToRight)
        {
            _drawingArea = drawingArea;
            _lookupAxis = lookupAxis;
            _valuesAxis = valuesAxis;
            _direction = direction;
        }

        public void DrawPoint(float lookup, float value, Brush brush)
        {
            var lookupPercent = _lookupAxis.GetValueInPecentage(lookup, IsInverted());
            var valuePercent = _valuesAxis.GetValueInPecentage(value);

            _drawingArea.FillCircle(brush, lookupPercent, valuePercent, 0.5f);
        }

        public void DrawLine(float fromLookup, float fromValue, float toLookup, float toValue, Pen pen)
        {
            var p1 = new PointF(
                _lookupAxis.GetValueInPecentage(fromLookup, IsInverted()),
                _valuesAxis.GetValueInPecentage(fromValue));

            var p2 = new PointF(
                _lookupAxis.GetValueInPecentage(toLookup, IsInverted()),
                _valuesAxis.GetValueInPecentage(toValue));

            _drawingArea.DrawLine(pen, p1, p2);
        }





        private bool IsInverted()
        {
            //if (_direction == EDirection.BottomToTop) return true;
            if (_direction == EDirection.RightToLeft) return true;
            return false;
        }

    }
}
