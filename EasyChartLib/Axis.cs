using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib
{
    internal class Axis
    {
        private readonly ChartSettings _settings;

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
        public float TickLength { get; private set; }
        public int DecimalDigits { get; private set; }
        public float SpaceNeeded { get; private set; }

        public Axis(ChartSettings settings, IEnumerable<float> relevantValues, SizeF digitSizeInPercentage, bool isVerticalAxis)
        {
            _settings = settings;

            var minValue = relevantValues.Min();
            var maxValue = relevantValues.Max();
            var gap = maxValue - minValue;
            MinValue = minValue - gap * 0.1f;
            MaxValue = maxValue + gap * 0.1f;

            if (MinValue < 0 && minValue >= 0) MinValue = 0;

            var maxDigits = relevantValues.Max(value => AutoRound(value).ToString().Length);
            var textLengthPercentage = isVerticalAxis ? digitSizeInPercentage.Height : digitSizeInPercentage.Width * maxDigits;
            SpaceNeeded = isVerticalAxis ? digitSizeInPercentage.Width * maxDigits : digitSizeInPercentage.Height;
            TickLength = CalcTickSize(MinValue, MaxValue, textLengthPercentage);
            DecimalDigits = GetDecimalDigits(TickLength);
        }

        public Axis (ChartSettings settings, float relevantValue, SizeF digitSizeInPercentage, bool isVerticalAxis)
            : this (settings, new [] { relevantValue }, digitSizeInPercentage, isVerticalAxis)
        {
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