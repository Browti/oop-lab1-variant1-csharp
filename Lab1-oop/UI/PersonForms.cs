using Lab31.App.Domain;

namespace Lab31.App.UI;

public sealed class StudentForm : IPersonForm
{
    public string TypeTitle => "Студент";

    public IPerson Read(IUserInput input)
    {
        string last = input.ReadName("Прізвище: ");
        string first = input.ReadName("Ім'я: ");
        string id = input.ReadValid("Студентський квиток (напр. KB123456): ",
            FieldRules.IsStudentId, "2 великі латинські літери + 6 цифр");
        int course = FieldRules.ParseCourse(input.ReadValid("Курс (1-6): ",
            FieldRules.IsCourse, "число від 1 до 6"));
        DateTime birth = FieldRules.ParseBirthDate(input.ReadValid("Дата народження (ДД-ММ РРРР): ",
            FieldRules.IsBirthDate, "формат ДД-ММ РРРР, напр. 15-07 2004"));

        return new Student(first, last, id, course, birth);
    }
}

public sealed class TeacherForm : IPersonForm
{
    public string TypeTitle => "Викладач";

    public IPerson Read(IUserInput input)
    {
        string last = input.ReadName("Прізвище: ");
        string first = input.ReadName("Ім'я: ");
        string id = input.ReadValid("Код викладача (напр. T00123): ",
            FieldRules.IsEmployeeId, "літера T + 5 цифр");
        string subject = input.ReadValid("Предмет (Enter - пропустити): ",
            v => string.IsNullOrEmpty(v) || FieldRules.IsSubject(v), "лише літери");

        return new Teacher(first, last, id, subject);
    }
}

public sealed class AstronautForm : IPersonForm
{
    public string TypeTitle => "Космонавт";

    public IPerson Read(IUserInput input)
    {
        string last = input.ReadName("Прізвище: ");
        string first = input.ReadName("Ім'я: ");
        string id = input.ReadValid("Код космонавта (напр. AS-0007): ",
            FieldRules.IsAstronautId, "AS- + 4 цифри");
        string hours = input.ReadValid("Нальот, год (Enter - пропустити): ",
            v => string.IsNullOrEmpty(v) || FieldRules.IsNonNegativeInt(v), "ціле число");

        int flightHours = string.IsNullOrEmpty(hours) ? 0 : FieldRules.ParseNonNegativeInt(hours);
        return new Astronaut(first, last, id, flightHours);
    }
}