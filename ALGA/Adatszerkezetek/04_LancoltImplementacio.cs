using System;
using System.Collections;
using System.Collections.Generic;
using OE.ALGA.Paradigmak;

namespace OE.ALGA.Adatszerkezetek
{
    // 4. heti labor feladat - Tesztek: 04_LancoltImplementacioTesztek.cs

    class LancElem<T>
    {
        public T tart;
        public LancElem<T>? kov;

        public LancElem(T tart, LancElem<T>? kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }

    public class LancoltVerem<T> : Verem<T>
    {
        private LancElem<T>? fej;
        public bool Ures => fej == null;
        public LancoltVerem()
        {
            fej = null;
        }

        public void Verembe(T ertek)
        {
            fej = new LancElem<T>(ertek, fej);
        }

        public T Verembol()
        {
            if (Ures)
                throw new NincsElemKivetel();
            T ertek = fej!.tart;
            fej = fej.kov;
            return ertek;
        }

        public T Felso()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return fej!.tart;
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        private LancElem<T>? fej;
        private LancElem<T>? vege;
        public bool Ures => fej == null;

        public LancoltSor()
        {
            fej = null;
            vege = null;
        }

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (Ures)
                fej = uj;
            else
                vege!.kov = uj;
            vege = uj;
        }

        public T Sorbol()
        {
            if (Ures)
                throw new NincsElemKivetel();
            T ertek = fej!.tart;
            fej = fej.kov;
            if (fej == null)
                vege = null;
            return ertek;
        }

        public T Elso()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return fej!.tart;
        }
    }

    public class LancoltLista<T> : Lista<T>, IBejarhato<T>
    {
        private LancElem<T>? fej;

        public int Elemszam
        {
            get
            {
                int db = 0;
                for (LancElem<T>? p = fej; p != null; p = p.kov)
                    db++;
                return db;
            }
        }

        public LancoltLista()
        {
            fej = null;
        }

        public T Kiolvas(int index)
        {
            if (index < 0 || index >= Elemszam)
                throw new HibasIndexKivetel();
            LancElem<T> p = fej!;
            for (int i = 0; i < index; i++)
                p = p.kov!;
            return p.tart;
        }

        public void Modosit(int index, T ertek)
        {
            if (index < 0 || index >= Elemszam)
                throw new HibasIndexKivetel();
            LancElem<T> p = fej!;
            for (int i = 0; i < index; i++)
                p = p.kov!;
            p.tart = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(Elemszam, ertek);
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > Elemszam)
                throw new HibasIndexKivetel();
            if (index == 0)
            {
                fej = new LancElem<T>(ertek, fej);
                return;
            }
            LancElem<T> p = fej!;
            for (int i = 1; i < index; i++)
                p = p.kov!;
            p.kov = new LancElem<T>(ertek, p.kov);
        }

        public void Torol(T ertek)
        {
            while (fej != null && object.Equals(fej.tart, ertek))
                fej = fej.kov;

            LancElem<T>? p = fej;
            while (p != null && p.kov != null)
            {
                if (object.Equals(p.kov.tart, ertek))
                    p.kov = p.kov.kov;
                else
                    p = p.kov;
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            for (LancElem<T>? p = fej; p != null; p = p.kov)
                muvelet(p.tart);
        }

        public IBejaro<T> BejaroLetrehozas() => new LancoltListaBejaro<T>(fej);

        public IEnumerator<T> GetEnumerator()
        {
            IBejaro<T> bejaro = BejaroLetrehozas();
            while (bejaro.Kovetkezo())
                yield return bejaro.Aktualis;
        }
    }

    public class LancoltListaBejaro<T> : IBejaro<T>
    {
        private readonly LancElem<T>? fej;
        private LancElem<T>? aktualisElem;

        public T Aktualis => aktualisElem!.tart;

        internal LancoltListaBejaro(LancElem<T>? fej)
        {
            this.fej = fej;
            Alaphelyzet();
        }

        public void Alaphelyzet()
        {
            aktualisElem = null;
        }

        public bool Kovetkezo()
        {
            if (aktualisElem == null)
                aktualisElem = fej;
            else
                aktualisElem = aktualisElem.kov;
            return aktualisElem != null;
        }
    }
}