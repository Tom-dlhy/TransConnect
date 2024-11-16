using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using System.Data;

namespace Projet_final
{
    internal class Entreprise : Interface
    {
        private string nom;
        private List<Salarié> liste_salariés;
        private List<Client> liste_clients;
        private List<Commande> liste_commandes;
        private List<Chauffeur> liste_chauffeurs;
        private List<Chef_equipe> liste_chefequipe;
        private List<Véhicule> liste_véhicules;
        private List<Ville> liste_villes;
        private List<Trajet> liste_trajets;
        private Noeud organigramme;
        private string[] identifiants;

        public Entreprise(string nom)
        {
            this.nom = nom;
            this.liste_salariés = new List<Salarié>();
            this.liste_clients = new List<Client>();
            this.liste_commandes = new List<Commande>();
            this.liste_chauffeurs = new List<Chauffeur>();
            this.liste_véhicules = new List<Véhicule>();
            this.liste_chefequipe = new List<Chef_equipe>();
            this.liste_villes = new List<Ville>();
            this.liste_trajets = new List<Trajet>();
            this.identifiants = new string[2];
            foreach (Salarié salarié in liste_salariés)
            {
                if (salarié.Poste == "Directeur Général")
                {
                    organigramme = new Noeud(salarié);
                }
                break;
            }
        }

        #region Getters et Setters

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public List<Salarié> Liste_salariés
        {
            get { return liste_salariés; }
            set { liste_salariés = value; }
        }

        public void AfficherSalariés()
        {
            foreach (Salarié salarié in liste_salariés)
            {
                Console.WriteLine(salarié.NSS + " " + salarié.Prenom + " " + salarié.Nom);
            }
        }

        public List<Client> Liste_clients
        {
            get { return liste_clients; }
            set { liste_clients = value; }
        }

        public void AfficherClients()
        {
            foreach (Client client in liste_clients)
            {
                Console.WriteLine(client.ShortString());
            }
        }

        public List<Commande> Liste_commandes
        {
            get { return liste_commandes; }
            set { liste_commandes = value; }
        }

        public List<Chauffeur> Liste_chauffeurs
        {
            get { return liste_chauffeurs; }
            set { liste_chauffeurs = value; }
        }

        public List<Chef_equipe> Liste_chef_equipe
        {
            get { return liste_chefequipe; }
            set { liste_chefequipe = value; }
        }

        public List<Ville> Liste_villes
        {
            get { return liste_villes; }
            set { liste_villes = value; }
        }

        public List<Trajet> Liste_trajets
        {
            get { return liste_trajets; }
            set { liste_trajets = value; }
        }

        public Noeud Organigramme
        {
            get { return organigramme; }
            set { organigramme = value; }
        }

        public List<Véhicule> Liste_véhicules
        {
            get { return liste_véhicules; }
            set { liste_véhicules = value; }
        }

        public string[] Identifiants
        {
            get { return identifiants; }
            set { identifiants = value; }
        }


        #endregion

        #region Affichage

        /// <summary>
        /// Affiche la liste des chefs d'équipe
        /// </summary>

        public void AfficherChefEquipe()
        {
            foreach (Chef_equipe chef in liste_chefequipe)
            {
                Console.WriteLine(chef.ShortString());
            }
        }

        /// <summary>
        /// Affiche la liste des villes proposées
        /// </summary>

        public void AfficherVilles()
        {
            foreach (Ville ville in liste_villes)
            {
                Console.WriteLine(ville.Nom);
            }
        }


        #endregion

        #region Lecture des fichiers

        /// <summary>
        /// Lis le fichier contenant les identifiants de l'administrateur
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void Admin(string filepath)
        {
            var lines = File.ReadAllLines(filepath);

            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                identifiants[0] = values[0];
                identifiants[1] = values[1];
            }
        }

        /// <summary>
        /// Lis le fichier contenant les salariés
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeSalariés(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_salariés = new List<Salarié>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                Salarié new_salarié = new Salarié(values[0], values[1], values[2], DateTime.Parse(values[3]), values[4], values[5], values[6], DateTime.Parse(values[7]), values[8], Convert.ToDouble(values[9]), values[10]);
                AjouterSalarié(new_salarié);
            }
        }

        /// <summary>
        /// Lis le fichier contenant les chauffeurs
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeChauffeurs(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_chauffeurs = new List<Chauffeur>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                if (values[2] == "" && values[3] == "")         // pas de chef d'équipe ni de dates
                {
                    Chauffeur new_chauffeur = new Chauffeur(Convert.ToInt32(values[0]), liste_salariés.Find(x => x.NSS == values[1]));
                    AjouterChauffeur(new_chauffeur);
                }
                else if (values[2] != "" && values[3] == "")    // chef d'équipe mais pas de dates
                {
                    Chef_equipe chef = liste_chefequipe.Find(x => x.NSS == values[2]);

                    Chauffeur new_chauffeur = new Chauffeur(Convert.ToInt32(values[0]), liste_salariés.Find(x => x.NSS == values[1]), chef);
                    chef.AjouterChauffeur(new_chauffeur);
                    AjouterChauffeur(new_chauffeur);
                }
                else if (values[2] == "" && values[3] != "")    // pas de chef d'équipe mais dates
                {
                    List<DateTime> dates = new List<DateTime>();
                    for (int j = 3; j < values.Count(); j++)
                    {
                        dates.Add(DateTime.Parse(values[j]));
                    }
                    Chauffeur new_chauffeur = new Chauffeur(Convert.ToInt32(values[0]), liste_salariés.Find(x => x.NSS == values[1]), dates);
                    AjouterChauffeur(new_chauffeur);
                }
                else                            //chef d'équipe et dates
                {
                    Chef_equipe chef = liste_chefequipe.Find(x => x.NSS == values[2]);
                    List<DateTime> dates = new List<DateTime>();
                    for (int j = 3; j < values.Count(); j++)
                    {
                        dates.Add(DateTime.Parse(values[j]));
                    }
                    Chauffeur new_chauffeur = new Chauffeur(Convert.ToInt32(values[0]), liste_salariés.Find(x => x.NSS == values[1]), chef, dates);
                    chef.AjouterChauffeur(new_chauffeur);
                    AjouterChauffeur(new_chauffeur);
                }
            }

        }

        /// <summary>
        /// Lis le fichier contenant les chefs d'équipe
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeChefEquipe(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_chefequipe = new List<Chef_equipe>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                Chef_equipe chef = new Chef_equipe(liste_salariés.Find(x => x.NSS == values[0]), Convert.ToInt32(values[1]));
                liste_chefequipe.Add(chef);
            }
        }

        /// <summary>
        /// Lis le fichier contenant les villes
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeVilles(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_villes = new List<Ville>();
            for (int i = 0; i < lines.Count(); i++)
            {
                Ville new_ville = new Ville(lines[i]);
                liste_villes.Add(new_ville);
            }
        }

        /// <summary>
        /// Lis le fichier contenant les trajets
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeTrajets(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_trajets = new List<Trajet>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                Trajet new_trajet = new Trajet(liste_villes.Find(x => x.Nom == values[0]), liste_villes.Find(x => x.Nom == values[1]), Convert.ToInt32(values[2]), Convert.ToInt32(values[3]));
                liste_villes.Find(x => x.Nom == values[0]).AjouterTrajet(new_trajet);
                liste_villes.Find(x => x.Nom == values[1]).AjouterTrajet(new_trajet);
                liste_trajets.Add(new_trajet);
            }
        }

        /// <summary>
        /// Lis le fichier contenant les clients
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeClients(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_clients = new List<Client>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                Client new_client = new Client(values[0], values[1], values[2], DateTime.Parse(values[3]), values[4], values[5], values[6], values[7], Convert.ToInt32(values[8]));
                AjouterClient(new_client);
            }
        }

        /// <summary>
        /// Lis le fichier contenant les véhicules
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeVéhicules(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_véhicules = new List<Véhicule>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                if (values[3] != "")
                {
                    List<DateTime> dates = new List<DateTime>();
                    for (int j = 3; j < values.Count(); j++)
                    {
                        dates.Add(DateTime.Parse(values[j]));
                    }
                    Véhicule new_véhicule = new Véhicule(values[0], values[1], dates);
                    liste_véhicules.Add(new_véhicule);
                }
                else
                {
                    Véhicule new_véhicule = new Véhicule(values[0], values[1]);
                    liste_véhicules.Add(new_véhicule);
                }
            }
        }

        /// <summary>
        /// Lis le fichier contenant les commandes
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void ListeCommandes(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            liste_commandes = new List<Commande>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var values = lines[i].Split(',');
                if(values[0] != "")
                {
                    Client client = liste_clients.Find(x => x.ID_Client == values[1]);
                    Chauffeur chauffeur = liste_chauffeurs.Find(x => x.NSS == values[6]);
                    Véhicule véhicule = liste_véhicules.Find(x => x.Immatriculation == values[7]);
                    Commande new_commande = new Commande(values[0], client, DateTime.Parse(values[2]), values[3], values[4], Convert.ToDouble(values[5]), chauffeur, véhicule, Convert.ToBoolean(values[8]));
                    AjouterCommande(new_commande);
                }
            }
        }

        #endregion


        /// <summary>
        /// Permet de convertir une date en string
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Retourne la date sous forme de string</returns>
        public string DateToString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }


        #region Création des fichiers

        /// <summary>
        /// Creer un fichier contenant les identifiants de l'administrateur
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierAdmin(string filepath)
        {
            List<string> lignes = new List<string>();
            lignes.Add(identifiants[0] + "," + identifiants[1]);
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les salariés
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierSalariés(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Salarié salarié in liste_salariés)
            {
                lignes.Add(salarié.NSS + "," + salarié.Nom + "," + salarié.Prenom + "," + salarié.Naissance + "," + salarié.Adresse + "," + salarié.Mail + "," + salarié.Telephone + "," + DateToString(salarié.DateEntree) + "," + salarié.Poste + "," + salarié.Salaire + "," + salarié.NSS_responsable);
            }
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les chauffeurs
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierChauffeurs(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Chauffeur chauffeur in liste_chauffeurs)
            {
                if(chauffeur.Chef_equipe != null)
                {
                    string dates = "";
                    if (chauffeur.Dates_livraison.Count > 0)
                    {
                        foreach(DateTime date in chauffeur.Dates_livraison)
                        {
                            dates += ("," +date.ToString());
                        }
                    }
                    else
                    {
                        dates = ",";
                    }
                    lignes.Add(chauffeur.Nb_commandes_livres + "," + chauffeur.NSS + "," + chauffeur.Chef_equipe.NSS + dates);
                }

                else
                {
                    string dates = "";
                    if (chauffeur.Dates_livraison.Count > 0)
                    {
                        foreach (DateTime date in chauffeur.Dates_livraison)
                        {
                            dates = ("," + date.ToShortDateString());
                        }
                    }
                    else
                    { 
                        dates = (",");
                    }
                    lignes.Add(chauffeur.Nb_commandes_livres + "," + chauffeur.NSS + "," + dates);

                }
            }
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les chefs d'équipe
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierChefEquipe(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Chef_equipe chef in liste_chefequipe)
            {
                string chauffeurs = "";
                if(chef.Chauffeurs.Count > 0)
                {
                    for (int i = 0; i < chef.Chauffeurs.Count - 1; i++)
                    {
                        chauffeurs += chef.Chauffeurs[i].NSS + ",";
                    }
                    chauffeurs += chef.Chauffeurs[chef.Chauffeurs.Count - 1].NSS;
                }

                lignes.Add(chef.NSS + "," + chef.Nombre_chauffeurs_max + "," + chauffeurs);
            }
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les clients
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierClients(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Client client in liste_clients)
            {
                lignes.Add(client.ID_Client + "," + client.Nom + "," + client.Prenom + "," + client.Naissance + "," + client.Adresse + "," + client.Mail + "," + client.Telephone + "," + client.Ville + "," + client.Nb_commandes);
            }
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les villes
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierVilles(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Ville ville in liste_villes)
            {
                lignes.Add(ville.Nom);
            }
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les trajets
        /// </summary>
        /// <param name="pathfile">Le chemin d'accès au csv</param>
        public void CreerFichierTrajets(string pathfile)
        {
            List<string> lignes = new List<string>();
            foreach (Trajet trajet in liste_trajets)
            {
                lignes.Add(trajet.Ville1.Nom + "," + trajet.Ville2.Nom + "," + trajet.Distance + "," + trajet.Duree);
            }
            File.WriteAllLines(pathfile, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les véhicules
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierVéhicules(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Véhicule véhicule in liste_véhicules)
            {
                if(véhicule.Dates_utilisation.Count > 0)
                {
                    string dates = "";
                    foreach(DateTime date in véhicule.Dates_utilisation)
                    {
                        dates += ("," + date.ToString());
                    }
                    lignes.Add(véhicule.Immatriculation + "," + véhicule.Type + "," + véhicule.Prix + dates);
                }
                else
                {
                    lignes.Add(véhicule.Immatriculation + "," + véhicule.Type + "," + véhicule.Prix + ",");
                }
            }
            File.WriteAllLines(filepath, lignes);
        }

        /// <summary>
        /// Creer un fichier contenant les commandes
        /// </summary>
        /// <param name="filepath">Le chemin d'accès au csv</param>
        public void CreerFichierCommandes(string filepath)
        {
            List<string> lignes = new List<string>();
            foreach (Commande commande in liste_commandes)
            {
                lignes.Add(commande.ID_Commande + "," + commande.Client.ID_Client + ","  + commande.DateCommande + "," + commande.PointA + "," + commande.PointB + "," + commande.Prix + "," + commande.Chauffeur.NSS + "," + commande.Véhicule.Immatriculation + "," + commande.Livree);
            }
            File.WriteAllLines(filepath, lignes);
        }

        #endregion

        #region Fonctions Ajouts

        /// <summary>
        /// Permet d'ajouter un salarié à la liste des salariés
        /// </summary>
        /// <param name="salarié">Salarié à ajouter dans la liste</param>
        public void AjouterSalarié(Salarié salarié)
        {
            liste_salariés.Add(salarié);
        }

        /// <summary>
        /// Permet d'ajouter un client à la liste des clients
        /// </summary>
        /// <param name="client">Client à ajouter dans la liste</param>
        public void AjouterClient(Client client)
        {
            liste_clients.Add(client);
        }

        /// <summary>
        /// Permet d'ajouter une commande à la liste des commandes
        /// </summary>
        /// <param name="commande">Commande à ajouter dans la liste de commande</param>
        public void AjouterCommande(Commande commande)
        {
            liste_commandes.Add(commande);
        }

        /// <summary>
        /// Permet d'ajouter un chauffeur à la liste des chauffeurs
        /// </summary>
        /// <param name="chauffeur">Chauffeur à ajouter dans la liste</param>
        public void AjouterChauffeur(Chauffeur chauffeur)
        {
            liste_chauffeurs.Add(chauffeur);
        }

        /// <summary>
        /// Permet d'ajouter un chef d'équipe à la liste des chefs d'équipe
        /// </summary>
        /// <param name="chef">Chef d'éqipe à ajouter dans la liste</param>
        public void AjouterChefEquipe(Chef_equipe chef)
        {
            liste_chefequipe.Add(chef);
        }

        /// <summary>
        /// Permet d'ajouter une véhicule à la liste des véhicules
        /// </summary>
        /// <param name="véhicule">Véhicule à ajouter dans la liste</param>
        public void AjouterVéhicule(Véhicule véhicule)
        {
            liste_véhicules.Add(véhicule);
        }

        #endregion

        #region Organigramme

        /// <summary>
        /// Permet de créer l'organigramme de l'entreprise à partir du boss
        /// </summary>
        public void CreerOrganigramme()
        {
            string NSS_Boss = "";
            foreach (Salarié salarié in liste_salariés)
            {
                if (salarié.NSS_responsable == "")
                {
                    NSS_Boss = salarié.NSS;
                    this.organigramme = new Noeud(salarié);
                    break;
                }
            }
            foreach (Salarié directeur in liste_salariés.FindAll(s => s.NSS_responsable == NSS_Boss))
            {
                Noeud ndirecteur = organigramme.AjouterEnfant(directeur);
                foreach (Salarié chef in liste_salariés.FindAll(s => s.NSS_responsable == directeur.NSS))
                {
                    Noeud nchef = ndirecteur.AjouterEnfant(chef);
                    foreach (Salarié subordonné in liste_salariés.FindAll(s => s.NSS_responsable == chef.NSS))
                    {
                        nchef.AjouterEnfant(subordonné);
                    }
                }
            }
        }

        /// <summary>
        /// Permet d'afficher l'organigramme de l'entreprise
        /// </summary>
        public void AfficherOrganigramme()
        {
            Console.WriteLine("(Directeur) " + organigramme.Personne.Nom);
            for (int i = 0; i < organigramme.Enfants.Count; i++)
            {
                Noeud directeur = organigramme.Enfants[i];
                Salarié salarié1 = liste_salariés.Find(x => x.Nom == directeur.Personne.Nom);
                Console.WriteLine("\t\t" + "(" + salarié1.Poste +") " + directeur.Personne.Nom);

                for (int j = 0; j < directeur.Enfants.Count; j++)
                {
                    Noeud chef = directeur.Enfants[j];
                    Salarié salarié2 = liste_salariés.Find(x => x.Nom == chef.Personne.Nom);
                    Console.WriteLine("\t\t\t\t\t\t\t" +"(" + salarié2.Poste + ") " + chef.Personne.Nom);

                    for (int k = 0; k < chef.Enfants.Count; k++)
                    {

                        Noeud subordonné = chef.Enfants[k];
                        Salarié salarié3 = liste_salariés.Find(x => x.Nom == subordonné.Personne.Nom);
                        Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t" + "(" + salarié3.Poste + ") " + chef.Enfants[k].Personne.Nom);
                    }
                }
            }
        }

        #endregion

        #region Menus

        /// <summary>
        /// Menu de sélection d'actions sur les salariés
        /// </summary>
        public void MenuSalariés()
        {
            ListeSalariés("ListeSalariés.csv");
            Console.Clear();
            Console.WriteLine("1: Afficher les salariés");
            Console.WriteLine("2: Afficher l'oganigramme");
            Console.WriteLine("3: Modifier un salarié");
            Console.WriteLine("4: Supprimer un salarié");
            Console.WriteLine("5: Menu chauffeurs");
            Console.WriteLine("6: Menu chef d'équipe");
            Console.WriteLine("7: Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6" && choix != "7");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    AfficherSalariés();
                    Console.ReadKey();
                    MenuSalariés();
                    break;
                case "2":
                    this.CreerOrganigramme();
                    this.AfficherOrganigramme();
                    Console.ReadKey();
                    Console.WriteLine("\n\n");
                    MenuSalariés();
                    break;
                case "3":
                    foreach(Salarié salarié in liste_salariés)
                    {
                        Console.WriteLine(salarié.NSS + " " + salarié.ShortString() + " " + salarié.Poste);
                    }
                    Console.WriteLine("\nEntrez le NSS du salarié à modifier : ");
                    string NSS_modif = Console.ReadLine();
                    bool test = false;
                    for(int i = 0; i < liste_salariés.Count; i++)
                    {
                        if (liste_salariés[i].NSS == NSS_modif)
                        {
                            Console.Clear();
                            liste_salariés[i] = MenuModifSalarié(liste_salariés[i]);
                            Console.WriteLine("Salarié modifié");
                            CreerFichierSalariés("ListeSalariés.csv");
                            ListeSalariés("ListeSalariés.csv");
                            Thread.Sleep(1000);
                            Console.Clear();
                            CreerOrganigramme();
                            AfficherOrganigramme();
                            test = true;
                            break;
                        }
                    }
                    if (test == false)
                    {
                        Console.Clear();
                        Console.WriteLine("Le salarié n'existe pas");
                    }
                    MenuSalariés();

                    break;    
                case "4":
                    AfficherSalariés();
                    Console.WriteLine("\nEntrez le NSS du salarié à supprimer : ");
                    string NSS_suppr = Console.ReadLine();
                    for(int i = 0; i < liste_salariés.Count; i++)
                    {
                        if (liste_salariés[i].NSS == NSS_suppr)
                        {
                            Console.Clear();
                            Console.WriteLine("Etes vous sur de vouloir supprimer " + liste_salariés[i].Prenom + " " + liste_salariés[i].Nom + " (O/N)");
                            string choix_suppr = Console.ReadLine();
                            if (choix_suppr == "O")
                            {
                                if (liste_chefequipe.Find(x => x.NSS == NSS_suppr) != null)
                                {
                                    Chef_equipe chef = liste_chefequipe.Find(x => x.NSS == NSS_suppr);
                                    
                                    for(int j = chef.Chauffeurs.Count-1; j >= 0; j--)
                                    {
                                        chef.SupprimerChauffeur(chef.Chauffeurs[j]);
                                    }
                                    liste_chefequipe.Remove(chef);
                                    CreerFichierChefEquipe("ListechefEquipe.csv");
                                    CreerFichierChauffeurs("ListeChauffeurs.csv");
                                    ListeChefEquipe("ListechefEquipe.csv");
                                    ListeChauffeurs("ListeChauffeurs.csv");
                                }
                                else if (liste_chauffeurs.Find(x => x.NSS == NSS_suppr) != null)
                                {
                                    Chauffeur chauffeur = liste_chauffeurs.Find(x => x.NSS == NSS_suppr);
                                    if (chauffeur.Chef_equipe != null)
                                    {
                                        chauffeur.Chef_equipe.SupprimerChauffeur(chauffeur);
                                        liste_chauffeurs.Remove(chauffeur);
                                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                                        CreerFichierChefEquipe("ListechefEquipe.csv");
                                        ListeChauffeurs("ListeChauffeurs.csv");
                                        ListeChefEquipe("ListechefEquipe.csv");
                                    }
                                }
                                liste_salariés.RemoveAt(i);
                                Console.Clear();
                                Console.WriteLine("Salarié supprimé");
                                CreerFichierSalariés("ListeSalariés.csv");
                                ListeSalariés("ListeSalariés.csv");
                                Console.ReadKey();
                            }
                            Thread.Sleep(1000);
                            Console.Clear();
                            CreerOrganigramme();
                            AfficherOrganigramme();
                            Console.ReadKey();
                            MenuSalariés();
                            break;
                        }
                    }
                    Console.WriteLine("\n\n");
                    MenuSalariés();
                    break;
                case "5":
                    MenuChauffeurs();
                    break;
                case "6":
                    MenuChefEquipe();
                    Console.ReadKey();
                    MenuSalariés();
                    break;
                case "7":
                    MenuPrincipal();
                    break;
            }
        }

        /// <summary>
        /// Méthode permettant de modifier un salarié
        /// </summary>
        /// <param name="salarié_modif">Salarié à modifier</param>
        /// <returns>Salarié modifié</returns>
        public Salarié MenuModifSalarié(Salarié salarié_modif)
        {
            if (salarié_modif != null)
            {
                Console.WriteLine("Que souhaitez vous modifier ?");
                Console.WriteLine("1: Nom");
                Console.WriteLine("2: Prénom");
                Console.WriteLine("3: Adresse");
                Console.WriteLine("4: Mail");
                Console.WriteLine("5: Téléphone");
                Console.WriteLine("6: Date d'entrée");
                Console.WriteLine("7: Poste");
                Console.WriteLine("8: Salaire");
                Console.WriteLine("9: NSS responsable");
                Console.WriteLine("10: Retour au menu précédent");
                string choix_modif = "";
                do
                {
                    choix_modif = Console.ReadLine();
                } while (choix_modif != "1" && choix_modif != "2" && choix_modif != "3" && choix_modif != "4" && choix_modif != "5" && choix_modif != "6" && choix_modif != "7" && choix_modif != "8" && choix_modif != "9" && choix_modif != "10");
                Console.Clear();
                switch (choix_modif)
                {
                    case "1":
                        Console.WriteLine("Entrez le nouveau nom : ");
                        string nom_modif = Console.ReadLine();
                        salarié_modif.Nom = nom_modif;
                        MenuSalariés();
                        break;
                    case "2":
                        Console.WriteLine("Entrez le nouveau prénom : ");
                        string prenom_modif = Console.ReadLine();
                        salarié_modif.Prenom = prenom_modif;
                        MenuSalariés();
                        break;
                    case "3":
                        Console.WriteLine("Entrez la nouvelle adresse : ");
                        string adresse_modif = Console.ReadLine();
                        salarié_modif.Adresse = adresse_modif;
                        MenuSalariés();
                        break;
                    case "4":
                        Console.WriteLine("Entrez le nouveau mail : ");
                        string mail_modif = Console.ReadLine();
                        salarié_modif.Mail = mail_modif;
                        MenuSalariés();
                        break;
                    case "5":
                        Console.WriteLine("Entrez le nouveau téléphone : ");
                        string telephone_modif = Console.ReadLine();
                        salarié_modif.Telephone = telephone_modif;
                        MenuSalariés();
                        break;
                    case "6":
                        Console.WriteLine("Entrez la nouvelle date d'entrée : ");
                        DateTime dateEntree_modif = Convert.ToDateTime(Console.ReadLine());
                        salarié_modif.DateEntree = dateEntree_modif;
                        MenuSalariés();
                        break;
                    case "7":
                        Console.WriteLine("Entrez le nouveau poste : ");
                        string poste_modif = Console.ReadLine();
                        salarié_modif.Poste = poste_modif;
                        MenuSalariés();
                        break;
                    case "8":
                        Console.WriteLine("Entrez le nouveau salaire : ");
                        double salaire_modif = Convert.ToDouble(Console.ReadLine());
                        salarié_modif.Salaire = salaire_modif;
                        MenuSalariés();
                        break;
                    case "9":
                        Console.WriteLine("Entrez le nouveau NSS responsable : ");
                        string NSS_responsable_modif = Console.ReadLine();
                        salarié_modif.NSS_responsable = NSS_responsable_modif;
                        MenuSalariés();
                        break;
                    case "10":
                        MenuSalariés();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Le salarié n'existe pas");
            }
            return salarié_modif;
        }

        /// <summary>
        /// Menu de sélection d'actions sur les chauffeurs
        /// </summary>
        public void MenuChauffeurs()
        {
            ListeChauffeurs("ListeChauffeurs.csv");
            Console.Clear();
            Console.WriteLine("1: Ajouter un chauffeur");
            Console.WriteLine("2: Afficher/Modifier un chauffeur");
            Console.WriteLine("3: Supprimer un chauffeur");
            Console.WriteLine("4: Afficher les chauffeurs sans équipe");
            Console.WriteLine("5: Afficher les chauffeurs en déplacement");
            Console.WriteLine("6: Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    Console.WriteLine("Entrez le nom du chauffeur : ");
                    string nom = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez le prénom du chauffeur : ");
                    string prenom = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez la date de naissance du chauffeur : ");
                    DateTime naissance = Convert.ToDateTime(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Entrez l'adresse du chauffeur : ");
                    string adresse = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez le mail du chauffeur : ");
                    string mail = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez le téléphone du chauffeur : ");
                    string telephone = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez la date d'entrée du chauffeur : ");
                    DateTime dateEntree = Convert.ToDateTime(Console.ReadLine());
                    string poste = "Chauffeur";
                    Console.Clear();
                    Console.WriteLine("Entrez le salaire du chauffeur : ");
                    double salaire = Convert.ToDouble(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Voulez vous affecter ce nouveau chauffeur à un chef d'équipe ? (O/N)");
                    string choix_chef = Console.ReadLine();
                    if (choix_chef == "O")
                    {
                        Console.Clear();
                        Console.WriteLine("Voici la liste des chef d'équipes disponibles :\n");
                        foreach (Chef_equipe chef2 in liste_chefequipe)
                        {
                            if (chef2.Chauffeurs.Count < chef2.Nombre_chauffeurs_max)
                            {
                                Console.WriteLine(chef2.ShortString());
                            }
                        }
                        Console.WriteLine("\nEntrez le NSS du chef d'équipe : ");
                        string NSS_chef = Console.ReadLine();
                        Chef_equipe chef = liste_chefequipe.Find(x => x.NSS == NSS_chef);
                        Salarié new_salarié = new Salarié(nom, prenom, naissance, adresse, mail, telephone, dateEntree, poste, salaire);
                        new_salarié.NSS_responsable = NSS_chef;
                        Chauffeur new_chauffeur = new Chauffeur(new_salarié, chef);
                        chef.AjouterChauffeur(new_chauffeur);
                        AjouterChauffeur(new_chauffeur);
                        liste_salariés.Add(new_salarié);
                        CreerFichierChefEquipe("ListechefEquipe.csv");
                    }
                    else
                    {
                        Salarié new_salarié = new Salarié(nom, prenom, naissance, adresse, mail, telephone, dateEntree, poste, salaire);
                        Chauffeur new_chauffeur = new Chauffeur(new_salarié);
                        AjouterChauffeur(new_chauffeur);
                        liste_salariés.Add(new_salarié);
                    }
                    CreerFichierSalariés("ListeSalariés.csv");
                    CreerFichierChauffeurs("ListeChauffeurs.csv");
                    Thread.Sleep(1000);
                    Console.Clear();
                    CreerOrganigramme();
                    AfficherOrganigramme();
                    Console.ReadKey();
                    MenuChauffeurs();
                    break;
                case "2":
                    foreach (Chauffeur chauffeur in liste_chauffeurs)
                    {
                        Salarié salarié = liste_salariés.Find(x => x.NSS == chauffeur.NSS);
                        Console.WriteLine(chauffeur.NSS + " " + chauffeur.Prenom + " " + chauffeur.Nom);
                    }
                    Console.WriteLine("\t\t");
                    Console.WriteLine("Entrez le NSS du chauffeur à afficher/modifier : ");
                    string NSS = Console.ReadLine();
                    bool test2 = false;
                    for (int i = 0; i < liste_chauffeurs.Count; i++)
                    {
                        if (liste_chauffeurs[i].NSS == NSS)
                        {

                            liste_chauffeurs[i] = MenuModifChauffeur(liste_chauffeurs[i]);
                            test2 = true;
                            break;
                        }
                    }
                    if (test2 == false)
                    {
                        Console.WriteLine("Le chauffeur n'existe pas");
                    }
                    MenuChauffeurs();
                    break;
                case "3":
                    foreach (Chauffeur chauffeur in liste_chauffeurs)
                    {
                        Console.WriteLine(chauffeur.NSS + " " + chauffeur.Prenom + " " + chauffeur.Nom);
                    }
                    Console.WriteLine("\t\t");
                    Console.WriteLine("Entrez le NSS du chauffeur à afficher/modifier : ");
                    string NSS_supp = Console.ReadLine();
                    Salarié salarié_supp = liste_salariés.Find(liste_salariés => liste_salariés.NSS == NSS_supp);
                    if(salarié_supp == null)
                    {
                        Console.WriteLine("Le chauffeur n'existe pas");
                    }
                    else
                    {
                        Chauffeur chauffeur_supp = liste_chauffeurs.Find(liste_chauffeurs => liste_chauffeurs.NSS == NSS_supp);
                        Chef_equipe chef_supp = chauffeur_supp.Chef_equipe;
                        if(chef_supp != null)
                        {
                            chef_supp.SupprimerChauffeur(chauffeur_supp);
                            CreerFichierChefEquipe("ListeChefEquipe.csv");
                        }
                        liste_chauffeurs.Remove(chauffeur_supp);
                        liste_salariés.Remove(salarié_supp);
                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                        CreerFichierSalariés("ListeSalariés.csv");
                    }

                    MenuChauffeurs();
                    break;
                case "4":
                    int test = 0;
                    foreach (Chauffeur chauffeur in liste_chauffeurs)
                    {
                        if (chauffeur.Chef_equipe == null)
                        {
                            Console.WriteLine(chauffeur.NSS + " " + chauffeur.Prenom + " " + chauffeur.Nom);
                            test++;
                        }
                    }
                    if (test == 0)
                    {
                        Console.WriteLine("Aucun chauffeur sans chef d'équipe");
                    }
                    Console.ReadKey();
                    Console.Clear();
                    MenuChauffeurs();
                    break;
                case "5":
                    int test3 = 0;
                    foreach (Chauffeur chauffeur in liste_chauffeurs)
                    {
                        foreach (DateTime date in chauffeur.Dates_livraison)
                        {
                            if (date.Year == DateTime.Now.Year && date.Month == DateTime.Now.Month && date.Day == DateTime.Now.Day)
                            {
                                Console.WriteLine(chauffeur.NSS + " " + chauffeur.Prenom + " " + chauffeur.Nom);
                                test3++;
                            }
                        }
                    }
                    if (test3 == 0)
                    {
                        Console.WriteLine("Aucun chauffeur en déplacement");
                    }
                    Console.ReadKey();
                    Console.Clear();
                    MenuChauffeurs();
                    break;
                case "6":
                    MenuSalariés();
                    break;
            }
        }

        /// <summary>
        /// Méthode permettant de modifier un chauffeur
        /// </summary>
        /// <param name="chauffeur">Chauffeur à modifier</param>
        /// <returns>Chauffeur modifié</returns>
        public Chauffeur MenuModifChauffeur(Chauffeur chauffeur)
        {
            Console.Clear();
            if(chauffeur != null)
            {
                Console.WriteLine("1: Afficher les informations du chauffeur");
                if(chauffeur.Chef_equipe == null)
                {
                    Console.WriteLine("2: Affecter un chef d'équipe du chauffeur");
                    Console.WriteLine("3: Fin des modifications");
                    string choix = "";
                    do
                    {
                        choix = Console.ReadLine();
                    } while (choix != "1" && choix != "2" && choix != "3");
                    Console.Clear();
                    switch (choix)
                    {
                        case "1":
                            Console.WriteLine(chauffeur.ToString());
                            Console.ReadKey();
                            MenuModifChauffeur(chauffeur);
                            break;
                        case "2":
                            Console.WriteLine("Voici la liste des chef d'équipes disponibles :\n");
                            bool test = false;
                            foreach(Chef_equipe chef in liste_chefequipe)
                            {
                                if(chef.Chauffeurs.Count < chef.Nombre_chauffeurs_max)
                                {
                                    Console.WriteLine(chef.ShortString());
                                    test = true;
                                }
                            }
                            if (test == false)
                                {
                                    Console.WriteLine("Aucun chef d'équipe disponible");
                                }
                            else
                            {
                                Console.WriteLine("\nEntrez le NSS du chef d'équipe : ");
                                string NSS_chef = Console.ReadLine();
                                bool test2 = false;
                                foreach(Chef_equipe chef2 in liste_chefequipe)
                                {
                                    if(chef2.NSS == NSS_chef)
                                    {
                                        chauffeur.Chef_equipe = chef2;
                                        chauffeur.NSS_responsable = NSS_chef;
                                        chef2.AjouterChauffeur(chauffeur);
                                        Salarié salarié = liste_salariés.Find(x => x.NSS == chauffeur.NSS);
                                        salarié.NSS_responsable = NSS_chef;
                                        CreerFichierSalariés("ListeSalariés.csv");
                                        CreerFichierChefEquipe("ListeChefEquipe.csv");
                                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                                        ListeSalariés("ListeSalariés.csv");
                                        ListeChefEquipe("ListeChefEquipe.csv");
                                        ListeChauffeurs("ListeChauffeurs.csv");
                                        test2 = true;
                                        Console.Clear();
                                        Console.WriteLine("Chef d'équipe affecté");
                                    }
                                }
                                if(test2 == false)
                                {
                                    Console.WriteLine("Le chef d'équipe n'existe pas");
                                }
                            }
                            Thread.Sleep(1000);
                            Console.Clear();
                            CreerOrganigramme();
                            AfficherOrganigramme();
                            Console.ReadKey();
                            MenuModifChauffeur(chauffeur);
                            break;
                        case "3":
                            MenuChauffeurs();
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("2: Supprimer le d'équipe du chauffeur");
                    Console.WriteLine("3: Fin des modifications");
                    string choix = "";
                    do
                    {
                        choix = Console.ReadLine();
                    } while (choix != "1" && choix != "2" && choix != "3");
                    Console.Clear();
                    switch (choix)
                    {
                        case "1":
                            Console.WriteLine(chauffeur.ToString());
                            Console.ReadKey();
                            MenuModifChauffeur(chauffeur);
                            break;
                        case "2":
                            chauffeur.Chef_equipe.SupprimerChauffeur(chauffeur);
                            Console.WriteLine("Le chauffeur n'a maintenant plus de chef d'équipe");
                            Salarié salarié = liste_salariés.Find(x => x.NSS == chauffeur.NSS);
                            salarié.NSS_responsable = "";
                            CreerFichierChauffeurs("ListeChauffeurs.csv");
                            CreerFichierChefEquipe("ListeChefEquipe.csv");
                            CreerFichierSalariés("ListeSalariés.csv");
                            ListeSalariés("ListeSalariés.csv");
                            ListeChefEquipe("ListeChefEquipe.csv");
                            ListeChauffeurs("ListeChauffeurs.csv");
                            Thread.Sleep(1000);
                            Console.Clear();
                            CreerOrganigramme();
                            AfficherOrganigramme();
                            Console.ReadKey();
                            MenuModifChauffeur(chauffeur);
                            break;
                        case "3":
                            MenuChauffeurs();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Le chauffeur n'existe pas");
            }
            return chauffeur;
        }

        /// <summary>
        /// Menu de sélection d'actions sur les chefs d'équipe
        /// </summary>
        public void MenuChefEquipe()
        {
            ListeChefEquipe("ListeChefEquipe.csv");
            ListeChauffeurs("ListeChauffeurs.csv");
            Console.Clear();
            Console.WriteLine("1: Ajouter un chef d'équipe");
            Console.WriteLine("2: Afficher/Modifier un chef d'équipe");
            Console.WriteLine("3: Supprimer un chef d'équipe");
            Console.WriteLine("4: Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    Console.WriteLine("Entrez le nom du salarié : ");
                    string nom = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez le prénom du salarié : ");
                    string prenom = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez la date de naissance du salarié : ");
                    DateTime naissance = Convert.ToDateTime(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Entrez l'adresse du salarié : ");
                    string adresse = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez le mail du salarié : ");
                    string mail = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez le téléphone du salarié : ");
                    string telephone = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Entrez la date d'entrée du salarié : ");
                    DateTime dateEntree = Convert.ToDateTime(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Entrez le salaire du salarié : ");
                    double salaire = Convert.ToDouble(Console.ReadLine());
                    Console.Clear();
                    Salarié new_salarié = new Salarié(nom, prenom, naissance, adresse, mail, telephone, dateEntree, "Chef d'Equipe", salaire);
                    new_salarié.NSS_responsable = "188023641962788";
                    AjouterSalarié(new_salarié);
                    Chef_equipe new_chef = new Chef_equipe(new_salarié);
                    AjouterChefEquipe(new_chef);
                    CreerFichierSalariés("ListeSalariés.csv");
                    CreerFichierChefEquipe("ListeChefEquipe.csv");
                    Console.Clear();
                    Console.WriteLine("Chef d'équipe ajouté");
                    Thread.Sleep(1000);
                    Console.Clear();
                    CreerOrganigramme();
                    AfficherOrganigramme();
                    Console.ReadKey();
                    MenuChefEquipe();
                    break;
                case "2":
                    AfficherChefEquipe();
                    string NSS = "";
                    do
                    {
                        Console.WriteLine("\nEntrez le NSS du chef d'équipe à afficher/modifier : ");
                        NSS = Console.ReadLine();
                        Console.Clear();
                        if(!VerifNSSChef(NSS, liste_chefequipe))
                        {
                            AfficherChefEquipe();
                        }
                    } while (!VerifNSSChef(NSS, liste_chefequipe));

                    Chef_equipe chef = liste_chefequipe.Find(x => x.NSS == NSS);
                    Console.WriteLine(chef.ToString());
                    Console.WriteLine(chef.Chauffeurs.Count +"/" + chef.Nombre_chauffeurs_max +" chauffeur(s) sous sa responsabilitée");
                    Console.WriteLine("\nSouhaitez vous modifier ce chef d'équipe ? (O/N)");
                    string choix_modif = "";
                    do
                    {
                        choix_modif = Console.ReadLine();
                    } while (choix_modif != "O" && choix_modif != "N");
                    if (choix_modif == "O")
                    {
                        chef = MenuModifChefEquipe(chef);
                    }
                    else
                    {
                        MenuChefEquipe();
                    }
                    break;
                case "3":
                    AfficherChefEquipe();
                    Console.WriteLine("\nEntrez le NSS du chef d'équipe à supprimer : ");
                    string NSS_supp = "";
                    do
                    {
                        NSS_supp = Console.ReadLine();
                    } while (!VerifNSSChef(NSS_supp, liste_chefequipe));
                    Chef_equipe chef_supp = liste_chefequipe.Find(x => x.NSS == NSS_supp);
                    Console.Clear();
                    Console.WriteLine("Etes vous sur de vouloir supprimer " + chef_supp.Prenom + " " + chef_supp.Nom + " ? (O/N)");
                    string choix_supp = Console.ReadLine();
                    if (choix_supp == "O")
                    {
                        foreach (Chauffeur chauffeur in chef_supp.Chauffeurs)
                        {
                            chauffeur.Chef_equipe = null;
                            chauffeur.NSS_responsable = "";
                        }
                        liste_chefequipe.Remove(chef_supp);
                        Salarié salarié_supp = liste_salariés.Find(x => x.NSS == NSS_supp);
                        liste_salariés.Remove(salarié_supp);
                        CreerFichierChefEquipe("ListeChefEquipe.csv");
                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                        CreerFichierSalariés("ListeSalariés.csv");
                        ListeSalariés("ListeSalariés.csv");
                        ListeChefEquipe("ListeChefEquipe.csv");
                        ListeChauffeurs("ListeChauffeurs.csv");
                        Console.Clear();
                        Console.WriteLine("Chef d'équipe supprimé");
                        Thread.Sleep(1000);
                        Console.Clear();
                        CreerOrganigramme();
                        AfficherOrganigramme();
                        Console.ReadKey();
                    }
                    MenuChefEquipe();
                    break;
                case "4":
                    MenuSalariés();
                    break;

            }
        }

        /// <summary>
        /// Méthode permettant de modifier un chef d'équipe
        /// </summary>
        /// <param name="chef">Chef d'équipe à modifier</param>
        /// <returns>chef d'équipe modifié</returns>
        public Chef_equipe MenuModifChefEquipe(Chef_equipe chef)
        {
            Console.Clear();
            Console.WriteLine("1. Afficher les informations du chef d'équipe");
            Console.WriteLine("2. Modifier le nombre de chauffeurs maximum");
            Console.WriteLine("3. Supprimer un chauffeur de l'équipe");
            Console.WriteLine("4. Fin des modifications");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    Console.WriteLine(chef.ToString());
                    Console.ReadKey();
                    MenuModifChefEquipe(chef);
                    break;
                case "2":
                    Console.WriteLine("Entrez le nouveau nombre de chauffeurs maximum : ");
                    int nbmax = Convert.ToInt32(Console.ReadLine());
                    chef.Nombre_chauffeurs_max = nbmax;
                    Console.Clear();
                    Console.WriteLine("Nombre de chauffeurs maximum modifié");
                    Console.ReadKey();
                    CreerFichierChefEquipe("ListeChefEquipe.csv");
                    MenuModifChefEquipe(chef);
                    break;
                case "3":
                    foreach (Chauffeur chauffeur in chef.Chauffeurs)
                    {
                        Console.WriteLine(chauffeur.ShortString());
                    }
                    Console.WriteLine("\nEntrez le NSS du chauffeur à supprimer : ");
                    string NSS_chauffeur = "";
                    do
                    {
                        NSS_chauffeur = Console.ReadLine();
                    } while (!VerifNSSChauffeur(NSS_chauffeur, chef.Chauffeurs));
                    Chauffeur chauffeur_supp = liste_chauffeurs.Find(x => x.NSS == NSS_chauffeur);
                    Console.Clear();
                    Console.WriteLine("Etes vous sur de vouloir supprimer " + chauffeur_supp.Prenom + " " + chauffeur_supp.Nom + " ? (O/N)");
                    string choix_supp = Console.ReadLine();
                    if (choix_supp == "O")
                    {
                        chef.SupprimerChauffeur(chauffeur_supp);
                        Salarié salarié_supp = liste_salariés.Find(x => x.NSS == NSS_chauffeur);
                        salarié_supp.NSS_responsable = "";
                        CreerFichierSalariés("ListeSalariés.csv");
                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                        CreerFichierChefEquipe("ListeChefEquipe.csv");
                        ListeSalariés("ListeSalariés.csv");
                        ListeChauffeurs("ListeChauffeurs.csv");
                        ListeChefEquipe("ListeChefEquipe.csv");
                        Console.Clear();
                        Console.WriteLine("Chauffeur supprimé");
                        Thread.Sleep(1000);
                        Console.Clear();
                        CreerOrganigramme();
                        AfficherOrganigramme();
                        Console.ReadKey();
                    }
                    MenuModifChefEquipe(chef);
                    break;
                case "4":
                    MenuChefEquipe();
                    break;
            }
            return chef;
        }

        /// <summary>
        /// Menu de sélection d'actions sur les clients 
        /// </summary>
        public void MenuClients()
        {
            ListeClients("ListeClients.csv");
            Console.Clear();
            Console.WriteLine("1: Ajouter un client");
            Console.WriteLine("2: Afficher les clients");
            Console.WriteLine("3. Rechercher un client");
            Console.WriteLine("4: Supprimer un client");
            Console.WriteLine("5: Afficher les avantages clients");
            Console.WriteLine("6: Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    Console.WriteLine("Entrez le nom du client : ");
                    string nom = Console.ReadLine();
                    Console.WriteLine("Entrez le prénom du client : ");
                    string prenom = Console.ReadLine();
                    Console.WriteLine("Entrez la date de naissance du client : ");
                    DateTime naissance = Convert.ToDateTime(Console.ReadLine());
                    Console.WriteLine("Entrez l'adresse du client : ");
                    string adresse = Console.ReadLine();
                    Console.WriteLine("Entrez le mail du client : ");
                    string mail = Console.ReadLine();
                    Console.WriteLine("Entrez le téléphone du client : ");
                    string telephone = Console.ReadLine();
                    Console.WriteLine("Entrez la ville du client : ");
                    string ville = Console.ReadLine();
                    Client new_client = new Client(nom, prenom, naissance, adresse, mail, telephone, ville);
                    AjouterClient(new_client);
                    CreerFichierClients("ListeClients.csv");
                    ListeClients("ListeClients.csv");
                    Console.Clear();
                    Console.WriteLine("Client ajouté");
                    Console.ReadKey();
                    MenuClients();
                    break;
                case "2":
                    {
                        Console.Clear();
                        Console.WriteLine(liste_clients.Count + " clients trouvés\n\n");
                        liste_clients.Sort((x, y) => x.Nom.CompareTo(y.Nom));
                        foreach (Client client in liste_clients)
                        {
                            Console.WriteLine("ID client: " + client.ID_Client + "\nPrénom: " + client.Prenom + "\nNom: " + client.Nom + "\nNombre de commandes passées: " + client.Nb_commandes);
                            if (client.Nb_commandes == 0)
                            {
                                Console.WriteLine("Grade: Sans grade\n");
                            }
                            else if (client.Nb_commandes < 5)
                            {
                                Console.WriteLine("Grade: Bronze\n");
                            }
                            else if (client.Nb_commandes < 10)
                            {
                                Console.WriteLine("Grade: Argent\n");
                            }
                            else if (client.Nb_commandes < 15)
                            {
                                Console.WriteLine("Grade: Or\n");
                            }
                            else
                            {
                                Console.WriteLine("Grade: Diamant\n");
                            }
                        }

                        Console.WriteLine("\nVoulez vous ajouter un filtre de recherche (0/N) ?");
                        string choix_filtre = "";
                        do
                        {
                            choix_filtre = Console.ReadLine();
                        } while (choix_filtre != "O" && choix_filtre != "N");
                        Console.Clear();
                        if(choix_filtre == "O")
                        {
                            Console.WriteLine("1: Filtrer par ville");
                            Console.WriteLine("2: Filtrer par grade");
                            Console.WriteLine("3: Ordre alphabétique");
                            Console.WriteLine("4: Montant des achats cumulés");
                            string choix_filtre2 = "";
                            do 
                            { 
                                choix_filtre2 = Console.ReadLine(); 
                            } while (choix_filtre2 != "1" && choix_filtre2 != "2" && choix_filtre2 != "3" && choix_filtre2 != "4");
                            Console.Clear();
                            switch (choix_filtre2)
                            {
                                case "1":
                                    Console.WriteLine("Entrez la ville à filtrer : ");
                                    string ville_filtre = Console.ReadLine();
                                    List<Client> clients_filtre = liste_clients.FindAll(x => x.Ville == ville_filtre);
                                    Console.Clear();
                                    if (clients_filtre.Count == 0)
                                    {
                                        Console.WriteLine("Aucun client trouvé");
                                        Console.ReadKey();
                                        MenuClients();
                                    }
                                    else
                                    {
                                        foreach (Client client in clients_filtre)
                                        {
                                            Console.WriteLine("ID client: " + client.ID_Client + "\nPrénom: " + client.Prenom + "\nNom: " + client.Nom + "\nNombre de commandes passées: " + client.Nb_commandes);
                                            if (client.Nb_commandes == 0)
                                            {
                                                Console.WriteLine("Grade: Sans grade\n");
                                            }
                                            else if (client.Nb_commandes < 5)
                                            {
                                                Console.WriteLine("Grade: Bronze\n");
                                            }
                                            else if (client.Nb_commandes < 10)
                                            {
                                                Console.WriteLine("Grade: Argent\n");
                                            }
                                            else if (client.Nb_commandes < 15)
                                            {
                                                Console.WriteLine("Grade: Or\n");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Grade: Diamant\n");
                                            }
                                        }
                                    }
                                    break;
                                case "2":
                                    List<Client> clients_grade = liste_clients;
                                    clients_grade.Sort((x, y) => x.Nb_commandes.CompareTo(y.Nb_commandes));
                                    foreach (Client client in clients_grade)
                                    {
                                        Console.WriteLine("ID client: " + client.ID_Client + "\nPrénom: " + client.Prenom + "\nNom: " + client.Nom + "\nNombre de commandes passées: " + client.Nb_commandes);
                                        if (client.Nb_commandes == 0)
                                        {
                                            Console.WriteLine("Grade: Sans grade\n");
                                        }
                                        else if (client.Nb_commandes < 5)
                                        {
                                            Console.WriteLine("Grade: Bronze\n");
                                        }
                                        else if (client.Nb_commandes < 10)
                                        {
                                            Console.WriteLine("Grade: Argent\n");
                                        }
                                        else if (client.Nb_commandes < 15)
                                        {
                                            Console.WriteLine("Grade: Or\n");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Grade: Diamant\n");
                                        }
                                    }
                                    break;
                                case "3":
                                    liste_clients.Sort((x, y) => x.Nom.CompareTo(y.Nom));
                                    foreach (Client client in liste_clients)
                                    {
                                        Console.WriteLine("ID client: " + client.ID_Client + "\nPrénom: " + client.Prenom + "\nNom: " + client.Nom + "\nNombre de commandes passées: " + client.Nb_commandes);
                                        if (client.Nb_commandes == 0)
                                        {
                                            Console.WriteLine("Grade: Sans grade\n");
                                        }
                                        else if (client.Nb_commandes < 5)
                                        {
                                            Console.WriteLine("Grade: Bronze\n");
                                        }
                                        else if (client.Nb_commandes < 10)
                                        {
                                            Console.WriteLine("Grade: Argent\n");
                                        }
                                        else if (client.Nb_commandes < 15)
                                        {
                                            Console.WriteLine("Grade: Or\n");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Grade: Diamant\n");
                                        }
                                    }
                                    break;
                                case "4":
                                    Dictionary<Client, double> dict_clients = new Dictionary<Client, double>();
                                    foreach (Client client in liste_clients)
                                    {
                                        dict_clients[client] = 0;
                                    }
                                    foreach (Commande commande in liste_commandes)
                                    {
                                        dict_clients[commande.Client] += commande.Prix;
                                    }
                                    List<KeyValuePair<Client, double>> clients_achats = dict_clients.ToList();
                                    clients_achats = clients_achats.OrderByDescending(x => x.Value).ToList();
                                    foreach (KeyValuePair<Client, double> client in clients_achats)
                                    {
                                        Console.WriteLine("ID client: " + client.Key.ID_Client + "\nPrénom: " + client.Key.Prenom + "\nNom: " + client.Key.Nom + "\nNombre de commandes passées: " + client.Key.Nb_commandes + "\nMontant des achats cumulés: " + client.Value + "€");
                                        if (client.Key.Nb_commandes == 0)
                                        {
                                            Console.WriteLine("Grade: Sans grade\n");
                                        }
                                        else if (client.Key.Nb_commandes < 5)
                                        {
                                            Console.WriteLine("Grade: Bronze\n");
                                        }
                                        else if (client.Key.Nb_commandes < 10)
                                        {
                                            Console.WriteLine("Grade: Argent\n");
                                        }
                                        else if (client.Key.Nb_commandes < 15)
                                        {
                                            Console.WriteLine("Grade: Or\n");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Grade: Diamant\n");
                                        }
                                    }
                                    break;
                            }
                            Console.WriteLine("\nVoulez vous modifier les informations d'un client ? (O/N)");
                            string choix_modif = "";
                            do
                            {
                                choix_modif = Console.ReadLine();
                            } while (choix_modif != "O" && choix_modif != "N");
                            if(choix_modif == "O")
                            {
                                Console.WriteLine("Entrez l'ID du client à modifier : ");
                                string ID_modif = Console.ReadLine();
                                Client client_modif = liste_clients.Find(x => x.ID_Client == ID_modif);
                                if (client_modif != null)
                                {
                                    client_modif = MenuModifClient(client_modif);
                                    CreerFichierClients("ListeClients.csv");
                                    ListeClients("ListeClients.csv");
                                    Console.Clear();
                                    Console.WriteLine("Client modifié");
                                    Console.ReadKey();
                                    MenuClients();
                                }
                                else
                                {
                                    Console.WriteLine("ID invalide");
                                    Console.ReadKey();
                                    MenuClients();
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("Voulez vous modifier les informations d'un client ? (O/N)");
                            string choix_modif = "";
                            do
                            {
                                choix_modif = Console.ReadLine();
                            } while (choix_modif != "O" && choix_modif != "N");
                            if (choix_modif == "O")
                            {
                                Console.WriteLine("Entrez l'ID du client à modifier : ");
                                string ID_modif = Console.ReadLine();
                                Client client_modif = liste_clients.Find(x => x.ID_Client == ID_modif);
                                if (client_modif != null)
                                {
                                    client_modif = MenuModifClient(client_modif);
                                    CreerFichierClients("ListeClients.csv");
                                    ListeClients("ListeClients.csv");
                                    Console.Clear();
                                    Console.WriteLine("Client modifié");
                                }
                                else
                                {
                                    Console.WriteLine("ID invalide");
                                }
                            }
                            Console.ReadKey();
                            MenuClients();
                        }

                        break;
                    }
                case "3":
                    Console.WriteLine("Entrez le nom du client à rechercher : ");
                    string nom_recherche = Console.ReadLine();
                    Console.Clear();
                    List<Client> clients = liste_clients.FindAll(x => x.Nom == nom_recherche);
                    if (clients.Count == 0)
                    {
                        Console.WriteLine("Aucun client trouvé");
                    }
                    else
                    {
                        foreach (Client client in clients)
                        {
                            Console.WriteLine(client.ID_Client + " " + client.Nom + " " + client.Prenom + " " + client.Ville);
                        }

                        Console.WriteLine("\nVoulez vous ajouter un filtre de recherche en fonction des villes (O/N) ?");
                        string choix_recherche = "";
                        do
                        {
                            choix_recherche = Console.ReadLine();
                        } while (choix_recherche != "O" && choix_recherche != "N");
                        if (choix_recherche == "O")
                        {
                            Console.WriteLine("Entrez la ville à rechercher : ");
                            string ville_recherche = Console.ReadLine();
                            List<Client> clients2 = clients.FindAll(x => x.Ville == ville_recherche);
                            Console.Clear();
                            if (clients2.Count == 0)
                            {
                                Console.WriteLine("Aucun client trouvé");
                                Console.ReadKey();
                                MenuClients();
                            }
                            else
                            { 
                                foreach (Client client in clients2)
                                {
                                    Console.WriteLine(client.ID_Client + " " + client.Nom + " " + client.Prenom + " " + client.Ville + " " + client.Mail);
                                }
                                Console.WriteLine("\nVeuillez indiquer l'ID du client à afficher");
                                string ID_recherche = Console.ReadLine();
                                Client client_recherche = liste_clients.Find(x => x.ID_Client == ID_recherche);
                                Console.Clear();
                                if(client_recherche == null)
                                {              
                                    Console.WriteLine("ID invalide");                                  
                                }
                                else
                                {
                                    Console.WriteLine(client_recherche.ToString());

                                    Console.WriteLine("\nVoulez vous modifier les informations d'un client ? (O/N)");
                                    string choix_modif = "";
                                    do
                                    {
                                        choix_modif = Console.ReadLine();
                                    } while (choix_modif != "O" && choix_modif != "N");
                                    if (choix_modif == "O")
                                    {
                                        Console.WriteLine("Entrez l'ID du client à modifier : ");
                                        string ID_modif = Console.ReadLine();
                                        Client client_modif = liste_clients.Find(x => x.ID_Client == ID_modif);
                                        if (client_modif != null)
                                        {
                                            client_modif = MenuModifClient(client_modif);
                                            CreerFichierClients("ListeClients.csv");
                                            ListeClients("ListeClients.csv");
                                            Console.Clear();
                                            Console.WriteLine("Client modifié");
                                        }
                                        else
                                        {
                                            Console.WriteLine("ID invalide");
                                        }
                                    }
                                }
                                Console.ReadKey();
                                MenuClients();

                            }
                        }
                        else
                        {
                            Console.Clear();
                            foreach (Client client in clients)
                            {
                                Console.WriteLine(client.ID_Client + " " + client.Nom + " " + client.Prenom + " " + client.Ville);
                            }
                            Console.WriteLine("\nVeuillez indiquer l'ID du client à afficher");
                            string ID_recherche = Console.ReadLine();
                            Client client_recherche = liste_clients.Find(x => x.ID_Client == ID_recherche);
                            Console.Clear();
                            if (client_recherche == null)
                            {
                                Console.WriteLine("ID invalide");
                            }
                            else
                            {
                                Console.WriteLine(client_recherche.ToString());

                                Console.WriteLine("\nVoulez vous modifier les informations d'un client ? (O/N)");
                                string choix_modif = "";
                                do
                                {
                                    choix_modif = Console.ReadLine();
                                } while (choix_modif != "O" && choix_modif != "N");
                                if (choix_modif == "O")
                                {
                                    Console.WriteLine("Entrez l'ID du client à modifier : ");
                                    string ID_modif = Console.ReadLine();
                                    Client client_modif = liste_clients.Find(x => x.ID_Client == ID_modif);
                                    if (client_modif != null)
                                    {
                                        client_modif = MenuModifClient(client_modif);
                                        CreerFichierClients("ListeClients.csv");
                                        ListeClients("ListeClients.csv");
                                        Console.Clear();
                                        Console.WriteLine("Client modifié");
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID invalide");
                                    }
                                }
                            }
                            Console.ReadKey();
                            MenuClients();
                        }
                    }
                    break;
                case "4":
                    foreach(Client client in liste_clients)
                    {
                            Console.WriteLine(client.ID_Client + " " + client.Prenom + " " + client.Nom);
                    }
                    Console.WriteLine("\nEntrez l'ID du client à supprimer : ");
                    string ID_supp = "";
                    do
                    {
                        ID_supp = Console.ReadLine();
                        if(!VerifIDClient(ID_supp, liste_clients))
                        {
                            Console.WriteLine("ID invalide");
                        }
                    } while (!VerifIDClient(ID_supp, liste_clients));
                    Client client_supp = liste_clients.Find(x => x.ID_Client == ID_supp);
                    Console.Clear();
                    string choix_supp = "";
                    if (client_supp != null)
                    {
                        Console.WriteLine("Etes vous sur de vouloir supprimer " + client_supp.Prenom + " " + client_supp.Nom + " ? (O/N)");
                        choix_supp = Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Le client n'existe pas");
                    }
                    if (choix_supp == "O")
                    {
                        liste_clients.Remove(client_supp);
                        CreerFichierClients("ListeClients.csv");
                        ListeClients("ListeClients.csv");
                        Console.WriteLine("Client supprimé");
                    }
                    Console.ReadKey();
                    MenuClients();
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Voici les avantages clients : ");
                    Console.WriteLine("\nBronze : 1 à 4 commandes");
                    Console.WriteLine("\t-5% sur les prochaine commandes");
                    Console.WriteLine("\nArgent : 5 à 9 commandes");
                    Console.WriteLine("\t-10% sur les prochaine commandes");
                    Console.WriteLine("\nOr : 10 à 14 commandes");
                    Console.WriteLine("\t-15% sur les prochaine commandes");
                    Console.WriteLine("\nDiamant : 15 commandes et plus");
                    Console.WriteLine("\t-20% sur les prochaine commandes");
                    Console.ReadKey();
                    MenuClients();
                    break;
                case "6":
                    MenuPrincipal();
                    break;
            }
        }

        /// <summary>
        /// Méthode permettant de modifier un client
        /// </summary>
        /// <param name="client_modif">Client à modifier</param>
        /// <returns>Client modifié</returns>
        public Client MenuModifClient(Client client_modif)
        {
            bool fin = false;
            do
            {
                Console.Clear();
                Console.WriteLine(client_modif.ToString());
                Console.WriteLine("\n1: Modifier le nom");
                Console.WriteLine("2: Modifier le prénom");
                Console.WriteLine("3: Modifier l'adresse");
                Console.WriteLine("4: Modifier le mail");
                Console.WriteLine("5: Modifier le téléphone");
                Console.WriteLine("6: Modifier la ville");
                Console.WriteLine("7: Afficher les commandes");
                Console.WriteLine("8: Fin des modifications");
                string choix_modif2 = "";
                do
                {
                    choix_modif2 = Console.ReadLine();
                } while (choix_modif2 != "1" && choix_modif2 != "2" && choix_modif2 != "3" && choix_modif2 != "4" && choix_modif2 != "5" && choix_modif2 != "6" && choix_modif2 != "7" && choix_modif2 != "8");
                Console.Clear();
                switch (choix_modif2)
                {
                    case "1":
                        Console.WriteLine("Entrez le nouveau nom : ");
                        string nom_modif = Console.ReadLine();
                        client_modif.Nom = nom_modif;
                        break;
                    case "2":
                        Console.WriteLine("Entrez le nouveau prénom : ");
                        string prenom_modif = Console.ReadLine();
                        client_modif.Prenom = prenom_modif;
                        break;
                    case "3":
                        Console.WriteLine("Entrez la nouvelle adresse : ");
                        string adresse_modif = Console.ReadLine();
                        client_modif.Adresse = adresse_modif;
                        break;
                    case "4":
                        Console.WriteLine("Entrez le nouveau mail : ");
                        string mail_modif = Console.ReadLine();
                        client_modif.Mail = mail_modif;
                        break;
                    case "5":
                        Console.WriteLine("Entrez le nouveau téléphone : ");
                        string telephone_modif = Console.ReadLine();
                        client_modif.Telephone = telephone_modif;
                        break;
                    case "6":
                        Console.WriteLine("Entrez la nouvelle ville : ");
                        string ville_modif = Console.ReadLine();
                        client_modif.Ville = ville_modif;
                        break;
                    case "7":
                        Console.WriteLine("Voici les commandes de " + client_modif.Prenom + " " + client_modif.Nom + ":");
                        bool test = false;
                        foreach (Commande commande in liste_commandes)
                        {
                            if (commande.Client.ID_Client == client_modif.ID_Client)
                            {
                                Console.WriteLine(commande.ToString());
                                test = true;
                            }
                        }
                        if (test == false)
                        {
                            Console.Clear();
                            Console.WriteLine("Le client n'a effectué aucune commande");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.ReadKey();
                            Console.WriteLine("\nVoulez vous mettre à jour le statut d'une commande ? (O/N)");
                            string choix_modif3 = "";
                            do
                            {
                                choix_modif3 = Console.ReadLine();
                            } while (choix_modif3 != "O" && choix_modif3 != "N");
                            Console.Clear();
                            foreach (Commande commande in liste_commandes)
                            {
                                if (commande.Client.ID_Client == client_modif.ID_Client)
                                {
                                    Console.WriteLine(commande.ToString());
                                }
                            }
                            if (choix_modif3 == "O")
                            {
                                Console.WriteLine("Entrez l'ID de la commande à modifier : ");
                                string ID_commande = Console.ReadLine();
                                Commande commande_modif = liste_commandes.Find(x => x.ID_Commande == ID_commande && x.Client.ID_Client == client_modif.ID_Client);
                                if (commande_modif != null)
                                {
                                    Console.Clear();
                                    if (commande_modif.Livree == false)
                                    {
                                        Console.WriteLine("La commande est affichée comme étant non livrée, voulez vous la passer comme étant livrée ? (O/N)");
                                        string choix_modif4 = "";
                                        do
                                        {
                                            choix_modif4 = Console.ReadLine();
                                        } while (choix_modif4 != "O" && choix_modif4 != "N");
                                        if (choix_modif4 == "O")
                                        {
                                            commande_modif.Livree = true;
                                            Console.Clear();
                                            Console.WriteLine("Commande modifiée");
                                            CreerFichierCommandes("ListeCommandes.csv");
                                            ListeCommandes("ListeCommandes.csv");
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Aucune modification effectuée");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("La commande est affichée comme étant livrée, voulez vous la passer comme étant non livrée ? (O/N)");
                                        string choix_modif4 = "";
                                        do
                                        {
                                            choix_modif4 = Console.ReadLine();
                                        } while (choix_modif4 != "O" && choix_modif4 != "N");
                                        if (choix_modif4 == "O")
                                        {
                                            commande_modif.Livree = false;
                                            Console.Clear();
                                            Console.WriteLine("Commande modifiée");
                                            CreerFichierCommandes("ListeCommandes.csv");
                                            ListeCommandes("ListeCommandes.csv");
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Aucune modification effectuée");
                                        }
                                    }
                                    Console.ReadKey();
                                }
                            }
                        }
                        break;
                    case "8":
                        fin = true;
                        break;
                }
            } while (!fin);
            return client_modif;
        }

        /// <summary>
        /// Menu de sélection d'actions sur les commandes
        /// </summary>
        public void MenuCommandes()
        {
            Console.Clear();
            Console.WriteLine("1: Ajouter une commande");
            Console.WriteLine("2: Afficher les commandes en cours");
            Console.WriteLine("3: Afficher les commandes passées");
            Console.WriteLine("4: Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    CreerCommande();
                    MenuCommandes();
                    break;
                case "2":
                    int compteur = 0;

                    foreach (Commande commande in liste_commandes)
                    {
                        if (commande.Livree == false)
                        {
                            Console.WriteLine(commande.ToString() + "\n");
                            compteur++;
                        }                        
                    }

                    if (compteur == 0)
                    {
                        Console.WriteLine("Aucune commande en cours");
                        Console.ReadKey();
                        MenuCommandes();
                    }
                    string choix_commande = "";
                    Console.WriteLine("\nVoulez vous modifier une commande ? (O/N)");
                    do
                    {
                        choix_commande = Console.ReadLine();

                    } while(choix_commande == "O" && choix_commande == "N");

                    if(choix_commande == "O")
                    {
                        Console.WriteLine("\nEntrez l'ID de la commande à modifier : ");
                        string ID = "";
                        do
                        {
                            ID = Console.ReadLine();
                            if(!VerifIDCommande(ID, liste_commandes))
                            {
                                Console.WriteLine("ID invalide");
                            }
                        }while(!VerifIDCommande(ID, liste_commandes));
                        Commande commande_modif = liste_commandes.Find(x => x.ID_Commande == ID);
                        Console.Clear();
                        Console.WriteLine(commande_modif.ToString());
                        Console.WriteLine("\n1: Mettre a jour le statut de la commande");
                        Console.WriteLine("2: Supprimer la commande");
                        string choix_modif = "";
                        do
                        {
                            choix_modif = Console.ReadLine();
                        } while (choix_modif != "1" && choix_modif != "2");
                        if(choix_modif == "1")
                        {
                            Console.WriteLine("Commande déjà livrée, voulez vous la modifier comme non livrée ? (O/N)");
                            string choix_modif2 = "";
                            do
                            {
                                choix_modif2 = Console.ReadLine();
                            } while (choix_modif2 != "O" && choix_modif2 != "N");
                            if (choix_modif2 == "O")
                            {
                                commande_modif.Livree = true;
                                Console.Clear();
                                Console.WriteLine("Commande modifiée");
                            }
                            else
                            {
                                Console.WriteLine("Commande non modifiée");
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Etes-vous sûre de vouloir supprimer la commande ? (O/N)");
                            string choix_modif2 = "";
                            do
                            {
                                choix_modif2 = Console.ReadLine();
                            } while (choix_modif2 != "O" && choix_modif2 != "N");
                            if (choix_modif2 == "N")
                            {
                                Console.WriteLine("Commande non supprimée");
                                MenuCommandes();
                            }
                            Chauffeur chauffeur = liste_chauffeurs.Find(x => x.NSS == commande_modif.Chauffeur.NSS);
                            Véhicule vehicule = liste_véhicules.Find(x => x.Immatriculation == commande_modif.Véhicule.Immatriculation);
                            Client client = liste_clients.Find(x => x.ID_Client == commande_modif.Client.ID_Client);
                            vehicule.Dates_utilisation.Remove(commande_modif.DateCommande);
                            chauffeur.Dates_livraison.Remove(commande_modif.DateCommande);
                            chauffeur.Nb_commandes_livres = chauffeur.Nb_commandes_livres - 1;
                            client.Nb_commandes = client.Nb_commandes - 1;
                            liste_commandes.Remove(commande_modif);
                        }
                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                        CreerFichierClients("ListeClients.csv");
                        CreerFichierCommandes("ListeCommandes.csv");
                        CreerFichierVéhicules("ListeVéhicules.csv");
                        ListeChauffeurs("ListeChauffeurs.csv");
                        ListeClients("ListeClients.csv");
                        ListeCommandes("ListeCommandes.csv");
                        ListeVéhicules("ListeVéhicules.csv");
                    }

                    Console.Clear();
                    Console.WriteLine("Retour au menu des commandes");
                    Console.ReadKey();
                    MenuCommandes();
                    break;
                case "3":
                    int compteur2 = 0;
                    foreach (Commande commande in liste_commandes)
                    {
                        if (commande.Livree == true)
                        {
                            Console.WriteLine(commande.ToString());
                            compteur2++;
                        }
                    }
                    if (compteur2 == 0)
                    {
                        Console.WriteLine("Aucune commande passée");
                    }
                    string choix_commande2 = "";
                    Console.WriteLine("\nVoulez vous modifier une commande ? (O/N)");
                    do
                    {
                        choix_commande2 = Console.ReadLine();

                    } while (choix_commande2 == "O" && choix_commande2 == "N");

                    if(choix_commande2 == "O")
                    {
                        Console.WriteLine("\nEntrez l'ID de la commande à modifier : ");
                        string ID = "";
                        do
                        {
                            ID = Console.ReadLine();
                            if (!VerifIDCommande(ID, liste_commandes))
                            {
                                Console.WriteLine("ID invalide");
                            }
                        } while (!VerifIDCommande(ID, liste_commandes));
                        Commande commande_modif = liste_commandes.Find(x => x.ID_Commande == ID);
                        Console.Clear();
                        Console.WriteLine(commande_modif.ToString());
                        Console.WriteLine("\n1: Mettre a jour le statut de la commande");
                        Console.WriteLine("2: Supprimer la commande");
                        string choix_modif = "";
                        do
                        {
                            choix_modif = Console.ReadLine();
                        } while (choix_modif != "1" && choix_modif != "2");
                        if (choix_modif == "1")
                        {
                            Console.WriteLine("Commande déjà livrée, voulez vous la modifier comme non livrée ? (O/N)");
                            string choix_modif2 = "";
                            do
                            {
                                choix_modif2 = Console.ReadLine();
                            } while (choix_modif2 != "O" && choix_modif2 != "N");
                            if (choix_modif2 == "O")
                            {
                                commande_modif.Livree = false;
                                Console.Clear();
                                Console.WriteLine("Commande modifiée");
                            }
                            else
                            {
                                Console.WriteLine("Commande non modifiée");
                            }
                        }
                        else
                        {
                            Chauffeur chauffeur = liste_chauffeurs.Find(x => x.NSS == commande_modif.Chauffeur.NSS);
                            Véhicule vehicule = liste_véhicules.Find(x => x.Immatriculation == commande_modif.Véhicule.Immatriculation);
                            Client client = liste_clients.Find(x => x.ID_Client == commande_modif.Client.ID_Client);
                            vehicule.Dates_utilisation.Remove(commande_modif.DateCommande);
                            chauffeur.Dates_livraison.Remove(commande_modif.DateCommande);
                            chauffeur.Nb_commandes_livres = chauffeur.Nb_commandes_livres - 1;
                            client.Nb_commandes = client.Nb_commandes - 1;
                            liste_commandes.Remove(commande_modif);
                        }
                        CreerFichierChauffeurs("ListeChauffeurs.csv");
                        CreerFichierClients("ListeClients.csv");
                        CreerFichierCommandes("ListeCommandes.csv");
                        CreerFichierVéhicules("ListeVéhicules.csv");
                        Console.ReadKey();
                        MenuCommandes();
                    }

                    Console.Clear();
                    Console.WriteLine("Retour au menu des commandes");
                    Console.ReadKey();
                    MenuCommandes();
                    break;
                case "4":
                    MenuPrincipal();
                    break;  
            }
        }

        /// <summary>
        /// Méthode permettant de créer une commande
        /// </summary>
        public void CreerCommande()
        {
            AfficherClients();

            Console.WriteLine("\nEntrez l'ID du client : ");
            string ID_Client = "";
            do
            {
                ID_Client = Console.ReadLine();
                if(!VerifIDClient(ID_Client, liste_clients))
                {
                    Console.WriteLine("ID client invalide");
                }
            } while (!VerifIDClient(ID_Client, liste_clients));
            Console.Clear();

            AfficherVilles();

            string ville1 = "";
            Console.WriteLine("\nEntrez le point de départ : ");
            do
            {
                ville1 = Console.ReadLine();
                if(!VerifVille(ville1, liste_villes))
                {
                    Console.WriteLine("Ville invalide");
                }
            }while(!VerifVille(ville1, liste_villes));


            Console.Clear();
            AfficherVilles();

            string ville2 = "";
            Console.WriteLine("\nEntrez le point d'arrivée : ");
            do
            {
                ville2 = Console.ReadLine();
                if (!VerifVille(ville2, liste_villes))
                {
                    Console.WriteLine("Ville invalide");
                }
            } while (!VerifVille(ville2, liste_villes));

            Ville v1 = Liste_villes.Find(ville => ville.Nom == ville1);
            Ville v2 = Liste_villes.Find(ville => ville.Nom == ville2);
            Console.Clear();

            int[] result = PlusCourtChemin(v1, v2);

            Console.WriteLine("Distance entre " + v1 + " et " + v2 + ": " + result[0] +" km");
            Console.WriteLine("Temps de la commande : " + result[1]/60 +  "h " + result[1]%60 + "min"  );

            Console.WriteLine("Voulez vous valider le trajet ? (O/N)");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "O" && choix != "N");

            Console.Clear();
            if(choix == "O")
            {
                Console.WriteLine("Entrez la date de livraison : ");
                Console.WriteLine("Format : jj/mm/aaaa");
                DateTime date_livraison = Convert.ToDateTime(Console.ReadLine());
                Client client = liste_clients.Find(x => x.ID_Client == ID_Client);
                Commande commande = new Commande(client, date_livraison, v1.Nom, v2.Nom);
                Console.Clear();
                string choix_vehicule = "";
                do
                {
                    Console.WriteLine("Veuillez indiquer le type de véhicule : ");
                    Console.WriteLine("1: Voiture");
                    Console.WriteLine("2: Comionnette");
                    Console.WriteLine("3: Camion");
                    choix_vehicule = Console.ReadLine();
                    Console.Clear();
                } while (choix_vehicule != "Voiture" && choix_vehicule != "Camionnette" && choix_vehicule != "Camion");
                //chauffeur dans list chauffeur qui à le moins de commandes et ne livre pas déjà à cette date
                Chauffeur chauffeur = liste_chauffeurs.Find(x => x.Nb_commandes_livres == liste_chauffeurs.Min(y => y.Nb_commandes_livres) && !x.Dates_livraison.Contains(date_livraison));

                //véhicule dans list céhicule qui à le moins de commandes et ne livre pas déjà à cette date
                Véhicule vehicule = liste_véhicules.Find(x => x.Dates_utilisation.Count() == liste_véhicules.Min(y => y.Dates_utilisation.Count()) && !x.Dates_utilisation.Contains(date_livraison) && x.Type == choix_vehicule);

                if (chauffeur == null || vehicule == null)
                {
                    Console.WriteLine("Aucun chauffeur ou véhicule disponible pour cette date, commande annulée");
                    Console.ReadKey();
                    MenuCommandes();
                }
                else
                {
                    commande.Prix = vehicule.Prix;
                    commande.Prix += result[0] * 0.5;
                    if (DateTime.Now.Year - chauffeur.DateEntree.Year > 10)                     //tarification horaire en fonction de l'ancienneté du chauffeur
                    {
                        commande.Prix += result[1] / 60 * 10;                                   // plafond au bout de 10 ans d'ancienneté
                    }
                    else
                    {
                        commande.Prix += result[1] / 60 * (DateTime.Now.Year - chauffeur.DateEntree.Year);     
                    }
                    if (client.Nb_commandes == 0)
                    {
                        Console.WriteLine("Vous n'avez pas de réduction");
                        commande.Prix = commande.Prix;
                    }
                    else if (client.Nb_commandes < 5)
                    {
                        Console.WriteLine("Grace à votre grade Bronze vous bénéficiez de 5% de réduction et économisez : " + Math.Round((commande.Prix * 0.05), 0) + " euros");
                        commande.Prix = commande.Prix * 0.95;
                    }
                    else if (client.Nb_commandes < 10)
                    {
                        Console.WriteLine("Grace à votre grade Argent vous bénéficiez de 10% de réduction et économisez : " + Math.Round((commande.Prix * 0.1), 0) + " euros");
                        commande.Prix = commande.Prix * 0.9;
                    }
                    else if (client.Nb_commandes < 15)
                    {
                        Console.WriteLine("Grace à votre grade Or vous bénéficiez de 15% de réduction et économisez : " + Math.Round((commande.Prix * 0.15), 0) + " euros");
                        commande.Prix = commande.Prix * 0.85;
                    }
                    else
                    {
                        Console.WriteLine("Grace à votre grade Diamant vous bénéficiez de 20% de réduction et économisez : " + Math.Round((commande.Prix * 0.2), 0) + " euros");
                        commande.Prix = commande.Prix * 0.8;
                    }
                    
                    commande.Prix = Math.Round(commande.Prix, 0);
                    Console.WriteLine("Prix de la commande finale: " + commande.Prix + " euros");
                    Console.WriteLine("Voulez vous valider la commande ? (O/N)");
                    string choix2 = "";
                    do
                    {
                        choix2 = Console.ReadLine();
                    } while (choix2 != "O" && choix2 != "N");
                    if (choix2 != "O")
                    {
                        Console.Clear();
                        Console.WriteLine("Commande annulée");
                        Console.ReadLine();
                        MenuCommandes();
                    }
                    if (DateTime.Now.Year - chauffeur.DateEntree.Year < 5)
                    {
                        chauffeur.Salaire += result[1] / 60 * 5;                                   //revalorisation du salaire du chauffeur à hauteur de 5€ de l'heure
                    }
                    else if(DateTime.Now.Year - chauffeur.DateEntree.Year < 10)
                    {
                        chauffeur.Salaire += result[1] / 60 * 10;                                  //revalorisation du salaire du chauffeur à hauteur de 10€ de l'heure
                    }
                    else
                    {
                        chauffeur.Salaire += result[1] / 60 * 15;                                  //revalorisation du salaire du chauffeur à hauteur de 15€ de l'heure
                    }
                    Salarié salarié = liste_salariés.Find(x => x.NSS == chauffeur.NSS);
                    salarié.Salaire = chauffeur.Salaire;
                    CreerFichierSalariés("ListeSalariés.csv");
                    ListeSalariés("ListeSalariés.csv");
                    commande.Chauffeur = chauffeur;
                    commande.Véhicule = vehicule;
                    chauffeur.Dates_livraison.Add(date_livraison);
                    chauffeur.Nb_commandes_livres++;
                    AjouterCommande(commande);
                    client.Nb_commandes++;
                    CreerFichierChauffeurs("ListeChauffeurs.csv");
                    ListeChauffeurs("ListeChauffeurs.csv");
                    vehicule.Dates_utilisation.Add(date_livraison);
                    CreerFichierVéhicules("ListeVéhicules.csv");
                    ListeVéhicules("ListeVéhicules.csv");
                    CreerFichierClients("ListeClients.csv"); 
                    ListeClients("ListeClients.csv");
                    CreerFichierCommandes("ListeCommandes.csv");
                    ListeCommandes("ListeCommandes.csv");
                    Console.Clear();
                    Console.WriteLine("Commande validée");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Récapitulatif de la commande\n");
                    Console.WriteLine("Commande n°" + commande.ID_Commande + "\nClient : " + commande.Client.Nom + " " + commande.Client.Prenom + "\nDate de commande : " + commande.DateCommande.ToShortDateString() + "\nChauffeur: " + commande.Chauffeur.Prenom + " " + commande.Chauffeur.Nom + "\nVéhicule: " + commande.Véhicule.Type + " n°" + commande.Véhicule.Immatriculation + "\nPrix total: " + commande.Prix + " euros");
                    Console.ReadKey();
                    Console.Clear();
                    }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Commande annulée");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Menu de statistiques
        /// </summary>
        public void MenuStatistiques()
        {
            Console.Clear();
            Console.WriteLine("1: Afficher les finances de " + Nom);
            Console.WriteLine("2: Afficher les chauffeurs en fonction du nombre de livraisons effectués");
            Console.WriteLine("3: Afficher les 3 clients les plus fidèles et dépensiés");
            Console.WriteLine("4: Afficher le prix moyen d'une commande");
            Console.WriteLine("5: Afficher le nombre moyen de commandes par clients");
            Console.WriteLine("6: Afficher les 3 villes les plus demandées");
            Console.WriteLine("7: Afficher les types de véhicules les plus utilisés");
            Console.WriteLine("8: Afficher les commandes sur une période de temps donnée");
            Console.WriteLine("9: Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6" && choix != "7" && choix != "8" && choix != "9");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    double benefice = 0;
                    double charge = 0;
                    foreach (Salarié salarié in liste_salariés)
                    {
                        charge += salarié.Salaire;
                    }


                    double revenus = 0;
                    foreach(Commande commande in liste_commandes)
                    {
                        revenus += commande.Prix;
                    }
                    if(revenus > charge)
                    {
                        benefice = revenus - charge;
                        Console.WriteLine("La société " + Nom + " est en bénéfice de " + benefice + " euros cette année");
                    }
                    else
                    {
                        benefice = charge - revenus;
                        Console.WriteLine("La société " + Nom + " est en déficit de " + benefice + " euros cette année");
                    }


                    Console.WriteLine("\nLes revenues sont de " + revenus + " euros");
                    Console.WriteLine("Les charges sont de " + charge + " euros");
                    Console.ReadKey();
                    MenuStatistiques();

                    break;
                case "2":
                    List<Chauffeur> liste_chauffeurs_trie = liste_chauffeurs.OrderByDescending(x => x.Nb_commandes_livres).ToList();        // delegate ORDERBY
                    Console.WriteLine("Liste des chauffeurs en fonction de leur nombre de livraisons : ");
                    foreach(Chauffeur chauffeur in liste_chauffeurs_trie)
                    {
                        Console.WriteLine(chauffeur.ShortString() + ": " + chauffeur.Nb_commandes_livres + " livraisons");
                    }

                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "3":
                    List<Client> liste_clients_trie = liste_clients.OrderByDescending(x => x.Nb_commandes).ToList();
                    Console.WriteLine("Les 3 clients les plus fidèles sont : ");
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(liste_clients_trie[i].Prenom + " " + liste_clients_trie[i].Nom + ": " + liste_clients_trie[i].Nb_commandes + " commandes");
                    }

                    //afficher les clients qui ont le plus dépensés

                    Console.WriteLine("\nLes 3 clients les plus dépensiers sont : ");
                    Dictionary<Client, double> dict_clients = new Dictionary<Client, double>();
                    foreach (Client client in liste_clients)
                    {
                        dict_clients[client] = 0;
                    }
                    foreach (Commande commande in liste_commandes)
                    {
                        dict_clients[commande.Client] += commande.Prix;
                    }
                    List<KeyValuePair<Client, double>> liste_clients_trie2 = dict_clients.ToList();
                    liste_clients_trie2 = liste_clients_trie2.OrderByDescending(x => x.Value).ToList();
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(liste_clients_trie2[i].Key.Prenom + " " + liste_clients_trie2[i].Key.Nom + ": " + liste_clients_trie2[i].Value + " euros dépensés");
                    }

                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "4":
                    double prix_moyen = liste_commandes.Average(x => x.Prix);                           // delegate AVERAGE
                    Console.WriteLine("Le prix moyen d'une commande est de " + prix_moyen + " euros");
                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "5":
                    double nb_moyen = liste_clients.Average(x => x.Nb_commandes);                       // delegate AVERAGE
                    Console.WriteLine("Le nombre moyen de commandes par client est de " + nb_moyen);
                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "6":
                    Dictionary<Ville, int> dict_villes = new Dictionary<Ville, int>();
                    foreach(Ville ville in liste_villes)
                    {
                        dict_villes[ville] = 0;
                    }
                    foreach(Commande commande in liste_commandes)
                    {
                        foreach(Ville ville in liste_villes)
                        {
                            if(commande.PointB == ville.Nom)
                            {
                                dict_villes[ville]++;
                            }
                        }
                    }
                    List<KeyValuePair<Ville, int>> liste_villes_trie = dict_villes.ToList();
                    liste_villes_trie = liste_villes_trie.OrderByDescending(x => x.Value).ToList();
                    Console.WriteLine("Les 3 villes les plus demandées sont : ");
                    for(int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(liste_villes_trie[i].Key.Nom + " " + liste_villes_trie[i].Value);
                    }
                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "7":
                   Dictionary<string, int> dict_vehicules = new Dictionary<string, int>();
                    foreach (Véhicule vehicule in liste_véhicules)
                    {
                        dict_vehicules[vehicule.Type] = 0;
                    }
                    foreach (Commande commande in liste_commandes)
                    {
                        dict_vehicules[commande.Véhicule.Type]++;
                    }
                    List<KeyValuePair<string, int>> liste_vehicules_trie = dict_vehicules.ToList();
                    liste_vehicules_trie = liste_vehicules_trie.OrderByDescending(x => x.Value).ToList();
                    Console.WriteLine("Les types de véhicules les plus utilisés sont : ");
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(liste_vehicules_trie[i].Key + " " + liste_vehicules_trie[i].Value);
                    }
                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "8":
                    Console.WriteLine("Entrez la date de début : ");
                    Console.WriteLine("Format : jj/mm/aaaa");
                    DateTime date_debut = Convert.ToDateTime(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Entrez la date de fin : ");
                    Console.WriteLine("Format : jj/mm/aaaa");
                    DateTime date_fin = Convert.ToDateTime(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Liste des commandes entre le " + date_debut.ToShortDateString() + " et le " + date_fin.ToShortDateString() + " : ");
                    bool test = false;
                    foreach(Commande commande in liste_commandes)
                    {
                        if(commande.DateCommande >= date_debut && commande.DateCommande <= date_fin)
                        {
                            Console.WriteLine(commande.ToString());
                            test = true;
                        }
                    }
                    if(!test)
                    { 
                        Console.Clear();
                        Console.WriteLine("Aucune commande sur cette période");
                    }
                    Console.ReadKey();
                    MenuStatistiques();
                    break;
                case "9":
                    MenuPrincipal();
                    break;
            }
        }

        /// <summary>
        /// Menu de s'élection d'action administratives
        /// </summary>
        public void MenuAdmin()
        {
            Console.Clear();
            Console.WriteLine("1. Modifier l'identifiant");
            Console.WriteLine("2. Modifier le mot de passe");
            Console.WriteLine("3. Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
                if(choix != "1" && choix != "2" && choix != "3")
                {
                    Console.WriteLine("Choix invalide");
                }
            } while (choix != "1" && choix != "2" && choix != "3");
            Console.Clear();
            switch (choix)
            {
                case "1":
                    Console.Write("Entrez le nouvel identifiant : ");
                    string test1 = Console.ReadLine();
                    Console.Write("Confirmez l'identifiant : ");
                    string test2 = Console.ReadLine();
                    Console.Clear();
                    if(test1 != test2)
                    {
                        Console.WriteLine("Les mots de passe ne correspondent pas");
                        Console.ReadKey();
                        MenuAdmin();
                    }
                    else
                    {
                        identifiants[0] = test1;
                        Console.Clear();
                        CreerFichierAdmin("Identifiants.csv");
                        Console.WriteLine("Identifiant modifié");
                        Console.ReadKey();
                        MenuAdmin();
                    }
                    break;
                case "2":
                    Console.Write("Entrez le nouveau mot de passe : ");
                    string test3 = Console.ReadLine();
                    Console.Write("Confirmez le mot de passe : ");
                    string test4 = Console.ReadLine();
                    Console.Clear();
                    if (test4 != test3)
                    {
                        Console.WriteLine("Les mots de passe ne correspondent pas");
                        Console.ReadKey();
                        MenuAdmin();
                    }
                    else
                    {
                        identifiants[1] = test3;
                        CreerFichierAdmin("Identifiants.csv");
                        Console.WriteLine("Mot de passe modifié");
                        Console.ReadKey();
                        MenuAdmin();
                    }
                    break;
                case "3":
                    MenuPrincipal();
                    break;
            }

        }

        /// <summary>
        /// Menu de sélection des actions sur les trajets
        /// </summary>
        public void MenuTrajets()
        {
            Console.Clear();
            Console.WriteLine("1. Afficher les villes déjà proposées");
            Console.WriteLine("2. Ajouter une ville à la liste des destinations");
            Console.WriteLine("3. Ajouter un trajet");
            Console.WriteLine("4. Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
                if (choix != "1" && choix != "2" && choix != "3" && choix != "4")
                {
                    Console.WriteLine("Choix invalide");
                }
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4");
            Console.Clear();

            switch (choix)
            {
                case "1":

                    AfficherVilles();
                    Console.WriteLine("\nVoulez vous modifier une ville ? (O/N)");
                    string choix_modif = "";
                    do
                    {
                        choix_modif = Console.ReadLine();
                        if (choix_modif != "O" && choix_modif != "N")
                        {
                            Console.WriteLine("Choix invalide");
                        }
                    } while (choix_modif != "O" && choix_modif != "N");
                    Console.Clear();
                    if (choix_modif == "O")
                    {

                        AfficherVilles();
                        Console.WriteLine("\nEntrez le nom de la ville à modifier : ");
                        string nom_modif = Console.ReadLine();
                        Ville ville_modif = liste_villes.Find(x => x.Nom == nom_modif);
                        Console.Clear();
                        if(ville_modif == null)
                        { 
                            Console.WriteLine("Ville introuvable");
                            Console.ReadKey();
                            MenuTrajets();
                        }
                        Console.WriteLine(ville_modif.Nom);
                        foreach(Trajet t in ville_modif.Trajets)
                        {
                            Console.WriteLine(t.ToString());
                        }

                        Console.WriteLine("\n1: Ajouter un trajet");
                        Console.WriteLine("2: Supprimer un trajet");
                        Console.WriteLine("3: Fin des modifications");

                        string choix_modif2 = "";
                        do
                        {
                            choix_modif2 = Console.ReadLine();
                            if (choix_modif2 != "1" && choix_modif2 != "2" && choix_modif2 != "3")
                            {
                                Console.WriteLine("Choix invalide");
                            }
                        } while (choix_modif2 != "1" && choix_modif2 != "2" && choix_modif2 != "3");

                        switch(choix_modif2)
                        {
                            case "1":
                                Console.WriteLine("Veuillez entrer le nom de la deuxième ville : ");
                                string nom_ville2 = Console.ReadLine();
                                Ville new_ville = liste_villes.Find(x => x.Nom == nom_ville2);
                                if(new_ville == null)
                                {
                                    new_ville = new Ville(nom_ville2);
                                    new_ville.Trajets = new List<Trajet>();
                                    liste_villes.Add(new_ville);
                                }
                                Console.WriteLine("Entrez la distance entre les deux villes : ");
                                int new_distance = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Entrez la durée du trajet : ");
                                int new_duree = Convert.ToInt32(Console.ReadLine());
                                Trajet new_trajet = new Trajet(ville_modif, new_ville, new_distance, new_duree);
                                Console.Clear();
                                if (ExistTrajet(ville_modif, new_ville))
                                {
                                    Console.WriteLine("Trajet déjà existant");
                                }
                                else
                                {
                                    ville_modif.AjouterTrajet(new_trajet);
                                    new_ville.AjouterTrajet(new_trajet);
                                    liste_trajets.Add(new_trajet);
                                    CreerFichierTrajets("ListeTrajets.csv");
                                    CreerFichierVilles("ListeVilles.csv");
                                    Console.WriteLine("ListeVilles ajouté");
                                }
                                Console.ReadKey();
                                MenuTrajets();
                                break;
                            case "2":
                                Console.WriteLine("Veuillez entrer le nom de la deuxième ville : ");
                                string nom_ville3 = Console.ReadLine();
                                Ville ville3 = liste_villes.Find(x => x.Nom == nom_ville3);
                                Trajet t = ville_modif.Trajets.Find(x => x.Ville1.Nom == nom_ville3 || x.Ville2.Nom == nom_ville3);
                                Console.Clear();
                                if (t != null)
                                {
                                    t.Ville1.Trajets.Remove(t);
                                    t.Ville2.Trajets.Remove(t);
                                    liste_trajets.Remove(t);
                                    CreerFichierTrajets("ListeTrajets.csv");
                                    Console.WriteLine("Trajet supprimé");
                                }
                                else
                                {
                                    Console.WriteLine("Trajet introuvable");
                                }
                                Console.ReadKey();
                                MenuTrajets();
                                break;
                            case "3":
                                MenuTrajets();
                                break;
                        }
                    }
                    Console.ReadKey();
                    MenuTrajets();
                    break;
                case "2":
                    Console.WriteLine("Entrez le nom de la ville : ");
                    string nom = Console.ReadLine();
                    Ville ville = new Ville(nom);
                    liste_villes.Add(ville);
                    CreerFichierVilles("ListeVilles.csv");
                    Console.Clear();
                    Console.WriteLine("Ville ajoutée");
                    Console.ReadKey();
                    MenuTrajets();
                    break;
                case "3":
                    AfficherVilles();
                    Console.WriteLine("\nEntrez le nom de la première ville : ");
                    string ville1 = Console.ReadLine();
                    Console.Clear();
                    if(!VerifVille(ville1, liste_villes))
                    {
                        Console.WriteLine("Ville introuvable");
                        Console.ReadKey();
                        MenuTrajets();
                    }
                    AfficherVilles();
                    Console.WriteLine("\nEntrez le nom de la deuxième ville : ");
                    string ville2 = Console.ReadLine();
                    if(!VerifVille(ville2, liste_villes))
                    {
                        Console.WriteLine("Ville introuvable");
                        Console.ReadKey();
                        MenuTrajets();
                    }
                    Console.Clear();
                    Ville v1 = liste_villes.Find(x => x.Nom == ville1);
                    Ville v2 = liste_villes.Find(x => x.Nom == ville2);
                    Console.WriteLine("Entrez la distance entre les deux villes : ");
                    int distance = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Entrez la durée du trajet : ");
                    int duree = Convert.ToInt32(Console.ReadLine());
                    Trajet trajet = new Trajet(v1, v2, distance, duree);
                    if (ExistTrajet(v1, v2))
                    {
                        Console.WriteLine("Trajet déjà existant");
                        Console.ReadKey();
                        MenuTrajets();
                    }
                    v1.AjouterTrajet(trajet);
                    v2.AjouterTrajet(trajet);
                    liste_trajets.Add(trajet);
                    CreerFichierTrajets("ListeTrajets.csv");
                    Console.Clear();
                    Console.WriteLine("Trajet ajouté");
                    Console.ReadKey();
                    MenuTrajets();
                    break;
                case "4":
                    MenuPrincipal();
                    break;
            }
                
        }

        /// <summary>
        /// Menu de sélection des actions sur les véhiculs
        /// </summary>
        public void MenuVéhicule()
        {
            Console.Clear();
            ListeVéhicules("ListeVéhicules.csv");
            Console.WriteLine("1. Ajouter un véhicule");
            Console.WriteLine("2. Supprimer un véhicule");
            Console.WriteLine("3. Afficher les véhicules");
            Console.WriteLine("4. Retour au menu principal");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
                if (choix != "1" && choix != "2" && choix != "3" && choix != "4")
                {
                    Console.WriteLine("Choix invalide");
                }
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4");
            Console.Clear();
            switch(choix)
            {
                case "1":
                    Console.WriteLine("Entrez le type de véhicule : ");
                    Console.WriteLine("1: Voiture");
                    Console.WriteLine("2: Camionnette");
                    Console.WriteLine("3: Camion");
                    string type = "";
                    do
                    {
                        type = Console.ReadLine();
                    }while(type != "1" && type != "2" && type != "3");
                    Console.Clear();
                    Console.WriteLine("Entrez le numéro d'immatriculation : ");
                    string immatriculation = Console.ReadLine();
                    Console.Clear();
                    if(type == "1")
                    {
                        Véhicule vehicule = new Véhicule(immatriculation, "Voiture");
                        Console.WriteLine("Etes vous sur de vouloir ajouter le véhicule suivant à la flotte ?");
                        Console.WriteLine("\nImmatriculation : " + immatriculation + "\nType : " + vehicule.Type + "\nPrix : " + vehicule.Prix);
                        if(Console.ReadLine() == "O")
                        { 
                            Console.Clear();
                            Console.WriteLine("Véhicule ajouté");
                            AjouterVéhicule(vehicule);
                            CreerFichierVéhicules("ListeVéhicules.csv");
                            ListeVéhicules("ListeVéhicules.csv");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule non ajouté");
                        }
                    }
                    else if(type == "2")
                    {
                        Véhicule vehicule = new Véhicule(immatriculation, "Camionnette");
                        Console.WriteLine("Etes vous sur de vouloir ajouter le véhicule suivant à la flotte ? (O/N)");
                        Console.WriteLine("\nImmatriculation : " + immatriculation + "\nType : " + vehicule.Type + "\nPrix : " + vehicule.Prix);
                        if (Console.ReadLine() == "O")
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule ajouté");
                            AjouterVéhicule(vehicule);
                            CreerFichierVéhicules("ListeVéhicules.csv");
                            ListeVéhicules("ListeVéhicules.csv");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule non ajouté");
                        }
                    }
                    else
                    {
                        Véhicule vehicule = new Véhicule(immatriculation, "Camion");
                        Console.WriteLine("Etes vous sur de vouloir ajouter le véhicule suivant à la flotte ?");
                        Console.WriteLine("\nImmatriculation : " + immatriculation + "\nType : " + vehicule.Type + "\nPrix : " + vehicule.Prix);
                        if (Console.ReadLine() == "O")
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule ajouté");
                            AjouterVéhicule(vehicule);
                            CreerFichierVéhicules("ListeVéhicules.csv");
                            ListeVéhicules("ListeVéhicules.csv");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule non ajouté");
                        }
                    }
                    Console.ReadKey();
                    MenuVéhicule();
                    break;
                case "2":
                    Console.WriteLine("Liste des véhicule : ");
                    foreach(Véhicule véhicule in liste_véhicules)
                    {
                        Console.WriteLine(véhicule.ToString());
                    } 
                    Console.WriteLine("Voulez vous ajouter un filtre à la recherche ? (O/N)");
                    string choix_filtre = "";
                    do
                    {
                        choix_filtre = Console.ReadLine();
                    } while (choix_filtre != "O" && choix_filtre != "N");
                    Console.Clear();
                    if(choix_filtre == "O")
                    {
                        Console.WriteLine("Entrez le type de véhicule : ");
                        Console.WriteLine("1: Voiture");
                        Console.WriteLine("2: Camionnette");
                        Console.WriteLine("3: Camion");
                        string type2 = "";
                        do
                        {
                            type2 = Console.ReadLine();
                        } while (type2 != "1" && type2 != "2" && type2 != "3");
                        Console.Clear();
                        if(type2 == "1")
                        {
                            Console.WriteLine("Liste des voitures : ");
                            foreach (Véhicule vehicule in liste_véhicules)
                            {
                                if(vehicule.Type == "Voiture")
                                {
                                    Console.WriteLine(vehicule.ToString());
                                }
                            }
                        }
                        else if(type2 == "2")
                        {
                            Console.WriteLine("Liste des camionnettes : ");
                            foreach (Véhicule vehicule in liste_véhicules)
                            {
                                if (vehicule.Type == "Camionnette")
                                {
                                    Console.WriteLine(vehicule.ToString());
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Liste des camions : ");
                            foreach (Véhicule vehicule in liste_véhicules)
                            {
                                if (vehicule.Type == "Camion")
                                {
                                    Console.WriteLine(vehicule.ToString());
                                }
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("Liste des véhicule : ");
                        foreach (Véhicule véhicule in liste_véhicules)
                        {
                            Console.WriteLine(véhicule.ToString());
                        }
                    }

                    Console.WriteLine("\nEntrez le numéro d'immatriculation du véhicule à supprimer : ");
                    string immatriculation2 = Console.ReadLine();
                    Véhicule vehicule2 = liste_véhicules.Find(x => x.Immatriculation == immatriculation2);
                    if(vehicule2 == null)
                    {
                        Console.WriteLine("Véhicule introuvable");
                    }
                    else
                    {
                        Console.WriteLine("Etes vous sur de vouloir supprimer le véhicule suivant de la flotte ?");
                        Console.WriteLine("\nImmatriculation : " + immatriculation2 + "\nType : " + vehicule2.Type + "\nPrix : " + vehicule2.Prix);
                        if (Console.ReadLine() == "O")
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule supprimé");
                            liste_véhicules.Remove(vehicule2);
                            CreerFichierVéhicules("ListeVéhicules.csv");
                            ListeVéhicules("ListeVéhicules.csv");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Véhicule non supprimé");
                        }
                    }
                    Console.ReadKey();
                    MenuVéhicule();
                    break;
                case "3":
                    Console.WriteLine("Liste des véhicule : ");
                    foreach (Véhicule véhicule in liste_véhicules)
                    {
                        Console.WriteLine(véhicule.ToString());
                    }
                    Console.WriteLine("\nVoulez vous ajouter un filtre à la recherche ? (O/N)");
                    string choix_filtre2 = "";
                    do
                    {
                        choix_filtre2 = Console.ReadLine();
                    } while (choix_filtre2 != "O" && choix_filtre2 != "N");
                    Console.Clear();
                    if (choix_filtre2 == "O")
                    {
                        Console.WriteLine("Entrez le type de véhicule : ");
                        Console.WriteLine("1: Voiture");
                        Console.WriteLine("2: Camionnette");
                        Console.WriteLine("3: Camion");
                        string type2 = "";
                        do
                        {
                            type2 = Console.ReadLine();
                        } while (type2 != "1" && type2 != "2" && type2 != "3");
                        Console.Clear();
                        if (type2 == "1")
                        {
                            Console.WriteLine("Liste des voitures : \n");
                            foreach (Véhicule vehicule in liste_véhicules)
                            {
                                if (vehicule.Type == "Voiture")
                                {
                                    Console.WriteLine(vehicule.ToString());
                                }
                            }
                        }
                        else if (type2 == "2")
                        {
                            Console.WriteLine("Liste des camionnettes : \n");
                            foreach (Véhicule vehicule in liste_véhicules)
                            {
                                if (vehicule.Type == "Camionnette")
                                {
                                    Console.WriteLine(vehicule.ToString());
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Liste des camions : \n");
                            foreach (Véhicule vehicule in liste_véhicules)
                            {
                                if (vehicule.Type == "Camion")
                                {
                                    Console.WriteLine(vehicule.ToString());
                                }
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("Liste des véhicule : \n");
                        foreach (Véhicule véhicule in liste_véhicules)
                        {
                            Console.WriteLine(véhicule.ToString());
                        }
                    }
                    Console.ReadKey();
                    MenuVéhicule();
                    break;
                case "4":
                    MenuPrincipal();
                    break;
            }
        }

        /// <summary>
        /// Menu principal
        /// </summary>
        public void MenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("1. Menu Salariés");
            Console.WriteLine("2. Menu Clients");
            Console.WriteLine("3. Menu Commandes");
            Console.WriteLine("4. Menu Véhicules");
            Console.WriteLine("5. Statistiques");
            Console.WriteLine("6. Partie administrateur");
            Console.WriteLine("7. Gérer les destinations");
            Console.WriteLine("8. Quitter");
            Console.WriteLine("9: Tests unitaires");
            string choix = "";
            do
            {
                choix = Console.ReadLine();
                if (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6" && choix != "7" && choix != "8" && choix != "9")
                {
                    Console.WriteLine("Choix invalide");
                }
            } while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6" && choix != "7" && choix != "8" && choix != "9");
            switch (choix)
            {
                case "1":
                    MenuSalariés();
                    break;
                case "2":
                    MenuClients();
                    break;
                case "3":
                    MenuCommandes();
                    break;
                case "4":
                    MenuVéhicule();
                    break;
                case "5":
                    MenuStatistiques();
                    break;
                case "6":
                    Console.Clear();
                    Console.Write("Entrez l'identifiant : ");
                    string identifiant = Console.ReadLine();
                    Console.Write("Entrez le mot de passe : ");
                    string mdp = Console.ReadLine();
                    if (identifiant == identifiants[0] && mdp == identifiants[1])
                    {
                        MenuAdmin();
                    }
                    else
                    {
                        Console.WriteLine("Identifiants incorrects");
                        Console.ReadKey();
                        MenuPrincipal();
                    }
                    break;

                case "7":
                    MenuTrajets();
                    break;
                case "8":
                    Environment.Exit(0);
                    break;
                case "9":
                    MenuTestUnitaires();
                    break;
            }
        }

        #endregion

        #region Tests unitaires

        /// <summary>
        /// Menu contenants les tests unitaires
        /// </summary>
        public void MenuTestUnitaires()
        {
            Console.Clear();
            Console.WriteLine("Test unitaire sur les salariés : ");
            Console.WriteLine("\n1: Test unitaire sur la création d'un salarié");
            TestUnitaireCreationSalarié();
            Console.WriteLine("\n2: Test unitaire sur la modification d'un salarié");
            TestUnitaireModificationSalarié();
            Console.WriteLine("\n3: Test unitaire sur la suppression d'un salarié");
            TestUnitaireSuppressionSalarié();


            Console.WriteLine("\nTest unitaire sur les clients : ");
            Console.WriteLine("\n1: Test unitaire sur la création d'un client");
            TestUnitaireCreationClient();
            Console.WriteLine("\n2: Test unitaire sur la modification d'un client");
            TestUnitaireModificationClient();
            Console.WriteLine("\n3: Test unitaire sur la suppression d'un client");
            TestUnitaireSuppressionClient();


            Console.WriteLine("\nTest unitaire sur les véhicules : ");
            Console.WriteLine("\n1: Test unitaire sur la création d'un véhicule");
            TestUnitaireCreationVéhicule();
            Console.WriteLine("\n2: Test unitaire sur la modification d'un véhicule");
            TestUnitaireModificationVéhicule();
            Console.WriteLine("\n3: Test unitaire sur la suppression d'un véhicule");
            TestUnitaireSuppressionVéhicule();

            Console.WriteLine("\nTest unitaire Djikstra : ");
            TestUnitaireDjikstra();


        }

        /// <summary>
        /// Test unitaire sur la création d'un salarié 
        /// </summary>
        public void TestUnitaireCreationSalarié()
        {
            string nom = "TEST";
            string prenom = "TEST";
            string NSS = "111111111111";
            string NSS_responsable = "22222222222";
            DateTime date_naissance = Convert.ToDateTime("01/01/1990");
            DateTime date_entrée = Convert.ToDateTime("01/01/2010");
            string adresse = "1 rue de la paix";
            string mail = "test@gmail.com";
            string telephone = "0123456789";
            string poste = "Salarié";
            double salaire = 999999;
            Salarié test = new Salarié(NSS, nom, prenom, date_naissance, adresse, mail, telephone, date_entrée, poste, salaire, NSS_responsable);

            AjouterSalarié(test);
            CreerFichierSalariés("ListeSalariés.csv");
            ListeSalariés("ListeSalariés.csv");

            Salarié vérif = liste_salariés.Find(x => x.NSS == test.NSS);

            if (vérif != null)
            {
                if (vérif.Nom != test.Nom)
                {
                    Console.WriteLine("Erreur de stockage du nom");

                }
                else if (vérif.Prenom != test.Prenom)
                {
                    Console.WriteLine("Erreur de stockage du prénom");
                }
                else if (vérif.NSS != test.NSS)
                {
                    Console.WriteLine("Erreur de stockage du NSS");
                }
                else if(vérif.NSS_responsable != test.NSS_responsable)
                {
                    Console.WriteLine("Erreur de stockage du NSS responsable");
                }
                else if (vérif.Naissance != test.Naissance)
                {
                    Console.WriteLine("Erreur de stockage de la date de naissance");
                }
                else if (vérif.DateEntree != test.DateEntree)
                {
                    Console.WriteLine("Erreur de stockage de la date d'entrée");
                }
                else if (vérif.Adresse != test.Adresse)
                {
                    Console.WriteLine("Erreur de stockage de l'adresse");
                }
                else if (vérif.Mail != test.Mail)
                {
                    Console.WriteLine("Erreur de stockage du mail");
                }
                else if (vérif.Telephone != test.Telephone)
                {
                    Console.WriteLine("Erreur de stockage du téléphone");
                }
                else if (vérif.Poste != test.Poste)
                {
                    Console.WriteLine("Erreur de stockage du poste");
                }
                else if (vérif.Salaire != test.Salaire)
                {
                    Console.WriteLine("Erreur de stockage du salaire");
                }
                else
                {
                    Console.WriteLine("Test création réussi");
                }

            }
            else
            {
                Console.WriteLine("Salarié non trouvé");
            }
        }

        /// <summary>
        /// Test unitaire sur la modification d'un salarié
        /// </summary>
        public void TestUnitaireModificationSalarié()
        {

            {

                Salarié test = liste_salariés.Find(x => x.NSS == "111111111111");
                if(test != null)
                {
                    test.Nom = "TEST2";
                    test.Prenom = "TEST2";
                    test.NSS_responsable = "333333333333";
                    test.Naissance = Convert.ToDateTime("01/01/1991");
                    test.DateEntree = Convert.ToDateTime("01/01/2011");
                    test.Adresse = "2 rue de la paix";
                    test.Mail = "test2@gmail.com";
                    test.Telephone = "9876543210";
                    test.Poste = "Salarié2";
                    test.Salaire = 888888;

                    CreerFichierSalariés("ListeSalariés.csv");
                    ListeSalariés("ListeSalariés.csv");

                    Salarié vérif = liste_salariés.Find(x => x.NSS == test.NSS);

                    if (vérif != null)
                    {
                        if (vérif.Nom != test.Nom)
                        {
                            Console.WriteLine("Erreur de stockage du nom");

                        }
                        else if (vérif.Prenom != test.Prenom)
                        {
                            Console.WriteLine("Erreur de stockage du prénom");
                        }
                        else if (vérif.NSS != test.NSS)
                        {
                            Console.WriteLine("Erreur de stockage du NSS");
                        }
                        else if(vérif.NSS_responsable != test.NSS_responsable)
                        {
                            Console.WriteLine("Erreur de stockage du NSS responsable");
                        }
                        else if (vérif.Naissance != test.Naissance)
                        {
                            Console.WriteLine("Erreur de stockage de la date de naissance");
                        }
                        else if (vérif.DateEntree != test.DateEntree)
                        {
                            Console.WriteLine("Erreur de stockage de la date d'entrée");
                        }
                        else if (vérif.Adresse != test.Adresse)
                        {
                            Console.WriteLine("Erreur de stockage de l'adresse");
                        }
                        else if (vérif.Mail != test.Mail)
                        {
                            Console.WriteLine("Erreur de stockage du mail");
                        }
                        else if (vérif.Telephone != test.Telephone)
                        {
                            Console.WriteLine("Erreur de stockage du téléphone");
                        }
                        else if (vérif.Poste != test.Poste)
                        {
                            Console.WriteLine("Erreur de stockage du poste");
                        }
                        else if (vérif.Salaire != test.Salaire)
                        {
                            Console.WriteLine("Erreur de stockage du salaire");
                        }
                        else
                        {
                            Console.WriteLine("Test modification réussi");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Salarié non trouvé");
                    }
                }
                else
                {
                    Console.WriteLine("Salarié non trouvé");
                }
            }
        }

        /// <summary>
        /// Test unitaire sur la suppression d'un salarié
        /// </summary>
        public void TestUnitaireSuppressionSalarié()
        {
            Salarié test = liste_salariés.Find(x => x.NSS == "111111111111");
            if(test != null)
            {
                liste_salariés.Remove(test);
                CreerFichierSalariés("ListeSalariés.csv");
                ListeSalariés("ListeSalariés.csv");

                Salarié vérif = liste_salariés.Find(x => x.NSS == test.NSS);

                if (vérif == null)
                {
                    Console.WriteLine("Test suppression réussi");
                }
                else
                {
                    Console.WriteLine("Erreur de suppression");
                    vérif.ToString();
                }
            }
            else
            {
                Console.WriteLine("Salarié non trouvé");
            }
        }

        /// <summary>
        /// Test unitaire sur la création d'un client 
        /// </summary>
        public void TestUnitaireCreationClient()
        {
            string nom = "CLIENT_TEST";
            string prenom = "CLIENT_TEST";
            string adresse = "1 rue de la paix";
            string mail = "client_test@gmail.com";
            string telephone = "0123456789";
            string id_client = "XXXXX";
            string ville = "VILLE_TEST";
            int nb_commandes = 999999;
            DateTime naissance = Convert.ToDateTime("01/01/1990");
            Client test = new Client(id_client, nom, prenom, naissance, adresse, mail, telephone, ville, nb_commandes);

            AjouterClient(test);
            CreerFichierClients("ListeClients.csv");
            ListeClients("ListeClients.csv");

            Client vérif = liste_clients.Find(x => x.ID_Client == test.ID_Client);

            if (vérif != null)
            {
                if (vérif.Nom != test.Nom)
                {
                    Console.WriteLine("Erreur de stockage du nom");

                }
                else if (vérif.Prenom != test.Prenom)
                {
                    Console.WriteLine("Erreur de stockage du prénom");
                }
                else if (vérif.ID_Client != test.ID_Client)
                {
                    Console.WriteLine("Erreur de stockage de l'ID Client");
                }
                else if (vérif.Naissance != test.Naissance)
                {
                    Console.WriteLine("Erreur de stockage de la date de naissance");
                }
                else if (vérif.Adresse != test.Adresse)
                {
                    Console.WriteLine("Erreur de stockage de l'adresse");
                }
                else if (vérif.Mail != test.Mail)
                {
                    Console.WriteLine("Erreur de stockage du mail");
                }
                else if (vérif.Telephone != test.Telephone)
                {
                    Console.WriteLine("Erreur de stockage du téléphone");
                }
                else if (vérif.Ville != test.Ville)
                {
                    Console.WriteLine("Erreur de stockage de la ville");
                }
                else
                {
                    Console.WriteLine("Test création réussi");
                }

            }
            else
            {
                Console.WriteLine("Client non trouvé");
            }


        }

        /// <summary>
        /// Test unitaire sur la modification d'un client
        /// </summary>
        public void TestUnitaireModificationClient()
        {
            Client test = liste_clients.Find(x => x.ID_Client == "XXXXX");
            if(test != null)
            {
                test.Nom = "CLIENT_TEST2";
                test.Prenom = "CLIENT_TEST2";
                test.Naissance = Convert.ToDateTime("01/01/1991");
                test.Adresse = "2 rue de la paix";
                test.Mail = "client2_test@gmail.com";
                test.Telephone = "9876543210";
                test.Ville = "VILLE_TEST2";
                test.Nb_commandes = 888888;

                CreerFichierClients("ListeClients.csv");
                ListeClients("ListeClients.csv");

                Client vérif = liste_clients.Find(x => x.ID_Client == test.ID_Client);

                if (vérif != null)
                {
                    if (vérif.Nom != test.Nom)
                    {
                        Console.WriteLine("Erreur de stockage du nom");

                    }
                    else if (vérif.Prenom != test.Prenom)
                    {
                        Console.WriteLine("Erreur de stockage du prénom");
                    }
                    else if (vérif.ID_Client != test.ID_Client)
                    {
                        Console.WriteLine("Erreur de stockage de l'ID Client");
                    }
                    else if (vérif.Naissance != test.Naissance)
                    {
                        Console.WriteLine("Erreur de stockage de la date de naissance");
                    }
                    else if (vérif.Adresse != test.Adresse)
                    {
                        Console.WriteLine("Erreur de stockage de l'adresse");
                    }
                    else if (vérif.Mail != test.Mail)
                    {
                        Console.WriteLine("Erreur de stockage du mail");
                    }
                    else if (vérif.Telephone != test.Telephone)
                    {
                        Console.WriteLine("Erreur de stockage du téléphone");
                    }
                    else if (vérif.Ville != test.Ville)
                    {
                        Console.WriteLine("Erreur de stockage de la ville");
                    }
                    else if (vérif.Nb_commandes != test.Nb_commandes)
                    {
                        Console.WriteLine("Erreur de stockage du nombre de commande");
                    }
                    else
                    {
                        Console.WriteLine("Test modification réussi");
                    }

                }
                else
                {
                    Console.WriteLine("Client non trouvé");
                }

            }

        }

        /// <summary>
        /// Test unitaire sur la suppression d'un client
        /// </summary>
        public void TestUnitaireSuppressionClient()
        {
            liste_clients.Remove(liste_clients.Find(x => x.ID_Client == "XXXXX"));
            CreerFichierClients("ListeClients.csv");
            ListeClients("ListeClients.csv");

            Client vérif = liste_clients.Find(x => x.ID_Client == "XXXXX");
            if (vérif == null)
            {
                Console.WriteLine("Test suppression réussi");
            }
            else
            {
                Console.WriteLine("Erreur de suppression");
            }
        }

        /// <summary>
        /// Test unitaire sur la création d'un véhicule
        /// </summary>
        public void TestUnitaireCreationVéhicule()
        {
            string immatriculation = "TEST";
            string type = "Voiture";
            List<DateTime> dates = new List<DateTime>();

            Véhicule test = new Véhicule(immatriculation, type, dates);
            double prix = 999999;
            test.Dates_utilisation.Add(Convert.ToDateTime("01/01/2010"));

            AjouterVéhicule(test);
            CreerFichierVéhicules("ListeVéhicules.csv");
            ListeVéhicules("ListeVéhicules.csv");

            Véhicule vérif = liste_véhicules.Find(x => x.Immatriculation == test.Immatriculation);

            if (vérif != null)
            {
                if (vérif.Immatriculation != test.Immatriculation)
                {
                    Console.WriteLine("Erreur de stockage de l'immatriculation");

                }
                else if (vérif.Type != test.Type)
                {
                    Console.WriteLine("Erreur de stockage du type");
                }
                else if (vérif.Prix != test.Prix)
                {
                    Console.WriteLine("Erreur de stockage du prix");
                }
                else
                {
                    Console.WriteLine("Test création réussi");
                }

            }
            else
            {
                Console.WriteLine("Véhicule non trouvé");
            }
        }

        /// <summary>
        /// Test unitaire sur la modification d'un véhicule
        /// </summary>
        public void TestUnitaireModificationVéhicule()
        {
            Véhicule test = liste_véhicules.Find(x => x.Immatriculation == "TEST");
            if(test != null)
            {
                test.Type = "Camion";
                test.Prix = 150;
                test.Dates_utilisation.Add(Convert.ToDateTime("01/01/2011"));

                CreerFichierVéhicules("ListeVéhicules.csv");
                ListeVéhicules("ListeVéhicules.csv");

                Véhicule vérif = liste_véhicules.Find(x => x.Immatriculation == test.Immatriculation);

                if (vérif != null)
                {
                    if (vérif.Immatriculation != test.Immatriculation)
                    {
                        Console.WriteLine("Erreur de stockage de l'immatriculation");

                    }
                    else if (vérif.Type != test.Type)
                    {
                        Console.WriteLine("Erreur de stockage du type");
                    }
                    else if (vérif.Prix != test.Prix)
                    {
                        Console.WriteLine("Erreur de stockage du prix");
                    }
                    else
                    {
                        Console.WriteLine("Test modification réussi");
                    }

                }
                else
                {
                    Console.WriteLine("Véhicule non trouvé");
                }
            }
            else
            {
                Console.WriteLine("Véhicule non trouvé");
            }
        }

        /// <summary>
        /// Test unitaire sur la suppression d'un véhicule
        /// </summary>
        public void TestUnitaireSuppressionVéhicule()
        {
            liste_véhicules.Remove(liste_véhicules.Find(x => x.Immatriculation == "TEST"));
            CreerFichierVéhicules("ListeVéhicules.csv");
            ListeVéhicules("ListeVéhicules.csv");

            Véhicule vérif = liste_véhicules.Find(x => x.Immatriculation == "TEST");
            if (vérif == null)
            {
                Console.WriteLine("Test suppression réussi");
            }
            else
            {
                Console.WriteLine("Erreur de suppression");
            }
        }

        /// <summary>
        /// Test unitaire sur l'algorithme de Djikstra
        /// </summary>
        public void TestUnitaireDjikstra()
        {
            Ville ville_test1 = liste_villes.Find(x => x.Nom == "Paris");
            Ville ville_test2 = liste_villes.Find(x => x.Nom == "Rouen");

            int[] test = PlusCourtChemin(ville_test1, ville_test2);

            if (test[0] == 133 && test[1] == 105)
            {
                Console.WriteLine("Test Djikstra réussi");
            }
            else
            {
                Console.WriteLine("Erreur Djikstra");
            }

        }
        #endregion

        #region Trajets

        /// <summary>
        /// Méthode permettant de vérifier si un trajet entre 2 villes existe déjà
        /// </summary>
        /// <param name="ville1">Ville de départ du trajet</param>
        /// <param name="ville2">Ville d'arrivé du trajet</param>
        /// <returns>booléen en fonction de si e trajet existe déjà ou non</returns>
        public bool ExistTrajet(Ville ville1, Ville ville2)
        {
            foreach (Trajet t in liste_trajets)
            {
                if ((t.Ville1 == ville1 && t.Ville2 == ville2) || (t.Ville1 == ville2 && t.Ville2 == ville1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de trouver le trajet le plus court entre 2 villes
        /// </summary>
        /// <param name="départ">Ville de départ</param>
        /// <param name="arrivee">Ville d'arrivée</param>
        /// <returns>la distance du trajet et la durée du trajet</returns>
        public int[] PlusCourtChemin(Ville départ, Ville arrivee)
        {
            Dictionary<Ville, int> distances = new Dictionary<Ville, int>();                //stocke la distance minimale connue de départ à chaque ville
            Dictionary<Ville, int> durees = new Dictionary<Ville, int>();                   //stocke la durée minimale connue de départ à chaque ville
            Dictionary<Ville, Ville> précédents = new Dictionary<Ville, Ville>();           //stocke le prédécesseur immédiat de chaque ville dans le chemin le plus court trouvé
            List<Ville> noeudsNonVisités = new List<Ville>();                               //liste des villes qui n'ont pas encore été visitées
            foreach (Ville v in liste_villes)                                               
            {
                distances[v] = int.MaxValue;                                                //La distance est initialisée à int.MaxValue == infini
                précédents[v] = null;                                                       //Les prédécesseurs de chaque ville sont initialisés à null
                noeudsNonVisités.Add(v);                                                    //toutes les villes sont initialisées comme non visitées
            }
            distances[départ] = 0;
            durees[départ] = 0;
            while (noeudsNonVisités.Count > 0)
            {
                Ville v = null;
                foreach (Ville v3 in noeudsNonVisités)
                {
                    if (v == null || distances[v3] < distances[v])                          //v = v3 si la distance de v3 est plus petite que la distance de v ou v est null initialement
                    {
                        v = v3;
                    }
                }
                if (distances[v] == int.MaxValue)                                           //SI distance minimale == int.MaxValue => ville est inaccessible
                {
                    break;
                }
                noeudsNonVisités.Remove(v);                                                 //La ville avec la distance minimale (v) est maintenant marquée comme visitée

                foreach (Trajet t in v.Trajets)
                {
                    Ville v3;                                                               //Si t.Ville1 est v, alors v3 est t.Ville2, sinon v3 est t.Ville1
                    if (t.Ville1 == v)
                    {
                        v3 = t.Ville2;
                    }
                    else
                    {
                        v3 = t.Ville1;
                    }                      
                    int distance = distances[v] + t.Distance;                               //Calcul de la nouvelle distance minimale 
                    int duree = durees[v] + t.Duree;                                        //Calcul de la nouvelle duree liée à la distance minimale
                    if (distance < distances[v3])
                    {
                        distances[v3] = distance;                                           //Mise à jour de la distance minimale
                        durees[v3] = duree;                                                 //Mise à jour de la durée liée à la distance minimale
                        précédents[v3] = v;                                                 //Mise à jour du prédécesseur de v3
                    }
                }
            }
            int[] result = new int[2];
            result[0] = distances[arrivee];
            result[1] = durees[arrivee];
            return result;
        }

        /// <summary>
        /// Méthode permettant d'afficher tous les trajets des villes 
        /// </summary>
        public void AfficherTrajets()
        {
            foreach (Ville v in liste_villes)
            {
                Console.WriteLine(v.Nom);
                foreach (Trajet t in v.Trajets)
                {
                    Console.WriteLine("  " + t.Ville1.Nom + " - " + t.Ville2.Nom + " : " + t.Distance + " / " + t.Duree);
                }
            }
        }

        #endregion

        #region Vérificateurs

        /// <summary>
        /// Méthode permettant de vérifier si un chauffeur est déjà dans la liste de chauffeurs
        /// </summary>
        /// <param name="NSS">NSS du chauffeur à vérifier</param>
        /// <param name="liste">liste de chauffeurs</param>
        /// <returns>retourne vrai si il est contenu dans la liste</returns>
        public bool VerifNSSChauffeur(string NSS, List<Chauffeur> liste)
        {
            foreach(Chauffeur chauffeur in liste)
            {
                if (chauffeur.NSS == NSS)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de vérifier si un chef d'équipe est déjà dans la liste de chefs d'équipes
        /// </summary>
        /// <param name="NSS">NSS du chef d'équipe à vérifier</param>
        /// <param name="liste">liste de chefs d'équipes</param>
        /// <returns>retourne vrai si il est contenu dans la liste</returns>
        public bool VerifNSSChef(string NSS, List<Chef_equipe> liste)
        {
            foreach (Chef_equipe chef in liste)
            {
                if (chef.NSS == NSS)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de vérifier si un salarié est déjà dans la liste de salariés
        /// </summary>
        /// <param name="NSS">NSS du salarié à vérifier</param>
        /// <param name="liste">liste de salariés</param>
        /// <returns>retourne vrai si il est contenu dans la liste</returns>
        public bool VerifNSSSalarié(string NSS, List<Salarié> liste)
        {
            foreach (Salarié salarié in liste)
            {
                if (salarié.NSS == NSS)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de vérifier si un client est déjà dans la liste de clients
        /// </summary>
        /// <param name="NSS">ID du client à vérifier</param>
        /// <param name="liste">liste de clients</param>
        /// <returns>retourne vrai si il est contenu dans la liste</returns>
        public bool VerifIDClient(string id, List<Client> liste_clients)
        {
            foreach (Client client in liste_clients)
            {
                if (client.ID_Client == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de vérifier si une ville est déjà dans la liste des villes
        /// </summary>
        /// <param name="NSS">nom de la ville à vérifier</param>
        /// <param name="liste">liste des villes</param>
        /// <returns>retourne vrai si elle est contenu dans la liste</returns>
        public bool VerifVille(string ville, List<Ville> liste_villes)
        {
            foreach (Ville v in liste_villes)
            {
                if (v.Nom == ville)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Méthode permettant de vérifier si une commande est déjà dans la liste des commandes
        /// </summary>
        /// <param name="NSS">id de la commande à vérifier</param>
        /// <param name="liste">liste des commandes</param>
        /// <returns>retourne vrai si elle est contenu dans la liste</returns>
        public bool VerifIDCommande(string id, List<Commande> liste_commandes)
        {
            foreach (Commande commande in liste_commandes)
            {
                if (commande.ID_Commande == id)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        public override string ToString()
        {
            string result = "Entreprise : " + nom + "\n";
            if (liste_salariés.Count > 0)
            {
                result += "Salariés : \n";
                foreach (Salarié salarié in liste_salariés)
                {
                    result += salarié + "\n";
                }
            }
            else
            {
                result += "Aucun salarié\n";
            }
            if (liste_clients.Count > 0)
            {
                result += "\nClients : \n";
                foreach (Client client in liste_clients)
                {
                    result += client + "\n";
                }
            }
            else
            {
                result += "Aucun client\n";
            }
            if (liste_commandes.Count > 0)
            {
                result += "\nCommandes : \n";
                foreach (Commande commande in liste_commandes)
                {
                    result += commande + "\n";
                }
            }
            else
            {
                result += "Aucune commande\n";
            }
            return result;
        }
    }
}
