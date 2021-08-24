using System;
using System.Collections.Generic;

namespace DCICompressor
{
	struct uint24 : IComparable<uint24>
	{
		byte b0;
		byte b1;
		byte b2;
		private uint uintValue;

		public uint24(byte[] bytes)
		{
			if (bytes.Length != 3)
			{
				throw new ArgumentException("Array is not 3 bytes long!");
			}

			b0 = bytes[0];
			b1 = bytes[1];
			b2 = bytes[2];
			uintValue = (uint)(b0 << 16 | b1 << 8 | b2);
		}

		public uint24(uint number)
		{
			if (Math.Round(Math.Log2(number + 1)) > 24)
			{
				throw new ArgumentException("number is more than 24 bits long!");
			}

			b0 = (byte)(number >> 16);
			b1 = (byte)((number & 0x00ff00) >> 8);
			b2 = (byte)(number & 0xff);
			uintValue = (uint)(b0 << 16 | b1 << 8 | b2);
		}
		public uint ToUInt()
		{
			return uintValue;
			//if (bytes.Length != 3)
			//{
			//	throw new ArgumentException("array is not 3 bytes long!");
			//}

			//return (uint)(b0<<16 | b1 << 8 | b2);
		}

		public byte[] ToByteArray()
		{
			byte[] bytes = new byte[3];
			bytes[0] = b0;
			bytes[1] = b1;
			bytes[2] = b2;
			//bytes[0] = (byte) (number >> 16);
			//bytes[1] = (byte) ((number & 0x00ff00)>>8);
			//bytes[2] = (byte) (number & 0xff);
			return bytes;
		}

		public int CompareTo(uint24 other)
		{
			return Math.Sign(uintValue - other.uintValue);
		}

		public override string ToString()
		{
			//return "hello";
			return (Convert.ToString(b0, 2) + Convert.ToString(b1, 2) + Convert.ToString(b2, 2));
		}

		public static string ToBinaryString(uint24 num)
		{
			string value = Convert.ToString(num.uintValue, 2);
			int padding = 24 - value.Length;
			return new string('0', padding) + value;

			//return Convert.ToString(num.uintValue, 2);
		}

		public static uint24 TryParse(string num)
		{
			foreach(char c in num)
			{
				if (!char.IsDigit(c))
				{
					throw new ArgumentException("not a numbe!");
				}
			}

			//if (num.Length < 24)
			//{
			//	int padding = 24 - (num.Length);
			//	num = new string('0', padding) + num;
				
			//}
			uint value =(uint)  Convert.ToUInt32(num,2);
			return new uint24(value);
		}

		//public static uint24 Parse(string num)
		//{

		//}
	}
}
