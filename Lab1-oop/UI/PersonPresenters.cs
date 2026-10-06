using System.Globalization;
using Lab31.App.Domain;

namespace Lab31.App.UI;

public sealed class StudentPresenter : IPersonPresenter
{
    public string Title => "Студент";
    public bool CanPresent(IPerson person) => person is Student;

    public DisplayField[] GetFields(IPerson person)
    {
        var s = (Student)person;
        return
        [
            new("Прізвище", s.LastName),
            new("Ім'я", s.FirstName),
            new("Студентський квиток", s.Id),
            new("Курс", s.Course.ToString(CultureInfo.InvariantCulture)),
            new("Дата народження", s.BirthDate.ToString(FieldRules.BirthDateFormat, CultureInfo.InvariantCulture)),
            new("Годин навчання", s.StudyHours.ToString(CultureInfo.InvariantCulture)),
            new("Пісень виконано", s.SongsSung.ToString(CultureInfo.InvariantCulture)),
        ];
    }
}

public sealed class TeacherPresenter : IPersonPresenter
{
    public string Title => "Викладач";
    public bool CanPresent(IPerson person) => person is Teacher;

    public DisplayField[] GetFields(IPerson person)
    {
        var t = (Teacher)person;
        return
        [
            new("Прізвище", t.LastName),
            new("Ім'я", t.FirstName),
            new("Код викладача", t.Id),
            new("Предмет", t.Subject),
            new("Проведено занять", t.LessonsTaught.ToString(CultureInfo.InvariantCulture)),
        ];
    }
}

public sealed class AstronautPresenter : IPersonPresenter
{
    public string Title => "Космонавт";
    public bool CanPresent(IPerson person) => person is Astronaut;

    public DisplayField[] GetFields(IPerson person)
    {
        var a = (Astronaut)person;
        return
        [
            new("Прізвище", a.LastName),
            new("Ім'я", a.FirstName),
            new("Код космонавта", a.Id),
            new("Нальот, год", a.FlightHours.ToString(CultureInfo.InvariantCulture)),
            new("Годин самонавчання", a.StudyHours.ToString(CultureInfo.InvariantCulture)),
        ];
    }
}