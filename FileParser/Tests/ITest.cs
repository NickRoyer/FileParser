using System;
using System.Collections.Generic;
using System.Text;
using FileParser.Repos;

namespace FileParser.Tests
{
    public interface ITest
    {
        IResult RunTest(IMovieRepo repo);

        int QueryCnt { get; set; }
        string ToConsoleString();
        string TestDataString();
    }

    public abstract class BaseTest : ITest
    {
        public abstract IResult RunTest(IMovieRepo repo);

        public virtual int QueryCnt { get; set; }

        public virtual string ToConsoleString()
        {
            StringBuilder a = new StringBuilder();
            a.AppendLine(TestDataString());
            a.AppendLine("Query Cnt: " + QueryCnt.ToString());
            return a.ToString();
        }

        public abstract string TestDataString();
    }

    public class GrossRevTest : BaseTest
    {
        public long MinGross { get; set; }
        public long MaxGross { get; set; }

        public override IResult RunTest(IMovieRepo repo)
        {
            long? foundCnt = null;
            for (int i = 0; i < QueryCnt; i++)
                foundCnt = repo.FindMoviesInGrossReceiptRange(MinGross, MaxGross);

            return  (IResult)new GrossRevResult() { FoundMovieCnt = foundCnt, Repo = repo, Test = this };
        }
       
        public override string TestDataString()
        {
            return "Test of Min Gross:" + MinGross.ToString() + " Max Gross: " + MaxGross.ToString();
        }
    }

    public class YearGenreTest : BaseTest
    {
        public long StartYear { get; set; }

        public long EndYear { get; set; }

        public string Genre { get; set; }

        public override  IResult RunTest(IMovieRepo repo)
        {
            long? foundCnt = null;
            for (int i = 0; i< QueryCnt; i++)
                foundCnt = repo.FindMovies(StartYear, EndYear, Genre);

            return new YearGenreTestResult() { FoundMovieCnt = foundCnt, Repo = repo, Test = this };
        }

        public override string TestDataString()
        {
            return "Test of : " + Genre + " Start: " + StartYear.ToString() + " End: " + EndYear.ToString() + " Total Years : " + (EndYear - StartYear + 1);
        }
    }

    public class RepoGenerationTest : BaseTest
    {
        public List<Movie> MovieList = null;
        public FirstField Field;
        private IMovieRepo Repo = null;

        public override IResult RunTest(IMovieRepo repo)
        {
            QueryCnt = 1;
            Repo = repo;
            repo.Init(MovieList, Field);
            return new CreateResult() { Repo = repo, Test = this };
        }

        public override string TestDataString()
        {
            return "Test initializing Repo";
        }
    }
}
