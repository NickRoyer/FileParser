using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser.DataStructures
{
    public class RedBlackTree<K,V> : BinaryTree<K,V> where K : IComparable<K> 
    {
        public class RedBlackTreeNode : BinaryTreeNode
        {
            public RedBlackTreeNode(K key, V value, byte color) : base(key, value)
            {
                Color = color;
            }

            public byte Color { get; set; }
        }

        public const byte RED = 1;
        public const byte BLACK = 0;

        public override void Add(K key, V value)
        {
            RedBlackTreeNode newNode = new RedBlackTreeNode(key, value, RED);
            InsertNode(newNode);
            Insert_Repair_Tree(newNode);
        }

        //These methods were transcribed from Wikipedia:  https://en.wikipedia.org/wiki/Red%E2%80%93black_tree

        private void Rotate_Left(RedBlackTreeNode nOrig)
        {
            RedBlackTreeNode nOrigRight = (RedBlackTreeNode)nOrig.Right;
            RedBlackTreeNode nOrigParent = (RedBlackTreeNode)nOrig.Parent;

            if (nOrigRight == null)
                throw new Exception("A leaf node cannot be promoted since it is empty");

            nOrig.Right = nOrigRight.Left;
            nOrigRight.Left = nOrig;
            nOrig.Parent = nOrigRight;

            if (nOrig.Right != null)
                nOrig.Right.Parent = nOrig;

            if (nOrigParent != null)
            {
                if (nOrig == nOrigParent.Left)
                    nOrigParent.Left = nOrigRight;
                else if (nOrig == nOrigParent.Right)
                    nOrigParent.Right = nOrigRight;
            }
            else // need to track the new root node
                Root = nOrigRight;

            nOrigRight.Parent = nOrigParent;
        }

        private void Rotate_Right(RedBlackTreeNode nOrig)
        {
            RedBlackTreeNode nOrigLeft = (RedBlackTreeNode)nOrig.Left;
            RedBlackTreeNode nOrigParent = (RedBlackTreeNode)nOrig.Parent;

            if (nOrigLeft == null)
                throw new Exception("A leaf node cannot be promoted since it is empty");

            nOrig.Left = nOrigLeft.Right;
            nOrigLeft.Right = nOrig;
            nOrig.Parent = nOrigLeft;

            if (nOrig.Left != null)
                nOrig.Left.Parent = nOrig;

            if (nOrigParent != null)
            {
                if (nOrig == nOrigParent.Left)
                    nOrigParent.Left = nOrigLeft;
                else if (nOrig == nOrigParent.Right)
                    nOrigParent.Right = nOrigLeft;
            }
            else // need to track the new root node
                Root = nOrigLeft;

            nOrigLeft.Parent = nOrigParent;
        }

        private void Insert_Repair_Tree(RedBlackTreeNode node)
        {
            RedBlackTreeNode nodeParent = (RedBlackTreeNode) node.Parent;
            RedBlackTreeNode nodeUncle = (RedBlackTreeNode)node.Uncle();

            if (nodeParent == null)
                Insert_1_Root(node);
            else if (nodeParent.Color == BLACK)
                Insert_2_SKIP(node);
            else if (nodeUncle != null && nodeUncle.Color == RED)
                Insert_3_ParentUncleRed(node);
            else
                Insert_4a_RedParent_BlackUncle(node);
        }

        private void Insert_1_Root(RedBlackTreeNode node)
        {
            if (node.Parent == null)
                node.Color = BLACK;
        }

        private void Insert_2_SKIP(RedBlackTreeNode node)
        {
            return; // Tree is Valiid 
        }

        private void Insert_3_ParentUncleRed(RedBlackTreeNode node)
        {
            RedBlackTreeNode nodeParent = (RedBlackTreeNode)node.Parent;            

            if (nodeParent != null)
                nodeParent.Color = BLACK;

            RedBlackTreeNode nodeUncle = (RedBlackTreeNode)node.Uncle();
            if (nodeUncle != null)
                nodeUncle.Color = BLACK;

            RedBlackTreeNode grandParent = (RedBlackTreeNode)node.GrandParent();
            if (grandParent != null)
            {
                grandParent.Color = RED;
                Insert_Repair_Tree(grandParent);
            }

        }

        private void Insert_4a_RedParent_BlackUncle(RedBlackTreeNode node)
        {
            RedBlackTreeNode nodeParent = (RedBlackTreeNode)node.Parent;
            RedBlackTreeNode nodeGrandParent = (RedBlackTreeNode)node.GrandParent();

            if (node == nodeParent.Right && nodeParent == nodeGrandParent.Left)
            {
                Rotate_Left(nodeParent);
                node = (RedBlackTreeNode)node.Left;
            }
            else if (node == nodeParent.Left && nodeParent == nodeGrandParent.Right)
            {
                Rotate_Right(nodeParent);
                node = (RedBlackTreeNode)node.Right;
            }

            Insert_4b_Outside(node);
        }

        private void Insert_4b_Outside(RedBlackTreeNode node)
        {
            RedBlackTreeNode nodeParent = (RedBlackTreeNode)node.Parent;
            RedBlackTreeNode nodeGrandParent = (RedBlackTreeNode)node.GrandParent();

            if (node == nodeParent.Left)
                Rotate_Right(nodeGrandParent);
            else
                Rotate_Left(nodeGrandParent);
            nodeParent.Color = BLACK;
            nodeGrandParent.Color = RED;
        }

    }
}
