using Microsoft.Extensions.Configuration;

namespace MineSweeperASP.NET.Models.Tests;

/// <summary>
/// 盤面サイズ テスト用stub
/// </summary>
public class StageSizeStub : IStageSize
{
    public (int rowCount, int columnCount, int bombCount) GetStageSize(IConfiguration _, StageType __) => (5, 6, 7);
}
