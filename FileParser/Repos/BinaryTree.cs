using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser.Repos
{
    public class BinaryTreeNode<K, V>
    {
        public BinaryTreeNode(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public K Key { get; set; }
        public V Value { get; set; }
        public BinaryTreeNode<K, V> Left { get; set; }
        public BinaryTreeNode<K, V> Right { get; set; }
        public BinaryTreeNode<K, V> Parent { get; set; }

        public BinaryTreeNode<K, V> GrandParent()
        {
            if (Parent == null)
                return null;
            return Parent.Parent;
        }

        public BinaryTreeNode<K, V> Sibling()
        {
            if (Parent == null)
                return null;
            if (this == Parent.Left)
                return Parent.Right;
            else
                return Parent.Left;
        }

        public BinaryTreeNode<K, V> Uncle(){
            BinaryTreeNode<K, V> g = GrandParent();
            if (g == null || Parent == null)
                return null;
            return Parent.Sibling();
        }   
    }

    public class BinaryTree<K, V> where K : IComparable<K>
    {
        public BinaryTreeNode<K, V> Root { get; protected set; }

        public int Count { get; protected set; }

        public virtual void Add(K key, V value)
        {
            BinaryTreeNode<K, V> newNode = new BinaryTreeNode<K, V>(key, value);
            InsertNode(newNode);
        }

        protected void InsertNode(BinaryTreeNode<K, V> newNode)
        {
            if (Root == null)
                Root = newNode;
            else
            {
                BinaryTreeNode<K, V> current = Root;
                BinaryTreeNode<K, V> parent;

                while (true)
                {
                    parent = current;
                    int compareRes = newNode.Key.CompareTo(current.Key);
                    if (compareRes == 0)
                    {
                        throw new Exception("Duplicate Key value being inserted into the binary tree. Key Value: " + newNode.Key.ToString());
                    }
                    else if (compareRes < 0)
                    {
                        current = current.Left;
                        if (current == null)
                        {
                            parent.Left = newNode;
                            newNode.Parent = parent;
                            Count++;
                            return;
                        }
                    }
                    else
                    {
                        current = current.Right;
                        if (current == null)
                        {
                            parent.Right = newNode;
                            newNode.Parent = parent;
                            Count++;
                            return;
                        }
                    }
                }
            }
        }

        //Find the minimum Node that has a Key Value >= the search key 
        public BinaryTreeNode<K, V> FindMinNodeForKey(K key)
        {
            if (Root == null)
                throw new Exception("The tree has not been initialized with at least one element.");

            BinaryTreeNode<K, V> current = Root;
            BinaryTreeNode<K, V> parent;
            while (true)
            {
                parent = current;
                int compareRes = key.CompareTo(current.Key);
                if (compareRes == 0)
                {
                    return current;
                }
                else if (compareRes < 0)
                {
                    current = current.Left;
                    if (current == null)
                    {
                        return parent;
                    }
                }
                else
                {
                    current = current.Right;
                    if (current == null)
                    {
                        return parent;
                    }
                }
            }
        }

        public V Get(K key)
        {
            BinaryTreeNode<K, V> foundNode = FindMinNodeForKey(key);
            if (foundNode.Key.CompareTo(key) == 0)
                return foundNode.Value;

            return default;
        }

        public IList<V> Range(K StartInc, K EndInc)
        {
            BinaryTreeNode<K, V> foundNode = FindMinNodeForKey(StartInc);
            return InOrderValuesRange(foundNode, ref StartInc, EndInc);
        }

        protected IList<V> InOrderValuesRangeSubTree(BinaryTreeNode<K, V> node, ref K rangeMin, K rangeMax, IList<V> l = null)
        {                        
            if (l == null)
                l = new List<V>();

            if (node == null)
                return l;

            if (node.Left != null && node.Key.CompareTo(rangeMin) > 0)
                InOrderValuesRangeSubTree(node.Left, ref rangeMin, rangeMax, l);

            if (node.Key.CompareTo(rangeMin) >= 0 && node.Key.CompareTo(rangeMax) <= 0)
            {
                l.Add(node.Value);
                rangeMin = node.Key;
            }

            if (node.Right != null && node.Key.CompareTo(rangeMax) <= 0)
                InOrderValuesRangeSubTree(node.Right, ref rangeMin, rangeMax, l);

            return l;
        }

        protected IList<V> InOrderValuesRange(BinaryTreeNode<K, V> node, ref K rangeMin, K rangeMax, IList<V> l = null)
        {
            if (l == null)
                l = new List<V>();

            if (node != null)
            {
                //If the initial node is greater than the min we need to move the min forward
                if (rangeMin.CompareTo(node.Key) < 0)
                    rangeMin = node.Key;

                //If the range has already been exhausted we can stop
                if (rangeMin.CompareTo(rangeMax) > 0)
                    return l;

                bool rootReached = false ;
                do
                {
                    InOrderValuesRangeSubTree(node, ref rangeMin, rangeMax, l);

                    bool moveLeft = false;
                    //On Right moving to parent continue, On Left moving to parent stop and traverse the tree for additional values on right children
                    //Once the root has been reached the entire tree has been traversed in order (by calling the recursive function)
                    while (!rootReached && !moveLeft)
                    {
                        BinaryTreeNode<K, V> parentNode = node.Parent;
                        if (parentNode != null)
                        {
                            if (parentNode.Left == node)
                                moveLeft = true;

                            node = parentNode;
                        }
                        else
                            rootReached = true;
                    }

                    //If the current key > the previous key increment the minimum
                    if (rangeMin.CompareTo(node.Key) < 0)
                        rangeMin = node.Key;

                } while (!rootReached && rangeMin.CompareTo(rangeMax) <= 0);
            }

            return l;
        }

        public IList<V> Values()
        {
            return Inorder(Root);
        }

        protected IList<V> Inorder(BinaryTreeNode<K, V> node, IList<V> l = null)
        {
            if (l == null)
                l = new List<V>();

            if (node == null)
                return l;

            /* first recur on left child */
            Inorder(node.Left, l);

            l.Add(node.Value);

            /* now recur on right child */
            Inorder(node.Right, l);

            return l;
        }

        public int MaxNodeDepth(BinaryTreeNode<K, V> node = null, int depth = 1)
        {
            int LeftDepth = depth;
            int RightDepth = depth;

            if (node == null)
                node = Root;

            if (node.Left == null && node.Right == null)
                return depth;

            if (node.Left != null)
                LeftDepth = MaxNodeDepth(node.Left, depth + 1);

            if (node.Right != null)
                RightDepth = MaxNodeDepth(node.Right, depth + 1);

            return Math.Max(LeftDepth, RightDepth);
        }
    }
}
