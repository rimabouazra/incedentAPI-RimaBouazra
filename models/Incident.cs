using System.ComponentModel.DataAnnotations;

namespace incedentAPI_RimaBouazra.models
{
    public class Incident
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,MinimumLength =3,ErrorMessage ="Entre 3 et 30 caracteres")]
        public string Title { get; set; } = null!;//la valeur ne doit pas etre nulle 
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;// initiamisee vide
        [Required]
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
