using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser
{
    public class Movie
    {
        public long Id { get; set; }
        public int GenreId { get; set; }

        public string Genre { get; set; } //Consider normalizing out to new entity

        public long Year { get; set; }
        public long Gross { get; set; }
        public string Name { get; set; }
    }
}
