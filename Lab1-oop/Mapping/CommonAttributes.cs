using Lab31.App.Domain;
using Lab31.Storage;

namespace Lab31.App.Mapping;

/// <summary>Спільні для всіх сутностей атрибути firstname/lastname.</summary>
internal sealed class CommonAttributes
{
    private const string FirstNameKey = "firstname";
    private const string LastNameKey = "lastname";

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public bool TryAccept(string key, string value)
    {
        switch (key)
        {
            case FirstNameKey: FirstName = value; return true;
            case LastNameKey: LastName = value; return true;
            default: return false;
        }
    }

    public static void Write(IPerson person, IRecordWriter writer)
    {
        writer.WriteAttribute(FirstNameKey, person.FirstName);
        writer.WriteAttribute(LastNameKey, person.LastName);
    }
}