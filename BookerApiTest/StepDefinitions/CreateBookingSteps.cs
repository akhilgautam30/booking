using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using RestSharp;

namespace BookerApiTest.StepDefinitions
{
    [Binding]
    public class CreateBookingSteps : BaseSteps
    {
        public CreateBookingSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [When(@"I send a POST request with the following valid booking data:")]
        public void WhenISendAPOSTRequestWithTheFollowingValidBookingData(Table table)
        {
            var row = table.Rows[0];
            var booking = new
            {
                firstname = row["firstname"],
                lastname = row["lastname"],
                totalprice = int.Parse(row["totalprice"]),
                depositpaid = bool.Parse(row["depositpaid"]),
                bookingdates = new
                {
                    checkin = row["checkin"],
                    checkout = row["checkout"]
                },
                additionalneeds = table.Rows[0].ContainsKey("additionalneeds") ? row["additionalneeds"] : null
            };

            var request = new RestRequest("booking", Method.Post); // Hardcoded endpoint
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(booking);

            _scenarioContext.Set(JsonConvert.SerializeObject(booking), "RequestData");
            ExecuteRequest(request);
        }

        [When(@"I send a POST request with booking data missing the (.*) field")]
        public void WhenISendAPOSTRequestWithBookingDataMissingTheField(string fieldToRemove)
        {
            var booking = new
            {
                firstname = "Jim",
                lastname = "Brown",
                totalprice = 111,
                depositpaid = true,
                bookingdates = new
                {
                    checkin = "2018-01-01",
                    checkout = "2019-01-01"
                },
                additionalneeds = "Breakfast"
            };

            var json = JObject.FromObject(booking);
            json.Remove(fieldToRemove);

            var request = new RestRequest("booking", Method.Post); // Hardcoded endpoint
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(json.ToString(), DataFormat.Json);

            ExecuteRequest(request);
        }

        [When(@"I send a POST request with booking data where (.*) is set to (.*)")]
        public void WhenISendAPOSTRequestWithBookingDataWhereFieldIsSetToValue(string fieldToSet, string value)
        {
            var booking = new
            {
                firstname = "Jim",
                lastname = "Brown",
                totalprice = 111,
                depositpaid = true,
                bookingdates = new
                {
                    checkin = "2018-01-01",
                    checkout = "2019-01-01"
                },
                additionalneeds = "Breakfast"
            };

            var json = JObject.FromObject(booking);

            // Handle special cases
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Trim('"');
            }

            if (value == "null")
            {
                json[fieldToSet] = null;
            }
            else if (value == "{}")
            {
                json[fieldToSet] = new JObject();
            }
            else if (value.StartsWith("{") && value.EndsWith("}"))
            {
                json[fieldToSet] = JObject.Parse(value);
            }
            else
            {
                // Try to parse to correct type
                if (int.TryParse(value, out var intValue))
                {
                    json[fieldToSet] = intValue;
                }
                else if (bool.TryParse(value, out var boolValue))
                {
                    json[fieldToSet] = boolValue;
                }
                else
                {
                    json[fieldToSet] = value;
                }
            }

            var request = new RestRequest("booking", Method.Post); // Hardcoded endpoint
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(json.ToString(), DataFormat.Json);

            ExecuteRequest(request);
        }

        [Then(@"the response should contain a bookingid")]
        public void ThenTheResponseShouldContainABookingid()
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            var responseBody = JObject.Parse(response.Content!);

            Assert.That(responseBody["bookingid"], Is.Not.Null, "Response should contain bookingid");
        }

        [Then(@"the response should match the request data")]
        public void ThenTheResponseShouldMatchTheRequestData()
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            var responseBody = JObject.Parse(response.Content!);
            var booking = responseBody["booking"]!;

            var requestData = _scenarioContext.Get<string>("RequestData");
            var requestJson = JObject.Parse(requestData);

            Assert.Multiple(() =>
            {
                Assert.That(booking["firstname"]!.ToString(), Is.EqualTo(requestJson["firstname"]!.ToString()));
                Assert.That(booking["lastname"]!.ToString(), Is.EqualTo(requestJson["lastname"]!.ToString()));
                Assert.That(booking["totalprice"]!.Value<int>(), Is.EqualTo(requestJson["totalprice"]!.Value<int>()));
                Assert.That(booking["depositpaid"]!.Value<bool>(), Is.EqualTo(requestJson["depositpaid"]!.Value<bool>()));
                Assert.That(booking["bookingdates"]!["checkin"]!.ToString(), Is.EqualTo(requestJson["bookingdates"]!["checkin"]!.ToString()));
                Assert.That(booking["bookingdates"]!["checkout"]!.ToString(), Is.EqualTo(requestJson["bookingdates"]!["checkout"]!.ToString()));

                if (requestJson["additionalneeds"] != null)
                {
                    Assert.That(booking["additionalneeds"]!.ToString(), Is.EqualTo(requestJson["additionalneeds"]!.ToString()));
                }
            });
        }

        [Then(@"the additionalneeds field should be empty")]
        public void ThenTheAdditionalneedsFieldShouldBeEmpty()
        {
            var response = _scenarioContext.Get<RestResponse>("Response");
            var responseBody = JObject.Parse(response.Content!);
            var booking = responseBody["booking"]!;

            Assert.That(booking["additionalneeds"]!.ToString(), Is.Empty);
        }
    }
}
