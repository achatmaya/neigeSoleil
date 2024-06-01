# Projet de gestion de réservations d'appartements

Ce projet est une application ASP.NET pour la gestion des réservations d'appartements et des contrats de gestion. Il permet aux utilisateurs de réserver des appartements et aux propriétaires de renouveler leurs contrats de gestion.

## Prérequis

Avant de commencer, assurez-vous d'avoir les éléments suivants installés sur votre machine :

- Visual Studio 2019+
- .NET 5.0+ SDK
- MySQL
- Postman ou Insomnia pour tester les API

## Configuration de la base de données

1. Installez MySQL et démarrez le service MySQL.
2. Créez une base de données pour le projet :

```sql
CREATE DATABASE quest_web;
```

## Configuration de l'application

1. Clonez le dépôt :

```bash
git clone https://github.com/votre-utilisateur/votre-repo.git
cd votre-repo
```

2. Ouvrez le projet dans Visual Studio.

3. Configurez la chaîne de connexion à la base de données dans `appsettings.json` :

```json
{
"ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=quest_web;User=root;Password=votre-mot-de-passe;"
}
}
```

## Création des migrations et mise à jour de la base de données

1. Ouvrez une ligne de commande dans le répertoire du projet.

2. Ajoutez une migration initiale :

```bash
dotnet tool install --global dotnet-ef --version=6
```

```bash
dotnet ef migrations add InitialCreate
```

3. Appliquez les migrations pour créer les tables dans la base de données :

```bash
dotnet ef database update
```

## Exécution de l'application

1. Exécutez l'application depuis Visual Studio en appuyant sur `F5` ou en utilisant la commande suivante :

```bash
dotnet run
```

L'application sera accessible à l'adresse `https://localhost:5001` pour HTTPS et `http://localhost:5000` pour HTTP.

## Documentation des API

- [Fichier Insomnia](./doc/Insomnia_ApiRestEtna.json)


## Tests

Utilisez Postman ou Insomnia pour tester les différentes API en envoyant des requêtes aux endpoints définis. Assurez-vous d'avoir un token JWT valide pour les endpoints protégés par l'authentification.

## Contribution

Les contributions sont les bienvenues ! Veuillez ouvrir une issue pour discuter des changements que vous souhaitez apporter.

## Licence

Ce projet est sous licence MIT. Voir le fichier `LICENSE` pour plus de détails.
