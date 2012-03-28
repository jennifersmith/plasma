# Plasma

Plasma is a web automation framework for .NET that allows you to write fast and painless web based tests of your ASP.NET web application. You can fire requests at your application programatically and assert by checking the HTML response that you get back. As an added benefit, the ASP.NET application is hosted within the test runner process: you don't need to have a real server or depend on IIS. Being standalone means that Plasma tests can easily be run on a build server.

At the lowest level, responses are returned as plain text (it is HTTP after all...). To make querying the response easier, we have incorporated some of the [http://code.google.com/p/selenium/?redir=1](Webdriver) interfaces which provide a useful way of looking up specific page elements and finding/manipulating their values. We plan to make a complete web driver implementation later so you have a choice between low level HTTP interaction and a more 'browser like' driver for your tests. 

See the [wiki](https://github.com/jennifersmith/plasma/wiki) for examples.

## Support

It supports all .NET web applications that can be run in IIS or the web development server. It has been tested with .NET versions 3.5 and above but it should also support 2.0. If you have any problems getting your particular web application to run, please raise an issue on the GitHub issue page for this project: [http://github.com/jennifersmith/plasma/issues].

The committers have used it mainly on ASP.NET MVC projects but the plasma test suite covers webforms also.

Plasma is a standalone library that does not depend on any particular testing framework. We use NUnit currently for the examples and functional tests but you can use any testing framework you like.

## JavaScript

Plasma tests remove the need to use the browser or even a real server in the tests. While this makes the tests very reliable and lightening-fast, you cannot test any JavaScript behaviour. A browser automation library such as Selenium/Webdriver's Firefox/Chrome driver or additional coverage with JavaScript unit tests are both good options here. 

## Road map

We have a [road map](https://github.com/jennifersmith/plasma/wiki/Roadmap) listing the features we want to 

## Contributing

Happy to receive any pull requests. So if you can fix a bug or have a feature to add please go ahead!

## Contributors

In no particular order:

* [Alex Harin](http://github.com/aharin)
* [Jennifer Smith](http://www.jennifersmith.co.uk)
* [Steve Smith](http://stevesmithblog.com/)
* [Chris Bird](http://www.christopherbird.co.uk)
* [Tom Scott](https://github.com/tomwscott)

## Licence

Plasma is distributed under the terms of the Microsoft Permissive Licence: [http://www.microsoft.com/opensource/licenses.mspx#Ms-PL]

