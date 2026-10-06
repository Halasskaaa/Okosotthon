using System;
using System.Collections.Generic;
using System.Text;

namespace Okosotthon
{
    public abstract class OkosEszkoz
    {
        private string azonosito;
        private string nev;
        private bool onlineE;
        private DateTime utolsoFrissites;

        public OkosEszkoz(string azonosito, string nev)
        {
            throw new NotImplementedException();
        }

        public void Csatlakozas()
        {
            throw new NotImplementedException();
        }

        public void KapcsolatBontasa()
        {
            throw new NotImplementedException();
        }
        public bool DiagnosztikaFuttatasa()
        {
            throw new NotImplementedException();
        }

        public virtual void GyariBeallitasokVisszaallitasa()
        {
            throw new NotImplementedException();
        }

        public abstract void ParancsVegrehajtasa(string parancs);
        public abstract string AllapotJelentes();
        protected abstract bool OnTesztFuttatasa();
    }
}
