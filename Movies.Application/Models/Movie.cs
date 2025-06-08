using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class Movie
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public required int YearOfRelease { get; init; }

        public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
    }
}
