using System;
using System.Collections.Generic;
using System.IO;

namespace DCICompressor
{
	public class AdaptiveHuffmanEncoder
	{
		public static string Encode(string pathToFileToEncode, string pathToDestination)
		{
			BMPFile file = new BMPFile(pathToFileToEncode);
			string code = "";
			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			for(int i = 0; i < file.PixelData.Length; i += 3)
			{
				Index first = i, last = i + 3;
				//Console.WriteLine(i);
				uint24 newSign = new uint24(file.PixelData[first..last]);
			
				if (!tree.Contains(newSign))
				{
					tree.AddNode(newSign);
					code += tree.OutputCodeOnFirstApperace(newSign);
				}

				else
				{
					tree.AddNode(newSign);
					code += tree.OutputCode(newSign);
				}

			}

			//Console.WriteLine(code);
			return code;
		}

		public static string EncodeCorrect(string pathToFileToEncode, string pathToDestination)
		{
			BMPFile file = new BMPFile(pathToFileToEncode);
			BinaryWriter writer = new BinaryWriter(File.Open(pathToDestination,FileMode.Create));

			Console.WriteLine($"pixel data length : {file.PixelData.Length}\n header data : {file.HeaderData.Length}");
			string code = "";
			HuffmanTree<uint24> tree = new HuffmanTree<uint24>();
			for (int i = 0; i < file.PixelData.Length; i += 3)
			{

				//Console.WriteLine($"i: {i},\t {file.PixelData.Length}");

				Index first = i, last = i + 3;
				uint24 newSign = new uint24(file.PixelData[first..last]);
				if (!tree.Contains(newSign))
				{
					tree.AddNode(newSign);
					//writer.Write(tree.OutputCodeOnFirstApperace(newSign));
					code += tree.OutputCodeOnFirstApperace(newSign);
				}

				else
				{
					tree.AddNode(newSign);
					//writer.Write(newSign.ToByteArray());
					code += tree.OutputCode(newSign);
				}
			}
			code = code.Replace(" ", "");
			Console.WriteLine($"Code length : {code.Length}");
			Console.WriteLine(code);

			int codeLengthInBytes = code.Length / 8;
			byte[] codeArray = new byte[codeLengthInBytes + 1 + 54];
			
			for(int i = 0; i < file.HeaderData.Length; i++)
			{
				codeArray[i] = file.HeaderData[i];
			}

			
			for(int i = file.HeaderData.Length; i < codeLengthInBytes; i++)
			{
				string temp = code[0..8];
				//Console.WriteLine(temp);
				codeArray[i] = (Convert.ToByte(temp, 2));
				code = code[8..];
			}

			codeArray[codeArray.Length-1] = Convert.ToByte(code, 2);

			writer.Write(codeArray);
			writer.Close();
			//Console.WriteLine(code);
			return code;
		}

		public static string EncodeNonBMP(string pathToFileToEncode, string output)
		{
			HuffmanTree<byte> tree = new HuffmanTree<byte>();
			byte[] bytes = File.ReadAllBytes(pathToFileToEncode);
			string code = "";
			foreach (byte b in bytes)
			{
				char c = (char)b;

				if (!tree.Contains(b))
				{
					tree.AddNode(b);
					string currentCodeAndPath = tree.OutputCodeOnFirstApperace(b);
					code += currentCodeAndPath;
				}

				else
				{
					tree.AddNode(b);
					string currentCode = tree.OutputCode(b);
					code += currentCode;
				}
			}
			List<byte> list = new List<byte>();
			code = code.Replace(" ", "");
			while(code.Length > 8)
			{
				string temp = code[0..8];
				list.Add(Convert.ToByte(temp,2));
				code = code[8..];
			}

			list.Add(Convert.ToByte(code, 2));
			
			BinaryWriter writer = new BinaryWriter(File.Open(output, FileMode.Create));
			writer.Write(list.ToArray());
			writer.Close();
			//Console.WriteLine(code.Length/8);
			return code;
		}
	}
}
