using System;

namespace RtcCalculator
{
	public class HexadecimalRtcValue : IComparable, IEquatable<HexadecimalRtcValue>
	{
		private readonly int value;

		public HexadecimalRtcValue GetLeftMedianValueBetween(HexadecimalRtcValue otherValue)
		{
			decimal median = (Value + otherValue.Value) / 2;
			
			var resultingValue = (int) Math.Round(median, MidpointRounding.ToZero);

			return new HexadecimalRtcValue(resultingValue);
		}
		
		public HexadecimalRtcValue GetRightMedianValueBetween(HexadecimalRtcValue otherValue)
		{
			decimal median = (Value + otherValue.Value) / 2;
			
			var resultingValue = (int) Math.Round(median, MidpointRounding.AwayFromZero);

			return new HexadecimalRtcValue(resultingValue);
		}

		public int Value => value;

		public HexadecimalRtcValue(int value)
		{
			if (value is < 0x00 or > 0xff)
				throw new ArgumentOutOfRangeException(nameof(value), "Invalid RTC checksum value!");

			this.value = value;
		}

		public int CompareTo(object? obj)
		{
			if (obj is not HexadecimalRtcValue otherValue)
				throw new ArgumentException($"Object should be of type {nameof(HexadecimalRtcValue)}", nameof(obj));

			return Value.CompareTo(otherValue.Value);
		}

		public bool Equals(HexadecimalRtcValue? other)
		{
			return other != null && Value.Equals(other.Value);
		}

		public int Diff(HexadecimalRtcValue other)
		{
			return other.Value > Value ? other.Value - Value : Value - other.Value;
		}
	}
}