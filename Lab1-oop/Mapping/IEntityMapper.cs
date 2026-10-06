using Lab31.App.Domain;
using Lab31.Storage;

namespace Lab31.App.Mapping;

public interface IEntityMapper
{
    string TypeName { get; }
    bool CanMap(IPerson person);
    IPerson Read(IRecordReader reader);                 // поелементно
    void Write(IPerson person, IRecordWriter writer);   // поелементно
}