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


		public static string[] RemoveEntriesWithLengthAbove1(string[] array)
		{
			List<String> tempListToStoreEntries = new List<String>();

			foreach (string s in array)
			{
				if (s.Length == 1 && s[0].CompareTo('9')>0)
				{
					tempListToStoreEntries.Add(s);
				}
			}
			return tempListToStoreEntries.ToArray();
		}

		public static float NormalizeValue(float valueToScale, float bottomValue, float upperValue, float scaleFactor) 
		{
			float delta = upperValue - bottomValue;
			float scaledValue = bottomValue + (delta * scaleFactor);

			return scaledValue;
		}

		public static string[] NormalizeValues(string[] originalScale, string[] valuesToScale, float bottomValue, float UpperValue)
		{
			string[] scaledValuesArray = new string[valuesToScale.Length];

			scaledValuesArray[0] = bottomValue.ToString();
			for (int i = 1; i < scaledValuesArray.Length; i++)
			{
				float value;
				if (float.TryParse(valuesToScale[i], out value))
				{
					scaledValuesArray[i] = (NormalizeValue(value, bottomValue, UpperValue, float.Parse(originalScale[i]))).ToString();
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
