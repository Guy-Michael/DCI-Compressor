using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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

			encoder.Encode("AAABBsertyuiolkjAAAAAhgfdsAAACC");
		}
		public string Encode(string input)
		{
			string[] frequencyScale;
			Dictionary<char, float> frequencies = new Dictionary<char, float>();
			float lowerBound, upperBound;
			
			string code = "";

			frequencies = GenerateFrequencies(input);
			frequencyScale = GenerateFrequencyScale(frequencies);
			//generateLowerAndUpperBounds(input,frequencies, out lowerBound, out upperBound);
			//code = generateCode(lowerBound, upperBound);

			return code;
		}

		//Generate a dictionary containing the frequencies of all characters in the string.
		private Dictionary<char, float> GenerateFrequencies(string input)
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

			Dictionary<char, float> frequencies = new Dictionary<char, float>();
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
		private string[] GenerateFrequencyScale(Dictionary<char, float> frequencies)
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
		private void generateLowerAndUpperBounds(string input, Dictionary<char,float> frequencies, out float lowerBound, out float upperBound)
		{
			char previousKey = frequencies.Keys.First();

		
			int i = 0;
			foreach(char key in frequencies.Keys)
			{
				if (i == 0)
				{ }
				else
				{
					frequencies[key] += frequencies[previousKey];
					previousKey = key;
				}

				i++;
			}

			Console.WriteLine("\n\n");
			foreach(char key in frequencies.Keys)
			{
				Console.WriteLine(frequencies[key]);
			}
			lowerBound = 0;
			upperBound = 0;
		}
		private string generateCode(float lowerBound, float upperBound)
		{
			return "";
		}





		
	}
}
