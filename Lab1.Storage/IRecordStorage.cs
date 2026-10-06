namespace Lab31.Storage;

public interface IRecordStorage
{
    string Location { get; }
    IRecordWriter OpenAppend();
    IRecordReader OpenRead();
    bool Delete(string objectName);
}