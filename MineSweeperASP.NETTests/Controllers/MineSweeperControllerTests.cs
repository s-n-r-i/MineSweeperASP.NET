using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MineSweeperASP.NET.MineSweeperModels;
using MineSweeperASP.NET.MineSweeperModels.Tests;
using MineSweeperASP.NET.Models;
using MineSweeperASP.NET.Models.Tests;
using Moq;
using Xunit;
using static MineSweeperASP.NET.MineSweeperModels.Tests.DataGeneratorStubHelper;

namespace MineSweeperASP.NET.Controllers.Tests;

/// <summary>
/// MineSweeperController test
/// </summary>
public class MineSweeperControllerTests
{
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    private Mock<HttpContext> MockHttpContext { get; set; }
    private Mock<IConfiguration> MockConfiguration { get; set; }
    private Mock<ILogger<MineSweeperController>> MockLogger { get; set; }
    private IStageSize StageSize { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

    public MineSweeperControllerTests()
    {
        var mockSession = new MockHttpSession();
        MockHttpContext = new();
        MockHttpContext.Setup(s => s.Session).Returns(mockSession);

        MockConfiguration = new();
        MockLogger = new();
        StageSize = new StageSizeStub();
    }

    [Fact(DisplayName = "1.Index")]
    public void IndexTest()
    {
        var controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        var result = controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact(DisplayName = "2.Stage(Post)")]
    public void StageTest1()
    {
        var controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;
        var model = new MineSweeperIndexViewModel { StageType = StageType.Small };

        var result = controller.Stage(model);
        var stage = controller.HttpContext.Session.Get<StageType>("stage");

        Assert.IsType<ViewResult>(result);
        Assert.IsType<StageType>(stage);
    }

    [Fact(DisplayName = "3.Stage(パラメータ無し)")]
    public void StageTest2()
    {
        var controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;

        var result = controller.Stage(default(int?));
        var restore = controller.HttpContext.Session.Get<RestoreData>("restore");

        Assert.IsType<ViewResult>(result);
        Assert.IsType<RestoreData>(restore);
    }

    [Fact(DisplayName = "4.Stage(パラメータあり)")]
    public void StageTest3()
    {
        var controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;

        _ = controller.Stage(default(int?));

        controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;

        var result = controller.Stage(0);
        var restore = controller.HttpContext.Session.Get<RestoreData>("restore");

        Assert.IsType<ViewResult>(result);
        Assert.IsType<RestoreData>(restore);
    }

    [Fact(DisplayName = "5.Stage(クリア時)")]
    public void StageTest4()
    {
        var controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;

        _ = controller.Stage(default(int?));

        var indexes = GetClearProcedure();
        IActionResult? result = null;
        foreach (var index in indexes)
        {
            controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
            controller.ControllerContext.HttpContext = MockHttpContext.Object;
            result = controller.Stage(index);
        }

        var restore = controller.HttpContext.Session.Get<RestoreData>("restore");

        Assert.IsType<ViewResult>(result);
        Assert.Null(restore);
    }

    [Fact(DisplayName = "6.Stage(ゲームオーバー時)")]
    public void StageTest5()
    {
        var controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;

        _ = controller.Stage(default(int?));

        controller = new MineSweeperController(MockConfiguration.Object, MockLogger.Object, StageSize, new MineSweeper(new DataGeneratorStub()));
        controller.ControllerContext.HttpContext = MockHttpContext.Object;

        var result = controller.Stage(6);
        var restore = controller.HttpContext.Session.Get<RestoreData>("restore");

        Assert.IsType<ViewResult>(result);
        Assert.Null(restore);
    }
}
