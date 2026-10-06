using Lab31.App.Common;
using Lab31.App.Domain;
using Lab31.App.Mapping;
using Lab31.Storage;

namespace Lab31.App.Persistence;

public sealed class FilePersonRepository : IPersonRepository
{
    private readonly IRecordStorage _storage;
    private readonly IEntityMapper[] _mappers;

    public FilePersonRepository(IRecordStorage storage, IEntityMapper[] mappers)
    {
        _storage = storage;
        _mappers = mappers;
    }

    public string StorageLocation => _storage.Location;

    public void Add(IPerson person)
    {
        IEntityMapper mapper = FindMapper(person)
            ?? throw new InvalidOperationException($"Немає мапера для типу {person.GetType().Name}.");

        if (FindById(person.Id) is not null)
            throw new InvalidOperationException($"Особа з ідентифікатором {person.Id} вже існує.");

        using IRecordWriter writer = _storage.OpenAppend();
        writer.BeginRecord(mapper.TypeName, person.ObjectName);
        mapper.Write(person, writer);
        writer.EndRecord();
    }

    public IPerson[] GetAll()
    {
        IPerson[] result = [];
        using IRecordReader reader = _storage.OpenRead();

        while (reader.NextRecord())
        {
            IEntityMapper? mapper = FindMapper(reader.TypeName);
            if (mapper is null) continue;                    // невідомий тип пропускаємо
            ArrayUtil.Append(ref result, mapper.Read(reader));
        }
        return result;
    }

    public IPerson? FindById(string id)
    {
        foreach (IPerson p in GetAll())
            if (string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase)) return p;
        return null;
    }

    public bool Remove(string id)
    {
        IPerson? person = FindById(id);
        return person is not null && _storage.Delete(person.ObjectName);
    }
    public void Update(IPerson person)
    {
        if (!Remove(person.Id))
            throw new InvalidOperationException($"Особу з ідентифікатором {person.Id} не знайдено.");
        Add(person);
    }
    private IEntityMapper? FindMapper(IPerson person)
    {
        foreach (IEntityMapper m in _mappers) if (m.CanMap(person)) return m;
        return null;
    }

    private IEntityMapper? FindMapper(string typeName)
    {
        foreach (IEntityMapper m in _mappers) if (m.TypeName == typeName) return m;
        return null;
    }
}