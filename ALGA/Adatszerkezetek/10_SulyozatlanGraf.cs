using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 10. heti labor feladat - Tesztek: 10_SulyozatlanGrafTesztek.cs

    public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public int Honnan { get; }
        public int Hova { get; }

        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public int CompareTo(EgeszGrafEl? other)
        {
            if (Honnan != other!.Honnan)
                return Honnan.CompareTo(other.Honnan);
            return Hova.CompareTo(other.Hova);
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        private int n;
        private bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n, n];
        }

        public int CsucsokSzama => n;

        public int ElekSzama
        {
            get
            {
                int db = 0;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (M[i, j])
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

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                Halmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (M[i, j])
                            elek.Beszur(new EgeszGrafEl(i, j));
                return elek;
            }
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return M[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            Halmaz<int> szomszedok = new FaHalmaz<int>();
            for (int j = 0; j < n; j++)
                if (M[csucs, j])
                    szomszedok.Beszur(j);
            return szomszedok;
        }
    }

    public class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            Halmaz<V> elert = new FaHalmaz<V>();
            Sor<V> sor = new LancoltSor<V>();
            elert.Beszur(start);
            sor.Sorba(start);

            while (!sor.Ures)
            {
                V k = sor.Sorbol();
                muvelet(k);
                g.Szomszedai(k).Bejar(x =>
                {
                    if (!elert.Eleme(x))
                    {
                        elert.Beszur(x);
                        sor.Sorba(x);
                    }
                });
            }

            return elert;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            Halmaz<V> F = new FaHalmaz<V>();
            MelysegiBejarasRekurzio(g, start, F, muvelet);
            return F;
        }

        public static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet(k);
            g.Szomszedai(k).Bejar(x =>
            {
                if (!F.Eleme(x))
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
            });
        }
    }
}
