using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib.PercentageGraphics
{
    internal partial class PercentGraphics
    {

        public PercentGraphics CreateSubArea(RectangleF rect)
        {
            var actualRect = GetActualRect(rect);
            var actualArea = new PercentGraphics(_bmp, actualRect);
            return actualArea;
        }
        public PercentGraphics CreateSubArea(float x, float y, float width, float height)
        {
            var actualRect = new RectangleF(x, y, width, height);
            return CreateSubArea(actualRect);
        }


        public List<PercentGraphics> HorizontalSplit(float splitSize)
        {
            var parts = new List<PercentGraphics>();
            parts.Add(CreateSubArea(0, 0, splitSize, FullScale));
            parts.Add(CreateSubArea(splitSize, 0, FullScale - splitSize, FullScale));
            return parts;
        }

        public List<PercentGraphics> HorizontalMultiSplit(int count)
        {
            var partWidth = FullScale / count;
            var parts = Enumerable.Range(0, count).Select((index) => CreateSubArea(index * partWidth, 0, partWidth, FullScale));
            return parts.ToList();
        }

        public List<PercentGraphics> HorizontalMultiSplit(List<float?> lengthSizes)
        {
            var resized = AutoResizeLengths(lengthSizes, FullScale);

            var parts = new List<PercentGraphics>();
            var position = 0f;
            foreach (var length in resized)
            {
                var newPart = CreateSubArea(position, 0, length, FullScale);
                position += length;
                parts.Add(newPart);
            }

            return parts;
        }


        public List<PercentGraphics> SplitChunkInDirection(SizeF chunkSize, EDirection direction, out PercentGraphics chunkArea, out PercentGraphics remainingArea)
        {
            var directionObj = new DirectionObj(direction);
            var chunkLength = directionObj.IsVertical ? chunkSize.Height : chunkSize.Width;

            return SplitChunkInDirection(chunkLength, direction, out chunkArea, out remainingArea);
        }

        public List<PercentGraphics> SplitChunkInDirection(float chunkLength, EDirection direction, out PercentGraphics chunkArea, out PercentGraphics remainingArea)
        {
            var directionObj = new DirectionObj(direction);

            var chunkIndex = 0;
            var remainingIndex = 1;

            if (directionObj.IsInverted)
            {
                chunkLength = FullScale - chunkLength;
                chunkIndex = 1;
                remainingIndex = 0;
            }

            var areas = directionObj.IsVertical ? VerticalSplit(chunkLength) : HorizontalSplit(chunkLength);
            chunkArea = areas[chunkIndex];
            remainingArea = areas[remainingIndex];
            return areas;
        }



        public PercentGraphics[,] MatrixSplit(float verticalSplitSize, float horizontalSplitSize)
        {
            var verticalParts = VerticalSplit(verticalSplitSize);
            var top = verticalParts[0];
            var bottom = verticalParts[1];

            var topParts = top.HorizontalSplit(horizontalSplitSize);
            var bottomParts = bottom.HorizontalSplit(horizontalSplitSize);

            var areas = new PercentGraphics[2, 2];
            areas[0, 0] = topParts[0];
            areas[0, 1] = topParts[1];
            areas[1, 0] = bottomParts[0];
            areas[1, 1] = bottomParts[1];
            return areas;
        }

        public List<PercentGraphics> VerticalSplit(float splitSize)
        {
            var parts = new List<PercentGraphics>();
            parts.Add(CreateSubArea(0, 0, FullScale, splitSize));
            parts.Add(CreateSubArea(0, splitSize, FullScale, FullScale - splitSize));
            return parts;
        }

        public List<PercentGraphics> VerticalMultiSplit(int count)
        {
            var partHeight = FullScale / count;
            var parts = Enumerable.Range(0, count).Select((index) => CreateSubArea(0, index * partHeight, FullScale, partHeight));
            return parts.ToList();
        }

        public List<PercentGraphics> VerticalMultiSplit(List<float?> lengthSizes)
        {
            var resized = AutoResizeLengths(lengthSizes, FullScale);

            var parts = new List<PercentGraphics>();
            var position = 0f;
            foreach (var length in resized)
            {
                var newPart = CreateSubArea(0, position, FullScale, length);
                position += length;
                parts.Add(newPart);
            }

            return parts;
        }


        private List<float> AutoResizeLengths(List<float?> lengthSizes, float scaleSize)
        {
            var newItems = lengthSizes.Select(item => item ?? 0).ToList();

            var total = newItems.Sum(length => length);
            var remainedLength = scaleSize - total;
            if (remainedLength < 0) throw new System.OverflowException("Total length sizes exceeded Scale");


            if (remainedLength > 0)
            {
                var autoResizeIndexes = newItems.Select((length, index) => index).Where(index => newItems[index] == 0).ToList();
                if (autoResizeIndexes.Count == 0) throw new System.ArithmeticException("No auto-resize item for remained length");
                var autoResizeLength = remainedLength / autoResizeIndexes.Count;
                foreach (var index in autoResizeIndexes)
                {
                    newItems[index] = autoResizeLength;
                }
            }

            return newItems;
        }




    }
}
