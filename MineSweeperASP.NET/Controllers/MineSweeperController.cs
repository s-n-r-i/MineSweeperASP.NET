using Microsoft.AspNetCore.Mvc;
using MineSweeperASP.NET.MineSweeperModels;
using MineSweeperASP.NET.Models;
using System.Diagnostics;

namespace MineSweeperASP.NET.Controllers;

/// <summary>
/// MineSweeper Controller
/// </summary>
public class MineSweeperController : Controller
{
    /// <summary>
    /// configuration
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// logger
    /// </summary>
    private readonly ILogger<MineSweeperController> _logger;

    /// <summary>
    /// 盤面サイズ生成
    /// </summary>
    private readonly IStageSize _stageSize;

    /// <summary>
    /// MineSweeper本体
    /// </summary>
    private readonly IMineSweeper _mineSweeper;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <param name="stageSize"></param>
    /// <param name="mineSweeper"></param>
    public MineSweeperController(IConfiguration configuration, ILogger<MineSweeperController> logger, IStageSize stageSize, IMineSweeper mineSweeper)
    {
        _configuration = configuration;
        _logger = logger;
        _stageSize = stageSize;
        _mineSweeper = mineSweeper;
    }

    /// <summary>
    /// Index
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index() => View();

    /// <summary>
    /// Stage
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Stage([Bind("StageType")] MineSweeperIndexViewModel model)
    {
        HttpContext.Session.Set<StageType>("stage", model.StageType);
        return Stage(default(int?));
    }

    /// <summary>
    /// Stage
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Stage(int? id)
    {
        var stage = HttpContext.Session.Get<StageType>("stage");

        if (id == null || HttpContext.Session.Get<RestoreData>("restore") is not { } restoreData)
        {
            var (rowCount, columnCount, bombCount) = _stageSize.GetStageSize(_configuration, stage);
            _mineSweeper.Start(rowCount, columnCount, bombCount);
            var remaining = _mineSweeper.GetRemainingCells();
            restoreData = new RestoreData(
                            _mineSweeper.Status,
                            _mineSweeper.RowCount,
                            _mineSweeper.ColumnCount,
                            _mineSweeper.BombCount,
                            _mineSweeper.RemainingCellCount,
                            Enumerable.Empty<Cell>(),
                            remaining);
        }
        else
        {
            _mineSweeper.Restore(restoreData);
            var opened = _mineSweeper.Open(id.Value);
            restoreData.Update(_mineSweeper.Status, opened, _mineSweeper.RemainingCellCount);
        }

        System.Diagnostics.Debug.WriteLine(_mineSweeper.ToString());

        if (_mineSweeper.Status is StatusType.Failure or StatusType.Success)
        {
            HttpContext.Session.Remove("restore");
        }
        else
        {
            HttpContext.Session.Set<RestoreData>("restore", restoreData);
        }

        HttpContext.Session.Set<StageType>("stage", stage); //セッション更新
        var vm = new MineSweeperStageViewModel(restoreData);
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
