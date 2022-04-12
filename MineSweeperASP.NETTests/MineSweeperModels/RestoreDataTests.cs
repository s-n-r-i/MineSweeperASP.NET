using Xunit;
using static MineSweeperASP.NET.MineSweeperModels.Tests.DataGeneratorStubHelper;

namespace MineSweeperASP.NET.MineSweeperModels.Tests;

/// <summary>
/// 状態復元用データ test
/// </summary>
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
    public RestoreDataTests()
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

    [Fact(DisplayName = "1.オブジェクト構築")]
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
        Assert.Equal(MineSweeper.Status, restore.Status);
        Assert.Equal(MineSweeper.RowCount, restore.RowCount);
        Assert.Equal(MineSweeper.ColumnCount, restore.ColumnCount);
        Assert.Equal(MineSweeper.BombCount, restore.BombCount);
        Assert.Equal(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
        Assert.Empty(restore.OpenedCells);
        Assert.Equal(MineSweeper.GetRemainingCells().Count(), restore.ClosedCells.Count());
    }

    [Fact(DisplayName = "2.更新テスト(1)")]
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
        Assert.Equal(MineSweeper.Status, restore.Status);
        Assert.Single(restore.OpenedCells);
        Assert.Equal(0, restore.OpenedCells.First().Index);
        Assert.Equal(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
    }

    [Fact(DisplayName = "3.更新テスト(2)")]
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

        Assert.Equal(MineSweeper.Status, restore.Status);
        Assert.Single(openedCells);
        Assert.Equal(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
        Assert.Equal(RowCount * ColumnCount, restore.OpenedCells.Count());
        Assert.Empty(restore.ClosedCells);
    }

    [Fact(DisplayName = "4.更新テスト(3)")]
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

        Assert.Equal(MineSweeper.Status, restore.Status);
        Assert.Single(openedCells);
        Assert.Equal(MineSweeper.RemainingCellCount, restore.RemainingCellCount);
        Assert.Equal(RowCount * ColumnCount, restore.OpenedCells.Count());
        Assert.Empty(restore.ClosedCells);
    }
}
