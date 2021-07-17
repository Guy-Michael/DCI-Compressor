using System;
using System.Collections.Generic;
using System.Threading;

namespace DCICompressor
{
	class Program
	{
		public static List<Tuple<String, float>> list = new List<Tuple<String, float>>();
		static float[] initialScale = { 0, 0.6f, 0.8f, 1 };


		static void Main(string[] args)
		{
			//word is ABBCA
			list.Add(new Tuple<String, float>("A", 0.6f));
			list.Add(new Tuple<String, float>("B", 0.2f));
			list.Add(new Tuple<String, float>("C", 0.2f));
			float[] scale = new float[4];
			scale[0] = 0;
			scale[1] = 0.6f;
			scale[2] = 0.8f;
			scale[3] = 1;

			//letter A
			scale = rescaleArray(scale, 0, 1);
			//letter B
			scale = rescaleArray(scale, 1, 2);

			////letter B
			scale = rescaleArray(scale, 1, 2);

			////letter C
			scale = rescaleArray(scale, 2, 3);

			////letter A
			scale = rescaleArray(scale, 0, 1);


			//	//Part two.
			//	float[] scaleForCompression = { scale[0], scale[1] };
			//	float[] alternatingVal = { 0, 1 };
			//	string compressedMessage = "";


		}


		public static string encode(float[] array, int minIndex, int maxIndex)
		{
			string code = "";

			float minVal = array[minIndex], maxVal = array[maxIndex];
			float lp = 0, rp = 1;
			while (!(lp>minVal && lp<maxVal && rp>minVal && rp<maxVal))
			{

			}





			return code;
		}
			public static float[] rescaleArray(float[] array,int minIndex,int maxIndex)
		{
			float delta = (array[maxIndex] - array[minIndex]);
			Console.WriteLine("delta: " + delta + "\t min: " + array[minIndex] + "\tmax: " + array[maxIndex]);
			float[] rescaledArray = new float[array.Length];

			rescaledArray[0] = array[minIndex];
			for (int i=1; i<rescaledArray.Length; i++)
			{
				rescaledArray[i] = delta * initialScale[i] + rescaledArray[0];
			}

			for (int i = 0; i < rescaledArray.Length; i++)
			{
				Console.WriteLine(rescaledArray[i]);
			}

			Console.WriteLine();
			return rescaledArray;
		}
	}
}
