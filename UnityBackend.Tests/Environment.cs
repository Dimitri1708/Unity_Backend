using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity_Backend.Controllers;
using Unity_Backend.DTO_s;
using Unity_Backend.Repositories;
using Unity_Backend.Models;

namespace UnityBackend.Tests
{
    public class ObjectControllerTests
    {
        private readonly Mock<IObjectRepository> _mockRepo;
        private readonly ObjectController _controller;

        public ObjectControllerTests()
        {
            _mockRepo = new Mock<IObjectRepository>();
            _controller = new ObjectController(_mockRepo.Object);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult_OnSuccess()
        {
            var dtoWrapper = new ObjectCreateDtoListWrapper { objectCreateDtoList = new List<ObjectCreateDto>() };
            var result = await _controller.Create(dtoWrapper);
            Assert.IsType<CreatedResult>(result);
            _mockRepo.Verify(r => r.Create(dtoWrapper.objectCreateDtoList), Times.Once);
        }

        [Fact]
        public async Task Read_ReturnsOkWithObjects_WhenObjectsExist()
        {
            var envId = "env1";
            var objects = new List<ObjectReadDto> { new ObjectReadDto() };
            _mockRepo.Setup(r => r.Read(envId)).ReturnsAsync(objects);
            var result = await _controller.Read(envId);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var wrapper = Assert.IsType<ObjectReadDtoListWrapper>(okResult.Value);
            Assert.NotEmpty(wrapper.objectReadDtoList);
        }

        [Fact]
        public async Task Delete_ReturnsOk_OnSuccess()
        {
            var ids = new ObjectIdListWrapper { objectIdList = new List<string> { "id1" } };
            var result = await _controller.Delete(ids);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("objects deleted successfully", okResult.Value.ToString(), StringComparison.OrdinalIgnoreCase);
            _mockRepo.Verify(r => r.Delete(ids.objectIdList, It.IsAny<string>()), Times.Once);
        }
    }
}


