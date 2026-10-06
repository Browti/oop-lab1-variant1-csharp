namespace Lab31.App.UI;

public interface IUserInput
{
    string ReadValid(string prompt, Func<string?, bool> isValid, string errorHint);
}

public static class UserInputExtensions
{
    public static string ReadName(this IUserInput input, string prompt) =>
        input.ReadValid(prompt, Lab31.App.Domain.FieldRules.IsName,
            "лише літери, апостроф і дефіс, без пробілів");
}