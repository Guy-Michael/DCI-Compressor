using System;
using System.Collections.Generic;
using System.IO;

namespace DCICompressor.Adaptive_Huffman
{
	class AdaptiveHuffmanEncoder
	{
		public static void Encode(string pathToFileToEncode, string pathToDestination)
		{
			BMPFile file = new BMPFile(pathToFileToEncode);

			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			for(int i = 0; i < file.PixelData.Length; i += 3)
			{
				Index first = i, last = i + 3;
				uint24 newSign = new uint24(file.PixelData[first..last]);
				tree.AddNodeAndOutputCode(newSign);
			}
		}

		public static void EncodeNonBMP(string pathToFileToEncode)
		{
			HuffmanTree<byte> tree = new HuffmanTree<byte>();
			byte[] bytes = File.ReadAllBytes(pathToFileToEncode);
			foreach(byte b in bytes)
			{
				tree.AddNodeAndOutputCode(b);
			}
		}
	}
}
