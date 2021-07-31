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

			Console.WriteLine(encoder.Encode("ABCAA"));
		}
		public string Encode(string input)
		{
			string[] frequencyScale;
			SortedDictionary<char, float> frequencies = new SortedDictionary<char, float>();
			float lowerBound, upperBound;
			
			string code = "";

			frequencies = GenerateFrequencies(input);
			Console.WriteLine("finished generating frequencies.");
			
			frequencyScale = GenerateFrequencyScale(frequencies);
			Console.WriteLine("finished generating frequency scale");
			
			GenerateLowerAndUpperBounds(frequencyScale, input, out lowerBound, out upperBound);
			Console.WriteLine("finished generating bounds");

			Console.WriteLine($"upper bound {upperBound}\tlower bound{lowerBound}");
			code = GenerateCode(lowerBound, upperBound );
			Console.WriteLine("Finished generating code");
			return code;
		}



		//Generate a dictionary containing the frequencies of all characters in the string.
		private SortedDictionary<char, float> GenerateFrequencies(string input)
		{
			Dictionary<char, int> quantities = new Dictionary<char, int>();
			

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

			SortedDictionary<char, float> frequencies = new SortedDictionary<char, float>();
			int numberOfSigns = input.Length;

			foreach(KeyValuePair<char,int> pair in quantities)
			{
				frequencies.Add(pair.Key, ((float) pair.Value / (float) numberOfSigns));
			}

			return frequencies;
		}

		//Generate the frequency scale. The scale an array of strings,
		//where for every i%2==0, array[i] is a sign,
		//otherwise, array[i] is a sum of frequencies up to it.
		private string[] GenerateFrequencyScale(SortedDictionary<char, float> frequencies)
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
				float prevSumOfFrequencies = float.Parse(frequencyScale[i - 1]);

				frequencyScale[i + 1] = (frequencies.Values.ElementAt(scaleToFrequencyIndex) + prevSumOfFrequencies).ToString();
			}

			return frequencyScale;	
		}
		private void GenerateLowerAndUpperBounds(string[] frequencyScale, string input, out float lowerBound, out float upperBound)
		{
			string[] alteredFrequencyScale = frequencyScale;
			string[] signs = Utils.RemoveEntriesWithLengthAbove1(frequencyScale);

			//These are temporary values just so the file will compile.
			lowerBound = 0;
			upperBound = 1;

			for(int i=0; i<input.Length; i++)
			{
				string currentSign = input[i].ToString();
				int signIndex = Array.BinarySearch(signs, currentSign);

				//modify the sign via the function f(n) -> 2n+1
				//if index is 0, account for that by adding an additional 1.
				signIndex = (signIndex * 2) + 1;

				//assign lower and upper bounds.
				lowerBound = float.Parse(alteredFrequencyScale[signIndex - 1]);
				upperBound = float.Parse(alteredFrequencyScale[signIndex + 1]);
				//Console.WriteLine($"upper bound {upperBound}\tlower bound {lowerBound}");
				//Thread.Sleep(500);
				alteredFrequencyScale = Utils.NormalizeValues(frequencyScale, alteredFrequencyScale, lowerBound, upperBound);
				
			}


		}

		private string altGenerateCode(float valueToSearch)
		{
			string code = string.Empty;
			float leftPointer = 0, rightPointer = 1;
			while(leftPointer != valueToSearch && rightPointer != valueToSearch)
			{
				float midValue = (leftPointer + rightPointer) / 2f;
				
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

				Console.WriteLine($"left pointer: {leftPointer}\tmid value: {midValue}\tright pointer: {rightPointer}");
				Thread.Sleep(500);

			}

			return code;
		}

		private string GenerateCode(float lowerBound, float upperBound)
		{

			//This is basically a binary search.

			float leftPointer = 0, rightPointer = 1;
			bool inside = false;

			string code = "";
			while(!inside)
			{
				float midPoint = (leftPointer + rightPointer) / 2f;

				if (leftPointer > lowerBound && leftPointer < upperBound && rightPointer > lowerBound && rightPointer < upperBound)
				{
					inside = true;
					break;
				}

				else if (leftPointer < lowerBound && midPoint > upperBound)
				{
					rightPointer = midPoint;
					code += "0";
				}

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
