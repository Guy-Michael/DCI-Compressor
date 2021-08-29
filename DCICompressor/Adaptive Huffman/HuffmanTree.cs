using System;
using System.Collections.Generic;

namespace DCICompressor
{
	class HuffmanTree<T> where T : IComparable<T>
	{
		//This is the maximum number unique symbols.
		private uint s_Counter =  (uint)((Math.Pow(2, 24) * 2) + 1);
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
				if (node == null)
				{
					return null;
				}

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

		/*
		 * Sections:
		 * 1.Determine if the current value exists in the tree or not.
		 * 2.Add it accordingly.
		 * 3.Determine whether the newly added\incremented node violates the sibling invariant:
		 *	3.1  if the frequency of "current" bigger than the frequency of the next in line?
		 *	3.2 if so, find the biggest node with the same frequency of the next in line.
		 *	3.3 swap current node with the biggest in block.
		 * 4.increment parent of the current node.
		 * 
		 * Things to keep in mind:
		 * 1.Before incrementing the parent, make sure you are incrementing the correct parent.
		 * 2.Explicitly assign parent and children.
		 */


		public string AddNodeCorrect(T newSign)
		{
			string code = string.Empty;
			HuffNode<T> currentNode = locateNodeBySymbol(newSign);
			//1.Determing if the current value already exists.
			if (currentNode == null)
			{
				//****************PREV NYT IS NULL!!!!!!****************************//
				//Console.WriteLine($"Looking for identifier {s_Counter + 1}");
				HuffNode<T> prevNYT = LocateNodeByIdentifier(s_Counter + 1);

				currentNode = new HuffNode<T>(newSign);
				currentNode.Identifier = s_Counter--;

				HuffNode<T> newNYT = new HuffNode<T>();
				newNYT.Identifier = s_Counter--;

				prevNYT.RightChild = currentNode;
				prevNYT.LeftChild = newNYT;

				prevNYT.RightChild.Parent = prevNYT;
				prevNYT.LeftChild.Parent = prevNYT;

				code = OutputCodeOnFirstApperace(newSign);
				//prevNYT.Frequency++;

				//HuffNode<T> prevNYT = NullNode;

				//prevNYT.RightChild = currentNode;
				//prevNYT.LeftChild = newNYT;

				////Explicitly set the null node as parent.
				//prevNYT.RightChild.Parent = prevNYT;
				//prevNYT.LeftChild.Parent = prevNYT;

				//NullNode = prevNYT.LeftChild;
				//currentNode.Frequency++;
			}
			else
			{
				code = OutputCode(newSign);
				currentNode.Frequency++;
			}

			//3. Determine if "current" violates the sibling invariant.
			
			while(currentNode != Root)
			{
				currentNode.Parent.Frequency++;
				HuffNode<T> nextInLine = LocateNodeByIdentifier(currentNode.Identifier + 1);
				if (nextInLine == null || nextInLine == Root)
				{
				}

				else if (nextInLine.Frequency < currentNode.Frequency)
				{
					HuffNode<T> biggest = GetHighestIdentifierByFrequency(nextInLine.Frequency);
					swapNodes(currentNode, biggest);


					//if (biggest.IsLeftChild())
					//{
					//	biggest.Parent.LeftChild = currentNode;
					//	currentNode.Parent = biggest.Parent;
					//}

					//else if (biggest.IsRightChild())
					//{
					//	biggest.Parent.RightChild = currentNode;
					//	currentNode.Parent = biggest.Parent;
					//}

					//if (currentNode.IsLeftChild())
					//{
					//	currentNode.Parent.LeftChild = biggest;
					//	biggest.Parent = currentNode.Parent;
					//}

					//else if (currentNode.IsRightChild())
					//{
					//	currentNode.Parent.RightChild = biggest;
					//	biggest.Parent = currentNode.Parent;
					//}

					biggest.Parent.Frequency++;
					currentNode = biggest.Parent;
					continue;
				}
		
				currentNode = currentNode.Parent;
				
				//3.1 Finding the biggest node in the current block.

				//HuffNode<T> maxInBlock = GetHighestIdentifierByFrequency()
				//HuffNode<T> biggestInCurrentBlock = nextInLine;
				//int blockFrequency = biggestInCurrentBlock.Frequency;
				//while(blockFrequency == currentNode.Frequency + 1)
				//{
				//	//nextInLine = LocateNodeByIdentifier()
				//}



			}
			return code;
			//Root.Frequency++;
		}

		public HuffNode<T> GetHighestIdentifierByFrequency(int frequency)
		{
			List<HuffNode<T>> list = new List<HuffNode<T>>();
			locate(Root, frequency);

			if (list.Count == 0)
			{
				return null;
			}

			HuffNode<T> max = list[0];
			foreach(HuffNode<T> node in list)
			{
				//Console.WriteLine($"frequency: {node.Frequency}\t id: {node.Identifier}");
				if (node.Identifier > max.Identifier)
				{
					max = node;
				}
			}

			return max;

			void locate(HuffNode<T> node, int frequency)
			{
				if (node == null)
				{
					return;
				}

				if (node.Frequency == frequency)
				{
					 list.Add(node);
				}

				if (node.RightChild != null)
				{
					locate(node.RightChild, frequency);
				}

				if (node.LeftChild != null)
				{
					locate(node.LeftChild, frequency);
				}
			}
		}
	
		public void AddNode(T newSign)
		{
			string code = string.Empty;

			HuffNode<T> currentNode = locateNodeBySymbol(newSign);
			if (currentNode == null)
			{
				currentNode = new HuffNode<T>(newSign); // This is new right.
				currentNode.Identifier = s_Counter--;

				HuffNode<T> newNYT = new HuffNode<T>(); // this is new left.
				newNYT.Identifier = s_Counter--;

				NullNode.RightChild = currentNode;
				NullNode.LeftChild = newNYT;

				currentNode.Parent = NullNode;
				newNYT.Parent = NullNode;

				NullNode = NullNode.LeftChild;
			}

			else
			{
				//Console.WriteLine("Incrementing " + newSign);
				//currentNode.Frequency++;
			}

			//incrementParentChain(currentNode);
			while (!currentNode.Equals(Root))
			{
				currentNode.Frequency++;
				uint num = currentNode.Identifier;
				HuffNode<T> BiggestInBlockNode = LocateNodeByIdentifier(num+1);
				int currentFrequency = currentNode.Frequency;
				int biggetFrequencyInBlock = BiggestInBlockNode.Frequency;
				while(currentFrequency == BiggestInBlockNode.Frequency + 1 && !Root.Equals(LocateNodeByIdentifier(num+1)))
				{
					num++;
					if (LocateNodeByIdentifier(num) != null)
					{
						BiggestInBlockNode = LocateNodeByIdentifier(num);
						biggetFrequencyInBlock = BiggestInBlockNode.Frequency;
					}
				}

				if (num != currentNode.Identifier)
				{
					if (currentNode.IsSibling(BiggestInBlockNode))
					{
						currentNode.Parent.SwapChildrenKeepIDs();
					}

					else
					{

						swapNodes(currentNode, BiggestInBlockNode);
						//HuffNode<T> BiggestInBlockParent = BiggestInBlockNode.Parent;
						//HuffNode<T> currentParent = currentNode.Parent;
					}
				}
				currentNode.Frequency++;

				if (currentNode.Parent.Equals(Root))
				{
					break;
				}
				currentNode = currentNode.Parent;
			}
			Root.Frequency++;

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
						Console.WriteLine(sign);
						string signAsString = sign.ToString();
						byte signValue = Byte.Parse(signAsString);
						string binary = Convert.ToString(signValue, 2);

						if (binary.Length < 8)
						{
							binary = new string('0', 8 - (binary.Length)) + binary;

						}
						Console.WriteLine($"Sign is {sign}, binary is {binary}, binary length is :{binary.Length}");
						if (signValue == 0)
						{
							//Console.WriteLine("Value is : " + binary);
						}
						//Console.WriteLine(sign);
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
				//Console.WriteLine(left + right);
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
						{	//Console.WriteLine("Sign is a byte!");
							string signAsString = sign.ToString();
							byte signValue = Byte.Parse(signAsString);
							binary = Convert.ToString(signValue, 2);
							

							if (binary.Length < 8)
							{
								binary = new string('0', 8 - binary.Length) + binary;
							
							}

							Console.WriteLine($"Sign is {sign}, binary is {binary}, binary length is :{binary.Length}");
							if (signValue == 0)
							{
								Console.WriteLine("Value is 0!");
								//Console.WriteLine("Value is : " + binary);
							}


						}

						else if (sign is uint24)
						{
							string temp = sign.ToString();
							uint24 num = uint24.TryParse(temp);
							binary = uint24.ToBinaryString(num);
						}

						return code + binary;
					}
					return string.Empty;
				}
				string left = outputCode(node.LeftChild, sign, code + "0");
				string right = outputCode(node.RightChild, sign, code + "1");
				return left + right;
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


//private void incrementParentChain(HuffNode<T> node)
//{
//	if (node != null && node.Parent != null)
//	{
//		node.Parent.Frequency++;
//		incrementParentChain(node.Parent);
//	}
//}

//public void FindAndIncrementNodeWithSign(T newSign)
//{
//	HuffNode<T> tempNode = Root;
//	if (tempNode.Value.CompareTo(newSign) == 0)
//	{
//		tempNode.Frequency++;
//	}
//	else
//	{
//		FindNodeWithVal(Root.LeftChild, newSign);
//		FindNodeWithVal(Root.RightChild, newSign);
//	}
//}


//This was inside if (currentNode == null)
//Console.WriteLine("Adding " + newSign + "as a new node");
////HuffNode<T> prevNull = NullNode;


//HuffNode<T> signNode = new HuffNode<T>(newSign); // new right child.
//signNode.Identifier = s_Counter--;
////signNode.Parent = prevNull;

//HuffNode<T> newNYT = new HuffNode<T>(); // new left child;
//newNYT.Identifier = s_Counter--;
////newNYT.Parent = prevNull;

//NullNode.LeftChild = newNYT;
//NullNode.RightChild = signNode;

//NullNode = newNYT;
//currentNode = signNode;


//This was inside if (currentNode == null)
//prevNull.Frequency++;

//code = outputCodeOf(newSign);


//HuffNode<T> prevNull = NullNode;


//HuffNode<T> signNode = new HuffNode<T>(newSign); // new right child.
//signNode.Identifier = s_Counter--;
//signNode.Parent = prevNull;

//HuffNode<T> newNYT = new HuffNode<T>(); // new left child;
//newNYT.Identifier = s_Counter--;
//newNYT.Parent = prevNull;

//prevNull.LeftChild = newNYT;
//prevNull.RightChild = signNode;

//NullNode = newNYT;
//currentNode = signNode;
////prevNull.Frequency++;

////code = outputCodeOf(newSign);

//This was inside the else block at the end
//Console.WriteLine($"targer parent : {targetParent == null} \t currentParent: {currentParent == null}");
//Console.WriteLine($"currentValue { char.Parse(currentNode.Frequency.ToString())}");

//if (targetNode.IsLeftChild())
//{
//	targetParent.LeftChild = currentNode;
//}

//else if (targetNode.IsRightChild())
//{
//	targetParent.RightChild = currentNode;
//}

//if (currentNode.IsLeftChild())
//{
//	currentParent.LeftChild = targetNode;
//}

//else if (currentNode.IsRightChild())
//{
//	currentParent.RightChild = targetNode;
//}
