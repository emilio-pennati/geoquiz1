# GeoQuiz Nations - Architettura

## Pattern Architetturale
MVVM (Model-View-ViewModel) con CommunityToolkit.Mvvm

## Struttura Progetto
```
src/
├── Models/           # DTO e modelli dati
├── Services/         # Logica di business e API
├── ViewModels/       # ViewModels per ogni schermata
├── Views/            # XAML e code-behind
├── Converters/       # Convertitori XAML
└── Resources/        # Risorse statiche
```

## Stack Tecnologico
- .NET MAUI (cross-platform)
- CommunityToolkit.Mvvm
- System.Net.Http per REST
- SQLite per persistenza locale

## Flusso Dati
1. ViewModels espongono dati via ObservableProperty
2. Services chiamano API REST Countries
3. Risultati trasformati in modelli locali
4. ViewBinding espone stati (IsBusy, ErrorMessage, HasData, IsEmptyState)

## Navigation
Shell navigation con route e query parameters

## Dependency Injection
Constructor injection per Services e ViewModels in MauiProgram.cs