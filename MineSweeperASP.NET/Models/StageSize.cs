namespace MineSweeperASP.NET.Models;

/// <summary>
/// 盤面サイズ生成
/// </summary>
public class StageSize : IStageSize
{
    /// <summary>
    /// appsettingsから盤面サイズ取得
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="stageType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public (int rowCount, int columnCount, int bombCount) GetStageSize(IConfiguration configuration, StageType stageType)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        return stageType switch
        {
            StageType.Small =>
                (configuration.GetValue<int>("StageSize:Small:RowCount"),
                configuration.GetValue<int>("StageSize:Small:ColumnCount"),
                configuration.GetValue<int>("StageSize:Small:BombCount")),
            StageType.Medium =>
                (configuration.GetValue<int>("StageSize:Medium:RowCount"),
                configuration.GetValue<int>("StageSize:Medium:ColumnCount"),
                configuration.GetValue<int>("StageSize:Medium:BombCount")),
            StageType.Large =>
                (configuration.GetValue<int>("StageSize:Large:RowCount"),
                configuration.GetValue<int>("StageSize:Large:ColumnCount"),
                configuration.GetValue<int>("StageSize:Large:BombCount")),
            _ => throw new ArgumentNullException(nameof(stageType)),
        };
    }
}
