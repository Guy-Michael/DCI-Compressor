using System;
using System.Collections.Generic;
using System.IO;

namespace DCICompressor
{
	class BMPFile
	{
		private string m_Path;
		private byte[] m_HeaderData;
		private byte[] m_PixelData;
		private int m_Width;
		private int m_Height;
		private int m_BytesPerColor;
		private int m_PaddingCountPerRow;

		public string Path
		{
			get { return m_Path; }
			private set { m_Path = value; }
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
		public int Width
		{
			get { return m_Width; }
			private set { m_Width = value; }
		}
		public int Height
		{
			get { return m_Height; }
			private set { m_Height = value; }
		}
		public int BytesPerColor
		{
			get { return m_BytesPerColor; }
			private set { m_BytesPerColor = value; }
		}
		public int PaddingCountPerRow
		{
			get { return m_PaddingCountPerRow; }
			private set { m_PaddingCountPerRow = value; }
		}

		public BMPFile(string path)
		{
			Path = path;
			byte[] bytes;
			if (IsThisABMPFile(path))
			{
				bytes = File.ReadAllBytes(path);
				uint pixelDataOffset = BitConverter.ToUInt32(bytes, 0xa);

				Index pixelOffset = (int)(pixelDataOffset);
				HeaderData = bytes[0..pixelOffset];
				PixelData = bytes[pixelOffset..];

				Width = BitConverter.ToInt32(bytes, 0x12);
				Height = BitConverter.ToInt32(bytes, 0x16);
				BytesPerColor = BitConverter.ToUInt16(bytes, 0x1c);
				PaddingCountPerRow = (Width * 3) % 4;
			}

			else
			{
				Console.WriteLine("Not a BMP FILE!");
			}
			
		}
	
		public BMPFile(byte[] data)
		{

			if (IsThisABMPFile(data))
			{
				uint pixelDataOffset = BitConverter.ToUInt32(data, 0xa);

				Index pixelOffset = (int)(pixelDataOffset);
				HeaderData = data[0..pixelOffset];
				PixelData = data[pixelOffset..];

				Width = BitConverter.ToInt32(data, 0x12);
				Height = BitConverter.ToInt32(data, 0x16);
				BytesPerColor = BitConverter.ToUInt16(data, 0x1c);
			}

			else
			{
				Console.WriteLine("Not a BMP FILE!");
			}
		}

		private bool IsThisABMPFile(string path)
		{
			byte[] authenticationBytes = File.ReadAllBytes(path)[0..2];
			char firstChar = (char)authenticationBytes[0];
			char secondChar = (char)authenticationBytes[1];

			return (firstChar == 'B' && secondChar == 'M');

		}

		private bool IsThisABMPFile(byte[] data)
		{
			byte[] authenticationBytes = data[0..2];
			char firstChar = (char)authenticationBytes[0];
			char secondChar = (char)authenticationBytes[1];

			return (firstChar == 'B' && secondChar == 'M');

		}
	}
}