using System.Text;

namespace Counter;

class Program
{
    public static void Main()
    {
        Console.WriteLine("Напишите название файла(формат .txt) в той же директории что и программа");
        WhileInput(out string fileName);

        FileManagment fileManagment = new(fileName);
        Console.WriteLine(fileName);
        Console.WriteLine(fileManagment.FilePath);
        List<List<int>> data = fileManagment.ReadData();
        Console.WriteLine("Напишите число-цель: ");
        WhileInput(out int target);
        foreach (List<int> row in data)
        {
            fileManagment.WriteData(Counter.Solution([.. row], target));
        }
    }

    public static void WhileInput(out string input)
    {
        while (string.IsNullOrWhiteSpace(input = Console.ReadLine()))
        {
            Console.WriteLine("Введите заного");
        }
    }

    public static void WhileInput(out int input)
    {
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Введите заного");
        }
    }
}

public class FileManagment
{
    public FileManagment(string fileName)
    {
        _fileName = fileName;
        FilePath = GetFullPath(_fileName);


        string[] splitFileName = _fileName.Split('.', StringSplitOptions.RemoveEmptyEntries);
        string stringResultPath = string.Join('.', splitFileName[0], "Result", splitFileName[1]);

        Console.WriteLine(stringResultPath);

        FileResultPath = GetFullPath(stringResultPath);
    }

    private string _fileName;
    public string FileName
    {
        get { return _fileName; }
        set
        {
            _fileName = value;
            FilePath = GetFullPath(_fileName);

            string[] splitFileName = _fileName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            string stringResultPath = string.Join('.', splitFileName[0], "Result", splitFileName[1]);

            FileResultPath = GetFullPath(stringResultPath);
        }
    }

    public string FilePath { get; private set; }

    public string FileResultPath { get; private set; }

    private static string GetFullPath(string fileName)
    {
        string[] directoryWord = Directory.GetCurrentDirectory().Split('\\', StringSplitOptions.RemoveEmptyEntries);
        return string.Join('\\', directoryWord[0..(directoryWord.Length - 4)]) + $"\\{fileName}";
    }

    public List<List<int>> ReadData()
    {
        using StreamReader sr = new(FilePath);
        List<List<int>> result = [];
        List<int> numbers = [];
        string[] strNumbers = [];
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                strNumbers = line.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
                numbers.AddRange(Array.ConvertAll(strNumbers, int.Parse));
            } else
            {
                result.Add([.. numbers]);
                numbers.Clear();
            }
        }
        result.Add([.. numbers]);
        numbers.Clear();
        return result;
    }

    public void WriteData(string result)
    {
        File.AppendAllText(FileResultPath, result, Encoding.UTF8);
    }
}

public class Counter
{
    public static string Solution(int[] array, int target)
    {
        int count = 0;
        int step = 0;
        List<int> stepResult = [];
        for (int i = 0; i < array.Length; i++)
        {
            step++;
            if (array[i] == target)
            {
                count++;
                stepResult.Add(step);
                step = 0;
            }
        }
        StringBuilder result = new();
        result.Append($"Количество повторений: {count}\n");
        result.Append("Промежуток между числом\n");
        foreach (int number in stepResult)
        {
            result.Append(number != stepResult[^1] ? $"{number}, ": number);
        }
        result.Append('\n');
        return result.ToString();
    }
}