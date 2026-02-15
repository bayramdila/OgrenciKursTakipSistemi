using System;
using System.Data.SqlClient;
using OgrenciKursTakipSistemi.Database;

namespace OgrenciKursTakipSistemi.Services
{
    /// <summary>
    /// Öğrenci işlemlerini yöneten servis sınıfı.
    /// CRUD ve many-to-many işlemleri burada yapılır.
    /// </summary>
    public class StudentService
    {
        // 🔹 Öğrencileri Listeleme
        public void ListStudents()
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = "SELECT Id, FirstName, LastName, Email FROM Students";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(
                            $"{reader["Id"]} - {reader["FirstName"]} {reader["LastName"]} ({reader["Email"]})"
                        );
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Listeleme hatası: " + ex.Message);
                }
            }
        }

        // 🔹 Öğrenci Ekleme
        public void AddStudent(string firstName, string lastName, string email)
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Students (FirstName, LastName, Email)
                        VALUES (@FirstName, @LastName, @Email)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Öğrenci başarıyla eklendi ✅");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ekleme hatası: " + ex.Message);
                }
            }
        }

        // 🔹 Öğrenci Silme
        public void DeleteStudent(int id)
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM Students WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                        Console.WriteLine("Öğrenci silindi ✅");
                    else
                        Console.WriteLine("Bu ID'ye ait öğrenci bulunamadı ❌");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Silme hatası: " + ex.Message);
                }
            }
        }

        // 🔹 Öğrenci Güncelleme
        public void UpdateStudent(int id, string firstName, string lastName, string email)
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        UPDATE Students
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email
                        WHERE Id = @Id";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", email);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                        Console.WriteLine("Öğrenci güncellendi ✅");
                    else
                        Console.WriteLine("Bu ID'ye ait öğrenci bulunamadı ❌");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Güncelleme hatası: " + ex.Message);
                }
            }
        }

        // 🔹 Öğrenciye Kurs Atama (Duplicate Kontrolü Dahil)
        public void AssignStudentToCourse(int studentId, int courseId)
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    // 🔸 Önce duplicate kontrolü yapıyoruz
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM StudentCourses 
                        WHERE StudentId = @StudentId 
                        AND CourseId = @CourseId";

                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@StudentId", studentId);
                    checkCommand.Parameters.AddWithValue("@CourseId", courseId);

                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        Console.WriteLine("Bu öğrenci zaten bu kursa kayıtlı ❌");
                        return;
                    }

                    // 🔸 Eğer kayıt yoksa INSERT yapıyoruz
                    string insertQuery = @"
                        INSERT INTO StudentCourses (StudentId, CourseId)
                        VALUES (@StudentId, @CourseId)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@StudentId", studentId);
                    insertCommand.Parameters.AddWithValue("@CourseId", courseId);

                    insertCommand.ExecuteNonQuery();

                    Console.WriteLine("Öğrenci kursa başarıyla atandı ✅");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Atama hatası: " + ex.Message);
                }
            }
        }

        // 🔹 Öğrencinin Kayıtlı Olduğu Kursları Listeleme (JOIN)
        public void ListCoursesOfStudent(int studentId)
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT c.CourseName
                        FROM StudentCourses sc
                        JOIN Courses c ON sc.CourseId = c.Id
                        WHERE sc.StudentId = @StudentId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    SqlDataReader reader = command.ExecuteReader();

                    Console.WriteLine("\nKayıtlı Kurslar:");

                    bool hasCourse = false;

                    while (reader.Read())
                    {
                        hasCourse = true;
                        Console.WriteLine("- " + reader["CourseName"]);
                    }

                    if (!hasCourse)
                    {
                        Console.WriteLine("Bu öğrenci herhangi bir kursa kayıtlı değil.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Kurs listeleme hatası: " + ex.Message);
                }
            }
        }
    }
}
