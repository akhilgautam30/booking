using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using RestSharp;

namespace BookerApiTest.StepDefinitions
{
    [Binding]
    public class UpdateBookingSteps : BaseSteps
    {
        public UpdateBookingSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        private enum AuthMethod { Token, BasicAuth, None }

        [When(@"I update booking ID (.*) with valid data using token authentication:")]
        public void WhenUpdateWithTokenAuth(int bookingId, Table table)
        {
            UpdateBooking(bookingId, table, AuthMethod.Token);
        }

        [When(@"I update booking ID (.*) with valid data using Basic Auth:")]
        public void WhenUpdateWithBasicAuth(int bookingId, Table table)
        {
            UpdateBooking(bookingId, table, AuthMethod.BasicAuth);
        }

        [When(@"I update booking ID (.*) with valid data without authentication:")]
        public void WhenUpdateWithoutAuth(int bookingId, Table table)
        {
            UpdateBooking(bookingId, table, AuthMethod.None);
        }

        [When(@"I update booking ID (.*) with the following invalid data:")]
        public void WhenUpdateWithInvalidData(int bookingId, Table table)
        {
            UpdateBooking(bookingId, table, AuthMethod.Token);
        }

        private void UpdateBooking(int bookingId, Table table, AuthMethod authMethod)
        {
            var row = table.Rows[0];
            var bookingData = new
            {
                firstname = row["firstname"],
                lastname = row["lastname"],
                totalprice = GetIntValue(row["totalprice"]),
                depositpaid = GetBoolValue(row["depositpaid"]),
                bookingdates = new
                {
                    checkin = row["checkin"],
                    checkout = row["checkout"]
                },
                additionalneeds = row.ContainsKey("additionalneeds") ? row["additionalneeds"] : null
            };

            var request = new RestRequest($"booking/{bookingId}", Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            switch (authMethod)
            {
                case AuthMethod.Token:
                    var token = _scenarioContext.Get<string>("AuthToken");
                    request.AddHeader("Cookie", $"token={token}");
                    break;
                case AuthMethod.BasicAuth:
                    request.AddHeader("Authorization", "Basic YWRtaW46cGFzc3dvcmQxMjM=");
                    break;
            }

            request.AddJsonBody(bookingData);
            _scenarioContext.Set(bookingData, "LastRequestData");
            ExecuteRequest(request);
        }

        private int GetIntValue(string value)
        {
            return int.TryParse(value, out var result) ? result : 0;
        }

        private bool GetBoolValue(string value)
        {
            return value.ToLower() switch
            {
                "true" => true,
                "false" => false,
                _ => false
            };
        }

        [Then(@"the response should match the updated booking data")]
        public void ThenVerifyResponseMatchesRequest()
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            var responseData = JObject.Parse(response.Content!);
            var requestData = _scenarioContext.Get<object>("LastRequestData");
            var requestJson = JObject.FromObject(requestData);

            Assert.Multiple(() =>
            {
                Assert.That(responseData["firstname"]?.ToString(), Is.EqualTo(requestJson["firstname"]?.ToString()));
                Assert.That(responseData["lastname"]?.ToString(), Is.EqualTo(requestJson["lastname"]?.ToString()));
                Assert.That(responseData["totalprice"]?.Value<int>(), Is.EqualTo(requestJson["totalprice"]?.Value<int>()));
                Assert.That(responseData["depositpaid"]?.Value<bool>(), Is.EqualTo(requestJson["depositpaid"]?.Value<bool>()));
                Assert.That(responseData["bookingdates"]?["checkin"]?.ToString(), Is.EqualTo(requestJson["bookingdates"]?["checkin"]?.ToString()));
                Assert.That(responseData["bookingdates"]?["checkout"]?.ToString(), Is.EqualTo(requestJson["bookingdates"]?["checkout"]?.ToString()));

                if (requestJson["additionalneeds"] != null)
                {
                    Assert.That(responseData["additionalneeds"]?.ToString(), Is.EqualTo(requestJson["additionalneeds"]?.ToString()));
                }
            });
        }
    }
}