namespace MineSweeperASP.NET.Models;

/// <summary>
/// 盤面サイズ生成 インターフェース
/// </summary>
public interface IStageSize
{
    public (int rowCount, int columnCount, int bombCount) GetStageSize(IConfiguration configuration, StageType stageType);
}
