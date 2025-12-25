using FluentValidation;
using RestApiProject.Models;

namespace RestApiProject.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(b => b.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(100).WithMessage("Author cannot exceed 100 characters");

        RuleFor(b => b.Year)
            .InclusiveBetween(1800, DateTime.Now.Year + 1).WithMessage("Year must be between 1800 and next year");

        RuleFor(b => b.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative");
    }
}