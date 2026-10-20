using NUnit.Framework;
using Okosotthon;
using System;

[TestFixture]
public class OkosotthonTesztek
{
    private Termosztat termosztat;
    private OkosZar okosZar;
    private OkosotthonKozpont kozpont;

    [SetUp]
    public void Beallitas()
    {
        this.termosztat = new Termosztat("TH-01", "Nappali Termosztát", 22.0);
        this.okosZar = new OkosZar("LK-01", "Bejárati Zár", "1234");
        this.kozpont = new OkosotthonKozpont();
    }

    [Test]
    public void Kezdetben_MindenEszkoz_Offline()
    {
        Assert.That(this.termosztat.OnlineE, Is.False);
        Assert.That(this.okosZar.OnlineE, Is.False);
    }

    [Test]
    public void Csatlakozas_OnlineAllapotraValt()
    {
        this.termosztat.Csatlakozas();
        Assert.That(this.termosztat.OnlineE, Is.True);
    }

    [Test]
    public void KapcsolatBontasa_OfflineAllapotraValt()
    {
        this.termosztat.Csatlakozas();
        this.termosztat.KapcsolatBontasa();
        Assert.That(this.termosztat.OnlineE, Is.False);
    }

    [Test]
    public void Diagnosztika_OfflineEszkozon_HamissalTerVissza()
    {
        bool eredmeny = this.termosztat.DiagnosztikaFuttatasa();
        Assert.That(eredmeny, Is.False);
    }

    [Test]
    public void Diagnosztika_OnlineEszkozon_IgazzalTerVissza()
    {
        this.termosztat.Csatlakozas();
        bool eredmeny = this.termosztat.DiagnosztikaFuttatasa();
        Assert.That(eredmeny, Is.True);
    }

    [Test]
    public void Diagnosztika_ErvenytelenHomerseklethoz_HamissalTerVissza()
    {
        this.termosztat.Csatlakozas();
        this.termosztat.ParancsVegrehajtasa("BEALLIT_HOMERSEKLET:50.0");

        bool eredmeny = this.termosztat.DiagnosztikaFuttatasa();
        Assert.That(eredmeny, Is.False);
    }

    [Test]
    public void Termosztat_ParancsVegrehajtasa_FrissitiACelHomersekletet()
    {
        this.termosztat.ParancsVegrehajtasa("BEALLIT_HOMERSEKLET:24.5");
        Assert.That(this.termosztat.CelHomerseklet, Is.EqualTo(24.5));
    }

    [Test]
    public void OkosZar_Kezdetben_ZartAllapotbanVan()
    {
        Assert.That(this.okosZar.ZartE, Is.True);
    }

    [Test]
    public void OkosZar_HelyesPinkoddal_Kinyilik()
    {
        this.okosZar.ParancsVegrehajtasa("NYITAS:1234");
        Assert.That(this.okosZar.ZartE, Is.False);
    }

    [Test]
    public void OkosZar_HelytelenPinkoddal_ZarvaMarad()
    {
        this.okosZar.ParancsVegrehajtasa("NYITAS:0000");
        Assert.That(this.okosZar.ZartE, Is.True);
    }

    [Test]
    public void OkosZar_GyariBeallitasok_VisszaallitjaAPinkodotEsBezar()
    {
        this.okosZar.ParancsVegrehajtasa("NYITAS:1234");
        Assert.That(this.okosZar.ZartE, Is.False);

        this.okosZar.GyariBeallitasokVisszaallitasa();

        Assert.That(this.okosZar.ZartE, Is.True);

        // Az új alapértelmezett PIN-kódnak ("0000") működnie kell
        this.okosZar.ParancsVegrehajtasa("NYITAS:0000");
        Assert.That(this.okosZar.ZartE, Is.False);
    }

    [Test]
    public void Kozpont_OsszesCsatlakoztatasa_MindenEszkoztOnlineRendreAllit()
    {
        this.kozpont.EszkozHozzaadasa(this.termosztat);
        this.kozpont.EszkozHozzaadasa(this.okosZar);

        this.kozpont.OsszesCsatlakoztatasa();

        Assert.That(this.termosztat.OnlineE, Is.True);
        Assert.That(this.okosZar.OnlineE, Is.True);
    }

    [Test]
    public void Kozpont_DiagnosztikaFuttatasa_HelyesSikeresSzamotAdVissza()
    {
        this.kozpont.EszkozHozzaadasa(this.termosztat);
        this.kozpont.EszkozHozzaadasa(this.okosZar);

        // Még offline állapotban van minden -> 0 sikeres diagnosztika
        int sikeresOffline = this.kozpont.RendszerDiagnosztikaFuttatasa();
        Assert.That(sikeresOffline, Is.EqualTo(0));

        // Csatlakoztatás után mindkét eszköz sikeresen átmegy a teszten
        this.kozpont.OsszesCsatlakoztatasa();
        int sikeresOnline = this.kozpont.RendszerDiagnosztikaFuttatasa();
        Assert.That(sikeresOnline, Is.EqualTo(2));
    }
}