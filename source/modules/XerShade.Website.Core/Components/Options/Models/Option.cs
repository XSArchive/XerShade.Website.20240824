using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XerShade.Website.Core.Components.Options.Models;

[Table("CoreOptions"), PrimaryKey("OptionId")]
[Index("OptionName", IsUnique = true)]
public class Option
{
    [Required]
    public int OptionId { get; set; }
    [Required]
    public string OptionName { get; set; } = string.Empty;
    public string OptionValue { get; set; } = string.Empty;
    public bool AutoLoad { get; set; } = false;
}
