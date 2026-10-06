namespace Lab31.Storage;

public interface IRecordWriter : IDisposable
{
    void BeginRecord(string typeName, string objectName);
    void WriteAttribute(string name, string value);
    void EndRecord();
}