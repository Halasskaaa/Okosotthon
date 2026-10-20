using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Okosotthon
{
    public class OkosZar : OkosEszkoz, IHangjelzo, IStatisztika
    {
        private string pinKod;
        private int nyitasokSzama;
        public bool ZartE { get; private set; }

        public OkosZar(string azonosito, string nev, string pinKod)
            : base(azonosito, nev)
        {
            this.ZartE = true;
            this.pinKod = pinKod;
            this.nyitasokSzama = 0;
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
                    if(this.ZartE)
                    {
                        this.nyitasokSzama++;
                    }
                    this.ZartE = false;
                } else
                {
                    Csipog(200);
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
            this.nyitasokSzama = 0;
        }

        public void Csipog(int hosszu)
        {
            if (hosszu <= 0)
            {
                return;
            }
            try
            {
                Console.Beep(800, hosszu);
            }
            catch (PlatformNotSupportedException)
            {
                Console.WriteLine($"Csipogás: {hosszu} ms");
            }
        }
        public void VeszjelzesInditasa()
        {
            Console.WriteLine("VÉSZJELZÉS! Illetéktelen hozzáférés gyanúja.");
        }
        public string StatisztikatKeszit()
        {
            return $"Okoszár statisztika: Sikeres nyitások száma: {nyitasokSzama}.";
        }
    }
}