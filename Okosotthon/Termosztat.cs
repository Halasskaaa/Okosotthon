using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Okosotthon
{
    public class Termosztat : OkosEszkoz
    {
        private double JelenlegiHomerseklet;
        private double CelHomerseklet;

        public double JelenlegiHomerseklet1 { get => JelenlegiHomerseklet; private set => JelenlegiHomerseklet = value; }
        public double CelHomerseklet1 { get => CelHomerseklet; private set => CelHomerseklet = value; }

        public Termosztat(string azonosito, string nev, double celHomerseklet)
        : base(azonosito, nev)
        {
            this.JelenlegiHomerseklet = 21.0;
            this.CelHomerseklet = celHomerseklet;
        }

        public override void ParancsVegrehajtasa(string parancs)
        {
            string parancsE = "BEALLIT_HOMERSEKLET:";

            if (parancs != null || parancs.StartsWith(parancsE))
            {
                string ertek = parancs.Substring(parancsE.Length);
                this.CelHomerseklet = Convert.ToDouble(ertek);
            }
        }

        public override string AllapotJelentes()
        {
            return $"Jelenlegi hőmérséklet: {this.JelenlegiHomerseklet1} °C, Cél hőmérséklet: {this.CelHomerseklet1} °C";
        }

        protected override bool OnTesztFuttatasa()
        {
            if (this.CelHomerseklet1 > 5.0 && this.CelHomerseklet1 < 35.0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

