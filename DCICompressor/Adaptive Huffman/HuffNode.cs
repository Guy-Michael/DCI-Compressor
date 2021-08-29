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
		private bool m_IsNYT;

		public HuffNode(T i_Value)
		{
			Value = i_Value;
			Frequency = 1;
		}

		public HuffNode()
		{
			Value = default(T);
			Frequency = 0;
		}

		public HuffNode<T> LeftChild
		{
			get { return m_LeftChild; }
			set 
			{ 
				m_LeftChild = value;
			}
		}

		public HuffNode<T> RightChild
		{
			get { return m_RightChild; }
			set
			{
				m_RightChild = value;
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

		public bool IsNYT
		{
			get { return m_IsNYT; }
			set { m_IsNYT = value; }
		}

		public bool IsLeaf()
		{
			return (LeftChild == null && RightChild == null);
		}

		internal bool IsLeftChild()
		{
			if (Parent == null)
			{
				return false;
			}

			return Parent.LeftChild == this;
		}

		internal bool IsRightChild()
		{
			if (Parent == null)
			{
				return false;
			}

			return Parent.RightChild == this;
		}

		public override string ToString()
		{
			string value = string.Empty;
			if (Value is byte)
			{
				value = value.ToString();
				if (value.Length < 8)
				{
					value = new string('0', 8 - value.Length) + value;
				}
			}

			return value;
		}
	}
}
