using System.Text;

namespace Counter;

class Program
{
    public static void Main()
    {
        Console.WriteLine("Напишите путь к файлу или переместите сам файл в консоль");
        WhileInput(out string fileName);
        FileManagment fileManagment = new(fileName);

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
        while (string.IsNullOrWhiteSpace(input = Console.ReadLine()) || !Path.Exists(input))
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
    public FileManagment(string filePath)
    {
        FilePath = filePath;
        string fileResultName = Path.GetFileNameWithoutExtension(FilePath) + ".Result" + ".txt";
        FileResultPath = Path.GetDirectoryName(FilePath) + $"\\{fileResultName}";
    }

    public string FilePath;

    public string FileResultPath { get; private set; }

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