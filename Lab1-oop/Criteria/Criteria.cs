using Lab31.App.Common;
using Lab31.App.Domain;

namespace Lab31.App.Criteria;

public interface IPersonCriteria
{
    bool IsSatisfiedBy(IPerson person);
}

public sealed class LastNameCriteria : IPersonCriteria
{
    private readonly string _lastName;
    public LastNameCriteria(string lastName) => _lastName = lastName;

    public bool IsSatisfiedBy(IPerson person) =>
        string.Equals(person.LastName, _lastName, StringComparison.OrdinalIgnoreCase);
}

public sealed class StudentCourseCriteria : IPersonCriteria
{
    private readonly int _course;
    public StudentCourseCriteria(int course) => _course = course;

    public bool IsSatisfiedBy(IPerson person) => person is Student s && s.Course == _course;
}

public sealed class StudentBornInSummerCriteria : IPersonCriteria
{
    public bool IsSatisfiedBy(IPerson person) => person is Student s && s.WasBornInSummer();
}

public sealed class AllOfCriteria : IPersonCriteria
{
    private readonly IPersonCriteria[] _criteria;
    public AllOfCriteria(params IPersonCriteria[] criteria) => _criteria = criteria;

    public bool IsSatisfiedBy(IPerson person)
    {
        foreach (IPersonCriteria c in _criteria)
            if (!c.IsSatisfiedBy(person)) return false;
        return true;
    }
}

public static class PersonCriteriaExtensions
{
    public static IPerson[] Filter(this IPerson[] people, IPersonCriteria criteria)
    {
        IPerson[] result = [];
        foreach (IPerson p in people)
            if (criteria.IsSatisfiedBy(p)) ArrayUtil.Append(ref result, p);
        return result;
    }
}