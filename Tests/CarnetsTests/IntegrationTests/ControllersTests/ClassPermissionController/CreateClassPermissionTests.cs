using Carnets.API;
using Carnets.Application.SpecificPermissions.Dtos;
using Carnets.Domain.Enums;
using System.Net;
using Xunit;

namespace CarnetsTests.IntegrationTests.ControllersTests.ClassPermissionController
{
    public class CreateClassPermissionTests : ClassPermissionControllerTestBase
    {
        private const string Endpoint = "/class-permission";

        public CreateClassPermissionTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(OwnerToken)]
        [InlineData(WorkerToken)]
        public async Task CreateClassPermission_Valid_AsOwnerAndWorker_Ok(string token)
        {
            //arrange
            AddBearerToken(token);
            var permissionName = "Test permission name";
            var expectedPermissionType = PermissionType.ClassPermission.ToString();
            var expectedStatusCode = HttpStatusCode.OK;
            var payload = PayloadBuilder.GetPayload(new CreateClassPermissionDto()
            {
                PermissionName = permissionName
            });

            //act
            var response = await _client.PostAsync(Endpoint, payload);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);

            var created = await DeserializeMessageContent<ClassPermissionDto>(response);
            Assert.NotNull(created);
            Assert.Equal(expectedPermissionType, created.PermissionType);
            Assert.Equal(permissionName, created.PermissionName);
        }

        [Theory]
        [InlineData(OwnerToken)]
        [InlineData(WorkerToken)]
        public async Task CreateClassPermission_MissingPermissionName_AsOwnerAndWorker_BadRequest(string token)
        {
            //arrange
            AddBearerToken(token);
            var expectedError = "The PermissionName field is required";
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var payload = PayloadBuilder.GetPayload(new CreateClassPermissionDto()
            {
                PermissionName = null
            });

            //act
            var response = await _client.PostAsync(Endpoint, payload);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(expectedError, content);
        }

        [Theory]
        [InlineData(MemberToken)]
        [InlineData(TrainerToken)]
        [InlineData(AdminToken)]
        public async Task CreateClassPermission_AsOther_Forbidden(string token)
        {
            //arrange
            AddBearerToken(token);
            var expectedStatusCode = HttpStatusCode.Forbidden;
            var permissionName = "Test permission name";
            var payload = PayloadBuilder.GetPayload(new CreateClassPermissionDto()
            {
                PermissionName = permissionName
            });

            //act
            var response = await _client.PostAsync(Endpoint, payload);

            //assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
