using System;
using System.Collections.Generic;
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

			encoder.Encode("AAFGJUTDCVMJKOIUYTRFNJUYTRDABCBCCACBC");
		}
		public string Encode(string input)
		{
			float[] frequencyScale = new float[input.Length];
			Dictionary<char, float> frequencies = new Dictionary<char, float>();
			float lowerBound, upperBound;
			
			string code = "";

			frequencies = GenerateFrequencies(input);
			frequencyScale = GenerateFrequencyScale(frequencies);
			generateLowerAndUpperBounds(out lowerBound, out upperBound);
			code = generateCode(lowerBound, upperBound);

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

		private float[] GenerateFrequencyScale(Dictionary<char, float> frequencies)
		{
			throw new NotImplementedException();
		}

		private void generateLowerAndUpperBounds(out float lowerBound, out float upperBound)
		{
			throw new NotImplementedException();
		}


		private string generateCode(float lowerBound, float upperBound)
		{
			throw new NotImplementedException();
		}





		
	}
}
