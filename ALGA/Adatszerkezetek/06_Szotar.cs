using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 6. heti labor feladat - Tesztek: 06_SzotarTesztek.cs

    class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        private SzotarElem<K, T>?[] E;
        private Func<K, int> h;
        private Lista<SzotarElem<K, T>> U;

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>?[meret];
            h = kulcs => hasitoFuggveny(kulcs) % E.Length;
            U = new LancoltLista<SzotarElem<K, T>>();
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret)
            : this(meret, kulcs => Math.Abs(kulcs!.GetHashCode()))
        {
        }

        private SzotarElem<K, T>? KulcsKeres(K kulcs)
        {
            SzotarElem<K, T>? elem = E[h(kulcs)];
            if (elem != null && object.Equals(elem.kulcs, kulcs))
                return elem;

            SzotarElem<K, T>? talalt = null;
            U.Bejar(x =>
            {
                if (object.Equals(x.kulcs, kulcs))
                    talalt = x;
            });
            return talalt;
        }

        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem != null)
            {
                elem.tart = ertek;
                return;
            }

            SzotarElem<K, T> uj = new SzotarElem<K, T>(kulcs, ertek);
            int index = h(kulcs);
            if (E[index] == null)
                E[index] = uj;
            else
                U.Hozzafuz(uj);
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem == null)
                throw new HibasKulcsKivetel();
            return elem.tart;
        }

        public void Torol(K kulcs)
        {
            int index = h(kulcs);
            if (E[index] != null && object.Equals(E[index]!.kulcs, kulcs))
            {
                E[index] = null;
                return;
            }

            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem == null)
                throw new HibasKulcsKivetel();
            U.Torol(elem);
        }
    }
}
