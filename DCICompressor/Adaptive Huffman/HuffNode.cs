using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCICompressor.Adaptive_Huffman
{
	class HuffNode<T>
	{
		private HuffNode<T> m_LeftChild;
		private HuffNode<T> m_RightChild;
		private T m_Value;
		private int m_Frequency;
		public HuffNode<T> LeftChild
		{
			get { return m_LeftChild; }
			set { m_LeftChild = value; }
		}

		public HuffNode<T> RightChild
		{
			get { return m_RightChild; }
			set { m_RightChild = value; }
		}

		public T Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		public int Frequency
		{
			get { return m_Frequency; }
			set { m_Frequency = value; }
		}

		public bool IsLeaf()
		{
			return (LeftChild == null && RightChild == null);
		}

	}
}
