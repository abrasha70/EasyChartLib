using EasyChartLib.PercentageGraphics;
using System;
using System.Drawing;

namespace EasyChartLib
{


    internal class RankChartDrawer
    {
        public enum EDirection
        {
            BottomToTop,
            TopToBottom,
            LeftToRight,
            RightToLeft,
        }

        private readonly PercentGraphics _drawingArea;
        private readonly Axis _valuesAxis;
        private readonly EDirection _direction;

        public RankChartDrawer(PercentGraphics drawingArea, Axis valuesAxis, EDirection direction = EDirection.BottomToTop)
        {
            _drawingArea = drawingArea;
            _valuesAxis = valuesAxis;
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
            var valuePercentage = _valuesAxis.GetValueInPecentage(value, IsInverted());

            //crop:
            if (valuePercentage < 0 || valuePercentage > 100) return;

            var columnMarginSize = 100 - lineSize;
            DrawRotatingLine(pen, columnMarginSize / 2, valuePercentage, 100 - columnMarginSize / 2, valuePercentage);
        }


        private void FillChartColumnArea(Brush brush, float? fromValue, float? toValue, float columnSize)
        {
            var fromPercentage = _valuesAxis.GetValueInPecentage(fromValue ?? 0, IsInverted());
            var toPercentage = _valuesAxis.GetValueInPecentage(toValue ?? 100, IsInverted());

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


        //private void DrawRotatingString(string text, Font font, Brush brush, PointF point, Alignment alignment)
        //{
        //    if (IsVertical())
        //    {
        //        _drawingArea.DrawString(text, font, brush, point, alignment);
        //    }
        //    else
        //    {
        //        var rotatedPoint = new PointF(point.Y, point.X);
        //        _drawingArea.DrawString(text, font, brush, rotatedPoint, alignment);
        //    }
        //}
        //private void DrawRotatingString(string text, Font font, Brush brush, PointF point)
        //{
        //    if (IsVertical())
        //    {
        //        _drawingArea.DrawString(text, font, brush, point);
        //    }
        //    else
        //    {
        //        var rotatedPoint = new PointF(point.Y, point.X);
        //        _drawingArea.DrawString(text, font, brush, rotatedPoint);
        //    }
        //}




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
