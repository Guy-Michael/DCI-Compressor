using System;
using System.Collections.Generic;

namespace DCICompressor
{
	class HuffmanTree<T> where T : IComparable<T>
	{
		//This is the maximum number unique symbols.
		private uint s_Counter = (uint)((Math.Pow(2, 24) * 2) + 1);
		private HuffNode<T> m_Root;
		public HuffNode<T> Root
		{
			get { return m_Root; }
			set { m_Root = value; }
		}

		private HuffNode<T> m_NullNode;
		private HuffNode<T> NullNode
		{
			get { return m_NullNode; }
			set { m_NullNode = value; }
		}

		public HuffmanTree()
		{
			NullNode = new HuffNode<T>();
			Root = NullNode;
			Root.Identifier = s_Counter--;
		}

		public bool Contains(T sign)
		{
			return locateNodeBySymbol(sign) != null;
			//if (Root.Value.CompareTo(sign) == 0)
			//{
			//	return true;
			//}

			//else
			//{
			//	return NodeContains(Root.LeftChild, sign) || NodeContains(Root.RightChild, sign);
			//}

			//bool NodeContains(HuffNode<T> node, T sign)
			//{
			//	if (node == null)
			//	{
			//		return false;
			//	}

			//	if (node.Value.CompareTo(sign) == 0)
			//	{
			//		return true;
			//	}

			//	{
			//		return NodeContains(node.RightChild, sign) || NodeContains(node.LeftChild, sign);
			//	}
			//}
		}

		private HuffNode<T> LocateNodeByIdentifier(uint identifier)
		{
			HuffNode<T> node = locate(Root, identifier);

			return node;
			HuffNode<T> locate(HuffNode<T> node, uint identifier)
			{
				if (node.Identifier == identifier)
				{
					return node;
				}

				HuffNode<T> Result = null;

				if (node.RightChild != null)
				{
					Result = locate(node.RightChild, identifier);
				}

				if (Result == null && node.LeftChild != null)
				{
					Result = locate(node.LeftChild, identifier);
				}
				return Result;
			}
		}

		private HuffNode<T> locateNodeBySymbol(T symbol)
		{
			HuffNode<T> node = locate(Root, symbol);

			return node;
			HuffNode<T> locate(HuffNode<T> node, T symbol)
			{
				if (node.Value.CompareTo(symbol) == 0)
				{
					return node;
				}

				HuffNode<T> Result = null;

				if (node.RightChild != null)
				{
					Result = locate(node.RightChild, symbol);
				}

				if (Result == null && node.LeftChild != null)
				{
					Result = locate(node.LeftChild, symbol);
				}
				return Result;
			}
		}

		public void AddNode(T newSign)
		{
			string code = string.Empty;
			Root.Frequency++;
			HuffNode<T> currentNode = locateNodeBySymbol(newSign);
			if (currentNode == null)
			{
				HuffNode<T> prevNull = NullNode;


				HuffNode<T> signNode = new HuffNode<T>(newSign); // new right child.
				signNode.Identifier = s_Counter--;
				signNode.Parent = prevNull;

				HuffNode<T> newNYT = new HuffNode<T>(); // new left child;
				newNYT.Identifier = s_Counter--;
				newNYT.Parent = prevNull;

				prevNull.LeftChild = newNYT;
				prevNull.RightChild = signNode;

				NullNode = newNYT;
				currentNode = signNode;
				//prevNull.Frequency++;

				//code = outputCodeOf(newSign);
			}

			else
			{
				currentNode.Frequency++;
			}

				while (currentNode != Root)
				{
					uint num = currentNode.Identifier;
					HuffNode<T> temp = LocateNodeByIdentifier(num+1);
					while(currentNode.Frequency == temp.Frequency+1 && !temp.Equals(Root))
					{
						num++;
						temp = LocateNodeByIdentifier(num);
					}

					if (num != currentNode.Identifier)
					{
						HuffNode<T> targetNode = temp;
						if (currentNode.IsSibling(targetNode))
						{
							currentNode.Parent.SwapChildrenKeepIDs();
						}

						else
						{
							HuffNode<T> targetParent = targetNode.Parent;
							HuffNode<T> currentParent = currentNode.Parent;

							//Console.WriteLine($"targer parent : {targetParent == null} \t currentParent: {currentParent == null}");
							//Console.WriteLine($"currentValue { char.Parse(currentNode.Frequency.ToString())}");
							
							if (targetParent.LeftChild.Equals(targetNode))
							{
								targetParent.LeftChild = currentNode;
							}

							if (targetParent.RightChild.Equals(targetNode))
							{
								targetParent.RightChild = currentNode;
							}

							if (currentParent.LeftChild.Equals(currentNode))
							{
								currentParent.LeftChild = targetNode;
							}

							if (currentParent.RightChild.Equals(currentNode))
							{
								currentParent.RightChild = targetNode;
							}
						}

						//code = outputCodeOf(newSign);
					}
					//currentNode.Frequency++;

					currentNode = currentNode.Parent;
				}

		}

		private void incrementParentChain(HuffNode<T> node)
		{
			if (node != null && node.Parent != null)
			{
				node.Parent.Frequency++;
				incrementParentChain(node.Parent);
			}
		}

		public string OutputCode(T sign)
		{
			string code = string.Empty;
			return outputCode(Root, sign, code);

			string outputCode(HuffNode<T> node, T sign, string code)
			{
				if (node.IsLeaf())
				{
					if (node.Value.CompareTo(sign) == 0)
					{ 
						//string binary = "";
						//if (sign is byte)
						//{
						//	binary = Convert.ToString(Byte.Parse(sign.ToString()), 2);
						//	if (binary.Length < 8)
						//	{
						//		binary = new string('0', 8 - (binary.Length)) + binary;
						//	}
						//}

						//else if (sign is uint24)
						//{
						//	string temp = sign.ToString();
						//	uint24 num = uint24.TryParse(temp);
						//	binary = uint24.ToBinaryString(num);

						//}
						//Console.WriteLine($"code is{code}\t binary is {binary}");
						return code;
					}
					return string.Empty;
				}
				string left = outputCode(node.LeftChild, sign, code + "0");
				string right = outputCode(node.RightChild, sign, code + "1");

				return left + right;
			}
		}

		public string OutputCodeOnFirstApperace(T sign)
		{
			string code = string.Empty;
			return outputCode(Root, sign, code);

			string outputCode(HuffNode<T> node, T sign, string code)
			{
				if (node.IsLeaf())
				{
					if (node.Value.CompareTo(sign) == 0)
					{

						string binary = "";
						if (sign is byte)
						{
							binary = Convert.ToString(Byte.Parse(sign.ToString()), 2);
							if (binary.Length < 8)
							{
								binary = new string('0', 8 - (binary.Length)) + binary;
							}
						}

						else if (sign is uint24)
						{
							string temp = sign.ToString();
							uint24 num = uint24.TryParse(temp);
							binary = uint24.ToBinaryString(num);
						}
						//Console.WriteLine($"code is{code}\t binary is {binary}");
						return code + " " + binary;
					}
					return string.Empty;
				}
				string left = outputCode(node.LeftChild, sign, code + "0");
				string right = outputCode(node.RightChild, sign, code + "1");

				return left + right;
			}
		}

		public void FindAndIncrementNodeWithSign(T newSign)
		{
			HuffNode<T> tempNode = Root;
			if (tempNode.Value.CompareTo(newSign) == 0)
			{
				tempNode.Frequency++;
			}
			else
			{
				FindNodeWithVal(Root.LeftChild, newSign);
				FindNodeWithVal(Root.RightChild, newSign);
			}
		}

		private void FindNodeWithVal(HuffNode<T> node, T newSign)
		{
			if (node != null)
			{
				if (node.Value.CompareTo(newSign) == 0)
				{
					IncrementNode(node);
				}

				else
				{
					FindNodeWithVal(node.LeftChild, newSign);
					FindNodeWithVal(node.RightChild, newSign);
				}
			}
		}
		private void IncrementNode(HuffNode<T> node)
		{
			node.Frequency++;
		}
	}
}