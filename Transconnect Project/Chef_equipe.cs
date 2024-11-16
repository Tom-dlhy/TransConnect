using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Chef_equipe : Salarié, Interface
    {
        private List<Chauffeur> chauffeurs;
        private int nombre_chauffeurs_max;

        public Chef_equipe(Salarié salarié) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chauffeurs = new List<Chauffeur>();
            this.nombre_chauffeurs_max = 3;
        }

        public Chef_equipe(Salarié salarié, List<Chauffeur> chauffeurs) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chauffeurs = chauffeurs;
            this.nombre_chauffeurs_max = 3;
        }

        public Chef_equipe(Salarié salarié, int nombre_chauffeurs_max, List<Chauffeur> chauffeurs) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chauffeurs = chauffeurs;
            this.nombre_chauffeurs_max = nombre_chauffeurs_max;
        }

        public Chef_equipe(Salarié salarié, int nombre_chauffeurs_max) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chauffeurs = new List<Chauffeur>();
            this.nombre_chauffeurs_max = nombre_chauffeurs_max;
        }

        public List<Chauffeur> Chauffeurs
        {
            get { return chauffeurs; }
            set { chauffeurs = value; }
        }

        public int Nombre_chauffeurs_max
        {
            get { return nombre_chauffeurs_max; }
            set { nombre_chauffeurs_max = value; }
        }

        public void AjouterChauffeur(Chauffeur chauffeur)
        {
            bool test = true;

            foreach (Chauffeur chauffeur1 in chauffeurs)
            {
                if (chauffeur1.NSS == chauffeur.NSS)
                {
                    test = false;
                }
            }

            if (test != false)
            {
                if (chauffeurs.Count < nombre_chauffeurs_max)
                {
                    chauffeurs.Add(chauffeur);
                }
                else
                {
                    Console.WriteLine("Le nombre maximum de chauffeurs est atteint veuillez choisir un autre chef d'équipe");
                }
            }
        }

        public void SupprimerChauffeur(Chauffeur chauffeur)
        {
            chauffeurs.Remove(chauffeur);
            chauffeur.NSS_responsable = "";
            chauffeur.Chef_equipe = null;
        }

        public override string ToString()
        {
            string chauffeursString = this.NSS + " " + base.ShortString() + "\nEquipe :\n\n";
            foreach (Chauffeur chauffeur in chauffeurs)
            {
                chauffeursString += chauffeur.ToString() + "\n\n";
            }
            return chauffeursString;
        }

        public string ShortString()
        {
            return this.NSS + " " + this.Nom + " " + this.Prenom;
        }


    }
}
