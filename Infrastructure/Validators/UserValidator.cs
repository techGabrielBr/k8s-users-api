using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Infrastructure.Validators
{
    public static class UserValidator
    {
        public static List<string> Validate(
            string name,
            string email,
            string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add("Nome é obrigatório");

            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(email))
                errors.Add("E-mail inválido");

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Senha é obrigatória");
            }
            else
            {
                if (password.Length < 8)
                    errors.Add("Senha deve ter no mínimo 8 caracteres");

                if (!password.Any(char.IsDigit))
                    errors.Add("Senha deve conter ao menos um número");

                if (!password.Any(char.IsLetter))
                    errors.Add("Senha deve conter ao menos uma letra");

                if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                    errors.Add("Senha deve conter ao menos um caractere especial");
            }

            return errors;
        }
    }
}
