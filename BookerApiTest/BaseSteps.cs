using Allure.Net.Commons;
using Reqnroll;
using RestSharp;

namespace BookerApiTest
{
    public class BaseSteps
    {
        protected readonly ScenarioContext _scenarioContext;
        protected RestClient _client;

        public BaseSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _client = new RestClient("https://restful-booker.herokuapp.com");
        }

        protected void ExecuteRequest(RestRequest request)
        {
            //// Add Allure logging
            //AllureApi.AddAttachment("Request",
            //    $"{request.Method} {request.Resource}",
            //    "text/plain");

            var response = _client.Execute(request);
            _scenarioContext.Set(response, "Response");

            //// Log response to Allure
            //AllureApi.AddAttachment("Response",
            //    $"Status: {response.StatusCode}\nContent: {response.Content}",
            //    "text/plain");
        }

        // Add this method to track HTTP details
        protected void LogRequestResponse(RestRequest request, RestResponse response)
        {
            AllureApi.AddAttachment("request.txt",
                $"Method: {request.Method}\nResource: {request.Resource}",
                "text/plain");

            AllureApi.AddAttachment("response.txt",
                $"Status: {response.StatusCode}\nContent: {response.Content}",
                "text/plain");
        }
    }
}
