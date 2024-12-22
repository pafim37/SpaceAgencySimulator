using System.ComponentModel.DataAnnotations.Schema;

namespace Sas.Body.Service.Models
{
    public class BodyEntity : Entity
    {
        public string? Name { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }

        [ForeignKey(nameof(PositionId))]
        public int PositionId { get; set; }
        public VectorEntity? Position { get; set; }

        [ForeignKey(nameof(VelocityId))]
        public int VelocityId { get; set; }
        public VectorEntity? Velocity { get; set; }
    }
}
