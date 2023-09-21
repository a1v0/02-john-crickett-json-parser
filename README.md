# 02-john-crickett-json-parser

JSON parser coded entirely in C#, inspired by John Crickett's coding challenges.

I couldn't get the C# test suite to work, which means I could only follow the happy path. There is one big problem in the happy path that I was not able to figure out: the final happy-path test, ensuring that my parser can handle `null` values, always returns this error:

```
Unhandled exception. Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: Cannot perform runtime binding on a null reference
```

Through all my research, I couldn't work out why my nullable Boolean _value_ gets parsed as a `null` _reference_.

## Note on the program's structure

I am fairly new to object-oriented design. I'm sure there are many ways in which I could restructure my code, e.g. moving certain methods to classes of their own, or splitting my huge class into several files, using the `partial` feature in C#. However, in the interest of time, since this project took a fair amount of time, I decided to call it a day once I had completed the happy path.
