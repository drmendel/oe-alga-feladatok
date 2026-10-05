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
}
