namespace Lab31.App.Domain;

public interface ICanStudy { int StudyHours { get; } void Study(); }
public interface ICanTeach { int LessonsTaught { get; } void Teach(); }
public interface ICanSing { int SongsSung { get; } void Sing(); }
