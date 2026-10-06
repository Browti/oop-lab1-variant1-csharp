using System.Text;

namespace Lab31.Storage;

internal sealed class RecordFileReader : IRecordReader
{
    private readonly StreamReader _reader;
    private bool _inRecord;

    public RecordFileReader(Stream stream) => _reader = new StreamReader(stream, Encoding.UTF8);

    public string TypeName { get; private set; } = string.Empty;
    public string ObjectName { get; private set; } = string.Empty;
    public string AttributeName { get; private set; } = string.Empty;
    public string AttributeValue { get; private set; } = string.Empty;

    public bool NextRecord()
    {
        while (_inRecord && NextAttribute()) { }   // пропускаємо невичитане

        SkipWhitespace();
        if (_reader.Peek() < 0) return false;

        TypeName = ReadToken();
        SkipWhitespace();
        ObjectName = ReadToken();
        SkipWhitespace();
        Expect('{');
        _inRecord = true;
        return true;
    }

    public bool NextAttribute()
    {
        if (!_inRecord) return false;

        SkipWhitespace();
        if (_reader.Peek() == ',')
        {
            _reader.Read();
            SkipWhitespace();
        }

        if (_reader.Peek() == '}')
        {
            _reader.Read();
            if (_reader.Peek() == ';') _reader.Read();
            _inRecord = false;
            AttributeName = AttributeValue = string.Empty;
            return false;
        }

        AttributeName = ReadQuoted();
        SkipWhitespace();
        Expect(':');
        SkipWhitespace();
        AttributeValue = ReadQuoted();
        return true;
    }

    public void Dispose() => _reader.Dispose();

    private void SkipWhitespace()
    {
        int c;
        while ((c = _reader.Peek()) >= 0 && char.IsWhiteSpace((char)c)) _reader.Read();
    }

    private void Expect(char expected)
    {
        if (_reader.Read() != expected)
            throw new FormatException($"Помилка формату файлу: очікувався символ '{expected}'.");
    }

    private string ReadToken()
    {
        var sb = new StringBuilder();
        int c;
        while ((c = _reader.Peek()) >= 0 && !char.IsWhiteSpace((char)c) && c != '{')
            sb.Append((char)_reader.Read());
        if (sb.Length == 0) throw new FormatException("Помилка формату файлу: очікувалась назва.");
        return sb.ToString();
    }

    private string ReadQuoted()
    {
        Expect('"');
        var sb = new StringBuilder();
        while (true)
        {
            int c = _reader.Read();
            if (c < 0) throw new FormatException("Несподіваний кінець файлу.");
            if (c == '"') break;
            if (c == '\\')
            {
                c = _reader.Read();
                if (c < 0) throw new FormatException("Несподіваний кінець файлу.");
            }
            sb.Append((char)c);
        }
        return sb.ToString();
    }
}