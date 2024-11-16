using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Ville : Interface
    {

        private string nom;
        private List<Trajet> trajets;

        public Ville(string nom)
        {
            this.nom = nom;
            this.trajets = new List<Trajet>();
        }

        public string Nom 
        { 
            get => nom; 
            set => nom = value; 
        }
        public List<Trajet> Trajets 
        { 
            get => trajets; 
            set => trajets = value; 
        }

        public void AjouterTrajet(Trajet trajet)
        {
            this.trajets.Add(trajet);
        }

        public void SupprimerTrajet(Trajet trajet)
        {
            this.trajets.Remove(trajet);
        }

        public override string ToString()
        {
            return nom;
        }

    }
}
