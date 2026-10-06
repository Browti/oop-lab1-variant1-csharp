namespace Lab31.App.Domain;

public sealed class Astronaut : Person, ICanStudy
{
    public Astronaut(string firstName, string lastName, string astronautId,
                     int flightHours = 0, int studyHours = 0)
        : base(firstName, lastName, FieldRules.RequireAstronautId(astronautId))
    {
        FlightHours = FieldRules.RequireNonNegative(flightHours, nameof(flightHours));
        StudyHours = FieldRules.RequireNonNegative(studyHours, nameof(studyHours));
    }

    public int FlightHours { get; }
    public int StudyHours { get; private set; }

    public void Study() => StudyHours++;     // самонавчання
}