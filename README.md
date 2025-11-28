# Semestrální práce BDAS2 ------- SK LHOTA
<br>
<br>

## Základní informace 📖


<p>
    Tento projekt byl vytvořen jako školní týmový projekt v rámci předmětu BDAS2 (Databázové systémy II). 
    Jedná se o databázovou aplikaci, která slouží jako <b>informační systém sportovního klubu SK Lhota</b>
</p>


**Škola:** UPCE FEI

**Obor:** IT (Informační technologie)

**Rok:** 2025

---
<br>

## Informační systém SK Lhota ⚽


Databázová aplikace převážně slouží ke správě <b>trenérů, hráčů, tréninků, kontraktů a sponzorů</b>. Aplikace také umožňuje zobrazování výsledků odehraných zápasů a také zobrazovat nadcházející zápasy.

Cílem aplikace je usnadnit správu členské základny, sponzorů a sledování hráčských kontraktů.


![alt text][logo]

[logo]: https://github.com/BDAS2-tym/BDAS2---Semestr-ln-pr-ce---SK-LHOTA/blob/main/BDAS2_Sem_Prace-Cincibus_Tluchor/Images/Img/Logo_SK_Lhota.png

---
<br>

## Použité technologie 💻

| Oblast | Technologie |
|---------|--------------|
| Databáze | Oracle |
| Backend | C# (.NET 8)|
| Frontend | WPF (Windows Presentation Foundation) |
| Návrh databáze | SQL Datamodeler |
| Verzování | Git + GitHub |

---
<br>

## Funkcionality ⚙️


- 🟨 Přihlášení pomocí uživatelských účtů
- 🟨 Evidence členů
- 🟨 Evidence sponzorů
- 🟨 Evidence hračských kontraktů
- 🟨 Zobrazení zápasů a jejich výsledků
- 🟨 Vyhledávání a filtrování údajů
- 🟨 Export/Import kontraktů do formátu PDF
- 🟨 Správa binárních souborů

---
<br>

## Datový model 🗃️


### Hlavní entity
| Entita                   | Atributy                                                                                          |
|---------------------------|---------------------------------------------------------------------------------------------------|
| **_Clen_Klubu_**            | IdClenKlubu, RodneCislo, Jmeno, Prijmeni, TypClena, TelefonniCislo                              |
| **_Trener_**                | TrenerskaLicence, PocetLetPraxe, Specializace                                                    |
| **_Hrac_**                  | PoziceNaHristi, PocetVstrelenychGolu, PocetZlutychKaret, PocetCervenychKaret                     |
| **_Trenink_**               | IdTrenink, Datum, Misto, Popis                                                                   |
| **_Disclipinarni_Opatreni_**| IdDisclipinarniOpatreni, DatumOpatreni, DelkaTrestu, Duvod                                       |
| **_Kontrakt_**              | DatumZacatku, DatumKonce, Plat, CisloNaAgenta, VystupniKlazule                                   |
| **_Uzivatelsky_Ucet_**      | IdUzivatelskyUcet, UzivatelskeJmeno, Email, Heslo, PosledniPrihlaseni                            |
| **_Log_Table_**             | IdLog, Operace, Cas, Uzivatel, Tabulka                                                           |
| **_Sponzor_**               | IdSponzor, Jmeno, SponzorovanaCastka                                                             |
| **_Soutez_**                | IdSoutez, StartDatum, KonecDatum                                                                 |
| **_Zapas_**                 | IdZapas, Datum                                                                                    |
| **_Vysledek_Zapasu_**       | Vysledek, PocetZlutychKaret, PocetCervenychKaret, PocetGolyDomaciTym, PocetGolyHosteTym         |

<br>


### Číselníky
| Číselník       | Atributy                        |
|----------------|--------------------------------|
| **_Pozice_Hrac_**    | IdPozice, NazevPozice          |
| **_Role_**           | IdRole, NazevRole              |
| **_Typ_Soutez_**     | IdTypSouteze, NazevSouteze     |
| **_Stav_Zapas_**    | IdStav, StavZapasu             |

<br>


### ERD diagram
_TODO ERD Doplnit!!_

---
<br>

## Instalace a spuštění 🚀
_TODO Instalace aplikace_ 

---
<br>

## Autoři 🧑‍💻


* Aleš Tlučhoř
* Milan Cincibus
---
<br>

## Licence 📜


Tento projekt byl vytvořen jako **školní práce**.  
Projekt je určen výhradně pro **nekomerční a vzdělávací účely**.

**_© 2025 – Cincibus & Tlučhoř_**
