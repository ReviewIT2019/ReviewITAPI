using Microsoft.EntityFrameworkCore;
using Moq;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Entities.Contexts;
using ReviewIT.Backend.Entities.Entities;
using ReviewIT.Backend.Entities.Entitities;
using ReviewIT.Backend.Models.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReviewIT.Backend.Models.Tests.Repositories
{
    public class StageRepositoryTest
    {
        [Fact(DisplayName = "CreateAsync given stage adds to database")]
        public async Task CreateAsync_given_stage_adds_to_database()
        {
            // Arrange
            var entity = default(Stage);
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.Add(It.IsAny<Stage>())).Callback<Stage>(t => entity = t);

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageNoIdDTO
                {
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                await repository.CreateAsync(stage);

                // Assert
                Assert.Equal("StageName", entity.Name);
                Assert.Equal("StageDescription", entity.Description);
                Assert.False(entity.StageInitiated);
                Assert.Equal(1, entity.StudyId);
            }
        }

        [Fact(DisplayName = "CreateAsync given stage returns id")]
        public async Task CreateAsync_given_stage_returns_id()
        {
            // Arrange
            var entity = default(Stage);
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.Add(It.IsAny<Stage>())).Callback<Stage>(t => entity = t);
            context.Setup(c => c.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(0))
                .Callback(() => entity.Id = 11);

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageNoIdDTO
                {
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                var result = await repository.CreateAsync(stage);

                // Assert
                Assert.Equal(11, result);
            }
        }

        [Fact(DisplayName = "CreateAsync given stage call SaveChangesAsync")]
        public async Task CreateAsync_given_stage_calls_SaveChangesAsync()
        {
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.Add(It.IsAny<Stage>()));

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageNoIdDTO
                {
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                var result = await repository.CreateAsync(stage);

                // Assert
                context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
            }
        }

        [Fact(DisplayName = "FindAsync given id returns stage")]
        public async Task FindAsync_given_id_returns_stage()
        {
            // Arrange
            // Setup InMemory DB with study
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(FindAsync_given_id_returns_stage));

            using (var context = new Context(builder.Options))
            {
                var entity = new Stage
                {
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                context.Stages.Add(entity);
                await context.SaveChangesAsync();

                var id = entity.Id;

                using (var repository = new StageRepository(context))
                {
                    // Act
                    var stage = await repository.FindAsync(id);

                    // Assert
                    Assert.Equal("StageName", stage.Name);
                    Assert.Equal("StageDescription", stage.Description);
                    Assert.False(stage.StageInitiated);
                    Assert.Equal(1, stage.StudyId);
                }
            }
        }

        [Fact(DisplayName = "FindAsync given nonexisting id returns null")]
        public async Task FindAsync_given_nonexisting_id_returns_null()
        {
            // Arrange
            // Setup empty InMemory DB
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(FindAsync_given_nonexisting_id_returns_null));

            using (var context = new Context(builder.Options))
            using (var repository = new StageRepository(context))
            {
                // Act
                var stage = await repository.FindAsync(11);

                // Assert
                Assert.Null(stage);
            }
        }

        [Fact(DisplayName = "ReadAsync returns list of stages")]
        public async Task ReadAsync_returns_list_of_stages()
        {
            // Arrange
            // Setup InMemory DB with study
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(ReadAsync_returns_list_of_stages));

            using (var context = new Context(builder.Options))
            {
                var entity = new Stage
                {
                    Id = 1,
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                context.Stages.Add(entity);
                await context.SaveChangesAsync();

                using (var repository = new StageRepository(context))
                {
                    // Act
                    var stages = await repository.ReadAsync();
                    var stage = stages.Single();

                    // Assert
                    Assert.Equal("StageName", stage.Name);
                    Assert.Equal("StageDescription", stage.Description);
                    Assert.False(stage.StageInitiated);
                    Assert.Equal(1, stage.StudyId);
                }
            }
        }

        [Fact(DisplayName = "ReadAsync with no stages returns empty list")]
        public async Task ReadAsync_with_no_stages_returns_emptylist()
        {
            // Arrange
            // Setup empty InMemory DB
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(ReadAsync_with_no_stages_returns_emptylist));

            using (var context = new Context(builder.Options))
            using (var repository = new StageRepository(context))
            {
                // Act
                var stages = await repository.ReadAsync();

                // Assert
                Assert.Empty(stages);
            }
        }

        [Fact(DisplayName = "UpdateAsync given stage updates stage")]
        public async Task UpdateAsync_given_stage_updates_stage()
        {
            // Arrange
            var context = new Mock<IContext>();
            var entity = new Stage { Id = 11 };
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageDTO
                {
                    Id = 11,
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                await repository.UpdateAsync(stage);
            }

            // Assert
            Assert.Equal("StageName", entity.Name);
            Assert.Equal("StageDescription", entity.Description);
            Assert.False(entity.StageInitiated);
            Assert.Equal(1, entity.StudyId);
        }

        [Fact(DisplayName = "UpdateAsync given stage calls SaveChangesAsync")]
        public async Task UpdateAsync_given_stage_calls_SaveChangesAsync()
        {
            // Arrange
            var context = new Mock<IContext>();
            var entity = new Stage { Id = 11 };
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageDTO
                {
                    Id = 11,
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                await repository.UpdateAsync(stage);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact(DisplayName = "UpdateAsync given stage returns true")]
        public async Task UpdateAsync_given_stage_returns_true()
        {
            // Arrange
            var context = new Mock<IContext>();
            var entity = new Stage { Id = 11 };
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageDTO
                {
                    Id = 11,
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                var result = await repository.UpdateAsync(stage);

                // Assert
                Assert.True(result);
            }
        }

        [Fact(DisplayName = "UpdateAsync given nonexisting stage returns false")]
        public async Task UpdateAsync_given_nonexisting_stage_returns_false()
        {
            // Arrange 
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(default(Stage));

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageDTO
                {
                    Id = 11,
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                var result = await repository.UpdateAsync(stage);

                // Assert
                Assert.False(result);
            }
        }

        [Fact(DisplayName = "UpdateAsync given nonexisting stage doesnot SaveChangesAsync")]
        public async Task UpdateAsync_given_nonexisting_stage_doesnot_SaveChangesAsync()
        {
            // Arrange
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(default(Stage));

            using (var repository = new StageRepository(context.Object))
            {
                var stage = new StageDTO
                {
                    Id = 11,
                    Name = "StageName",
                    Description = "StageDescription",
                    StageInitiated = false,
                    StudyId = 1
                };

                // Act
                await repository.UpdateAsync(stage);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact(DisplayName = "DeleteAsync given stage removes")]
        public async Task DeleteAsync_given_stage_removes()
        {
            // Arrange
            var entity = new Stage();
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StageRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.Stages.Remove(entity));
        }

        [Fact(DisplayName = "DeleteAsync given stage calls SaveChangesAsync")]
        public async Task DeleteAsync_given_stage_calls_SaveChangesAsync()
        {
            // Arrange
            var entity = new Stage();
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StageRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact(DisplayName = "DeleteAsync given nonexisting stage doesnot remove")]
        public async Task DeleteAsync_given_nonexisting_stage_doesnot_remove()
        {
            // Arrange
            var entity = new Stage();
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(default(Stage));

            using (var repository = new StageRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.Stages.Remove(entity), Times.Never);
        }

        [Fact(DisplayName = "DeleteAsync given nonexisting stage doesnot call SaveChangesAsync")]
        public async Task DeleteAsync_given_nonexisting_stage_doesnot_call_SaveChangesAsync()
        {
            // Arrange
            var context = new Mock<IContext>();
            context.Setup(c => c.Stages.FindAsync(11)).ReturnsAsync(default(Stage));

            using (var repository = new StageRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact(DisplayName = "Dispose disposes context")]
        public void Dispose_disposes_context()
        {
            var context = new Mock<IContext>();

            using (var repository = new StageRepository(context.Object))
            {
            }

            context.Verify(c => c.Dispose());
        }
    }
}
