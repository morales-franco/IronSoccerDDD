# Iron Soccer DDD
### Domain Driven Design - Implementation with Net Core 3.1 + E.F Core

#### Topics
1. Domain Driven Design implementation
2. Implementing Domain Model in E.F Core
3. Repository Pattern 
4. Unit of Work Pattern
5. N-Layer Pattern
6. Entity & ValueObjects
7. Rich Model - NO Anemic
8. Domain Events
9. Implementing Value Objects in E.F Core as OwnEntities
10. Middleware - Generic Handler Error
11. Basic API - Response with Envelope (wrapper)
12. Notification Pattern to validation in domain model - Result
13. Guard to validation in domain model
14. Extension of Dependency Injection Engine with SCRUTOR - Registering dependencies scanning assemblies

#### Domain Rules

**Country Entity**
1. Fixes entities - the system only has 5 countries: Argentina | New Zealand | Germany | Chile | Spain
2. Every country can register a max of 2 teams.
3. As an API consumer, I want to retrieve a country list

**Team Entity**
1. As an API consumer, I want to retrieve a team list
2. As a user, I want to create a team - I must to specify: Team Name & CountryId
3. As a user, I want to join new player in a team - I must to specify: TeamId & PlayerId
4. A team can have a max of 5 players
5. A team can play a max of 2 matches per day
6. A team can play a max of 2 matches per period (MM-YYYY)

**Player Entity**
1. As an API consumer, I want to retrieve a player list
2. As a user, I want to create a player - I must to specify: FirstName | LastName | BirthDate | Email | Phone | TeamId
3. The player's email must be UNIQUE en the aplication.
4. The player must be 16 years old or upper

**Match Entity**
1. As an API consumer, I want to retrieve a match list
2. As an API consumer, I want to retrieve a match by Id
3. As a User, I want to create a new match - I must to specify: TeamAId | TeamBId | Match Date
4. As a User, I want to delete a match by Id
5. As a User, I want to re-schedule the match date - Update Match Date
6. As a User, I want to set the end result of the match adding: Winning Team & BestPlayer

