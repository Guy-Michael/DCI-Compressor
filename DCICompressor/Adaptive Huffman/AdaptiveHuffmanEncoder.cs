using System;
using System.Collections.Generic;
using System.IO;

namespace DCICompressor.Adaptive_Huffman
{
	class AdaptiveHuffmanEncoder
	{
		public static string Encode(string pathToFileToEncode, string pathToDestination)
		{
			BMPFile file = new BMPFile(pathToFileToEncode);
			string code = "";
			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			for(int i = 0; i < file.PixelData.Length; i += 3)
			{
				Index first = i, last = i + 3;
				uint24 newSign = new uint24(file.PixelData[first..last]);
				tree.AddNode(newSign);
				code += tree.OutputCodeOnFirstApperace(newSign);
			}

			Console.WriteLine(code);
			return code;
		}

		public static string EncodeNonBMP(string pathToFileToEncode)
		{
			HuffmanTree<byte> tree = new HuffmanTree<byte>();
			byte[] bytes = File.ReadAllBytes(pathToFileToEncode);
			Console.WriteLine($"number of bytes : {bytes.Length}");
			string code = "";
			foreach(byte b in bytes)
			{
				char c = (char)b;

				if (!tree.Contains(b))
				{
					tree.AddNode(b);
					string currentCodeAndPath = tree.OutputCodeOnFirstApperace(b) + " ";
					code += currentCodeAndPath;
				}

				else
				{
					tree.AddNode(b);
					string currentCode = tree.OutputCode(b) + " ";
					code += currentCode;
				}
			}
			Console.WriteLine(code);

			code = code.Replace(" ", "");
			return code;
		}
	}
}
