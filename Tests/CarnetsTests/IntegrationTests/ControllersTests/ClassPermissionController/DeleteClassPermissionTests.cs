using Carnets.API;
using Carnets.Application.SpecificPermissions.Dtos;
using Common.Consts;
using System.Net;
using Xunit;

namespace CarnetsTests.IntegrationTests.ControllersTests.ClassPermissionController
{
    public class DeleteClassPermissionTests : ClassPermissionControllerTestBase
    {
        private const string Endpoint = "/class-permission/";

        public DeleteClassPermissionTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        { }

        [Theory]
        [InlineData(OwnerToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(WorkerToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(OwnerToken, SeedConsts.DefaultClassPermissionId2)]
        [InlineData(WorkerToken, SeedConsts.DefaultClassPermissionId2)]
        public async Task DeleteClassPermission_ExistingPermission_AsWorkerAndOwner_Ok(string token, string permissionId)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.OK;

            //act
            var response = await _client.DeleteAsync(Endpoint + permissionId);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData(OwnerToken)]
        [InlineData(WorkerToken)]
        public async Task DeleteClassPermission_MissingPermission_AsWorkerAndOwner_NotFound(string token)
        {
            //arrange
            AddBearerToken(token);
            string permissionId = "Not exisiting id";
            var expectedStatusCode = HttpStatusCode.NotFound;

            //act
            var response = await _client.DeleteAsync(Endpoint + permissionId);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        // admin
        [Theory]
        [InlineData(MemberToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(TrainerToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(AdminToken, SeedConsts.DefaultClassPermissionId)]
        [InlineData(MemberToken, "Not exisiting id")]
        [InlineData(TrainerToken, "Not exisiting id")]
        [InlineData(AdminToken, "Not exisiting id")]
        public async Task DeleteClassPermission_AsOtherUser_Forbiddent(string token, string permissionId)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.Forbidden;

            //act
            var response = await _client.DeleteAsync(Endpoint + permissionId);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
