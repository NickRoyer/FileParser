using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FileParser.Repos;

namespace ParserUnitTests
{
    public class BinaryTreeUnitTest
    {
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

        [Fact]
        public void VerifyFindLeft()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(5, "Test Root");
            testTree.Add(3, "Test 3");
            testTree.Add(12, "Test 12");

            Assert.Equal("Test 3", testTree.Get(3));
            Assert.Equal("Test 12", testTree.Get(12));
        }

        [Fact]
        public void VerifyFindDoesNotFind()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(5, "Test Root");
            testTree.Add(3, "Test 3");
            testTree.Add(12, "Test 12");

            Assert.Null(testTree.Get(19));
            Assert.Null(testTree.Get(1));
        }

        [Fact]
        public void VerifyFindRight()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(5, "Test Root");
            testTree.Add(8, "Test 8");
            testTree.Add(12, "Test 12");

            Assert.Equal("Test 12", testTree.Get(12));
            Assert.Equal("Test 8", testTree.Get(8));
        }

        [Fact]
        public void VerifyMultiDirectionalFind()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

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

        [Fact]
        public void VerifyFindException()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            Assert.Throws<Exception>(() => testTree.Get(12));
        }

        [Fact]
        public void VerifyDuplicateException()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();
            testTree.Add(15, "Test 15");

            Assert.Throws<Exception>(() => testTree.Add(15, "Test 15"));
        }

        public string BuildTestString(List<string> s)
        {
            return String.Join('_', s);
        }

        [Fact]
        public void VerifyRangeSmallTree()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

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

        [Fact]
        public void VerifyRangeSmallTreeRightOnly()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

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

        [Fact]
        public void VerifyRangeLargeTree2()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

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

        [Fact]
        public void VerifyRangeLargeTree3()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

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

        [Fact]
        public void VerifyRangeLargeTree4()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();
            
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

        [Fact]
        public void VerifyRangeLargeTree5()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();
                              
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

        [Fact]
        public void VerifyRangeSingleElement()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(4, "4");

            string a = BuildTestString(testTree.Range(4, 4));
            Assert.Equal("4", a);
        }

        [Fact]
        public void VerifyRangeSingleItemRange()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

            testTree.Add(4, "4");
            testTree.Add(2, "2");
            testTree.Add(8, "8");
            testTree.Add(3, "3");
            testTree.Add(1, "1");
            testTree.Add(6, "6");

            string a = BuildTestString(testTree.Range(4, 4));
            Assert.Equal("4", a);
        }

        [Fact]
        public void VerifyInOrderTreePrint()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();
            testTree.Add(1, "1");
            testTree.Add(2, "2");
            testTree.Add(4, "4a");
            testTree.Add(3, "3");

            string a = BuildTestString(testTree.Values());
            Assert.NotNull(a);
            Assert.Equal("1_2_3_4a", a.ToString());
        }

        [Fact]
        public void VerifyRangeLargeTree()
        {
            BinaryTree<long, string> testTree = new BinaryTree<long, string>();

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
