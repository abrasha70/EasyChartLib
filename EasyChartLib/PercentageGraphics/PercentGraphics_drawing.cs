using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib.PercentageGraphics
{
    internal partial class PercentGraphics
    {


        public SizeF MeasureString(string text, Font font)
        {
            var size = _gfx.MeasureString(text, font);
            var relSize = GetRelativeSize(size);
            return relSize;
        }

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            var actualPoint = GetActualPoint(point);
            _gfx.DrawString(text, font, brush, actualPoint);
        }

        public void DrawString(string text, Font font, Brush brush, PointF point, Alignment alignment)
        {
            var adjustedPoint = new PointF(point.X, point.Y);

            //adjust alignment:
            var textSize = MeasureString(text, font);
            if (alignment.Horizontal == HorizontalAlignment.LeftToPoint) adjustedPoint.X -= textSize.Width;
            if (alignment.Horizontal == HorizontalAlignment.CenteredToPoint) adjustedPoint.X -= textSize.Width / 2;
            if (alignment.Vertical == VerticalAlignment.AbovePoint) adjustedPoint.Y -= textSize.Height;
            if (alignment.Vertical == VerticalAlignment.CenteredToPoint) adjustedPoint.Y -= textSize.Height / 2;

            DrawString(text, font, brush, adjustedPoint);
        }


        public void FillRectange(Brush brush, RectangleF rect)
        {
            var actualRect = GetActualRect(rect);
            _gfx.FillRectangle(brush, actualRect);
        }
        public void FillRectange(Brush brush, float x, float y, float width, float height)
        {
            var rect = new RectangleF(x, y, width, height);
            FillRectange(brush, rect);
        }

        public void DrawPoint(Pen pen, float x, float y)
        {
            var actualRadius = pen.Width;
            var actualPoint = GetActualPoint(x, y);
            var actualRect = new RectangleF(actualPoint.X - actualRadius, actualPoint.Y - actualRadius, actualRadius, actualRadius);

            _gfx.FillEllipse(pen.Brush, actualRect);
        }


        public void DrawRectangle(Pen pen, RectangleF rect)
        {
            var actualRect = GetActualRect(rect);
            _gfx.DrawRectangle(pen, actualRect.X, actualRect.Y, actualRect.Width, actualRect.Height);
        }

        public void DrawLine(Pen pen, PointF p1, PointF p2)
        {
            var actualP1 = GetActualPoint(p1);
            var actualP2 = GetActualPoint(p2);
            _gfx.DrawLine(pen, actualP1, actualP2);
        }
        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            var p1 = new PointF(x1, y1);
            var p2 = new PointF(x2, y2);
            DrawLine(pen, p1, p2);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            var rect = new RectangleF(x, y, width, height);
            DrawRectangle(pen, rect);
        }

        public void DrawBorder(Pen pen)
        {
            DrawRectangle(pen, 0, 0, 100, 100);
        }


    }
}
