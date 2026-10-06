using System;
using System.Collections.Generic;
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
            JelenlegiHomerseklet = 21.0;
            this.CelHomerseklet = celHomerseklet;
        }

        public override void ParancsVegrehajtasa(string parancs)
        {
            throw new NotImplementedException();
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

