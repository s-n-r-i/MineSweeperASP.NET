using System.Collections.Generic;

namespace MineSweeperASP.NET.MineSweeperModels.Tests;

/// <summary>
/// 初期データジェネレーター テスト用stub
/// </summary>
public class DataGeneratorStub : IDataGenerator
{
    public IEnumerable<int> GetRandomIntArray(int _, int __)
    {
        return new[] { 6, 7, 9, 15, 17, 22, 29 };
    }
}

public static class DataGeneratorStubHelper
{
    /// <summary>
    /// クリア手順実施
    /// </summary>
    /// <param name="mineSweeper"></param>
    public static void DoClear(IMineSweeper mineSweeper)
    {
        var indexes = GetClearProcedure();
        foreach (var index in indexes)
        {
            mineSweeper.Open(index);
        }
    }

    /// <summary>
    /// クリア手順取得
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<int> GetClearProcedure()
    {
        return new[] { 0, 1, 2, 3, 4, 5, 8, 12, 13, 14, 16, 18, 23, 28 };
    }
}
