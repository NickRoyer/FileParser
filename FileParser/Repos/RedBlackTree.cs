using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser.Repos
{
    public class RedBlackTreeNode<K,V> : BinaryTreeNode<K,V> 
    {
        public RedBlackTreeNode(K key, V value, byte color):base(key, value)
        {
            Color = color;
        }

        public byte Color { get; set; }
    }

    public class RedBlackTree<K,V> : BinaryTree<K,V> where K : IComparable<K> 
    {
        public const byte RED = 1;
        public const byte BLACK = 0;

        public override void Add(K key, V value)
        {
            RedBlackTreeNode<K, V> newNode = new RedBlackTreeNode<K, V>(key, value, RED);
            InsertNode(newNode);
            Insert_Repair_Tree(newNode);
        }

        //These methods were transcribed from Wikipedia:  https://en.wikipedia.org/wiki/Red%E2%80%93black_tree

        private void Rotate_Left(RedBlackTreeNode<K, V> nOrig)
        {
            RedBlackTreeNode<K, V> nOrigRight = (RedBlackTreeNode < K, V > )nOrig.Right;
            RedBlackTreeNode<K, V> nOrigParent = (RedBlackTreeNode < K, V > )nOrig.Parent;

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

        private void Rotate_Right(RedBlackTreeNode<K, V> nOrig)
        {
            RedBlackTreeNode<K, V> nOrigLeft = (RedBlackTreeNode < K, V > )nOrig.Left;
            RedBlackTreeNode<K, V> nOrigParent = (RedBlackTreeNode < K, V > )nOrig.Parent;

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

        private void Insert_Repair_Tree(RedBlackTreeNode<K, V> node)
        {
            RedBlackTreeNode<K, V> nodeParent = (RedBlackTreeNode<K, V>)node.Parent;
            RedBlackTreeNode<K, V> nodeUncle = (RedBlackTreeNode<K, V>)node.Uncle();

            if (nodeParent == null)
                Insert_1_Root(node);
            else if (nodeParent.Color == BLACK)
                Insert_2_SKIP(node);
            else if (nodeUncle != null && nodeUncle.Color == RED)
                Insert_3_ParentUncleRed(node);
            else
                Insert_4a_RedParent_BlackUncle(node);
        }

        private void Insert_1_Root(RedBlackTreeNode<K, V> node)
        {
            if (node.Parent == null)
                node.Color = BLACK;
        }

        private void Insert_2_SKIP(RedBlackTreeNode<K, V> node)
        {
            return; // Tree is Valiid 
        }

        private void Insert_3_ParentUncleRed(RedBlackTreeNode<K, V> node)
        {
            RedBlackTreeNode<K, V> nodeParent = (RedBlackTreeNode<K, V>)node.Parent;            

            if (nodeParent != null)
                nodeParent.Color = BLACK;

            RedBlackTreeNode<K, V> nodeUncle = (RedBlackTreeNode<K, V>)node.Uncle();
            if (nodeUncle != null)
                nodeUncle.Color = BLACK;

            RedBlackTreeNode<K, V> grandParent = (RedBlackTreeNode<K, V>)node.GrandParent();
            if (grandParent != null)
            {
                grandParent.Color = RED;
                Insert_Repair_Tree(grandParent);
            }

        }

        private void Insert_4a_RedParent_BlackUncle(RedBlackTreeNode<K, V> node)
        {
            RedBlackTreeNode<K, V> nodeParent = (RedBlackTreeNode<K, V>)node.Parent;
            RedBlackTreeNode<K, V> nodeGrandParent = (RedBlackTreeNode<K, V>)node.GrandParent();

            if (node == nodeParent.Right && nodeParent == nodeGrandParent.Left)
            {
                Rotate_Left(nodeParent);
                node = (RedBlackTreeNode<K, V>)node.Left;
            }
            else if (node == nodeParent.Left && nodeParent == nodeGrandParent.Right)
            {
                Rotate_Right(nodeParent);
                node = (RedBlackTreeNode<K, V>)node.Right;
            }

            Insert_4b_Outside(node);
        }

        private void Insert_4b_Outside(RedBlackTreeNode<K, V> node)
        {
            RedBlackTreeNode<K, V> nodeParent = (RedBlackTreeNode<K,V>)node.Parent;
            RedBlackTreeNode<K, V> nodeGrandParent = (RedBlackTreeNode<K,V>)node.GrandParent();

            if (node == nodeParent.Left)
                Rotate_Right(nodeGrandParent);
            else
                Rotate_Left(nodeGrandParent);
            nodeParent.Color = BLACK;
            nodeGrandParent.Color = RED;
        }

    }
}
