using MineSweeperASP.NET.MineSweeperModels;

namespace MineSweeperASP.NET.Models;

/// <summary>
/// Stage ViewModel
/// </summary>
public class MineSweeperStageViewModel
{
    /// <summary>
    /// 盤面の状態
    /// </summary>
    public StatusType Status => RestoreData.Status;

    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount => RestoreData.RowCount;

    /// <summary>
    /// 列数
    /// </summary>
    public int ColumnCount => RestoreData.ColumnCount;

    /// <summary>
    /// 未オープンセル数(-爆弾数)
    /// </summary>
    public int RemainingCellCount => RestoreData.RemainingCellCount;

    /// <summary>
    /// 状態復元用データ
    /// </summary>
    private RestoreData RestoreData { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="restore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MineSweeperStageViewModel(RestoreData restore)
    {
        RestoreData = restore ?? throw new ArgumentNullException(nameof(restore));
    }

    /// <summary>
    /// 行・列からインデックス取得
    /// </summary>
    /// <param name="rowNumber"></param>
    /// <param name="columnNumber"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int GetIndex(int rowNumber, int columnNumber)
    {
        if (rowNumber > RowCount - 1)
            throw new ArgumentOutOfRangeException(nameof(rowNumber));

        if (columnNumber > ColumnCount - 1)
            throw new ArgumentOutOfRangeException(nameof(columnNumber));

        return rowNumber * ColumnCount + columnNumber;
    }

    /// <summary>
    /// 行・列で指定されるセルが開かれているか
    /// </summary>
    /// <param name="rowNumber"></param>
    /// <param name="columnNumber"></param>
    /// <returns></returns>
    public bool IsOpened(int rowNumber, int columnNumber)
    {
        var index = GetIndex(rowNumber, columnNumber);
        return RestoreData.OpenedCells.Any(x => x.Index == index);
    }

    /// <summary>
    /// 行・列で指定されるセルの表記文字列取得
    /// </summary>
    /// <param name="rowNumber"></param>
    /// <param name="columnNumber"></param>
    /// <returns></returns>
    public string GetOpenedString(int rowNumber, int columnNumber)
    {
        var index = GetIndex(rowNumber, columnNumber);
        var openedCell = RestoreData.OpenedCells.SingleOrDefault(x => x.Index == index);
        if (openedCell != default)
        {
            return openedCell.IsBomb ? "*" : openedCell.NeighborBombCount.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 終了しているか
    /// </summary>
    /// <returns></returns>
    public bool IsEnd() => RestoreData.Status is StatusType.Failure or StatusType.Success;
}
