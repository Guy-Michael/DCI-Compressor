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
			/*
			 * Need to derive Ci, Di, R 
			 */

			string decodedMessage = string.Empty;

			const ulong WHOLE = uint.MaxValue;
			const ulong HALF = WHOLE / 2;
			const ulong QUARTER = WHOLE / 4;
			const ulong PRECISION = 32;
			
			ulong lowerBound = 0, upperBound = WHOLE, approximation = 0, i = 0, R = 0;

			//Deriving R => The sum of all individual quantities.
			for (int k = 2; k < scale.Length; k += 2)
				{
					ulong currentQuantity = ulong.Parse(scale[k]) - ulong.Parse(scale[k - 2]);
					R += currentQuantity;
				}

			while (i < PRECISION + 1  && i < (ulong)encodedMessage.Length)
				{
					if (encodedMessage[(int)i] == '1')
					{
						approximation += (ulong)Math.Pow(2, PRECISION - i  );
					}

					i += 1;
				}

				int numberOfSymbolsFound = 0;
				while (numberOfSymbolsFound < decodedMessageLength)
				{
					for (int j = 1; j < scale.Length; j += 2)
					{
						ulong delta = upperBound - lowerBound;
						ulong upperApproximation = lowerBound + (ulong)Math.Round((double)(delta * ulong.Parse(scale[j + 1]) / R));
						ulong lowerApproximation = lowerBound + (ulong)Math.Round((double)(delta * ulong.Parse(scale[j - 1]) / R));
						if (lowerApproximation <= approximation && approximation < upperApproximation)
						{
							decodedMessage += scale[j];
							lowerBound = lowerApproximation;
							upperBound = upperApproximation;
							numberOfSymbolsFound++;
							break;
						}
					}

					while (upperBound < HALF || lowerBound >= HALF)
					{
						if (upperBound < HALF)
						{
							lowerBound *= 2;
							upperBound *= 2;
							approximation *= 2;
						}

						else if (lowerBound >= HALF)
						{
							lowerBound = 2 * (lowerBound - HALF);
							upperBound = 2 * (upperBound - HALF);
							approximation = 2 * (approximation - HALF);
						}

						if ((int)i < encodedMessage.Length && encodedMessage[encodedMessage.Length - 1 - (int)i] == '1')
						{
							approximation += 1;
						}
					i += 1;
					}

					while (lowerBound >= QUARTER && upperBound < 3 * QUARTER)
					{
						lowerBound = 2 * (lowerBound - QUARTER);
						upperBound = 2 * (upperBound - QUARTER);
						approximation = 2 * (approximation - QUARTER);

						if ((int)i < encodedMessage.Length && encodedMessage[encodedMessage.Length-1 - (int)i] == '1')
						{
							approximation += 1;

						}
					i += 1;
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
