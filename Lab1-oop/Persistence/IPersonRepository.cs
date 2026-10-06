using Lab31.App.Domain;

namespace Lab31.App.Persistence;

public interface IPersonRepository
{
    string StorageLocation { get; }
    void Add(IPerson person);
    IPerson[] GetAll();
    IPerson? FindById(string id);
    bool Remove(string id); 
    void Update(IPerson person);
}