using System.ComponentModel.DataAnnotations;

namespace Lab5.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        public String FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public ICollection<Subscription> Subscription { get; set; }
    }
}
