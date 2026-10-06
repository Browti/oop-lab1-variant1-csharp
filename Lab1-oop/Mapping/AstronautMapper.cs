using System.Globalization;
using Lab31.App.Domain;
using Lab31.Storage;

namespace Lab31.App.Mapping;

public sealed class AstronautMapper : IEntityMapper
{
    private const string IdKey = "astronautId";
    private const string HoursKey = "flightHours";
    private const string StudyHoursKey = "studyHours";

    public string TypeName => nameof(Astronaut);
    public bool CanMap(IPerson person) => person is Astronaut;

    public IPerson Read(IRecordReader reader)
    {
        var common = new CommonAttributes();
        string id = "", hours = "", studyHours = "";

        while (reader.NextAttribute())
        {
            if (common.TryAccept(reader.AttributeName, reader.AttributeValue)) continue;
            switch (reader.AttributeName)
            {
                case IdKey: id = reader.AttributeValue; break;
                case HoursKey: hours = reader.AttributeValue; break;
                case StudyHoursKey: studyHours = reader.AttributeValue; break;
            }
        }

        return new Astronaut(common.FirstName, common.LastName, id,
            FieldRules.ParseOptionalNonNegativeInt(hours),
            FieldRules.ParseOptionalNonNegativeInt(studyHours));
    }

    public void Write(IPerson person, IRecordWriter writer)
    {
        var a = (Astronaut)person;
        CommonAttributes.Write(a, writer);
        writer.WriteAttribute(IdKey, a.Id);
        writer.WriteAttribute(HoursKey, a.FlightHours.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttribute(StudyHoursKey, a.StudyHours.ToString(CultureInfo.InvariantCulture));
    }
}