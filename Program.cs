using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Console;

namespace EFcore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Student students = new();
            StudentDbContext dbCtx = new();
            Course course = new();
            Menu selections = new();
            while (true)
            {
                char selectedIndex = selections.DisplayOptions();

                switch (selectedIndex)
                {
                    case '1':
                        selections.AddStudent(students, dbCtx);
                        break;
                    case '2':
                        selections.NewCourse(dbCtx, course);
                        break;
                    case '3':
                        selections.EditStudent(dbCtx);
                        break;
                    case '4':
                        selections.EditCourse(dbCtx);
                        break;
                    case '5':
                        selections.AllList(dbCtx);
                        break;
                    case 'q':
                        Clear();
                        WriteLine("\nProgram avslutas...");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
