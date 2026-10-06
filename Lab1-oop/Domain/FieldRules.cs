using System.Globalization;
using System.Text.RegularExpressions;

namespace Lab31.App.Domain;

public static class FieldRules
{
    public const string BirthDateFormat = "dd-MM yyyy";   // ХХ-ХХ ХХХХ

    private static readonly Regex NameRx = new(@"^\p{L}+(?:[-'’]\p{L}+)*$");
    private static readonly Regex SubjectRx = new(@"^\p{L}[\p{L} '’-]{0,49}$");
    private static readonly Regex StudentIdRx = new(@"^[A-Z]{2}\d{6}$");
    private static readonly Regex EmployeeIdRx = new(@"^T\d{5}$");
    private static readonly Regex AstronautIdRx = new(@"^AS-\d{4}$");
    private static readonly Regex CourseRx = new(@"^[1-6]$");
    private static readonly Regex HoursRx = new(@"^\d{1,6}$");
    private static readonly Regex BirthDateRx =
        new(@"^(0[1-9]|[12]\d|3[01])-(0[1-9]|1[0-2]) \d{4}$");

    // ---------- перевірки (для консольного вводу) ----------
    public static bool IsName(string? v) => v is not null && NameRx.IsMatch(v);
    public static bool IsSubject(string? v) => v is not null && SubjectRx.IsMatch(v);
    public static bool IsStudentId(string? v) => v is not null && StudentIdRx.IsMatch(v);
    public static bool IsEmployeeId(string? v) => v is not null && EmployeeIdRx.IsMatch(v);
    public static bool IsAstronautId(string? v) => v is not null && AstronautIdRx.IsMatch(v);
    public static bool IsCourse(string? v) => v is not null && CourseRx.IsMatch(v);
    public static bool IsNonNegativeInt(string? v) => v is not null && HoursRx.IsMatch(v);
    public static bool IsBirthDate(string? v) =>
        v is not null && BirthDateRx.IsMatch(v) && TryParseBirthDate(v, out _);

    // ---------- перетворення рядка ----------
    public static int ParseCourse(string? v) =>
        IsCourse(v) ? int.Parse(v!, CultureInfo.InvariantCulture) : throw Invalid("course");

    public static int ParseNonNegativeInt(string? v) =>
        IsNonNegativeInt(v) ? int.Parse(v!, CultureInfo.InvariantCulture) : throw Invalid("number");
    public static int ParseOptionalNonNegativeInt(string? v) =>
    string.IsNullOrEmpty(v) ? 0 : ParseNonNegativeInt(v);
    public static DateTime ParseBirthDate(string? v)
    {
        if (v is not null && BirthDateRx.IsMatch(v) && TryParseBirthDate(v, out DateTime d)) return d;
        throw Invalid("birthdate");
    }

    // ---------- обов'язкові значення (для конструкторів сутностей) ----------
    public static string RequireName(string? v, string field) => IsName(v) ? v! : throw Invalid(field);
    public static string RequireSubject(string? v) => IsSubject(v) ? v! : throw Invalid("subject");
    public static string RequireStudentId(string? v) => IsStudentId(v) ? v! : throw Invalid("studentId");
    public static string RequireEmployeeId(string? v) => IsEmployeeId(v) ? v! : throw Invalid("employeeId");
    public static string RequireAstronautId(string? v) => IsAstronautId(v) ? v! : throw Invalid("astronautId");

    public static int RequireCourse(int c) =>
        IsCourse(c.ToString(CultureInfo.InvariantCulture)) ? c : throw Invalid("course");

    public static int RequireNonNegative(int v, string field) => v >= 0 ? v : throw Invalid(field);

    public static DateTime RequireBirthDate(DateTime d) =>
        IsBirthDateInRange(d) ? d.Date : throw Invalid("birthdate");

    private static bool TryParseBirthDate(string v, out DateTime d) =>
        DateTime.TryParseExact(v, BirthDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out d)
        && IsBirthDateInRange(d);

    private static bool IsBirthDateInRange(DateTime d) => d.Year >= 1900 && d.Date <= DateTime.Today;

    private static ArgumentException Invalid(string field) =>
        new($"Некоректне значення поля '{field}'.", field);
}