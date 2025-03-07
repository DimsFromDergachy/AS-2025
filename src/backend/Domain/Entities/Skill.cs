using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AS_2025.Domain.Entities;

public record Skill
{
    public SkillType Type { get; set; }

    [Range(1, 5, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int ProficiencyLevel { get; set; }
}