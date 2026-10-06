using System.Text;

namespace Lab31.Storage;

internal sealed class RecordFileWriter : IRecordWriter
{
    private readonly StreamWriter _writer;
    private bool _recordOpen;
    private bool _firstAttribute;

    public RecordFileWriter(Stream stream) =>
        _writer = new StreamWriter(stream, new UTF8Encoding(false));

    public void BeginRecord(string typeName, string objectName)
    {
        if (_recordOpen) throw new InvalidOperationException("Попередній запис не завершено.");
        EnsureToken(typeName);
        EnsureToken(objectName);
        _writer.WriteLine($"{typeName} {objectName}");
        _writer.Write("{ ");
        _recordOpen = true;
        _firstAttribute = true;
    }

    public void WriteAttribute(string name, string value)
    {
        if (!_recordOpen) throw new InvalidOperationException("Запис не розпочато.");
        if (!_firstAttribute)
        {
            _writer.WriteLine(",");
            _writer.Write("  ");
        }
        _writer.Write($"\"{Escape(name)}\": \"{Escape(value)}\"");
        _firstAttribute = false;
    }

    public void EndRecord()
    {
        if (!_recordOpen) throw new InvalidOperationException("Запис не розпочато.");
        _writer.WriteLine("};");
        _recordOpen = false;
    }

    public void Dispose() => _writer.Dispose();

    private static string Escape(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    private static void EnsureToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Порожня назва типу/об'єкта.");
        foreach (char c in token)
            if (char.IsWhiteSpace(c) || c == '{')
                throw new ArgumentException($"Недопустимий символ у назві '{token}'.");
    }
}