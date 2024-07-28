# SharpLox

SharpLox is a lox interpreter written entirely in C#.
It was created with a guidance from marvelous book ["Crafting Interpreters" by Robert Nystrom](https://craftinginterpreters.com/), though I made some changes to make the whole project more elegant (at least in my opinion) and to add some small features. The goal of this project was to introduce myself to the world of languages interpreters.

## Project Structure

- **Lox Class** -> entry point of the interpreter
- **BuiltIns** -> runtime representation of lox internals, such as _Functions_ and _Classes_
- **Exceptions** -> custom interpreter expressions (one of which used for very unconventional task - implementing _return_ statement)
- **Expressions** -> intepreter expressions
- **NativeFunctions** -> native lox functions implemented in intepreter (in other words - standar library)
- **Services** -> main interpreter logic, it includes lexing, parsing, resolving and interpreting
- **Statements** -> language statements
- **Tokens** -> tokens implementation
- **Utils** -> helper methods (for which I couldn't figure out better place)

## Usage

SharLox runs in two modes:

### Interpreting files

**usage**: `./sharplox <filename>`

As name suggests, it interprets lox program saved in file.

### Interpreting prompt

**usage**: `./sharplox`

In this case, it waits for user input and run intepreter immediately on it.
