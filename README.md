This doesnÅft actually use GeneralizedAsync. It just uses the generalness in
`GetAwaiter()` to demonstrate how one can write a pull-style parser with, e.g.,
recursive/nested constructs while providing a push-style API for consumers.
It does not actually provide a good example of such a parser and, due to
operating character by character, is terribly inefficient and the API is a
bit convoluted. But I thought IÅfd show as a proof of concept how to abuse
the `async`/`await` state machine.
