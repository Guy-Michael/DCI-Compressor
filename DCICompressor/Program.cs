using System;
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


			string path = ("C:\\Users\\Guy\\Desktop\\Compression\\ExampleInputs\\");
			string fileName = "entropy";
			string inExtension = ".bmp";
			string outExtension = ".dci";
			string code = AdaptiveHuffmanEncoder.EncodeCorrect(path + fileName + inExtension, path +fileName+ outExtension);
			byte[] result = AdaptiveHuffmanDecoder.DecodeBMPCorrect(path + fileName + outExtension, path + "new"+fileName +inExtension);

		}
	}
}
