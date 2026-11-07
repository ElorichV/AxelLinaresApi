using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using AxelLinaresApi.Controllers;
using AxelLinaresApi.Data;
using AxelLinaresApi.Models;
using AxelLinaresApi.Tests.Fixtures;

namespace AxelLinaresApi.Tests
{
    public class PortfolioApiTests
    {
        private static readonly string[] ForbiddenInternalRepositories =
        {
            "G10hdz/aldava",
            "G10hdz/tallygraph",
            "G10hdz/agent-configs",
            "G10hdz/pop-loops",
            "jeanethS/BrandOss",
            "Migokc/BrandOss",
            "jeanethS/mi-primera-chamba",
            "jeanethS/sas-mpc",
            "G10hdz/ai-coach",
            "G10hdz/Ergane-per",
            "G10hdz/Ergane",
            "G10hdz/Echo"
        };

        [Fact]
        public void DataSeeder_Initialize_SeedsCompleteProfileWithNineCategorizedProjectsAndThreeExperiences()
        {
            // Arrange
            using var fixture = new SqliteInMemoryDbFixture();

            // Act
            DataSeeder.Initialize(fixture.Context);
            var profile = fixture.Context.UserProfiles
                .Include(u => u.Experiences)
                .Include(u => u.Projects)
                .Include(u => u.Certifications)
                .Single();

            // Assert
            Assert.Equal("AXEL ALBERTO LINARES LORRABAQUIO", profile.Name);
            Assert.Equal(3, profile.Experiences.Count);
            Assert.Equal(9, profile.Projects.Count);
            Assert.Equal(4, profile.Certifications.Count);
            Assert.All(profile.Projects, p =>
            {
                Assert.False(string.IsNullOrWhiteSpace(p.Category));
                Assert.False(string.IsNullOrWhiteSpace(p.Badge));
                Assert.NotEmpty(p.TechStack);
                Assert.Equal(3, p.ArchitectureNotes.Count);
            });
        }

        [Fact]
        public void DataSeeder_Initialize_IsIdempotentAndDoesNotDuplicateRecordsOnSecondRun()
        {
            // Arrange
            using var fixture = new SqliteInMemoryDbFixture();

            // Act
            DataSeeder.Initialize(fixture.Context);
            DataSeeder.Initialize(fixture.Context);

            // Assert
            Assert.Equal(1, fixture.Context.UserProfiles.Count());
            Assert.Equal(3, fixture.Context.Experiences.Count());
            Assert.Equal(9, fixture.Context.Projects.Count());
            Assert.Equal(4, fixture.Context.Certifications.Count());
        }

        [Fact]
        public void DataSeeder_ConfidentialityInvariant_NeverExposesInternalPositronicaReposOrDirectChambaLinks()
        {
            // Arrange
            using var fixture = new SqliteInMemoryDbFixture();
            DataSeeder.Initialize(fixture.Context);

            // Act
            var projects = fixture.Context.Projects.AsNoTracking().ToList();
            var experiences = fixture.Context.Experiences.AsNoTracking().ToList();
            var chambaProject = projects.Single(p => p.Title.Contains("Mi Primera Chamba"));
            var positronicaExp = experiences.Single(e => e.Company.Contains("Positronica"));

            // Assert
            Assert.Null(chambaProject.ProjectUrl);
            Assert.Equal("https://positronicalabs.dev", positronicaExp.CompanyUrl);

            foreach (var forbidden in ForbiddenInternalRepositories)
            {
                Assert.DoesNotContain(projects, p =>
                    (p.ProjectUrl != null && p.ProjectUrl.Contains(forbidden, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Description != null && p.Description.Contains(forbidden, StringComparison.OrdinalIgnoreCase)));
            }
        }

        [Fact]
        public void ProfileDbContext_PipeValueConverter_PreservesSkillsContainingCommasInsideParentheses()
        {
            // Arrange
            using var fixture = new SqliteInMemoryDbFixture();
            DataSeeder.Initialize(fixture.Context);

            // Act
            var persistedProfile = fixture.Context.UserProfiles.AsNoTracking().Single();

            // Assert
            Assert.Contains("AWS (S3, CloudFront)", persistedProfile.HardSkills);
            Assert.Contains("JetBrains (Rider, DataGrip, PyCharm)", persistedProfile.HardSkills);
            Assert.Contains("Orquestación Multi-Agente (n8n.cloud, Firecrawl)", persistedProfile.HardSkills);
        }

        [Fact]
        public async Task GetProfile_WhenDatabaseIsEmpty_ReturnsRfc7807NotFoundProblemDetails()
        {
            // Arrange
            using var fixture = new SqliteInMemoryDbFixture();
            var controller = new ProfileController(fixture.Context);

            // Act
            var actionResult = await controller.GetProfile("es");

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.5.4", problem.Type);
        }

        [Fact]
        public async Task GetProfile_WhenLanguageIsEnglish_TranslatesAllNineProjectsThreeExperiencesAndSoftSkills()
        {
            // Arrange
            using var fixture = new SqliteInMemoryDbFixture();
            DataSeeder.Initialize(fixture.Context);
            var controller = new ProfileController(fixture.Context);

            // Act
            var esResult = Assert.IsType<OkObjectResult>(await controller.GetProfile("es"));
            var esSnapshotDescriptions = ((UserProfile)esResult.Value!).Projects.Select(p => p.Description).ToList();

            var enResult = Assert.IsType<OkObjectResult>(await controller.GetProfile("en"));
            var enProfile = Assert.IsType<UserProfile>(enResult.Value);

            // Assert
            Assert.Equal(9, enProfile.Projects.Count);
            Assert.Equal(3, enProfile.Experiences.Count);
            Assert.Contains("Technical Leadership", enProfile.SoftSkills);

            // Contract Exhaustiveness: Every single project description must be translated from its Spanish seed
            for (int i = 0; i < enProfile.Projects.Count; i++)
            {
                Assert.NotEqual(esSnapshotDescriptions[i], enProfile.Projects[i].Description);
            }

            var chambaEn = enProfile.Projects.Single(p => p.Title.Contains("Mi Primera Chamba"));
            Assert.Null(chambaEn.ProjectUrl);
        }

        [Theory]
        [InlineData("Axel\r\nBcc:victim@evil.com", "axel@example.com", "Mensaje válido de más de diez caracteres.")]
        [InlineData("Axel Linares", "axel\r\n@example.com", "Mensaje válido de más de diez caracteres.")]
        [InlineData("Axel Linares", "correo-invalido", "Mensaje válido de más de diez caracteres.")]
        [InlineData("Axel Linares", "axel@example.com", "            ")]
        public void ContactRequest_Validation_RejectsCrlfHeaderInjectionInvalidEmailAndWhitespaceMessages(
            string name, string email, string message)
        {
            // Arrange
            var request = new ContactRequest
            {
                Name = name,
                Email = email,
                Message = message
            };
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(request, context, results, validateAllProperties: true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public async Task SendEmailAsync_WhenSmtpCredentialsMissing_ReturnsRfc7807ServiceUnavailableWithoutNetworkCall()
        {
            // Arrange
            var emptyConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>())
                .Build();
            var controller = new EmailController(emptyConfig, NullLogger<EmailController>.Instance);
            var validRequest = new ContactRequest
            {
                Name = "Axel Linares",
                Email = "axel.a.linares@gmail.com",
                Message = "Hola Axel, nos gustaría agendar una entrevista técnica para el puesto Mid-Level."
            };

            // Act
            var result = await controller.SendEmailAsync(validRequest, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, objectResult.StatusCode);
            var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("https://tools.ietf.org/html/rfc7231#section-6.6.4", problem.Type);
        }
    }
}
