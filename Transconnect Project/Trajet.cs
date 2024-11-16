using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Trajet : Interface
    {
        private Ville ville1;
        private Ville ville2;
        private int distance;
        private int duree;

        public Trajet(Ville ville1, Ville ville2, int distance, int duree)
        {
            this.ville1 = ville1;
            this.ville2 = ville2;
            this.distance = distance;
            this.duree = duree;
        }

        public Ville Ville1 
        { 
            get => ville1; 
            set => ville1 = value; 
        }
        public Ville Ville2 
        { 
            get => ville2; 
            set => ville2 = value; 
        }
        public int Distance 
        { 
            get => distance; 
            set => distance = value; 
        }
        public int Duree 
        { 
            get => duree; 
            set => duree = value; 
        }
        
        public override string ToString()
        {
            return "Trajet de " + ville1.Nom + " à " + ville2.Nom + " : " + distance + " km, " + duree + " minutes";
        }

    }
}
