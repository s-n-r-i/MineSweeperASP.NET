namespace MineSweeperASP.NET.MineSweeperModels;

/// <summary>
/// MineSweeper 状態復元用データ
/// </summary>
public class RestoreData
{
    /// <summary>
    /// 盤面の状態
    /// </summary>
    public StatusType Status { get; private set; }

    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// 列数
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// 爆弾数
    /// </summary>
    public int BombCount { get; }

    /// <summary>
    /// 未オープンセル数(-爆弾数)
    /// </summary>
    public int RemainingCellCount { get; private set; }

    /// <summary>
    /// オープン済みセル
    /// </summary>
    public IEnumerable<Cell> OpenedCells { get; private set; }

    /// <summary>
    /// 未オープンセル
    /// </summary>
    public IEnumerable<Cell> ClosedCells { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="status"></param>
    /// <param name="rowCount"></param>
    /// <param name="columnCount"></param>
    /// <param name="bombCount"></param>
    /// <param name="remainingCellCount"></param>
    /// <param name="openedCells"></param>
    /// <param name="closedCells"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RestoreData(
        StatusType status,
        int rowCount,
        int columnCount,
        int bombCount,
        int remainingCellCount,
        IEnumerable<Cell> openedCells,
        IEnumerable<Cell> closedCells)
    {
        Status = status;
        RowCount = rowCount;
        ColumnCount = columnCount;
        BombCount = bombCount;
        RemainingCellCount = remainingCellCount;
        OpenedCells = openedCells ?? throw new ArgumentNullException(nameof(openedCells));
        ClosedCells = closedCells ?? throw new ArgumentNullException(nameof(closedCells));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="currentStatus"></param>
    /// <param name="additionalOpendCells"></param>
    /// <param name="remainingCellCount"></param>
    public void Update(StatusType currentStatus, IEnumerable<Cell> additionalOpendCells, int remainingCellCount)
    {
        Status = currentStatus;
        RemainingCellCount = remainingCellCount;

        if (additionalOpendCells == null)
            return;

        if (currentStatus is StatusType.Failure or StatusType.Success)
        {
            OpenedCells = OpenedCells.Union(ClosedCells).OrderBy(x => x.Index).ToArray();
            ClosedCells = Enumerable.Empty<Cell>();
        }
        else
        {
            var differernces = ClosedCells.Except(additionalOpendCells).ToArray(); //recordなのでcomparer無しで差集合を作れる
            OpenedCells = OpenedCells.Union(additionalOpendCells).OrderBy(x => x.Index).ToArray();
            ClosedCells = differernces;
        }
    }
}
