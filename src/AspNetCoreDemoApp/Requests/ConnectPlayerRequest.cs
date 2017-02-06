using FluentValidation;

namespace AspNetCoreDemoApp
{
    public class ConnectPlayerRequest
    {
        public string PlayerName { get; set; }
    }

    public class ConnectPlayerRequestValidator : AbstractValidator<ConnectPlayerRequest>
    {
        public ConnectPlayerRequestValidator()
        {
            this.RuleFor(x => x.PlayerName)
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Player's name is required.");
        }
    }
}