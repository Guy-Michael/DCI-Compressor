using System;
using System.Threading;

class h
{
	//public static void Main(String[] args)
	//{
	//	int[] array = { 1, 2, 3, 4, 5, 6, 1, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
	//	int target = 7;
	//	Console.WriteLine(binarySearch(array, target));
	//}
	public static int binarySearch(int[] array, int target)
	{
		int lp = 0, rp = array.Length - 1;

		while (lp != rp)
		{
			int currentIndex = (lp + rp) / 2;
			if (array[currentIndex] == target)
			{
				return currentIndex;
			}

			else if (array[currentIndex] > target)
			{
				rp = currentIndex;
			}

			else lp = currentIndex;

			Console.WriteLine(lp + "\t" + rp);
			Thread.Sleep(1000);
		}


		return -1;
	}
}