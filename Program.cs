using System;
using OgrenciKursTakipSistemi.Services;

class Program
{
    static void Main(string[] args)
    {
        StudentService studentService = new StudentService();
        CourseService courseService = new CourseService();

        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("===== ÖĞRENCİ KURS TAKİP SİSTEMİ =====");
            Console.WriteLine("1 - Öğrencileri Listele");
            Console.WriteLine("2 - Öğrenci Ekle");
            Console.WriteLine("3 - Öğrenci Güncelle");
            Console.WriteLine("4 - Öğrenci Sil");
            Console.WriteLine("5 - Öğrenciye Kurs Ata");
            Console.WriteLine("6 - Öğrencinin Kurslarını Listele");
            Console.WriteLine("0 - Çıkış");
            Console.Write("\nSeçiminiz: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    studentService.ListStudents();
                    Pause();
                    break;

                case "2":
                    Console.Write("Ad: ");
                    string firstName = Console.ReadLine();

                    Console.Write("Soyad: ");
                    string lastName = Console.ReadLine();

                    Console.Write("Email: ");
                    string email = Console.ReadLine();

                    studentService.AddStudent(firstName, lastName, email);
                    Pause();
                    break;

                case "3":
                    studentService.ListStudents();
                    Console.Write("ID: ");
                    int updateId = int.Parse(Console.ReadLine());

                    Console.Write("Yeni Ad: ");
                    string newFirst = Console.ReadLine();

                    Console.Write("Yeni Soyad: ");
                    string newLast = Console.ReadLine();

                    Console.Write("Yeni Email: ");
                    string newMail = Console.ReadLine();

                    studentService.UpdateStudent(updateId, newFirst, newLast, newMail);
                    Pause();
                    break;

                case "4":
                    studentService.ListStudents();
                    Console.Write("Silinecek ID: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    studentService.DeleteStudent(deleteId);
                    Pause();
                    break;

                case "5":
                    studentService.ListStudents();
                    courseService.ListCourses();

                    Console.Write("Öğrenci ID: ");
                    int studentId = int.Parse(Console.ReadLine());

                    Console.Write("Kurs ID: ");
                    int courseId = int.Parse(Console.ReadLine());

                    studentService.AssignStudentToCourse(studentId, courseId);
                    Pause();
                    break;

                case "6":
                    studentService.ListStudents();
                    Console.Write("Öğrenci ID: ");
                    int listId = int.Parse(Console.ReadLine());

                    studentService.ListCoursesOfStudent(listId);
                    Pause();
                    break;

                case "0":
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Geçersiz seçim ❌");
                    Pause();
                    break;
            }
        }
    }

    static void Pause()
    {
        Console.WriteLine("\nDevam etmek için bir tuşa bas...");
        Console.ReadKey();
    }
}


