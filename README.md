
# Paging Demo

This ASP.NET Core MVC Web API is an example of how to easily add paging and sorting to your application.

## How to use the project

Open the project in Visual Studio Code or Visual Studio 2017 and run it.  
Swagger is enabled for the solution, so you can test is directly from a browser. Or you can use Postman to test it.

## How it works

This solution takes advantage of .NET's model binding. If you open [PagingOptions.cs](src\Models\PagingOptions.cs), you'll see it has two properties named Offset and Limit. These are nullable ints, so they are not required. .NET's model binding will grab any inputs from the query string with those names and set the property values. Then in [HomeController.cs](src\Controllers\HomeController.cs), we add **[FromQuery] PagingOptions pagingOptions** to any endpoint we want to page.

Bonus tip:  We're using dependency injection to pull default paging options from our appSettings file.

# Contributing Guidelines

Thanks for taking the time to contribute! If you want it to do something different than it does, feel free to fork this repo. Just let people know you were inspired by me and we're all good. :)

## Issues
Ensure the bug was not already reported by searching on GitHub under issues. If you're unable to find an open issue addressing the bug, open a new issue.

Please pay attention to the following points while opening an issue.

### Write detailed information
Detailed information is very helpful to understand an issue.

For example:
* How to reproduce the issue, step-by-step.
* The expected behavior (or what is wrong).
* 
## Pull Requests
Pull Requests are always welcome. 

Ensure the PR description clearly describes the problem and solution. It should also include the relevant issue number (if applicable).

## License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

MIT © [blakfordoakes]()