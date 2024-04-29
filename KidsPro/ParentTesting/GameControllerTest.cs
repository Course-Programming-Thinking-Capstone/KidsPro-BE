using Application;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Moq;
using WebAPI.Controllers;

namespace ParentTesting;

[TestFixture]
public class GameControllerTest
{
    private Mock<IAccountService> _mockService;
    private Mock<IGameService> _mockGameService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private GamesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IAccountService>();
        _mockGameService = new Mock<IGameService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _controller = new GamesController(_mockService.Object, _mockGameService.Object);
    }


    
}