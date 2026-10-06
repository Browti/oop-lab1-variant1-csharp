using System.Globalization;
using Lab31.App.Domain;
using Lab31.Storage;

namespace Lab31.App.Mapping;

public sealed class TeacherMapper : IEntityMapper
{
    private const string IdKey = "employeeId";
    private const string SubjectKey = "subject";
    private const string LessonsKey = "lessonsTaught";

    public string TypeName => nameof(Teacher);
    public bool CanMap(IPerson person) => person is Teacher;

    public IPerson Read(IRecordReader reader)
    {
        var common = new CommonAttributes();
        string id = "", lessons = "";
        string? subject = null;

        while (reader.NextAttribute())
        {
            if (common.TryAccept(reader.AttributeName, reader.AttributeValue)) continue;
            switch (reader.AttributeName)
            {
                case IdKey: id = reader.AttributeValue; break;
                case SubjectKey: subject = reader.AttributeValue; break;
                case LessonsKey: lessons = reader.AttributeValue; break;
            }
        }

        return new Teacher(common.FirstName, common.LastName, id, subject,
            FieldRules.ParseOptionalNonNegativeInt(lessons));
    }

    public void Write(IPerson person, IRecordWriter writer)
    {
        var t = (Teacher)person;
        CommonAttributes.Write(t, writer);
        writer.WriteAttribute(IdKey, t.Id);
        writer.WriteAttribute(SubjectKey, t.Subject);
        writer.WriteAttribute(LessonsKey, t.LessonsTaught.ToString(CultureInfo.InvariantCulture));
    }
}