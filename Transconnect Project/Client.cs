using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Client : Personne, Interface, IComparable
    {
        private string id_client;
        private string ville;
        private List<Commande> commandes;
        private int nb_commandes;
        
        public Client(string nom, string prenom, DateTime naissance, string adresse, string mail, string telephone, string ville) : base(nom, prenom, naissance, adresse, mail, telephone)
        {
            Guid uuid = Guid.NewGuid();
            this.id_client = uuid.ToString().Substring(0,5);
            this.ville = ville;
            this.commandes = new List<Commande>();
            this.nb_commandes = 0;
        }

        public Client(string id_client, string nom, string prenom, DateTime naissance, string adresse, string mail, string telephone, string ville, int nb_commandes) : base(nom, prenom, naissance, adresse, mail, telephone)
        {
            this.id_client = id_client;
            this.ville = ville;
            this.commandes = new List<Commande>();
            this.nb_commandes = nb_commandes;
        }

        public string ID_Client
        {
            get { return id_client; }
            set { id_client = value; }
        }

        public string Ville
        {
            get { return ville; }
            set { ville = value; }
        }

        public List<Commande> Commandes
        {
            get { return commandes; }
            set { commandes = value; }
        }

        public int Nb_commandes
        {
            get { return nb_commandes; }
            set { nb_commandes = value; }
        }

        public void AjouterCommande(Commande commande)
        {
            commandes.Add(commande);
        }

        public override string ToString()
        {
            string result = "ID Client : " + id_client + "\n" + base.ToString() + "\nVille : " + ville + "\n";
            if(commandes.Count > 0) 
            {
                result += "Commandes : \n";
                foreach(Commande commande in commandes)
                {
                    result += commande + "\n";
                }
            }
            else
            {
                result += "Aucune commande\n";
            }
            result += "Nombres de commandes passées : " + nb_commandes + "\n";
            return result;
        }

        public override string ShortString()
        {
            return this.ID_Client + " " + this.Nom + " " + this.Prenom + " " + this.nb_commandes;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Client otherClient = obj as Client;
            if (otherClient != null)
                return this.Nom.CompareTo(otherClient.Nom);
            else
                throw new ArgumentException("Object is not a Client");
        }



    }
}
