using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCICompressor.Adaptive_Huffman
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
			Console.WriteLine(decode);
			return decode;
		}
	}


}
