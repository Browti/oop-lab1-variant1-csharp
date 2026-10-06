namespace Lab31.App.Domain;

public interface IPerson
{
    string FirstName { get; }
    string LastName { get; }
    string Id { get; }          // унікальний ідентифікатор
    string ObjectName { get; }  // назва об'єкта у файлі
}