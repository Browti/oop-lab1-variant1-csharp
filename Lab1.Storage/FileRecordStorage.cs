namespace Lab31.Storage;

public sealed class FileRecordStorage : IRecordStorage
{
    public FileRecordStorage(string path) => Location = path;

    public string Location { get; }

    public IRecordWriter OpenAppend() =>
        new RecordFileWriter(new FileStream(Location, FileMode.Append, FileAccess.Write, FileShare.Read));

    public IRecordReader OpenRead() =>
        File.Exists(Location)
            ? new RecordFileReader(new FileStream(Location, FileMode.Open, FileAccess.Read, FileShare.Read))
            : new RecordFileReader(Stream.Null);

    /// <summary>Видаляє запис за назвою об'єкта: переписує файл без нього (поелементне копіювання).</summary>
    public bool Delete(string objectName)
    {
        if (!File.Exists(Location)) return false;

        string temp = Location + ".tmp";
        bool deleted = false;

        using (IRecordReader reader = OpenRead())
        using (var writer = new RecordFileWriter(new FileStream(temp, FileMode.Create, FileAccess.Write)))
        {
            while (reader.NextRecord())
            {
                if (reader.ObjectName == objectName)
                {
                    deleted = true;
                    continue;
                }

                writer.BeginRecord(reader.TypeName, reader.ObjectName);
                while (reader.NextAttribute())
                    writer.WriteAttribute(reader.AttributeName, reader.AttributeValue);
                writer.EndRecord();
            }
        }

        if (deleted) File.Move(temp, Location, overwrite: true);
        else File.Delete(temp);
        return deleted;
    }
}