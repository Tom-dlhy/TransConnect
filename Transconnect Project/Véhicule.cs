using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_final
{
    internal class Véhicule : Interface
    {
        private string immatriculation;
        private string type;
        private double prix;
        private List<DateTime> dates_utilisation;

        public Véhicule(string immatriculation, string type, List<DateTime> dates_utilisation)
        {
            this.immatriculation = immatriculation;
            this.type = char.ToUpper(type[0]) + type.Substring(1);  //standardisation du type
            if (this.type == "Voiture")
            {
                this.prix = 50;
            }
            else if (this.type == "Camionnette")
            {
                this.prix = 100;
            }
            else if (this.type == "Camion")
            {
                this.prix = 150;
            }
            else
            {
                this.prix = 0;
            }

            this.dates_utilisation = dates_utilisation;
        }

        public Véhicule(string immatriculation, string type)
        {
            this.immatriculation = immatriculation;
            this.type = char.ToUpper(type[0]) + type.Substring(1);
            if (this.type == "Voiture")
            {
                this.prix = 50;
            }
            else if (this.type == "Camionnette")
            {
                this.prix = 100;
            }
            else if (this.type == "Camion")
            {
                this.prix = 150;
            }
            else
            {
                this.prix = 0;
            }

            this.dates_utilisation = new List<DateTime>();
        }

        public string Immatriculation
        {
            get { return immatriculation; }
            set { immatriculation = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public double Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        public List<DateTime> Dates_utilisation
        {
            get { return dates_utilisation; }
            set { dates_utilisation = value; }
        }

        public void AjouterDateUtilisation(DateTime date)
        {
            dates_utilisation.Add(date);
        }

        public void RetirerDateUtilisation(DateTime date)
        {
            dates_utilisation.Remove(date);
        }

        public bool DateDisponible(DateTime date)
        {
            bool test = true;
            if (dates_utilisation.Count == 0)
            {
                test = true;
            }
            else
            {
                foreach (DateTime date_utilisation in dates_utilisation)
                {
                    if (date_utilisation.Year == date.Year && date_utilisation.Day == date.Day && date.Month == date_utilisation.Month)
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
            string result =  "\nImmatriculation : " + immatriculation + "\nType : " + type + "\nPrix : " + prix;
            if (dates_utilisation.Count == 0)
            {
                result += "\nDates d'utilisation : Aucune";
            }
            else
            {
                result += "\nDates d'utilisation : ";
                foreach (DateTime date in dates_utilisation)
                {
                    result += "\n\t" + date.ToShortDateString();
                }
            }
            return result;
        }
    }
}
