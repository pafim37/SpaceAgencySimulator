using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.BodyDatabase.Entities
{
    public class BodyEntity : Entity
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public double Mass { get; set; }
        [Required]
        public double Radius { get; set; }

        [Required]
        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public PositionEntity? Position { get; set; }

        [Required]
        [ForeignKey("Velocity")]
        public int VelocityId { get; set; }
        public VelocityEntity? Velocity { get; set; }
    }
}
