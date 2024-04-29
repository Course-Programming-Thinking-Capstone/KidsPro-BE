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
    public async Task LoginByEmailAsync_ReturnsOkResult_WhenCredentialsAreValid()
    {
        // Arrange
        var input = new EmailCredential
        {
            Email = "long88ka@gmail.com",
            Password = "0000"
        };
        var expectedResponse = new LoginAccountDto();

        _mockRepo.Setup(r => r.LoginByEmailAsync(input.Email)).ReturnsAsync(new Account());
        _mockUnitOfWork.Setup(u => u.AccountRepository).Returns(_mockRepo.Object);
        _mockService.Setup(s => s.LoginByEmailAsync(input)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.LoginByEmailAsync(input);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedResponse));
    }

    [Test]
    public void LoginByEmailAsync_ThrowsUnauthorizedException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var input = new EmailCredential
        {
            Email = "long88ka@gmail.com",
            Password = "0000"
        };

        _mockService.Setup(s => s.LoginByEmailAsync(input))
            .ThrowsAsync(new UnauthorizedException("Invalid credentials"));

        // Act & Assert
          Assert.ThrowsAsync<UnauthorizedException>(
            async () => await _controller.LoginByEmailAsync(input)
        );
    }

}