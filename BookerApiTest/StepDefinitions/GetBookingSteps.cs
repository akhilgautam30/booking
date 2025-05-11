using BookerApiTest.Models;
using Newtonsoft.Json;
using Reqnroll;
using RestSharp;

namespace BookerApiTest.StepDefinitions
{
    [Binding]
    public class GetBookingSteps : BaseSteps
    {
        private int _bookingId;

        public GetBookingSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [Given(@"I request booking details for ID (\d+) with Accept: application/json header")]
        public void GivenIRequestBookingDetailsForIDWithHeader(int bookingId)
        {
            _scenarioContext.Set(bookingId, "BookingID");
        }

        [Given(@"I have an invalid booking ID (.*)")]
        public void GivenIHaveAnInvalidBookingID(string bookingId)
        {
            // Handle different invalid ID types
            if (int.TryParse(bookingId, out int id))
            {
                _bookingId = id;
            }
            else
            {
                _bookingId = -1; // Represent non-numeric IDs as -1
            }
            _scenarioContext.Set(_bookingId, "BookingID");
        }

        [When(@"I request booking details for that ID")]
        public void WhenIRequestBookingDetailsForThatID()
        {
            var bookingId = _scenarioContext.Get<int>("BookingID");
            var request = new RestRequest($"booking/{bookingId}", Method.Get);
            request.AddHeader("Accept", "application/json");
            ExecuteRequest(request);
        }


        [Then(@"the response should contain valid booking details with JSON content type")]
        public void ThenTheResponseShouldContainValidBookingDetailsWithJsonContentType()
        {
            var response = _scenarioContext.Get<RestResponse>("Response");

            // Validate content type
            Assert.That(response.ContentType, Does.Contain("application/json"),
                $"Expected JSON content type but got {response.ContentType}");

            // Validate response body structure
            var booking = JsonConvert.DeserializeObject<Booking>(response.Content);
            Assert.That(booking, Is.Not.Null, "Failed to deserialize booking response");
        }

        [Then(@"the booking details should have all required fields with correct types")]
        public void ThenTheBookingDetailsShouldHaveAllRequiredFieldsWithCorrectTypes()
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            var booking = JsonConvert.DeserializeObject<Booking>(response.Content);

            Assert.Multiple(() =>
            {
                // Validate string fields
                Assert.That(booking.FirstName, Is.Not.Null.And.Not.Empty, "FirstName is missing");
                Assert.That(booking.LastName, Is.Not.Null.And.Not.Empty, "LastName is missing");

                // Validate numeric field
                Assert.That(booking.TotalPrice, Is.GreaterThanOrEqualTo(0), "TotalPrice is invalid");

                // Validate boolean field
                Assert.That(booking.DepositPaid, Is.TypeOf<bool>(), "DepositPaid should be boolean");

                // Validate nested dates
                Assert.That(booking.BookingDates.CheckIn, Is.Not.Null.And.Match(@"\d{4}-\d{2}-\d{2}"),
                    "Invalid CheckIn date format");
                Assert.That(booking.BookingDates.CheckOut, Is.Not.Null.And.Match(@"\d{4}-\d{2}-\d{2}"),
                    "Invalid CheckOut date format");

                // Validate additional needs (optional field)
                if (!string.IsNullOrEmpty(booking.AdditionalNeeds))
                {
                    Assert.That(booking.AdditionalNeeds, Is.Not.Empty, "AdditionalNeeds should not be empty if present");
                }
            });
        }

        [Then("the booking details response status code should be (.*)")]
        public void ThenTheBookingDetailsResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode),
                $"Expected status code {expectedStatusCode} but got {(int)response.StatusCode}");
        }


    }
}
