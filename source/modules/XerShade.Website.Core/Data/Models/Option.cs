using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XerShade.Website.Core.Data.Models;

[Table("CoreOptions"), PrimaryKey("OptionId")]
[Index("OptionName", IsUnique = true)]
public class Option
{
    [Required]
    public int OptionId { get; set; }
    [Required]
    public string OptionName { get; set; }
    public string OptionValue { get; set; }
    public bool AutoLoad { get; set; }

    public Option()
    {
        this.OptionName = string.Empty;
        this.OptionValue = string.Empty;
        this.AutoLoad = false;
    }

    public Option(string optionName, string optionValue, bool autoLoad)
    {
        this.OptionName = optionName ?? throw new ArgumentNullException(nameof(optionName));
        this.OptionValue = optionValue;
        this.AutoLoad = autoLoad;
    }

    public Option(int optionId, string optionName, string optionValue, bool autoLoad)
    {
        this.OptionId = optionId;
        this.OptionName = optionName ?? throw new ArgumentNullException(nameof(optionName));
        this.OptionValue = optionValue;
        this.AutoLoad = autoLoad;
    }
}
