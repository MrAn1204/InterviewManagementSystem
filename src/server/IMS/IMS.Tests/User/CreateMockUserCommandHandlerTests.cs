
using AutoMapper;
using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace IMS.Tests.Handlers
{
    [TestFixture]
    public class CreateMockUserCommandHandlerTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<RoleManager<Role>> _roleManagerMock;
        private Mock<IUnitOfWorks> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;

        private CreateMockUserCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<Role>>();
            _roleManagerMock = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);

            _unitOfWorkMock = new Mock<IUnitOfWorks>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateMockUserCommandHandler(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task Handle_ShouldCreateUserSuccessfully_WhenValidRequest()
        {
            // Arrange
            var command = new CreateMockUserCommand
            {
                Email = "test.user@example.com",
                FullName = "Test User",
                DepartmentId = 1,
                DOB = new DateTime(1995, 1, 1),
                Address = "123 Test St",
                Roles = new[] { "Admin" },
                Gender = "Male"
            };

            var department = new Department { Id = 1, DepartmentName = "IT" };

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" }
            }.AsQueryable();

            var user = new User
            {
                Id = 10,
                Email = command.Email,
                UserName = "testuser",
                FullName = command.FullName
            };

            _unitOfWorkMock.Setup(u => u.DepartmentRepository.GetByIdAsync(command.DepartmentId))
                .ReturnsAsync(department);

            _roleManagerMock.Setup(r => r.Roles)
                .Returns(roles);

            _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            var dbTransactionMock = new Mock<IDbContextTransaction>();
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                .ReturnsAsync(dbTransactionMock.Object);

            _mapperMock.Setup(m => m.Map<UserDetailViewModel>(It.IsAny<User>()))
                .Returns(new UserDetailViewModel
                {
                    Id = 10,
                    Email = user.Email,
                    Username = "testuser",
                    FullName = user.FullName ?? "",
                    Roles = new[] { "Admin" }
                });

            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "Admin" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("test.user@example.com"));
            Assert.That(result.Roles.First(), Is.EqualTo("Admin"));
            Assert.That(result.Username, Is.EqualTo("testuser"));
        }
        



    }
}
