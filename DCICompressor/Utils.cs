using System;
using System.Collections.Generic;
namespace DCICompressor
{
	class Utils
	{

		public static int BinarySearch<T>(T[] array, T value) where T:IComparable
		{
			int leftPointer=0, rightPointer=array.Length-1;

			while (leftPointer != rightPointer )
			{
				int midPoint = (rightPointer + leftPointer) / 2;

				int comparisonResult = value.CompareTo(array[midPoint]);

				if (comparisonResult==0)
				{

					return midPoint;
				}

				else if (comparisonResult>0)
				{
					leftPointer = midPoint;
				}

				else
				{
					rightPointer = midPoint;
				}

				
			}
			return -1;
		}

		public static int LinearSearch<T>(T[] array, T value) where T : IComparable
		{
			for(int i = 0; i< array.Length; i++)
			{
				if (array[i].CompareTo(value) == 0)
					return i;
			}

			return -1;
		}

		public static string[] RemoveEntriesWithLengthAbove1(string[] array)
		{
			List<String> tempListToStoreEntries = new List<String>();

			for (int i = 1; i < array.Length; i += 2)
			{
				{
					tempListToStoreEntries.Add(array[i]);
				}
			}

			foreach (string s in tempListToStoreEntries)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine();
			return tempListToStoreEntries.ToArray();
		}

		public static ulong NormalizeValue(ulong valueToScale, ulong bottomValue, ulong upperValue, double scaleFactor) 
		{

			ulong delta = upperValue - bottomValue;
			//Console.WriteLine($"scale factor: {scaleFactor} " + "delta * scale factor: " + (ulong) (delta * scaleFactor)); 
			ulong scaledValue = (uint) (bottomValue + ((double)delta * scaleFactor));
			//Console.WriteLine($"delta: {delta}\tscaleFactor: {scaleFactor}\tscaledValue: {scaledValue}");
			return scaledValue;
		}

		public static string[] NormalizeValues(string[] originalScale, string[] valuesToScale, ulong bottomValue, ulong UpperValue)
		{
			string[] scaledValuesArray = new string[valuesToScale.Length];

			scaledValuesArray[0] = bottomValue.ToString();
			for (int i = 1; i < scaledValuesArray.Length; i++)
			{
				ulong value;
				if (ulong.TryParse(valuesToScale[i], out value))
				{
					double scaleFactor = (double)((ulong.Parse(originalScale[i]) - ulong.Parse(originalScale[i - 2]))) / uint.MaxValue;
					scaledValuesArray[i] = (NormalizeValue(value, bottomValue, UpperValue, scaleFactor)).ToString();
				}

				else
				{
					scaledValuesArray[i] = valuesToScale[i];
				}
			}
			return scaledValuesArray;
		}
	}
}
