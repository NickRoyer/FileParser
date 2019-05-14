using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser.Repos
{
    public enum FirstField { Year, Genre }

    public interface IMovieRepo
    {
        void Init(ICollection<Movie> movieList, FirstField FF);

        FirstField Field { get; set; }

        string Type();

        //Note these were changed to long instead of List<Movie> to remove the cost of merging lists from the comparisons
        long FindMovies(long startYear, long endYear, string genre);

        long FindMoviesInGrossReceiptRange(long minGross, long maxGross);
    }
}
