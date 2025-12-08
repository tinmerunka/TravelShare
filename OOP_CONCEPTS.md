# ?? OOP Koncepti u TravelShare projektu

## Detaljno objašnjenje implementiranih principa objektno-orijentiranog programiranja

---

## 1?? NASLJEÐIVANJE (Inheritance)

### ?? Definicija
Nasljeðivanje omoguæava kreiranje novih klasa (deriviranih klasa) koje preuzimaju svojstva i metode postojeæih klasa (baznih klasa).

### ?? Implementacija u TravelShare

#### Hijerarhija korisnièkih klasa

```csharp
// Bazna klasa - definira zajednièka svojstva svih korisnika
public abstract class UserBase
{
    // Zajednièka svojstva
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Zajednièke metode
    public virtual string GetDisplayName()
    {
        return $"{FirstName} {LastName}";
    }

    // Apstraktna metoda - mora biti implementirana u deriviranim klasama
    public abstract string GetUserType();
}
```

```csharp
// Derivirana klasa - Student nasljeðuje UserBase
public class Student : UserBase
{
    // Dodatna svojstva specifièna za studenta
    public string StudentId { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public TravelPreferences? Preferences { get; set; }

    // Implementacija apstraktne metode
    public override string GetUserType()
    {
        return "Student";
    }

    // Override postojeæe metode - dodaje studentID
    public override string GetDisplayName()
    {
        return $"{base.GetDisplayName()} ({StudentId})";
    }
}
```

```csharp
// Druga derivirana klasa - Administrator
public class Administrator : UserBase
{
    // Dodatna svojstva specifièna za administratora
    public string Department { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();

    // Implementacija apstraktne metode
    public override string GetUserType()
    {
        return "Administrator";
    }

    // Dodatna metoda specifièna za administratora
    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }
}
```

### ? Prednosti

1. **Eliminacija duplikacije koda**
   - `Email`, `FirstName`, `LastName` nisu ponavljani
   - `GetDisplayName()` samo jednom implementiran

2. **Lakše održavanje**
   - Dodavanje novog svojstva u `UserBase` ? automatski dostupno u svim deriviranim klasama

3. **Jasna hijerarhija**
   - Oèito je da su Student i Administrator tipovi korisnika

4. **Proširivost**
   - Lako dodati nove tipove: `public class Guest : UserBase { ... }`

---

## 2?? POLIMORFIZAM (Polymorphism)

### ?? Definicija
Polimorfizam omoguæava objektima razlièitih klasa da se tretiraju kao objekti zajednièke bazne klase, ali da se ponašaju razlièito prema svojoj konkretnoj implementaciji.

### ?? Implementacija u TravelShare

#### Primjer 1: Virtual metode sa override

```csharp
// U kontroleru - metoda prihvaæa UserBase, ali poziva specificnu implementaciju
private void DisplayUserInfo(UserBase user)
{
    // Ovdje se poziva odgovarajuæa implementacija GetDisplayName()
    // - Ako je user tipa Student ? "Marko Horvat (0246012345)"
    // - Ako je user tipa Administrator ? "Ana Kovaè"
    var displayName = user.GetDisplayName();
    
    // Isto za GetUserType()
    var userType = user.GetUserType(); // "Student" ili "Administrator"
    
    Console.WriteLine($"{userType}: {displayName}");
}

// Korištenje
Student student = new Student { FirstName = "Marko", LastName = "Horvat", StudentId = "0246012345" };
Administrator admin = new Administrator { FirstName = "Ana", LastName = "Kovaè" };

DisplayUserInfo(student); // Output: Student: Marko Horvat (0246012345)
DisplayUserInfo(admin);   // Output: Administrator: Ana Kovaè
```

#### Primjer 2: Polimorfizam u AccountController

```csharp
private void StoreUserInSession(UserBase user)
{
    // user može biti Student ili Administrator
    // Svaka klasa ima svoju implementaciju GetUserType()
    var userType = user.GetUserType(); // Polimorfno pozivanje
    
    var userJson = JsonSerializer.Serialize(user, user.GetType(), options);
    HttpContext.Session.SetString("CurrentUser", userJson);
    HttpContext.Session.SetString("UserType", userType);
}

// Korištenje sa razlièitim tipovima
Student student = /* ... */;
Administrator admin = /* ... */;

StoreUserInSession(student); // Radi sa Student
StoreUserInSession(admin);   // Radi sa Administrator
```

#### Primjer 3: Polimorfizam u AuthenticationProvider

```csharp
// Bazna klasa
public abstract class AuthenticationProvider
{
    public abstract string ProviderName { get; }
    
    // Template method - koristi polimorfne metode
    public async Task<AuthenticationResult> AuthenticateAsync(string identifier, string credential)
    {
        // Poziva se konkretna implementacija ValidateCredentialsAsync
        if (!await ValidateCredentialsAsync(identifier, credential))
            return AuthenticationResult.Failed("Invalid credentials");
        
        // Poziva se konkretna implementacija GetUserAsync
        var user = await GetUserAsync(identifier);
        
        return AuthenticationResult.Success(user);
    }
    
    protected abstract Task<bool> ValidateCredentialsAsync(string identifier, string credential);
    protected abstract Task<UserBase?> GetUserAsync(string identifier);
}

// Konkretna implementacija
public class EmailAuthenticationProvider : AuthenticationProvider
{
    public override string ProviderName => "Email";
    
    // Specifièna implementacija za email autentifikaciju
    protected override Task<bool> ValidateCredentialsAsync(string identifier, string credential)
    {
        // Email-specific validacija
        if (_mockUsers.TryGetValue(identifier, out var userData))
        {
            return Task.FromResult(userData.password == credential);
        }
        return Task.FromResult(false);
    }
    
    protected override Task<UserBase?> GetUserAsync(string identifier)
    {
        // Email-specific dohvaæanje korisnika
        /* ... */
    }
}
```

### ? Prednosti

1. **Fleksibilnost**
   - Ista metoda radi sa razlièitim tipovima
   - Lako dodavanje novih tipova bez mijenjanja postojeæeg koda

2. **Ponovno korištenje koda**
   - `DisplayUserInfo()` radi za sve tipove korisnika

3. **Održivost**
   - Dodavanje novog tipa korisnika ne zahtjeva promjene u kontrolerima

---

## 3?? APSTRAKCIJA (Abstraction)

### ?? Definicija
Apstrakcija sakriva kompleksnu implementaciju i pokazuje samo bitne karakteristike objekta.

### ?? Implementacija u TravelShare

#### Primjer 1: Apstraktna bazna klasa

```csharp
public abstract class UserBase
{
    // Konkretna svojstva - svi korisnici ih imaju
    public int Id { get; set; }
    public string Email { get; set; }
    
    // Apstraktna metoda - MORA biti implementirana
    // Ne definiramo kako, samo ŠTO mora postojati
    public abstract string GetUserType();
    
    // Virtual metoda - CAN BE overridden
    public virtual string GetDisplayName()
    {
        return $"{FirstName} {LastName}";
    }
}

// ? NE MOŽEŠ: var user = new UserBase(); // Compiler error!
// ? MOŽEŠ: var user = new Student();
```

#### Primjer 2: Interface apstrakcija

```csharp
// Interface definira UGOVOR - što mora postojati, ne i kako
public interface IAuthenticationService
{
    // Deklaracija metoda bez implementacije
    Task<AuthenticationResult> AuthenticateAsync(string email, string password);
    Task<bool> RegisterUserAsync(Student student, string password);
    void SetCurrentUser(UserBase user);
    UserBase? GetCurrentUser();
    void Logout();
}

// Konkretna implementacija
public class MockAuthenticationService : IAuthenticationService
{
    // MORA implementirati sve metode iz interface-a
    public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
    {
        /* Konkretna implementacija */
    }
    
    /* ... ostale metode ... */
}
```

#### Primjer 3: Apstrakcija u kontroleru

```csharp
public class AccountController : Controller
{
    // Kontroler zna o interface-u, ne o konkretnoj implementaciji
    private readonly IAuthenticationService _authService;
    private readonly IUserService _userService;
    
    public AccountController(IAuthenticationService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }
    
    // Koristi apstraktni interface, ne konkretnu klasu
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Ne zna (niti treba znati) da li je Mock, Database, ili API implementacija
        var result = await _authService.AuthenticateAsync(model.Email, model.Password);
        /* ... */
    }
}
```

### ? Prednosti

1. **Skrivanje kompleksnosti**
   - Kontroler ne zna kako `_authService` radi interno
   - Samo zna da može pozvati `AuthenticateAsync()`

2. **Zamjenjivost implementacija**
   ```csharp
   // Trenutno: Mock
   builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
   
   // Kasnije: Stvarna database implementacija
   builder.Services.AddScoped<IAuthenticationService, DatabaseAuthenticationService>();
   
   // Kontroler ostaje isti! ?
   ```

3. **Testabilnost**
   ```csharp
   // U unit testu možemo koristiti mock
   var mockAuthService = new Mock<IAuthenticationService>();
   var controller = new AccountController(mockAuthService.Object, ...);
   ```

---

## 4?? ENKAPSULACIJA (Encapsulation)

### ?? Definicija
Enkapsulacija sakriva interne detalje objekta i izlaže samo ono što je potrebno kroz javne metode i svojstva.

### ?? Implementacija u TravelShare

#### Primjer 1: Private fields sa public properties

```csharp
public class AuthenticationResult
{
    // Private setteri - ne može se mijenjati izvana
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public UserBase? User { get; private set; }
    
    // Private konstruktor - forsira korištenje factory metoda
    private AuthenticationResult() { }
    
    // Public factory metode - kontroliran naèin kreiranja
    public static AuthenticationResult Success(UserBase user)
    {
        return new AuthenticationResult
        {
            IsSuccess = true,
            User = user
        };
    }
    
    public static AuthenticationResult Failed(string errorMessage)
    {
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}

// ? Ispravan naèin:
var result = AuthenticationResult.Success(user);

// ? NE MOŽEŠ:
// var result = new AuthenticationResult();
// result.IsSuccess = true; // Compiler error!
```

#### Primjer 2: Private metode u kontroleru

```csharp
public class AccountController : Controller
{
    // Private fields - sakriveni od vanjskog svijeta
    private readonly IAuthenticationService _authService;
    private readonly IUserService _userService;
    
    // Public konstruktor - DI container može pristupiti
    public AccountController(IAuthenticationService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }
    
    // Public metoda - pristupaèna preko HTTP-a
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        /* ... */
        StoreUserInSession(result.User); // Poziva private metodu
        /* ... */
    }
    
    // Private helper metoda - interna logika, ne izložena izvana
    private void StoreUserInSession(UserBase user)
    {
        var userJson = JsonSerializer.Serialize(user, user.GetType(), options);
        HttpContext.Session.SetString("CurrentUser", userJson);
    }
    
    // Private helper metoda
    private UserBase? GetCurrentUser()
    {
        /* ... */
    }
}
```

#### Primjer 3: Protected metode u baznoj klasi

```csharp
public abstract class AuthenticationProvider
{
    // Protected - dostupno deriviranim klasama, ali ne izvana
    protected abstract Task<bool> ValidateCredentialsAsync(string identifier, string credential);
    protected abstract Task<UserBase?> GetUserAsync(string identifier);
    
    // Protected virtual - može se override-ati
    protected virtual async Task LogAuthenticationAsync(UserBase user)
    {
        user.LastLoginAt = DateTime.UtcNow;
        await Task.CompletedTask;
    }
    
    // Public - dostupno svima
    public async Task<AuthenticationResult> AuthenticateAsync(string identifier, string credential)
    {
        /* ... koristi protected metode ... */
    }
}
```

### ? Prednosti

1. **Kontrola pristupa**
   - Private: Samo unutar klase
   - Protected: Klasa i njene derivirane klase
   - Public: Svi

2. **Sigurnost podataka**
   ```csharp
   // Ne može se promijeniti izvana
   public bool IsSuccess { get; private set; }
   ```

3. **Lakše održavanje**
   - Private metode mogu se mijenjati bez utjecaja na vanjski kod

4. **Validacija**
   ```csharp
   private decimal _budget;
   public decimal Budget
   {
       get => _budget;
       set
       {
           if (value < 0)
               throw new ArgumentException("Budget cannot be negative");
           _budget = value;
       }
   }
   ```

---

## ?? DESIGN PATTERNS u projektu

### 1. Template Method Pattern

**Gdje:** `AuthenticationProvider`

**Zašto:** Definira skeleton algoritma, konkretne klase implementiraju detalje

```csharp
public abstract class AuthenticationProvider
{
    // Template method - fiksna struktura
    public async Task<AuthenticationResult> AuthenticateAsync(string id, string cred)
    {
        // Korak 1: Validacija
        if (!await ValidateCredentialsAsync(id, cred))
            return AuthenticationResult.Failed("Invalid");
        
        // Korak 2: Dohvati korisnika
        var user = await GetUserAsync(id);
        
        // Korak 3: Logiraj
        await LogAuthenticationAsync(user);
        
        // Korak 4: Vrati rezultat
        return AuthenticationResult.Success(user);
    }
    
    // Hook metode - derivirane klase definiraju implementaciju
    protected abstract Task<bool> ValidateCredentialsAsync(...);
    protected abstract Task<UserBase?> GetUserAsync(...);
}
```

### 2. Factory Pattern

**Gdje:** `AuthenticationResult`

**Zašto:** Kontroliran naèin kreiranja objekata

```csharp
public static AuthenticationResult Success(UserBase user)
{
    return new AuthenticationResult { IsSuccess = true, User = user };
}

public static AuthenticationResult Failed(string error)
{
    return new AuthenticationResult { IsSuccess = false, ErrorMessage = error };
}
```

### 3. Dependency Injection Pattern

**Gdje:** Program.cs i kontroleri

**Zašto:** Loose coupling, testabilnost

```csharp
// Registracija
builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();

// Injekcija
public AccountController(IAuthenticationService authService)
{
    _authService = authService;
}
```

---

## ? SOLID Principi

### S - Single Responsibility Principle
**Svaka klasa ima jednu odgovornost**

- `UserBase` - Definira korisnika
- `AuthenticationProvider` - Bavi se autentifikacijom
- `AccountController` - Upravlja HTTP zahtjevima za account

### O - Open/Closed Principle
**Otvoreno za proširenje, zatvoreno za modifikaciju**

- Novi tip korisnika: `public class Guide : UserBase { }` ?
- Ne mijenjamo `UserBase` ?

### L - Liskov Substitution Principle
**Derivirane klase mogu zamijeniti baznu klasu**

```csharp
UserBase user = new Student(); // ?
user = new Administrator();     // ?
DisplayUserInfo(user);          // Radi za oba ?
```

### I - Interface Segregation Principle
**Mali, fokusirani interfejsi**

- `IAuthenticationService` - Samo autentifikacija
- `IUserService` - Samo user operations
- Ne jedan veliki `IUserManagementService` ?

### D - Dependency Inversion Principle
**Ovisi o apstrakcijama, ne o konkretnim klasama**

```csharp
// ? Ovisnost o interface-u
private readonly IAuthenticationService _authService;

// ? Ovisnost o konkretnoj klasi
// private readonly MockAuthenticationService _authService;
```

---

**Zakljuèak:** TravelShare projekt demonstrira sva èetiri kljuèna OOP principa kroz èistu, održivu i proširivu arhitekturu! ??
