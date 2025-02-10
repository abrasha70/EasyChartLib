using EasyChartLib.PercentageGraphics;
using System;
using System.Drawing;

namespace EasyChartLib
{
    internal class AxisDrawer
    {
        private readonly PercentGraphics _drawingArea;
        private readonly Axis _axis;
        private readonly DirectionObj _direction;

        internal AxisDrawer(PercentGraphics drawingArea, Axis axis, EDirection direction)
        {
            _drawingArea = drawingArea;
            _axis = axis;
            _direction = new DirectionObj(direction);
        }

        internal void DrawAxis(Pen tickPen, Font font, Brush textColor)
        {
            var tickSize = (decimal)_axis.TickLength;
            var minValue = (decimal)Math.Ceiling((decimal)_axis.MinValue / tickSize) * tickSize;
            var maxValue = (decimal)Math.Floor((decimal)_axis.MaxValue / tickSize) * tickSize;
            var textFormat = _axis.GetAxisTextFormat();

            for (decimal tick = minValue; tick <= maxValue; tick += tickSize)
            {
                var percentTick = _axis.GetValueInPecentage((float)tick, _direction.IsReversed);

                //crop:
                if (tick < minValue || tick > maxValue) continue;
                if (percentTick < 5 || percentTick > 95) continue;


                if (_direction.IsVertical)
                {
                    DrawLevelLine(tickPen, (float)tick, 75, 25);
                    var alignment = new Alignment { Horizontal = HorizontalAlignment.LeftToPoint, Vertical = VerticalAlignment.CenteredToPoint };
                    _drawingArea.DrawString(tick.ToString(textFormat), font, textColor, new PointF(75, percentTick), alignment);
                }
                else
                {
                    DrawLevelLine(tickPen, (float)tick, 0, 25);
                    var alignment = new Alignment { Horizontal = HorizontalAlignment.CenteredToPoint, Vertical = VerticalAlignment.AbovePoint };
                    _drawingArea.DrawString(tick.ToString(textFormat), font, textColor, new PointF(percentTick, 100), alignment);
                }
            }
        }

        private void DrawLevelLine(Pen pen, float value, float offset, float lineSize)
        {
            var valuePercentage = _axis.GetValueInPecentage(value, _direction.IsReversed);

            //crop:
            if (valuePercentage < 0 || valuePercentage > 100) return;

            DrawRotatingLine(pen, offset, valuePercentage, offset + lineSize, valuePercentage);
        }

        private void DrawRotatingLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (_direction.IsVertical)
            {
                _drawingArea.DrawLine(pen, x1, y1, x2, y2);
            }
            else
            {
                _drawingArea.DrawLine(pen, y1, x1, y2, x2);
            }
        }


    }
}
