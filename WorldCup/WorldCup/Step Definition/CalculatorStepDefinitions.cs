using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace WorldCup.Step_Definition
{
  
        [Binding]
        [Scope(Feature = "Calculator")]
        public sealed class CalculatorStepDefinitions
        {
            private readonly ScenarioContext _scenarioContext;

            public CalculatorStepDefinitions(ScenarioContext scenarioContext)
            {
                _scenarioContext = scenarioContext;
            }

            [Given("the first number is (.*)")]
            public void GivenTheFirstNumberIs(int number)
            {
                Console.WriteLine("First Number   ");

            }

            [Given("the second number is (.*)")]
            public void GivenTheSecondNumberIs(int number)
            {
                Console.WriteLine("Second Number");
            }

            [When("the two numbers are added")]
            public void WhenTheTwoNumbersAreAdded()
            {

                Console.WriteLine("Numbers are Added");
            }

            [Then("the result should be (.*)")]
            public void ThenTheResultShouldBe(int result)
            {
                Console.WriteLine("Data matched");
            }
        }
    }


