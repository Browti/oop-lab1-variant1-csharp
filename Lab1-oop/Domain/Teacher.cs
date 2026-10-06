namespace Lab31.App.Domain;

public sealed class Teacher : Person, ICanTeach
{
    public const string DefaultSubject = "Не вказано";

    public Teacher(string firstName, string lastName, string employeeId,
                   string? subject = null, int lessonsTaught = 0)
        : base(firstName, lastName, FieldRules.RequireEmployeeId(employeeId))
    {
        Subject = string.IsNullOrWhiteSpace(subject) ? DefaultSubject : FieldRules.RequireSubject(subject);
        LessonsTaught = FieldRules.RequireNonNegative(lessonsTaught, nameof(lessonsTaught));
    }

    public string Subject { get; }
    public int LessonsTaught { get; private set; }

    public void Teach() => LessonsTaught++;
}