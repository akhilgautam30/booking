using Newtonsoft.Json;
using Reqnroll;
using RestSharp;

namespace BookerApiTest.StepDefinitions
{
    [Binding]
    public class GetBookingIdsSteps : BaseSteps
    {
        public GetBookingIdsSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [When(@"I request all booking IDs")]
        public void WhenIRequestAllBookingIDs()
        {
            var request = new RestRequest("booking", Method.Get);
            ExecuteRequest(request);
        }

        [When(@"I request booking IDs with parameters")]
        public void WhenIRequestBookingIDsWithParameters(Table table)
        {
            var request = new RestRequest("booking", Method.Get);

            foreach (var row in table.Rows)
            {
                if (!string.IsNullOrEmpty(row["Value"]))
                {
                    request.AddQueryParameter(row["Parameter"], row["Value"]);
                }
            }

            ExecuteRequest(request);
        }

        [Then("the response should contain booking IDs matching the criteria")]
        public void ThenTheResponseShouldContainBookingIDsMatchingTheCriteria()
        {
            var response = _scenarioContext["Response"] as RestResponse;
            if (response == null)
            {
                throw new InvalidOperationException("Response is not available in the scenario context.");
            }

            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Expected status code 200 but got a different status code.");

            var bookingIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            Assert.That(bookingIds, Is.Not.Null, "Response content is null or could not be deserialized.");
            Assert.That(bookingIds.Count, Is.GreaterThan(0), "No booking IDs were returned in the response.");
        }

        [Then("the response should not contain booking IDs")]
        public void ThenTheResponseShouldNotContainBookingIDs()
        {
            var response = _scenarioContext["Response"] as RestResponse;
            if (response == null)
            {
                throw new InvalidOperationException("Response is not available in the scenario context.");
            }

            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Expected status code 200 but got a different status code.");

            var bookingIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            Assert.That(bookingIds == null || bookingIds.Count == 0, Is.True, "Booking IDs were returned when none were expected.");
        }

        [Then("the response should contain booking IDs")]
        public void ThenTheResponseShouldContainBookingIDs()
        {
            var response = _scenarioContext["Response"] as RestResponse;
            if (response == null)
            {
                throw new InvalidOperationException("Response is not available in the scenario context.");
            }

            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Expected status code 200 but got a different status code.");

            var bookingIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            Assert.That(bookingIds, Is.Not.Null, "Response content is null or could not be deserialized.");
            Assert.That(bookingIds.Count, Is.GreaterThan(0), "No booking IDs were returned in the response.");
        }
    }
}
