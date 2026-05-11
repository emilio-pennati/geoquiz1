# GeoQuiz Nations - Piano di Lavoro

## Iterazioni Previste

### Iterazione 1: Setup Progetto
- Creazione progetto .NET MAUI
- Configurazione MVVM con CommunityToolkit.Mvvm
- Setup struttura folder (Models, Services, ViewModels, Views)
- Configurazione dipendenze

### Iterazione 2: Servizio API
- Implementazione CountryService con REST Countries
- Modelli dati per Country, Quiz, Stats
- Gestione errori HTTP

### Iterazione 3: Schermata Search
- Implementazione SearchPage con SearchBar e CollectionView
- ViewModel con logica ricerca
- Binding dati bandiere

### Iterazione 4: Schermata CountryDetail
- Implementazione CountryDetailPage
- Visualizzazione bandiera, capitale, popolazione, lingue
- Modalità studio

### Iterazione 5: Quiz Base
- Implementazione QuizPage
- Generazione domande casuali
- 4 opzioni di risposta
- Scoring locale

### Iterazione 6: Quiz Cronometrato
- Timer per quiz
- Modalità timed vs relax
- Timeout per risposta

### Iterazione 7: Filtri per Continente
- Selezione continente nella search
- Quiz filtrato per regione
- Filtro anche in CountryDetail

### Iterazione 8: Quiz Bandiere
- Domande con immagine bandiera
- "Quale paese ha questa bandiera?"
- Shuffle opzioni

### Iterazione 9: Paesi Preferiti
- Aggiungi/rimuovi preferiti
- Salvataggio locale
- Filtro "solo preferiti"

### Iterazione 10: Statistiche Base
- Punteggi totali
- Numero quiz completati
- Percentuale corrette

### Iterazione 11: Persistenza e Stats Avanzate
- SQLite per salvataggio punteggi
- Cronologia sessioni
- Statistiche utente dettagliate

## Rischi
- Rate limit API
- Dati Paesi mancanti o inconsistenti
- Gestione offline