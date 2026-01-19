using StudentApiDataAcessLayer;
using static StudentApiDataAcessLayer.StudentData;

namespace StudentApiBusinessLayer
{
    public class Student
    {

        public enum enMode
        {
            AddNew = 0, Update = 1
        };

        public enMode Mode = enMode.AddNew;

        public StudentDTO SDTO { get { return new StudentDTO(this.id, this.name, this.age, this.grade); } }
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int grade { get; set; }


        public Student(StudentDTO studentDTO, enMode mode = enMode.AddNew)
        {
            this.id = studentDTO.id;
            this.name = studentDTO.name;
            this.age = studentDTO.age;
            this.grade = studentDTO.grade;
            Mode = mode;
        }

        public static Student Find(int StudentID)
        {
            StudentDTO studentDTO = StudentData.GetStudentById(StudentID);
            if (studentDTO == null)
            {
                return null;
            }
            return new Student(studentDTO, enMode.Update);
        }

        public static bool Delete(int id)
        {
            return StudentData.DeleteStudent(id);
        }
        public static List<StudentDTO> GetAllStudents()
        {
            return StudentData.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }

        public static double GetAverageGrades()
        {
            return StudentData.GetAverageGrades();
        }

        private bool _AddNewStudent()
        {
            this.id = StudentData.AddStudent(SDTO);
            return (this.id != -1);
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewStudent())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateStudent();

            }

            return false;
        }
    }
}
