using Lab31.App.Behaviors;
using Lab31.App.Common;
using Lab31.App.Criteria;
using Lab31.App.Data;
using Lab31.App.Domain;
using Lab31.App.Persistence;

namespace Lab31.App.UI;

public sealed class ConsoleMenu : IUserInput
{
    private readonly IPersonRepository _repository;
    private readonly IPersonForm[] _forms;
    private readonly IBehavior[] _behaviors;
    private readonly ISampleDataProvider _sampleData;

    public ConsoleMenu(IPersonRepository repository, IPersonForm[] forms,
                       IBehavior[] behaviors, ISampleDataProvider sampleData)
    {
        _repository = repository;
        _forms = forms;
        _behaviors = behaviors;
        _sampleData = sampleData;
    }

    public void Run()
    {
        Console.WriteLine($"Файл даних: {_repository.StorageLocation}");
        bool exit = false;
        while (!exit)
        {
            PrintMenu();
            string choice = Prompt("Ваш вибір: ");
            try
            {
                exit = Handle(choice);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException
                                          or FormatException or IOException)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }

    public string ReadValid(string prompt, Func<string?, bool> isValid, string errorHint)
    {
        while (true)
        {
            string value = Prompt(prompt);
            if (isValid(value)) return value;
            Console.WriteLine($"  Некоректне значення: {errorHint}");
        }
    }

    private bool Handle(string choice)
    {
        switch (choice)
        {
            case "1": AddPerson(); break;
            case "2": PrintPeople(_repository.GetAll()); break;
            case "3": FindByLastName(); break;
            case "4": FindById(); break;
            case "5": RemovePerson(); break;
            case "6": ExecuteBehavior(); break;
            case "7": ShowVariantTask(); break;
            case "8": SeedSampleData(); break;
            case "0": return true;
            default: Console.WriteLine("Невідома команда."); break;
        }
        return false;
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Лабораторна 1 (варіант 1) ===");
        Console.WriteLine("1. Додати особу");
        Console.WriteLine("2. Показати всіх з файлу");
        Console.WriteLine("3. Знайти за прізвищем");
        Console.WriteLine("4. Знайти за ідентифікатором");
        Console.WriteLine("5. Видалити за ідентифікатором");
        Console.WriteLine("6. Виконати дію (Study / Teach / Sing)");
        Console.WriteLine("7. Завдання: студенти 3-го курсу, народжені влітку");
        Console.WriteLine("8. Заповнити демонстраційними даними");
        Console.WriteLine("0. Вихід");
    }

    private void AddPerson()
    {
        for (int i = 0; i < _forms.Length; i++)
            Console.WriteLine($"  {i + 1}. {_forms[i].TypeTitle}");

        IPersonForm form = _forms[ReadNumber("Тип особи: ", 1, _forms.Length) - 1];
        IPerson person = form.Read(this);
        _repository.Add(person);
        Console.WriteLine("Додано:");
        ShowPerson(person);
    }

    private void FindByLastName()
    {
        string last = ReadValid("Прізвище: ", Lab31.App.Domain.FieldRules.IsName, "лише літери");
        PrintPeople(_repository.GetAll().Filter(new LastNameCriteria(last)));
    }

    private void FindById()
    {
        string id = ReadValid("Ідентифікатор: ", v => !string.IsNullOrWhiteSpace(v), "не порожній");
        IPerson? person = _repository.FindById(id);
        if (person is null) Console.WriteLine("Особу не знайдено.");
        else ShowPerson(person);
    }

    private void RemovePerson()
    {
        string id = ReadValid("Ідентифікатор: ", v => !string.IsNullOrWhiteSpace(v), "не порожній");
        Console.WriteLine(_repository.Remove(id) ? "Видалено." : "Особу не знайдено.");
    }

    private void ExecuteBehavior()
    {
        string id = ReadValid("Ідентифікатор особи: ", v => !string.IsNullOrWhiteSpace(v), "не порожній");
        IPerson? person = _repository.FindById(id);
        if (person is null)
        {
            Console.WriteLine("Особу не знайдено.");
            return;
        }

        IBehavior[] available = [];
        foreach (IBehavior b in _behaviors)
            if (b.AppliesTo(person)) ArrayUtil.Append(ref available, b);

        if (available.Length == 0)
        {
            Console.WriteLine("Для цієї особи немає доступних дій.");
            return;
        }

        for (int i = 0; i < available.Length; i++)
            Console.WriteLine($"  {i + 1}. {available[i].Title}");

        IBehavior behavior = available[ReadNumber("Дія: ", 1, available.Length) - 1];
        int times = ReadNumber("Кількість повторень (1-20): ", 1, 20);

        for (int i = 0; i < times; i++) behavior.Execute(person);
        _repository.Update(person);   // стан зберігається у файлі
        Console.WriteLine($"{person.LastName}: {behavior.DescribeResult(person)}");
    }

    private void ShowVariantTask()
    {
        IPerson[] matches = _repository.GetAll().Filter(
            new AllOfCriteria(new StudentCourseCriteria(3), new StudentBornInSummerCriteria()));

        Console.WriteLine($"Студентів 3-го курсу, народжених влітку: {matches.Length}");
        PrintPeople(matches);
    }

    private void SeedSampleData()
    {
        int added = 0;
        foreach (IPerson p in _sampleData.Create())
        {
            if (_repository.FindById(p.Id) is not null) continue;
            _repository.Add(p);
            added++;
        }
        Console.WriteLine($"Додано записів: {added}.");
    }

    private void PrintPeople(IPerson[] people)
    {
        if (people.Length == 0)
        {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }
        foreach (IPerson p in people)
        {
            ShowPerson(p);
            Console.WriteLine();
        }
    }

    private void ShowPerson(IPerson person)
    {
        foreach (IPersonPresenter presenter in _presenters)
        {
            if (!presenter.CanPresent(person)) continue;

            Console.WriteLine($"[{presenter.Title}]");
            foreach (DisplayField field in presenter.GetFields(person))
                Console.WriteLine($"  {field.Label}: {field.Value}");   // кожен атрибут окремо
            return;
        }
        Console.WriteLine($"  {person.LastName} {person.FirstName}");
    }

    private int ReadNumber(string prompt, int min, int max)
    {
        string s = ReadValid(prompt, v => int.TryParse(v, out int n) && n >= min && n <= max,
            $"число від {min} до {max}");
        return int.Parse(s);
    }

    private static string Prompt(string text)
    {
        Console.Write(text);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
    private readonly IPersonPresenter[] _presenters;

    public ConsoleMenu(IPersonRepository repository, IPersonForm[] forms,
                       IBehavior[] behaviors, ISampleDataProvider sampleData,
                       IPersonPresenter[] presenters)
    {
        _repository = repository;
        _forms = forms;
        _behaviors = behaviors;
        _sampleData = sampleData;
        _presenters = presenters;
    } 
}
