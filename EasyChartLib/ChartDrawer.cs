﻿using EasyChartLib.PercentageGraphics;
using System;
using System.Drawing;

namespace EasyChartLib
{
    public class ChartDrawer
    {
        public enum EDirection
        {
            BottomToTop,
            TopToBottom,
            LeftToRight,
            RightToLeft,
        }

        private readonly PercentGraphics _drawingArea;
        private readonly Axis _axis;
        private readonly EDirection _direction;

        public ChartDrawer(PercentGraphics drawingArea, Axis axis, EDirection direction = EDirection.BottomToTop)
        {
            _drawingArea = drawingArea;
            _axis = axis;
            _direction = direction;
        }

        public void FillChartRange(Brush brush, float? fromValue, float? toValue)
        {
            FillChartColumnArea(brush, fromValue, toValue, 100);
        }


        public void FillChartColumn(Brush brush, float value, float columnSize = 50)
        {
            FillChartColumnArea(brush, 0, value, columnSize);
        }


        public void DrawLevelLine(Pen pen, float value, float lineSize = 100)
        {
            var valuePercentage = GetValueInPecentage(value, 0);

            //crop:
            if (valuePercentage < 0 || valuePercentage > 100) return;

            var columnMarginSize = 100 - lineSize;
            DrawRotatingLine(pen, columnMarginSize / 2, valuePercentage, 100 - columnMarginSize / 2, valuePercentage);
        }
        public void DrawLevelLine(Pen pen, float value, float offset, float lineSize)
        {
            var valuePercentage = GetValueInPecentage(value, 0);

            //crop:
            if (valuePercentage < 0 || valuePercentage > 100) return;

            DrawRotatingLine(pen, offset, valuePercentage, offset + lineSize, valuePercentage);
        }

        internal void DrawAxis(Pen tickPen, Font font, Brush textColor)
        {
            var tickSize = _axis.TickSize;
            var minValue = (float)Math.Floor(_axis.MinValue / tickSize) * tickSize;
            var maxValue = (float)Math.Ceiling(_axis.MaxValue / tickSize) * tickSize;

            for (float tick = minValue; tick < maxValue; tick += tickSize)
            {
                var percentTick = GetValueInPecentage(tick, 0);

                //crop:
                if (tick < minValue || tick > maxValue) continue;

                DrawLevelLine(tickPen, tick, 75, 25);

                var alignment = new Alignment { Horizontal = HorizontalAlignment.LeftToPoint, Vertical = VerticalAlignment.CenteredToPoint };
                _drawingArea.DrawString(tick.ToString(), font, textColor, new PointF(75, percentTick), alignment);
            }
        }

        private void FillChartColumnArea(Brush brush, float? fromValue, float? toValue, float columnSize)
        {
            var fromPercentage = GetValueInPecentage(fromValue, 0);
            var toPercentage = GetValueInPecentage(toValue, 100);

            var minPercentage = Math.Min(fromPercentage, toPercentage);
            var maxPercentage = Math.Max(fromPercentage, toPercentage);

            //crop:
            if (maxPercentage > 100) maxPercentage = 100;
            if (minPercentage < 0) minPercentage = 0;
            if (minPercentage == 0 && maxPercentage == 0) return;
            if (minPercentage == 100 && maxPercentage == 100) return;

            var columnMarginSize = 100 - columnSize;
            FillRotatingRectangle(brush, columnMarginSize / 2, minPercentage, 100 - columnMarginSize, maxPercentage - minPercentage);
        }


        private void FillRotatingRectangle(Brush brush, float x, float y, float width, float height)
        {
            if (IsVertical())
            {
                _drawingArea.FillRectange(brush, x, y, width, height);
            }
            else
            {
                _drawingArea.FillRectange(brush, y, x, height, width);
            }
        }

        private void DrawRotatingLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (IsVertical())
            {
                _drawingArea.DrawLine(pen, x1, y1, x2, y2);
            }
            else
            {
                _drawingArea.DrawLine(pen, y1, x1, y2, x2);
            }
        }



        private float GetValueInPecentage(float? value, float nullPercentage)
        {

            var isInverted = IsInverted();

            float percentage;
            if (value == null)
                percentage = nullPercentage;
            else
                percentage = (value.Value - _axis.MinValue) / (_axis.MaxValue - _axis.MinValue) * 100;

            if (isInverted) percentage = 100 - percentage;

            return percentage;
        }


        //private float GetScale()
        //{
        //    var isVertical = IsVertical();
        //    return isVertical ? _drawingArea.ScaleHeight : _drawingArea.ScaleWidth;
        //}

        private bool IsVertical()
        {
            if (_direction == EDirection.BottomToTop) return true;
            if (_direction == EDirection.TopToBottom) return true;
            return false;
        }

        private bool IsInverted()
        {
            if (_direction == EDirection.BottomToTop) return true;
            if (_direction == EDirection.RightToLeft) return true;
            return false;
        }
    }
}
