using System;
using System.Collections;
using System.Collections.Generic;
using OE.ALGA.Paradigmak;

namespace OE.ALGA.Adatszerkezetek
{
    // 3. heti labor feladat - Tesztek: 03_TombImplementacioTesztek.cs

    public class TombVerem<T> : Verem<T>
    {
        private readonly T[] E;
        private int n;

        public bool Ures => n == 0;

        public TombVerem(int meret)
        {
            E = new T[meret];
            n = 0;
        }

        public void Verembe(T ertek)
        {
            if (n >= E.Length)
                throw new NincsHelyKivetel();
            E[n++] = ertek;
        }

        public T Verembol()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return E[--n];
        }

        public T Felso()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return E[n - 1];
        }
    }

    public class TombSor<T> : Sor<T>
    {
        private readonly T[] E;
        private int e;
        private int u;
        private int n;

        public bool Ures => n == 0;

        public TombSor(int meret)
        {
            E = new T[meret];
            e = 0;
            u = 0;
            n = 0;
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length)
                throw new NincsHelyKivetel();
            E[u] = ertek;
            u = (u + 1) % E.Length;
            n++;
        }

        public T Sorbol()
        {
            if (Ures)
                throw new NincsElemKivetel();
            T ertek = E[e];
            e = (e + 1) % E.Length;
            n--;
            return ertek;
        }

        public T Elso()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return E[e];
        }
    }

    public class TombLista<T> : Lista<T>, IBejarhato<T>, IEnumerable<T>
    {
        private T[] E;
        private int n;

        public int Elemszam => n;

        public TombLista(int meret = 4)
        {
            E = new T[meret];
            n = 0;
        }

        public T Kiolvas(int index)
        {
            if (index < 0 || index >= n)
                throw new HibasIndexKivetel();
            return E[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index < 0 || index >= n)
                throw new HibasIndexKivetel();
            E[index] = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > n)
                throw new HibasIndexKivetel();

            if (n >= E.Length)
            {
                T[] uj = new T[E.Length * 2];
                for (int i = 0; i < n; i++)
                    uj[i] = E[i];
                E = uj;
            }

            for (int i = n; i > index; i--)
                E[i] = E[i - 1];
            E[index] = ertek;
            n++;
        }

        public void Torol(T ertek)
        {
            int db = 0;
            for (int i = 0; i < n; i++)
            {
                if (object.Equals(E[i], ertek))
                    db++;
                else
                    E[i - db] = E[i];
            }
            n -= db;
        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
                muvelet(E[i]);
        }

        public TombListaBejaro<T> BejaroLetrehozas() => new TombListaBejaro<T>(E, n);

        IBejaro<T> IBejarhato<T>.BejaroLetrehozas() => BejaroLetrehozas();

        public IEnumerator<T> GetEnumerator()
        {
            IBejaro<T> bejaro = BejaroLetrehozas();
            while (bejaro.Kovetkezo())
                yield return bejaro.Aktualis;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class TombListaBejaro<T> : IBejaro<T>
    {
        private readonly T[] E;
        private readonly int n;
        private int aktualisIndex;

        public T Aktualis => E[aktualisIndex];

        public TombListaBejaro(T[] E, int n)
        {
            this.E = E;
            this.n = n;
            Alaphelyzet();
        }

        public void Alaphelyzet()
        {
            aktualisIndex = -1;
        }

        public bool Kovetkezo()
        {
            aktualisIndex++;
            return aktualisIndex < n;
        }
    }
}
