using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 11. heti labor feladat - Tesztek: 11_KupacTesztek.cs

    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobb;

        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = E;
            this.n = n;
            nagyobb = nagyobbPrioritas;
            KupacotEpit();
        }

        public static int Bal(int i) => 2 * i;

        public static int Jobb(int i) => 2 * i + 1;

        public static int Szulo(int i) => i / 2;

        protected void Kupacol(int i)
        {
            int max = i;
            if (Bal(i) < n && nagyobb(E[Bal(i)], E[max]))
                max = Bal(i);
            if (Jobb(i) < n && nagyobb(E[Jobb(i)], E[max]))
                max = Jobb(i);

            if (max != i)
            {
                T csere = E[i];
                E[i] = E[max];
                E[max] = csere;
                Kupacol(max);
            }
        }

        protected void KupacotEpit()
        {
            for (int i = n / 2; i >= 0; i--)
                Kupacol(i);
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        public KupacRendezes(T[] A) : base(A, A.Length, (x, y) => x.CompareTo(y) > 0)
        {
        }

        public void Rendezes()
        {
            for (int i = n - 1; i > 0; i--)
            {
                T csere = E[0];
                E[0] = E[i];
                E[i] = csere;
                n--;
                Kupacol(0);
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas)
            : base(new T[meret], 0, nagyobbPrioritas)
        {
        }

        public bool Ures => n == 0;

        private void KulcsotFelvisz(int i)
        {
            while (i > 0 && nagyobb(E[i], E[Szulo(i)]))
            {
                T csere = E[i];
                E[i] = E[Szulo(i)];
                E[Szulo(i)] = csere;
                i = Szulo(i);
            }
        }

        public void Sorba(T ertek)
        {
            if (n == E.Length)
                throw new NincsHelyKivetel();
            E[n] = ertek;
            n++;
            KulcsotFelvisz(n - 1);
        }

        public T Sorbol()
        {
            if (Ures)
                throw new NincsElemKivetel();
            T elso = E[0];
            E[0] = E[n - 1];
            n--;
            Kupacol(0);
            return elso;
        }

        public T Elso()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return E[0];
        }

        public void Frissit(T elem)
        {
            for (int i = 0; i < n; i++)
                if (object.Equals(E[i], elem))
                {
                    KulcsotFelvisz(i);
                    Kupacol(i);
                    return;
                }
            throw new NincsElemKivetel();
        }
    }
}
