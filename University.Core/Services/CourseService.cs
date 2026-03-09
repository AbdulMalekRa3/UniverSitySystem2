using Microsoft.Extensions.Logging;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using UniversityData.Entities;
using UniversityData.Repositories;


namespace University.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;
        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }
        public void Create(AddCourseForm form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            var course = new Course()
            {
                Name = form.Name,
                Credit = form.Credit,
            };

            _courseRepository.Add(course);
            _courseRepository.SaveChanges();
            _logger.LogInformation("Course Created");

        }

        public void Delete(int id)
        {

            var course = _courseRepository.GetById(id);
            if (course == null) throw new NotFoundException("Course not found");
            _courseRepository.Delete(course);
            _courseRepository.SaveChanges();
            _logger.LogInformation($"Course with id {id} Deleted");
        }

        public List<CourseDTO> GetAll()
        {
            var courses = _courseRepository.GetAll();
            _logger.LogInformation("Queried All Courses");
            return courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Credit = c.Credit,
            }).ToList();
        }

        public CourseDTO GetById(int id)
        {

            var course = _courseRepository.GetById(id);
            _logger.LogInformation($"Queried Course with id {id}");
            if (course == null) throw new NotFoundException("Course not found");
            return new CourseDTO()
            {
                Id = course.Id,
                Name = course.Name,
                Credit = course.Credit,
            };
        }

        public void Update(int id, UpdateCourseForm form)
        {

            if (form == null) throw new ArgumentNullException(nameof(form));
            if (id < 0) throw new ArgumentNullException(nameof(id));

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            var course = _courseRepository.GetById(id);
            if (course == null) throw new NotFoundException("Course not found");

            course.Name = form.Name;
            course.Credit = form.Credit;

            _courseRepository.Update(course);
            _courseRepository.SaveChanges();
            _logger.LogInformation($"Updated Course with id {id}");

        }
    }

    public interface ICourseService
    {
        List<CourseDTO> GetAll();
        CourseDTO GetById(int id);
        void Create(AddCourseForm form);
        void Update(int id, UpdateCourseForm form);
        void Delete(int id);
    }
}