# Plasma

Plasma is a web automation framework for .NET that allows you to write fast and painless web based tests of your ASP.NET web application. You can fire requests at your application programatically and assert by checking the HTML response that you get back. As an added benefit, the ASP.NET application is hosted within the test runner process: you don't need to have a real server or depend on IIS. Being standalone means that Plasma tests can easily be run on a build server.

At the lowest level, responses are returned as plain text (it is HTTP after all...). To make querying the response easier, we have incorporated some of the [http://code.google.com/p/selenium/?redir=1](Webdriver) interfaces which provide a useful way of looking up specific page elements and finding/manipulating their values. We plan to make a complete web driver implementation later so you have a choice between low level HTTP interaction and a more 'browser like' driver for your tests. 

# Support

It supports all .NET web applications that can be run in IIS or the web development server. It has been tested with .NET versions 3.5 and above but it should also support 2.0. If you have any problems getting your particular web application to run, please raise an issue on the GitHub issue page for this project: [http://github.com/jennifersmith/plasma/issues].

The committers have used it mainly on ASP.NET MVC projects but the plasma test suite covers webforms also.

Plasma is a standalone library that does not depend on any particular testing framework. We use NUnit currently for the examples and functional tests but you can use any testing framework you like.

# JavaScript

Plasma tests remove the need to use the browser or even a real server in the tests. While this makes the tests very reliable and lightening-fast, you cannot test any JavaScript behaviour. A browser automation library such as Selenium/Webdriver's Firefox/Chrome driver or additional coverage with JavaScript unit tests are both good options here. 

# Examples

## A basic example:

This fires up the ASP.NET web application at the given path and checks to see if the homepage has the correct title.

    [TestFixture]
    public class MyFirstPlasmaTest
    {
        [SetUp]
				public void SetUp()
        {
            _appInstance = new AspNetApplication("/", "c:\path\to\webapp"); 
        }
       
        [TearDown]
        public void TearDown() 
        {
            _appInstance.Close();
        }
 
        [Test]
        public void ShouldSeeTitleOnTheHomePage()
        {
            var homePage = _appInstance.ProcessRequest("/");
						var titleElement = homePage.Html().FindElement(By.TagName("h1"));
						Assert.That(titleElement.Text, Is.EqualTo("This is My Application"));	 
        }
    } 
    
Note that the new AspNetApplication() line takes a few seconds (it's like running an ASP.NET for first time after compilation). It is generally a good idea to call this once per test run. You can do this in NUnit using a [http://www.nunit.org/index.php?p=fixtureSetup&r=2.2.10](TestFixtureSetupAttribute).
    
## Submitting forms

Submitting forms is just a case of forming a post request with a bunch of key-value pairs representing the contents of the form and calling ProcessRequest. However, to make things a bit more straightforward, plasma has support for 'submitting' forms found in the HTML by creating a form post request.

    [TestFixture]
    public class MySecondPlasmaTest
    {
        [Test]
        public void ShouldBeAbleToLogIn()
        {
            var loginPage = aspNetApplication.ProcessRequest("/login").Html();
            AspNetForm aspNetForm = loginPage.GetForm();
            aspNetForm["username"] = "Bob";
            aspNetForm["password"] = "password";
            AspNetResponse loginResponse = aspNetApplication.ProcessRequest(aspNetForm.GenerateFormPostRequest());

            Assert.That(loginResponse.Body, Is.StringContaining("Welcome to my Application, Bob"));
        }
    } 
 

## Cookies

## Changing state on the web server


# Todo list

We are still actively working on Plasma and plan to extend the library to avoid the amount of boiler plate code on the consumer. Our current todo list includes:

* Implementing FindElement(By.CssSelector( ... which currently does not work
* Adding a web driver implementation 'PlasmaWebDriver' and requisite interfaces
* Adding examples for how to override functionality in various IoC containers
* Adding helpers for dealing with app domain to app domain communication

# Contributing

Happy to receive any pull requests. So if you can fix a bug or have a feature to add please go ahead!

# Licence

Plasma is distributed under the terms of the Microsoft Permissive Licence: [http://www.microsoft.com/opensource/licenses.mspx#Ms-PL]

