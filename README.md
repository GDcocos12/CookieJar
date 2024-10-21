# CookieJar - Stack Machine

**CookieJar** is a simple stack machine implemented in C#. It allows performing basic stack operations such as pushing, popping, and mathematical calculations. The program also supports executing commands with labels and comments.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Commands](#commands)
- [Example](#example)
- [Requirements](#requirements)
- [License](#license)

## Installation

You can download the latest version of the program as an executable (.exe) from the [Releases](https://github.com/GDcocos12/CookieJar/releases) section of the repository.

1. Go to the Releases page and download the latest `.exe` file.
2. Place the downloaded file in a desired directory.

## Usage

1. Run the `CookieJar.exe` file.
2. Enter commands in the `CodeTextBox`.
3. Click the `Run` button to execute the commands.
4. The results of the command execution and the state of the stack will be displayed in the `ResultTextBox` and `StackPanelValues`, respectively.
5. Click the `Reset` button to clear the stack and the text fields.

## Commands

Here is a list of supported commands:

- `PUSH <number>` - pushes a number onto the stack.
- `POP` - removes the top value from the stack.
- `ADD` - adds the top two values on the stack.
- `SUB` - subtracts the top value from the second top value on the stack.
- `MUL` - multiplies the top two values on the stack.
- `DIV` - divides the second top value by the top value on the stack (checks for division by zero).
- `REM` - computes the remainder of the second top value divided by the top value on the stack.
- `CPY` - copies the top value of the stack.
- `PRINT <text>` - outputs text to the `ResultTextBox`.
- `TOP` - displays the top value of the stack.
- `JUMPIFNULL <label>` - jumps to the specified label if the top value of the stack is zero.
- `JUMPIFNOTNULL <label>` - jumps to the specified label if the top value of the stack is not zero.
- `FINISH` - finishes the execution of the program.

## Example

Example of command input:

```
PUSH 5
PUSH 10
ADD
PRINT "Result of addition"
TOP
FINISH
```

Output in the `ResultTextBox` will be:

```
Result of addition
Top value: 15
Program finished.
```

## Requirements

- Windows operating system
- .NET Framework 4.5 or higher (included in the executable)

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
