namespace Lab31.App.Domain;

public sealed partial class Student : Person, ICanStudy, ICanSing
{
    public Student(string firstName, string lastName, string studentId, int course, DateTime birthDate,
                   int studyHours = 0, int songsSung = 0)
        : base(firstName, lastName, FieldRules.RequireStudentId(studentId))
    {
        Course = FieldRules.RequireCourse(course);
        BirthDate = FieldRules.RequireBirthDate(birthDate);
        StudyHours = FieldRules.RequireNonNegative(studyHours, nameof(studyHours));
        SongsSung = FieldRules.RequireNonNegative(songsSung, nameof(songsSung));
    }

    public int Course { get; }
    public DateTime BirthDate { get; }
    public int StudyHours { get; private set; }
    public int SongsSung { get; private set; }

    public void Study() => StudyHours++;   // навчання у ВНЗ
    public void Sing() => SongsSung++;     // додаткове вміння за варіантом

    public bool WasBornInSummer() => BirthDate.Month is >= 6 and <= 8;
}