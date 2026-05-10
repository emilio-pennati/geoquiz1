# GeoQuiz Nations - Specifica

## Idea Generale
GeoQuiz Nations è un'applicazione educativa che combina dati sui Paesi del mondo con modalità quiz. L'utente può cercare un Paese, studiarne le informazioni principali e mettersi alla prova con domande a risposta multipla.

## Funzionalità Minime
- Ricerca di un Paese per nome
- Scheda informativa con bandiera, capitale, continente, popolazione, lingue
- Quiz a risposta multipla (es. "Qual è la capitale della Francia?")
- Salvataggio del punteggio locale
- Cronologia delle sessioni di quiz

## Funzionalità Avanzate
- Modalità studio (consultazione) e modalità test (quiz cronometrato)
- Quiz filtrati per continente (Europa, Asia, Africa, ecc.)
- Statistiche locali dettagliate (percentuale risposte corrette, tempo medio)
- Paesi preferiti salvati in locale
- Generazione casuale di serie di domande con shuffle delle risposte
- Quiz sulle bandiere (mostrare la bandiera, indovinare il Paese)

## API
REST Countries (https://restcountries.com/) - API gratuita, open source, senza API Key

## Schermate
1. **Search**: Ricerca Paesi (SearchBar, CollectionView)
2. **CountryDetail**: Scheda informativa (ScrollView, Image bandiera, Grid)
3. **Quiz**: Quiz a risposta multipla (Button 4 opzioni, Label, ProgressBar)
4. **Stats**: Statistiche e punteggi (CollectionView, Label)
5. **Settings**: Filtri quiz, reset statistiche (Picker, Switch, Button)

## Requisiti Non Funzionali
- Performace: Risposta API entro 3 secondi
- Affidabilità: Gestione errori offline
- Usabilità: Interfaccia intuitiva per utenti di tutte le età
- Compatibilità: Android 6.0+, iOS 12.0+