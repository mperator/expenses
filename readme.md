# Projekt
07.11.2020

https://www.khanacademy.org/computing/computer-programming/programming/good-practices/a/planning-a-programming-project


## 1. What do you want to make?

**Expenses**: Eine Anwendung mit der Kosten für ein Event z.B. Urlaub für mehrere Leute getrackt werden können. Am Ende des Events bekommt jeder eine Auflistung über eine Überweisung, die er an jemand anderen Tätigen muss.

## 2. What technology will you use?
Als Technologie sollen .NET Core 3.1 für Backend, ReactJs für Frontend und SQLServer zum Speichern der Daten verwendet werden. 

**Libraries**
- ApplicationInsights soll für Logging verwendet werden.
- Serilog Logger
- AutoMapper
- FluentValidation
- mermaid: https://mermaid-js.github.io/mermaid/#/
- SwashBuckle

## 3. What features will it include?
- Betrag mit Bezichnung und Datum
- Event für Zeitraum und Personen
- Benutzerregistrierung

- Berechtigung zur Verwaltung von Events
- Benachrichtigungssystem
- Währungen mit evtl. API für aktuellen Umrechnungskurs
- PDF Generierung über die Ausgaben
- Statistiken pro Nutzer

- Expenses Aufteilen (Expense individuall auf User verteilen)

## 4. But what features must it include?

### Release v1
- Benutzerverwaltung (Anmeldung, Registierung, Einladung)
- EventVerwaltung
    - Betrag mit Bezeichnung und Datum

### Release v2
- Berechtigung zur Verwaltung von Events
- Benachrichtigungssystem
- Währungen mit evtl. API für aktuellen Umrechnungskurs
- PDF Generierung über die Ausgaben
- Statistiken pro Nutzer
- Expenses Aufteilen (Expense individuall auf User verteilen)

## 5. How will you implement it?
### Interation 1

#### Database
User: (FirstName, LastName, DateOfBirth)
Event: (Id, Title, Description, Creator, BeginDate, EndDate, Currency)
EventParticipants: (E_Id, UserId)
EventExpenses: (E_Id, EX_Id)
Expenses: (Id, Title, Description, Date, Amount, Issuer)
ExpensesParticipants: (EX_Id, UserId)

#### Businessrules
- Issuer can update event data
- Issuer can invite other users
- Issuer can modify and delete all expenses
- Participants can create expenses
- Participants can delete own expenses
- Participants can view all expenses
- Expense participant must be an event participant
- User can change login data (Password, TwoFactorAuth, BackupCodes, Email Reset)

#### Architecture
MSSQL <--> Backend (ASP.NET Core 3.1) <--> Frontend (reactjs)

expenses
- frontend
    - src
    - ...
- backend
    - Expenses.Api
- .git

## 6. What's your timeline?
Weekly Wednesday Meetup 20:00 Uhr
- Week 1: Initialize Project, introduce object structure, API Design, User Flow

AuthController
    POST api/auth/register
        FromBody: UserRegisterModel
        Returns: 
            201 NoContent
            400 BadRequest
                X is invalid or already taken.

    POST api/auth/login
        FromBody: LoginModel
        Returns:
            200 Ok -> TokenModel
            400 BadRequest

    POST api/auth/logout
        Returns:
            201 NoContent

    POST api/auth/refreshToken
        FromCookie
        Returns: 
            200 Ok -> TokenModel
            400 BadRequest
            401 Unauthorized

    POST api/auth/emailVerification

UserController
    [Authorized]
    GET api/users/info
    Get userdata from backend
        Returns:
            200 Ok -> UserInfoReadModel
            401 Unauthorized

    [Authorized]
    POST api/users/changeEmail
        FromBody: UserInfoEmailWriteModel
    Request email change
        Returns: 
            201 NoContent
            401 Unauthorized

    [Authorized]
    PUT api/users/info
        FromBody: UserInfoWriteModel
        Returns: 
            200 Ok -> UserInfoReadModel
            401 Unauthorized

[Authorized]
EventController
    POST api/events
        FromBody: EventWriteModel
        Returns: 
            200 Ok -> EventReadModel
            400 BadRequest
            401 Unauthorized
            403 Forbidden
    
    GET api/events
        FromQuery: Filter options
        Returns: 
            200 Ok -> List<EventReadModel>
            400 BadRequest
            401 Unauthorized
            403 Forbidden

    GET api/events/{id}
        Returns: 
            200 Ok -> EventReadModel
            400 BadRequest
            401 Unauthorized
            403 Forbidden

    PUT api/events/{id}
        FromBody: EventWriteModel
        Returns: 
            201 NoContent
            400 BadRequest
            401 Unauthorized
            403 Forbidden

    DELETE api/events/{id}
        Returns: 
            201 NoContent
            400 BadRequest
            401 Unauthorized
            403 Forbidden

[Authorized]
ExpenseController
    POST api/events/{eventid}/expenses
        FromBody: ExpenseWriteModel
        Returns: 
            200 Ok -> ExpenseReadModel
            400 BadRequest
            401 Unauthorized
            403 Forbidden
    
    GET api/events/{eventid}/expenses
        FromQuery: Filter options
        Returns: 
            200 Ok -> List<ExpenseReadModel>
            400 BadRequest
            401 Unauthorized
            403 Forbidden

    GET api/events/{eventid}/expenses/{id}
        Returns: 
            200 Ok -> ExpenseReadModel
            400 BadRequest
            401 Unauthorized
            403 Forbidden

    PUT api/events/{eventid}/expenses/{id}
        FromBody: ExpenseWriteModel
        Returns: 
            201 NoContent
            400 BadRequest
            401 Unauthorized
            403 Forbidden

    DELETE api/events/{eventid}/expenses/{id}
        Returns: 
            201 NoContent
            400 BadRequest
            401 Unauthorized
            403 Forbidden

- Week 2: Implementation objects, api, ...
- Week 3: Design UI
- Week 4: Implementation UI
- Week 5: Finalization Release v1
Release v1


