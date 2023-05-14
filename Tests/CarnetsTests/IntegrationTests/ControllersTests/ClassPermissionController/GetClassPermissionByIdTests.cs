using Carnets.API;
using Carnets.Application.SpecificPermissions.Dtos;
using Common.Consts;
using System.Net;
using Xunit;

namespace CarnetsTests.IntegrationTests.ControllersTests.ClassPermissionController
{
    public class GetClassPermissionByIdTests : ClassPermissionControllerTestBase
    {
        private const string Endpoint = "/class-permission/";

        public GetClassPermissionByIdTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(OwnerToken, SeedConsts.DefaultClassPermissionId, "Default Class Permission")]
        [InlineData(WorkerToken, SeedConsts.DefaultClassPermissionId, "Default Class Permission")]
        [InlineData(AdminToken, SeedConsts.DefaultClassPermissionId, "Default Class Permission")]
        [InlineData(OwnerToken, SeedConsts.DefaultClassPermissionId2, "Default Class Permission 2")]
        [InlineData(WorkerToken, SeedConsts.DefaultClassPermissionId2, "Default Class Permission 2")]
        [InlineData(AdminToken, SeedConsts.DefaultClassPermissionId2, "Default Class Permission 2")]
        public async Task GetClassPermissionById_ExistingPermission_AsWorkerOwnerAndAdmin_Ok(string token, string permissionId, string permissionName)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedPermission = new ClassPermissionDto()
            {
                PermissionId = permissionId,
                PermissionName = permissionName,
            };

            //act
            var response = await _client.GetAsync(Endpoint + permissionId);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);

            var resultPermission = await DeserializeMessageContent<ClassPermissionDto>(response);
            Assert.Equal(expectedPermission.AsJson(), resultPermission.AsJson());
        }

        [Theory]
        [InlineData(OwnerToken)]
        [InlineData(WorkerToken)]
        [InlineData(AdminToken)]
        public async Task GetClassPermissionById_MissingPermission_AsWorkerOwnerAndAdmin_NotFound(string token)
        {
            //arrange
            AddBearerToken(token);
            string permissionId = "Not exisiting id";
            var expectedStatusCode = HttpStatusCode.NotFound;
            
            //act
            var response = await _client.GetAsync(Endpoint + permissionId);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData(MemberToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(TrainerToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(MemberToken, "Not exisiting id")]
        [InlineData(TrainerToken, "Not exisiting id")]
        public async Task GetClassPermissionById_AsOtherUser_Forbiddent(string token, string permissionId)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.Forbidden;

            //act
            var response = await _client.GetAsync(Endpoint + permissionId);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
