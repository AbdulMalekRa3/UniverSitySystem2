
using UniversityData.Context;
using UniversityData.Entities;

namespace UniversityData.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityDbContext _context;

        public CourseRepository(UniversityDbContext context)
        {
            _context = context;
        }

        public void Add(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            course.CreatedTime = DateTime.Now;
            _context.Courses.Add(course);
        }

        public void Delete(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            _context.Courses.Remove(course);
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetById(int id)
        {
            return _context.Courses.Find(id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            course.LastUpdatedTime = DateTime.Now;
            _context.Courses.Update(course);
        }
    }

    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course GetById(int id);
        void Add(Course course);
        void Update(Course course);
        void Delete(Course course);
        void SaveChanges();
    }
}
