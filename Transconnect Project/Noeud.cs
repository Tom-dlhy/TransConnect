using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Noeud
    {
        private Personne personne;
        private Noeud parent;
        private List<Noeud> enfants;

        public Noeud(Personne personne)
        {
            this.personne = personne;
            this.parent = null;
            this.enfants = new List<Noeud>();
        }
        public Personne Personne 
        { 
            get => personne; 
            set => personne = value; 
        }
        public Noeud Parent 
        { 
            get => parent; 
            set => parent = value; 
        }
        public List<Noeud> Enfants 
        { 
            get => enfants; 
            set => enfants = value; 
        }

        public Noeud AjouterEnfant(Personne personne)
        {
            Noeud enfant = new Noeud(personne);
            this.enfants.Add(enfant);
            enfant.Parent = this;
            return enfant;
        }

    }
}
