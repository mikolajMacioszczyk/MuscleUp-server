using Carnets.API;
using Carnets.Application.SpecificPermissions.Dtos;
using Common.Consts;
using System.Net;
using Xunit;

namespace CarnetsTests.IntegrationTests.ControllersTests.ClassPermissionController
{
    public class GetAllClassPermisionsTests : ClassPermissionControllerTestBase
    {
        private const string Endpoint = "/class-permission";

        public GetAllClassPermisionsTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(OwnerToken)]
        [InlineData(WorkerToken)]
        public async Task GetAllPermisions_AsWorkerAndOwner_Ok(string token)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedCount = 2;
            var expectedPermissions = new List<ClassPermissionDto>()
            {
                new ClassPermissionDto()
                {
                    PermissionId = SeedConsts.DefaultClassPermissionId,
                    PermissionName = "Default Class Permission",
                },
                new ClassPermissionDto()
                {
                    PermissionId = SeedConsts.DefaultClassPermissionId2,
                    PermissionName = "Default Class Permission 2",
                },
            };

            //act
            var response = await _client.GetAsync(Endpoint);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);

            var list = await DeserializeMessageContent<List<ClassPermissionDto>>(response);
            Assert.Equal(expectedCount, list.Count);
            Assert.Equal(expectedPermissions.OrderBy(p => p.PermissionId).AsJson(), list.OrderBy(p => p.PermissionId).AsJson());
        }

        [Theory]
        [InlineData(AdminToken)]
        [InlineData(MemberToken)]
        [InlineData(TrainerToken)]
        public async Task GetAllPermisions_AsWorkerAndOwner_Forbidden(string token)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.Forbidden;

            //act
            var response = await _client.GetAsync(Endpoint);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
