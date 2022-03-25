using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using static MineSweeperASP.NET.MineSweeperModels.Tests.DataGeneratorStubHelper;

namespace MineSweeperASP.NET.MineSweeperModels.Tests;

/// <summary>
/// 状態復元用データ test
/// </summary>
[TestClass()]
public class RestoreDataTests
{
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    private IMineSweeper MineSweeper { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

    private const int RowCount = 5;
    private const int ColumnCount = 6;
    private const int BombCount = 7;

    /// <summary>
    /// 開始時共通処理
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        // できあがる盤面
        // 222110
        // **3*31
        // 223*4*
        // 0012*3
        // 00012*
        MineSweeper = new MineSweeper(new DataGeneratorStub());
        MineSweeper.Start(RowCount, ColumnCount, BombCount);
    }

    [TestMethod("1.オブジェクト構築")]
    public void RestoreDataTest()
    {
        var restore = new RestoreData(
                            MineSweeper.Status,
                            MineSweeper.RowCount,
                            MineSweeper.ColumnCount,
                            MineSweeper.BombCount,
                            MineSweeper.RemainingCellCount,
                            Enumerable.Empty<Cell>(),
                            MineSweeper.GetRemainingCells());
        Assert.AreEqual(MineSweeper.Status, restore.Status);
        Assert.AreEqual(MineSweeper.RowCount, restore.RowCount);
        Assert.AreEqual(MineSweeper.ColumnCount, restore.ColumnCount);
        Assert.AreEqual(MineSweeper.BombCount, restore.BombCount);
        Assert.AreEqual(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
        Assert.AreEqual(0, restore.OpenedCells.Count());
        Assert.AreEqual(MineSweeper.GetRemainingCells().Count(), restore.ClosedCells.Count());
    }

    [TestMethod("2.更新テスト(1)")]
    public void UpdateTest1()
    {
        // 通常時更新後の状態を確認
        var restore = new RestoreData(
                    MineSweeper.Status,
                    MineSweeper.RowCount,
                    MineSweeper.ColumnCount,
                    MineSweeper.BombCount,
                    MineSweeper.RemainingCellCount,
                    Enumerable.Empty<Cell>(),
                    MineSweeper.GetRemainingCells());
        var openedCells = MineSweeper.Open(0);
        restore.Update(MineSweeper.Status, openedCells, MineSweeper.RemainingCellCount);
        Assert.AreEqual(MineSweeper.Status, restore.Status);
        Assert.AreEqual(1, restore.OpenedCells.Count());
        Assert.AreEqual(0, restore.OpenedCells.First().Index);
        Assert.AreEqual(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
    }

    [TestMethod("3.更新テスト(2)")]
    public void UpdateTest2()
    {
        // クリア時更新後の状態を確認
        var restore = new RestoreData(
                    MineSweeper.Status,
                    MineSweeper.RowCount,
                    MineSweeper.ColumnCount,
                    MineSweeper.BombCount,
                    MineSweeper.RemainingCellCount,
                    Enumerable.Empty<Cell>(),
                    MineSweeper.GetRemainingCells());
        var indexes = GetClearProcedure();
        IEnumerable<Cell> openedCells = Enumerable.Empty<Cell>();
        foreach (var index in indexes)
        {
            openedCells = MineSweeper.Open(index);
            restore.Update(MineSweeper.Status, openedCells, MineSweeper.RemainingCellCount);
        }

        Assert.AreEqual(MineSweeper.Status, restore.Status);
        Assert.AreEqual(1, openedCells.Count());
        Assert.AreEqual(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
        Assert.AreEqual(RowCount * ColumnCount, restore.OpenedCells.Count());
        Assert.AreEqual(0, restore.ClosedCells.Count());
    }

    [TestMethod("4.更新テスト(3)")]
    public void UpdateTest3()
    {
        // ゲームオーバー時更新後の状態を確認
        var restore = new RestoreData(
                    MineSweeper.Status,
                    MineSweeper.RowCount,
                    MineSweeper.ColumnCount,
                    MineSweeper.BombCount,
                    MineSweeper.RemainingCellCount,
                    Enumerable.Empty<Cell>(),
                    MineSweeper.GetRemainingCells());
        IEnumerable<Cell> openedCells = Enumerable.Empty<Cell>();
        openedCells = MineSweeper.Open(0);
        restore.Update(MineSweeper.Status, openedCells, MineSweeper.RemainingCellCount);
        openedCells = MineSweeper.Open(6);
        restore.Update(MineSweeper.Status, openedCells, MineSweeper.RemainingCellCount);

        Assert.AreEqual(MineSweeper.Status, restore.Status);
        Assert.AreEqual(1, openedCells.Count());
        Assert.AreEqual(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
        Assert.AreEqual(RowCount * ColumnCount, restore.OpenedCells.Count());
        Assert.AreEqual(0, restore.ClosedCells.Count());
    }
}
