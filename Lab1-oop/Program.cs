using System.Text;
using Lab31.App.Behaviors;
using Lab31.App.Data;
using Lab31.App.Mapping;
using Lab31.App.Persistence;
using Lab31.App.UI;
using Lab31.Storage;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.GetEncoding(1251);   // кирилиця (укр.)

IRecordStorage storage = new FileRecordStorage(Path.Combine(AppContext.BaseDirectory, "people.txt"));

IEntityMapper[] mappers = [new StudentMapper(), new TeacherMapper(), new AstronautMapper()];
IPersonRepository repository = new FilePersonRepository(storage, mappers);

IPersonForm[] forms = [new StudentForm(), new TeacherForm(), new AstronautForm()];
IBehavior[] behaviors = [new StudyBehavior(), new TeachBehavior(), new SingBehavior()];

IPersonPresenter[] presenters = [new StudentPresenter(), new TeacherPresenter(), new AstronautPresenter()];

new ConsoleMenu(repository, forms, behaviors, new SampleDataProvider(), presenters).Run();