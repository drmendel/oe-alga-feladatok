using System;

namespace OE.ALGA.Optimalizalas
{
    // 9. heti labor feladat - Tesztek: 09_VisszalepesesKeresesTesztek.cs

    public class VisszalepesesOptimalizacio<T>
    {
        protected int n;
        protected int[] M;
        protected T[,] R;
        protected Func<int, T, bool> ft;
        protected Func<int, T, T[], bool> fk;
        protected Func<T[], double> josag;

        public int LepesSzam { get; protected set; }

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], double> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
            LepesSzam = 0;
        }

        public T[] OptimalisMegoldas()
        {
            T[] E = new T[n];
            T[] O = new T[n];
            bool van = false;
            LepesSzam = 0;
            Backtrack(0, E, ref van, ref O);
            return O;
        }

        protected virtual void Backtrack(int szint, T[] E, ref bool van, ref T[] O)
        {
            for (int i = 0; i < M[szint]; i++)
            {
                LepesSzam++;
                if (ft(szint, R[szint, i]) && fk(szint, R[szint, i], E))
                {
                    E[szint] = R[szint, i];
                    if (szint == n - 1)
                    {
                        if (!van || josag(E) > josag(O))
                        {
                            van = true;
                            for (int j = 0; j < n; j++)
                                O[j] = E[j];
                        }
                    }
                    else
                        Backtrack(szint + 1, E, ref van, ref O);
                }
            }
        }
    }

    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        protected Func<int, T[], double> fb;

        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], double> josag, Func<int, T[], double> fb)
            : base(n, M, R, ft, fk, josag)
        {
            this.fb = fb;
        }

        protected override void Backtrack(int szint, T[] E, ref bool van, ref T[] O)
        {
            for (int i = 0; i < M[szint]; i++)
            {
                LepesSzam++;
                if (ft(szint, R[szint, i]) && fk(szint, R[szint, i], E))
                {
                    E[szint] = R[szint, i];
                    if (szint == n - 1)
                    {
                        if (!van || josag(E) > josag(O))
                        {
                            van = true;
                            for (int j = 0; j < n; j++)
                                O[j] = E[j];
                        }
                    }
                    else if (!van || josag(E) + fb(szint, E) > josag(O))
                        Backtrack(szint + 1, E, ref van, ref O);
                }
            }
        }
    }

    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema problema;

        public int LepesSzam { get; protected set; }

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public virtual bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];
            for (int szint = 0; szint < problema.n; szint++)
            {
                M[szint] = 2;
                R[szint, 0] = true;
                R[szint, 1] = false;
            }

            VisszalepesesOptimalizacio<bool> optimalizalo = new VisszalepesesOptimalizacio<bool>(
                problema.n, M, R,
                (szint, r) => !r || problema.w[szint] <= problema.Wmax,
                (szint, r, E) =>
                {
                    int suly = r ? problema.w[szint] : 0;
                    for (int i = 0; i < szint; i++)
                        if (E[i])
                            suly += problema.w[i];
                    return suly <= problema.Wmax;
                },
                E => problema.OsszErtek(E));

            bool[] optimalis = optimalizalo.OptimalisMegoldas();
            LepesSzam = optimalizalo.LepesSzam;
            return optimalis;
        }

        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema)
        {
        }

        public override bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];
            for (int szint = 0; szint < problema.n; szint++)
            {
                M[szint] = 2;
                R[szint, 0] = true;
                R[szint, 1] = false;
            }

            SzetvalasztasEsKorlatozasOptimalizacio<bool> optimalizalo = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(
                problema.n, M, R,
                (szint, r) => !r || problema.w[szint] <= problema.Wmax,
                (szint, r, E) =>
                {
                    int suly = r ? problema.w[szint] : 0;
                    for (int i = 0; i < szint; i++)
                        if (E[i])
                            suly += problema.w[i];
                    return suly <= problema.Wmax;
                },
                E => problema.OsszErtek(E),
                (szint, E) =>
                {
                    int kapacitas = problema.Wmax;
                    for (int i = 0; i <= szint; i++)
                        if (E[i])
                            kapacitas -= problema.w[i];
                    float becsles = 0;
                    for (int i = szint; i < problema.n; i++)
                        if (problema.w[i] <= kapacitas)
                            becsles += problema.p[i];
                    return becsles;
                });

            bool[] optimalis = optimalizalo.OptimalisMegoldas();
            LepesSzam = optimalizalo.LepesSzam;
            return optimalis;
        }
    }
}
