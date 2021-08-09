using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCICompressor
{
	class ArithmeticEncoderPreviousImplementation
	{

		

		public string Encode(string input)
		{
			double lowerBound, upperBound;
			SortedDictionary<char, double> frequencies = generateFrequencies(input);
			string[] frequencyScale = createFrequencyScale(frequencies);
			foreach(string s in frequencyScale)
			{
				Console.WriteLine(s);
			}
			GenerateLowerAndUpperBounds(frequencyScale, input, out lowerBound, out upperBound);

			return GenerateCode(lowerBound, upperBound);
		}

		private SortedDictionary<char, double> generateFrequencies(string input)
		{
			SortedDictionary<char, ulong> quantities = new SortedDictionary<char, ulong>();
			SortedDictionary<char, double> frequencies = new SortedDictionary<char, double>();
			//foreach character in the string, check if it is in the dictionary.
			//if it is, increment its quantity.
			//if it is not, add it to the dictionary with a quantity of 1.
			foreach (char ch in input)
			{
				if (quantities.ContainsKey(ch))
				{
					quantities[ch] = quantities[ch] + 1;
				}

				else
				{
					quantities.Add(ch, 1);
				}
			}

			foreach (KeyValuePair<char, ulong> pair in quantities)
			{
				frequencies.Add(pair.Key, ((double)pair.Value / input.Length));
			}

			return frequencies;
		}
		private string[] createFrequencyScale(SortedDictionary<char, double> frequencies)
		{
			string[] frequencyScale = new string[(frequencies.Count * 2) + 1];
			double totalAmountOfFrequencies = 0;
			foreach (KeyValuePair<char, double> pair in frequencies)
			{
				totalAmountOfFrequencies += pair.Value;
			}

			frequencyScale[0] = "0";
			frequencyScale[1] = frequencies.Keys.ElementAt(0).ToString();
			frequencyScale[2] = frequencies.Values.ElementAt(0).ToString();

			for (int i = 3; i < frequencyScale.Length; i += 2)
			{
				int scaleToQuantityIndex = (i - 1) / 2;
				double  prevSumOfFrequencies = double.Parse(frequencyScale[i - 1]);
				frequencyScale[i + 1] = (frequencies[frequencyScale[i][0]] + prevSumOfFrequencies).ToString();
			}
			//quantityScale[quantityScale.Length - 1] = uint.MaxValue.ToString();
			return frequencyScale;
		}
		private void GenerateLowerAndUpperBounds(string[] quantityScale, string input, out double lowerBound, out double upperBound)
		{
			string[] alteredQuantityScale = (string[])quantityScale.Clone();
			string[] signs = Utils.RemoveEntriesWithLengthAbove1(quantityScale);


			//These are temporary values just so the file will compile.
			lowerBound = 0;
			upperBound = double.Parse(quantityScale.Last());

			for (int i = 0; i < input.Length; i++)
			{
				string currentSign = input[i].ToString();

				int signIndex = Utils.LinearSearch<string>(signs, currentSign);

				//modify the sign via the function f(n) -> 2n+1
				signIndex = (signIndex * 2) + 1;
				Console.WriteLine($"lower: {lowerBound}\tupper: {upperBound}\tdelta:{upperBound - lowerBound}");
				//assign lower and upper bounds.
				//Console.WriteLine($"sign - 1 {alteredQuantityScale[signIndex - 1]}\t sign + 1: {alteredQuantityScale[signIndex + 1]}");
				lowerBound = ulong.Parse(alteredQuantityScale[signIndex - 1]);
				upperBound = ulong.Parse(alteredQuantityScale[signIndex + 1]);


				//Thread.Sleep(500);
				alteredQuantityScale = Utils.NormalizeDoubleValues(quantityScale, alteredQuantityScale, lowerBound, upperBound);

			}
		}

		private string GenerateCode(double lowerBound, double upperBound)
		{
			string code = string.Empty;
			ulong midValue = 0, leftPointer = 0, rightPointer = 1;

			while (!(leftPointer > lowerBound && leftPointer < upperBound && rightPointer > lowerBound && rightPointer < upperBound))
			{
				midValue = (leftPointer + rightPointer) / 2;

				//Handle cases where mid is not in between lower and upper bounds.
				if (midValue < lowerBound)
				{
					leftPointer = midValue;
					code += "1";
				}

				else if (midValue > upperBound)
				{
					rightPointer = midValue;
					code += "0";
				}

				//Handle cases where mid is between lower and upper bounds.

				else if (midValue >= lowerBound && midValue <= upperBound)
				{
					if (leftPointer < lowerBound)
					{
						leftPointer = midValue;
						code += "1";
					}

					else if (rightPointer > upperBound)
					{
						Console.WriteLine("in here!");
						rightPointer = midValue;
						code += "0";
					}

					else if (leftPointer == lowerBound || rightPointer == upperBound || leftPointer == rightPointer)
					{
						break;
					}
				}

				Console.WriteLine($"low: {lowerBound}    left: {leftPointer}    mid: {midValue}     right: {rightPointer}    upper: {upperBound}");
				//Thread.Sleep(500);
			}
			return code;
		}
	}
}
