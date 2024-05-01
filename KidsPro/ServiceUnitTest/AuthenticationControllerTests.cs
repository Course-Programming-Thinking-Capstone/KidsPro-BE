using Application;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
namespace UnitTest;
[TestFixture]
public class AuthenticationControllerTests
{
    private Mock<IAccountService> _mockService;
    private Mock<IAccountRepository> _mockRepo;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private AuthenticationController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IAccountService>();
        _mockRepo = new Mock<IAccountRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _controller = new AuthenticationController(_mockService.Object);
    }

    [Test]
    public void LoginByEmailAsync_ThrowsUnauthorizedException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new EmailCredential
        {
            Email = "test@example.com",
            Password = "invalidpassword"
        };

        _mockService.Setup(s => s.LoginByEmailAsync(request))
            .ThrowsAsync(new UnauthorizedException("Invalid credentials"));

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedException>(
            async () => await _controller.LoginByEmailAsync(request)
        );
    }
    [Test]
    public async Task CheckConfirmation_ReturnsOkResult()
    {
        // Arrange
        var code = "confirmationCode";

        // Act
        var result = await _controller.CheckConfirmation(code);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
    [Test]
    public async Task SendConfirmation_ReturnsOkResult()
    {
        // Act
        var result = await _controller.SendConfirmation();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
   
    [Test]
    public async Task StudentLoginToWebAsync_ReturnsOkResult_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new StudentLoginRequest
        {
            Account = "username",
            Password = "password"
        };
        var expectedResult = new LoginAccountDto();

        _mockService.Setup(s => s.StudentLoginToWeb(request)).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.StudentLoginToWebAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        Assert.AreEqual(expectedResult, (result.Result as OkObjectResult).Value);
    }
 
    [Test]
    public void RegisterByEmailAsync_ThrowsConflictException_WhenEmailIsAlreadyRegistered()
    {
        // Arrange
        var request = new EmailRegisterDto
        {
            Email = "existing@example.com",
            Password = "password"
        };

        _mockService.Setup(s => s.RegisterByEmailAsync(request))
            .ThrowsAsync(new ConflictException("Email is already registered"));

        // Act & Assert
        Assert.ThrowsAsync<ConflictException>(
            async () => await _controller.RegisterByEmailAsync(request)
        );
    }
    [Test]
    public void CheckConfirmation_ThrowsException_WhenConfirmationCodeIsInvalid()
    {
        // Arrange
        var code = "invalidCode";

        _mockService.Setup(s => s.CheckConfirmation(code))
            .ThrowsAsync(new Exception("Invalid confirmation code"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(
            async () => await _controller.CheckConfirmation(code)
        );
    }
    [Test]
    public async Task UpdateToNotActivatedStatusAsync_ReturnsOkResult()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var result = await _controller.UpdateToNotActivatedStatusAsync(email);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

}