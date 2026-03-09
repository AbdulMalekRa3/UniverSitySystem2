using System.ComponentModel.DataAnnotations;

namespace University.Core.Forms
{
    public class UpdateCourseForm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 10)]
        public int Credit { get; set; }
    }
}
