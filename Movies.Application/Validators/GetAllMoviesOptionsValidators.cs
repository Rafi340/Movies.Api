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
            "title", "yearOfRelease"
        };
        public GetAllMoviesOptionsValidators()
        {
            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.Now.Year);
            RuleFor(x => x.SortField)
                .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Sort field must be one of the following: " + string.Join(", ", AcceptableSortFields));
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 25)
                .WithMessage("You can get between 1 and 25  per page");


        }

    }
}
