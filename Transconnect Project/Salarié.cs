using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Salarié : Personne, Interface, IComparable
    {
        private string nSS;
        private DateTime dateEntree;
        private string poste;
        private double salaire;
        private string nSS_responsable;

        public Salarié(string nom, string prenom, DateTime naissance, string adresse, string mail, string telephone, DateTime dateEntree, string poste, double salaire) : base(nom, prenom, naissance, adresse, mail, telephone)
        {
            this.dateEntree = dateEntree;
            this.poste = poste;
            this.salaire = salaire;
            this.nSS_responsable = "";
            this.nSS = "";
            Random sex = new Random();
            this.nSS += sex.Next(1, 3);
            this.nSS += naissance.Year.ToString().Substring(2, 2);
            if (naissance.Month < 10)
            {
                this.nSS += "0" + naissance.Month;
            }
            else
            {
                this.nSS += naissance.Month + " ";
            }
            Random departement = new Random();
            int dep = departement.Next(1, 100);
            if (dep < 10)
            {
                this.nSS += "0" + dep;
            }
            else
            {
                this.nSS += dep;
            }
            Random commune = new Random();
            int com = commune.Next(1, 999);
            if (com < 10)
            {
                this.nSS += "00" + com;
            }
            else if (com < 100)
            {
                this.nSS += "0" + com;
            }
            else
            {
                this.nSS += com;
            }
            Random ordres = new Random();
            int ordre = commune.Next(1, 999);
            if (ordre < 100)
            {
                this.nSS += "0" + ordre;
            }
            else if (ordre < 10)
            {
                this.nSS += "00" + ordre;
            }
            else
            {
                this.nSS += ordre;
            }
            Random cles = new Random();
            int cle = cles.Next(1, 99);
            if (cle < 10)
            {
                this.nSS += "0" + cle;
            }
            else
            {
                this.nSS += cle;
            }
        }

        public Salarié(string nSS, string nom, string prenom, DateTime naissance, string adresse, string mail, string telephone, DateTime dateEntree, string poste, double salaire, string nSS_responsable) : base(nom, prenom, naissance, adresse, mail, telephone)
        {
            this.nSS = nSS;
            this.dateEntree = dateEntree;
            this.poste = poste;
            this.salaire = salaire;
            this.NSS_responsable = nSS_responsable;
        }

        public Salarié(string nSS, string nom, string prenom, DateTime naissance, string adresse, string mail, string telephone, DateTime dateEntree, string poste, double salaire) : base(nom, prenom, naissance, adresse, mail, telephone)
        {
            this.nSS = nSS;
            this.dateEntree = dateEntree;
            this.poste = poste;
            this.salaire = salaire;
            this.NSS_responsable = "";
        }

        public string NSS
        {
            get { return nSS; }
            set { nSS = value; }
        }

        public DateTime DateEntree
        {
            get { return dateEntree; }
            set { dateEntree = value; }
        }

        public string Poste
        {
            get { return poste; }
            set { poste = value; }
        }

        public double Salaire
        {
            get { return salaire; }
            set { salaire = value; }
        }

        public string NSS_responsable
        {
            get { return nSS_responsable; }
            set { nSS_responsable = value; }
        }

        public override string ToString()
        {
            return "\nnSS : " + nSS + base.ToString() + "\nDate d'entrée : " + dateEntree.ToShortDateString() + "\nPoste : " + poste + "\nSalaire : " + salaire + "\nnSS responsable : " + this.nSS_responsable;
        }

        public override string ShortString()
        {
            return this.Nom + " " + this.Prenom;
        }

        public int CompareTo(object obj)
        {
            Salarié salarié = obj as Salarié;
            if (salarié != null)
            {
                return this.Nom.CompareTo(salarié.Nom);
            }
            else
            {
                throw new ArgumentException("Object is not a Salarié");
            }
        }

    }
}
