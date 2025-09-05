// Chương trình quản lý trường học - GOOD CODE
// Áp dụng các nguyên tắc Clean Code
// Sử dụng OOP, SOLID principles, và proper separation of concerns

using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagement
{
    // Entity classes với proper encapsulation
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double GPA { get; set; }

        public Student(string id, string name, int age, double gpa)
        {
            Id = id;
            Name = name;
            Age = age;
            GPA = gpa;
        }

        public override string ToString()
        {
            return $"Student[Id={Id}, Name={Name}, Age={Age}, GPA={GPA:F2}]";
        }
    }

    public class Teacher
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }

        public Teacher(string id, string name, string major)
        {
            Id = id;
            Name = name;
            Major = major;
        }

        public override string ToString()
        {
            return $"Teacher[Id={Id}, Name={Name}, Major={Major}]";
        }
    }

    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }

        public Course(string id, string name, int credits)
        {
            Id = id;
            Name = name;
            Credits = credits;
        }

        public override string ToString()
        {
            return $"Course[Id={Id}, Name={Name}, Credits={Credits}]";
        }
    }

    public class Enrollment
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Enrollment(string studentId, string courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
            EnrollmentDate = DateTime.Now;
        }
    }

    public class Grade
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public double Score { get; set; }

        public Grade(string studentId, string courseId, double score)
        {
            StudentId = studentId;
            CourseId = courseId;
            Score = score;
        }
    }

    // Repository pattern để quản lý dữ liệu
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        T GetById(string id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
    }

    public class Repository<T> : IRepository<T>
    {
        private readonly List<T> _items = new List<T>();
        private readonly Func<T, string> _getIdFunc;

        public Repository(Func<T, string> getIdFunc)
        {
            _getIdFunc = getIdFunc;
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Remove(item);
        }

        public void Update(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var existingItem = GetById(_getIdFunc(item));
            if (existingItem != null)
            {
                var index = _items.IndexOf(existingItem);
                _items[index] = item;
            }
        }

        public T GetById(string id)
        {
            return _items.FirstOrDefault(item => _getIdFunc(item) == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _items.Where(predicate).ToList();
        }
    }

    // Service layer để xử lý business logic
    public class StudentService
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentService(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void AddStudent(string id, string name, int age, double gpa)
        {
            ValidateStudentData(id, name, age, gpa);
            var student = new Student(id, name, age, gpa);
            _studentRepository.Add(student);
        }

        public void RemoveStudent(string id)
        {
            var student = _studentRepository.GetById(id);
            if (student != null)
            {
                _studentRepository.Remove(student);
            }
        }

        public void UpdateStudent(string id, string name, int age, double gpa)
        {
            ValidateStudentData(id, name, age, gpa);
            var student = new Student(id, name, age, gpa);
            _studentRepository.Update(student);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentRepository.GetAll();
        }

        public IEnumerable<Student> FindStudentsByName(string name)
        {
            return _studentRepository.Find(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Student> GetStudentsWithHighGPA(double minGPA = 8.0)
        {
            return _studentRepository.Find(s => s.GPA >= minGPA);
        }

        public IEnumerable<Student> GetStudentsSortedByName()
        {
            return _studentRepository.GetAll().OrderBy(s => s.Name);
        }

        public IEnumerable<Student> GetStudentsSortedByGPA()
        {
            return _studentRepository.GetAll().OrderByDescending(s => s.GPA);
        }

        private void ValidateStudentData(string id, string name, int age, double gpa)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Student ID cannot be empty");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Student name cannot be empty");
            if (age < 0 || age > 150) throw new ArgumentException("Invalid age");
            if (gpa < 0 || gpa > 4.0) throw new ArgumentException("GPA must be between 0 and 4.0");
        }
    }

    public class EnrollmentService
    {
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Course> _courseRepository;

        public EnrollmentService(
            IRepository<Enrollment> enrollmentRepository,
            IRepository<Student> studentRepository,
            IRepository<Course> courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public void EnrollStudent(string studentId, string courseId)
        {
            ValidateEnrollment(studentId, courseId);
            var enrollment = new Enrollment(studentId, courseId);
            _enrollmentRepository.Add(enrollment);
        }

        public void CancelEnrollment(string studentId, string courseId)
        {
            var enrollment = _enrollmentRepository.Find(e => 
                e.StudentId == studentId && e.CourseId == courseId).FirstOrDefault();
            
            if (enrollment != null)
            {
                _enrollmentRepository.Remove(enrollment);
            }
        }

        public IEnumerable<Enrollment> GetAllEnrollments()
        {
            return _enrollmentRepository.GetAll();
        }

        private void ValidateEnrollment(string studentId, string courseId)
        {
            if (_studentRepository.GetById(studentId) == null)
                throw new ArgumentException($"Student with ID {studentId} not found");
            
            if (_courseRepository.GetById(courseId) == null)
                throw new ArgumentException($"Course with ID {courseId} not found");
        }
    }

    // UI layer với proper separation
    public class SchoolManagementUI
    {
        private readonly StudentService _studentService;
        private readonly EnrollmentService _enrollmentService;

        public SchoolManagementUI(StudentService studentService, EnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _enrollmentService = enrollmentService;
        }

        public void Run()
        {
            while (true)
            {
                DisplayMainMenu();
                var choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        HandleStudentManagement();
                        break;
                    case 2:
                        HandleEnrollmentManagement();
                        break;
                    case 3:
                        DisplayReport();
                        break;
                    case 99:
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine("\n============= MAIN MENU =============");
            Console.WriteLine("1. Student Management");
            Console.WriteLine("2. Enrollment Management");
            Console.WriteLine("3. Generate Report");
            Console.WriteLine("99. Exit");
            Console.Write("Enter your choice: ");
        }

        private int GetUserChoice()
        {
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : 0;
        }

        private void HandleStudentManagement()
        {
            while (true)
            {
                Console.WriteLine("\n--- STUDENT MANAGEMENT ---");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Remove Student");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Display All Students");
                Console.WriteLine("5. Find Students by Name");
                Console.WriteLine("6. Show High GPA Students");
                Console.WriteLine("7. Sort by Name");
                Console.WriteLine("8. Sort by GPA");
                Console.WriteLine("9. Back to Main Menu");

                var choice = GetUserChoice();
                if (choice == 9) break;

                ProcessStudentChoice(choice);
            }
        }

        private void ProcessStudentChoice(int choice)
        {
            try
            {
                switch (choice)
                {
                    case 1:
                        AddStudent();
                        break;
                    case 2:
                        RemoveStudent();
                        break;
                    case 3:
                        UpdateStudent();
                        break;
                    case 4:
                        DisplayAllStudents();
                        break;
                    case 5:
                        FindStudentsByName();
                        break;
                    case 6:
                        ShowHighGPAStudents();
                        break;
                    case 7:
                        DisplayStudentsSortedByName();
                        break;
                    case 8:
                        DisplayStudentsSortedByGPA();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void AddStudent()
        {
            Console.Write("Enter student ID: ");
            var id = Console.ReadLine();
            Console.Write("Enter student name: ");
            var name = Console.ReadLine();
            Console.Write("Enter student age: ");
            var age = int.Parse(Console.ReadLine());
            Console.Write("Enter student GPA: ");
            var gpa = double.Parse(Console.ReadLine());

            _studentService.AddStudent(id, name, age, gpa);
            Console.WriteLine("Student added successfully!");
        }

        private void RemoveStudent()
        {
            Console.Write("Enter student ID to remove: ");
            var id = Console.ReadLine();
            _studentService.RemoveStudent(id);
            Console.WriteLine("Student removed successfully!");
        }

        private void UpdateStudent()
        {
            Console.Write("Enter student ID to update: ");
            var id = Console.ReadLine();
            Console.Write("Enter new name: ");
            var name = Console.ReadLine();
            Console.Write("Enter new age: ");
            var age = int.Parse(Console.ReadLine());
            Console.Write("Enter new GPA: ");
            var gpa = double.Parse(Console.ReadLine());

            _studentService.UpdateStudent(id, name, age, gpa);
            Console.WriteLine("Student updated successfully!");
        }

        private void DisplayAllStudents()
        {
            var students = _studentService.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("No students found.");
                return;
            }

            Console.WriteLine("\nAll Students:");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }

        private void FindStudentsByName()
        {
            Console.Write("Enter name to search: ");
            var name = Console.ReadLine();
            var students = _studentService.FindStudentsByName(name);
            
            if (!students.Any())
            {
                Console.WriteLine("No students found with that name.");
                return;
            }

            Console.WriteLine($"\nStudents with name containing '{name}':");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }

        private void ShowHighGPAStudents()
        {
            var students = _studentService.GetStudentsWithHighGPA();
            if (!students.Any())
            {
                Console.WriteLine("No students with high GPA found.");
                return;
            }

            Console.WriteLine("\nStudents with GPA >= 8.0:");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }

        private void DisplayStudentsSortedByName()
        {
            var students = _studentService.GetStudentsSortedByName();
            Console.WriteLine("\nStudents sorted by name:");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }

        private void DisplayStudentsSortedByGPA()
        {
            var students = _studentService.GetStudentsSortedByGPA();
            Console.WriteLine("\nStudents sorted by GPA (highest first):");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }

        private void HandleEnrollmentManagement()
        {
            // Implementation for enrollment management
            Console.WriteLine("Enrollment management functionality would be implemented here.");
        }

        private void DisplayReport()
        {
            // Implementation for report generation
            Console.WriteLine("Report generation functionality would be implemented here.");
        }
    }

    // Main program entry point
    public class Program
    {
        public static void Main(string[] args)
        {
            // Dependency injection setup
            var studentRepository = new Repository<Student>(s => s.Id);
            var teacherRepository = new Repository<Teacher>(t => t.Id);
            var courseRepository = new Repository<Course>(c => c.Id);
            var enrollmentRepository = new Repository<Enrollment>(e => $"{e.StudentId}_{e.CourseId}");

            var studentService = new StudentService(studentRepository);
            var enrollmentService = new EnrollmentService(enrollmentRepository, studentRepository, courseRepository);

            var ui = new SchoolManagementUI(studentService, enrollmentService);
            ui.Run();
        }
    }
}
