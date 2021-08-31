using System.Net.Sockets;
using System.Diagnostics;


namespace Networking
{
	public static class NetworkCommunication
	{
		private static TcpClient client;// = new TcpClient("127.0.0.1", 1234);
		
		public static void SendDataOverNetwork(byte[] i_Buffer, int i_Offset, int i_Size)
		{
			client.GetStream().Write(i_Buffer, i_Offset, i_Size);
		}

		public static void OpenClientSocket()
		{
			client = new TcpClient("127.0.0.1", 1234);
		}

		public static void CloseClientSocket()
		{
			client?.Close();
		}
	}
}
