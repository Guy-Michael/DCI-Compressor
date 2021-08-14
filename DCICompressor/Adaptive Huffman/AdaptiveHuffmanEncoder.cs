using System;
using System.Collections.Generic;

namespace DCICompressor.Adaptive_Huffman
{
	class AdaptiveHuffmanEncoder
	{
		public static void Encode(string pathToFileToEncode, string pathToDestination)
		{
			BMPFile file = new BMPFile(pathToFileToEncode);
			//uint val = uint24.ToUInt(file.PixelData[0..2]);
			//uint24 val = new uint24(file.PixelData[0..2]);
			HuffNode<uint> nullNode = new HuffNode<uint>();
			HuffNode<uint> rootNode = nullNode;

			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			for(int i = 0; i < file.PixelData.Length; i += 3)
			{
				Index first = i, last = i + 2;
				uint24 newSign = new uint24(file.PixelData[first..last]);
				if (tree.Contains(newSign))
				{
					tree.FindAndIncrementNodeWithSign(newSign);
				}

				else
				{
					tree.AddNode(newSign);
				}
				
				//if (tree)

				//uint newVal = val.ToUInt(file.PixelData[first..last]);
				//HuffNode<uint> newNode = new HuffNode<uint>(newVal);


			}

		}

		//private static HuffNode<uint> addNode(HuffNode<uint> root)
		//{

		//}


	}
}
