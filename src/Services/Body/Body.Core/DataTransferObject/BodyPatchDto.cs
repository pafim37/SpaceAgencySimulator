using System.ComponentModel.DataAnnotations;

namespace Sas.Body.Core.DataTransferObject
{
    public class BodyPatchDto
    {
        public required int Id { get; set; }
        [MinLength(1), MaxLength(50)]
        public string? Name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Mass must be a positive value.")]
        public double? Mass { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Radius must be a positive value.")]
        public double? Radius { get; set; }
        public bool? Enabled { get; set; }
        public VectorDto? Position { get; set; }
        public VectorDto? Velocity { get; set; }
    }
}
