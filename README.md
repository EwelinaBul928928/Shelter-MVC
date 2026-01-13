# Shelter — Dokumentacja (PL)

## Spis treści
1. [Instrukcja instalacji](#1-instrukcja-instalacji)
2. [Konfiguracja](#2-konfiguracja)
3. [Przegląd aplikacji](#3-przegląd-aplikacji)
4. [Architektura i encje](#4-architektura-i-encje)
5. [Technologie](#5-technologie)
6. [Przykłady użycia](#6-przykłady-użycia)
7. [Endpointy](#7-endpointy)

---

## 1. Instrukcja instalacji

### Wymagania systemowe
- **.NET SDK**: 10.0 lub nowszy
- **Baza danych**: SQLite (wbudowana w projekt)
- **Przeglądarka**: Dowolna nowoczesna przeglądarka internetowa

### Krok po kroku

1. **Sklonuj repozytorium lub pobierz projekt**
   ```bash
   git clone https://github.com/EwelinaBul928928/Shelter-MVC.git
   cd Shelter-MVC
   ```

2. **Zastosuj migracje** (baza danych zostanie utworzona automatycznie przy pierwszym uruchomieniu):
   ```bash
   cd Shelter
   dotnet ef database update
   ```
   *Uwaga: Migracje są również automatycznie stosowane przy starcie aplikacji w `Program.cs`*

3. **Uruchom aplikację:**
   ```bash
   dotnet run
   ```

4. **Otwórz w przeglądarce:**
   - `http://localhost:5276` (HTTP)

---

## 2. Konfiguracja

### Konfiguracja bazy danych
W pliku `appsettings.json` skonfiguruj connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=shelter.db"
  }
}
```

Domyślnie używana jest baza SQLite (`shelter.db`), która zostanie utworzona automatycznie w katalogu projektu.

### Konfiguracja sesji
Sesja użytkownika jest konfigurowana w `Program.cs`:
- **Timeout**: 30 minut
- **HttpOnly**: true (zabezpieczenie przed XSS)
- **IsEssential**: true

---

## 3. Przegląd aplikacji

### Czym jest Shelter?
Webowa aplikacja do zarządzania schroniskiem dla zwierząt. Pozwala na:
- Przeglądanie dostępnych zwierząt do adopcji
- Składanie wniosków adopcyjnych
- Zarządzanie wizytami weterynaryjnymi
- Rejestrację własnych zwierząt klientów
- Umawianie wizyt weterynaryjnych
- Przeglądanie aktualności schroniska

### User Stories

**Gość:**
- Przegląda dostępne zwierzęta
- Czyta aktualności
- Rejestruje konto

**Klient:**
- Składa wnioski adopcyjne
- Przegląda status swoich wniosków
- Rejestruje własne zwierzęta
- Umawia wizyty weterynaryjne
- Przegląda historię wizyt swoich zwierząt (własnych i zaadoptowanych)

**Administrator:**
- Zarządza zwierzętami (dodawanie, edycja, usuwanie)
- Przegląda i zatwierdza wnioski adopcyjne
- Zarządza wizytami weterynaryjnymi dla zwierząt ze schroniska
- Przegląda pełną historię wizyt wszystkich zwierząt
- Ma dostęp do panelu administracyjnego z dashboardem

### User Flows

**Składanie wniosku adopcyjnego:**
1. Zaloguj się jako klient
2. Przejdź do sekcji "Zwierzęta"
3. Wybierz zwierzę do adopcji
4. Kliknij "Złóż wniosek adopcyjny"
5. Wypełnij formularz (adres, telefon, doświadczenie, sytuacja mieszkaniowa)
6. Wyślij wniosek
7. Wniosek pojawia się w "Moje wnioski" ze statusem "Pending"

**Rejestracja własnego zwierzęcia:**
1. Zaloguj się jako klient
2. Przejdź do "Weterynaria" → "Zarejestruj zwierzę"
3. Dodaj dane zwierzęcia
4. Zwierzę pojawia się w "Moje zwierzęta"

**Umówienie wizyty:**
1. Zaloguj się jako klient
2. Przejdź do "Weterynaria" → "Umów wizytę"
3. Wybierz zwierzę (własne lub zaadoptowane)
4. Wybierz weterynarza
5. Wybierz usługę
6. Wybierz datę i godzinę
7. Dodaj notatki (opcjonalnie)
8. Potwierdź wizytę
9. Wizyta pojawia się w "Moje wizyty"

**Zarządzanie zwierzętami (Admin):**
1. Zaloguj się jako Admin
2. Przejdź do "Panel Admina" → "Zwierzęta"
3. Dodaj/Edytuj/Usuń zwierzę

**Zatwierdzanie wniosku adopcyjnego (Admin):**
1. Zaloguj się jako Admin
2. Przejdź do "Panel Admina" → "Wnioski adopcyjne"
3. Przeglądnij szczegóły wniosku
4. Zatwierdź lub odrzuć wniosek
5. Status zwierzęcia automatycznie zmienia się na "Adopted" po zatwierdzeniu

### Zachowanie UI
- **Frontend**: Bootstrap 5.3.0 (CDN)
- **Responsywny design**: Dostosowany do urządzeń mobilnych
- **Kolorystyka**: Zielona - gradienty i akcenty w odcieniach zieleni
- **Nawigacja**: Prosta z efektami hover na linkach
- **Sesja**: Zarządzanie stanem użytkownika przez sesję ASP.NET Core

---

## 4. Architektura i encje

### Diagram ERD

![Diagram ERD](docs/DIAGRAM1.png)
![Diagram ERD](docs/DIAGRAM2.png)

### Relacje

- **User → AdoptionApplication** (1:N) - Użytkownik może mieć wiele wniosków adopcyjnych
- **User → NewsPost** (1:N, jako Author) - Użytkownik może być autorem wielu postów
- **User → ClientAnimal** (1:N) - Użytkownik może mieć wiele zarejestrowanych zwierząt
- **User → VeterinaryAppointment** (1:N) - Użytkownik może mieć wiele wizyt
- **Animal → AdoptionApplication** (1:N) - Zwierzę może mieć wiele wniosków adopcyjnych
- **Animal → VeterinaryVisit** (1:N) - Zwierzę może mieć wiele wizyt weterynaryjnych (historia)
- **Animal → VeterinaryAppointment** (1:N) - Zwierzę może mieć wiele umówionych wizyt
- **Veterinarian → VeterinaryVisit** (1:N) - Weterynarz może wykonać wiele wizyt
- **Veterinarian → VeterinaryAppointment** (1:N) - Weterynarz może mieć wiele umówionych wizyt
- **VeterinaryService → VeterinaryVisit** (1:N) - Usługa może być użyta w wielu wizytach
- **VeterinaryService → VeterinaryAppointment** (1:N) - Usługa może być umówiona w wielu wizytach
- **ClientAnimal → VeterinaryAppointment** (1:N) - Zwierzę klienta może mieć wiele umówionych wizyt

### Encje i ich właściwości

#### User
- `Id` (int, PK)
- `Email` (string, wymagane, max 100)
- `PasswordHash` (string, wymagane, max 255)
- `FirstName` (string, wymagane, max 50)
- `LastName` (string, wymagane, max 50)
- `Phone` (string?, opcjonalne, max 20)
- `Role` (string, wymagane, max 20, domyślnie "Client")

#### Animal
- `Id` (int, PK)
- `Name` (string, wymagane, max 100)
- `Species` (string, wymagane, max 50)
- `Breed` (string?, opcjonalne, max 100)
- `Age` (int)
- `Gender` (string, wymagane, max 10)
- `Description` (string?, opcjonalne, max 1000)
- `PhotoUrl` (string?, opcjonalne, max 500)
- `Status` (string, wymagane, max 20, domyślnie "Available")
- `AdmissionDate` (DateTime)

#### AdoptionApplication
- `Id` (int, PK)
- `UserId` (int, FK → User)
- `AnimalId` (int, FK → Animal)
- `ApplicationDate` (DateTime)
- `Status` (string, wymagane, max 20, domyślnie "Pending")
- `Address` (string, wymagane, max 200)
- `Phone` (string, wymagane, max 20)
- `ExperienceWithAnimals` (string, wymagane, max 500)
- `HasOtherPets` (string, wymagane, max 10, domyślnie "Nie")
- `HasGarden` (string, wymagane, max 10, domyślnie "Nie")
- `LivingSituation` (string, wymagane, max 500)
- `Notes` (string?, opcjonalne, max 1000)

#### ClientAnimal
- `Id` (int, PK)
- `UserId` (int, FK → User)
- `Name` (string, wymagane, max 100)
- `Species` (string, wymagane, max 50)
- `Breed` (string?, opcjonalne, max 100)
- `Age` (int)
- `Gender` (string, wymagane, max 10)
- `Description` (string?, opcjonalne, max 1000)
- `PhotoUrl` (string?, opcjonalne, max 500)
- `RegistrationDate` (DateTime)

#### Veterinarian
- `Id` (int, PK)
- `FirstName` (string, wymagane, max 100)
- `LastName` (string, wymagane, max 100)
- `Specialization` (string?, opcjonalne, max 500)
- `Phone` (string?, opcjonalne, max 20)
- `Email` (string?, opcjonalne, max 200)
- `Description` (string?, opcjonalne, max 1000)

#### VeterinaryService
- `Id` (int, PK)
- `Name` (string, wymagane, max 200)
- `Description` (string?, opcjonalne, max 1000)
- `Price` (decimal, wymagane)
- `Category` (string?, opcjonalne, max 50)

#### VeterinaryVisit
- `Id` (int, PK)
- `AnimalId` (int, FK → Animal)
- `VisitDate` (DateTime, wymagane)
- `Description` (string?, opcjonalne, max 1000)
- `Diagnosis` (string?, opcjonalne, max 500)
- `Cost` (decimal?)
- `VeterinarianId` (int?, FK → Veterinarian)
- `ServiceId` (int?, FK → VeterinaryService)

#### VeterinaryAppointment
- `Id` (int, PK)
- `AppointmentDate` (DateTime, wymagane)
- `Notes` (string?, opcjonalne, max 500)
- `Status` (string, wymagane, max 20, domyślnie "Scheduled")
- `VeterinarianId` (int?, FK → Veterinarian)
- `ServiceId` (int?, FK → VeterinaryService)
- `AnimalId` (int?, FK → Animal) - dla zwierząt ze schroniska
- `ClientAnimalId` (int?, FK → ClientAnimal) - dla zwierząt klientów
- `UserId` (int?, FK → User)
- `Cost` (decimal?)
- `IsFree` (bool, domyślnie false) - true dla wizyt zwierząt ze schroniska

#### NewsPost
- `Id` (int, PK)
- `Title` (string, wymagane, max 200)
- `Content` (string, wymagane, max 5000)
- `PublicationDate` (DateTime)
- `AuthorId` (int?, FK → User)
- `PhotoUrl` (string?, opcjonalne, max 500)
- `Category` (string, wymagane, max 50, domyślnie "Information")

### Moduły / odpowiedzialności

**Kontrolery:**

- **HomeController** — strona główna, obsługa błędów
- **AccountController** — rejestracja, logowanie, wylogowanie (hashowanie hasła SHA256)
- **AnimalsController** — przeglądanie zwierząt, szczegóły zwierzęcia, filtrowanie po gatunku
- **AdoptionController** — składanie wniosków adopcyjnych, przeglądanie własnych wniosków
- **VeterinaryController** — zarządzanie wizytami, rejestracja zwierząt klientów, umawianie wizyt, historia wizyt
- **AdminController** — panel administracyjny, zarządzanie zwierzętami (CRUD), przeglądanie i zarządzanie wnioskami adopcyjnymi
- **NewsController** — przeglądanie aktualności, szczegóły posta
- **AnimalsApiController** — REST API dla zwierząt (CRUD)

**Data:**
- **ShelterDbContext** (EF Core) — kontekst bazy danych z konfiguracją relacji i zachowań usuwania

**Widoki:**
- Razor Views w katalogu `Views/`
- Layout: `_Layout.cshtml` z Bootstrap 5.3.0 (CDN)

**Sesja:**
- Zarządzanie sesją użytkownika przez `HttpContext.Session`:
  - `UserId` (int?)
  - `UserRole` (string) - "Admin" lub "Client"
  - `UserName` (string)

### Konta użytkowników

| Email | Hasło | Rola | Nazwa |
|-------|-------|------|-------|
| admin@mail.com | admin123 | Admin | Admin |
| client@mail.com | user123 | Client | Jan Kowalski |

*Uwaga: Użytkownik `client@mail.com` może mieć zarejestrowane własne zwierzę (nie ze schroniska) w systemie.*

---

## 5. Technologie

### Backend
- **ASP.NET Core MVC** (.NET 10.0)
- **Entity Framework Core** 10.0.0
- **SQLite** (wbudowana baza danych)
- **C#** (z nullable reference types)

### Frontend
- **Bootstrap** 5.3.0 (CDN)
- **jQuery** 3.7.0 (CDN)
- **Razor Views** (ASP.NET Core)
- **HTML/CSS/JavaScript**
- **Custom CSS** (`site.css`) - zielona kolorystyka

### Baza danych
- **SQLite** (pliki `.db`, `.db-shm`, `.db-wal`)
- **Entity Framework Core Migrations** - automatyczne stosowanie przy starcie

### Bezpieczeństwo
- **Hashowanie hasła**: SHA256 (w `AccountController`)
- **Sesja**: HttpOnly cookies, timeout 30 minut
- **Walidacja**: Data Annotations w modelach

---

## 6. Przykłady użycia

### Macierz uprawnień (zadania według ról)

| Zadanie | Gość | Klient | Admin |
|---------|------|--------|-------|
| Przeglądanie dostępnych zwierząt | ✅ | ✅ | ✅ |
| Przeglądanie aktualności | ✅ | ✅ | ✅ |
| Rejestracja konta | ✅ | ❌ | ❌ |
| Logowanie | ✅ | ✅ | ✅ |
| Składanie wniosku adopcyjnego | ❌ | ✅ | ✅ |
| Przeglądanie własnych wniosków | ❌ | ✅ | ✅ |
| Rejestracja własnego zwierzęcia | ❌ | ✅ | ❌ |
| Umawianie wizyty weterynaryjnej | ❌ | ✅ | ✅ |
| Przeglądanie historii wizyt swoich zwierząt | ❌ | ✅ | ❌ |
| Dostęp do panelu admina | ❌ | ❌ | ✅ |
| Zarządzanie zwierzętami (CRUD) | ❌ | ❌ | ✅ |
| Zarządzanie wnioskami adopcyjnymi | ❌ | ❌ | ✅ |
| Przeglądanie pełnej historii wizyt | ❌ | ❌ | ✅ |
| Umawianie wizyt dla zwierząt ze schroniska | ❌ | ❌ | ✅ |

## 7. Endpointy

### Kontrolery MVC

#### Home
- `GET /` lub `GET /Home` — strona główna
- `GET /Home/Error` — strona błędu

#### Account
- `GET /Account/Login` — formularz logowania
- `POST /Account/Login` — logowanie użytkownika
- `GET /Account/Register` — formularz rejestracji
- `POST /Account/Register` — rejestracja nowego użytkownika
- `GET /Account/Logout` — wylogowanie użytkownika

#### Animals
- `GET /Animals` — lista dostępnych zwierząt (z filtrowaniem po gatunku: `?species=Pies|Kot|Inne|All`)
- `GET /Animals/Details/{id}` — szczegóły zwierzęcia z historią wizyt weterynaryjnych

#### Adoption
- `GET /Adoption/Apply/{animalId}` — formularz wniosku adopcyjnego
- `POST /Adoption/Apply` — składanie wniosku adopcyjnego
- `GET /Adoption/MyApplications` — lista własnych wniosków (wymaga logowania)

#### Veterinary
- `GET /Veterinary` — strona główna weterynarii (lista weterynarzy i usług)
- `GET /Veterinary/RegisterClientAnimal` — rejestracja własnego zwierzęcia (wymaga logowania)
- `POST /Veterinary/RegisterClientAnimal` — zapisanie zarejestrowanego zwierzęcia
- `GET /Veterinary/MyClientAnimals` — lista własnych zwierząt (zarejestrowanych i zaadoptowanych) z historią wizyt (wymaga logowania)
- `GET /Veterinary/BookAppointment` — umawianie wizyty (wymaga logowania)
- `POST /Veterinary/BookAppointment` — zapisanie umówionej wizyty
- `GET /Veterinary/MyAppointments` — lista własnych wizyt (wymaga logowania)
- `GET /Veterinary/AllAnimalsHistory` — pełna historia wizyt wszystkich zwierząt (tylko Admin)
- `GET /Veterinary/BookAppointmentForShelterAnimal` — umawianie wizyty dla zwierzęcia ze schroniska (tylko Admin)
- `POST /Veterinary/BookAppointmentForShelterAnimal` — zapisanie wizyty dla zwierzęcia ze schroniska
- `GET /Veterinary/ShelterAppointments` — lista wszystkich wizyt (tylko Admin)

#### Admin
- `GET /Admin` — panel administracyjny (dashboard ze statystykami)
- `GET /Admin/Animals` — zarządzanie zwierzętami (lista wszystkich)
- `GET /Admin/Create` — formularz dodawania nowego zwierzęcia
- `POST /Admin/Create` — dodanie nowego zwierzęcia
- `GET /Admin/Edit/{id}` — formularz edycji zwierzęcia
- `POST /Admin/Edit/{id}` — aktualizacja zwierzęcia
- `GET /Admin/Delete/{id}` — usuwanie zwierzęcia
- `GET /Admin/Adoptions` — lista wszystkich wniosków adopcyjnych
- `GET /Admin/AdoptionDetails/{id}` — szczegóły wniosku adopcyjnego
- `POST /Admin/UpdateAdoptionStatus` — aktualizacja statusu wniosku (status: "Pending", "Approved", "Rejected")

#### News
- `GET /News` — lista aktualności (posortowane od najnowszych)
- `GET /News/Details/{id}` — szczegóły aktualności

### API Endpointy

#### AnimalsApi (`/api/Animals`)
- `GET /api/Animals` — lista wszystkich zwierząt (JSON)
- `GET /api/Animals/{id}` — szczegóły zwierzęcia (JSON)
- `POST /api/Animals` — dodanie nowego zwierzęcia (JSON body)
- `PUT /api/Animals/{id}` — aktualizacja zwierzęcia (JSON body)
- `DELETE /api/Animals/{id}` — usunięcie zwierzęcia

**Przykład użycia API:**

```bash
# Pobierz wszystkie zwierzęta
curl http://localhost:5276/api/Animals

# Pobierz zwierzę o ID 1
curl http://localhost:5276/api/Animals/1

# Dodaj nowe zwierzę
curl -X POST http://localhost:5276/api/Animals \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Burek",
    "species": "Pies",
    "breed": "Labrador",
    "age": 3,
    "gender": "Mężczyzna",
    "status": "Available",
    "admissionDate": "2026-01-13T00:00:00"
  }'
```

