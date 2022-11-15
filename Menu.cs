using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace EFcore
{
    internal class Menu
    {
        public char DisplayOptions()
        {
            Clear();
            WriteLine("-----------------------");
            WriteLine("Välj altenativ i menyn.");
            WriteLine("-----------------------");
            List<string> options = new List<string> { "[1]: Registrera ny student",
                                                      "[2]: Registrera ny kurs",
                                                      "[3]: Ändra Student ",
                                                      "[4]: Ändra kurs",
                                                      "[5]: Lista av alla studenter]",
                                                      "[Q]: Avsluta program]" };
           
            options.ForEach(s => WriteLine(s));
            char select = ReadKey().KeyChar;
            return select;
        }
        public void AddStudent(Student std, StudentDbContext dbCtx)
        {
            Clear();
            while (true)
            {
                try
                {
                    WriteLine("Registrera ny student");
                    WriteLine("---------------------");
                    Write("Ange förnamn: ");
                    var firstName = ReadLine();
                    if (string.IsNullOrEmpty(firstName))
                    {
                        Clear();
                        WriteLine("Du måste ange ett namn!");
                        WriteLine("Tryck valfi knapp återgå till meny...");
                        ReadKey();
                    }
                    else if (!string.IsNullOrEmpty(firstName))
                    {
                        Write("Ange efternamn: ");
                        var lastName = ReadLine();
                        Write("Ange stad: ");
                        var city = ReadLine();
                        if (dbCtx.Courses.Count() == 0) 
                        {
                            std = new Student()
                            {
                                FirstName = TitleCase(firstName),
                                LastName = TitleCase(lastName),
                                City = TitleCase(city),
                            };

                            if (Confirmation() == true)
                            {
                                dbCtx.Add(std);
                                dbCtx.SaveChanges();
                                Clear();
                            }
                        }
                        else if (dbCtx.Courses.Count() != 0)
                        {
                            WriteLine("----------");
                            WriteLine("Kurs lista");
                            WriteLine("----------");
                            foreach (var item in dbCtx.Courses)
                            {
                                WriteLine($"ID: {item.CourseID}  {item.CourseName} Start: {item.StartDate.ToString("d")} - {item.EndDate.ToString("d")}");
                            }
                            WriteLine("-------------------------------");
                            Write("Välj kurs id för att lägga till: ");
                            int id = Convert.ToInt32(ReadLine());
                            var co = dbCtx.Courses.Where(c => c.CourseID == id).First<Course>();
                            std = new Student()
                            {
                                FirstName = TitleCase(firstName),
                                LastName = TitleCase(lastName),
                                City = TitleCase(city),

                            };

                            if (Confirmation() == true)
                            {
                                std.Courses?.Add(co);
                                dbCtx.Add(std);
                                dbCtx.SaveChanges();
                                Clear();
                            }
                        }

                    }
                    break;
                }
                catch
                {
                    Clear();
                    WriteLine("Något gick fel!");
                    WriteLine("Tryck valfi knapp återgå till meny...");
                    ReadKey();
                    break;
                }
            }
        }
        public void NewCourse(StudentDbContext dbCtx, Course courses)
        {
            Clear();
            while (true)
            {
                try
                {
                    WriteLine("Registera ny kurs");
                    WriteLine("-----------------");
                    Write("Kurs namn: ");
                    var courseName = ReadLine();
                    if (string.IsNullOrEmpty(courseName))
                    {
                        WriteLine("Du måste har kurs namn!");
                        WriteLine("Tryck valfi knapp återgå till meny...");
                        ReadKey();
                        break;

                    }
                    Write("Start datum (ÅÅÅÅ-MM-DD): ");
                    var startCourse = ReadLine();
                    Write("Avsluta (ÅÅÅÅ-MM-DD): ");
                    var endCourse = ReadLine();

                    DateTimeOffset startOffset = DateTimeOffset.Parse(startCourse!);
                    DateTimeOffset endOffset = DateTimeOffset.Parse(endCourse!);
                    courses = new()
                    {
                        CourseName = courseName!,
                        StartDate = startOffset,
                        EndDate = endOffset
                    };
                    if (Confirmation() == true)
                    {
                        dbCtx.Courses.Add(courses);
                        dbCtx.SaveChanges();
                    }
                    break;
                }
                catch
                {
                    WriteLine("Något är fel!");
                    WriteLine("Tryck valfi knapp återgå till meny...");
                    ReadKey();
                    break;
                }
            }
        }
        public void EditStudent(StudentDbContext dbCtx)
        {
            Clear();
            while (true)
            {
                try
                {
                    List<string> options = new List<string> { "[1]: Ändra förnamn",
                                                              "[2]: Ändra efternamn",
                                                              "[3]: Ändra stad",
                                                              "[4]: Radera student",
                                                              "[5]: Registrera kursen till student",
                                                              "[Q]: Återgå till huvud meny"};
                    WriteLine("{0, -6} {1, -15} {2, -15} {3, -15} {4, -10}", "ID", "Namn", "Efternamn", "Stad", "Kurs");
                    WriteLine("-------------------------------------------------------------");
                    foreach (var student in dbCtx.Students.Include(sc => sc.Courses))
                    {
                        var co = string.Join(", ", student.Courses!.Select(c => c.CourseName));
                        WriteLine($"{student.StudentId,-6} {student.FirstName,-15} " +
                                  $"{student.LastName,-15} {student.City,-15} {co,-10}");
                    }
                    WriteLine("-------------------------------------------------------------");

                    Write("Ange student id du vill ändra: ");
                    int search = Convert.ToInt32(ReadLine());
                    var std = dbCtx.Students.Where(s => s.StudentId == search).First<Student>();
                    Clear();

                    WriteLine($"ID {std.StudentId} Namn: {std.FirstName} {std.LastName} Stad: {std.City}");
                    WriteLine("------------------");
                    options.ForEach(s => WriteLine(s));
                    WriteLine("------------------");
                    char select = ReadKey().KeyChar;

                    if (select == '1')
                    {
                        Write("\nNy förnamn: ");
                        var firstName = ReadLine();
                        if (string.IsNullOrEmpty(firstName))
                        {
                            Clear();
                            WriteLine("Du måste ha ett förnamn!");
                            WriteLine("Tryck valfi knapp återgå till meny...");
                            ReadKey();

                        }
                        else
                            if (Confirmation() == true)
                        {
                            std.FirstName = TitleCase(firstName);
                            dbCtx.SaveChanges();
                            Clear();
                        }
                    }
                    else if (select == '2')
                    {
                        Write("\nNy efternamn: ");
                        var lastName = ReadLine();
                        if (Confirmation() == true)
                        {
                            std.LastName = TitleCase(lastName);
                            dbCtx.SaveChanges();
                            Clear();
                        }
                    }
                    else if (select == '3')
                    {
                        Write("\nNy stad: ");
                        var city = ReadLine();

                        if (Confirmation() == true)
                        {
                            std.City = TitleCase(city);
                            dbCtx.SaveChanges();
                            Clear();

                        }
                    }
                    else if (select == '4')
                    {
                        WriteLine("\nÄr du säkert på att radera denna student?\n");
                        WriteLine($"ID {std.StudentId} Namn: {std.FirstName} {std.LastName} Stad: {std.City}");
                        if (Confirmation() == true)
                        {
                            dbCtx.Students.Remove(std);
                            dbCtx.SaveChanges();
                            Clear();
                        }
                    }
                    else if (select == '5')
                    {
                        WriteLine();
                        dbCtx.Courses.ToList()
                                     .ForEach(s => WriteLine($"Kurs id: {s.CourseID} {s.CourseName} {s.StartDate.ToString("d")} {s.EndDate.ToString("d")}"));
                        WriteLine("-----------------------");
                        Write("Välj kurs id från lista: ");
                        var selectID = Convert.ToInt32(ReadLine());
                        var co = dbCtx.Courses.Where(s => s.CourseID == selectID).First<Course>();
                        if (std.Courses!.Any(s => s.CourseID == selectID))
                        {
                            Clear();
                            WriteLine("Kursen är redan inlagd!");
                            WriteLine("Tryck valfi knapp återgå till meny...");
                            ReadKey();

                        }
                        else
                        {
                            if (Confirmation() == true)
                            {
                                std.Courses?.Add(co);
                                dbCtx.SaveChanges();
                                Clear();
                            }
                        }
                    }
                    else if (select == 'q') { }
                    break;
                }
                catch
                {
                    Clear();
                    WriteLine("Något gick fel");
                    WriteLine("Tryck valfi knapp återgå till meny...");
                    ReadKey();
                    break;
                }
            }
        }
        public void EditCourse(StudentDbContext dbCtx)
        {
            Clear();
            while (true)
            {
                try
                {
                    if (dbCtx.Courses.Count() == 0)
                    {
                        WriteLine("Det fins inga kurser i databas");
                        Write("Tryck på valfri knapp för att gå tillbaka...");
                        ReadKey();
                        break;
                    }
                    else

                        CoursesList(dbCtx);
                    Write("Välj kurs id för att ändra: ");
                    var searchID = Convert.ToInt32(ReadLine());
                    var coid = dbCtx.Courses.Where(c => c.CourseID == searchID).First<Course>();
                    Clear();
                    WriteLine($"Kurs id: {coid.CourseID} {coid.CourseName} {coid.StartDate.ToString("d")}-{coid.EndDate.ToString("d")}");

                    List<string> options = new() { "[1]: Ändra kursnamn",
                                               "[2]: Ändra startdatum & avlutdatum",
                                               "[3]: Radera kurs",
                                               "[Q]: Återgå till huvud meny" };
                    WriteLine("---------------------");
                    WriteLine("Välj altenativ i meny");
                    WriteLine("---------------------");
                    options.ForEach(s => WriteLine(s));
                    char select = ReadKey().KeyChar;
                    if (select == '1')
                    {
                        Write("\nNy kurs namn: ");
                        var courseName = ReadLine();
                        if (string.IsNullOrEmpty(courseName))
                        {
                            Clear();
                            WriteLine("Du måste ha kurs namn!");
                            WriteLine("Tryck valfi knapp återgå till meny...");
                            ReadKey();

                        }
                        else
                        {
                            if (Confirmation() == true)
                            {
                                coid.CourseName = courseName;
                                dbCtx.SaveChanges();
                                Clear();
                            }
                        }
                    }
                    else if (select == '2')
                    {
                        WriteLine("\nNy start & avslut datum: ");
                        Write("Star datum: ");
                        var newStartDate = ReadLine();
                        Write("Avslut datum: ");
                        var newEnDate = ReadLine();
                        DateTimeOffset startDate = DateTimeOffset.Parse(newStartDate!);
                        DateTimeOffset endDate = DateTimeOffset.Parse(newEnDate!);
                        if (Confirmation() == true)
                        {
                            coid.StartDate = startDate;
                            coid.EndDate = endDate;
                            dbCtx.SaveChanges();
                            Clear();
                        }
                    }
                    else if (select == '3')
                    {
                        WriteLine($"\nRedera kurs {coid.CourseName} {coid.StartDate.ToString("d")} - {coid.EndDate.ToString("d")}");
                        if (Confirmation() == true)
                        {
                            dbCtx.Remove(coid);
                            dbCtx.SaveChanges();
                            Clear();
                        }
                    }
                    else if (select == 'q') { }
                    break;
                }
                catch
                {
                    WriteLine("Något gick fel!");
                    WriteLine("Tryck valfi knapp återgå till meny...");
                    ReadKey();
                    break;
                }
            }
        }
        public void AllList(StudentDbContext dbCtx)
        {
            Clear();
            while (true)
            {
                WriteLine("----------------------------");
                WriteLine("[1]: Lista på alla studenter \n[2]: Lista på alla kurser \n[Q]: Tillbaka till huvud meny");
                WriteLine("----------------------------");
                char select = ReadKey().KeyChar;
                if (select == '1')
                {
                    Clear();
                    StudentsList(dbCtx);
                    Write("Tryck på valfri knapp för att gå tillbaka...");
                    ReadKey();
                    Clear();
                }
                else if (select == '2')
                {
                    Clear();
                    CoursesList(dbCtx);
                    Write("Tryck på valfri knapp för att gå tillbaka...");
                    ReadKey();
                    Clear();
                }
                else if (select == 'q')
                {
                    Clear();
                    break;
                }
            }
        }
        static Boolean Confirmation()
        {
            while (true)
            {
                WriteLine("-----------------------------------------------");
                WriteLine("Tryck enter för att spara / ESC för att avbryt.");
                WriteLine("-----------------------------------------------");
                ConsoleKey KeyPress = ReadKey().Key;

                if (KeyPress == ConsoleKey.Enter)
                {
                    WriteLine("\nSparade!");
                    WriteLine("Tryck valfi knapp återgå till meny...");
                    ReadKey();

                    return true;
                }
                else if (KeyPress == ConsoleKey.Escape)
                {

                    WriteLine(">Avbryts!");
                    WriteLine("Tryck valfri knapp återgå till meny...");
                    ReadKey();

                    return false;
                }
            }
        }
        static string TitleCase(string? text)
        {
            TextInfo info = new CultureInfo("en-En", false).TextInfo;
            string txt = info.ToTitleCase(text!);
            return txt;
        }
        static void StudentsList(StudentDbContext dbCtx)
        {
            WriteLine("{0, -6} {1, -15} {2, -15} {3, -15} {4, -10}", "ID", "Namn", "Efternamn", "Stad", "Kurs");
            WriteLine("-----------------------------------------------------------");
            foreach (var student in dbCtx.Students.Include(sc => sc.Courses))
            {
                var co = string.Join(", ", student.Courses!.Select(c => c.CourseName));
                WriteLine($"{student.StudentId,-6} {student.FirstName,-15} " +
                          $"{student.LastName,-15} {student.City,-15} {co,-1} ");
            }
            WriteLine("-----------------------------------------------------------");
        }
        static void CoursesList(StudentDbContext dbCtx)
        {
            WriteLine("{0, -5} {1, -13} {2, -8} {3, 13}", "ID", "Kurs", "Start", "Avslut");
            WriteLine("------------------------------------------");
            dbCtx.Courses.ToList().ForEach(c => WriteLine($"{c.CourseID,-5} {c.CourseName,-13} {c.StartDate.ToString("d"),-15} {c.EndDate.ToString("d"),-13}"));
            WriteLine("------------------------------------------");
        }
    }
}
