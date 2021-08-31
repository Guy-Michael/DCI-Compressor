using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCICompressor;
using Networking;

namespace Runner
{
	public partial class CompressorGUI : Form
	{
		private string m_InputPath;
		private string m_OutputPath;

		public CompressorGUI()
		{
			InitializeComponent();

		}

		private void compress_button(object sender, EventArgs e)
		{

			string path = getBMPPathFromDialog();
			Index lastIndexOf = path.LastIndexOf('.');
			string m_PathWithoutExtension = path[0..lastIndexOf];

			m_InputPath = path;
			m_OutputPath = m_PathWithoutExtension + ".dci";

			lockButtons();
			Task.Run(() => EncodeTask(m_InputPath,m_OutputPath)).ContinueWith(t => unlockButtons()); ;
		}

		private void Display_button(object sender, EventArgs e)
		{
			
			Process proc = Process.Start(@"BMPImageDisplayerOverNetwork.exe");
		
			lockButtons();
			string path = getDCIPathFromDialog();
			progressBar1.Value = 0;
			Task.Run(() => DecodeAndDisplayTask(path)).ContinueWith(a => unlockButtons());
		}


		private void DecodeAndDisplayTask(string path)
		{		
			IEnumerator<List<byte>> decoderEnumerator = AdaptiveHuffmanDecoder.DecodeBMPOverNetwork(path);
			NetworkCommunication.OpenClientSocket();
			int prevProgress = 0, progress = 0;
			while (decoderEnumerator.MoveNext())
			{

				List<byte> b = decoderEnumerator.Current;
				if (b.Count == 1)
				{
					progress = b[0];
					if (progress > prevProgress)
					{
						progressBar1.PerformStep();
					}
					prevProgress = progress;
				}
				else
				{
					NetworkCommunication.SendDataOverNetwork(b.ToArray(), 0, b.Count);
				}
			}
		
			NetworkCommunication.CloseClientSocket();
		}

		private string getBMPPathFromDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "bmp files (*.bmp)|*.bmp";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				return dialog.FileName;
			}

			return string.Empty;
		}

		private string getDCIPathFromDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "bmp files (*.dci)|*.dci";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				return dialog.FileName;
			}

			return string.Empty;
		}

		private void EncodeTask(string i_InputPath, string i_OutputPath)
		{
			progressBar1.Value = 0;
			IEnumerator<int> encoderEnumerator = AdaptiveHuffmanEncoder.Encode8Bit(m_InputPath, m_OutputPath);

			int prevProgress = 0;
			while (encoderEnumerator.MoveNext())
			{
				int progress = encoderEnumerator.Current;
				if (progress > prevProgress)
				{
					progressBar1.PerformStep();
				}
				prevProgress = progress;
				//progressBar1.Invoke((Action)(() => progressBar1.Value = progress));
			}
		}

		private void decompress_click(object sender, EventArgs e)
		{
			progressBar1.Value = 0;
			string path = getDCIPathFromDialog();

			Index index = path.LastIndexOf('.') + 1;
			lockButtons();
			string outPath = path[0..index] + "bmp";
			Task.Run(() => decompressTask(path, outPath)).ContinueWith(a => unlockButtons());
		}

		private void decompressTask(string i_InputPath, string i_OutputPath)
		{
			IEnumerator<int> decomEnumerator = AdaptiveHuffmanDecoder.DecodeBMPAndStoreOnDisk(i_InputPath, i_OutputPath);
			int progress = 0, prevProgress = 0;
			while(decomEnumerator.MoveNext())
			{
				progress = decomEnumerator.Current;
				if (progress > prevProgress)
				{
					progressBar1.PerformStep();
				}
				prevProgress = progress;
			}
		}

		private void lockButtons()
		{
			button1.Enabled = false;
			button2.Enabled = false;
			button3.Enabled = false;
		}

		private void unlockButtons()
		{
			button1.Enabled = true;
			button2.Enabled = true;
			button3.Enabled = true;
		}
	}
}
