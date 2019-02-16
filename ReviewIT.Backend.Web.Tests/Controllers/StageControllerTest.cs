using Microsoft.AspNetCore.Mvc;
using Moq;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Models.Repositories;
using ReviewIT.Backend.Web.Controllers;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReviewIT.Backend.Web.Tests.Controllers
{
    public class StageControllerTest
    {
        private readonly Mock<ILogger<StageController>> log = new Mock<ILogger<StageController>>();

        [Fact(DisplayName = "Get returns Ok with stages")]
        public async Task Get_returns_Ok_with_stages()
        {
            // Arrange
            var stages = new StageDTO[0];

            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(stages);

            var controller = new StageController(repository.Object, log.Object);

            // Act
            var result = await controller.Get() as OkObjectResult;

            // Assert
            Assert.Equal(stages, result.Value);
        }

        [Fact(DisplayName = "Get given id returns Ok with stage")]
        public async Task Get_given_id_returns_Ok_with_stage()
        {
            // Arrange
            var stage = new StageDTO();

            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.FindAsync(11)).ReturnsAsync(stage);

            var controller = new StageController(repository.Object, log.Object);

            // Act
            var result = await controller.Get(11) as OkObjectResult;

            // Assert
            Assert.Equal(stage, result.Value);
        }

        [Fact(DisplayName = "Get given nonexisting id returns NotFound")]
        public async Task Get_given_nonexisting_id_returns_NotFound()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.FindAsync(11)).ReturnsAsync(default(StageDTO));

            var controller = new StageController(repository.Object, log.Object);

            // Act
            var result = await controller.Get(11);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid stage returns BadRequest")]
        public async Task Post_given_invalid_stage_returns_BadRequest()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();

            var controller = new StageController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var stage = new StageNoIdDTO();

            // Act
            var result = await controller.Post(stage);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Post given invalid stage doesnot call CreateAsync")]
        public async Task Post_given_invalid_stage_doesnot_call_CreateAsync()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();

            var controller = new StageController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var stage = new StageNoIdDTO();

            // Act
            await controller.Post(stage);

            // Assert
            repository.Verify(r => r.CreateAsync(It.IsAny<StageNoIdDTO>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid stage calls CreateAsync")]
        public async Task Post_given_valid_stage_calls_CreateAsync()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            var controller = new StageController(repository.Object, log.Object);

            var stage = new StageNoIdDTO();

            // Act
            await controller.Post(stage);

            // Assert
            repository.Verify(r => r.CreateAsync(stage));
        }

        [Fact(DisplayName = "Post given valid stage returns CreatedAtAction")]
        public async Task Post_given_valid_stage_returns_CreatedAtAction()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<StageNoIdDTO>())).ReturnsAsync(11);

            var controller = new StageController(repository.Object, log.Object);

            var stage = new StageNoIdDTO();

            // Act
            var result = await controller.Post(stage) as CreatedAtActionResult;

            // Assert
            Assert.Equal(nameof(StageController.Get), result.ActionName);
            Assert.Equal(11, result.RouteValues["id"]);
        }

        [Fact(DisplayName = "Put given invalid stage returns BadRequest")]
        public async Task Put_given_invalid_stage_returns_BadRequest()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();

            var controller = new StageController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var stage = new StageDTO { Id = 11 };

            // Act
            var result = await controller.Put(11, stage);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Put given id not eq stageId returns BadRequest")]
        public async Task Put_given_id_not_eq_to_stageId_returns_BadRequest()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            var controller = new StageController(repository.Object, log.Object);

            var customer = new StageDTO { Id = 11 };

            // Act
            var result = await controller.Put(0, customer);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Put given invalid stage doesnot call UpdateAsync")]
        public async Task Put_given_invalid_stage_doesnot_call_UpdateAsync()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();

            var controller = new StageController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var stage = new StageDTO();

            // Act
            await controller.Put(11, stage);

            // Assert
            repository.Verify(r => r.UpdateAsync(It.IsAny<StageDTO>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid stage calls UpdateAsync")]
        public async Task Put_given_valid_stage_calls_UpdateAsync()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            var controller = new StageController(repository.Object, log.Object);

            var stage = new StageDTO { Id = 11 };

            // Act
            await controller.Put(11, stage);

            // Arrange
            repository.Verify(r => r.UpdateAsync(stage));
        }

        [Fact(DisplayName = "Put given nonexisting stage returns NotFound")]
        public async Task Put_given_nonexisting_stage_returns_NotFound()
        {
            // Act
            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<StageDTO>())).ReturnsAsync(false);

            var controller = new StageController(repository.Object, log.Object);

            var stage = new StageDTO { Id = 11 };

            // Act
            var result = await controller.Put(11, stage);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid stage returns NoContent")]
        public async Task Put_given_valid_stage_returns_NoContent()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<StageDTO>())).ReturnsAsync(true);

            var controller = new StageController(repository.Object, log.Object);

            var stage = new StageDTO { Id = 11 };

            // Act
            var result = await controller.Put(11, stage);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given nonexisting stage returns NotFound")]
        public async Task Delete_given_nonexisting_stage_returns_NotFound()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.DeleteAsync(11)).ReturnsAsync(false);

            var controller = new StageController(repository.Object, log.Object);

            // Act
            var result = await controller.Delete(11);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid stage returns NoContent")]
        public async Task Delete_given_valid_stage_returns_NoContent()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            repository.Setup(r => r.DeleteAsync(11)).ReturnsAsync(true);

            var controller = new StageController(repository.Object, log.Object);

            // Act
            var result = await controller.Delete(11);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given id calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            // Arrange
            var repository = new Mock<IStageRepository>();
            var controller = new StageController(repository.Object, log.Object);

            // Act
            await controller.Delete(11);

            // Assert
            repository.Verify(r => r.DeleteAsync(11));
        }

        [Fact(DisplayName = "Dispose calls repository dispose")]
        public void Dispose_calls_repository_dispose()
        {
            var repository = new Mock<IStageRepository>();

            using (var controller = new StageController(repository.Object, log.Object))
            {
            }

            repository.Verify(r => r.Dispose());
        }
    }
}