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
		string input;
		public ArithmeticEncoder()
		{
		}


		public static void Main(String[] args)
		{
			ArithmeticEncoder encoder = new ArithmeticEncoder();

			Console.WriteLine(encoder.Encode("ABCBBCCCDAKKKHJGYTHHFJURWSFAACAA"));
		}
		public string Encode(string input)
		{
			string[] frequencyScale;
			SortedDictionary<char, ulong> frequencies = new SortedDictionary<char, ulong>();
			ulong lowerBound, upperBound;
			
			string code = "";

			frequencies = GenerateQuantities(input);
			Console.WriteLine("finished generating frequencies.");

			frequencyScale = scaleQuantitiesBasedOnMaximumCapacity(frequencies);
			Console.WriteLine("finished generating frequency scale");
			
			GenerateLowerAndUpperBounds(frequencyScale, input, out lowerBound, out upperBound);
			Console.WriteLine("finished generating bounds");

			code = alternativeGenerateCode(lowerBound, upperBound);
			Console.WriteLine("Finished generating code");
			return code;
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

			foreach (string val in quantityScale)
			{
				Console.WriteLine(val);
			}



			return quantityScale;
		}

		//Generate the frequency scale. The scale an array of strings,
		//where for every i%2==0, array[i] is a sign,
		//otherwise, array[i] is a sum of frequencies up to it.
		private string[] GenerateFrequencyScale(SortedDictionary<char, ulong> frequencies)
		{
			string[] frequencyScale=new string[frequencies.Count*2 +1];


			//Initialize array entries at the first 3 indecies to make life easier.
			//This assumes that there will be at least 3 signs, which is guranteed because of how
			//the image formats this software will work with are composed.

			frequencyScale[0] = "0";
			frequencyScale[1] = frequencies.Keys.ElementAt(0).ToString();
			frequencyScale[2] = frequencies.Values.ElementAt(0).ToString();

			for(int i=3; i<frequencyScale.Length; i+=2)
			{
				int scaleToFrequencyIndex = (i - 1) / 2;
				frequencyScale[i] = frequencies.Keys.ElementAt(scaleToFrequencyIndex).ToString();
				ulong prevSumOfFrequencies = ulong.Parse(frequencyScale[i - 1]);

				frequencyScale[i + 1] = (frequencies.Values.ElementAt(scaleToFrequencyIndex) + prevSumOfFrequencies).ToString();
			}

			foreach(string val in frequencyScale)
			{
				Console.WriteLine(val);
			}
			return frequencyScale;	
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
				int signIndex = Array.BinarySearch(signs, currentSign);

				//modify the sign via the function f(n) -> 2n+1
				//if index is 0, account for that by adding an additional 1.
				signIndex = (signIndex * 2) + 1;



				//foreach(string val in quantityScale)
				//{
				//	Console.WriteLine(val);
				//}


				//assign lower and upper bounds.
				lowerBound = ulong.Parse(alteredFrequencyScale[signIndex - 1]);
				upperBound = ulong.Parse(alteredFrequencyScale[signIndex + 1]);

				//Console.WriteLine($"lower: {lowerBound}\tupper: {upperBound}\tdelta:{upperBound - lowerBound}");
				//Thread.Sleep(500);
				alteredFrequencyScale = Utils.NormalizeValues(quantityScale, alteredFrequencyScale, lowerBound, upperBound);
				
			}


		}

		private string alternativeGenerateCode(ulong lowerBound, ulong upperBound)
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
				}

				Console.WriteLine($"low: {lowerBound}    left: {leftPointer}    mid: {midValue}     right: {rightPointer}    upper: {upperBound}");
				Thread.Sleep(500);			
					}
			return code;
		}
		private string altGenerateCode(ulong valueToSearch) 
		{
			string code = string.Empty;
			ulong leftPointer = 0, rightPointer = uint.MaxValue, midValue = 0;
			while(leftPointer != valueToSearch && rightPointer != valueToSearch && midValue != valueToSearch)
			{
				midValue = (leftPointer + rightPointer) / 2;
				
				if (valueToSearch < midValue)
				{
					code += "0";
					rightPointer = midValue;
				}

				else if (valueToSearch > midValue)
				{
					code += "1";
					leftPointer = midValue;
				}

				Console.WriteLine($"value: {valueToSearch}\tleft pointer: {leftPointer}\tmid value: {midValue}\tright pointer: {rightPointer}");
				Thread.Sleep(100);

			}

			return code;
		}

		private string GenerateCode(ulong lowerBound, ulong upperBound)
		{

			//This is basically a binary search.

			ulong leftPointer = 0, rightPointer = uint.MaxValue;
			bool inside = false;

			string code = "";
			while(!inside)
			{
				ulong midPoint = (leftPointer + rightPointer) / 2;
				
				//This means the range [leftPointer, rightPointer] is inside [lowerBound, upperBound].
				if (leftPointer > lowerBound && leftPointer < upperBound && rightPointer > lowerBound && rightPointer < upperBound)
				{
					inside = true;
					break;
				}

				//Not sure what this is for.
				//else if (leftPointer < lowerBound && midPoint > upperBound)
				//{
				//	rightPointer = midPoint;
				//	code += "0";
				//}

				else if (rightPointer > upperBound && midPoint < lowerBound)
				{
					leftPointer = midPoint;
					code += "1";
				}

				//if this section is reached, lowerBound < midPoint < upperBound.

				else if (!(leftPointer > lowerBound && leftPointer < upperBound))
				{
					code += "1";
					leftPointer = midPoint;
				}

				else if (!(rightPointer > lowerBound && rightPointer < upperBound))
				{
					code += "0";
					rightPointer = midPoint;
				}

				else { inside = true; }

				//Console.WriteLine($"lp {leftPointer}   rp {rightPointer}   mid {midPoint}   upper {upperBound}   lower {lowerBound}");
				//Thread.Sleep(500);

			}
			return code;
		}	
	}
}
