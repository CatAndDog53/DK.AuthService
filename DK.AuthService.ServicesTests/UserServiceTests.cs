using DK.AuthService.Model;
using DK.AuthService.Model.Dtos;
using DK.AuthService.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DK.AuthService.ServicesTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _userService = new UserService(_mockUserManager.Object, null);
        }

        [Test]
        public async Task GetCurrentUserInfo_ValidUserName_ReturnsUserInfoDto()
        {
            // Arrange
            var currentUserName = "testuser";
            var expectedUserInfo = new UserInfoDto
            {
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@example.com"
            };

            var userWithCurrentUsername = new ApplicationUser
            {
                UserName = currentUserName,
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@example.com"
            };

            _mockUserManager
                .Setup(u => u.FindByNameAsync(currentUserName))
                .ReturnsAsync(userWithCurrentUsername);

            // Act
            var result = await _userService.GetCurrentUserInfo(currentUserName);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserName, Is.EqualTo(expectedUserInfo.UserName));
            Assert.That(result.FirstName, Is.EqualTo(expectedUserInfo.FirstName));
            Assert.That(result.LastName, Is.EqualTo(expectedUserInfo.LastName));
            Assert.That(result.Email, Is.EqualTo(expectedUserInfo.Email));
        }

        [Test]
        public async Task GetAllUsersInfo_ReturnsListOfUserInfoDto()
        {
            // Arrange
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "user1",
                    FirstName = "User",
                    LastName = "One",
                    Email = "user1@example.com"
                },
                new ApplicationUser
                {
                    UserName = "user2",
                    FirstName = "User",
                    LastName = "Two",
                    Email = "user2@example.com"
                }
            };

            var expectedUsersInfo = users.Select(u => new UserInfoDto
            {
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            });

            _mockUserManager
                .Setup(u => u.Users)
                .Returns(users.AsQueryable());

            // Act
            var result = await _userService.GetAllUsersInfo();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(expectedUsersInfo.Count()));

            foreach (var expectedUserInfo in expectedUsersInfo)
            {
                var actualUserInfo = result.FirstOrDefault(u => u.UserName == expectedUserInfo.UserName);

                Assert.IsNotNull(actualUserInfo);
                Assert.That(actualUserInfo.FirstName, Is.EqualTo(expectedUserInfo.FirstName));
                Assert.That(actualUserInfo.LastName, Is.EqualTo(expectedUserInfo.LastName));
                Assert.That(actualUserInfo.Email, Is.EqualTo(expectedUserInfo.Email));
            }
        }

        [Test]
        public async Task RegisterAsync_ValidData_CallsCreateAsyncAndAddToRoleAsync()
        {
            // Arrange
            var registerDto = new RegisterDataDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@example.com",
                UserName = "testuser",
                Password = "password"
            };

            _mockUserManager
                .Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager
                .Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var response = await _userService.RegisterAsync(registerDto);

            // Assert
            Assert.IsTrue(response.IsSucceed);
            Assert.That(response.Message, Is.EqualTo("User created successfully!"));
            _mockUserManager.Verify(
                u => u.CreateAsync(It.IsAny<ApplicationUser>(), registerDto.Password), Times.Once);
            _mockUserManager.Verify(
                u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task RegisterAsync_WithInvalidUsername_DoesNotCallCreateAsync()
        {
            // Arrange
            var registerDto = new RegisterDataDto
            {
                UserName = "existingUser",
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@example.com",
                Password = "password"
            };

            var userByName = new ApplicationUser
            {
                UserName = "existingUser"
            };

            _mockUserManager
                .Setup(u => u.FindByNameAsync(registerDto.UserName))
                .ReturnsAsync(userByName);

            // Act
            var response = await _userService.RegisterAsync(registerDto);

            // Assert
            Assert.IsFalse(response.IsSucceed);
            Assert.That(response.Message, Is.EqualTo($"User with username {userByName.UserName} already exists!"));

            _mockUserManager.Verify(
                u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task RegisterAsync_InvalidEmail_DoesNotCallCreateAsync()
        {
            // Arrange
            var registerDto = new RegisterDataDto
            {
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                Email = "existing@example.com",
                Password = "password"
            };

            var userByEmail = new ApplicationUser
            {
                Email = "existing@example.com"
            };

            _mockUserManager
                .Setup(u => u.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync(userByEmail);

            // Act
            var response = await _userService.RegisterAsync(registerDto);

            // Assert
            Assert.IsFalse(response.IsSucceed);
            Assert.That(response.Message, Is.EqualTo($"User with email {userByEmail.Email} already exists!"));

            _mockUserManager.Verify(
                u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateUserInfoAsync_ValidData_CallsUpdateAsync()
        {
            // Arrange
            var currentUserName = "testuser";
            var updatedUserInfo = new UpdateUserInfoDto
            {
                FirstName = "Test",
                LastName = "User",
                UserName = "testuser_updated"
            };
            var userWithWantedUsername = new ApplicationUser { UserName = updatedUserInfo.UserName };

            _mockUserManager
                .Setup(u => u.FindByNameAsync(currentUserName))
                .ReturnsAsync(userWithWantedUsername);
            _mockUserManager
                .Setup(u => u.FindByNameAsync(updatedUserInfo.UserName))
                .ReturnsAsync((ApplicationUser)null);
            _mockUserManager
                .Setup(u => u.UpdateAsync(userWithWantedUsername))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var response = await _userService.UpdateUserInfoAsync(currentUserName, updatedUserInfo);

            // Assert
            Assert.IsTrue(response.IsSucceed);
            Assert.That(response.Message, Is.EqualTo("User updated successfully!"));
            _mockUserManager.Verify(
                u => u.UpdateAsync(userWithWantedUsername), Times.Once);
        }

        [Test]
        public async Task UpdateUserInfoAsync_InvalidUsername_DoesNotCallUpdateAsync()
        {
            // Arrange
            var currentUserName = "testuser";
            var updatedUserInfo = new UpdateUserInfoDto
            {
                UserName = "existingUser",
                FirstName = "John",
                LastName = "Doe"
            };

            var userWithWantedUsername = new ApplicationUser
            {
                UserName = "existingUser"
            };

            _mockUserManager
                .Setup(u => u.FindByNameAsync(updatedUserInfo.UserName))
                .ReturnsAsync(userWithWantedUsername);

            // Act
            var response = await _userService.UpdateUserInfoAsync(currentUserName, updatedUserInfo);

            // Assert
            Assert.IsFalse(response.IsSucceed);
            Assert.That(response.Message, Is.EqualTo($"Username {userWithWantedUsername.UserName} is already taken!"));

            _mockUserManager.Verify(
                u => u.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Test]
        public async Task ChangeUserPasswordAsync_ValidData_CallsChangePasswordAsync()
        {
            // Arrange
            var currentUserName = "testuser";
            var changeUserPasswordDto = new ChangeUserPasswordDto
            {
                OldPassword = "Old password",
                NewPassword = "New password"
            };
            var userWithCurrentUsername = new ApplicationUser { UserName = currentUserName };

            _mockUserManager
                .Setup(u => u.FindByNameAsync(currentUserName))
                .ReturnsAsync(userWithCurrentUsername);
            _mockUserManager
                .Setup(u => u.ChangePasswordAsync(userWithCurrentUsername, changeUserPasswordDto.OldPassword, changeUserPasswordDto.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var response = await _userService.ChangeUserPassword(currentUserName, changeUserPasswordDto);

            // Assert
            Assert.IsTrue(response.IsSucceed);
            Assert.That(response.Message, Is.EqualTo("Password was successfully changed!"));

            _mockUserManager.Verify(
                u => u.ChangePasswordAsync(userWithCurrentUsername, changeUserPasswordDto.OldPassword, changeUserPasswordDto.NewPassword), Times.Once);
        }
    }
}