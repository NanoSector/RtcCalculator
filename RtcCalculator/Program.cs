using System;

namespace RtcCalculator
{
	class Program
	{
		static void Main(string[] args)
		{
			HexadecimalRtcValue? leftBound = null;
			HexadecimalRtcValue? rightBound = null;
			var tryingLeftSide = true;
			
			while (leftBound == null)
			{
				Console.Write("Please input the left bound as a decimal value (or press enter to accept 0x00/0): ");
				string? line = Console.ReadLine();

				if (line == null || line.Trim().Length == 0)
				{
					leftBound = new HexadecimalRtcValue(0x00);
					break;
				}

				try
				{
					int value = int.Parse(line);
					leftBound = new HexadecimalRtcValue(value);
				}
				catch (FormatException)
				{
					Console.WriteLine("Please input a valid number.");
				}
				catch (ArgumentOutOfRangeException)
				{
					Console.WriteLine("The value should be between 0 (00) and 255 (ff), try again.");
				}
			}
			
			while (rightBound == null)
			{
				Console.Write("Please input the right bound as a decimal value (or press enter to accept 0xff/255): ");
				string? line = Console.ReadLine();

				if (line == null || line.Trim().Length == 0)
				{
					rightBound = new HexadecimalRtcValue(0xff);
					break;
				}

				try
				{
					int value = int.Parse(line);
					rightBound = new HexadecimalRtcValue(value);
					
					if (leftBound.CompareTo(rightBound) >= 0)
					{
						Console.WriteLine("Right bound should be greater than left bound.");
						rightBound = null;
					}
				}
				catch (FormatException)
				{
					Console.WriteLine("Please input a valid number.");
				}
				catch (ArgumentOutOfRangeException)
				{
					Console.WriteLine("The value should be between 0 (00) and 255 (ff), try again.");
				}
			}
			
			Console.WriteLine($"Got left bound: {leftBound.Value:X2}");
			Console.WriteLine($"Got right bound: {rightBound.Value:X2}");
			Console.WriteLine("Ready to go!");

			var lastPressedKey = new ConsoleKeyInfo();

			while (lastPressedKey.Key != ConsoleKey.Q)
			{
				HexadecimalRtcValue leftMedian = leftBound.GetLeftMedianValueBetween(rightBound);
				HexadecimalRtcValue rightMedian = leftBound.GetLeftMedianValueBetween(rightBound);
				
				if (lastPressedKey.Key == ConsoleKey.Y)
				{
					bool solved = tryingLeftSide 
						? leftBound.Diff(leftMedian) == 0
						: rightMedian.Diff(rightBound) == 0;
					
					if (solved)
					{
						Console.WriteLine("There's your solution!");
						break;
					}

					if (tryingLeftSide)
						rightBound = leftMedian;
					else
						leftBound = rightMedian;

					tryingLeftSide = true;
					lastPressedKey = new ConsoleKeyInfo();
					continue;
				}
				
				if (lastPressedKey.Key == ConsoleKey.N)
				{
					// Resort to giving the previous range when we have tried both sides, or when the overlapping range has a difference of one (e.g. 00-01)
					if (!tryingLeftSide || leftBound.Diff(rightBound) <= 1)
					{
						Console.WriteLine($"FINAL JUDGEMENT: Revert back to range rtcfx_exclude={leftBound.Value:X2}-{rightBound.Value:X2}");
						break;
					}

					tryingLeftSide = false;
				}


				if (tryingLeftSide)
				{
					Console.Write($"Try the following range: rtcfx_exclude={leftBound.Value:X2}-{leftMedian.Value:X2}");
				}
				else
				{
					Console.Write($"Try the following range: rtcfx_exclude={rightMedian.Value:X2}-{rightBound.Value:X2}");
				}
				
				Console.Write(" - Does this fix your issue? (Y/N) ");
				lastPressedKey = Console.ReadKey();
				Console.WriteLine();
			}

			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}
	}
}