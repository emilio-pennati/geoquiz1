# GeoQuiz Nations - Matrice di Test

## Test Cases

### Search
| ID | Caso di Test | Risultato Atteso | Esito |
|----|--------------|------------------|-------|
| S1 | Ricerca per nome esatto | Paese trovato | - |
| S2 | Ricerca con caratteri parziali | Lista risultati | - |
| S3 | Ricerca nessun risultato | Messaggio "Nessun Paese trovato" | - |
| S4 | Ricerca con input vuoto | Lista completa Paesi | - |

### CountryDetail
| ID | Caso di Test | Risultato Atteso | Esito |
|----|--------------|------------------|-------|
| D1 | Visualizzazione bandiera | Immagine visibile | - |
| D2 | Dati mancanti (es. lingue) | N/A visualizzato | - |
| D3 | Paese aggiunto a preferiti | Salvato in locale | - |

### Quiz
| ID | Caso di Test | Risultato Atteso | Esito |
|----|--------------|------------------|-------|
| Q1 | Risposta corretta | Punteggio aumentato | - |
| Q2 | Risposta errata | Nessun punto, mostra正确答案 | - |
| Q3 | Fine quiz | Mostra punteggio finale | - |
| Q4 | Quiz cronometrato | Timer funziona | - |

### Stats
| ID | Caso di Test | Risultato Atteso | Esito |
|----|--------------|------------------|-------|
| ST1 | Statistiche vuote | Messaggio "Nessuna sessione" | - |
| ST2 | Cronologia visualizzata | Lista sessioni passate | - |
| ST3 | Reset statistiche | Dati cancellati | - |