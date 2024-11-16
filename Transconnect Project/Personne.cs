using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    abstract class Personne : Interface
    {
        protected string nom;
        protected string prenom;
        protected DateTime naissance;
        protected string adresse;
        protected string mail;
        protected string telephone;

        public Personne(string nom, string prenom, DateTime naissance, string adresse, string mail, string telephone)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.naissance = naissance;
            this.adresse = adresse;
            this.mail = mail;
            this.telephone = telephone;
        }


        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public DateTime Naissance
        {
            get { return naissance; }
            set { naissance = value; }
        }

        public string Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public override string ToString()
        {
            return "\nNom : " + nom + "\nPrénom : " + prenom + "\nDate de naissance : " + naissance.ToShortDateString() + "\nAdresse : " + adresse + "\nMail : " + mail + "\nTéléphone : " + telephone;
        }

        public abstract string ShortString();

    }
}
