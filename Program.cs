using System;
using MySql.Data.MySqlClient;

namespace DesktopInformationSystem
{
    public class Person
    {
        private int id;
        private string name;
        private string telephone;
        private string email;
        private string role;

        public int ID
        {
            get { return id; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("ID must be a positive integer.");
                id = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty or whitespace.");
                name = value;
            }
        }

        public string Telephone
        {
            get { return telephone; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !long.TryParse(value, out _))
                    throw new ArgumentException("Telephone must be a valid number.");
                telephone = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty or whitespace.");
                email = value;
            }
        }

        public string Role
        {
            get { return role; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Role cannot be empty or whitespace.");
                role = value;
            }
        }
    }

    public class Teacher : Person
    {
        private double salary;
        private string subject1;
        private string subject2;

        public double Salary
        {
            get { return salary; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Salary must be a non-negative number.");
                salary = value;
            }
        }

        public string Subject1
        {
            get { return subject1; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Subject1 cannot be empty or whitespace.");
                subject1 = value;
            }
        }

        public string Subject2
        {
            get { return subject2; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Subject2 cannot be empty or whitespace.");
                subject2 = value;
            }
        }
    }

    public class Admin : Person
    {
        private double salary;
        private bool isFullTime;
        private int workingHours;

        public double Salary
        {
            get { return salary; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Salary must be a non-negative number.");
                salary = value;
            }
        }

        public bool IsFullTime
        {
            get { return isFullTime; }
            set { isFullTime = value; }
        }

        public int WorkingHours
        {
            get { return workingHours; }
            set
            {
                if (value < 0 || value > 168)
                    throw new ArgumentException("Working hours must be between 0 and 168.");
                workingHours = value;
            }
        }
    }

    public class Student : Person
    {
        private string currentSubject1;
        private string currentSubject2;
        private string previousSubject1;
        private string previousSubject2;

        public string CurrentSubject1
        {
            get { return currentSubject1; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CurrentSubject1 cannot be empty or whitespace.");
                currentSubject1 = value;
            }
        }

        public string CurrentSubject2
        {
            get { return currentSubject2; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CurrentSubject2 cannot be empty or whitespace.");
                currentSubject2 = value;
            }
        }

        public string PreviousSubject1
        {
            get { return previousSubject1; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PreviousSubject1 cannot be empty or whitespace.");
                previousSubject1 = value;
            }
        }

        public string PreviousSubject2
        {
            get { return previousSubject2; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PreviousSubject2 cannot be empty or whitespace.");
                previousSubject2 = value;
            }
        }
    }

    class Program
    {
        static string connectionString = "server=localhost;user=root;database=c-sharp;port=3306;password=ok";

        static void Main(string[] args)
        {
            MySqlConnection conn = null;
            try
            {
                Console.WriteLine("Connecting...");

                conn = new MySqlConnection(connectionString);
                conn.Open();

                Console.WriteLine("Connection successful!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                Environment.Exit(1);
            }

            while (true)
            {
                Console.WriteLine("Desktop Information System");
                Console.WriteLine("1. Add new record");
                Console.WriteLine("2. View all records");
                Console.WriteLine("3. View records by role");
                Console.WriteLine("4. Edit record");
                Console.WriteLine("5. Delete record");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddRecord();
                        break;
                    case "2":
                        ViewAllRecords();
                        break;
                    case "3":
                        ViewRecordsByRole();
                        break;
                    case "4":
                        EditRecord();
                        break;
                    case "5":
                        DeleteRecord();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again. \n");
                        break;
                }
            }
        }

        static void AddRecord()
        {
            Console.Write("Enter role (Teacher/Admin/Student): ");
            string role = Console.ReadLine();
            Person person;

            switch (role.ToLower())
            {
                case "teacher":
                    person = new Teacher();
                    break;
                case "admin":
                    person = new Admin();
                    break;
                case "student":
                    person = new Student();
                    break;
                default:
                    Console.WriteLine("Invalid role.");
                    return;
            }

            Console.Write("Enter name: ");
            person.Name = Console.ReadLine();
            Console.Write("Enter telephone: ");
            person.Telephone = Console.ReadLine();
            Console.Write("Enter email: ");
            person.Email = Console.ReadLine();
            person.Role = role;

            if (person is Teacher teacher)
            {
                Console.Write("Enter salary: ");
                teacher.Salary = Convert.ToDouble(Console.ReadLine());
                Console.Write("Enter first subject: ");
                teacher.Subject1 = Console.ReadLine();
                Console.Write("Enter second subject: ");
                teacher.Subject2 = Console.ReadLine();
                AddTeacherToDatabase(teacher);
            }
            else if (person is Admin admin)
            {
                Console.Write("Enter salary: ");
                admin.Salary = Convert.ToDouble(Console.ReadLine());
                Console.Write("Is full-time (yes/no): ");
                admin.IsFullTime = Console.ReadLine().ToLower() == "yes";
                Console.Write("Enter working hours: ");
                admin.WorkingHours = Convert.ToInt32(Console.ReadLine());
                AddAdminToDatabase(admin);
            }
            else if (person is Student student)
            {
                Console.Write("Enter first current subject: ");
                student.CurrentSubject1 = Console.ReadLine();
                Console.Write("Enter second current subject: ");
                student.CurrentSubject2 = Console.ReadLine();
                Console.Write("Enter first previous subject: ");
                student.PreviousSubject1 = Console.ReadLine();
                Console.Write("Enter second previous subject: ");
                student.PreviousSubject2 = Console.ReadLine();
                AddStudentToDatabase(student);
            }

            Console.WriteLine("Record added successfully.\n");
        }

        static void AddTeacherToDatabase(Teacher teacher)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string insertTeacherSql = "INSERT INTO teachers (name, telephone, email, role, salary, subject1, subject2) " +
                                          "VALUES (@name, @telephone, @email, @role, @salary, @subject1, @subject2)";
                MySqlCommand cmd = new MySqlCommand(insertTeacherSql, conn);
                cmd.Parameters.AddWithValue("@name", teacher.Name);
                cmd.Parameters.AddWithValue("@telephone", teacher.Telephone);
                cmd.Parameters.AddWithValue("@email", teacher.Email);
                cmd.Parameters.AddWithValue("@role", teacher.Role);
                cmd.Parameters.AddWithValue("@salary", teacher.Salary);
                cmd.Parameters.AddWithValue("@subject1", teacher.Subject1);
                cmd.Parameters.AddWithValue("@subject2", teacher.Subject2);

                cmd.ExecuteNonQuery();
            }
        }

        static void AddAdminToDatabase(Admin admin)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string insertAdminSql = "INSERT INTO admins (name, telephone, email, role, salary, isFullTime, workingHours) " +
                                        "VALUES (@name, @telephone, @email, @role, @salary, @isFullTime, @workingHours)";
                MySqlCommand cmd = new MySqlCommand(insertAdminSql, conn);
                cmd.Parameters.AddWithValue("@name", admin.Name);
                cmd.Parameters.AddWithValue("@telephone", admin.Telephone);
                cmd.Parameters.AddWithValue("@email", admin.Email);
                cmd.Parameters.AddWithValue("@role", admin.Role);
                cmd.Parameters.AddWithValue("@salary", admin.Salary);
                cmd.Parameters.AddWithValue("@isFullTime", admin.IsFullTime);
                cmd.Parameters.AddWithValue("@workingHours", admin.WorkingHours);

                cmd.ExecuteNonQuery();
            }
        }

        static void AddStudentToDatabase(Student student)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string insertStudentSql = "INSERT INTO students (name, telephone, email, role, currentSubject1, currentSubject2, previousSubject1, previousSubject2) " +
                                          "VALUES (@name, @telephone, @email, @role, @currentSubject1, @currentSubject2, @previousSubject1, @previousSubject2)";
                MySqlCommand cmd = new MySqlCommand(insertStudentSql, conn);
                cmd.Parameters.AddWithValue("@name", student.Name);
                cmd.Parameters.AddWithValue("@telephone", student.Telephone);
                cmd.Parameters.AddWithValue("@email", student.Email);
                cmd.Parameters.AddWithValue("@role", student.Role);
                cmd.Parameters.AddWithValue("@currentSubject1", student.CurrentSubject1);
                cmd.Parameters.AddWithValue("@currentSubject2", student.CurrentSubject2);
                cmd.Parameters.AddWithValue("@previousSubject1", student.PreviousSubject1);
                cmd.Parameters.AddWithValue("@previousSubject2", student.PreviousSubject2);

                cmd.ExecuteNonQuery();
            }
        }

        static void ViewAllRecords()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT id, name, telephone, email, 'Teacher' AS role, salary AS extra1, subject1 AS extra2, subject2 AS extra3, NULL AS extra4, NULL AS extra5 
                    FROM teachers 
                    UNION ALL 
                    SELECT id, name, telephone, email, 'Admin' AS role, salary AS extra1, isFullTime AS extra2, workingHours AS extra3, NULL AS extra4, NULL AS extra5 
                    FROM admins 
                    UNION ALL 
                    SELECT id, name, telephone, email, 'Student' AS role, NULL AS extra1, currentSubject1 AS extra2, currentSubject2 AS extra3, previousSubject1 AS extra4, previousSubject2 AS extra5 
                    FROM students 
                    ORDER BY role, id"; // Ensure the records are ordered by role and id

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                string currentRole = string.Empty;

                while (rdr.Read())
                {
                    string role = rdr["role"].ToString();
                    if (role != currentRole)
                    {
                        currentRole = role;
                        Console.WriteLine($"\n{currentRole}s:\n");
                    }
                    Console.WriteLine($"ID: {rdr["id"]}, Name: {rdr["name"]}, Telephone: {rdr["telephone"]}, Email: {rdr["email"]}, Role: {rdr["role"]}");

                    if (role == "Teacher")
                    {
                        Console.WriteLine($"Salary: {rdr["extra1"]}, Subject1: {rdr["extra2"]}, Subject2: {rdr["extra3"]}\n");
                    }
                    else if (role == "Admin")
                    {
                        Console.WriteLine($"Salary: {rdr["extra1"]}, Is Full-Time: {rdr["extra2"]}, Working Hours: {rdr["extra3"]}\n");
                    }
                    else if (role == "Student")
                    {
                        Console.WriteLine($"Current Subject1: {rdr["extra2"]}, Current Subject2: {rdr["extra3"]}, Previous Subject1: {rdr["extra4"]}, Previous Subject2: {rdr["extra5"]}\n");
                    }
                }
                rdr.Close();
            }
        }

        static void ViewRecordsByRole()
        {
            Console.Write("Enter role (Teacher/Admin/Student): ");
            string role = Console.ReadLine().ToLower();

            string sql;
            switch (role)
            {
                case "teacher":
                    sql = "SELECT id, name, telephone, email, salary, subject1, subject2 FROM teachers";
                    break;
                case "admin":
                    sql = "SELECT id, name, telephone, email, salary, isFullTime, workingHours FROM admins";
                    break;
                case "student":
                    sql = "SELECT id, name, telephone, email, currentSubject1, currentSubject2, previousSubject1, previousSubject2 FROM students";
                    break;
                default:
                    Console.WriteLine("Invalid role.");
                    return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine($"ID: {rdr["id"]}, Name: {rdr["name"]}, Telephone: {rdr["telephone"]}, Email: {rdr["email"]}");
                    if (role == "teacher")
                    {
                        Console.WriteLine($"Salary: {rdr["salary"]}, Subject1: {rdr["subject1"]}, Subject2: {rdr["subject2"]}\n");
                    }
                    else if (role == "admin")
                    {
                        Console.WriteLine($"Salary: {rdr["salary"]}, Is Full-Time: {rdr["isFullTime"]}, Working Hours: {rdr["workingHours"]}\n");
                    }
                    else if (role == "student")
                    {
                        Console.WriteLine($"Current Subject1: {rdr["currentSubject1"]}, Current Subject2: {rdr["currentSubject2"]}, Previous Subject1: {rdr["previousSubject1"]}, Previous Subject2: {rdr["previousSubject2"]}\n");
                    }
                }
                rdr.Close();
            }
        }

        static void EditRecord()
        {
            Console.Write("Enter ID of the record to edit: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter new name: ");
            string name = Console.ReadLine();
            Console.Write("Enter new telephone: ");
            string telephone = Console.ReadLine();
            Console.Write("Enter new email: ");
            string email = Console.ReadLine();
            Console.Write("Enter role (Teacher/Admin/Student): ");
            string role = Console.ReadLine().ToLower();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                switch (role)
                {
                    case "teacher":
                        Console.Write("Enter new salary: ");
                        double teacherSalary = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Enter new first subject: ");
                        string subject1 = Console.ReadLine();
                        Console.Write("Enter new second subject: ");
                        string subject2 = Console.ReadLine();
                        sql = "UPDATE teachers SET name=@name, telephone=@telephone, email=@email, salary=@salary, subject1=@subject1, subject2=@subject2 WHERE id=@id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@salary", teacherSalary);
                        cmd.Parameters.AddWithValue("@subject1", subject1);
                        cmd.Parameters.AddWithValue("@subject2", subject2);
                        break;

                    case "admin":
                        Console.Write("Enter new salary: ");
                        double adminSalary = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Is full-time (yes/no): ");
                        bool isFullTime = Console.ReadLine().ToLower() == "yes";
                        Console.Write("Enter new working hours: ");
                        int workingHours = Convert.ToInt32(Console.ReadLine());
                        sql = "UPDATE admins SET name=@name, telephone=@telephone, email=@email, salary=@salary, isFullTime=@isFullTime, workingHours=@workingHours WHERE id=@id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@salary", adminSalary);
                        cmd.Parameters.AddWithValue("@isFullTime", isFullTime);
                        cmd.Parameters.AddWithValue("@workingHours", workingHours);
                        break;

                    case "student":
                        Console.Write("Enter new first current subject: ");
                        string currentSubject1 = Console.ReadLine();
                        Console.Write("Enter new second current subject: ");
                        string currentSubject2 = Console.ReadLine();
                        Console.Write("Enter new first previous subject: ");
                        string previousSubject1 = Console.ReadLine();
                        Console.Write("Enter new second previous subject: ");
                        string previousSubject2 = Console.ReadLine();
                        sql = "UPDATE students SET name=@name, telephone=@telephone, email=@email, currentSubject1=@currentSubject1, currentSubject2=@currentSubject2, previousSubject1=@previousSubject1, previousSubject2=@previousSubject2 WHERE id=@id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@currentSubject1", currentSubject1);
                        cmd.Parameters.AddWithValue("@currentSubject2", currentSubject2);
                        cmd.Parameters.AddWithValue("@previousSubject1", previousSubject1);
                        cmd.Parameters.AddWithValue("@previousSubject2", previousSubject2);
                        break;

                    default:
                        Console.WriteLine("Invalid role.\n");
                        return;
                }

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@telephone", telephone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Record updated successfully.\n");
        }

        static void DeleteRecord()
        {
            Console.Write("Enter Role of the record to delete (teacher/admin/student): ");
            string role = Console.ReadLine().Trim().ToLower(); // Normalize role input to lowercase and trim whitespace
            Console.Write("Enter ID of the record to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Determine the role of the record to delete
                    string roleSql = "SELECT role FROM (SELECT id, 'teacher' AS role FROM teachers " +
                                     "UNION ALL " +
                                     "SELECT id, 'admin' AS role FROM admins " +
                                     "UNION ALL " +
                                     "SELECT id, 'student' AS role FROM students) AS all_roles WHERE id = @id";
                    MySqlCommand roleCmd = new MySqlCommand(roleSql, conn);
                    roleCmd.Parameters.AddWithValue("@id", id);
                    string actualRole = roleCmd.ExecuteScalar()?.ToString()?.ToLower(); // Fetch and normalize role

                    if (string.IsNullOrEmpty(actualRole) || !actualRole.Equals(role))
                    {
                        Console.WriteLine($"No {role} record found with ID {id}.\n");
                        return;
                    }

                    string deleteSql = "";
                    switch (actualRole)
                    {
                        case "teacher":
                            deleteSql = "DELETE FROM teachers WHERE id=@id";
                            break;
                        case "admin":
                            deleteSql = "DELETE FROM admins WHERE id=@id";
                            break;
                        case "student":
                            deleteSql = "DELETE FROM students WHERE id=@id";
                            break;
                    }

                    MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Record with ID {id} deleted successfully from {actualRole} table.\n");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete record with ID {id} from {actualRole} table.\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting record: {ex.Message}");
                }
            }
        }

    }

}
