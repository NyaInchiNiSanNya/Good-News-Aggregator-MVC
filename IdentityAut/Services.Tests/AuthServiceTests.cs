using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Moq;
using IServices;
using Services.Account;
using AutoMapper;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;
using IServices.Services;
using Microsoft.EntityFrameworkCore;

namespace Services.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IUiThemeService> _uiMock = new Mock<IUiThemeService>();
        private readonly Mock<IRoleService> _roleMock = new Mock<IRoleService>();
        
        
        private AuthService CreateService()
        {
            var service = new AuthService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _uiMock.Object,
                _roleMock.Object
            );

            return service;
        }

        [Fact]
        public async void RegistrationAsync_UserExist_ReturnFalse()
        {

            var service = CreateService();

            var result= await service.RegistrationAsync(It.IsAny<UserRegistrationDto>());

            Assert.False(result);
        }
    }
}