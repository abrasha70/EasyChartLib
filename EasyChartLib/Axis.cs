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

        public Axis(ChartSettings settings, List<SingleCategoryData> categories, SizeF digitSizeInPercentage, bool isVerticalAxis)
        {
            _settings = settings;

            var allValues = GetRelevantValues(categories, settings.AxisMode);
            var minValue = allValues.Min();
            var maxValue = allValues.Max();
            var gap = maxValue - minValue;
            MinValue = minValue - gap * 0.1f;
            MaxValue = maxValue + gap * 0.1f;

            if (MinValue < 0 && minValue >= 0) MinValue = 0;

            var maxDigits = allValues.Max(value => AutoRound(value).ToString().Length);
            var textLengthPercentage = isVerticalAxis ? digitSizeInPercentage.Height : digitSizeInPercentage.Width * maxDigits;
            SpaceNeeded = isVerticalAxis ? digitSizeInPercentage.Width * maxDigits : digitSizeInPercentage.Height;
            TickLength = CalcTickSize(MinValue, MaxValue, textLengthPercentage);
            DecimalDigits = GetDecimalDigits(TickLength);
        }

        public Axis (ChartSettings settings, SingleCategoryData category, SizeF digitSizeInPercentage, bool isVerticalAxis)
            : this (settings, new List<SingleCategoryData> { category }, digitSizeInPercentage, isVerticalAxis)
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
            var power10 = Math.Round(Math.Log10(value));
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



        private List<float> GetRelevantValues(List<SingleCategoryData> categories, EAxisMode axisMode)
        {
            switch (axisMode)
            {
                case EAxisMode.All:
                    return GetAllValues(categories);
                case EAxisMode.Focused:
                    return GetFocusedValues(categories);
                case EAxisMode.FocusedAndAround:
                    return GetFocusedAndAroundValues(categories);
                case EAxisMode.FocusedAndNearby:
                    return GetFocusedAndNearbyValues(categories);
                default:
                    return null;
            }
        }

        private List<float> GetAllValues(List<SingleCategoryData> categories)
        {
            var measured = categories.Select(category => category.Measured);
            var targets = categories.Select(category => category.Target);
            var rankMins = categories.SelectMany(category => category.GetRanksAsRanges().Select(rank => rank.FromValue));
            var rankMaxs = categories.SelectMany(category => category.GetRanksAsRanges().Select(rank => rank.ToValue));

            var unified = measured.Union(targets).Union(rankMins).Union(rankMaxs);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private List<float> GetFocusedValues(List<SingleCategoryData> categories)
        {
            var measuredValues = categories.Select(category => category.Measured);
            var targetValues = categories.Select(category => category.Target);

            var unified = measuredValues.Union(targetValues);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private List<float> GetFocusedAndAroundValues(List<SingleCategoryData> categories)
        {
            var measured = categories.Select(category => category.Measured);
            var targets = categories.Select(category => category.Target);
            var surroundRanks = categories.SelectMany(
                category => category.GetRanksAsRanges().Where(
                    rank =>
                    IsBetween(category.Measured, rank.FromValue, rank.ToValue) ||
                    IsBetween(category.Target, rank.FromValue, rank.ToValue)
                    )
                );
            var rankMins = surroundRanks.Select(rank => rank.FromValue);
            var rankMaxs = surroundRanks.Select(rank => rank.ToValue);

            var unified = measured.Union(targets).Union(rankMins).Union(rankMaxs);
            var result = unified.Where(item => item.HasValue).Select(item => item.Value);

            return result.ToList();
        }

        private List<float> GetFocusedAndNearbyValues(List<SingleCategoryData> categories)
        {
            var allValues = new List<float?>();

            foreach (var category in categories)
            {
                allValues.Add(category.Measured);
                allValues.Add(category.Target);
                var ranks = category.GetRanksAsRanges();
                for (int rankIndex = 0; rankIndex < ranks.Count; rankIndex++)
                {
                    var rank = ranks[rankIndex];
                    var prev = rankIndex > 0 ? ranks[rankIndex - 1] : null;
                    var next = rankIndex < ranks.Count - 1 ? ranks[rankIndex + 1] : null;

                    var overlapCurrent = IsBetween(category.Measured, rank.FromValue, rank.ToValue) || IsBetween(category.Target, rank.FromValue, rank.ToValue);
                    var overlapPrev = prev != null && (IsBetween(category.Measured, prev.FromValue, prev.ToValue) || IsBetween(category.Target, prev.FromValue, prev.ToValue));
                    var overlapNext = next != null && (IsBetween(category.Measured, next.FromValue, next.ToValue) || IsBetween(category.Target, next.FromValue, next.ToValue));

                    if (overlapCurrent || overlapPrev || overlapNext)
                    {
                        allValues.Add(rank.FromValue);
                        allValues.Add(rank.ToValue);
                    }
                }
            }
            var result = allValues.Where(item => item.HasValue).Select(item => item.Value).Distinct();

            return result.ToList();
        }


        private bool IsBetween(float? testedValue, float? minValue, float? maxValue)
        {
            if (!testedValue.HasValue) return false;
            if (maxValue.HasValue && testedValue > maxValue) return false;
            if (minValue.HasValue && testedValue < minValue) return false;
            return true;
        }

    }
}