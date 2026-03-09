using System.ComponentModel.DataAnnotations;

namespace University.Core.Forms
{
    public class UpdateStudentForm
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

    }
}
