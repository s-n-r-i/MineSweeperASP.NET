using System.ComponentModel.DataAnnotations;

namespace MineSweeperASP.NET.Models;

/// <summary>
/// 盤面サイズ
/// </summary>
public enum StageType
{
    [Display(Name = "小")]
    Small,

    [Display(Name = "中")]
    Medium,

    [Display(Name = "大")]
    Large,
}
