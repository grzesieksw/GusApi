
# Biblioteka Gus Api

Przykładowe zastosowanie Api Regon portalu Gus do pobrania podstawowych danych adresowych.
Biblioteka wykorzystuje usługi BIR w wersji 1.1. 

## Jak zacząć?

Jeżeli podstawowe zastosowanie jest wystarczające, wystarczy skompilować projekt i wykorzystać powstały plik GusApi.dll w własnej aplikacji.

### Wymagania

Biblioteka jest domyślnie kompilowana w .net Standard 2.0, więc można ją wykorzystać zarówno w .net core > 2.0 oraz .net Framework > 4.6.1.
Wymagany jest klucz Api, który można uzyskać na stronie Gusu, po przesłaniu wniosku.



### Instalacja

W projekcie wystarczy dodać odwołanie do biblioteki GusApi.dll, oraz jeśli jest to wymagane dodać wymagane zależności.

Zainicjować klasę oraz pobrać potrzebne dane w następujący sposób:
```
ObslugaGus gus = new ObslugaGus();

gus.apiKey = "twojKluczApi";
var podmiot = gus.PobierzDanePodmiotu("twojnip");
```
