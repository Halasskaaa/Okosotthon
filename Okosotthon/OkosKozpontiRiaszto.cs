using System;
using System.Collections.Generic;
using System.Text;

namespace Okosotthon
{
    public class OkosKozpontiRiaszto : OkosEszkoz, IIdozitheto, IHangjelzo, IStatisztika
    {
        private DateTime idozitesVege;
        private bool idozitesAktiv;
        private bool veszjelzesAktiv;
        private int csipogasokSzama;
        private int veszjelzesekSzama;

        public OkosKozpontiRiaszto(string azonosito, string nev)
            : base(azonosito, nev)
        {
            idozitesAktiv = false;
            veszjelzesAktiv = false;
            csipogasokSzama = 0;
            veszjelzesekSzama = 0;
        }

        public override void ParancsVegrehajtasa(string parancs)
        {
            if (parancs == "VESZJELZES")
            {
                VeszjelzesInditasa();
            }
            else if (parancs == "LEALLITAS")
            {
                veszjelzesAktiv = false;
            }
            else if (parancs == "CSIPOG")
            {
                Csipog(200);
            }
        }

        public override string AllapotJelentes()
        {
            return veszjelzesAktiv
                ? "A központi vészjelző aktív."
                : "A központi vészjelző inaktív.";
        }

        protected override bool OnTesztFuttatasa()
        {
            return true;
        }

        public void IdozitestBeallit(int perc)
        {
            idozitesVege = DateTime.Now.AddMinutes(perc);
            idozitesAktiv = perc > 0;
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

        public void Csipog(int hosszu)
        {
            if (hosszu <= 0)
            {
                return;
            }

            csipogasokSzama++;

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
            veszjelzesAktiv = true;
            veszjelzesekSzama++;

            Console.WriteLine("VÉSZJELZÉS! A központi riasztó aktiválódott.");
        }

        public string StatisztikatKeszit()
        {
            return $"Központi riasztó statisztika: Csipogások: {csipogasokSzama}, Vészjelzések: {veszjelzesekSzama}.";
        }

        public override void GyariBeallitasokVisszaallitasa()
        {
            base.GyariBeallitasokVisszaallitasa();

            idozitesAktiv = false;
            veszjelzesAktiv = false;
            csipogasokSzama = 0;
            veszjelzesekSzama = 0;
        }
    }
}