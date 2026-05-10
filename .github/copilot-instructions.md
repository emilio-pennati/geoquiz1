# GitHub Copilot - GeoQuiz Nations

## Istruzioni per Copilot

Questo progetto usa il workflow Man-in-the-Loop:
1. Prima pianifica, poi implementa
2. Ogni iterazione ha un obiettivo verificabile
3. Codice generato deve essere rivisto
4. Testa sempre su device/emulator
5. Documenta ogni iterazione

## Stack
- .NET MAUI
- MVVM con CommunityToolkit.Mvvm
- Shell navigation

## Regole
- Non usare ListView, usa CollectionView
- Servizi iniettati nel costruttore
- Niente logica in code-behind
- Gestisci sempre HttpRequestException, JsonException, TaskCanceledException

## File chiave
- `docs/spec.md` - Specifica completa
- `docs/plan.md` - Piano iterazioni
- `docs/architecture.md` - Architettura tecnica