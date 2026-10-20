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

    [Test]
    public void Termosztat_IdozitoBeallitasa_VisszaadjaAMaradekIdot()
    {
        IIdozitheto idozito = this.termosztat;

        idozito.IdozitestBeallit(5);

        Assert.That(idozito.GetMaradtIdo(), Is.EqualTo(5));
    }

    [Test]
    public void Kozpont_StatisztikaLekerdezese_MindenStatisztikatVisszaad()
    {
        this.kozpont.EszkozHozzaadasa(this.termosztat);
        this.kozpont.EszkozHozzaadasa(this.okosZar);

        var statisztikak = this.kozpont.OsszesStatisztikaLekerdezese();

        Assert.That(statisztikak, Has.Count.EqualTo(2));
    }

    [Test]
    public void Termosztat_ImplementaljaAzUjInterfeszket()
    {
        Assert.That(termosztat, Is.InstanceOf<IIdozitheto>());
        Assert.That(termosztat, Is.InstanceOf<IStatisztika>());
    }

    [Test]
    public void Termosztat_IdozitoBeallitasa_Mukodik()
    {
        IIdozitheto idozito = termosztat;

        idozito.IdozitestBeallit(5);

        Assert.That(idozito.GetMaradtIdo(), Is.InRange(1, 5));
    }

    [Test]
    public void Termosztat_NullaPercesIdozito_NullaMaradekIdotAd()
    {
        IIdozitheto idozito = termosztat;

        idozito.IdozitestBeallit(0);

        Assert.That(idozito.GetMaradtIdo(), Is.EqualTo(0));
    }

    [Test]
    public void Termosztat_Statisztika_TartalmazzaAHomersekleteket()
    {
        string statisztika = termosztat.StatisztikatKeszit();

        Assert.That(statisztika, Does.Contain("21"));
        Assert.That(statisztika, Does.Contain("22"));
    }

    [Test]
    public void OkosZar_ImplementaljaAzUjInterfeszket()
    {
        Assert.That(okosZar, Is.InstanceOf<IHangjelzo>());
        Assert.That(okosZar, Is.InstanceOf<IStatisztika>());
    }

    [Test]
    public void OkosZar_Statisztika_SikeresNyitasokatSzamol()
    {
        okosZar.ParancsVegrehajtasa("NYITAS:1234");
        okosZar.ParancsVegrehajtasa("NYITAS:1234");
        okosZar.ParancsVegrehajtasa("ZARAS");
        okosZar.ParancsVegrehajtasa("NYITAS:1234");

        Assert.That(
            okosZar.StatisztikatKeszit(),
            Does.Contain("2"));
    }

    [Test]
    public void OkosZar_GyariVisszaallitas_NullazzaANyitasokSzamat()
    {
        okosZar.ParancsVegrehajtasa("NYITAS:1234");
        okosZar.GyariBeallitasokVisszaallitasa();

        Assert.That(okosZar.StatisztikatKeszit(), Does.Contain("0"));
    }

    [Test]
    public void OkosZar_VeszjelzesInditasa_KiirjaARiasztast()
    {
        StringWriter kimenet = new StringWriter();
        TextWriter eredetiKimenet = Console.Out;

        try
        {
            Console.SetOut(kimenet);
            okosZar.VeszjelzesInditasa();
        }
        finally
        {
            Console.SetOut(eredetiKimenet);
        }

        Assert.That(kimenet.ToString(), Does.Contain("VÉSZJELZÉS"));
    }

    [Test]
    public void KozpontiRiaszto_ImplementaljaMindharomInterfeszt()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        Assert.That(riaszto, Is.InstanceOf<IIdozitheto>());
        Assert.That(riaszto, Is.InstanceOf<IHangjelzo>());
        Assert.That(riaszto, Is.InstanceOf<IStatisztika>());
    }

    [Test]
    public void KozpontiRiaszto_IdozitoBeallitasa_Mukodik()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        IIdozitheto idozito = riaszto;
        idozito.IdozitestBeallit(3);

        Assert.That(idozito.GetMaradtIdo(), Is.InRange(1, 3));
    }

    [Test]
    public void KozpontiRiaszto_VeszjelzesAktivaljaARiasztast()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        riaszto.VeszjelzesInditasa();

        Assert.That(riaszto.AllapotJelentes(), Does.Contain("aktív"));
    }

    [Test]
    public void KozpontiRiaszto_Leallitas_InaktivAllapototAd()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        riaszto.ParancsVegrehajtasa("VESZJELZES");
        riaszto.ParancsVegrehajtasa("LEALLITAS");

        Assert.That(riaszto.AllapotJelentes(), Does.Contain("inaktív"));
    }

    [Test]
    public void KozpontiRiaszto_StatisztikaSzamoljaAVeszjelzeseket()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        riaszto.VeszjelzesInditasa();

        Assert.That(
            riaszto.StatisztikatKeszit(),
            Does.Contain("Vészjelzések: 1"));
    }

    [Test]
    public void Kozpont_OsszesVeszjelzesInditasa_AktivaljaARiasztot()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        kozpont.EszkozHozzaadasa(okosZar);
        kozpont.EszkozHozzaadasa(riaszto);

        kozpont.OsszesVeszjelzesInditasa();

        Assert.That(riaszto.AllapotJelentes(), Does.Contain("aktív"));
    }

    [Test]
    public void Kozpont_StatisztikaLekerdezese_MindenEszkozStatisztikajatVisszaadja()
    {
        OkosKozpontiRiaszto riaszto =
            new OkosKozpontiRiaszto("RI-01", "Központi riasztó");

        kozpont.EszkozHozzaadasa(termosztat);
        kozpont.EszkozHozzaadasa(okosZar);
        kozpont.EszkozHozzaadasa(riaszto);

        var statisztikak = kozpont.OsszesStatisztikaLekerdezese();

        Assert.That(statisztikak, Has.Count.EqualTo(3));
    }
}