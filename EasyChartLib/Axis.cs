using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib
{
    internal class Axis
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public Axis(IEnumerable<float> relevantValues)
        {
            var minValue = relevantValues.Min();
            var maxValue = relevantValues.Max();
            var gap = maxValue - minValue;
            MinValue = minValue - gap * 0.1f;
            MaxValue = maxValue + gap * 0.1f;

            if (MinValue < 0 && minValue >= 0) MinValue = 0;
        }

        public Axis (float relevantValue)
            : this (new [] { relevantValue })
        {
        }


        public float GetValueInPecentage(float value, bool isInverted = false)
        {
            float percentage = (value - MinValue) / (MaxValue - MinValue) * 100;

            if (isInverted) percentage = 100 - percentage;

            return percentage;
        }
    }
}