using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using UniversityData.Entities;
using UniversityData.Repositories;

namespace University.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public void Create(AddStudentForm form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            var student = new Student()
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Phone = form.Phone,
                Email = form.Email
            };

            _studentRepository.Add(student);
            _studentRepository.SaveChanges();

        }

        public void Delete(int id)
        {
            var studet = _studentRepository.GetById(id);
            if (studet == null) throw new NotFoundException("Student not found");
            _studentRepository.Delete(studet);
            _studentRepository.SaveChanges();
        }

        public List<StudentDTO> GetAll()
        {
            var students = _studentRepository.GetAll();

            return students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Phone = s.Phone,
                Email = s.Email
            }).ToList();
        }

        public StudentDTO GetById(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null) throw new NotFoundException("Student not found");
            return new StudentDTO()
            {
                Id = student.Id,
                Email = student.Email,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Phone = student.Phone
            };
        }

        public void Update(int id, UpdateStudentForm form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (id < 0) throw new ArgumentNullException(nameof(id));

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            var student = _studentRepository.GetById(id);
            if (student == null) throw new NotFoundException("Student not found");

            student.FirstName = form.FirstName;
            student.LastName = form.LastName;
            student.Phone = form.Phone;


            _studentRepository.Update(student);
            _studentRepository.SaveChanges();

        }
    }

    public interface IStudentService
    {
        List<StudentDTO> GetAll();
        StudentDTO GetById(int id);
        void Create(AddStudentForm form);
        void Update(int id, UpdateStudentForm form);
        void Delete(int id);
    }
}