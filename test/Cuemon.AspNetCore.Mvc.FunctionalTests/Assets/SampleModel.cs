using System.ComponentModel.DataAnnotations;

namespace Cuemon.AspNetCore.Mvc.Assets;
public class SampleModel
{
    [Required(ErrorMessage = "This field is required.")]
    [StringLength(100)]
    public string Name { get; set; }
}
