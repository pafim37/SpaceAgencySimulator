using System.ComponentModel.DataAnnotations;

namespace Sas.Body.Core.DataTransferObject
{
    public class NewBodyDto
    {
        [MinLength(1), MaxLength(50)]
        public required string Name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Mass must be a positive value.")]
        public required double Mass { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Radius must be a positive value.")]
        public required double Radius { get; set; }
        public required bool Enabled { get; set; }
        public required VectorDto Position { get; set; }
        public required VectorDto Velocity { get; set; }
    }
}
