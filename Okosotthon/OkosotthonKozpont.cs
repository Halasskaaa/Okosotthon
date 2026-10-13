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
                sikeresTesztek++ ;
            }

            return sikeresTesztek;
        }
    }
}
