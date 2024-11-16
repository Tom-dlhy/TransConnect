using Projet_final;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;




Entreprise TransConnect = new Entreprise("TransConnect");

TransConnect.Admin("Identifiants.csv");
Console.WriteLine("Indentifiant : ESILV // Mot de passe : toto");

Console.Write("Identifiant: ");
string identifiant = Console.ReadLine();
Console.Write("Mot de passe: ");
string mot_de_passe = Console.ReadLine();

if(TransConnect.Identifiants[0] == identifiant && TransConnect.Identifiants[1] == mot_de_passe)
{
    TransConnect.ListeSalariés("ListeSalariés.csv");
    TransConnect.ListeClients("ListeClients.csv");
    TransConnect.ListeVéhicules("ListeVéhicules.csv"); 
    TransConnect.ListeChefEquipe("ListechefEquipe.csv");
    TransConnect.ListeChauffeurs("ListeChauffeurs.csv");
    TransConnect.ListeVilles("ListeVilles.csv");
    TransConnect.ListeTrajets("ListeTrajets.csv");
    TransConnect.ListeCommandes("ListeCommandes.csv");

    Console.WriteLine("Connexion réussie");
    Thread.Sleep(1000);
    TransConnect.MenuPrincipal();
}
else
{
    Console.Clear();
    Console.WriteLine("Identifiant ou Mot de passe incorect");
}





