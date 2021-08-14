using System;
using System.Collections.Generic;
using System.IO;

namespace DCICompressor.Adaptive_Huffman
{
	class BMPFile
	{
		private string m_Path;
		private byte[] m_HeaderData;
		private byte[] m_PixelData;
		private uint m_Width;
		private uint m_Height;
		private uint m_BytesPerColor;

		public string Path
		{
			get { return m_Path; }
		}

		public byte[] HeaderData
		{
			get { return m_HeaderData; }
			set { m_HeaderData = value; }
		}
		public byte[] PixelData
		{
			get { return m_PixelData; }
			private set { m_PixelData = value; }
		}

		public uint Width
		{
			get { return m_Width; }
			private set { m_Width = value; }
		}

		public uint Height
		{
			get { return m_Height; }
			private set { m_Height = value; }
		}

		public uint  BytesPerColor
		{
			get { return m_BytesPerColor; }
			private set { m_BytesPerColor = value; }
		}


		public BMPFile(string path)
		{
			byte[] bytes;
			if (IsThisABMPFile(path))
			{
				bytes = File.ReadAllBytes(path);
				uint pixelDataOffset = BitConverter.ToUInt32(bytes, 0xa);

				Index pixelOffset = (int)(pixelDataOffset-1);
				HeaderData = bytes[0..pixelOffset];
				PixelData = bytes[pixelOffset..];

				Width = BitConverter.ToUInt32(bytes, 0x12);
				Height = BitConverter.ToUInt32(bytes, 0x16);
				BytesPerColor = BitConverter.ToUInt16(bytes, 0x1c);
			}

			else
			{
				Console.WriteLine("Not a BMP FILE!");
			}
			
		}

		private bool IsThisABMPFile(string path)
		{
			byte[] authenticationBytes = new byte[2];
			Array.Copy(File.ReadAllBytes(path), authenticationBytes, 2);
			char firstChar = (char)authenticationBytes[0];
			char secondChar = (char)authenticationBytes[1];

			return (firstChar == 'B' && secondChar == 'M');

		}
	}
}