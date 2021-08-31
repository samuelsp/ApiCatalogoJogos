using ExemploApiCatalogoJogos.InputModel;
using FluentValidation;

namespace ExemploApiCatalogoJogos.Models.Validators
{
    public class JogoInputModelValidator : AbstractValidator<JogoInputModel>
    {
        public JogoInputModelValidator()
        {
            RuleFor(j => j.Nome)                
                .MinimumLength(3)
                    .WithMessage("O nome do jogo deve conter no minimo 3 caracteres")
                .MaximumLength(100)
                    .WithMessage("O nome do jogo deve estar entre 3 e 100 caracteres");

            RuleFor(j => j.Produtora)
                .MinimumLength(1)
                    .WithMessage("O nome do jogo deve conter no minimo 1 caractere")
                .MaximumLength(100)
                    .WithMessage("O nome da produtora deve conter entre 1 e 100 caracteres");

            RuleFor(j => j.Preco)
                .InclusiveBetween(1, 1000)
                    .WithMessage("O preço deve ser de no mínimo 1 real e no máximo 1000 reais");
        }
    }
}
