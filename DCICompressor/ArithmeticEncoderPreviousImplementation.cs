using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCICompressor
{
	class ArithmeticEncoderPreviousImplementation
	{

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
				//Thread.Sleep(500);
			}
			return code;
		}
		private void GenerateLowerAndUpperBounds(string[] quantityScale, string input, out ulong lowerBound, out ulong upperBound)
		{
			string[] alteredQuantityScale = (string[])quantityScale.Clone();
			string[] signs = Utils.RemoveEntriesWithLengthAbove1(quantityScale);


			//These are temporary values just so the file will compile.
			lowerBound = 0;
			upperBound = ulong.Parse(quantityScale.Last());

			for (int i = 0; i < input.Length; i++)
			{
				string currentSign = input[i].ToString();

				int signIndex = Utils.LinearSearch<string>(signs, currentSign);

				//modify the sign via the function f(n) -> 2n+1
				signIndex = (signIndex * 2) + 1;
				Console.WriteLine($"lower: {lowerBound}\tupper: {upperBound}\tdelta:{upperBound - lowerBound}");
				//assign lower and upper bounds.
				//Console.WriteLine($"sign - 1 {alteredQuantityScale[signIndex - 1]}\t sign + 1: {alteredQuantityScale[signIndex + 1]}");
				lowerBound = ulong.Parse(alteredQuantityScale[signIndex - 1]);
				upperBound = ulong.Parse(alteredQuantityScale[signIndex + 1]);


				//Thread.Sleep(500);
				alteredQuantityScale = Utils.NormalizeValues(quantityScale, alteredQuantityScale, lowerBound, upperBound);

			}
		}


	}
}
