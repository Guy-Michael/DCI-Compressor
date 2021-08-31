using System;
using System.Collections.Generic;

namespace DCICompressor
{
	class HuffmanTree<T> where T : IComparable<T>
	{
		//This is the maximum number unique symbols.
		private uint s_Counter =  (uint)((Math.Pow(2, 8) * 2) + 1);
		private HuffNode<T> m_Root;
	
		public HuffNode<T> Root
		{
			get { return m_Root; }
			set { m_Root = value; }
		}

		public HuffmanTree()
		{
			Root = new HuffNode<T>();
			Root.IsNYT = true;
			Root.Identifier = s_Counter--;
		}

		private HuffNode<T> LocateNodeByIdentifier(uint i_Identifier)
		{
			HuffNode<T> node = locate(Root, i_Identifier);

			return node;
			HuffNode<T> locate(HuffNode<T> i_Node, uint i_Identifier)
			{
				if (i_Node == null)
				{
					return null;
				}

				if (i_Node.Identifier == i_Identifier)
				{
					return i_Node;
				}

				HuffNode<T> Result = null;

				if (i_Node.RightChild != null)
				{
					Result = locate(i_Node.RightChild, i_Identifier);
				}

				if (Result == null && i_Node.LeftChild != null)
				{
					Result = locate(i_Node.LeftChild, i_Identifier);
				}
				return Result;
			}
		}

		public HuffNode<T> locateNodeBySymbol(T i_Symbol)
		{
			HuffNode<T> node = locate(Root, i_Symbol);

			return node;
			HuffNode<T> locate(HuffNode<T> i_Node, T i_Symbol)
			{
				if(i_Node.IsLeaf() && i_Node.Frequency != 0)
				{
					if (i_Node.Value.CompareTo(i_Symbol) == 0)
					{
						return i_Node;
					}
				}
				

				HuffNode<T> Result = null;

				if (i_Node.RightChild != null)
				{
					Result = locate(i_Node.RightChild, i_Symbol);
				}

				if (Result == null && i_Node.LeftChild != null)
				{
					Result = locate(i_Node.LeftChild, i_Symbol);
				}
				return Result;
			}
		}

		public string AddNodeCorrect(T i_NewSign)
		{
			string code = string.Empty;
			HuffNode<T> currentNode = locateNodeBySymbol(i_NewSign);

			if (currentNode == null)
			{
				HuffNode<T> prevNYT = LocateNodeByIdentifier(s_Counter + 1);
				prevNYT.IsNYT = false;
				currentNode = new HuffNode<T>(i_NewSign);
				currentNode.Identifier = s_Counter--;

				HuffNode<T> newNYT = new HuffNode<T>();
				newNYT.Identifier = s_Counter--;
				newNYT.IsNYT = true;

				prevNYT.RightChild = currentNode;
				prevNYT.LeftChild = newNYT;

				prevNYT.RightChild.Parent = prevNYT;
				prevNYT.LeftChild.Parent = prevNYT;

				code = OutputCode(i_NewSign, true);
				IncrementNode(prevNYT);
				currentNode = currentNode.Parent;
			}
			else
			{
				code = OutputCode(i_NewSign, false);
			}

			while(currentNode != Root)
			{
				currentNode = currentNode.Parent;
				HuffNode<T> biggest = GetBiggestIdentifierInBlock(currentNode);

				if (currentNode != biggest && biggest != null)
				{
					swapNodes(currentNode, biggest);
					IncrementNode(biggest);
					currentNode = biggest;
				}

				else
				{
					IncrementNode(currentNode);
				}
			}
			return code;
		}

		public HuffNode<T> GetBiggestIdentifierInBlock(HuffNode<T> i_NodeToCheck)
		{
			List<HuffNode<T>> list = new List<HuffNode<T>>();
			locate(Root, i_NodeToCheck);

			if (list.Count == 0)
			{
				return null;
			}

			HuffNode<T> max = list[0];
			foreach (HuffNode<T> node in list)
			{
				if (node.Identifier > max.Identifier)
				{
					max = node;
				}
			}

			return max;

			void locate(HuffNode<T> i_Node, HuffNode<T> i_NodeToCheck)
			{
				if (i_Node == null)
				{
					return;
				}

				if (i_Node.Frequency == i_NodeToCheck.Frequency && i_NodeToCheck.Parent != i_Node)
				{
					list.Add(i_Node);
				}

				if (i_Node.RightChild != null)
				{
					locate(i_Node.RightChild, i_NodeToCheck);
				}

				if (i_Node.LeftChild != null)
				{
					locate(i_Node.LeftChild, i_NodeToCheck);
				}
			}
		}

		public string OutputCode(T sign, bool i_IsFirstApperance)
		{
			string code = string.Empty;
			return outputCode(Root, sign, code, i_IsFirstApperance);

			string outputCode(HuffNode<T> i_Node, T i_Sign, string i_Code, bool i_IsFirstApperace)
			{
				if (i_Node == null)
				{
					return string.Empty;
				}
				if (i_Node.IsLeaf() && !i_Node.IsNYT)
				{
					if (i_Node.Value.CompareTo(i_Sign) == 0)
					{

						string binary = "";
						if (i_Sign is byte)
						{ 
							string signAsString = i_Sign.ToString();
							byte signValue = Byte.Parse(signAsString);
							binary = Convert.ToString(signValue, 2);

							if (binary.Length < 8)
							{
								binary = new string('0', 8 - binary.Length) + binary;
							}
						}
						if (i_IsFirstApperace)
						{
							return i_Code + binary;
						}

						else
						{
							return i_Code;
						}
					}
					return string.Empty;
				}

				string left = outputCode(i_Node.LeftChild, i_Sign, i_Code + "0", i_IsFirstApperace);
				string right = outputCode(i_Node.RightChild, i_Sign, i_Code + "1", i_IsFirstApperace);
				return left + right;
			}
		}

		private void IncrementNode(HuffNode<T> i_Node)
		{
			i_Node.Frequency++;
		}
	
		private void swapNodes(HuffNode<T> i_Node1, HuffNode<T> i_Node2)
		{
			if (i_Node1 == null || i_Node2 == null)
			{
				return;
			}

			if (i_Node1.Parent == null || i_Node2.Parent == null)
			{
				return;
			}

			uint id1 = i_Node1.Identifier;
			uint id2 = i_Node2.Identifier;

			i_Node1.Identifier = id2;
			i_Node2.Identifier = id1;

			if (i_Node1.IsRightChild() && i_Node2.IsRightChild())
			{
				i_Node1.Parent.RightChild = i_Node2;
				i_Node2.Parent.RightChild = i_Node1;
			}

			else if (i_Node1.IsRightChild() && i_Node2.IsLeftChild())
			{
				i_Node1.Parent.RightChild = i_Node2;
				i_Node2.Parent.LeftChild = i_Node1;
			}

			else if (i_Node1.IsLeftChild() && i_Node2.IsLeftChild())
			{
				i_Node1.Parent.LeftChild = i_Node2;
				i_Node2.Parent.LeftChild = i_Node1;
			}

			else if (i_Node1.IsLeftChild() && i_Node2.IsRightChild())
			{
				i_Node1.Parent.LeftChild = i_Node2;
				i_Node2.Parent.RightChild = i_Node1;
			}

			HuffNode<T> tempParent = i_Node1.Parent;
			i_Node1.Parent = i_Node2.Parent;
			i_Node2.Parent = tempParent;
			
		}

	}
}