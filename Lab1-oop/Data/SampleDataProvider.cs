using Lab31.App.Domain;

namespace Lab31.App.Data;

public interface ISampleDataProvider
{
    IPerson[] Create();
}

public sealed class SampleDataProvider : ISampleDataProvider
{
    public IPerson[] Create() =>
    [
        new Student("Олексій", "Іваненко", "KB123456", 3, new DateTime(2004, 7, 15)),  // 3 курс, літо
        new Student("Марія",   "Петренко", "KB234567", 3, new DateTime(2004, 6, 2)),   // 3 курс, літо
        new Student("Андрій",  "Коваль",   "KB345678", 3, new DateTime(2004, 12, 20)), // 3 курс, зима
        new Student("Оксана",  "Шевченко", "KB456789", 1, new DateTime(2007, 8, 10)),  // 1 курс
        new Student("Дмитро",  "Мельник",  "KB567890", 3, new DateTime(2004, 8, 31)),  // 3 курс, літо
        new Student("Софія",   "Бондар",   "KB678901", 3, new DateTime(2004, 3, 14)),  // 3 курс, весна
        new Teacher("Ігор",    "Ткаченко", "T00123", "Програмування"),
        new Astronaut("Павло", "Кравець",  "AS-0007", 1200),
    ];
}