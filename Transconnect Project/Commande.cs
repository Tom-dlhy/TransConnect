using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Commande : Interface
    {
        private string id_commande;
        private Client client;
        private DateTime dateCommande;
        private string pointA;
        private string pointB;
        private double prix;
        private bool livree;    //statut de la commande
        private Chauffeur chauffeur;
        private Véhicule véhicule;

        public Commande(Client client, DateTime dateCommande, string pointA, string pointB)
        {
            Guid uuid = Guid.NewGuid();
            this.id_commande = uuid.ToString().Substring(0, 5);
            this.client = client;
            this.dateCommande = dateCommande;
            this.prix = 0;
            this.pointA = pointA;
            this.pointB = pointB;
            this.livree = false;
        }

        public Commande(string id_commande, Client client, DateTime dateCommande, string pointA, string pointB, double prix, Chauffeur chauffeur, Véhicule véhicule, bool livree)
        {
            this.id_commande = id_commande;
            this.client = client;
            this.dateCommande = dateCommande;
            this.prix = prix;
            this.pointA = pointA;
            this.pointB = pointB;
            this.livree = livree;
            this.chauffeur = chauffeur;
            this.véhicule = véhicule;
        }

        public string ID_Commande
        {
            get { return id_commande; }
            set { id_commande = value; }
        }

        public Client Client
        {
            get { return client; }
            set { client = value; }
        }

        public DateTime DateCommande
        {
            get { return dateCommande; }
            set { dateCommande = value; }
        }

        public double Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        public string PointA
        {
            get { return pointA; }
            set { pointA = value; }
        }

        public string PointB
        {
            get { return pointB; }
            set { pointB = value; }
        }

        public bool Livree
        {
            get { return livree; }
            set { livree = value; }
        }

        public Chauffeur Chauffeur
        {
            get { return chauffeur; }
            set { chauffeur = value; }
        }

        public Véhicule Véhicule
        {
            get { return véhicule; }
            set { véhicule = value; }
        }


        public override string ToString()
        {
            return "Commande n°" + id_commande + "\nClient : " + client.Nom + " " + client.Prenom + "\nDate de commande : " + dateCommande.ToShortDateString() + "\nChauffeur: " + chauffeur.Prenom + " " + chauffeur.Nom + "\nVéhicule: " + véhicule.Type  + " n°" + véhicule.Immatriculation + "\nPrix total: " + prix + " euros" + "\nLivrée : "  + livree;
            
        }
    }
}
