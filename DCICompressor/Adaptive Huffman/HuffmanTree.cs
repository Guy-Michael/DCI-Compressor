using System;
using System.Collections.Generic;

namespace DCICompressor.Adaptive_Huffman
{
	class HuffmanTree<T> where T : IComparable<T>
	{
		//This is the maximum number unique symbols.
		private uint s_Counter = (uint) ((Math.Pow(2, 24) * 2) +1);
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
		}

		public bool Contains(T sign)
		{
			if (Root.Value.CompareTo(sign) == 0)
			{
				return true;
			}

			else
			{
				return NodeContains(Root.LeftChild, sign) || NodeContains(Root.RightChild, sign);
			}

			bool NodeContains(HuffNode<T> node, T sign)
			{
				if (node.Value.CompareTo(sign) == 0)
				{
					return true;
				}

				else
				{
					return NodeContains(node.RightChild, sign) || NodeContains(node.LeftChild, sign);
				}
			}
		}

		private void enforceInvariants(HuffNode<T> node)
		{
			//Enforce Sibling invariant
			if (!node.Equals(node.Parent.RightChild))
			{
				if (node.Frequency > node.Parent.RightChild.Frequency)
				{
					node.Parent.SwapChildren();
				}
			}

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

		public void AddNodeAndOutputCode(T newSign)
		{
			HuffNode<T> currentNode = locateNodeBySymbol(newSign);
			if (currentNode == null)
			{
				HuffNode<T> prevNull = NullNode;
				prevNull.Frequency++;

				HuffNode<T> newNYT = new HuffNode<T>(); // new left child;
				newNYT.Identifier = s_Counter--;
				newNYT.Parent = prevNull;

				HuffNode<T> signNode = new HuffNode<T>(newSign); // new right child.
				signNode.Identifier = s_Counter--;
				signNode.Parent = prevNull;

				prevNull.LeftChild = newNYT;
				prevNull.RightChild = signNode;
			}

			else
			{ 
				while(currentNode != Root)
				{
					uint num = currentNode.Identifier;
					HuffNode<T> nextNode;
					do
					{
						nextNode = LocateNodeByIdentifier(num + 1);
						if (nextNode == null)
						{
							break;
						}
						num++;
					}
					while (currentNode.Frequency == nextNode.Frequency + 1);

					if (num != currentNode.Identifier)
					{
						HuffNode<T> targetNode = LocateNodeByIdentifier(num);

						if (currentNode.IsSibling(targetNode))
						{
							currentNode.Parent.SwapChildrenKeepIDs();
						}

						else
						{
							HuffNode<T> targetParent = targetNode.Parent;
							HuffNode<T> currentParent = currentNode.Parent;

							if (targetParent.LeftChild.Equals(targetNode))
							{
								targetParent.LeftChild = currentNode;
							}

							else if (targetParent.RightChild.Equals(targetNode))
							{
								targetParent.RightChild = currentNode;
							}

							if (currentParent.LeftChild.Equals(currentNode))
							{
								currentParent.LeftChild = targetNode;
							}

							else if (currentParent.RightChild.Equals(currentNode))
							{
								currentParent.RightChild = targetNode;
							}
						}
					}
					currentNode = currentNode.Parent;
					currentNode.Frequency++;
				}
			
			
			
			
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

		//private void findMaxByFrequency(uint frequency)
		//{


		//	int find(HuffNode<T> nodeToCheck, HuffNode<T> currentMax, uint frequency)
		//	{
		//		if (nodeToCheck.Frequency == frequency)
		//		{
		//			if (nodeToCheck.Identifier > currentMax.Identifier)
		//			{
		//				currentMax = nodeToCheck;
		//			}
		//		}

		//		//if (nodeToCheck.Frequency < frequency)
		//		//{
		//		//	return null;
		//		//}

		//		//if (nodeToCheck.Frequency == frequency)
		//		//{
		//		//	if (nodeToCheck.Identifier > currentMax.Identifier)
		//		//	{
		//		//		currentMax = nodeToCheck;
		//		//	}
		//		//}
		//		//HuffNode<T> leftMax;
		//		//HuffNode<T> rightMax;

		//		//if (!nodeToCheck.IsLeaf())
		//		//{
		//		//	leftMax = find(nodeToCheck.LeftChild, currentMax, frequency);
		//		//	rightMax = find(nodeToCheck.RightChild, currentMax, frequency);
		//		//}
		//	}
		//}
	}

	//private HuffNode<T> findAnomaly(HuffNode<T> node, T valToCompare)
	//{
	//	if (node.IsLeaf())
	//	{
	//		if (node.Frequency.CompareTo(valToCompare) < 0)
	//		{
	//			return node;
	//		}
	//	}

	//	else
	//	{

	//	}
	//}




}