using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FileParser.DataStructures;

namespace ParserUnitTests
{
    public class BTreeUnitTest
    {
        [Fact]
        public void VerifyOneNode()
        {
            BTree<int, string> testTree = new BTree<int, string>(10, 20);

            PopulateTree(testTree, 1);

            Assert.Equal(1, testTree.Count);
            Assert.True(testTree.CheckConstraints());
        }

        private void PopulateTree(BTree<int, string> tree, int cnt)
        {
            for (int i = 1; i <= cnt; i++)
            {                
                tree.Add(i, "test" + i);
            }
        }

        private void PopulateTreeRandom(BTree<int, int> tree, int cnt)
        {
            HashSet<int> randValues = new HashSet<int>();

            Random rnd = new Random();
            while(randValues.Count < cnt)
            {
                int i = rnd.Next(1, cnt * 5);
                if(!randValues.Contains(i))
                    randValues.Add(i);
            }

            foreach( int k in randValues)
            {
                tree.Add(k, k);
            }

        }

        [Fact]
        public void VerifyTenNodesHighCapacity()
        {
            BTree<int, string> testTree = new BTree<int, string>(100, 20);

            PopulateTree(testTree, 10);

            Assert.Equal(10, testTree.Count);
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerifyTenNodesWithCapacityGrowth()
        {
            BTree<int, string> testTree = new BTree<int, string>(10, 20);

            PopulateTree(testTree, 10);

            Assert.Equal(10, testTree.Count);
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerifyTenNodesWithResize()
        {
            BTree<int, string> testTree = new BTree<int, string>(10, 2);

            PopulateTree(testTree, 10);

            Assert.Equal(10, testTree.Count);
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerifyCapacityGrowth()
        {
            BTree<int, string> testTree = new BTree<int, string>(1, 5);

            PopulateTree(testTree, 10);

            Assert.Equal(10, testTree.Count);
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerifyFindOneNode()
        {
            BTree<int, string> testTree = new BTree<int, string>(10, 20);

            testTree.Add(1, "test");

            Assert.NotNull(testTree.Find(1));
            Assert.Equal("test", testTree.Find(1));
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerfiyUniqueCheck()
        {
            BTree<int, string> testTree = new BTree<int, string>(10, 10);

            testTree.Add(1, "Test");

            Assert.Equal(1, testTree.Count);
            Assert.Throws<Exception>(() => testTree.Add(1, "Double"));
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerfiyLeafUniqueCheck()
        {
            BTree<int, string> testTree = new BTree<int, string>(5, 10);

            PopulateTree(testTree, 10);

            Assert.Equal(10, testTree.Count);
            Assert.Throws<Exception>(() => testTree.Add(8, "Double"));
            Assert.True(testTree.CheckConstraints());
        }

        [Fact]
        public void VerifyRangeSelectInOrder()
        {
            BTree<int, int> testTree = new BTree<int, int>(100, 100);

            //Generates 5k random values from 1 -> 25k
            PopulateTreeRandom(testTree, 5000);

            IList<int> values = testTree.Range(0, 25001);

            Assert.Equal(5000, values.Count);

            int last = 0;
            foreach(int v in values)
            {
                Assert.True(v > last);
                last = v;
            }

            Assert.Equal(5000, testTree.Count);            
        }

        [Fact]
        public void VerifyRangeSelectSmall()
        {
            BTree<int, string> testTree = new BTree<int, string>(3, 5);

            PopulateTree(testTree, 10);

            IList<string> values = testTree.Range(2, 5);

            Assert.Equal(4, values.Count);
        }

        [Fact]
        public void VerifyRangeSelectSmall2()
        {
            BTree<int, string> testTree = new BTree<int, string>(3, 5);

            PopulateTree(testTree, 10);

            IList<string> values = testTree.Range(5, 7);

            Assert.Equal(3, values.Count);
        }


        //[Fact]
        //public void VerfiyRemoveIndex()
        //{
        //    BTree<int, string> testTree = new BTree<int, string>(5, 10);

        //    PopulateTree(testTree, 3);
        //    testTree.Remove(1);

        //    Assert.Equal(2, testTree.Count);
        //    Assert.True(testTree.CheckConstraints());
        //}

        //[Fact]
        //public void VerfiyRemoveLeaf()
        //{
        //    BTree<int, string> testTree = new BTree<int, string>(5, 10);

        //    PopulateTree(testTree, 10);
        //    testTree.Remove(7);

        //    Assert.Equal(9, testTree.Count);
        //    Assert.True(testTree.CheckConstraints());
        //}


    }
}
