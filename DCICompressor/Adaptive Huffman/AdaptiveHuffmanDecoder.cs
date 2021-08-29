using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DCICompressor
{
	class AdaptiveHuffmanDecoder
	{
		//public static string Decode8BitPerSymbol(string code)
		//{
		//	string decode = "";
		//	int symbolLength = 8;
		//	string temp = code;
		//	HuffmanTree<byte> tree = new HuffmanTree<byte>();
		//	HuffNode<byte> node = tree.Root;
			
		//	while(temp.Length != 0)
		//	{
		//		node = tree.Root;
		//		while (!node.IsLeaf())
		//		{
		//			if (temp[0] == '0')
		//			{
		//				node = node.LeftChild;
		//			}

		//			else if (temp[0] == '1')
		//			{
		//				node = node.RightChild;
		//			}
		//			temp = temp[1..];
		//		}

		//		if (node.IsLeaf())
		//		{
		//			string tempCode = "";
		//			if (node.Frequency == 0)
		//			{
		//				tempCode = temp[1..9];
		//				temp = temp[9..];
		//			}

		//			else
		//			{
		//				tempCode = Convert.ToString(node.Value, 2);
		//			}
		//			byte b = Convert.ToByte(tempCode, 2);
		//			tree.AddNode(b);

		//			char c = (char)b;
		//			decode += c;
		//		}
		//	}
			//Console.WriteLine(decode);
		//	return decode;
		//}

		public static byte[] Decode8BitBMPCorrectWithRegardsToHeader(string input, string output)
		{
			BinaryWriter writer = new BinaryWriter(File.Open(output, FileMode.Create));

			byte[] arr = File.ReadAllBytes(input);
			BMPFile file = new BMPFile(arr);


			//string code = string.Join("", file.PixelData.Select(x => Convert.ToString(x, 2)));
			string code = string.Empty;

			foreach (byte b in file.PixelData)
			{
				string tempCode = Convert.ToString(b, 2);
				int padding = 8 - tempCode.Length;
				tempCode = new string('0', padding) + tempCode;
				code += tempCode;
			}




		//	Console.WriteLine("The length of the code is: " + code.Length);
			List<byte> decode = new List<byte>();
			decode.AddRange(file.HeaderData);
			string temp = code;
			HuffmanTree<byte> tree = new HuffmanTree<byte>();
			HuffNode<byte> node = tree.Root;
			int k = 0;
			while (temp.Length > 0)
			{
				node = tree.Root;
					while (!node.IsLeaf() && temp.Length > 0 )
					{
						//Console.WriteLine($"code length is: {temp.Length}");
						if (temp[0].Equals('0'))
						{
							node = node.LeftChild;
						}

						else if (temp[0].Equals('1'))
						{
							node = node.RightChild;
						}
						temp = temp[1..];
					}

				if (node.IsLeaf())
				{
					string tempCode = "";
					//if (temp.Length < 8)
					//{
					//	tempCode = temp[0..];
					//	temp = string.Empty;
					//}

					//Console.WriteLine(temp);
					if (node.IsNYT)
					{
						tempCode = temp[1..9];
						temp = temp[9..];
					}

					else
					{

						tempCode = Convert.ToString(node.Value,2);
						//int padding = 8 - tempCode.Length;
						//tempCode = new string('0', padding) + tempCode;
					}

					//Console.WriteLine(tempCode);
				//	Console.WriteLine(tempCode);
					if (tempCode.Equals(string.Empty) == false)
					{
						byte b = Convert.ToByte(tempCode, 2);
						decode.Add(b);
						tree.AddNodeCorrect(b);

					}
					k++;

					//if (k%100 == 0)
					Console.WriteLine("Remaining: " + temp.Length);
				}
			}


			//Console.WriteLine("bytes in decode: " + decode.Count);
			writer.Write(decode.ToArray());
			//writer.Flush();
			writer.Close();
			return decode.ToArray();
		}



		public static byte[] Decode24BitBMPCorrectWithRegardsToHeader(string input, string output)
		{
			BinaryWriter writer = new BinaryWriter(File.Open(output, FileMode.Create));

			byte[] arr = File.ReadAllBytes(input);
			BMPFile file = new BMPFile(arr);
			string code = string.Join("", file.PixelData.Select(x => Convert.ToString(x, 2)));
			Console.WriteLine(code.Length);
			List<byte> decode = new List<byte>();
			decode.AddRange(file.HeaderData);
			string temp = code;
			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			HuffNode<uint24> node = tree.Root;

			while (temp.Length > 0)
			{
				node = tree.Root;
				while (!node.IsLeaf())
				{
					if (temp[0] == '0')
					{
						node = node.LeftChild;
					}

					else if (temp[0] == '1')
					{
						node = node.RightChild;
					}
					temp = temp[1..];
				}

				if (node.IsLeaf())
				{
					string tempCode = "";

					if (node.Frequency == 0)
					{
						tempCode = temp[0..24];
						temp = temp[24..];
					}

					else
					{
						tempCode = uint24.ToBinaryString(node.Value);
					}
					uint24 b = uint24.TryParse(tempCode);
					//Console.WriteLine("Adding " + b + "To tree ");
					tree.AddNodeCorrect(b);
					byte[] array = b.ToByteArray();
					decode.Add(array[0]);
					decode.Add(array[1]);
					decode.Add(array[2]);
					//Console.WriteLine("Found leaf! value is : " + b);
					foreach (byte by in array)
					{
						Console.Write($"value: {by}\t");
					}
					Console.WriteLine();
				}
			}


			Console.WriteLine("bytes in decode: " + decode.Count);
			writer.Write(decode.ToArray());
			writer.Flush();
			writer.Close();
			return decode.ToArray();
		}




		public static byte[] DecodeBMPCorrect(string input, string output)
		{
			BinaryWriter writer = new BinaryWriter(File.Open(output, FileMode.Create));

			byte[] arr = File.ReadAllBytes(input);
			BMPFile file = new BMPFile(arr);
			string code = string.Join("", arr.Select(x => Convert.ToString(x, 2)));
			Console.WriteLine(code.Length);
			List<byte> decode = new List<byte>();
			string temp = code;
			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			HuffNode<uint24> node = tree.Root;

			while (temp.Length > 0)
			{
				node = tree.Root;
				while (!node.IsLeaf())
				{
					if (temp[0] == '0')
					{
						node = node.LeftChild;
					}

					else if (temp[0] == '1')
					{
						node = node.RightChild;
					}
					temp = temp[1..];
				}

				if (node.IsLeaf())
				{
					string tempCode = "";

					if (node.Frequency == 0)
					{
						tempCode = temp[0..24];
						temp = temp[24..];
					}

					else
					{
						tempCode = uint24.ToBinaryString(node.Value);
					}
					uint24 b = uint24.TryParse(tempCode);
					tree.AddNodeCorrect(b);
					byte[] array = b.ToByteArray();
					decode.Add(array[0]);
					decode.Add(array[1]);
					decode.Add(array[2]);
					Console.WriteLine("Found leaf! value is : " + b);
					foreach(byte by in array)
					{
						Console.Write($"value: {by}\t");
					}
					Console.WriteLine();
				}
			}


			Console.WriteLine("bytes in decode: " + decode.Count);
			writer.Write(decode.ToArray());
			writer.Flush();
			writer.Close();
			return decode.ToArray();
		}


		//public static byte[] Decode24BitBMP(string code)
		//{
		//	List<byte> decode = new List<byte>();
		//	int symbolLength = 8;
		//	string temp = code;
		//	HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
		//	HuffNode<uint24> node = tree.Root;

		//	while (temp.Length != 0)
		//	{
		//		node = tree.Root;
		//		while (!node.IsLeaf())
		//		{
		//			if (temp[0] == '0')
		//			{
		//				node = node.LeftChild;
		//			}

		//			else if (temp[0] == '1')
		//			{
		//				node = node.RightChild;
		//			}
		//			temp = temp[1..];
		//		}

		//		if (node.IsLeaf())
		//		{
		//			string tempCode = "";
		//			if (node.Frequency == 0)
		//			{
		//				tempCode = temp[1..9];
		//				temp = temp[9..];
		//			}

		//			else
		//			{
		//				tempCode = uint24.ToBinaryString(node.Value);
		//			}
		//			uint24 b = uint24.TryParse(tempCode);
		//			tree.AddNode(b);
		//			byte[] array = b.ToByteArray();
		//			decode.Append(array[0]);
		//			decode.Append(array[1]);
		//			decode.Append(array[2]);

		//		}
		//	}
		//	return decode.ToArray();
		//}
	}
}