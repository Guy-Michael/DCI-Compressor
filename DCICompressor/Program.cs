using System;
using System.Collections.Generic;
using System.Threading;

namespace DCICompressor
{
	class Program
	{
		public static void Main(String[] args)
		{
			Console.WriteLine(uint.MaxValue-1);
			//ArithmeticEncoderPreviousImplementation encoder = new ArithmeticEncoderPreviousImplementation();
			ArithmeticEncoder encoder = new ArithmeticEncoder();
			ArithmeticDecoder decoder = new ArithmeticDecoder();
			string messages = "AAABBABDCABDCBABDCABCBACDDBDCABBCCCDDD";

			string encode = messages;
			string code;
			string[] scale;
			encoder.Encode(encode, out code, out scale);

			Console.WriteLine(code);
			string decoded = decoder.decode(code, scale, encode.Length, encode);
			Console.WriteLine(decoded);
			//ArithmeticEncoder encoder = new ArithmeticEncoder();
			//ArithmeticDecoder decoder = new ArithmeticDecoder();
			//string messages ="AAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBBBBBBBBBBBBBBBCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";

			//string encode = messages;
			//	string code;
			//	string[] scale;
			//	encoder.Encode(encode, out code, out scale);

			//	Console.WriteLine(code);

			//	Console.WriteLine("Starting decoding.");
			//	string decoded = decoder.decode(code, scale, encode.Length, encode);
		}
	}






	//	public static List<Tuple<String, float>> list = new List<Tuple<String, float>>();
	//	static float[] initialScale = { 0, 0.6f, 0.8f, 1 };


	//	//static void Main(string[] args)
	//	//{
	//	//	//word is ABBCA
	//	//	list.Add(new Tuple<String, float>("A", 0.6f));
	//	//	list.Add(new Tuple<String, float>("B", 0.2f));
	//	//	list.Add(new Tuple<String, float>("C", 0.2f));
	//	//	float[] scale = new float[4];
	//	//	scale[0] = 0;
	//	//	scale[1] = 0.6f;
	//	//	scale[2] = 0.8f;
	//	//	scale[3] = 1;

	//	//	//letter A
	//	//	scale = rescaleArray(scale, 0, 1);
	//	//	//letter B
	//	//	scale = rescaleArray(scale, 1, 2);

	//	//	////letter B
	//	//	scale = rescaleArray(scale, 1, 2);

	//	//	////letter C
	//	//	scale = rescaleArray(scale, 2, 3);

	//	//	////letter A
	//	//	scale = rescaleArray(scale, 0, 1);


	//	//	//	//Part two.
	//	//	//	float[] scaleForCompression = { scale[0], scale[1] };
	//	//	//	float[] alternatingVal = { 0, 1 };
	//	//	//	string compressedMessage = "";

	//	//	float[] f = { 0.68f, 0.712f };
	//	//	Console.WriteLine(encode(f,0,1));
	//	//	//Console.WriteLine(encode(scale, 0, 1));
	//	//}


	//	public static string encode(float[] array, int minIndex, int maxIndex)
	//	{
	//		string code = "";
	//		float leftPointer = 0, rightPointer = 1, lowerBound = array[minIndex], upperBound = array[maxIndex];
	//		bool inside = false;


	//		for (int i=0; i<15 && !inside; i++)
	//		{
	//			float midPoint = (leftPointer + rightPointer) / 2f;
	//			Console.WriteLine("Start");
	//			Console.WriteLine($"lp: {leftPointer}\trp: {rightPointer}\tmidVal: {midPoint}\nmivVal: {lowerBound}\tmaxVal: {upperBound}");

	//			if (leftPointer > lowerBound && leftPointer < upperBound && rightPointer > lowerBound && rightPointer < upperBound)
	//			{
	//				inside = true;
	//				Console.WriteLine("Broken!");
	//				break;
	//			}
				
	//			else if (leftPointer < lowerBound && midPoint > upperBound)
	//			{
	//				rightPointer = midPoint;
	//				code += "0";
	//			}

	//			else if (rightPointer > upperBound && midPoint < lowerBound)
	//			{
	//				leftPointer = midPoint;
	//				code += "1";
	//			}

	//			//if this section is reached, minVal<mid<maxVal.

	//			else if (!(leftPointer>lowerBound && leftPointer<upperBound))
	//			{
	//				code += "1";
	//				leftPointer = midPoint;
	//			}

	//			else if (!(rightPointer>lowerBound && rightPointer<upperBound))
	//			{
	//				code += "0";
	//				rightPointer = midPoint;
	//			}

	//			else { inside = true; }

				
	//			Console.WriteLine("End");
	//			Console.WriteLine($"lp: {leftPointer}\trp: {rightPointer}\tmidVal: {midPoint}\nmivVal: {lowerBound}\tmaxVal: {upperBound}");

	//			Console.WriteLine("*******************");
	//			//Thread.Sleep(1000);
	//		}
	//		return code;
	//	}
	//		public static float[] rescaleArray(float[] array,int minIndex,int maxIndex)
	//	{
	//		float delta = (array[maxIndex] - array[minIndex]);
	//		Console.WriteLine("delta: " + delta + "\t min: " + array[minIndex] + "\tmax: " + array[maxIndex]);
	//		float[] rescaledArray = new float[array.Length];

	//		rescaledArray[0] = array[minIndex];
	//		for (int i=1; i<rescaledArray.Length; i++)
	//		{
	//			rescaledArray[i] = delta * initialScale[i] + rescaledArray[0];
	//		}

	//		for (int i = 0; i < rescaledArray.Length; i++)
	//		{
	//			Console.WriteLine(rescaledArray[i]);
	//		}

	//		Console.WriteLine();
	//		return rescaledArray;
	//	}
	//}



}
