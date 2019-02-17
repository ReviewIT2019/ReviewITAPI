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
    public class StudyControllerTests
    {
        private readonly Mock<ILogger<StudyController>> log = new Mock<ILogger<StudyController>>();

        [Fact(DisplayName = "Get returns Ok with studies")]
        public async Task Get_returns_Ok_with_studies()
        {
            // Arrange
            var studies = new StudyDTO[0];

            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(studies);

            var controller = new StudyController(repository.Object, log.Object);

            // Act
            var result = await controller.Get() as OkObjectResult;

            // Assert
            Assert.Equal(studies, result.Value);
        }

        [Fact(DisplayName = "Get given id returns Ok with study")]
        public async Task Get_given_id_returns_Ok_with_study()
        {
            // Arrange
            var study = new StudyDTO();

            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.FindAsync(11)).ReturnsAsync(study);

            var controller = new StudyController(repository.Object, log.Object);

            // Act
            var result = await controller.Get(11) as OkObjectResult;

            // Assert
            Assert.Equal(study, result.Value);
        }

        [Fact(DisplayName = "Get given nonexisting id returns NotFound")]
        public async Task Get_given_nonexisting_id_returns_NotFound()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.FindAsync(11)).ReturnsAsync(default(StudyDTO));

            var controller = new StudyController(repository.Object, log.Object);

            // Act
            var result = await controller.Get(11);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid study returns BadRequest")]
        public async Task Post_given_invalid_study_returns_BadRequest()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();

            var controller = new StudyController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var study = new StudyNoIdDTO();

            // Act
            var result = await controller.Post(study);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Post given invalid study doesnot call CreateAsync")]
        public async Task Post_given_invalid_study_doesnot_call_CreateAsync()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();

            var controller = new StudyController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var study = new StudyNoIdDTO();

            // Act
            await controller.Post(study);

            // Assert
            repository.Verify(r => r.CreateAsync(It.IsAny<StudyNoIdDTO>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid study calls CreateAsync")]
        public async Task Post_given_valid_study_calls_CreateAsync()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            var controller = new StudyController(repository.Object, log.Object);

            var study = new StudyNoIdDTO();

            // Act
            await controller.Post(study);

            // Assert
            repository.Verify(r => r.CreateAsync(study));
        }

        [Fact(DisplayName = "Post given valid study returns CreatedAtAction")]
        public async Task Post_given_valid_study_returns_CreatedAtAction()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<StudyNoIdDTO>())).ReturnsAsync(11);

            var controller = new StudyController(repository.Object, log.Object);

            var study = new StudyNoIdDTO();

            // Act
            var result = await controller.Post(study) as CreatedAtActionResult;

            // Assert
            Assert.Equal(nameof(StudyController.Get), result.ActionName);
            Assert.Equal(11, result.RouteValues["id"]);
        }

        [Fact(DisplayName = "Put given invalid study returns BadRequest")]
        public async Task Put_given_invalid_study_returns_BadRequest()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();

            var controller = new StudyController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var study = new StudyDTO { Id = 11 };

            // Act
            var result = await controller.Put(11, study);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Put given id not eq studyId returns BadRequest")]
        public async Task Put_given_id_not_eq_to_studyId_returns_BadRequest()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            var controller = new StudyController(repository.Object, log.Object);

            var customer = new StudyDTO { Id = 11 };

            // Act
            var result = await controller.Put(0, customer);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Put given invalid study doesnot call UpdateAsync")]
        public async Task Put_given_invalid_study_doesnot_call_UpdateAsync()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();

            var controller = new StudyController(repository.Object, log.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var study = new StudyDTO();

            // Act
            await controller.Put(11, study);

            // Assert
            repository.Verify(r => r.UpdateAsync(It.IsAny<StudyDTO>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid study calls UpdateAsync")]
        public async Task Put_given_valid_study_calls_UpdateAsync()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            var controller = new StudyController(repository.Object, log.Object);

            var study = new StudyDTO { Id = 11 };

            // Act
            await controller.Put(11, study);

            // Arrange
            repository.Verify(r => r.UpdateAsync(study));
        }

        [Fact(DisplayName = "Put given nonexisting study returns NotFound")]
        public async Task Put_given_nonexisting_study_returns_NotFound()
        {
            // Act
            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<StudyDTO>())).ReturnsAsync(false);

            var controller = new StudyController(repository.Object, log.Object);

            var study = new StudyDTO { Id = 11 };

            // Act
            var result = await controller.Put(11, study);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid study returns NoContent")]
        public async Task Put_given_valid_study_returns_NoContent()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<StudyDTO>())).ReturnsAsync(true);

            var controller = new StudyController(repository.Object, log.Object);

            var study = new StudyDTO { Id = 11 };

            // Act
            var result = await controller.Put(11, study);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given nonexisting study returns NotFound")]
        public async Task Delete_given_nonexisting_study_returns_NotFound()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.DeleteAsync(11)).ReturnsAsync(false);

            var controller = new StudyController(repository.Object, log.Object);

            // Act
            var result = await controller.Delete(11);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid study returns NoContent")]
        public async Task Delete_given_valid_study_returns_NoContent()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            repository.Setup(r => r.DeleteAsync(11)).ReturnsAsync(true);

            var controller = new StudyController(repository.Object, log.Object);

            // Act
            var result = await controller.Delete(11);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given id calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            // Arrange
            var repository = new Mock<IStudyRepository>();
            var controller = new StudyController(repository.Object, log.Object);

            // Act
            await controller.Delete(11);

            // Assert
            repository.Verify(r => r.DeleteAsync(11));
        }

        [Fact(DisplayName = "Dispose calls repository dispose")]
        public void Dispose_calls_repository_dispose()
        {
            var repository = new Mock<IStudyRepository>();

            using (var controller = new StudyController(repository.Object, log.Object))
            {
                // Ignore warning, otherwise dispose wont be called
            }

            repository.Verify(r => r.Dispose());
        }
    }
}
