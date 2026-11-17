using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Body.Core.Models.Entities
{
    public class BodyEntity : Entity
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public double Mass { get; set; }
        [Required]
        public double Radius { get; set; }
        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(PositionId))]
        public int PositionId { get; set; }
        [Required]
        public required VectorEntity Position { get; set; }

        [ForeignKey(nameof(VelocityId))]
        public int VelocityId { get; set; }
        [Required]
        public required VectorEntity Velocity { get; set; }
    }
}
