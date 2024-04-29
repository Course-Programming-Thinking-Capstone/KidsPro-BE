using Application;
using Application.Dtos.Request.Game;
using Application.Dtos.Response.Game;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

namespace ParentTesting;

[TestFixture]
public class GameControllerTest
{
    private Mock<IAccountService> _mockService;
    private Mock<IGameService> _mockGameService;
    private Mock<IGameLevelRepository> _mockGameLevelRepo;
    private Mock<IGameRepository> _mockGameRepo;
    private Mock<IGameItemRepository> _mockGameItemRepo;
    private Mock<IGameLevelModifierRepository> _mockGameLevelModifierRepo;
    private Mock<IGameLevelDetailRepository> _mockGameLevelDetailRepo;
    private Mock<IGamePlayHistoryRepository> _mockGamePlayHistoryRepo;
    private Mock<IGameQuizRoomRepository> _mockGameQuizRoomRepo;
    private Mock<IGameStudentQuizRepository> _mockGameStudentQuizRepo;
    private Mock<IGameVersionRepository> _mockGameVersionRepo;

    private Mock<IUnitOfWork> _mockUnitOfWork;
    private GamesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IAccountService>();
        _mockGameService = new Mock<IGameService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _controller = new GamesController(_mockService.Object, _mockGameService.Object);

        // Setup mock repositories related to "game"
        _mockGameLevelRepo = new Mock<IGameLevelRepository>();
        _mockGameRepo = new Mock<IGameRepository>();
        _mockGameItemRepo = new Mock<IGameItemRepository>();
        _mockGameLevelModifierRepo = new Mock<IGameLevelModifierRepository>();
        _mockGameLevelDetailRepo = new Mock<IGameLevelDetailRepository>();
        _mockGamePlayHistoryRepo = new Mock<IGamePlayHistoryRepository>();
        _mockGameQuizRoomRepo = new Mock<IGameQuizRoomRepository>();
        _mockGameStudentQuizRepo = new Mock<IGameStudentQuizRepository>();
        _mockGameVersionRepo = new Mock<IGameVersionRepository>();

        // Setup mock unitOfWork
        _mockUnitOfWork.Setup(u => u.GameLevelRepository).Returns(_mockGameLevelRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameRepository).Returns(_mockGameRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameItemRepository).Returns(_mockGameItemRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameLevelModifierRepository).Returns(_mockGameLevelModifierRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameLevelDetailRepository).Returns(_mockGameLevelDetailRepo.Object);
        _mockUnitOfWork.Setup(u => u.GamePlayHistoryRepository).Returns(_mockGamePlayHistoryRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameQuizRoomRepository).Returns(_mockGameQuizRoomRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameStudentQuizRepository).Returns(_mockGameStudentQuizRepo.Object);
        _mockUnitOfWork.Setup(u => u.GameVersionRepository).Returns(_mockGameVersionRepo.Object);
    }

    [Test]
    public async Task GetUserItem_Returns_OkResult()
    {
        // Arrange
        var userId = 1;
        var expectedResult = new List<UserInventoryResponse>();

        _mockGameService.Setup(service => service.GetUserItem(userId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetUserItem(userId);
        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task BuyVoucher_Returns_OkResult()
    {
        // Arrange
        var buyVoucherRequest = new BuyVoucherRequest();
        var expectedResult = new BuyResponse();

        _mockGameService.Setup(service => service.BuyVoucher(buyVoucherRequest))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.BuyVoucher(buyVoucherRequest);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task GetUserItem_WithValidUserId_Returns_OkResult()
    {
        // Arrange
        var validUserId = 1; // Provide a valid user ID
        var expectedResult = new List<UserInventoryResponse>(); // Define the expected result

        _mockGameService.Setup(service => service.GetUserItem(validUserId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetUserItem(validUserId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task InitDatabase_Returns_OkResult()
    {
        // Arrange
        _mockGameService.Setup(service => service.InitDatabase())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.InitDatabase();

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public async Task UserBuyItem_WithValidData_Returns_OkResult()
    {
        // Arrange
        var itemId = 1; // Provide a valid item ID
        var userId = 1; // Provide a valid user ID
        var expectedResult = new BuyResponse(); // Define the expected result

        _mockGameService.Setup(service => service.BuyItemFromShop(itemId, userId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.UserBuyItem(itemId, userId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task FinishLevelGame_WithValidRequest_Returns_OkResult()
    {
        // Arrange
        var userFinishLevelRequest = new UserFinishLevelRequest(); // Provide valid data for the request
        var expectedResult = new UserFinishLevelResponse(); // Define the expected result

        _mockGameService.Setup(service => service.UserFinishLevel(userFinishLevelRequest))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.FinishLevelGame(userFinishLevelRequest);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task GetCurrentLevelByUserId_WithValidId_Returns_OkResult()
    {
        // Arrange
        var userId = 1; // Provide a valid user ID
        var expectedResult = new List<CurrentLevelData>();

        _mockGameService.Setup(service => service.GetUserCurrentLevel(userId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetCurrentLevelByUserId(userId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task GetAllGameMode_Returns_OkResult()
    {
        // Arrange
        var expectedResult = new List<ModeType>(); // Define the expected result

        _mockGameService.Setup(service => service.GetAllGameMode())
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAllGameMode();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task GetLevelData_WithValidData_Returns_OkResult()
    {
        // Arrange
        var id = 1; 
        var index = 1;
        var expectedResult = new LevelInformationResponse(); // Define the expected result

        _mockGameService.Setup(service => service.GetLevelInformation(id, index))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetLevelData(id, index);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult, okResult.Value);
    }

    [Test]
    public async Task AddNewLevels_WithValidData_Returns_OkResult()
    {
        // Arrange
        var modifiedLevelData = new ModifiedLevelDataRequest
        {
            CoinReward = 100,
            GemReward = 100,
            LevelIndex = 99,
            VStartPosition = 15,
            GameLevelTypeId = 2,
            LevelDetail = new List<LevelDetailRequest>()
            {
                new LevelDetailRequest
                {
                    VPosition = 16,
                    TypeId = 2
                }
            }
        }; // Provide valid data for the request

        // Act
        var result = await _controller.AddNewLevels(modifiedLevelData);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public async Task DeleteLevel_WithValidId_Returns_OkResult()
    {
        // Arrange
        var id = 1; // Provide a valid level id

        // Act
        var result = await _controller.DeleteLevel(id);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }
}