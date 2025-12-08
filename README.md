# ?? TravelShare - Studentska platforma za dijeljenje putovanja

> **Modul 1: Korisnièki sustav (User Management)** - Demo verzija

TravelShare je web aplikacija koja omoguæava studentima organizaciju zajednièkih putovanja, dijeljenje troškova i povezivanje sa drugim putnicima.

## ?? Sadržaj

- [Znaèajke](#-znaèajke)
- [Tehnologije](#-tehnologije)
- [Instalacija](#-instalacija)
- [Pokretanje](#-pokretanje)
- [Demo raèuni](#-demo-raèuni)
- [Struktura projekta](#-struktura-projekta)
- [Dokumentacija](#-dokumentacija)
- [Clean Code principi](#-clean-code-principi)

---

## ? Znaèajke

### Implementirane funkcionalnosti

- ? **Prijava u sustav** - Email/password autentifikacija
- ? **Odjava iz sustava** - Sigurna odjava sa brisanjem sesije
- ? **Pregled korisnièkog profila** - Prikaz svih korisnièkih podataka
- ? **Razlikovanje korisnièkih tipova** - Student vs Administrator
- ? **Travel preferencije** - Prikaz budžeta, destinacija, tipova putovanja
- ? **Session management** - 30-minutni session timeout
- ? **Responzivni UI** - Bootstrap 5 dizajn
- ? **Praæenje aktivnosti** - Pamæenje posljednje prijave

### Planirane funkcionalnosti

- ?? Registracija korisnika
- ?? Ureðivanje profila
- ?? Reset lozinke
- ?? OAuth (Google/Microsoft)
- ?? Administrator kontrole

---

## ?? Tehnologije

- **Framework:** ASP.NET Core 8.0 MVC
- **Language:** C# 12
- **UI:** Bootstrap 5 + Bootstrap Icons
- **Session:** In-Memory Session Storage
- **Design Patterns:** 
  - Template Method Pattern
  - Factory Pattern
  - Dependency Injection
  - Repository Pattern (spremno)

---

## ?? Instalacija

### Preduvjeti

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 ili Visual Studio Code
- Git (opcionalno)

### Kloniranje projekta

```bash
git clone https://github.com/your-repo/TravelShare.git
cd TravelShare
```

### Instalacija dependencies

```bash
cd TravelShare
dotnet restore
```

---

## ?? Pokretanje

### Putem Visual Studio

1. Otvori `TravelShare.sln`
2. Pritisni `F5` ili klikni **Run**
3. Aplikacija æe se otvoriti na `https://localhost:5001`

### Putem terminala

```bash
cd TravelShare
dotnet run
```

Aplikacija æe biti dostupna na:
- **HTTPS:** https://localhost:5001
- **HTTP:** http://localhost:5000

---

## ?? Demo raèuni

### Student raèun
```
Email:    student@travelshare.com
Password: password123
```

**Znaèajke:**
- Pregled travel preferencija
- Statistike putovanja (0 trenutno)
- Broj indeksa, fakultet, sveuèilište

### Administrator raèun
```
Email:    admin@travelshare.com
Password: admin123
```

**Znaèajke:**
- Odjel i dozvole
- Administratorski panel (planiran)

---

## ?? Struktura projekta

```
TravelShare/
?
??? Controllers/              # MVC Controllers
?   ??? HomeController.cs    # Landing page, homepage
?   ??? AccountController.cs # Login, Logout, Profile
?
??? Models/                   # Domain models
?   ??? Users/               # Inheritance demo
?   ?   ??? UserBase.cs      # Abstract base class
?   ?   ??? Student.cs       # Student type
?   ?   ??? Administrator.cs # Admin type
?   ?
?   ??? Authentication/       # Polymorphism demo
?       ??? AuthenticationProvider.cs       # Abstract provider
?       ??? EmailAuthenticationProvider.cs  # Concrete implementation
?
??? Services/                 # Business logic
?   ??? IAuthenticationService.cs
?   ??? MockAuthenticationService.cs
?   ??? IUserService.cs
?   ??? MockUserService.cs
?
??? ViewModels/               # View-specific models
?   ??? LoginViewModel.cs
?   ??? UserProfileViewModel.cs
?
??? Views/                    # Razor views
?   ??? Home/
?   ?   ??? Index.cshtml
?   ??? Account/
?   ?   ??? Login.cshtml
?   ?   ??? Profile.cshtml
?   ??? Shared/
?       ??? _Layout.cshtml
?
??? Program.cs                # App startup
```

---

## ?? Dokumentacija

Detaljnu dokumentaciju možete pronaæi u sljedeæim datotekama:

- **[STRUCTURE.md](STRUCTURE.md)** - Detaljna struktura projekta i OOP koncepti
- **[DIAGRAMS.md](DIAGRAMS.md)** - Use case, class, sequence dijagrami
- **[REQUIREMENTS.md](REQUIREMENTS.md)** - Funkcionalni zahtjevi i dekompozicija

### Kljuèni dokumenti

#### Use Case dijagrami
Sadrži 3 detaljna use case-a:
1. **UC-UM-001:** Prijava u sustav
2. **UC-UM-002:** Pregled korisnièkog profila
3. **UC-UM-003:** Postavljanje travel preferencija

#### Class dijagram
Prikazuje:
- Hijerarhiju `UserBase ? Student, Administrator`
- Polimorfizam u `AuthenticationProvider`
- Dependency Injection strukturu

---

## ?? Clean Code principi

### 1. **Nasljeðivanje (Inheritance)**

```csharp
// Bazna klasa
public abstract class UserBase
{
    public int Id { get; set; }
    public string Email { get; set; }
    // ... zajednièka svojstva
    
    public abstract string GetUserType();
}

// Derivirane klase
public class Student : UserBase
{
    public string StudentId { get; set; }
    public override string GetUserType() => "Student";
}

public class Administrator : UserBase
{
    public string Department { get; set; }
    public override string GetUserType() => "Administrator";
}
```

**Prednost:** Eliminira duplikaciju koda, omoguæava proširivanje.

---

### 2. **Polimorfizam (Polymorphism)**

```csharp
// Metoda može primiti bilo koju izvedenicu UserBase
private void StoreUserInSession(UserBase user)
{
    // Poziva odgovarajuæu override metodu
    var userType = user.GetUserType(); // "Student" ili "Administrator"
}
```

**Prednost:** Fleksibilan kod, lakše dodavanje novih tipova korisnika.

---

### 3. **Dependency Injection**

```csharp
// U Program.cs
builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();

// U kontroleru
public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;
    
    public AccountController(IAuthenticationService authService)
    {
        _authService = authService;
    }
}
```

**Prednost:** Loose coupling, lakše testiranje, zamjena implementacije.

---

### 4. **Template Method Pattern**

```csharp
public abstract class AuthenticationProvider
{
    // Fiksna struktura (template)
    public async Task<AuthenticationResult> AuthenticateAsync(string id, string cred)
    {
        if (!await ValidateCredentialsAsync(id, cred))
            return AuthenticationResult.Failed("Invalid");
        
        var user = await GetUserAsync(id);
        await LogAuthenticationAsync(user);
        return AuthenticationResult.Success(user);
    }
    
    // Konkretne klase implementiraju detalje
    protected abstract Task<bool> ValidateCredentialsAsync(...);
    protected abstract Task<UserBase?> GetUserAsync(...);
}
```

**Prednost:** Ponovno korištenje algoritma, fleksibilnost u detaljima.

---

## ?? Testiranje

### Manualni testni scenariji

1. **Login test**
   ```
   1. Otvori https://localhost:5001
   2. Klikni "Prijava"
   3. Unesi: student@travelshare.com / password123
   4. Verifikuj: Preusmjeravanje na homepage sa "Dobrodošao/la, Marko!"
   ```

2. **Profile test**
   ```
   1. Prijavi se kao student
   2. Klikni na avatar ? "Moj profil"
   3. Verifikuj: Prikazuju se travel preferencije
   ```

3. **Logout test**
   ```
   1. Prijavi se
   2. Klikni avatar ? "Odjava"
   3. Verifikuj: Vraæen na landing page, gost view
   ```

### Unit testovi (planiran)

```bash
dotnet test
```

---

## ?? Sigurnost

### Implementirane mjere

- ? HTTPS enforced
- ? Anti-forgery tokens
- ? Session-based authentication
- ? Session timeout (30 min)

### Za produkciju potrebno

- ?? Password hashing (BCrypt/Argon2)
- ?? Rate limiting na login
- ?? CAPTCHA za brute-force zaštitu
- ?? SQL injection prevention
- ?? XSS protection
- ?? CORS policy

---

## ?? Performance

### Trenutne metrike (Mock)

| Operacija      | Vrijeme     |
|----------------|-------------|
| Login          | < 500ms     |
| Load Profile   | < 300ms     |
| Session lookup | < 50ms      |

### Cilj za produkciju

| Operacija      | Cilj        |
|----------------|-------------|
| Login          | < 2000ms    |
| Load Profile   | < 1000ms    |
| API response   | < 500ms     |

---

## ?? Doprinos

Ovaj projekt je dio obrazovnog projekta. Za prijedloge i pitanja:

1. Otvori Issue
2. Kreiraj Pull Request
3. Kontaktiraj tim

---

## ?? Licenca

MIT License - Vidi [LICENSE](LICENSE) za detalje

---

## ?? Tim

**TravelShare Development Team**
- User Management Module
- 2024

---

## ?? Obrazovni ciljevi

Ovaj modul demonstrira:

1. **OOP koncepte**
   - Nasljeðivanje
   - Polimorfizam
   - Apstrakcija
   - Enkapsulacija

2. **Design Patterns**
   - Template Method
   - Factory
   - Dependency Injection

3. **Clean Code**
   - SOLID principi
   - DRY princip
   - Meaningful names
   - Documentation

4. **ASP.NET Core MVC**
   - Controllers
   - Views (Razor)
   - ViewModels
   - Routing
   - Session management

---

## ?? Kontakt

Za pitanja i podršku:
- Email: support@travelshare.com
- GitHub Issues: [Link to issues]

---

**Verzija:** 1.0.0 (Mockup)  
**Datum:** 2024  
**Status:** ? Demo verzija spremna
