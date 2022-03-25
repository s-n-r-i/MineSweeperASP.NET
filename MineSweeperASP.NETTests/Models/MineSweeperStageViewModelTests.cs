using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeperASP.NET.MineSweeperModels;
using System;
using System.Linq;

namespace MineSweeperASP.NET.Models.Tests;

/// <summary>
/// Stage ViewModel test
/// </summary>
[TestClass()]
public class MineSweeperStageViewModelTests
{
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    private MineSweeperStageViewModel VM { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

    private const int RowCount = 2;
    private const int ColumnCount = 5;

    /// <summary>
    /// 開始時共通処理
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        // 想定盤面
        // 23*10
        // **210
        var openedCells = new[]
        {
            new Cell(0,false,2),
            new Cell(1,false,3),
            new Cell(2,true,1),
            new Cell(9,false,0),
        };
        var closedCells = new[]
        {
            new Cell(3,false,1),
            new Cell(4,false,0),
            new Cell(5,true,1),
            new Cell(6,true,2),
            new Cell(7,false,2),
            new Cell(8,false,1),
        };

        var restore = new RestoreData(StatusType.Playing, RowCount, ColumnCount, 3, 5, openedCells, closedCells);
        VM = new(restore);
    }

    [TestMethod("1.オブジェクト構築")]
    public void MineSweeperViewModelTest()
    {
        Assert.AreEqual(RowCount, VM.RowCount);
        Assert.AreEqual(ColumnCount, VM.ColumnCount);
        Assert.AreEqual(5, VM.RemainingCellCount);
    }

    [TestMethod("2.インデックス取得")]
    public void GetIndexTest()
    {
        var index1 = VM.GetIndex(0, 0);
        var index2 = VM.GetIndex(0, 4);
        var index3 = VM.GetIndex(1, 0);
        var index4 = VM.GetIndex(1, 4);
        Assert.AreEqual(0, index1);
        Assert.AreEqual(4, index2);
        Assert.AreEqual(5, index3);
        Assert.AreEqual(9, index4);
    }

    [TestMethod("3.セルが開かれているか")]
    public void IsOpenedTest()
    {
        var isOpened1 = VM.IsOpened(0, 0);
        var isOpened2 = VM.IsOpened(1, 1);
        Assert.IsTrue(isOpened1);
        Assert.IsFalse(isOpened2);
    }

    [TestMethod("4.セル表記文字列")]
    public void GetOpenedStringTest()
    {
        // セルが開いている場合の表記文字列確認(開いていない場合は空文字が返る)
        var str1 = VM.GetOpenedString(0, 0);
        var str2 = VM.GetOpenedString(0, 1);
        var str3 = VM.GetOpenedString(0, 2);
        var str4 = VM.GetOpenedString(1, 0);
        var str5 = VM.GetOpenedString(1, 2);
        var str6 = VM.GetOpenedString(1, 4);
        Assert.AreEqual("2", str1);
        Assert.AreEqual("3", str2);
        Assert.AreEqual("*", str3);
        Assert.AreEqual("", str4);
        Assert.AreEqual("", str5);
        Assert.AreEqual("0", str6);
    }

    [TestMethod("5.終了フラグテスト")]
    public void IsEndTest()
    {
        var restore1 = new RestoreData(StatusType.Failure, 1, 1, 1, 0, Enumerable.Empty<Cell>(), Enumerable.Empty<Cell>());
        var vm1 = new MineSweeperStageViewModel(restore1);
        var restore2 = new RestoreData(StatusType.Success, 1, 1, 1, 0, Enumerable.Empty<Cell>(), Enumerable.Empty<Cell>());
        var vm2 = new MineSweeperStageViewModel(restore2);
        Assert.IsFalse(VM.IsEnd());
        Assert.IsTrue(vm1.IsEnd());
        Assert.IsTrue(vm2.IsEnd());
    }

    [TestMethod("6.例外(1)")]
    public void GetIndexExceptionTest1()
    {
        // 行指定が範囲外ならば例外
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => VM.GetIndex(0, 5));
    }

    [TestMethod("7.例外(2)")]
    public void GetIndexExceptionTest2()
    {
        // 列指定が範囲外ならば例外
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => VM.GetIndex(2, 0));
    }
}
