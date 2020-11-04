# Site de e-commerce (Application web & API) en ASP.NET Core

Le projet est une boutique e-shop avec un parcours d’achat complet (mise au panier, sélection de l'adresse d’envoi) avec les tables suivantes : Utilisateur, Adresse, Produit, Panier, Role, Categorie.

Le projet est composé de deux parties :

   - Une API web, permettant de manipuler un ensemble de données cohérent.
   
   - Une application web utilisant cette API.

Les fonctionnalités implémentées sont :

   - Un système d'authentification complet avec formulaire d'inscription, de connexion et de gestion des comptes.
   
   - Un utilisateur avec le ROLE_ADMIN doit avoir accès à des sections que les autres utilisateurs ne voient pas (ROLE_USER).

   - Une interface permettant de manipuler (ajouter, afficher, éditer, supprimer) des données de votre API au travers de l'application et en fonction de l'utilisateur connecté et de son rôle.

   - Un parcours d’achat de la mise au panier à la validation d’une commande.

   - Une fonctionnalité de recherche de Produit par Catégorie.
   
   - Un système de pagination :
       - Au niveau de l'API permettant de récupérer une partie des données (en utilisant des paramètres lors de la requête GET).
       - Au niveau de l'application pour permettre l'affichage d'une partie des données.

Technologies utilisées : C#, Microsoft Visual Studio, ASP.NET Core (Identity, Session, SqlServer), EntityFrameworkCore, Model View Controller (MVC), Newtonsoft.Json, Linq, PagedList.
