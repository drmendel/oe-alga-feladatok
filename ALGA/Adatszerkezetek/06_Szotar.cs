using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 6. heti labor feladat - Tesztek: 06_SzotarTesztek.cs

    

    public class SzotarElem<K, T>
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
        public SzotarElem<K, T>?[] E;
        private Func<K, int> h;
        private LancoltLista<SzotarElem<K, T>> U;

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>?[meret];
            h = kulcs => hasitoFuggveny(kulcs) % E.Length;
            U = new LancoltLista<SzotarElem<K, T>>();
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, kulcs => kulcs!.GetHashCode()){}

        private SzotarElem<K, T>? KulcsKeres(K kulcs)
        {
            SzotarElem<K, T>? elem = E[h(kulcs)];
            if (elem != null)
            {
                if (elem.kulcs!.Equals(kulcs))
                {
                    return elem;
                }
            }
            else
            {
                elem = null;
                U.Bejar(x =>
                {
                    if (object.Equals(x.kulcs, kulcs)) elem = x;
                });
            }
            return elem;
        }

        public void Beir(K kulcs, T tart)
        {
            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem != null)
            {
                elem.tart = tart;
            }
            else
            {
                SzotarElem<K, T> ujElem = new SzotarElem<K, T>(kulcs, tart);
                if (E[h(kulcs)] == null)
                {
                    E[h(kulcs)] = ujElem;
                }
                else
                {
                    U.Hozzafuz(ujElem);
                }
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if (elem != null)
            {
                return elem.tart;
            }
            throw new HibasKulcsKivetel();
        }

        public void Torol(K kulcs)
        {
            int index = h(kulcs);
            if (E[index] != null && Object.Equals(E[index]!.kulcs, kulcs))
            {
                E[index] = null;
                return;
            }

            SzotarElem<K, T>? elem = KulcsKeres(kulcs);
            if(elem == null) throw new HibasKulcsKivetel();
            else
            {
                U.Torol(elem);
            }
        }

    }
}
