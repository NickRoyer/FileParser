using System;
using System.Collections.Generic;
using System.Text;
using FileParser.Repos;

namespace FileParser.Tests
{
    public interface ITest
    {
        IResult RunTest(IMovieRepo repo);
        string ToString();
    }

    public interface IRepeatTest : ITest
    {
        int QueryCnt { get; set; }
    }

    public class GrossRevTest : IRepeatTest
    {
        public int QueryCnt { get; set; }
        public long MinGross { get; set; }
        public long MaxGross { get; set; }

        public IResult RunTest(IMovieRepo repo)
        {
            long? foundCnt = null;
            for (int i = 0; i < QueryCnt; i++)
                foundCnt = repo.FindMoviesInGrossReceiptRange(MinGross, MaxGross);

            return  (IResult)new GrossRevResult() { FoundMovieCnt = foundCnt, Repo = repo, Test = this };
        }

        public override string ToString()
        {
            StringBuilder a = new StringBuilder();
            a.AppendLine("Test of Min:" + MinGross.ToString() + " Max: " + MaxGross.ToString());
            a.AppendLine("Query Cnt: " + QueryCnt.ToString());
            return a.ToString();
        }
    }

    public class YearGenreTest : IRepeatTest
    {
        public int QueryCnt { get; set; }

        public long StartYear { get; set; }

        public long EndYear { get; set; }

        public string Genre { get; set; }

        public IResult RunTest(IMovieRepo repo)
        {
            long? foundCnt = null;
            for (int i = 0; i< QueryCnt; i++)
                foundCnt = repo.FindMovies(StartYear, EndYear, Genre);

            return new YearGenreTestResult() { FoundMovieCnt = foundCnt, Repo = repo, Test = this };
        }

        public override string ToString()
        {
            StringBuilder a = new StringBuilder();
            a.AppendLine("Test of : " + Genre + " Start: " + StartYear.ToString() + " End: " + EndYear.ToString());
            a.AppendLine("Total Years : " + (EndYear - StartYear + 1).ToString());
            a.AppendLine("Query Cnt: " + QueryCnt);
            return a.ToString();
        }
    }

}
