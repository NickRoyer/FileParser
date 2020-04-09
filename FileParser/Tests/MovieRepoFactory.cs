using System;
using System.Collections.Generic;
using System.Text;
using FileParser.Repos;

namespace FileParser.Tests
{
    public static class MovieRepoFactory
    {
        public enum Type { Dictionary, SearchTree, SortedDictionary, BinarySearchTree, RedBlackBinaryTree, Lookup, BTree, LinqList, LinqParList }

        public static IMovieRepo Repo(Type rt)
        {
            IMovieRepo returnRepo;
            switch (rt)
            {
                case Type.Dictionary:
                    returnRepo = new MovieDictionaryRepo();
                    break;
                case Type.SortedDictionary:
                    returnRepo = new MovieSortedDictionaryRepo();
                    break;
                case Type.SearchTree:
                    returnRepo = new MovieC5SearchTreeRepo();
                    break;
                case Type.Lookup:
                    returnRepo = new MovieLookupRepo();
                    break;
                case Type.BinarySearchTree:
                    returnRepo = new MovieBinaryTreeRepo(MovieBinaryTreeRepo.BinaryTreeType.BinaryTree);
                    break;
                case Type.RedBlackBinaryTree:
                    returnRepo = new MovieBinaryTreeRepo(MovieBinaryTreeRepo.BinaryTreeType.RedBlackBinaryTree);
                    break;
                case Type.BTree:
                    returnRepo = new MovieBTreeRepo();
                    break;
                case Type.LinqList:
                    returnRepo = new MovieListLinqRepo();
                    break;
                case Type.LinqParList:
                    returnRepo = new MovieListLinqParallel();
                    break;
                default:
                    throw new Exception("RepoType: " + rt.ToString() + "Not Implemented");
            }            

            return returnRepo;
        }
    }
}
