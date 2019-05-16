using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FileParser.Repos;

namespace ParserUnitTests
{
    public class BinaryTreeUnitTest
    {
        public enum TreeType { BinaryTree, RedBlackTree}

        public BinaryTree<long, string> TreeFactory(TreeType type)
        {
            switch (type)
            {
                case TreeType.BinaryTree:
                    return new BinaryTree<long, string>();
                case TreeType.RedBlackTree:
                    return new RedBlackTree<long, string>();
            }

            return null; 
        }

        [Fact]
        public void VerifyOneNode()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(1, "test");

            Assert.NotNull(testTree.Root);
            Assert.Equal(1, testTree.Root.Key);
            Assert.Equal("test", testTree.Root.Value);
        }

        [Fact]
        public void VerifyTwoLefts()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(5, "Test Root");
            testTree.Add(3, "Test 3");
            testTree.Add(1, "Test 1");

            BinaryTreeNode<long, string> currentNode = testTree.Root;

            Assert.NotNull(currentNode);
            Assert.Equal(5, currentNode.Key);
            Assert.Equal("Test Root", currentNode.Value);

            Assert.Null(currentNode.Right);
            Assert.NotNull(currentNode.Left);

            currentNode = testTree.Root.Left;

            Assert.Equal(3, currentNode.Key);
            Assert.Equal("Test 3", currentNode.Value);

            Assert.Null(currentNode.Right);
            Assert.NotNull(currentNode.Left);

            currentNode = currentNode.Left;

            Assert.Equal(1, currentNode.Key);
            Assert.Equal("Test 1", currentNode.Value);

        }

        [Fact]
        public void VerifyTwoLeftsRBTree()
        {
            RedBlackTree<long, string> testTree = new RedBlackTree<long, string>();

            testTree.Add(5, "Test Starts Root");
            testTree.Add(3, "Test Becomes Root");
            testTree.Add(1, "Test 1");

            BinaryTreeNode<long, string> currentNode = testTree.Root;

            Assert.NotNull(currentNode);
            Assert.Equal(3, currentNode.Key);
            Assert.Equal("Test Becomes Root", currentNode.Value);

            Assert.NotNull(currentNode.Right);
            Assert.NotNull(currentNode.Left);

            currentNode = testTree.Root.Right;

            Assert.Equal(5, currentNode.Key);
            Assert.Equal("Test Starts Root", currentNode.Value);

            Assert.Null(currentNode.Right);
            Assert.Null(currentNode.Left);

            currentNode = currentNode.Sibling();

            Assert.Null(currentNode.Right);
            Assert.Null(currentNode.Left);

            Assert.Equal(1, currentNode.Key);
            Assert.Equal("Test 1", currentNode.Value);
        }

        [Fact]
        public void VerifyTwoRights()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(5, "Test Root");
            testTree.Add(8, "Test 8");
            testTree.Add(12, "Test 12");

            BinaryTreeNode<long, string> currentNode = testTree.Root;

            Assert.NotNull(currentNode);
            Assert.Equal(5, currentNode.Key);
            Assert.Equal("Test Root", currentNode.Value);

            Assert.NotNull(currentNode.Right);
            Assert.Null(currentNode.Left);

            currentNode = testTree.Root.Right;

            Assert.Equal(8, currentNode.Key);
            Assert.Equal("Test 8", currentNode.Value);

            Assert.NotNull(currentNode.Right);
            Assert.Null(currentNode.Left);

            currentNode = currentNode.Right;

            Assert.Equal(12, currentNode.Key);
            Assert.Equal("Test 12", currentNode.Value);

        }

        [Fact]
        public void VerifyTwoRightsRBTree()
        {
            RedBlackTree<long, string> testTree = new RedBlackTree<long, string>();

            testTree.Add(5, "Test starts as Root");
            testTree.Add(8, "Test becomes Root");
            testTree.Add(12, "Test 12");

            BinaryTreeNode<long, string> currentNode = testTree.Root;

            Assert.NotNull(currentNode);
            Assert.Equal(8, currentNode.Key);
            Assert.Equal("Test becomes Root", currentNode.Value);

            Assert.NotNull(currentNode.Right);
            Assert.NotNull(currentNode.Left);

            currentNode = testTree.Root.Right;

            Assert.Equal(12, currentNode.Key);
            Assert.Equal("Test 12", currentNode.Value);

            Assert.Null(currentNode.Right);
            Assert.Null(currentNode.Left);

            currentNode = currentNode.Sibling();

            Assert.Equal(5, currentNode.Key);
            Assert.Equal("Test starts as Root", currentNode.Value);
        }

        [Fact]
        public void VerifyRBTreeRightBalance()
        {
            RedBlackTree<long, string> testTree = new RedBlackTree<long, string>();

            testTree.Add(1, "1");
            testTree.Add(2, "2");
            testTree.Add(3, "3");
            testTree.Add(4, "4");
            testTree.Add(5, "5");
            testTree.Add(6, "6");
            testTree.Add(7, "7");
            testTree.Add(8, "8");
            testTree.Add(9, "9");

            BinaryTreeNode<long, string> currentNode = testTree.Root;
            
            Assert.Equal(4, currentNode.Key);

            currentNode = currentNode.Left;

            Assert.Equal(2, currentNode.Key);
            Assert.Equal(6, currentNode.Sibling().Key);

            currentNode = currentNode.Left;
            Assert.Equal(1, currentNode.Key);
            Assert.Equal(3, currentNode.Sibling().Key);

            currentNode = testTree.Root.Right.Left;
            Assert.Equal(5, currentNode.Key);

            currentNode = currentNode.Sibling();
            Assert.Equal(8, currentNode.Key);

            currentNode = currentNode.Left;
            Assert.Equal(7, currentNode.Key);

            currentNode = currentNode.Sibling();
            Assert.Equal(9, currentNode.Key);
        }

        [Fact]
        public void VerifyRBTreeLeftBalance()
        {
            RedBlackTree<long, string> testTree = new RedBlackTree<long, string>();

            testTree.Add(9, "9");
            testTree.Add(8, "8");
            testTree.Add(7, "7");
            testTree.Add(6, "6");
            testTree.Add(5, "5");
            testTree.Add(4, "4");
            testTree.Add(3, "3");
            testTree.Add(2, "2");
            testTree.Add(1, "1");

            RedBlackTreeNode<long, string> currentNode = (RedBlackTreeNode<long, string>)testTree.Root;
            

            Assert.Equal(6, currentNode.Key);
            Assert.Equal(0, currentNode.Color);

            currentNode = (RedBlackTreeNode<long, string>)currentNode.Left;

            Assert.Equal(4, currentNode.Key);
            Assert.Equal(1, currentNode.Color);

            RedBlackTreeNode<long, string> siblingNode = (RedBlackTreeNode<long, string>)currentNode.Sibling();

            Assert.Equal(8, siblingNode.Key);
            Assert.Equal(1, siblingNode.Color);

            currentNode = (RedBlackTreeNode<long, string>)currentNode.Left;
            Assert.Equal(2, currentNode.Key);
            Assert.Equal(0, currentNode.Color);

            siblingNode = (RedBlackTreeNode<long, string>)currentNode.Sibling();
            Assert.Equal(5, siblingNode.Key);
            Assert.Equal(0, siblingNode.Color);

            currentNode = (RedBlackTreeNode<long, string>)currentNode.Left;
            Assert.Equal(1, currentNode.Key);
            Assert.Equal(1, currentNode.Color);

            siblingNode = (RedBlackTreeNode<long, string>)currentNode.Sibling();
            Assert.Equal(3, siblingNode.Key);
            Assert.Equal(1, currentNode.Color);

            currentNode = (RedBlackTreeNode<long, string>)testTree.Root.Right.Left;
            Assert.Equal(7, currentNode.Key);
            Assert.Equal(0, currentNode.Color);

            currentNode = (RedBlackTreeNode<long, string>)currentNode.Sibling();
            Assert.Equal(9, currentNode.Key);
            Assert.Equal(0, currentNode.Color);
        }


        [Fact]
        public void VerifyLeftAndRight()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(5, "Test Root");
            testTree.Add(3, "Test 3");
            testTree.Add(12, "Test 12");

            BinaryTreeNode<long, string> currentNode = testTree.Root;

            Assert.NotNull(currentNode);
            Assert.Equal(5, currentNode.Key);
            Assert.Equal("Test Root", currentNode.Value);

            Assert.NotNull(currentNode.Right);
            Assert.NotNull(currentNode.Left);

            currentNode = testTree.Root.Right;

            Assert.Equal(12, currentNode.Key);
            Assert.Equal("Test 12", currentNode.Value);

            Assert.Null(currentNode.Right);
            Assert.Null(currentNode.Left);

            currentNode = testTree.Root.Left;

            Assert.Equal(3, currentNode.Key);
            Assert.Equal("Test 3", currentNode.Value);

            Assert.Null(currentNode.Right);
            Assert.Null(currentNode.Left);
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyFindLeft(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(5, "Test Root");
            testTree.Add(3, "Test 3");
            testTree.Add(12, "Test 12");

            Assert.Equal("Test 3", testTree.Get(3));
            Assert.Equal("Test 12", testTree.Get(12));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyFindDoesNotFind(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(5, "Test Root");
            testTree.Add(3, "Test 3");
            testTree.Add(12, "Test 12");

            Assert.Null(testTree.Get(19));
            Assert.Null(testTree.Get(1));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyFindRight(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(5, "Test Root");
            testTree.Add(8, "Test 8");
            testTree.Add(12, "Test 12");

            Assert.Equal("Test 12", testTree.Get(12));
            Assert.Equal("Test 8", testTree.Get(8));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyMultiDirectionalFind(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(10, "Test Root");
            testTree.Add(6, "Test 6");
            testTree.Add(15, "Test 15");
            testTree.Add(8, "Test 8");
            testTree.Add(12, "Test 12");
            testTree.Add(3, "Test 3");
            testTree.Add(20, "Test 20");

            Assert.Equal("Test 12", testTree.Get(12));
            Assert.Equal("Test 8", testTree.Get(8));
            Assert.Equal("Test 20", testTree.Get(20));
            Assert.Null(testTree.Get(9));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyFindException(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            Assert.Throws<Exception>(() => testTree.Get(12));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyDuplicateException(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);
            testTree.Add(15, "Test 15");

            Assert.Throws<Exception>(() => testTree.Add(15, "Test 15"));
        }

        public string BuildTestString(IList<string> s)
        {
            return String.Join('_', s);
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeSmallTree(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(4, "4");
            testTree.Add(2, "2");
            testTree.Add(8, "8");
            testTree.Add(3, "3");
            testTree.Add(1, "1");
            testTree.Add(6, "6");

            string a = BuildTestString(testTree.Range(3, 100));
            Assert.Equal("3_4_6_8", a.ToString());

            a = BuildTestString(testTree.Range(0,100));
            Assert.Equal("1_2_3_4_6_8", a);

            a = BuildTestString(testTree.Range(3, 7));
            Assert.Equal("3_4_6", a.ToString());
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeSmallTreeRightOnly(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(1, "1");
            testTree.Add(2, "2");
            testTree.Add(3, "3");
            testTree.Add(4, "4");
            testTree.Add(5, "5");
            testTree.Add(6, "6");

            string a = BuildTestString(testTree.Range(3, 5));
            Assert.Equal("3_4_5", a.ToString());

            //string a = BuildTestString(testTree.Range(0, 100));
            //Assert.Equal("1_2_3_4_5_6", a);

            //a = BuildTestString(testTree.Range(3, 5));
            //Assert.Equal("3_4_5", a.ToString());
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeLargeTree2(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(225, "225");
            testTree.Add(300, "300");
            testTree.Add(125, "125");
            testTree.Add(100, "100");
            testTree.Add(400, "400");
            testTree.Add(200, "200");
            testTree.Add(1, "1");
            testTree.Add(2, "2");
            testTree.Add(3, "3");
            testTree.Add(4, "4");
            testTree.Add(5, "5");
            testTree.Add(6, "6");
            testTree.Add(8, "8");
            testTree.Add(21, "21");
            testTree.Add(17, "17");
            testTree.Add(15, "15");
            testTree.Add(19, "19");
            testTree.Add(12, "12");
            testTree.Add(14, "14");
            testTree.Add(13, "13");
            testTree.Add(16, "16");
            testTree.Add(18, "18");
            testTree.Add(20, "20");
            testTree.Add(30, "30");
            testTree.Add(31, "31");
            testTree.Add(22, "22");
            testTree.Add(27, "27");
            testTree.Add(23, "23");
            testTree.Add(28, "28");
            testTree.Add(29, "29");
            testTree.Add(101, "101");
            testTree.Add(105, "105");
            testTree.Add(120, "120");
            testTree.Add(190, "190");
            testTree.Add(290, "290");
            testTree.Add(390, "390");
            testTree.Add(115, "115");
            testTree.Add(24, "24");
            testTree.Add(26, "26");
            testTree.Add(25, "25");


            string a = BuildTestString(testTree.Range(22, 29));
            Assert.Equal("22_23_24_25_26_27_28_29", a);

            a = BuildTestString(testTree.Range(100, 199));
            Assert.Equal("100_101_105_115_120_125_190", a.ToString());
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeLargeTree3(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(1, "1");
            testTree.Add(225, "225");            
            testTree.Add(125, "125");
            testTree.Add(100, "100");
            testTree.Add(19, "19");
            testTree.Add(390, "390");            
            testTree.Add(200, "200");
            testTree.Add(2, "2");
            testTree.Add(400, "400");
            testTree.Add(3, "3");
            testTree.Add(4, "4");
            testTree.Add(5, "5");
            testTree.Add(300, "300");
            testTree.Add(6, "6");
            testTree.Add(8, "8");
            testTree.Add(21, "21");
            testTree.Add(17, "17");
            testTree.Add(15, "15");
            testTree.Add(22, "22");
            testTree.Add(12, "12");
            testTree.Add(290, "290");
            testTree.Add(14, "14");
            testTree.Add(13, "13");
            testTree.Add(16, "16");
            testTree.Add(18, "18");
            testTree.Add(20, "20");
            testTree.Add(30, "30");
            testTree.Add(31, "31");            
            testTree.Add(27, "27");
            testTree.Add(23, "23");
            testTree.Add(28, "28");
            testTree.Add(29, "29");
            testTree.Add(101, "101");
            testTree.Add(105, "105");
            testTree.Add(120, "120");
            testTree.Add(190, "190");                        
            testTree.Add(115, "115");
            testTree.Add(24, "24");
            testTree.Add(26, "26");
            testTree.Add(25, "25");


            string a = BuildTestString(testTree.Range(22, 29));
            Assert.Equal("22_23_24_25_26_27_28_29", a);

            a = BuildTestString(testTree.Range(100, 199));
            Assert.Equal("100_101_105_115_120_125_190", a.ToString());
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeLargeTree4(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(301, "391");
            testTree.Add(53, "53");
            testTree.Add(200, "200");
            testTree.Add(435, "435");
            testTree.Add(500, "500");
            testTree.Add(33, "33");
            testTree.Add(444, "444");
            testTree.Add(300, "300");
            testTree.Add(6, "6");
            testTree.Add(8, "8");
            testTree.Add(19, "19");
            testTree.Add(21, "21");
            testTree.Add(17, "17");
            testTree.Add(12, "12");
            testTree.Add(290, "290");
            testTree.Add(14, "14");
            testTree.Add(13, "13");
            testTree.Add(15, "15");
            testTree.Add(23, "23");
            testTree.Add(5, "5");
            testTree.Add(360, "360");
            testTree.Add(31, "31");
            testTree.Add(27, "27");
            testTree.Add(120, "120");
            testTree.Add(190, "190");
            testTree.Add(4, "4");
            testTree.Add(115, "115");
            testTree.Add(24, "24");
            testTree.Add(20, "20");
            testTree.Add(30, "30");
            testTree.Add(425, "425");
            testTree.Add(100, "100");
            testTree.Add(28, "28");
            testTree.Add(105, "105");
            testTree.Add(1, "1");
            testTree.Add(225, "225");
            testTree.Add(16, "16");
            testTree.Add(125, "125");
            testTree.Add(26, "26");
            testTree.Add(409, "409");
            testTree.Add(29, "29");
            testTree.Add(101, "101");
            testTree.Add(22, "22");
            testTree.Add(18, "18");
            testTree.Add(390, "390");
            testTree.Add(25, "25");
            testTree.Add(476, "476");
            testTree.Add(2, "2");
            testTree.Add(400, "400");
            testTree.Add(3, "3");

            string a = BuildTestString(testTree.Range(22, 29));
            Assert.Equal("22_23_24_25_26_27_28_29", a);

            a = BuildTestString(testTree.Range(100, 199));
            Assert.Equal("100_101_105_115_120_125_190", a.ToString());
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeLargeTree5(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(18, "18");
            testTree.Add(390, "390");
            testTree.Add(25, "25");
            testTree.Add(476, "476");
            testTree.Add(15, "15");
            testTree.Add(23, "23");
            testTree.Add(5, "5");
            testTree.Add(360, "360");
            testTree.Add(31, "31");
            testTree.Add(27, "27");
            testTree.Add(120, "120");
            testTree.Add(190, "190");
            testTree.Add(4, "4");
            testTree.Add(115, "115");
            testTree.Add(24, "24");
            testTree.Add(20, "20");
            testTree.Add(30, "30");
            testTree.Add(425, "425");
            testTree.Add(301, "391");
            testTree.Add(13, "13");
            testTree.Add(12, "12");
            testTree.Add(290, "290");
            testTree.Add(14, "14");
            testTree.Add(22, "22");
            testTree.Add(53, "53");
            testTree.Add(200, "200");
            testTree.Add(435, "435");
            testTree.Add(500, "500");
            testTree.Add(33, "33");
            testTree.Add(444, "444");
            testTree.Add(300, "300");
            testTree.Add(6, "6");
            testTree.Add(8, "8");
            testTree.Add(19, "19");
            testTree.Add(21, "21");
            testTree.Add(100, "100");
            testTree.Add(28, "28");
            testTree.Add(105, "105");
            testTree.Add(1, "1");
            testTree.Add(225, "225");
            testTree.Add(16, "16");
            testTree.Add(125, "125");
            testTree.Add(26, "26");
            testTree.Add(409, "409");
            testTree.Add(29, "29");
            testTree.Add(101, "101");
            testTree.Add(17, "17");            
            testTree.Add(2, "2");
            testTree.Add(400, "400");
            testTree.Add(3, "3");

            string a = BuildTestString(testTree.Range(22, 29));
            Assert.Equal("22_23_24_25_26_27_28_29", a);

            a = BuildTestString(testTree.Range(100, 199));
            Assert.Equal("100_101_105_115_120_125_190", a.ToString());

            a = BuildTestString(testTree.Range(6, 72));
            Assert.Equal("6_8_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30_31_33_53", a.ToString());
        }

        public int MaxNodeDepth(BinaryTreeNode<long, string> node, int depth = 1)
        {
            int LeftDepth = depth;
            int RightDepth = depth;

            if (node.Left == null && node.Right == null)
                return depth;

            if (node.Left != null)
                LeftDepth = MaxNodeDepth(node.Left, depth + 1);

            if (node.Right != null)
                RightDepth = MaxNodeDepth(node.Right, depth + 1);

            return Math.Max(LeftDepth, RightDepth);
        }

        [Fact]
        public void TestMaxNodeDepth()
        {
            BinaryTree<long, string> testTree = TreeFactory(TreeType.BinaryTree);

            testTree.Add(5, "5");

            Assert.Equal(1, MaxNodeDepth(testTree.Root));

            testTree.Add(3, "3");
            Assert.Equal(2, MaxNodeDepth(testTree.Root));

            testTree.Add(9, "9");
            Assert.Equal(2, MaxNodeDepth(testTree.Root));

            testTree.Add(1, "1");
            Assert.Equal(3, MaxNodeDepth(testTree.Root));

            testTree.Add(10, "10");
            Assert.Equal(3, MaxNodeDepth(testTree.Root));

            testTree.Add(6, "6");
            testTree.Add(7, "7");
            testTree.Add(8, "8");
            Assert.Equal(5, MaxNodeDepth(testTree.Root));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyLargeTreeGetsBalanced(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(18, "18");
            testTree.Add(390, "390");
            testTree.Add(25, "25");
            testTree.Add(476, "476");
            testTree.Add(15, "15");
            testTree.Add(23, "23");
            testTree.Add(5, "5");
            testTree.Add(360, "360");
            testTree.Add(31, "31");
            testTree.Add(27, "27");
            testTree.Add(120, "120");
            testTree.Add(190, "190");
            testTree.Add(4, "4");
            testTree.Add(115, "115");
            testTree.Add(24, "24");
            testTree.Add(20, "20");
            testTree.Add(30, "30");
            testTree.Add(425, "425");
            testTree.Add(301, "391");
            testTree.Add(13, "13");
            testTree.Add(12, "12");
            testTree.Add(290, "290");
            testTree.Add(14, "14");
            testTree.Add(22, "22");
            testTree.Add(53, "53");
            testTree.Add(200, "200");
            testTree.Add(435, "435");
            testTree.Add(500, "500");
            testTree.Add(33, "33");
            testTree.Add(444, "444");
            testTree.Add(300, "300");
            testTree.Add(6, "6");
            testTree.Add(8, "8");
            testTree.Add(19, "19");
            testTree.Add(21, "21");
            testTree.Add(100, "100");
            testTree.Add(28, "28");
            testTree.Add(105, "105");
            testTree.Add(1, "1");
            testTree.Add(225, "225");
            testTree.Add(16, "16");
            testTree.Add(125, "125");
            testTree.Add(26, "26");
            testTree.Add(409, "409");
            testTree.Add(29, "29");
            testTree.Add(101, "101");
            testTree.Add(17, "17");
            testTree.Add(2, "2");
            testTree.Add(400, "400");
            testTree.Add(3, "3");

            Assert.Equal(49, testTree.Count);

            if (type == TreeType.BinaryTree)
                Assert.Equal(11, MaxNodeDepth(testTree.Root));

            if (type == TreeType.RedBlackTree)
                Assert.Equal(7, MaxNodeDepth(testTree.Root));
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeSingleElement(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(4, "4");

            string a = BuildTestString(testTree.Range(4, 4));
            Assert.Equal("4", a);
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeSingleItemRange(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(4, "4");
            testTree.Add(2, "2");
            testTree.Add(8, "8");
            testTree.Add(3, "3");
            testTree.Add(1, "1");
            testTree.Add(6, "6");

            string a = BuildTestString(testTree.Range(4, 4));
            Assert.Equal("4", a);
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyInOrderTreePrint(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);
            testTree.Add(1, "1");
            testTree.Add(2, "2");
            testTree.Add(4, "4a");
            testTree.Add(3, "3");

            string a = BuildTestString(testTree.Values());
            Assert.NotNull(a);
            Assert.Equal("1_2_3_4a", a.ToString());
        }

        [Theory]
        [InlineData(TreeType.BinaryTree)]
        [InlineData(TreeType.RedBlackTree)]
        public void VerifyRangeLargeTree(TreeType type)
        {
            BinaryTree<long, string> testTree = TreeFactory(type);

            testTree.Add(21, "21");
            testTree.Add(17, "17");
            testTree.Add(15, "15");
            testTree.Add(19, "19");
            testTree.Add(12, "12");
            testTree.Add(14, "14");
            testTree.Add(13, "13");
            testTree.Add(16, "16");
            testTree.Add(18, "18");
            testTree.Add(20, "20");
            testTree.Add(30, "30");
            testTree.Add(31, "31");
            testTree.Add(22, "22");
            testTree.Add(27, "27");
            testTree.Add(23, "23");
            testTree.Add(28, "28");
            testTree.Add(29, "29");

            string a = BuildTestString(testTree.Range(19, 31));
            Assert.Equal("19_20_21_22_23_27_28_29_30_31", a);
        }

    }
}
