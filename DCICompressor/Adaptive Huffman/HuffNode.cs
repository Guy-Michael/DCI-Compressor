using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCICompressor
{
	class HuffNode<T>
	{
		private HuffNode<T> m_LeftChild;
		private HuffNode<T> m_RightChild;
		private HuffNode<T> m_Parent;
		private uint m_Identifier;
		private T m_Value;
		private int m_Frequency;

		public HuffNode(T value, int frequency)
		{
			Value = value;
			Frequency = frequency;
		}
		public HuffNode(T value)
		{
			LeftChild = null;
			RightChild = null;
			Value = value;
			Frequency = 1;
		}

		public HuffNode()
		{
			LeftChild = null;
			RightChild = null;
			Value = default(T);
			Frequency = 0;
		}


		public HuffNode<T> LeftChild
		{
			get { return m_LeftChild; }
			set 
			{ 
				m_LeftChild = value;
				//reCalcFrequency();
			}
		}

		public HuffNode<T> RightChild
		{
			get { return m_RightChild; }
			set
			{
				m_RightChild = value;
				//reCalcFrequency();
			}
		}

		public HuffNode<T> Parent
		{
			get { return m_Parent; }
			set { m_Parent = value; }
		}

		public uint Identifier
		{
			get { return m_Identifier; }
			set { m_Identifier = value; }
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

		private void reCalcFrequency()
		{
			Frequency = 0;
			if (LeftChild != null)
			{
				Frequency += LeftChild.Frequency;
			}

			if (RightChild != null)
			{
				Frequency += RightChild.Frequency;
			}
		}
		public bool IsLeaf()
		{
			return (LeftChild == null && RightChild == null);
		}

		public void SwapChildren()
		{
			HuffNode<T> tempNode = LeftChild;
			LeftChild = RightChild;
			RightChild = tempNode;
		}

		public bool IsSibling(HuffNode<T> other)
		{
			if (other == null)
				return false;
			return (Parent.Equals(other.Parent));
		}

		internal void SwapChildrenKeepIDs()
		{
			uint leftIdentifier = LeftChild.Identifier;
			uint rightIdentifier = RightChild.Identifier;
			
			HuffNode<T> tempNode = LeftChild;
			LeftChild = RightChild;
			RightChild = tempNode;

			LeftChild.Identifier = leftIdentifier;
			RightChild.Identifier = rightIdentifier;
		}

		//public int CompareTo(HuffNode<T> other)
		//{
		//	int result = 0;
		//	if (other == null)
		//	{
		//		result = 1;
		//	}

		//	else if (other.Frequency == Frequency)
		//	{
		//		result = Math.Sign(Identifier - other.Identifier);
		//	}

		//	else
		//	{

		//	}

		//	return result;
		//}
	}
}
