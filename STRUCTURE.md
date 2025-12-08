# TravelShare - User Management Module (MVC Structure)

## ?? Struktura projekta

```
TravelShare/
??? Controllers/                    # MVC Controllers
?   ??? HomeController.cs          # Landing page, logout
?   ??? AccountController.cs       # Login, Logout, Profile
?
??? Models/                         # Domain models
?   ??? Users/                      # User entities (Inheritance)
?   ?   ??? UserBase.cs            # Abstract base class
?   ?   ??? Student.cs             # Student user type
?   ?   ??? Administrator.cs       # Admin user type
?   ?   ??? TravelPreferences.cs   # Student preferences
?   ?
?   ??? Authentication/             # Authentication models (Polymorphism)
?   ?   ??? AuthenticationProvider.cs         # Abstract provider (Template Pattern)
?   ?   ??? EmailAuthenticationProvider.cs    # Email auth implementation
?   ?   ??? AuthenticationResult.cs           # Result object
?   ?
?   ??? ErrorViewModel.cs           # Error handling model
?
??? Services/                       # Business logic layer
?   ??? IAuthenticationService.cs   # Auth service interface
?   ??? MockAuthenticationService.cs # Mock implementation
?   ??? IUserService.cs             # User service interface
?   ??? MockUserService.cs          # Mock implementation
?
??? ViewModels/                     # View-specific models
?   ??? LoginViewModel.cs           # Login form data
?   ??? UserProfileViewModel.cs     # Profile page data
?
??? Views/                          # Razor views
?   ??? Home/
?   ?   ??? Index.cshtml           # Landing page
?   ??? Account/
?   ?   ??? Login.cshtml           # Login page
?   ?   ??? Profile.cshtml         # User profile page
?   ??? Shared/
?       ??? _Layout.cshtml         # Main layout with navbar
?       ??? Error.cshtml           # Error page
?
??? Program.cs                      # App configuration

```

## ?? Implementirani koncepti OOP-a

### 1. **Nasljeðivanje (Inheritance)**

#### UserBase ? Student, Administrator
```csharp
// Bazna klasa sa zajednièkim svojstvima
public abstract class UserBase { ... }

// Derivirane klase sa specifiènim svojstvima
public class Student : UserBase { ... }
public class Administrator : UserBase { ... }
```

**Prednosti:**
- Izbjegavanje ponavljanja koda
- Hijerarhijska struktura korisnika
- Zajednièka funkcionalnost (GetDisplayName, GetUserType)

### 2. **Polimorfizam (Polymorphism)**

#### Virtual Methods
```csharp
// U UserBase
public virtual string GetDisplayName()
{
    return $"{FirstName} {LastName}";
}

// U Student klasi - override
public override string GetDisplayName()
{
    return $"{base.GetDisplayName()} ({StudentId})";
}
```

**Demonstracija u kontroleru:**
```csharp
// Može primiti Student ili Administrator
UserBase? currentUser = GetCurrentUser();
// Poziva odgovarajuæu implementaciju
var name = currentUser.GetDisplayName();
```

### 3. **Apstrakcija (Abstraction)**

#### Abstract Classes i Methods
```csharp
public abstract class UserBase
{
    // Svi moraju implementirati
    public abstract string GetUserType();
}

public abstract class AuthenticationProvider
{
    // Template method pattern
    protected abstract Task<bool> ValidateCredentialsAsync(...);
}
```

### 4. **Enkapsulacija (Encapsulation)**

#### Private/Protected/Public modifikatori
```csharp
public class AccountController
{
    // Privatni servisi - skrivena implementacija
    private readonly IAuthenticationService _authService;
    
    // Javne akcije - dostupne preko HTTP-a
    public async Task<IActionResult> Login(LoginViewModel model)
    
    // Privatne helper metode
    private UserBase? GetCurrentUser()
}
```

## ?? Design Patterns

### 1. **Template Method Pattern**
```csharp
// U AuthenticationProvider
public async Task<AuthenticationResult> AuthenticateAsync(...)
{
    // Fiksni koraci
    if (!await ValidateCredentialsAsync(...)) return Failed();
    var user = await GetUserAsync(...);
    await LogAuthenticationAsync(user);
    return Success(user);
}
```

### 2. **Factory Pattern (Result Object)**
```csharp
public class AuthenticationResult
{
    // Statièke metode za kreiranje rezultata
    public static AuthenticationResult Success(UserBase user)
    public static AuthenticationResult Failed(string errorMessage)
}
```

### 3. **Dependency Injection**
```csharp
// U Program.cs
builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();

// U kontrolerima
public AccountController(IAuthenticationService authService, ...)
```

## ?? Funkcionalni zahtjevi (10)

### ? Implementirano:

1. **Prijava u sustav** - `AccountController.Login()`
2. **Odjava iz sustava** - `AccountController.Logout()`
3. **Pregled korisnièkog profila** - `AccountController.Profile()`
4. **Pregled osnovnih informacija** - Prikazano na profilu
5. **Pregled travel preferencija** - Za studente na profilu
6. **Razlikovanje tipova korisnika** - Student vs Administrator
7. **Pamæenje posljednje prijave** - `LastLoginAt` property
8. **Session management** - HttpContext.Session
9. **Mock autentifikacija** - EmailAuthenticationProvider
10. **Responzivni UI** - Bootstrap 5 + custom CSS

### ?? Za implementaciju (buduæe):

- Registracija korisnika
- Ureðivanje profila
- Reset lozinke
- Notifikacije
- Google/Microsoft OAuth
- Administrator kontrole (blokiraj/obriši)

## ?? Nefunkcionalni zahtjev

**Sigurnost i performanse:**
- ? Session storage za user data
- ? HTTPS (u produkciji)
- ? ValidateAntiForgeryToken na POST akcijama
- ?? Password hashing (trenutno plaintext u mocku)
- ?? Enkripcija osjetljivih podataka (za produkciju)
- **Cilj:** Autentifikacija < 2 sekunde ?

## ?? Pokretanje aplikacije

```bash
cd TravelShare
dotnet run
```

Pristup aplikaciji: `https://localhost:5001`

### Demo pristupni podaci:

**Student:**
- Email: `student@travelshare.com`
- Password: `password123`

**Administrator:**
- Email: `admin@travelshare.com`
- Password: `admin123`

## ?? Arhitektura

```
Presentation Layer (MVC)
    ?
Controllers (AccountController, HomeController)
    ?
Services (IAuthenticationService, IUserService)
    ?
Models (UserBase, Student, Administrator)
    ?
Data Layer (trenutno Mock, kasnije Database)
```

## ?? Clean Code principi

1. **Single Responsibility** - Svaka klasa ima jednu odgovornost
2. **DRY (Don't Repeat Yourself)** - Nasljeðivanje eliminira duplikaciju
3. **Dependency Injection** - Loose coupling izmeðu komponenti
4. **Interface Segregation** - Mali, fokusirani interfejsi
5. **Meaningful Names** - Jasna imena klasa i metoda
6. **Comments on Complex Logic** - XML dokumentacijski komentari

---

**Autor:** TravelShare Team  
**Datum:** 2024  
**Framework:** ASP.NET Core 8.0 MVC
