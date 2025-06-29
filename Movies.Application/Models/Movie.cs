using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public partial class Movie
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public string Slug => GenerateSlug();
        public float? Rating { get; set; }
        public int? UserRating { get; set; }
        public required int YearOfRelease { get; init; }

        public required List<string> Genres { get; init; } = new();


        private string GenerateSlug()
        {
            var sluggedTitle = RegexSlug().Replace(Title, String.Empty).ToLower().Replace(" ", "-");
            return $"{sluggedTitle}-{YearOfRelease}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking,5)]
        private static partial Regex RegexSlug();
    }
}
