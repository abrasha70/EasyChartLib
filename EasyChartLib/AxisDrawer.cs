using EasyChartLib.PercentageGraphics;
using System;
using System.Drawing;

namespace EasyChartLib
{
    internal class AxisDrawer
    {
        private readonly PercentGraphics _drawingArea;
        private readonly Axis _axis;

        public float TickLength { get; private set; }
        public int DecimalDigits { get; private set; }


        internal AxisDrawer(PercentGraphics drawingArea, Axis axis, SizeF digitSizeInPercentage)
        {
            _drawingArea = drawingArea;
            _axis = axis;

            var maxDigits = AutoRound((float)axis.MaxValue).ToString().Length;
            var textLengthPercentage = _axis.Direction.IsVertical ? digitSizeInPercentage.Height : digitSizeInPercentage.Width * maxDigits;
            TickLength = CalcTickSize((float)axis.MinValue, (float)axis.MaxValue, textLengthPercentage);
            DecimalDigits = GetDecimalDigits(TickLength);
        }

        internal void DrawAxis(Pen tickPen, Font font, Brush textColor)
        {
            var tickSize = (decimal)TickLength;
            var minValue = (decimal)Math.Ceiling((decimal)_axis.MinValue / tickSize) * tickSize;
            var maxValue = (decimal)Math.Floor((decimal)_axis.MaxValue / tickSize) * tickSize;
            var textFormat = GetAxisTextFormat();

            for (decimal tick = minValue; tick <= maxValue; tick += tickSize)
            {
                var percentTick = _axis.GetValueInPecentage((float)tick);

                //crop:
                if (tick < minValue || tick > maxValue) continue;
                if (percentTick < 5 || percentTick > 95) continue;


                if (_axis.Direction.IsVertical)
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
            var valuePercentage = _axis.GetValueInPecentage(value);

            //crop:
            if (valuePercentage < 0 || valuePercentage > 100) return;

            DrawRotatingLine(pen, offset, valuePercentage, offset + lineSize, valuePercentage);
        }

        private void DrawRotatingLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (_axis.Direction.IsVertical)
            {
                _drawingArea.DrawLine(pen, x1, y1, x2, y2);
            }
            else
            {
                _drawingArea.DrawLine(pen, y1, x1, y2, x2);
            }
        }







        private float CalcTickSize(float minValue, float maxValue, float textLengthPercentage)
        {
            var maxAmountOfTicks = (float)Math.Floor(100f / (textLengthPercentage * 2f));
            var gap = maxValue - minValue;
            var tickSize = gap / maxAmountOfTicks;

            var roundedTickSize = AutoRound(tickSize);
            return roundedTickSize;
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

        private int GetDecimalDigits(float tickLength)
        {
            var power10 = Math.Log10(tickLength);
            if (power10 >= 0) return 0;
            return (int)Math.Ceiling(Math.Abs(power10));
        }



        public string GetAxisTextFormat()
        {
            if (DecimalDigits == 0) return "0";
            return "0." + new string('0', DecimalDigits);
        }
    }
}
