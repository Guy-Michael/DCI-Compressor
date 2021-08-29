using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DCICompressor
{
	class Program
	{
		public static void Main(String[] args)
		{
			//Socket communication.
			//IPHostEntry host = Dns.GetHostEntry("localhost");
			//IPAddress ipAddress = host.AddressList[0];
			//IPEndPoint remoteEP = new IPEndPoint(ipAddress, 80);

			//Socket sender = new Socket(ipAddress.AddressFamily,
			//SocketType.Stream, ProtocolType.Tcp);

			//sender.Connect(remoteEP);
			//byte[] bytes = { 1, 2, 3 };
			//sender.Send(bytes);

			//string path = ("C:\\Users\\Guy\\Desktop\\Compression\\ExampleInputs\\");
			//BinaryWriter writer = new BinaryWriter(File.Create(path + "new testing file.tpa"));
			//byte b = 0b0;

			//for(int i = 0; i <20000; i++)
			//{
			//	writer.Write(b);
			//}
			//Console.WriteLine("DONE!");

			string path = ("C:\\Users\\Guy\\Desktop\\Compression\\ExampleInputs\\");
			string fileName = "entropy";
			string inExtension = ".bmp";
			string outExtension = ".dci";
			Console.WriteLine("Starting encoding..");
			string code = AdaptiveHuffmanEncoder.Encode8BitCorrect(path + fileName + inExtension, path + fileName + outExtension);

			Console.WriteLine("Starting decoding..");
			byte[] result = AdaptiveHuffmanDecoder.Decode8BitBMPCorrectWithRegardsToHeader(path + fileName + outExtension, path + "New  " + fileName + inExtension);

			Console.WriteLine("Result is : " + result.Length + " long.");

		}
	}
}
