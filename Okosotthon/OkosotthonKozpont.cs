using System;
using System.Collections.Generic;
using System.Text;

namespace Okosotthon
{
    public class OkosotthonKozpont
    {
        private readonly List<OkosEszkoz> eszkozok = new List<OkosEszkoz>();
        public void EszkozHozzaadasa(OkosEszkoz eszkoz)
        {
            eszkozok.Add(eszkoz);
        }

     
        public void OsszesCsatlakoztatasa()
        {
            foreach (OkosEszkoz eszkoz in eszkozok)
            {
                eszkoz.Csatlakozas();
            }
        }

        public int RendszerDiagnosztikaFuttatasa()
        {
            int sikeresTesztek = 0;
            foreach (OkosEszkoz eszkoz in eszkozok)
            {
                if (eszkoz.DiagnosztikaFuttatasa())
                {
                    sikeresTesztek++ ;
                }
                
            }

            return sikeresTesztek;
        }

        public void OsszesVeszjelzesInditasa()
        {
            foreach (OkosEszkoz eszkoz in eszkozok)
            {
                if (eszkoz is IHangjelzo h)
                {
                    h.VeszjelzesInditasa();
                }
            }
        }

        public List<string> OsszesStatisztikaLekerdezese()
        {
            List<string> statisztikak = new List<string>();

            foreach (OkosEszkoz eszkoz in eszkozok)
            {
                if (eszkoz is IStatisztika s)
                {
                    statisztikak.Add(s.StatisztikatKeszit());
                }
            }
            return statisztikak;
        }
    }
}
