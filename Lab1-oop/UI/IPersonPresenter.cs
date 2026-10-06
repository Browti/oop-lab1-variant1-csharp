using Lab31.App.Domain;

namespace Lab31.App.UI;

public readonly record struct DisplayField(string Label, string Value);

public interface IPersonPresenter
{
    string Title { get; }
    bool CanPresent(IPerson person);
    DisplayField[] GetFields(IPerson person);
}