using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 12-13. heti labor feladat - Tesztek: 12_SulyozottGrafTesztek.cs

    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public float Suly { get; }

        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            Suly = suly;
        }
    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        private int n;
        private float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    M[i, j] = float.NaN;
        }

        public int CsucsokSzama => n;

        public int ElekSzama
        {
            get
            {
                int db = 0;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (!float.IsNaN(M[i, j]))
                            db++;
                return db;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                Halmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                    csucsok.Beszur(i);
                return csucsok;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                Halmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (!float.IsNaN(M[i, j]))
                            elek.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j]));
                return elek;
            }
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(M[honnan, hova]);
        }

        public float Suly(int honnan, int hova)
        {
            if (float.IsNaN(M[honnan, hova]))
                throw new NincsElKivetel();
            return M[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            Halmaz<int> szomszedok = new FaHalmaz<int>();
            for (int j = 0; j < n; j++)
                if (!float.IsNaN(M[csucs, j]))
                    szomszedok.Beszur(j);
            return szomszedok;
        }
    }

    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable
        {
            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            g.Csucsok.Bejar(x => K.Beir(x, float.PositiveInfinity));
            K.Beir(start, 0);

            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (x, y) => K.Kiolvas(x) < K.Kiolvas(y));
            g.Csucsok.Bejar(x => S.Sorba(x));

            while (!S.Ures)
            {
                V u = S.Sorbol();
                g.Szomszedai(u).Bejar(x =>
                {
                    float ujHossz = K.Kiolvas(u) + g.Suly(u, x);
                    if (ujHossz < K.Kiolvas(x))
                    {
                        K.Beir(x, ujHossz);
                        S.Frissit(x);
                    }
                });
            }

            return K;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable
        {
            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            Szotar<V, V> Sz = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            g.Csucsok.Bejar(x => K.Beir(x, float.PositiveInfinity));
            K.Beir(start, 0);

            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (x, y) => K.Kiolvas(x) < K.Kiolvas(y));
            Halmaz<V> sorban = new FaHalmaz<V>();
            g.Csucsok.Bejar(x =>
            {
                S.Sorba(x);
                sorban.Beszur(x);
            });

            while (!S.Ures)
            {
                V u = S.Sorbol();
                sorban.Torol(u);
                g.Szomszedai(u).Bejar(x =>
                {
                    if (sorban.Eleme(x) && g.Suly(u, x) < K.Kiolvas(x))
                    {
                        Sz.Beir(x, u);
                        K.Beir(x, g.Suly(u, x));
                        S.Frissit(x);
                    }
                });
            }

            return Sz;
        }

        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g, V start = default!) where V : IComparable where E : SulyozottGrafEl<V>, IComparable
        {
            Halmaz<E> F = new FaHalmaz<E>();
            Szotar<V, int> komponens = new HasitoSzotarTulcsordulasiTerulettel<V, int>(g.CsucsokSzama);
            int sorszam = 0;
            g.Csucsok.Bejar(x =>
            {
                komponens.Beir(x, sorszam);
                sorszam++;
            });

            PrioritasosSor<E> S = new KupacPrioritasosSor<E>(g.ElekSzama, (x, y) => x.Suly < y.Suly);
            g.Elek.Bejar(e => S.Sorba(e));

            while (!S.Ures)
            {
                E el = S.Sorbol();
                int honnanKomponens = komponens.Kiolvas(el.Honnan);
                int hovaKomponens = komponens.Kiolvas(el.Hova);
                if (honnanKomponens != hovaKomponens)
                {
                    F.Beszur(el);
                    g.Csucsok.Bejar(x =>
                    {
                        if (komponens.Kiolvas(x) == hovaKomponens)
                            komponens.Beir(x, honnanKomponens);
                    });
                }
            }

            return F;
        }
    }
}
