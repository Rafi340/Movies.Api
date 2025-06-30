using FluentValidation;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class GetAllMoviesOptionsValidators : AbstractValidator<GetAllMoviesOptions>
    {
        private static readonly string[] AcceptableSortFields =
        {
            "title", "YearOfRelease"
        };
        public GetAllMoviesOptionsValidators()
        {
            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.Now.Year);
            RuleFor(x => x.SortField)
                .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Sort field must be one of the following: " + string.Join(", ", AcceptableSortFields));
        }
    }
}
