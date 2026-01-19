using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApiBusinessLayer;
using static StudentApiDataAcessLayer.StudentData;

namespace StudentsApiProject_DB.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/Student")]
    [ApiController]
    public class StudentApi : ControllerBase
    {

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudent()
        {
            List<StudentDTO> students = Student.GetAllStudents();
            if (students.Count == 0)
            {
                return BadRequest("No Students Found");
            }
            return Ok(students);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudent()
        {
            List<StudentDTO> students = Student.GetPassedStudents();
            if (students.Count == 0)
            {
                return BadRequest("No Students Found");
            }
            return Ok(students);
        }



        [HttpGet("AVG", Name = "GetAvrageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAvgGrades()
        {
            List<StudentDTO> students = Student.GetAllStudents();
            if (students.Count == 0)
            {
                return NotFound("No Studnts");
            }

            return Ok(Student.GetAverageGrades());
        }


        [HttpGet("{id}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
            Student student = Student.Find(id);
            if (student == null)
            {
                return NotFound("Student Not Found");
            }
            StudentDTO StudentDTO = student.SDTO;
           

            return Ok(StudentDTO);
        }




        [HttpPost(Name = "AddNewStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> AddNewStudent(StudentDTO NewStudent)
        {
            if (NewStudent == null || NewStudent.age <= 0 || string.IsNullOrEmpty(NewStudent.name))
            {
                return BadRequest($"Invalid Student Data");
            }

            Student student=new Student(NewStudent);
            student.Save();
            NewStudent.id= student.id;
            return CreatedAtRoute("GetStudentByID", new { id = NewStudent.id }, NewStudent);
        }




        [HttpPut("{id}", Name = "UpdateStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> UpdateStudentByID(int id, StudentDTO UpdateStudent)
        {
            if (id < 1 || UpdateStudent == null ) 
            {
                return BadRequest($"No Student With id {id}");
            }
            Student student = Student.Find(id);
            if (student==null)
            {
                return NotFound($"Not Found Student By ID{id}");
            }
            if (string.IsNullOrEmpty(UpdateStudent.name) || UpdateStudent.age <= 0 || UpdateStudent.grade < 0)
            {
                return BadRequest($"Invalid Data");

            }

            student.name = UpdateStudent.name;
            student.grade = UpdateStudent.grade;
            student.age = UpdateStudent.age;
            student.Save();

            return Ok(student.SDTO);
        }



        [HttpDelete("{id}", Name = "DeleteStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteStudentByID(int id)
        {
            if (id < 0)
            {
                return BadRequest($"No Student With id {id}");
            }
            Student student = Student.Find(id);
            if (student == null)
            {
                return NotFound($"Not Found Student By ID{id}");
            }

            if (Student.Delete(id))

                return Ok($"Student with ID {id} has been deleted.");
            else
                return NotFound($"Student with ID {id} not found. no rows deleted!");
        }


    }
}
