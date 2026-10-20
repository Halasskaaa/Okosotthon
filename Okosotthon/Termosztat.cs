using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Globalization;

namespace Okosotthon
{
    public class Termosztat : OkosEszkoz, IIdozitheto, IStatisztika
    {
        private double jelenlegiHomerseklet;
        private double celHomerseklet;
        private DateTime idozitesVege;
        private bool idozitesAktiv;

        public double JelenlegiHomerseklet
        {
            get => jelenlegiHomerseklet;
            private set => jelenlegiHomerseklet = value;
        }

        public double CelHomerseklet
        {
            get => celHomerseklet;
            private set => celHomerseklet = value;
        }

        public Termosztat(string azonosito, string nev, double celHomerseklet) : base(azonosito, nev)
        {
            this.JelenlegiHomerseklet = 21.0;
            this.CelHomerseklet = celHomerseklet;
        }

        public override void ParancsVegrehajtasa(string parancs)
        {
            string parancsE = "BEALLIT_HOMERSEKLET:";

            if (parancs != null && parancs.StartsWith(parancsE))
            {
                string ertek = parancs.Substring(parancsE.Length);

                if (double.TryParse(ertek, NumberStyles.Float,
                    CultureInfo.InvariantCulture, out double homerseklet))
                {
                    this.CelHomerseklet = homerseklet;
                }
            }
        }

        public override string AllapotJelentes()
        {
            return $"Jelenlegi hőmérséklet: {this.JelenlegiHomerseklet} °C, Cél hőmérséklet: {this.CelHomerseklet} °C";
        }

        protected override bool OnTesztFuttatasa()
        {
            return this.CelHomerseklet > 5.0 && this.CelHomerseklet < 35.0;
        }

        public void IdozitestBeallit(int perc)
        {
            idozitesVege = DateTime.Now.AddMinutes(perc);
            idozitesAktiv = true;
        }
        public int GetMaradtIdo()
        {
            if (!idozitesAktiv)
            {
                return 0;
            }
            
            double maradekPerc = (idozitesVege - DateTime.Now).TotalMinutes;

            if (maradekPerc <= 0)
            {
                idozitesAktiv = false;
                return 0;
            }

            return (int)Math.Ceiling(maradekPerc);
        }

        public string StatisztikatKeszit()
        {
            return $"Termosztát statisztika: Jelenlegi hőmérséklet: {JelenlegiHomerseklet} °C, Célhőmérséklet: {CelHomerseklet} °C.";
        }
    }
}