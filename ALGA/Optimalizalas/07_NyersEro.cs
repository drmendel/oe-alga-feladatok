using System;

namespace OE.ALGA.Optimalizalas
{
    // 7. heti labor feladat - Tesztek: 07_NyersEroTesztek.cs

    public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }

        public int OsszSuly(bool[] pakolas)
        {
            int osszeg = 0;
            for (int i = 0; i < n; i++)
                if (pakolas[i])
                    osszeg += w[i];
            return osszeg;
        }

        public float OsszErtek(bool[] pakolas)
        {
            float osszeg = 0;
            for (int i = 0; i < n; i++)
                if (pakolas[i])
                    osszeg += p[i];
            return osszeg;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        private int m;
        private Func<int, T> generator;
        private Func<T, double> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, double> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
            LepesSzam = 0;
        }

        public T OptimalisMegoldas()
        {
            T optimalis = generator(1);
            for (int i = 2; i <= m; i++)
            {
                T aktualis = generator(i);
                LepesSzam++;
                if (josag(aktualis) > josag(optimalis))
                    optimalis = aktualis;
            }
            return optimalis;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        private HatizsakProblema problema;

        public int LepesSzam { get; private set; }

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] Generator(int i)
        {
            bool[] pakolas = new bool[problema.n];
            int szam = i - 1;
            for (int j = 0; j < problema.n; j++)
            {
                pakolas[j] = szam % 2 == 1;
                szam /= 2;
            }
            return pakolas;
        }

        public double Josag(bool[] pakolas)
        {
            if (!problema.Ervenyes(pakolas))
                return -1;
            return problema.OsszErtek(pakolas);
        }

        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> megoldo = new NyersEro<bool[]>((int)Math.Pow(2, problema.n), Generator, Josag);
            bool[] optimalis = megoldo.OptimalisMegoldas();
            LepesSzam = megoldo.LepesSzam;
            return optimalis;
        }

        public double OptimalisErtek()
        {
            return Josag(OptimalisMegoldas());
        }
    }
}
