using ReviewIT.Backend.Entities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewIT.Backend.Entities.Entitities
{
    public class Stage : IIdEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool StageInitiated { get; set; }

        public int? StudyId { get; set; }
        public virtual Study Study { get; set; }
    }
}
