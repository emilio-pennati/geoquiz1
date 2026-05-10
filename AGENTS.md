# Regole per AI Agents - GeoQuiz Nations

Questo progetto segue il workflow **Man-in-the-Loop** con 5 fasi:

## Fasi del Workflow

1. **Planning** - Definire obiettivo, piano, branch prima di scrivere codice
2. **Build** - Implementare solo ciò che nel piano
3. **Review** - Leggere e verificare tutto il codice generato
4. **Testing** - Testare su emulator/device
5. **Doc & Git** - Documentare e committare

## Regole
- Mai generare grandi blocchi di codice senza richiesta
- Proporre sempre un piano prima di cambiamenti ampi
- Limitare ogni iterazione a una feature ben definita
- Non introdurre NuGet senza giustificazione
- Mai mettere logica in code-behind
- Sempre gestire stati: IsBusy, ErrorMessage, HasData, IsEmptyState

## Struttura Docs
- `docs/spec.md` - Specifica funzionale
- `docs/plan.md` - Piano di lavoro
- `docs/architecture.md` - Architettura
- `docs/test-matrix.md` - Test
- `docs/iterations/it-XX.md` - Log iterazioni

Vedi skill `man-in-the-loop-workflow` per dettagli completi.