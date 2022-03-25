using System.ComponentModel.DataAnnotations;

namespace MineSweeperASP.NET.Models;

/// <summary>
/// Index ViewModel
/// </summary>
public class MineSweeperIndexViewModel
{
    [Display(Name = "盤の選択")]
    [Required]
    public StageType StageType { get; set; }
}
