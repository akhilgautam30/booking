using Allure.Net.Commons;
using Reqnroll;
using System.Collections.Concurrent;

namespace BookingAPI.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private static readonly ConcurrentDictionary<string, TestResult> ActiveTestCases = new();

        [BeforeTestRun]
        public static void SetupAllure()
        {
            // Set up Allure results directory
            Console.WriteLine($"Allure Results Dir: {AllureLifecycle.Instance.ResultsDirectory}");
            AllureLifecycle.Instance.CleanupResultDirectory();

            // Manually write environment information to environment.properties
            var environmentInfo = new Dictionary<string, string>
            {
                { "Framework", ".NET 8.0" },
                { "OS", Environment.OSVersion.ToString() },
                { "User", Environment.UserName }
            };

            var resultsDirectory = AllureLifecycle.Instance.ResultsDirectory;
            var environmentFilePath = Path.Combine(resultsDirectory, "environment.properties");

            using (var writer = new StreamWriter(environmentFilePath))
            {
                foreach (var kvp in environmentInfo)
                {
                    writer.WriteLine($"{kvp.Key}={kvp.Value}");
                }
            }
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            var testCaseName = scenarioContext.ScenarioInfo.Title;

            // Avoid starting a new test case if one is already active
            if (ActiveTestCases.ContainsKey(testCaseName))
            {
                Console.WriteLine($"A test context is already active for '{testCaseName}'. Skipping StartTestCase.");
                return;
            }

            // Start the test case
            var testResult = new TestResult
            {
                name = testCaseName,
                labels = new List<Label>
                {
                    Label.Suite("Booking API Tests"),
                    Label.Feature(scenarioContext.ScenarioInfo.Title)
                }
            };

            try
            {
                AllureLifecycle.Instance.ScheduleTestCase(testResult);
                ActiveTestCases[testCaseName] = testResult;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error starting test case for '{testCaseName}': {ex.Message}");
            }
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            var testCaseName = scenarioContext.ScenarioInfo.Title;

            // Finalize the test case if it exists
            if (ActiveTestCases.TryRemove(testCaseName, out var testResult))
            {
                try
                {
                    AllureLifecycle.Instance.StopTestCase();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error stopping test case for '{testCaseName}': {ex.Message}");
                }
            }
        }
    }
}