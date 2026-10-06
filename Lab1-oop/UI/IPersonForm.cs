using Lab31.App.Domain;

namespace Lab31.App.UI;

public interface IPersonForm
{
    string TypeTitle { get; }
    IPerson Read(IUserInput input);
}