# GeoQuiz Nations - Note API

## REST Countries API

**Base URL**: `https://restcountries.com/v3.1`

**Autenticazione**: Nessuna (gratuita, open source)

**Rate Limit**: Nessun limite significativo documentato

## Endpoint Utilizzati

| Endpoint | Scopo | Esempio |
|----------|-------|---------|
| GET /v3.1/all | Tutti i Paesi (~250) | - |
| GET /v3.1/name/{name} | Ricerca per nome | /v3.1/name/italy |
| GET /v3.1/region/{region} | Paesi per continente | /v3.1/region/europe |
| GET /v3.1/alpha/{code} | Paese per codice ISO | /v3.1/alpha/IT |

## Campi Utilizzati
- `name.common` - Nome comune
- `capital` - Capitale (array)
- `region` / `continents` - Continente
- `population` - Popolazione
- `languages` - Lingue (oggetto key-value)
- `flags.png` - URL bandiera
- `cca2` - Codice ISO 2 lettere

## Gestione Errori
- 404: Paese non trovato
- Timeout: 10 secondi
- Offline: Cache locale opzionale