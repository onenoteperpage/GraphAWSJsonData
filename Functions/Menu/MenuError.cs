using System;

namespace GraphAWSJsonData.Functions.Menu
{
    public class MenuError
    {
        public static void InvalidSqliteDbPath(string sqliteDbFilePath)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Invalid SQLiteDB file path: ");
            Console.ResetColor(); 
            Console.WriteLine(sqliteDbFilePath);
            Environment.Exit(1);
        }

        public static void InvalidJsonInputPath(string jsonInputFile)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Invalid JSON filepath: ");
            Console.ResetColor();
            Console.WriteLine(jsonInputFile);
            Environment.Exit(1);
        }
    }
}
