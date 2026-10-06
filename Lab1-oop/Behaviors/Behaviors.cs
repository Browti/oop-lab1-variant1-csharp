using Lab31.App.Domain;

namespace Lab31.App.Behaviors;

public interface IBehavior
{
    string Title { get; }
    bool AppliesTo(IPerson person);
    void Execute(IPerson person);
    string DescribeResult(IPerson person);
}

public abstract class Behavior<TAbility> : IBehavior where TAbility : class
{
    public abstract string Title { get; }

    public bool AppliesTo(IPerson person) => person is TAbility;
    public void Execute(IPerson person) => Run((TAbility)person);
    public string DescribeResult(IPerson person) => Describe((TAbility)person);

    protected abstract void Run(TAbility ability);
    protected abstract string Describe(TAbility ability);
}

public sealed class StudyBehavior : Behavior<ICanStudy>
{
    public override string Title => "Навчатися (Study)";
    protected override void Run(ICanStudy a) => a.Study();
    protected override string Describe(ICanStudy a) => $"годин навчання: {a.StudyHours}";
}

public sealed class TeachBehavior : Behavior<ICanTeach>
{
    public override string Title => "Викладати (Teach)";
    protected override void Run(ICanTeach a) => a.Teach();
    protected override string Describe(ICanTeach a) => $"проведено занять: {a.LessonsTaught}";
}

public sealed class SingBehavior : Behavior<ICanSing>
{
    public override string Title => "Співати (Sing)";
    protected override void Run(ICanSing a) => a.Sing();
    protected override string Describe(ICanSing a) => $"виконано пісень: {a.SongsSung}";
}