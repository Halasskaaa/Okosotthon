using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Okosotthon
{
    public class OkosZar: OkosEszkoz
    {
        private bool ZartE;
        private string pinKod;

        public bool ZartE1 { get; private set; }
        public string PinKod;

        public OkosZar(string azonosito, string nev, string pinKod)
        : base(azonosito, nev)
        {
            this.ZartE = true;
            this.pinKod = pinKod;
        }

        public override void ParancsVegrehajtasa(string parancs)
        {
            if (parancs == "ZARAS")
            {
                this.ZartE = true;
            }
            else if (parancs != null && parancs.StartsWith("NYITAS:"))
            {
                string megadottPin = parancs.Substring("NYITAS:".Length);
                if (megadottPin == this.pinKod)
                {
                    this.ZartE = false;
                }
            }          
        }

        public override string AllapotJelentes()
        {
            return this.ZartE ? "Zárt állapotban van." : "Nyitott állapotban van.";
        }

        protected override bool OnTesztFuttatasa()
        {
            return true;
        }

        public override void GyariBeallitasokVisszaallitasa()
        {
            base.GyariBeallitasokVisszaallitasa();
            this.pinKod = "0000";
            this.ZartE = true;
        }
    }
}
