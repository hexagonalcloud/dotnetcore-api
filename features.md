# Features

Try to specify the requirments for the API using Given When Then.

## Some Guidelines

https://gojko.net/2015/02/25/how-to-get-the-most-out-of-given-when-then/

> A good trick, that prevents most of accidental misuse of Given-When-Then, is to use past tense for ‘Given’ clauses, present tense for ‘When’ and future tense for ‘Then’. This makes it clear that ‘Given’ statements are preconditions and parameters, and that ‘Then’ statements are postconditions and expectations.

>Make ‘Given’ and ‘Then’ passive – they should describe values rather than actions. Make sure ‘When’ is active – it should describe the action under test.

>Try having only one ‘When’ statement for each scenario.

 http://www.jroller.com/perryn/entry/given_when_then_and_how

 ## Tests

Create tests using the specifications. Start out with something like [this approach](http://rob.conery.io/2014/03/05/pragmatic-bdd/) and see how far we get without introducing any additional BDD frameworks.

## Feature: Products v 0.1

### Scenario: Request Products

- Given a resource Products
- When a Client sends a GET request
- Then the API sends a Products response
