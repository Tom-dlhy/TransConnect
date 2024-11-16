using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Chauffeur: Salarié, Interface
    {
        private Chef_equipe chef_equipe;
        private List<DateTime> dates_livraison;
        private int nb_commandes_livres;

        public Chauffeur(int nb_commandes_livres, Salarié salarié, List<DateTime> dates_livraison) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.dates_livraison = dates_livraison;
            this.nb_commandes_livres = nb_commandes_livres;
        }   //création d'un chauffeur à partir du fichier csv

        public Chauffeur(Salarié salarié, Chef_equipe chef_equipe, List<DateTime> dates_livraison) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chef_equipe = chef_equipe;
            this.dates_livraison = dates_livraison;
            this.NSS_responsable = chef_equipe.NSS;
            this.nb_commandes_livres = 0;
        }


        public Chauffeur(int nb_commandes_livres, Salarié salarié, Chef_equipe chef_equipe, List<DateTime> dates_livraison) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chef_equipe = chef_equipe;
            this.dates_livraison = dates_livraison;
            this.NSS_responsable = chef_equipe.NSS;
            this.nb_commandes_livres = nb_commandes_livres;
        }   //création d'un chauffeur à partir du fichier csv

        public Chauffeur(Salarié salarié, Chef_equipe chef_equipe) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chef_equipe = chef_equipe;
            this.NSS_responsable = chef_equipe.NSS;
            this.dates_livraison = new List<DateTime>();
            this.nb_commandes_livres = 0;
        }   //création d'un chauffeur dans le menu chauffeur

        public Chauffeur(int nb_commandes_livres, Salarié salarié, Chef_equipe chef_equipe) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.chef_equipe = chef_equipe;
            this.NSS_responsable = chef_equipe.NSS;
            this.dates_livraison = new List<DateTime>();    
            this.nb_commandes_livres = nb_commandes_livres;
        }   //création d'un chauffeur à partir du fichier csv

        public Chauffeur(Salarié salarié) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.dates_livraison = new List<DateTime>();
            this.nb_commandes_livres = 0;
        }   //création d'un chauffeur dans le menu chauffeur

        public Chauffeur(int nb_commandes_livres, Salarié salarié) : base(salarié.NSS, salarié.Nom, salarié.Prenom, salarié.Naissance, salarié.Adresse, salarié.Mail, salarié.Telephone, salarié.DateEntree, salarié.Poste, salarié.Salaire, salarié.NSS_responsable)
        {
            this.dates_livraison = new List<DateTime>();
            this.nb_commandes_livres = nb_commandes_livres;
        }   //création d'un chauffeur à partir du fichier csv


        public List<DateTime> Dates_livraison
        {
            get { return dates_livraison; }
            set { dates_livraison = value; }
        }

        public Chef_equipe Chef_equipe
        {
            get { return chef_equipe; }
            set { chef_equipe = value; }
        }

        public int Nb_commandes_livres
        {
            get { return nb_commandes_livres; }
            set { nb_commandes_livres = value; }
        }

        public void AjouterDateLivraison(DateTime date, int temps_trajet)
        {
            float nb_jours = temps_trajet/1440;
            float roundedNumber = (float)Math.Floor(nb_jours);
            if (temps_trajet > 0)
            {
                dates_livraison.Add(date);
                for (int i = 0; i < roundedNumber; i++)
                {
                    dates_livraison.Add(date.AddDays(i));
                }
            }
        }

        public void RetirerDateLivraison(DateTime date)
        {
            dates_livraison.Remove(date);
        }

        public bool DateDisponible(DateTime date)
        {
            bool test = true;
            if(dates_livraison.Count == 0)
            {
                test = true;
            }
            else
            {
                foreach (DateTime date_livraison in dates_livraison)
                {
                    if (date_livraison.Year == date.Year && date_livraison.Day == date.Day && date.Month == date_livraison.Month)
                    {
                        test = false;
                        return test;
                    }
                }
            }
            return test;
        }

        public override string ToString()
        {
            string result = base.ShortString();
            if(chef_equipe != null)
            {
                result += "\nChef d'équipe: " + chef_equipe.ShortString();
            }
            else
            {
                result += "\nChef d'équipe: Aucun";
            }
            result += "\nLivraisons en cours:";
            if(dates_livraison.Count == 0)
            {
                result += " Aucune livraison en cours";
            }
            else
            {
                foreach (DateTime date in dates_livraison)
                {
                    result += "\n\t" + date.ToShortDateString();
                }
            }
            return result;
        }

        public string ShortString()
        {
            return NSS + " " + Nom + " " + Prenom;
        }
    }
}
