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
			const ulong WHOLE = uint.MaxValue;
			
			ulong totalAmountOfQuantities = 0;
			
			foreach(KeyValuePair<char, ulong> pair in quantities)
			{
				totalAmountOfQuantities += pair.Value;
			}

			quantityScale[0] = "0";
			quantityScale[1] = quantities.Keys.ElementAt(0).ToString() ;
			quantityScale[2] =  ((ulong)(WHOLE *  ((double)quantities.Values.ElementAt(0) / totalAmountOfQuantities))).ToString();

			for (int i = 3; i < quantityScale.Length-1; i += 2)
			{
				int scaleToQuantityIndex = (i - 1) / 2;
				quantityScale[i] = quantities.Keys.ElementAt(scaleToQuantityIndex).ToString();
				ulong prevSumOfFrequencies = ulong.Parse(quantityScale[i - 1]);
				double ratio = (double)quantities.Values.ElementAt(scaleToQuantityIndex) /(double) totalAmountOfQuantities;
				ulong newCurrentVal = (ulong)Math.Round(WHOLE * ratio);
				quantityScale[i + 1] = (newCurrentVal+ prevSumOfFrequencies).ToString();
			}
			quantityScale[quantityScale.Length - 1] = uint.MaxValue.ToString();
			return quantityScale;
		}
		
		private string GenerateBoundsAndCodeWithFinitePrecision(string[] scale)
		{
			string code = string.Empty;
			ulong lowerBound = 0, upperBound = uint.MaxValue;

			//const ulong WHOLE = uint.MaxValue;
			//const ulong HALF = WHOLE / 2;
			//const ulong QUARTER = WHOLE / 4;

			const ulong WHOLE = uint.MaxValue - 1 ;  //choose largest even number
			const ulong HALF = WHOLE / 2;
			const ulong QUARTER = HALF / 2;

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
				ulong w = upperBound - lowerBound;
				upperBound = lowerBound + (ulong)Math.Round((double)w * ulong.Parse(scale[i + 1]) / cumlativeQuantity);
				lowerBound = lowerBound + (ulong)Math.Round((double)w * ulong.Parse(scale[i - 1]) / cumlativeQuantity);

				//ulong w = upperBound - lowerBound;

				//ulong currentUpperBound = ulong.Parse(scale[i + 1]);
				//ulong currentLowerBound = ulong.Parse(scale[i - 1]);

				//ulong scaledCurrentUpperBound = w * currentUpperBound;
				//ulong upperBoundRatio = (ulong) Math.Round((double)(scaledCurrentUpperBound / cumlativeQuantity));
				//upperBound = lowerBound + upperBoundRatio;


				//ulong scaledCurrentLowerBound = w * currentLowerBound;
				//ulong lowerBoundRatio = (ulong)Math.Round((double)(scaledCurrentLowerBound / cumlativeQuantity));
				//lowerBound += lowerBoundRatio;




				while ( upperBound < HALF || lowerBound >= HALF)
				{
					if ( upperBound < HALF)
					{
						code += "0" + new string('1', s);
						s = 0;
						lowerBound *= 2;
						upperBound *= 2;
					}

					else if ( lowerBound >= HALF)
					{
						code += "1" + new string('0', s);
						s = 0;
						lowerBound = 2 * (lowerBound - HALF);
						upperBound = 2 * (upperBound - HALF);
					}
				}


				while (lowerBound > QUARTER && upperBound < 3 * QUARTER)
				{
					s += 1;
					lowerBound = 2 * (lowerBound - QUARTER);
					upperBound = 2 * (upperBound - QUARTER);
				}

			}
			s += 1;
			if (lowerBound <= QUARTER)
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
