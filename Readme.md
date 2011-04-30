# Plasma

Plasma is a web automation framework for .NET that allows you to write fast and painless web based tests of your ASP.NET web application. You can fire requests at your application programatically and assert by checking the HTML response that you get back. As an added benefit, the ASP.NET application is hosted within the test runner process: you don't need to have a real server or depend on IIS. Being standalone means that Plasma tests can easily be run on a build server.

At the lowest level, responses are returned as plain text (it is HTTP after all...). To make querying the response easier, we have incorporated some of the [http://code.google.com/p/selenium/?redir=1](Webdriver) interfaces which provide a useful way of looking up specific page elements and finding/manipulating their values. We plan to make a complete web driver implementation later so you have a choice between low level HTTP interaction and a more 'browser like' driver for your tests. 

## Examples

### A basic example:

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
    
### Submitting forms

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

### Cookies

Plasma includes some support for adding cookies to requests and evaluating cookies from responses. If you use cookie-based authentication this allows you to test authentication and also automatically log in your user in the setup for a particular test.

In order to persist the cookies, we need to write some extra code to wrap the AspNetApplication and store cookies in the state.

	public class MyProjectTestEnvironment
	{
		private AspNetApplication _aspNetApplication;
		private IEnumerable<HttpCookie> _cookies = new HttpCookie[0];

		public MyProjectTestEnvironment(AspNetApplication aspNetApplication)
		{
			_aspNetApplication = aspNetApplication;
		}

		public AspNetResponse ProcessRequest(string url)
		{
			var request = new AspNetRequest(url);
			request.AddCookies(_cookies);
			var response = _aspNetApplication.ProcessRequest(request);
			_cookies = response.Cookies;
		}
									
		public AspNetResponse ProcessRequest(AspNetRequest request)
		{
			request.AddCookies(_cookies);
			var response = _aspNetApplication.ProcessRequest(request);
			_cookies = response.Cookies;
		}
									
		public IEnumerable<HttpCookie> Cookies
		{
			get
			{
				return _cookies;
			}
		}
			
		public void ClearCookies()
		{
			_cookies.Clear();
		}
	}

	[TestFixture]
	public class CookieAuthenticationTests 
	{

		[Test]
		public void ShouldAllowUsersToSeeTheSecretPageOnceTheyAreLoggedIn()
		{
			var environment = new MyProjectTestEnvironment(_aspNetApplication);
			var loginPage = environment.ProcessRequest("/login").Html();
			AspNetForm aspNetForm = loginPage.GetForm();
			aspNetForm["username"] = "Bob";
			aspNetForm["password"] = "password";
			
			// Our app uses forms authentication so we should now have a cookie
			environment.ProcessRequest(aspNetForm.GenerateFormPostRequest());
			Assert.That(environment.Cookies[0].Name, Is.EqualTo(".ASPXAUTH"));
			
			// ... meaning we can access the secret page
			var secretPageResponse = environment.ProcessRequest("/secretPage");
			Assert.That(secretPageResponse.Status, Is.EqualTo(200));
			
			// Clearing cookies and requesting the secret page will give a 403
			environment.ClearCookies();
			secretPageResponse = environment.ProcessRequest("/secretPage");
			Assert.That(secretPageResponse.Status, Is.EqualTo(403));
		}
	}  

### Changing state on the web server

Plasma sets up an ASP.NET application to run in an AppDomain embedded within the running process (see [http://blog.flair-systems.com/2010/05/c-fastfood-what-appdomain.html](here) for an explanation of AppDomains). Your tests can communicate with the running application via the magic of remoting via a number of helper functions that Plasma provides. You can use this to change state or add things to session.

    [Test]
    public void ShouldShowDetailsOfTheCurrentCheeseBeingViewed()
		{
			// we are using static state here cos it's an example. See later for a better way of doing it when you have an IoC container to hand
			aspNetApplication.InvokeInAspAppDomain(()=>{
				CheeseRepository.AddCheese(new Cheese(){
					Name = "Stilton",
				  Region = "Derby, Leicestershire, Nottingham",
					SourceOfMilk = Milk.Cow
				});
			});
			
			var stiltonPage = aspNetApplication.ProcessRequest("/cheeses/Stilton").Html();
			
			Assert.That(stiltonPage.FindElement(By.Id("region")), Is.StringContaining("Derby, Leicestershire, Nottingham"));
		}

Obviously this example is not ideal as you must let in static/global state, methods for testing and all manner of evils into your production code. Another technique is to make use of your IoC container. Override dependencies with new stub versions for your tests and set test data on these. This requires a little more work on application setup.

public class ApplicationSetup
{
		
}

## Gotchas with InvokeInAppDomain 

## Support

It supports all .NET web applications that can be run in IIS or the web development server. It has been tested with .NET versions 3.5 and above but it should also support 2.0. If you have any problems getting your particular web application to run, please raise an issue on the GitHub issue page for this project: [http://github.com/jennifersmith/plasma/issues].

The committers have used it mainly on ASP.NET MVC projects but the plasma test suite covers webforms also.

Plasma is a standalone library that does not depend on any particular testing framework. We use NUnit currently for the examples and functional tests but you can use any testing framework you like.

## JavaScript

Plasma tests remove the need to use the browser or even a real server in the tests. While this makes the tests very reliable and lightening-fast, you cannot test any JavaScript behaviour. A browser automation library such as Selenium/Webdriver's Firefox/Chrome driver or additional coverage with JavaScript unit tests are both good options here. 


## Todo list

We are still actively working on Plasma and plan to extend the library to avoid the amount of boiler plate code on the consumer. Our current todo list includes:

* Implementing FindElement(By.CssSelector( ... which currently does not work
* Adding a web driver implementation 'PlasmaWebDriver' and requisite interfaces
* Adding examples for how to override functionality in various IoC containers
* Adding helpers for dealing with app domain to app domain communication
* Improve the syntax around submitting forms
* Events to hook into setup/teardown of app domain?

## Contributing

Happy to receive any pull requests. So if you can fix a bug or have a feature to add please go ahead!

## Contributors

* [http://github.com/aharin](Alex Harin)
* [http://jennifersmith.co.uk](Jennifer Smith)
* [http://stevesmithblog.com/](Steve Smith)

## Licence

Plasma is distributed under the terms of the Microsoft Permissive Licence: [http://www.microsoft.com/opensource/licenses.mspx#Ms-PL]

