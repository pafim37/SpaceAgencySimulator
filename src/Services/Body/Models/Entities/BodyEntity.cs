using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Body.Service.Models.Entities
{
    public class BodyEntity : Entity
    {
        public required string Name { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }

        [ForeignKey(nameof(PositionId))]
        public int PositionId { get; set; }
        public VectorEntity Position { get; set; } = null!;

        [ForeignKey(nameof(VelocityId))]
        public int VelocityId { get; set; }
        public VectorEntity Velocity { get; set; } = null!;
    }
}
