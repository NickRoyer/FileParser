using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.Repos
{
    public class MovieListLinqParallel : IMovieRepo
    {        
        public FirstField Field { get; set; }
        public List<Movie> MovieList { get; set; } = new List<Movie>();

        public long FindMovies(long startYear, long endYear, string genre)
        {
            return MovieList.AsParallel().Count(ml => (ml.Year >= startYear && ml.Year <= endYear && ml.Genre == genre));
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            return MovieList.AsParallel().Count(ml => ml.Gross >= minGross && ml.Gross <= maxGross);
        }

        public void Init(ICollection<Movie> movieList, FirstField FF)
        {
            Field = FF;
            if (FF == FirstField.Year)
                MovieList.AddRange(movieList.OrderBy(ml => ml.Year).ThenBy(ml => ml.Genre));
            if (FF == FirstField.Genre)
                MovieList.AddRange(movieList.OrderBy(ml => ml.Genre).ThenBy(ml => ml.Year));
        }

        public string Type()
        {
            return "List Parallel Linq";
        }      
    }
}
