using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Domain.Filters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Inventory.Configuration.UnitTests.SeedWork;

namespace Inventory.Configuration.UnitTests.InfrastructureTests
{
    [TestFixture]
    class CredentialInfrastructureTests : DbUnitTests
    {
        [Test]
        public async Task Should_successfull_query_CredentialEntity_with_AsyncRepository()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetAsyncRepository<Credential>();

            // Act
            var credentials = await repo.ListAllAsync();

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsNotEmpty(credentials);

        }

        [Test]
        public async Task Should_successfull_query_CredentialEntity_with_GenericQueryStore()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();

            // Act
            var credentials = await repo.ListAllAsync();

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsNotEmpty(credentials);

        }

        [Test]
        public async Task Should_successfull_query_CredentialDto_with_GenericQueryStore()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();

            // Act
            var credentials = await repo.ListAllAsync<CredentialDto>();

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsNotEmpty(credentials);

        }

        [Test]
        public async Task Should_successfull_query_CredentialDto_with_GenericQueryStore_WithFilter()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();
            var filter = ExpressionFilterFactory.Create<Credential>().WithName(CredentialSeed.ADMINISTRATOR);

            // Act
            var credentials = await repo.GetByCriteriaAsync<CredentialDto>(filter, CredentialDto.Projection);

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsNotEmpty(credentials);

        }

        [Test]
        public async Task Should_successfull_query_Credential_with_GenericQueryStore_WithFilter()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();
            var filter = ExpressionFilterFactory.Create<Credential>().WithName(CredentialSeed.ADMINISTRATOR);

            // Act
            var credentials = await repo.GetByCriteriaAsync(filter);

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsNotEmpty(credentials);

        }

        [Test]
        public async Task Should_successfull_query_CredentialDto_with_GenericQueryStore_WithFilter_And_OrderBy()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();
            var filter = ExpressionFilterFactory.Create<Credential>().WithName(CredentialSeed.ADMINISTRATOR);

            // Act
            var credentials = await repo.GetByCriteriaAsync<CredentialDto>(filter, CredentialDto.Projection, orderBy: q => q.OrderBy(e => e.Id));

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsNotEmpty(credentials);

        }

        [Test]
        public async Task Should_Count_Credentias_WithFilter()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();
            var filter = ExpressionFilterFactory.Create<Credential>().WithName(CredentialSeed.ADMINISTRATOR);

            // Act
            var count = await repo.CountAsync(filter);

            // Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count > 0);

        }

        [Test]
        public async Task Should_Verify_Credential_Exists()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetGenericStore<Credential>();
            var filter = ExpressionFilterFactory.Create<Credential>().WithName(CredentialSeed.ADMINISTRATOR);

            // Act
            var exists = await repo.AnyAsync(filter);

            // Assert
            Assert.IsNotNull(exists);
            Assert.IsTrue(exists);

        }

    }
}
