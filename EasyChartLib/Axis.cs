using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib
{
    internal class Axis
    {
        public decimal MinValue { get; private set; }
        public decimal MaxValue { get; private set; }

        public Axis(decimal minValue, decimal maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public Axis(IEnumerable<float> relevantValues)
        {
            var minValue = relevantValues.Min();
            var maxValue = relevantValues.Max();
            var gap = maxValue - minValue;
            if (gap == 0)
            {
                MinValue = (decimal)(minValue * 0.9f);
                MaxValue = (decimal)(maxValue * 1.1f);
            }
            else
            {
                MinValue = (decimal)(minValue - gap * 0.1f);
                MaxValue = (decimal)(maxValue + gap * 0.1f);
            }

            if (MinValue < 0 && minValue >= 0) MinValue = 0;
        }

        public Axis (float relevantValue)
            : this (new [] { relevantValue })
        {
        }


        public float GetValueInPecentage(float value, bool isInverted = false)
        {
            var minValue = (float)MinValue;
            var maxValue = (float)MaxValue;

            float percentage = (value - minValue) / (maxValue - minValue) * 100;

            if (isInverted) percentage = 100 - percentage;

            return percentage;
        }
    }
}