//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Allure.Net.Commons;
//using Reqnroll;

//namespace BookerApiTest.Helpers
//{
//    [Binding]
//    public class Hooks
//    {
//        [BeforeTestRun]
//        public static void SetupAllure() => AllureLifecycle.Instance.CleanupResultDirectory();

//        [BeforeFeature]
//        public static void BeforeFeature(FeatureContext context) =>
//            AllureApi.AddFeature(context.FeatureInfo.Title); // Fixed: Replaced 'SetFeature' with 'AddFeature'

//        [BeforeScenario]
//        public static void BeforeScenario(ScenarioContext context) =>
//            AllureApi.AddTestParameter("Scenario", context.ScenarioInfo.Title); // Fixed: Replaced 'SetScenario' with 'AddTestParameter'
//    }
//}
