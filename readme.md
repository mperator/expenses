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
- Week 2: Implementation objects, api, ...
- Week 3: Design UI
- Week 4: Implementation UI
- Week 5: Finalization Release v1
Release v1
