
[README.txt](https://github.com/user-attachments/files/24959247/README.txt)
Celegreety � Talent Pricing API

Opis projekta:
Celegreety9 je ASP.NET Core Web API aplikacija za upravljanje korisnicima i cijenama talenata.
Aplikacija omogu?uje registraciju korisnika, spremanje i dohvat podataka o cijenama talenata te pra?enje povijesti promjena cijena. Tako?er je integrirana sa Stripe API-jem za kreiranje proizvoda i cijena.
Projekt koristi PostgreSQL bazu podataka, a za pristup bazi koristi se Dapper. Poslovna logika za rad s cijenama dijelom je implementirana kroz PostgreSQL funkcije.
Tehnolo�ki stack
Backend:
* .NET 9 (ASP.NET Core Web API)
* Dapper
* MediatR (CQRS � Commands i Queries)
* Stripe.net SDK
Baza podataka:
* PostgreSQL
* PostgreSQL funkcije (PL/pgSQL)
Ostalo:
* Stripe API
* OpenAPI / Swagger
Struktura projekta
Controllers:
* UsersController
* TalentPricingController
Features:
* Users
o Interfaces (IUserRepository)
o UserRepository
o Models (User)
* TalentPricings
o Commands (CreateTalentPricingCommand, UpdateTalentPricingCommand)
o Queries (GetTalentPricingQuery)
o Interfaces (ITalentPricingRepository)
o Models (TalentPricing, PricingHistory, TalentProfile)
o Service (StripeService)
o TalentPricingRepository
Ostalo:
* Program.cs
* appsettings.json
* launchSettings.json
Baza podataka
Projekt koristi sljede?e tablice:
* users
* talent_profiles
* pricing_history
Kori�tene PostgreSQL funkcije:
* fn_register_user
* fn_get_user_by_email
* fn_upsert_talent_pricing
* fn_insert_pricing_history
* fn_get_talent_pricing_with_history
Funkcionalnosti
Users:
* Registracija korisnika putem PostgreSQL funkcije
* Dohvat korisnika po email adresi
Talent Pricing:
* Kreiranje Stripe proizvoda za talent
* Kreiranje Stripe cijena (personal i business)
* Spremanje cijena u bazu
* Spremanje povijesti promjena cijena
* Dohvat trenutnih cijena i povijesti promjena
Validacija:
* Business cijena mora biti ve?a ili jednaka personal cijeni
API rute
Users:
* POST /api/users
* GET /api/users/by-email
Talent Pricing:
* POST /api/talentpricing
* PUT /api/talentpricing
* GET /api/talentpricing/{talentId}
* GET /api/talentpricing/ping
Pokretanje projekta
Preduvjeti:
* .NET SDK 9
* PostgreSQL
* pgAdmin 4
* Stripe test API key
Postavljanje baze:
* Kreirati bazu u PostgreSQL
* Izvr�iti SQL skripte za tablice i funkcije
* Postaviti connection string u appsettings.json
Primjer connection stringa:
Host=localhost;Port=5432;Database=celegreety;Username=postgres;Password=lozinka
Pokretanje:
* Otvoriti projekt u Visual Studio
* Pokrenuti aplikaciju
Aplikacija se pokre?e na:
http://localhost:5080
Napomena
Projekt je razvijen kao backend aplikacija s naglaskom na rad s PostgreSQL bazom, Dapper ORM-om, integraciju Stripe API-ja te jasnu organizaciju koda kroz Controller, Repository i Service slojeve.


Iva Vozab

