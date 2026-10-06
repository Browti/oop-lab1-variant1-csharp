namespace Lab31.Storage;

public interface IRecordReader : IDisposable
{
    string TypeName { get; }
    string ObjectName { get; }
    string AttributeName { get; }
    string AttributeValue { get; }

    /// <summary>Переходить до наступного запису; невичитані атрибути поточного пропускає.</summary>
    bool NextRecord();

    /// <summary>Читає наступний атрибут поточного запису.</summary>
    bool NextAttribute();
}