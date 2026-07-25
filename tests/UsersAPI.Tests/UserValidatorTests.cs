using UsersAPI.Infrastructure.Validators;
using Xunit;

namespace UsersAPI.Tests
{
    public class UserValidatorTests
    {
        [Fact]
        public void Validate_ComDadosValidos_NaoRetornaErros()
        {
            var errors = UserValidator.Validate(
                "Gabriel",
                "gabriel@teste.com",
                "Senha@123");

            Assert.Empty(errors);
        }

        [Fact]
        public void Validate_ComNomeVazio_RetornaErro()
        {
            var errors = UserValidator.Validate(
                "",
                "gabriel@teste.com",
                "Senha@123");

            Assert.Contains("Nome é obrigatório", errors);
        }

        [Theory]
        [InlineData("email-invalido")]
        [InlineData("sem-arroba.com")]
        public void Validate_ComEmailInvalido_RetornaErro(string email)
        {
            var errors = UserValidator.Validate(
                "Gabriel",
                email,
                "Senha@123");

            Assert.Contains("E-mail inválido", errors);
        }

        [Fact]
        public void Validate_ComSenhaCurta_RetornaErro()
        {
            var errors = UserValidator.Validate(
                "Gabriel",
                "gabriel@teste.com",
                "S@1a");

            Assert.Contains("Senha deve ter no mínimo 8 caracteres", errors);
        }

        [Fact]
        public void Validate_ComSenhaSemNumero_RetornaErro()
        {
            var errors = UserValidator.Validate(
                "Gabriel",
                "gabriel@teste.com",
                "Senha@abc");

            Assert.Contains("Senha deve conter ao menos um número", errors);
        }

        [Fact]
        public void Validate_ComSenhaSemCaractereEspecial_RetornaErro()
        {
            var errors = UserValidator.Validate(
                "Gabriel",
                "gabriel@teste.com",
                "Senha1234");

            Assert.Contains("Senha deve conter ao menos um caractere especial", errors);
        }
    }
}
