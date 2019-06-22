using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.DataStructures
{
    public class BTree<K, V> where K : IComparable<K>
    {        
        readonly List<BTreeNode> IndexList;
        private int LeafCapacity { get; set; }
        private double LeafCapacityPct { get; set; }
        private double CapacityGrowthPct { get; set; }
        private double MaxCapacityPct { get; set; }
        
        private int FastCount { get; set; } //stored local value used to calculate if balancing is required

        public int Count { get
            {
                return IndexList.Sum(i =>
                {
                    if (i.ChildList != null)
                        return i.ChildList.Count + 1;
                    else
                        return 1;
                }); 
            }
        }

        public BTree( int initIndexCapacity = 1000, int leafCapacity = 100, double capacityGrowthPct = .2, double maxCapacityPct = .85, double leafCapacityPct = .8 )
        {
            IndexList = new List<BTreeNode>(initIndexCapacity);
            LeafCapacity = leafCapacity;
            LeafCapacityPct = leafCapacityPct;
            CapacityGrowthPct = 1 + capacityGrowthPct;
            MaxCapacityPct = maxCapacityPct;
        }

        public class BTreeNode
        {
            public K Key { get; set; }
            public V Value { get; set; }

            public List<BTreeNode> ChildList { get; set; } = null;
        }

        //Returns the node index immediately previous to the key if the key is not found.
        //Returns null if at the start of the array
        private int? FindNodeIndex(K key, List<BTreeNode> nodeList)
        {
            if (nodeList == null || nodeList.Count == 0 )
                return null;

            int min = 0;
            int max = nodeList.Count-1;

            //Binary search to base case
            int mid = (min +max) / 2;
            while (min != mid)
            {                
                K curKey = nodeList[mid].Key;
                int res = key.CompareTo(curKey);

                switch (res)
                {
                    case 1:
                        min = mid;
                        break;
                    case 0:
                        return mid;
                    case -1:
                        max = mid;
                        break;
                }
                mid = (min + max) / 2;
            } 

            //Base case
            //When min == mid we know max == (min + 1) || min == max
            K minKey = nodeList[min].Key;
            int minRes = key.CompareTo(minKey);

            //If the key > min test against max
            if (minRes == 1 && min != max)
            {
                K maxKey = nodeList[max].Key;
                int maxRes = key.CompareTo(maxKey);

                switch (maxRes)
                {
                    case 1:
                        return max;
                    case 0:
                        return max;
                    case -1:
                        return min;
                }
            }
            else if (minRes >= 0)
                return min;
           
            return null;            
        }

        protected virtual BTreeNode FindNode(K key)
        {
            int? nodeIndex = FindNodeIndex(key, IndexList);

            if (nodeIndex != null)
            {
                BTreeNode foundNode = IndexList[(int)nodeIndex];

                if (foundNode.Key.CompareTo(key) == 0)
                    return foundNode;

                if (foundNode.ChildList != null)
                {
                    int? childIndex = FindNodeIndex(key, foundNode.ChildList);
                    if (childIndex != null)
                    {
                        foundNode = foundNode.ChildList[(int)childIndex];

                        if (foundNode.Key.CompareTo(key) == 0)
                            return foundNode;
                    }
                }
            }

            return default;
        }

        public V Find(K key)
        {            
            BTreeNode foundNode= FindNode(key);
            if (foundNode != null)
                return foundNode.Value;

            return default;
        }

        public IList<V> Range(K StartInc, K EndInc)
        {
            IList<V> returnList = new List<V>(FastCount);

            if (FastCount == 0)
                return returnList;

            int? foundIndex = FindNodeIndex(StartInc, IndexList);
            int index = (foundIndex != null) ? (int)foundIndex : 0;

            int childIndex = 0;

            BTreeNode indexNode = IndexList[index];
           
            if (indexNode.Key.CompareTo(StartInc) == -1 && indexNode.ChildList != null )
            {
                int? foundChildIndex = FindNodeIndex(StartInc, indexNode.ChildList);
                childIndex = (foundChildIndex == null) ? 0 : (int)foundChildIndex;
            }

            for(int i = (int)index;  i < IndexList.Count; i++)
            {
                indexNode = IndexList[i];

                if (indexNode.Key.CompareTo(EndInc) <= 0)
                {
                    if (indexNode.Key.CompareTo(StartInc) >= 0)
                        returnList.Add(indexNode.Value);
                }
                else
                    return returnList;

                if (indexNode.ChildList != null)
                {
                    for(int c = (int)childIndex;  c < indexNode.ChildList.Count; c++)
                    {
                        BTreeNode curNode = indexNode.ChildList[c];
                        if (curNode.Key.CompareTo(EndInc) <= 0)
                            returnList.Add(curNode.Value);
                        else
                            return returnList;
                    }
                    childIndex = 0;
                }                           
            }

            return returnList;
        }

        private void VerifyCapacity()
        {
            if ((FastCount + 1) > (IndexList.Capacity * MaxCapacityPct * LeafCapacity))
            {
                IndexList.Capacity = (int)(IndexList.Capacity * CapacityGrowthPct) + 1; //growth has to be at least 1
            }
        }

        private void VerifyUnique(BTreeNode node, K key)
        {
            if (node.Key.CompareTo(key) == 0)
                throw new Exception("Key already exists in the collection");
        }
      
        public virtual void Add(K key, V value)
        {
            VerifyCapacity();

            BTreeNode newNode= new BTreeNode { Key = key, Value = value };

            //Index is empty add
            if(IndexList.Count == 0)
            {
                IndexList.Add(newNode);
            }
            else
            {
                int? nodeIndex = FindNodeIndex(key, IndexList);

                //Insert at the begining of the index
                if (nodeIndex == null)
                {
                    IndexList.Insert(0, newNode);
                }
                else
                { 
                    BTreeNode indexNode = IndexList[(int)nodeIndex];

                    VerifyUnique(indexNode, key);

                    //Room exists in the IndexList to add directly
                    if (IndexList.Capacity * .6 >= IndexList.Count)
                    {
                        IndexList.Insert((int)nodeIndex + 1, newNode);
                    }
                    else
                    {
                        //Add to the leaf node if possible
                        if (indexNode.ChildList == null)
                        {
                            indexNode.ChildList = new List<BTreeNode>(LeafCapacity)
                        {
                            newNode
                        };
                        }
                        else
                        {
                            if (indexNode.ChildList.Count > 0)
                            {
                                int? childFind = FindNodeIndex(key, indexNode.ChildList);
                                int childNodeIndex = 0;
                                if (childFind != null)
                                {
                                    VerifyUnique(indexNode.ChildList[(int)childFind], key);
                                    childNodeIndex = (int)childFind + 1;
                                }                                 
                               
                                indexNode.ChildList.Insert(childNodeIndex, newNode);
                            }
                            else indexNode.ChildList.Add(newNode);

                            if (indexNode.ChildList.Count > LeafCapacity)
                            {
                                ResizeLeaf((int)nodeIndex);
                            }
                        }
                    }
                }                
            }

            FastCount++;
        }

        protected virtual void ResizeAllLeaves()
        {
            for (int i = 0; i < IndexList.Count; i++)
                ResizeLeaf(i);

            //Verify that the resizing worked
            if (0 != IndexList.Sum(i =>
            {
                if (i.ChildList != null && i.ChildList.Count > LeafCapacity)
                    return 1;
                else
                    return 0;
            }))
                throw new Exception("Leaf Capacity has been exceeded even after a resize of all leaves");
        }

        protected int IdealLeafCapacity()
        {
            int capacity = (int)(LeafCapacity * LeafCapacityPct) - 1;
            capacity = capacity > 0 ? capacity : 1;

            return capacity;
        }

        protected virtual void ResizeLeaf(int index)
        {
            BTreeNode indexNode = IndexList[index];
            int leafCapacityIndex = IdealLeafCapacity();

            if (indexNode.ChildList != null && 
                indexNode.ChildList.Count > leafCapacityIndex )
            {
                BTreeNode oldIndexNode = null;
                List<BTreeNode> oldIndexNodeList = null;

                BTreeNode newIndexNode = indexNode.ChildList[leafCapacityIndex];

                //If last position in the IndexList
                if (IndexList.Count - 1 == index)
                {
                    VerifyCapacity();
                    IndexList.Add(newIndexNode);
                }                    
                else
                {
                    oldIndexNode = IndexList[index + 1];
                    //Back up the old list and remove
                    oldIndexNodeList = oldIndexNode.ChildList;
                    oldIndexNode.ChildList = null;

                    IndexList[index + 1] = newIndexNode;
                }
                
                //Create the ChildList
                newIndexNode.ChildList = new List<BTreeNode>(LeafCapacity);

                //Add the items to the right of the new index node in the prev list as children
                newIndexNode.ChildList.AddRange(indexNode.ChildList.GetRange(leafCapacityIndex + 1, indexNode.ChildList.Count - leafCapacityIndex - 1));

                //Remove the range from the old list 
                indexNode.ChildList.RemoveRange(leafCapacityIndex, indexNode.ChildList.Count - leafCapacityIndex );

                //Add the old index node
                if(oldIndexNode != null)
                    newIndexNode.ChildList.Add(oldIndexNode);

                //Add all the old index nodes children
                if(oldIndexNodeList != null)
                    newIndexNode.ChildList.AddRange(oldIndexNodeList);

                //Resize the newly promoted leaf
                ResizeLeaf(index + 1);
            }
        }

        //not implemented
        //public virtual void Remove(K key)
        //{

        //    FastCount--;
        //}

        //For testing purposes
        public bool CheckConstraints()
        {
            if (this.Count != FastCount)
                throw new Exception("Count does not match fast Count");
            if (IndexList.Count > (IndexList.Capacity * MaxCapacityPct))
                throw new Exception("Max capacity has been exceeded without a resize in capacity");
            if (0 != IndexList.Sum(i =>
               {
                   if (i.ChildList != null && i.ChildList.Count > LeafCapacity)
                       return 1;
                   else
                       return 0;
               }))
                throw new Exception("Leaf Capacity has been exceeded.");

            return true;
        }

    }
}
