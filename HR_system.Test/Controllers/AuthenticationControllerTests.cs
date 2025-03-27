using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace HR_system.Test.Integration
{
    public class AuthenticationControllerTests :
        IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AuthenticationControllerTests(
            CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }


        [Theory]
        [InlineData("/Authentication/Login")]
        [InlineData("/Authentication/Register")]
        public async Task Get_UnautorizedEnfpointsContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Register_ValidData_ShouldRedirectToSuccessPage()
        {
            // Arrange: Get the registration page to extract anti-forgery token
            var getRegisterResponse = await _client.GetAsync("/Authentication/Register");
            getRegisterResponse.EnsureSuccessStatusCode();
            var responseContent = await getRegisterResponse.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(responseContent);
            var tokenElement = document.QuerySelector("input[name=__RequestVerificationToken]");
            var token = tokenElement?.GetAttribute("value");

            Assert.NotNull(token);

            // Act: Post registration form with valid user data
            var formData = new Dictionary<string, string>
        {
            { "__RequestVerificationToken", token! },
            { "Email", "testuser@example.com" },
            { "FirstName", "Something"},
            { "LastName", "Something"},
            { "JobTitle", "Something"},
            { "Department", "Something"},
            { "Salary", "1000"},
            { "Password", "Test@1234" },
            { "ConfirmPassword", "Test@1234" }
        };

            var content = new FormUrlEncodedContent(formData);
            var postResponse = await _client.PostAsync("/Authentication/Register", content);

            // Assert: Expecting a redirect response
            Assert.Equal(HttpStatusCode.Found, postResponse.StatusCode);
            Assert.Equal("/", postResponse.Headers.Location.ToString());
        }
    }

}
