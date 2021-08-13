using System;
using System.Threading;
namespace DCICompressor
{
	class ArithmeticDecoder
	{
		
		public ArithmeticDecoder()
		{
		}

		public string decode(string encodedMessage, string[] scale, int decodedMessageLength, string original)
		{
			string decodedMessage = string.Empty;	
			ulong a = 0, b = Constants.WHOLE, z = 0, i = 0, R = 0;

			//Deriving R => The sum of all individual quantities.
			for (int k = 2; k < scale.Length; k += 2)
			{
				ulong currentUpperBound = ulong.Parse(scale[k]);
				ulong currentLowerBound = ulong.Parse(scale[k - 2]);
				ulong currentQuantity = currentUpperBound - currentLowerBound;
				R += currentQuantity;
			}

			while (i <= Constants.PRECISION && i < (ulong)encodedMessage.Length)
			{
				//Console.WriteLine($"is {i} smaller than {encodedMessage.Length}? {i < (ulong) encodedMessage.Length}");
				if (encodedMessage[(int)i] == '1')
				{
					z += (ulong)Math.Pow(2, Constants.PRECISION - i);
				}

				i += 1;
			}

			int numberOfSymbolsFound = 0;
			while (numberOfSymbolsFound < decodedMessageLength)
			{
				for (int j = 1; j < scale.Length; j += 2)
				{
					ulong delta = b - a;
					//ulong b0 = a + (ulong)Math.Round((double)(delta * ulong.Parse(scale[j + 1]) / R));
					//ulong a0 = a + (ulong)Math.Round((double)(delta * ulong.Parse(scale[j - 1]) / R));

					ulong parsedUpperBound = ulong.Parse(scale[j + 1]);
					ulong parsedLowerBound = ulong.Parse(scale[j - 1]);

					double frequencyOfUpperBound = ((double)parsedUpperBound) / R;
					double frequencyOfLowerBound = ((double)parsedLowerBound) / R;

					double scaledUpperBound = delta * frequencyOfUpperBound;
					double scaledLowerBound = delta * frequencyOfLowerBound;

					ulong roundedUpperBound = (ulong)Math.Round(scaledUpperBound);
					ulong roundedLowerBound = (ulong)Math.Round(scaledLowerBound);

					ulong b0 = a + roundedUpperBound;
					ulong a0 = a + roundedLowerBound;
	
					if (a0 <= z && z < b0)
					{

						decodedMessage += scale[j];
						a = a0;
						b = b0;
						numberOfSymbolsFound++;
					
					}
				}
				while (b < Constants.HALF || a > Constants.HALF)
				{
					if (b < Constants.HALF)
					{
						a *= 2;
						b *= 2;
						z *= 2;
					}

					else if (a >= Constants.HALF)
					{
						a = 2 * (a - Constants.HALF);
						b = 2 * (b - Constants.HALF);
						z = 2 * (z - Constants.HALF);
					}

					if ((int)i < encodedMessage.Length && encodedMessage[encodedMessage.Length - 1 - (int)i] == '1')
					{
						z += 1;
					}
				i += 1;
				}

				while (a > Constants.QUARTER && b < 3 * Constants.QUARTER)
				{
					a = 2 * (a - Constants.QUARTER);
					b = 2 * (b - Constants.QUARTER);
					z = 2 * (z - Constants.QUARTER);

					if ((int)i < encodedMessage.Length && encodedMessage[encodedMessage.Length-1 - (int)i] == '1')
					{
						z += 1;	
						i += 1;
					}
			
				}

				//Thread.Sleep(300);
			}
			Console.WriteLine($"Length : {original.Length}\tHamming distance: {HammingDistance(original, decodedMessage)}");
			Console.WriteLine($"original: {original} \t decodedMessage: {decodedMessage}\t ");

			//return decodedMessage;
			return string.Empty;
		}

		public int HammingDistance(string str1, string str2)
		{
			int dist = 0;
			if (str1.Length != str2.Length)
			{
				Console.WriteLine("Lengths dont match!");
				return -1;
			}

			for(int i = 0; i< str1.Length; i++)
			{
				if (str1[i] != str2[i])
					dist++;
			}
			return dist;
			//return (int) Math.Abs((double) ulong.Parse(str1) - ulong.Parse(str2));
		}
	}



}
