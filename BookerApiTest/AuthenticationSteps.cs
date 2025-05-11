using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using RestSharp;

namespace BookerApiTest.StepDefinitions
{
    [Binding]
    public class AuthenticationSteps : BaseSteps
    {
        public AuthenticationSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [Given(@"I have a valid authentication token")]
        public void GivenIHaveAValidAuthenticationToken()
        {
            var request = new RestRequest("auth", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                username = "admin",
                password = "password123"
            });

            var response = _client.Execute(request);
            Assert.That(response.IsSuccessful, Is.True, $"Auth failed: {response.StatusCode}");

            var token = JObject.Parse(response.Content!)["token"]?.ToString();
            Assert.That(token, Is.Not.Null.And.Not.Empty, "Token was not received");

            _scenarioContext.Set(token!, "AuthToken");
        }

        [Given(@"I have an invalid authentication token")]
        public void GivenIHaveAnInvalidAuthenticationToken()
        {
            _scenarioContext.Set("invalid_token_123", "AuthToken");
        }
    }
}