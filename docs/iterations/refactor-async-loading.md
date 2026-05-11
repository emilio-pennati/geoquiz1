# Refactor: Async Loading

## Objective
Risoluzione problema caricamento bloccante quando si cambia tab

## Problem
- Il caricamento dei paesi avveniva in ContentPage_Loaded
- Ogni cambio tab causava un nuovo caricamento
- La UI non rispondeva durante il caricamento

## Solution
- SearchViewModel da Transient a Singleton
- Caricamento dati nel costruttore del ViewModel
- Rimosso caricamento da ContentPage_Loaded

## Files Modified
- GeoQuiz/MauiProgram.cs - SearchViewModel da AddTransient a AddSingleton
- GeoQuiz/ViewModels/SearchViewModel.cs - _ = LoadCountriesAsync() nel costruttore
- GeoQuiz/Views/SearchPage.xaml.cs - rimosso ContentPage_Loaded

## Tests Executed
- [x] Build: dotnet build - completato con successo

## Outcome
I dati vengono caricati una sola volta all'avvio e persistono quando si cambia tab