using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Core.DTOs;
using University.Core.Forms;
using University.Core.Services;
using UniverSity.Api.Filters;

namespace UniverSity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]

    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{id}")]
            [Authorize(Roles = "Teacher")]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetById(int id)
        {
            var dto = _courseService.GetById(id);
            return new ApiResponse(dto);
        }
        

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        [ProducesResponseType(typeof(List<CourseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetAll()
        {
            var dto = _courseService.GetAll();
            return new ApiResponse(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse Create([FromBody] AddCourseForm form)
        {
            _courseService.Create(form);
            return new ApiResponse(HttpStatusCode.Created);

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse Update(int id, [FromBody] UpdateCourseForm form)
        {
            _courseService.Update(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse Delete(int id)
        {
            _courseService.Delete(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
    }
