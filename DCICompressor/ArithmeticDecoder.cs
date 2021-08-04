using System;
using System.Threading;
namespace DCICompressor
{
	class ArithmeticDecoder
	{
		
		public ArithmeticDecoder()
		{
		}

		public string decode(string encodedMessage, string[] scale, int decodedMessageLength)
		{
			/*
			 * Need to derive Ci, Di, R 
			 */

			string decodedMessage = string.Empty;

			const ulong WHOLE = uint.MaxValue;
			const ulong HALF = WHOLE / 2;
			const ulong QUARTER = WHOLE / 4;
			const ulong PRECISION = 32;
			ulong a = 0, b = WHOLE, z = 0, i = 0;
			ulong R = 0;

			//Deriving R => The sum of all individual quantities.
			for (int k = 2; k < scale.Length; k += 2)
			{
				ulong currentQuantity = ulong.Parse(scale[k]) - ulong.Parse(scale[k - 2]);
				R += currentQuantity;
			}

			while (i <= PRECISION && i < (ulong)encodedMessage.Length)
			{
				if (encodedMessage[(int) i] == '1')
				{
					Console.WriteLine("Z grew");
					z += (ulong)Math.Pow(2, PRECISION - i);
				}

				i += 1;
			}

			bool ended = false;
			int numberOfSymbolsFound = 0;
			while (numberOfSymbolsFound < decodedMessageLength)
			{
				for (int j = 1; j < scale.Length; j += 2)
				{
					ulong w = b - a;
					ulong b0 = a + (ulong)Math.Round((double) w * ulong.Parse(scale[j + 1]) / R);
					ulong a0 = a + (ulong)Math.Round((double) w * ulong.Parse(scale[j - 1]) / R);

					if (a0 <= z && z < b0)
					{
						Console.WriteLine("Found a symbol!");
						decodedMessage += scale[j];
						a = a0;
						b = b0;
						numberOfSymbolsFound++;

					}
				}

				while (b < HALF || a > HALF)
				{
					if (b < HALF)
					{
						a *= 2;
						b *= 2;
						z *= 2;
					}

					else if (a > HALF)
					{
						a = 2 * (a - HALF);
						b = 2 * (b - HALF);
						z = 2 * (z - HALF);
					}

					if ((int) i < encodedMessage.Length && encodedMessage[(int) i] == '1')
					{
						z += 1;
					}
				}

				while (a > QUARTER && b < 3 * QUARTER)
				{
					a = 2 * (a - QUARTER);
					b = 2 * (b - QUARTER);
					z = 2 * (z - QUARTER);

					if ((int)i < encodedMessage.Length && encodedMessage[(int) i] == '1')
					{
						z += 1;
						i += 1;
					}
				}
				Console.WriteLine(decodedMessage);
				//Thread.Sleep(300);
			}
			return decodedMessage;
		}
	}
}
