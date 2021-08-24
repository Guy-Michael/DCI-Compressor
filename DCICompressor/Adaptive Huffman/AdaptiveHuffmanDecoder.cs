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
		public static string Decode(string code)
		{
			string decode = "";
			int symbolLength = 8;
			string temp = code;
			HuffmanTree<byte> tree = new HuffmanTree<byte>();
			HuffNode<byte> node = tree.Root;
			
			while(temp.Length != 0)
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
						tempCode = temp[1..9];
						temp = temp[9..];
					}

					else
					{
						tempCode = Convert.ToString(node.Value, 2);
					}
					byte b = Convert.ToByte(tempCode, 2);
					tree.AddNode(b);

					char c = (char)b;
					decode += c;
				}
			}
			//Console.WriteLine(decode);
			return decode;
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
					tree.AddNode(b);
					byte[] array = b.ToByteArray();
					decode.Append(array[0]);
					decode.Append(array[1]);
					decode.Append(array[2]);

				}
			}
			return decode.ToArray();
		}


		public static byte[] DecodeBMP(string code)
		{
			List<byte> decode = new List<byte>();
			int symbolLength = 8;
			string temp = code;
			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			HuffNode<uint24> node = tree.Root;

			while (temp.Length != 0)
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
						tempCode = temp[1..9];
						temp = temp[9..];
					}

					else
					{
						tempCode = uint24.ToBinaryString(node.Value);
					}
					uint24 b = uint24.TryParse(tempCode);
					tree.AddNode(b);
					byte[] array = b.ToByteArray();
					decode.Append(array[0]);
					decode.Append(array[1]);
					decode.Append(array[2]);

				}
			}
			return decode.ToArray();
		}
	}
}