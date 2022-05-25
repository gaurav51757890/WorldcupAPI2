using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;

namespace WorldCup.Hooks
{
    [Binding]
    public class HookBase
    {
        public IObjectContainer _objectContainer;
        //public static IConfiguration _configuration;
        [ThreadStatic]
        public static ExtentTest featureName;
        [ThreadStatic]
        public static ExtentTest scenario;
        private static ExtentReports extent;
        public ScenarioContext scenarioContext;
        public FeatureContext featureContext;

        public HookBase(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var scenerioname = scenarioContext.ScenarioInfo.Title;
            createNode(scenerioname);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void createNode(string scenarioname)
        {
            scenario = featureName.CreateNode<AventStack.ExtentReports.Gherkin.Model.Scenario>(scenarioname);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext) => featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);

        [AfterScenario]
        public void AfterScenario()
        {
        }

        public static ExtentReports InitializeReport()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "TestResults");
            if (extent == null)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    Directory.CreateDirectory(path);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, "index.html");
                var htmlReporter = new ExtentHtmlReporter(path);
                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;

                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
                return extent;
            }
            else
            {
                return extent;
            }
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            DeleteExecutionProof();
            extent = InitializeReport();
        }

        [AfterTestRun]
        public static void AfterTestRun() => extent.Flush();

        [MethodImpl(MethodImplOptions.Synchronized)]
        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            var stepName = scenarioContext.StepContext.StepInfo.Text;
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepError = scenarioContext.TestError;

            if (stepError == null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text);
                }
            }
            else if (stepError != null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
            }
        }

        public static void DeleteExecutionProof()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, "TestResults");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"{file} is deleted.");
                }
            }
        }
    }
}

