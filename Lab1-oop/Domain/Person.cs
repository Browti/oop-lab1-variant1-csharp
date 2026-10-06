namespace Lab31.App.Domain;

public abstract class Person : IPerson
{
    protected Person(string firstName, string lastName, string id)
    {
        FirstName = FieldRules.RequireName(firstName, nameof(firstName));
        LastName = FieldRules.RequireName(lastName, nameof(lastName));
        Id = id;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string Id { get; }
    public string ObjectName => $"{LastName}{FirstName}_{Id}";

}