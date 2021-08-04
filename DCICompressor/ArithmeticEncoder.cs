using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DCICompressor
{
	class ArithmeticEncoder
	{
		string code;
		public ArithmeticEncoder()
		{
		}

		public static void Main(String[] args)
		{
			ArithmeticEncoder encoder = new ArithmeticEncoder();
			ArithmeticDecoder decoder = new ArithmeticDecoder();

			string encode = "AAAABBBBCCCC";
			string code;
			string[] scale;
			encoder.Encode(encode, out code, out scale);

			Console.WriteLine(code);

			Console.WriteLine("Starting decoding.");
			string decoded = decoder.decode(code, scale, encode.Length);

			int HammingDistance = 0;
			for(int i = 0; i < encode.Length; i++)
			{
				if (encode[i] != decoded[i])
					HammingDistance++;
			}

			Console.WriteLine("Hamming distance: " + HammingDistance);
		}
		public void Encode(string input, out string code, out string[] scale)
		{
			SortedDictionary<char, ulong> quantities = new SortedDictionary<char, ulong>();

			quantities = GenerateQuantities(input);
			Console.WriteLine("finished generating quantites.");

			scale = scaleQuantitiesBasedOnMaximumCapacity(quantities);
			Console.WriteLine("finished generating quantity scale");

			code = GenerateBoundsAndCodeWithFinitePrecision(scale);
			Console.WriteLine("Finished generating code");
		}

		//Generate a dictionary containing the frequencies of all characters in the string.
		private SortedDictionary<char, ulong> GenerateQuantities(string input)
		{
			SortedDictionary<char, ulong> quantities = new SortedDictionary<char, ulong>();
			

			//foreach character in the string, check if it is in the dictionary.
			//if it is, increment its' quantity.
			//if it is not, add it to the dictionary with a quantity of 1.
			foreach(char ch in input)
			{
				if(quantities.ContainsKey(ch))
				{
					quantities[ch] = quantities[ch] + 1;
				}

				else
				{
					quantities.Add(ch, 1);
				}
			}

			//Create a new dictionary to store the frequencies,
			//which will be produced from the quantities.
			//Eventually, all quantities should sum to 1.

			//SortedDictionary<char, ulong> frequencies = new SortedDictionary<char, ulong>();
			//int numberOfSigns = input.Length;

			//foreach(KeyValuePair<char,int> pair in quantities)
			//{
			//	frequencies.Add(pair.Key, ((ulong) pair.Value / (ulong) numberOfSigns));
			//}

			//foreach (KeyValuePair<char, ulong> pair in quantities)
			//{
			//	Console.WriteLine($"key: {pair.Key}\tvalue: {pair.Value}");
			//}

			return quantities;
		}
		private string[] scaleQuantitiesBasedOnMaximumCapacity(SortedDictionary<char, ulong> quantities)
		{
			string[] quantityScale = new string[(quantities.Count * 2) + 1];
			ulong maxVal = uint.MaxValue;
			ulong numberOfItems = (ulong) quantities.Count;
			ulong totalAmountOfQuantities = 0;
			foreach(KeyValuePair<char, ulong> pair in quantities)
			{
				totalAmountOfQuantities += pair.Value;
			}

			quantityScale[0] = "0";
			quantityScale[1] = quantities.Keys.ElementAt(0).ToString() ;
			quantityScale[2] =  ((ulong)(maxVal *  ((double)quantities.Values.ElementAt(0) / totalAmountOfQuantities))).ToString();

			for (int i = 3; i < quantityScale.Length-1; i += 2)
			{
				int scaleToFrequencyIndex = (i - 1) / 2;
				quantityScale[i] = quantities.Keys.ElementAt(scaleToFrequencyIndex).ToString();
				ulong prevSumOfFrequencies = ulong.Parse(quantityScale[i - 1]);
				double ratio = (double)quantities.Values.ElementAt(scaleToFrequencyIndex) /(double) totalAmountOfQuantities;
				ulong newCurrentVal = (ulong)(maxVal * (ratio));
				quantityScale[i + 1] = (newCurrentVal+ prevSumOfFrequencies).ToString();
			}
			quantityScale[quantityScale.Length - 1] = uint.MaxValue.ToString();

			//foreach (string val in quantityScale)
			//{
			//	Console.WriteLine(val);
			//}



			return quantityScale;
		}
		private void GenerateLowerAndUpperBounds(string[] quantityScale, string input, out ulong lowerBound, out ulong upperBound)
		{
			string[] alteredFrequencyScale = quantityScale;
			string[] signs = Utils.RemoveEntriesWithLengthAbove1(quantityScale);

			//These are temporary values just so the file will compile.
			lowerBound = 0;
			upperBound = ulong.Parse(quantityScale.Last());

			for(int i=0; i<input.Length; i++)
			{
				string currentSign = input[i].ToString();
				//Console.WriteLine($"Searching for {currentSign} in index {i}:");
				//for (int j=0; j<signs.Length; j++)
				//{
				//	Console.Write($"{signs[j]}, ");
				//}
				int signIndex = Utils.LinearSearch<string>(signs, currentSign);

				//modify the sign via the function f(n) -> 2n+1
				//if index is 0, account for that by adding an additional 1.
				signIndex = (signIndex * 2) + 1;
				//assign lower and upper bounds.
				//Console.WriteLine($"sign - 1 {alteredFrequencyScale[signIndex - 1]}\t sign + 1: {alteredFrequencyScale[signIndex + 1]}");
				lowerBound = ulong.Parse(alteredFrequencyScale[signIndex - 1]);
				upperBound = ulong.Parse(alteredFrequencyScale[signIndex + 1]);

				//Console.WriteLine($"lower: {lowerBound}\tupper: {upperBound}\tdelta:{upperBound - lowerBound}");
				//Thread.Sleep(500);
				alteredFrequencyScale = Utils.NormalizeValues(quantityScale, alteredFrequencyScale, lowerBound, upperBound);
				
			}
		}
		private string GenerateCode(ulong lowerBound, ulong upperBound)
		{
			string code = string.Empty;
			ulong midValue = 0, leftPointer = 0, rightPointer = uint.MaxValue;

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
				Thread.Sleep(500);			
					}
			return code;
		}
		
		private string GenerateBoundsAndCodeWithFinitePrecision(string[] scale)
		{
			string code = string.Empty;
			ulong leftPointer = 0, rightPointer = uint.MaxValue;

			const ulong WHOLE = uint.MaxValue;
			const ulong HALF = WHOLE / 2;
			const ulong QUARTER = WHOLE / 4;

			ulong R = 0;
			for (int i = 2; i < scale.Length; i += 2)
			{
				ulong currentQuantity = ulong.Parse(scale[i]) - ulong.Parse(scale[i - 2]);
				R += currentQuantity;
			}

			int s = 0;
			for (int i = 1; i < scale.Length; i += 2)
			{
				ulong w = rightPointer - leftPointer;
				rightPointer = leftPointer + (uint)Math.Round((double)w * ulong.Parse(scale[i + 1]) / R);
				leftPointer = leftPointer + (uint) Math.Round((double)w * ulong.Parse(scale[i - 1]) / R);

				while( rightPointer < HALF || leftPointer > HALF)
				{
					if ( rightPointer < HALF)
					{
						code += "0" + new string('1', s);
						s = 0;
						leftPointer *= 2;
						rightPointer *= 2;
					}

					else if ( leftPointer > HALF)
					{
						code += "1" + new string('0', s);
						s = 0;
						leftPointer = 2 * (leftPointer - HALF);
						rightPointer = 2 * (rightPointer - HALF);
					}
				}


				while (leftPointer > QUARTER && rightPointer < 3 * QUARTER)
				{
					s += 1;
					leftPointer = 2 * (leftPointer - QUARTER);
					rightPointer = 2 * (rightPointer - QUARTER);
				}

			}
			s += 1;
			if (leftPointer <= QUARTER)
			{
				code += "0" + new string('1', s);
			}

			else
			{
				code += "1" + new string('0', s);
			}
			return code;
		}
	}
}
