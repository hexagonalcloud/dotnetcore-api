# GraphQL API

Initial setup using the Star Wars demo.

## Example query

{
  human (id: "2") {
    id,
    name,
    homePlanet,
    appearsIn,
    friends{
      name
    }
  }
}
