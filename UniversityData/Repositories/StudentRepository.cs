
using UniversityData.Context;
using UniversityData.Entities;

namespace UniversityData.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversityDbContext _context;

        public StudentRepository(UniversityDbContext context)
        {
            _context = context;
        }

        public void Add(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            student.CreatedTime = DateTime.Now;
            _context.Students.Add(student); 
        }

        public void Delete(Student student)
        {
            if (student == null) 
                throw new ArgumentNullException(nameof(student));
            _context.Students.Remove(student); 
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student GetById(int id)
        {
            return _context.Students.Find(id);
        }

        public void Update(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            student.LastUpdatedTime = DateTime.Now;
            _context.Students.Update(student);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

    public interface IStudentRepository
    {
        List<Student> GetAll();
        Student GetById(int id);
        void Add(Student student); 
        void Update(Student student);
        void Delete(Student student);
        void SaveChanges();
    }
}
