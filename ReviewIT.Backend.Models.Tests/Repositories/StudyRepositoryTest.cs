using Microsoft.EntityFrameworkCore;
using Moq;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Entities.Contexts;
using ReviewIT.Backend.Entities.Entities;
using ReviewIT.Backend.Models.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReviewIT.Backend.Models.Tests.Repositories
{
    public class StudyRepositoryTest
    {
        [Fact(DisplayName = "CreateAsync given study adds to database")]
        public async Task CreateAsync_given_study_adds_to_database()
        {
            // Arrange
            var entity = default(Study);
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.Add(It.IsAny<Study>())).Callback<Study>(t => entity = t);

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyNoIdDTO
                {
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                await repository.CreateAsync(study);

                // Assert
                Assert.Equal("StudyTitle", entity.Title);
                Assert.Equal("StudyDescription", entity.Description);
            }
        }

        [Fact(DisplayName = "CreateAsync given study returns id")]
        public async Task CreateAsync_given_study_returns_id()
        {
            // Arrange
            var entity = default(Study);
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.Add(It.IsAny<Study>())).Callback<Study>(t => entity = t);
            context.Setup(c => c.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(0))
                .Callback(() => entity.Id = 11);

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyNoIdDTO
                {
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                var result = await repository.CreateAsync(study);

                // Assert
                Assert.Equal(11, result);
            }
        }

        [Fact(DisplayName = "CreateAsync given study call SaveChangesAsync")]
        public async Task CreateAsync_given_study_calls_SaveChangesAsync()
        {
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.Add(It.IsAny<Study>()));

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyNoIdDTO
                {
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                var result = await repository.CreateAsync(study);

                // Assert
                context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
            }
        }

        [Fact(DisplayName = "FindAsync given id returns study")]
        public async Task FindAsync_given_id_returns_study()
        {
            // Arrange
            // Setup InMemory DB with study
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(FindAsync_given_id_returns_study));

            using (var context = new Context(builder.Options))
            {
                var entity = new Study
                {
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                context.Studies.Add(entity);
                await context.SaveChangesAsync();

                var id = entity.Id;

                using (var repository = new StudyRepository(context))
                {
                    // Act
                    var study = await repository.FindAsync(id);

                    // Assert
                    Assert.Equal("StudyTitle", study.Title);
                    Assert.Equal("StudyDescription", study.Description);
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
            using (var repository = new StudyRepository(context))
            {
                // Act
                var study = await repository.FindAsync(11);

                // Assert
                Assert.Null(study);
            }
        }

        [Fact(DisplayName = "ReadAsync returns list of studies")]
        public async Task ReadAsync_returns_list_of_studies()
        {
            // Arrange
            // Setup InMemory DB with study
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(ReadAsync_returns_list_of_studies));

            using (var context = new Context(builder.Options))
            {
                var entity = new Study
                {
                    Id = 1,
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                context.Studies.Add(entity);
                await context.SaveChangesAsync();

                using (var repository = new StudyRepository(context))
                {
                    // Act
                    var studies = await repository.ReadAsync();
                    var study = studies.Single();

                    // Assert
                    Assert.Equal("StudyTitle", study.Title);
                    Assert.Equal("StudyDescription", study.Description);
                }
            }
        }

        [Fact(DisplayName = "ReadAsync with no studies returns empty list")]
        public async Task ReadAsync_with_no_studies_returns_emptylist()
        {
            // Arrange
            // Setup empty InMemory DB
            var builder = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(nameof(ReadAsync_with_no_studies_returns_emptylist));

            using (var context = new Context(builder.Options))
            using (var repository = new StudyRepository(context))
            {
                // Act
                var study = await repository.ReadAsync();

                // Assert
                Assert.Empty(study);
            }
        }

        [Fact(DisplayName = "UpdateAsync given study updates study")]
        public async Task UpdateAsync_given_study_updates_study()
        {
            // Arrange
            var context = new Mock<IContext>();
            var entity = new Study { Id = 11 };
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyDTO
                {
                    Id = 11,
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                await repository.UpdateAsync(study);
            }

            // Assert
            Assert.Equal("StudyTitle", entity.Title);
            Assert.Equal("StudyDescription", entity.Description);
        }

        [Fact(DisplayName = "UpdateAsync given study calls SaveChangesAsync")]
        public async Task UpdateAsync_given_study_calls_SaveChangesAsync()
        {
            // Arrange
            var context = new Mock<IContext>();
            var entity = new Study { Id = 11 };
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyDTO
                {
                    Id = 11,
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                await repository.UpdateAsync(study);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact(DisplayName = "UpdateAsync given study returns true")]
        public async Task UpdateAsync_given_study_returns_true()
        {
            // Arrange
            var context = new Mock<IContext>();
            var entity = new Study { Id = 11 };
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyDTO
                {
                    Id = 11,
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                var result = await repository.UpdateAsync(study);

                // Assert
                Assert.True(result);
            }
        }

        [Fact(DisplayName = "UpdateAsync given nonexisting study returns false")]
        public async Task UpdateAsync_given_nonexisting_study_returns_false()
        {
            // Arrange 
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(default(Study));

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyDTO
                {
                    Id = 11,
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                var result = await repository.UpdateAsync(study);

                // Assert
                Assert.False(result);
            }
        }

        [Fact(DisplayName = "UpdateAsync given nonexisting study doesnot SaveChangesAsync")]
        public async Task UpdateAsync_given_nonexisting_study_doesnot_SaveChangesAsync()
        {
            // Arrange
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(default(Study));

            using (var repository = new StudyRepository(context.Object))
            {
                var study = new StudyDTO
                {
                    Id = 11,
                    Title = "StudyTitle",
                    Description = "StudyDescription"
                };

                // Act
                await repository.UpdateAsync(study);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact(DisplayName = "DeleteAsync given study removes")]
        public async Task DeleteAsync_given_study_removes()
        {
            // Arrange
            var entity = new Study();
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StudyRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.Studies.Remove(entity));
        }

        [Fact(DisplayName = "DeleteAsync given study calls SaveChangesAsync")]
        public async Task DeleteAsync_given_study_calls_SaveChangesAsync()
        {
            // Arrange
            var entity = new Study();
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(entity);

            using (var repository = new StudyRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact(DisplayName = "DeleteAsync given nonexisting study doesnot remove")]
        public async Task DeleteAsync_given_nonexisting_study_doesnot_remove()
        {
            // Arrange
            var entity = new Study();
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(default(Study));

            using (var repository = new StudyRepository(context.Object))
            {
                // Act
                await repository.DeleteAsync(11);
            }

            // Assert
            context.Verify(c => c.Studies.Remove(entity), Times.Never);
        }

        [Fact(DisplayName = "DeleteAsync given nonexisting study doesnot call SaveChangesAsync")]
        public async Task DeleteAsync_given_nonexisting_study_doesnot_call_SaveChangesAsync()
        {
            // Arrange
            var entity = new Study();
            var context = new Mock<IContext>();
            context.Setup(c => c.Studies.FindAsync(11)).ReturnsAsync(default(Study));

            using (var repository = new StudyRepository(context.Object))
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

            using (var repository = new StudyRepository(context.Object))
            {
            }

            context.Verify(c => c.Dispose());
        }
    }
}
