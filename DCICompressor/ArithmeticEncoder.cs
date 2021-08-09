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
			string[] messages = { "ABCD" };
			for(int i=0; i<messages.Length; i++)
			{
				string encode = messages[i];
				string code;
				string[] scale;
				encoder.Encode(encode, out code, out scale);

				Console.WriteLine(code);

				Console.WriteLine("Starting decoding.");
				string decoded = decoder.decode(code, scale, encode.Length, encode);
			}
		}
		public void Encode(string input, out string code, out string[] scale)
		{
			SortedDictionary<char, ulong> quantities = new SortedDictionary<char, ulong>();
			quantities = GenerateQuantities(input);
			scale = scaleQuantitiesBasedOnMaximumCapacity(quantities);
			code = GenerateBoundsAndCodeWithFinitePrecision(scale);
			
		}

		//Generate a dictionary containing the frequencies of all characters in the string.
		private SortedDictionary<char, ulong> GenerateQuantities(string input)
		{
			SortedDictionary<char, ulong> quantities = new SortedDictionary<char, ulong>();
			
			//foreach character in the string, check if it is in the dictionary.
			//if it is, increment its quantity.
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
			//quantities.Reverse();
			return quantities;
		}
	

		//********************METHODS FOR FINITE PRECISION IMPLEMENTATION************************//
		private string[] scaleQuantitiesBasedOnMaximumCapacity(SortedDictionary<char, ulong> quantities)
		{
			string[] quantityScale = new string[(quantities.Count * 2) + 1];
			//const ulong Constants.WHOLE = uint.MaxValue;
			
			ulong totalAmountOfQuantities = 0;
			
			foreach(KeyValuePair<char, ulong> pair in quantities)
			{
				totalAmountOfQuantities += pair.Value;
			}

			quantityScale[0] = "0";
			quantityScale[1] = quantities.Keys.ElementAt(0).ToString();

			//quantityScale[2] = ((ulong)(Constants.WHOLE * ((double)quantities.Values.ElementAt(0) / totalAmountOfQuantities))).ToString();
			double tempRatio = ((double)quantities.Values.ElementAt(0) / totalAmountOfQuantities);
			quantityScale[2] = ((ulong)(Constants.WHOLE * tempRatio)).ToString();



			for (int i = 3; i < quantityScale.Length-1; i += 2)
			{
				int scaleToQuantityIndex = (i - 1) / 2;
				quantityScale[i] = quantities.Keys.ElementAt(scaleToQuantityIndex).ToString();
				ulong prevSumOfFrequencies = ulong.Parse(quantityScale[i - 1]);

				//double ratio = (double)quantities.Values.ElementAt(scaleToQuantityIndex) /(double) totalAmountOfQuantities;
				ulong currentQuantity = quantities.Values.ElementAt(scaleToQuantityIndex);
				double ratio = ((double)currentQuantity) / totalAmountOfQuantities;

				ulong newCurrentVal = (ulong) Math.Round(Constants.WHOLE * ratio);
				quantityScale[i + 1] = (newCurrentVal + prevSumOfFrequencies).ToString();
			}

			quantityScale[quantityScale.Length - 1] = Constants.WHOLE.ToString();
			return quantityScale;
		}
		
		private string GenerateBoundsAndCodeWithFinitePrecision(string[] scale)
		{
			string code = string.Empty;
			ulong lowerBound = 0, upperBound = uint.MaxValue;

			//const ulong Constants.WHOLE = uint.MaxValue;
			//const ulong Constants.HALF = Constants.WHOLE / 2;
			//const ulong Constants.QUARTER = Constants.WHOLE / 4;

			//const ulong Constants.WHOLE = uint.MaxValue - 1 ;  //choose largest even number
			//const ulong Constants.HALF = Constants.WHOLE / 2;
			//const ulong Constants.QUARTER = Constants.HALF / 2;

			ulong cumlativeQuantity = 0;
			for (int i = 2; i < scale.Length; i += 2)
			{
				ulong currentLowerBound = ulong.Parse(scale[i - 2]);
				ulong currentUpperBound = ulong.Parse(scale[i]);

				ulong currentQuantity = currentUpperBound - currentLowerBound;
				cumlativeQuantity += currentQuantity;
			}

			int s = 0;
			for (int i = 1; i < scale.Length; i += 2)
			{
				ulong delta = upperBound - lowerBound;

				ulong currentUpperBound = ulong.Parse(scale[i + 1]);
				ulong currentLowerBound = ulong.Parse(scale[i - 1]);

				//upperBound = lowerBound + (ulong)Math.Round((double)w * ulong.Parse(scale[i + 1]) / cumlativeQuantity);
				ulong scaledCurrentUpperBound = delta * currentUpperBound;
				ulong upperBoundRatio = (ulong)Math.Round((double)(scaledCurrentUpperBound / cumlativeQuantity));
				upperBound = lowerBound + upperBoundRatio;

				//lowerBound = lowerBound + (ulong)Math.Round((double)w * ulong.Parse(scale[i - 1]) / cumlativeQuantity);
				ulong scaledCurrentLowerBound = delta * currentLowerBound;
				ulong lowerBoundRatio = (ulong)Math.Round((double)(scaledCurrentLowerBound / cumlativeQuantity));
				lowerBound += lowerBoundRatio;




				while ( upperBound < Constants.HALF || lowerBound >= Constants.HALF)
				{
					if ( upperBound < Constants.HALF)
					{
						code += "0" + new string('1', s);
						s = 0;
						lowerBound *= 2;
						upperBound *= 2;
					}

					else if ( lowerBound >= Constants.HALF)
					{
						code += "1" + new string('0', s);
						s = 0;
						lowerBound = 2 * (lowerBound - Constants.HALF);
						upperBound = 2 * (upperBound - Constants.HALF);
					}
				}


				while (lowerBound > Constants.QUARTER && upperBound < 3 * Constants.QUARTER)
				{
					s += 1;
					lowerBound = 2 * (lowerBound - Constants.QUARTER);
					upperBound = 2 * (upperBound - Constants.QUARTER);
				}

			}
			s += 1;
			if (lowerBound <= Constants.QUARTER)
			{
				code += "0" + new string('1', s);
			}

			else
			{
				code += "1" + new string('0', s);
			}
			return code;
		}

		private string[] createQuantityScaleWithoutScaling(SortedDictionary<char, ulong> quantities)
		{
			string[] quantityScale = new string[(quantities.Count * 2) + 1];
			ulong totalAmountOfQuantities = 0;
			foreach (KeyValuePair<char, ulong> pair in quantities)
			{
				totalAmountOfQuantities += pair.Value;
			}

			quantityScale[0] = "0";
			quantityScale[1] = quantities.Keys.ElementAt(0).ToString();
			quantityScale[2] = quantities.Values.ElementAt(0).ToString();

			for (int i = 3; i < quantityScale.Length; i += 2)
			{
				int scaleToQuantityIndex = (i - 1) / 2;
				quantityScale[i] = quantities.Keys.ElementAt(scaleToQuantityIndex).ToString();
				ulong prevSumOfFrequencies = ulong.Parse(quantityScale[i - 1]);
				quantityScale[i + 1] = (quantities[quantityScale[i][0]] + prevSumOfFrequencies).ToString();
			}
			//quantityScale[quantityScale.Length - 1] = uint.MaxValue.ToString();
			return quantityScale;
		}





	}
}
