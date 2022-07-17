using System;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommandAPI.Tests
{
	public class CommandsControllerTests : IDisposable
	{
		private Mock<ICommandAPIRepo> _mockRepo;
		private CommandsProfile _realProfile;
		private MapperConfiguration _configuration;
		private Mapper _mapper;

		public CommandsControllerTests()
		{
			_mockRepo = new Mock<ICommandAPIRepo>();
			_realProfile = new CommandsProfile();
			_configuration = new MapperConfiguration(config => config.AddProfile(_realProfile));
			_mapper = new Mapper(_configuration);
		}

		public void Dispose()
		{
			_mockRepo = null;
			_realProfile = null;
			_configuration = null;
			_mapper = null;
		}

		[Fact]
		public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetAllCommands())
				.Returns(GetCommands(0));
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetAllCommands();

			// Assert
			Assert.IsType<OkObjectResult>(actual.Result);
		}

		[Fact]
		public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResurce()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetAllCommands())
				.Returns(GetCommands(1));
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetAllCommands();

			// Assert
			var okResult = actual.Result as OkObjectResult;
			var commands = okResult?.Value as List<CommandReadDto>;
			Assert.Single(commands);
		}

		[Fact]
		public void GetAllCommands_Returns200Ok_WhenDBHasOneResource()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetAllCommands())
				.Returns(GetCommands(1));
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetAllCommands();

			// Assert
			Assert.IsType<OkObjectResult>(actual.Result);
		}

		[Fact]
		public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetAllCommands())
				.Returns(GetCommands(1));
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetAllCommands();

			// Assert
			Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(actual);
		}

		[Fact]
		public void GetCommandById_Returns404NotFound_WhenNonExistentIDProvided()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(0))
				.Returns(() => null);
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetCommandById(1);

			// Assert
			Assert.IsType<NotFoundResult>(actual.Result);
		}

		[Fact]
		public void GetCommandByID_Returns200Ok_WhenValidIDProvided()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(1))
				.Returns(new Command
				{
					Id = 1,
					HowTo = "mock",
					Platform = "Mock",
					CommandLine = "Mock"
				});
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetCommandById(1);

			// Assert
			Assert.IsType<OkObjectResult>(actual.Result);
		}

		[Fact]
		public void GetCommandByID_ReturnsCorrectResurceType_WhenValidIDProvided()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(1))
				.Returns(new Command
				{
					Id = 1,
					HowTo = "mock",
					Platform = "Mock",
					CommandLine = "Mock"
				});
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.GetCommandById(1);

			// Assert
			Assert.IsType<ActionResult<CommandReadDto>>(actual);
		}

		[Fact]
		public void CreateCommand_ReturnsCorrectResourceType_WhenValidOBjectSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(1))
				.Returns(new Command
				{
					Id = 1,
					HowTo = "mock",
					Platform = "Mock",
					CommandLine = "Mock"
				});
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.CreateCommand(new CommandCreateDto());

			// Assert
			Assert.IsType<ActionResult<CommandReadDto>>(actual);
		}

		[Fact]
		public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(1))
				.Returns(new Command
				{
					Id = 1,
					HowTo = "mock",
					Platform = "Mock",
					CommandLine = "Mock"
				});
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.CreateCommand(new CommandCreateDto());

			// Assert
			Assert.IsType<CreatedAtRouteResult>(actual.Result);
		}

		[Fact]
		public void UpdateCommand_Returns204NoContent_WhenValidObjcetSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(1))
				.Returns(new Command
				{
					Id = 1,
					HowTo = "mock",
					Platform = "Mock",
					CommandLine = "Mock"
				});
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.UpdateCommand(1, new CommandUpdateDto());

			// Assert
			Assert.IsType<NoContentResult>(actual);
		}

		[Fact]
		public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(0))
				.Returns(() => null);
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.UpdateCommand(0, new CommandUpdateDto());

			// Assert
			Assert.IsType<NotFoundResult>(actual);
		}

		[Fact]
		public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(0))
				.Returns(() => null);
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.PartialCommandUpdate(0, new JsonPatchDocument<CommandUpdateDto>());

			// Assert
			Assert.IsType<NotFoundResult>(actual);
		}

		[Fact]
		public void DeleteCommand_Returns204NoContent_WhenValidIDSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(1))
				.Returns(new Command
				{
					Id = 1,
					HowTo = "mock",
					Platform = "Mock",
					CommandLine = "Mock"
				});
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.DeleteCommand(1);

			// Assert
			Assert.IsType<NoContentResult>(actual);
		}

		[Fact]
		public void DeleteCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
		{
			// Arrange
			_mockRepo.Setup(repo => repo.GetCommandById(0))
				.Returns(() => null);
			var controller = new CommandsController(_mockRepo.Object, _mapper);

			// Act
			var actual = controller.DeleteCommand(0);

			// Assert
			Assert.IsType<NotFoundResult>(actual);
		}

		private static List<Command> GetCommands(int num)
		{
			var commands = new List<Command>();
			if (num > 0)
			{
				commands.Add(new Command
				{
					Id = 0,
					HowTo = "How to generate a migration",
					CommandLine = "dotnet ef migrations add <Name of Migration>",
					Platform = ".Net Core EF"
				});
			}
			return commands;
		}
	}
}