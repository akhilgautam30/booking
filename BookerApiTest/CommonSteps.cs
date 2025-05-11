using Reqnroll;
using RestSharp;

namespace BookerApiTest
{
    [Binding]
    public class CommonSteps : BaseSteps
    {
        public CommonSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode),
                $"Expected status code {expectedStatusCode} but got {(int)response.StatusCode}");
        }
    }
}
