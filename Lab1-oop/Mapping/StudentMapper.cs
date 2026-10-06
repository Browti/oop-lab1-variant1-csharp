using System.Globalization;
using Lab31.App.Domain;
using Lab31.Storage;

namespace Lab31.App.Mapping;

public sealed class StudentMapper : IEntityMapper
{
    private const string IdKey = "studentId";
    private const string CourseKey = "course";
    private const string BirthDateKey = "birthdate";
    private const string StudyHoursKey = "studyHours";
    private const string SongsKey = "songsSung";

    public string TypeName => nameof(Student);
    public bool CanMap(IPerson person) => person is Student;

    public IPerson Read(IRecordReader reader)
    {
        var common = new CommonAttributes();
        string id = "", course = "", birthDate = "", studyHours = "", songs = "";

        while (reader.NextAttribute())
        {
            if (common.TryAccept(reader.AttributeName, reader.AttributeValue)) continue;
            switch (reader.AttributeName)
            {
                case IdKey: id = reader.AttributeValue; break;
                case CourseKey: course = reader.AttributeValue; break;
                case BirthDateKey: birthDate = reader.AttributeValue; break;
                case StudyHoursKey: studyHours = reader.AttributeValue; break;
                case SongsKey: songs = reader.AttributeValue; break;
            }
        }

        return new Student(common.FirstName, common.LastName, id,
            FieldRules.ParseCourse(course), FieldRules.ParseBirthDate(birthDate),
            FieldRules.ParseOptionalNonNegativeInt(studyHours),
            FieldRules.ParseOptionalNonNegativeInt(songs));
    }

    public void Write(IPerson person, IRecordWriter writer)
    {
        var s = (Student)person;
        CommonAttributes.Write(s, writer);
        writer.WriteAttribute(IdKey, s.Id);
        writer.WriteAttribute(CourseKey, s.Course.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttribute(BirthDateKey,
            s.BirthDate.ToString(FieldRules.BirthDateFormat, CultureInfo.InvariantCulture));
        writer.WriteAttribute(StudyHoursKey, s.StudyHours.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttribute(SongsKey, s.SongsSung.ToString(CultureInfo.InvariantCulture));
    }
}