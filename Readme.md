# Plasma

Plasma is a web automation framework for .NET that allows you to write fast and painless web based tests of your ASP.NET web application. You can fire requests at your application programatically and assert by checking the HTML response that you get back. As an added benefit, the ASP.NET application is hosted within the test runner process: you don't need to have a real server or depend on IIS. Being standalone means that Plasma tests can easily be run on a build server.

# Support

It supports all .NET web applications that can be run in IIS or the web development server. It has been tested with .NET versions 3.5 and above but it should also support 2.0. If you have any problems getting your particular web application to run, please raise an issue on the GitHub issue page for this project: [http://github.com/jennifersmith/plasma/issues].

# JavaScript

Plasma tests remove the need to use the browser or even a real server in the tests. While this makes the tests very reliable and lightening-fast, you cannot test any JavaScript behaviour. A browser automation library such as Selenium/Webdriver or additional coverage with JavaScript unit tests are both good options here. 

# Examples

# API


