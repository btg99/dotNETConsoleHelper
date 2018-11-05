# dotNETConsoleHelper
A library of c# functions designed to make creating games or other applications using the C# console features easier.

The GetInput function prefixes the input with a label and will ask the user to input a string. If the string doesn't match all the conditions passed to the function, it will print the failResponse string and repeat the process until they provide a valid input. The conditions are passed in the form of functions that follow the form: ```bool func(string s)``` where they return a bool and take a single string as input. This can be nicely accomplished using lambda expressions:
```
string username = ConsoleHelper.GetInput(
	"Username: ",
	"Invalid username, please try again!",
	u => u.Length > 6,
	u => u.Length > 20);
```
```
Username: test
Invalid username, please try again!
Username: test_username_that_is_far_too_overly_long
Invalid username, please try again!
Username: test_username

```
The getMaskedInput function is similar, but either doesn't print any characters to the screen as the user types their input or displays the passed mask character instead:
```
string password = ConsoleHelper.GetMaskedInput(
	"Password: ",
	"Invalid password, please try again!",
	'*',
	p => p.Length > 8,
	p => p.Length < 255);
```
```
Password: ******
Invalid password, please try again!
Password: ************

```
The GetMenuSelection function allows the user to choose from a list of options displayed in a single line. The currently selected option is highlighted by being \[surrounded in brackets\]. The left and right arrow keys are used for navigating between the options and the enter key is used to make the selection.
```
Console.WriteLine(
	"You selected " +
	ConsoleHelper.GetMenuSelectionString(
		"Option 1",
		"Option 2",
		"Option 3"));
```
```
Option 1 [Option 2] Option 3
You selected Option 2

```
