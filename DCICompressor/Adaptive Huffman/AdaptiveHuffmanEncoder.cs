using System;
using System.Collections.Generic;
using System.IO;

namespace DCICompressor
{
	public class AdaptiveHuffmanEncoder
	{
		public static IEnumerator<int> Encode8Bit(string pathToFileToEncode, string pathToDestination)
		{
			//Create the .bmp file and the binary writer.
			BMPFile file = new BMPFile(pathToFileToEncode);
			BinaryWriter writer = new BinaryWriter(File.Open(pathToDestination, FileMode.Create));

			//Create code and tree.
			string code = "";
			HuffmanTree<byte> tree = new HuffmanTree<byte>();
			writer.Write(file.HeaderData);

			//Generating the entire encoding.
			for (int i = 0; i < file.PixelData.Length; i++)
			{
				byte newSign = file.PixelData[i];
				code += tree.AddNodeCorrect(newSign);
				int progress = (int) (((float)i / file.PixelData.Length-1)*100);
				yield return progress;
			}

			int codeLengthInBytes = (code.Length / 8) + 1;
			byte[] codeArray = new byte[codeLengthInBytes];


			for (int i = 0; i < codeArray.Length; i++)
			{
				if (code.Length >= 8)
				{
					string temp = code[0..8];
					codeArray[i] = (Convert.ToByte(temp, 2));
					code = code[8..];
				}
			}

			if (code.Length != 0)
			{
				codeArray[codeArray.Length - 1] = Convert.ToByte(code, 2);

			}

			writer.Write(codeArray);
			writer.Close();
		}
	}
}