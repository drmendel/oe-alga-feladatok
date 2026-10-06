using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 5. heti labor feladat - Tesztek: 05_BinarisKeresoFaTesztek.cs

    class FaElem<T> where T : IComparable<T>
    {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;

        public FaElem(T tart, FaElem<T>? bal, FaElem<T>? jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        private FaElem<T>? gyoker;

        public FaHalmaz()
        {
            gyoker = null;
        }

        public void Beszur(T ertek)
        {
            if (gyoker == null)
                gyoker = new FaElem<T>(ertek, null, null);
            else
                ReszfabaBeszur(gyoker, ertek);
        }

        private static void ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            int rel = ertek.CompareTo(p.tart);
            if (rel < 0)
            {
                if (p.bal == null)
                    p.bal = new FaElem<T>(ertek, null, null);
                else
                    ReszfabaBeszur(p.bal, ertek);
            }
            else if (rel > 0)
            {
                if (p.jobb == null)
                    p.jobb = new FaElem<T>(ertek, null, null);
                else
                    ReszfabaBeszur(p.jobb, ertek);
            }
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        private static bool ReszfaEleme(FaElem<T>? p, T ertek)
        {
            if (p == null)
                return false;
            int rel = ertek.CompareTo(p.tart);
            if (rel == 0)
                return true;
            return rel < 0 ? ReszfaEleme(p.bal, ertek) : ReszfaEleme(p.jobb, ertek);
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }

        private static FaElem<T>? ReszfabolTorol(FaElem<T>? p, T ertek)
        {
            if (p == null)
                throw new NincsElemKivetel();

            int rel = ertek.CompareTo(p.tart);
            if (rel < 0)
                p.bal = ReszfabolTorol(p.bal, ertek);
            else if (rel > 0)
                p.jobb = ReszfabolTorol(p.jobb, ertek);
            else
            {
                if (p.bal == null)
                    return p.jobb;
                if (p.jobb == null)
                    return p.bal;
                p.bal = KetGyerekesTorles(p, p.bal);
            }
            return p;
        }

        private static FaElem<T>? KetGyerekesTorles(FaElem<T> e, FaElem<T> p)
        {
            if (p.jobb != null)
            {
                p.jobb = KetGyerekesTorles(e, p.jobb);
                return p;
            }
            e.tart = p.tart;
            return p.bal;
        }

        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }

        private static void ReszfaBejarasPreOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p == null)
                return;
            muvelet(p.tart);
            ReszfaBejarasPreOrder(p.bal, muvelet);
            ReszfaBejarasPreOrder(p.jobb, muvelet);
        }

        private static void ReszfaBejarasInOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p == null)
                return;
            ReszfaBejarasInOrder(p.bal, muvelet);
            muvelet(p.tart);
            ReszfaBejarasInOrder(p.jobb, muvelet);
        }

        private static void ReszfaBejarasPostOrder(FaElem<T>? p, Action<T> muvelet)
        {
            if (p == null)
                return;
            ReszfaBejarasPostOrder(p.bal, muvelet);
            ReszfaBejarasPostOrder(p.jobb, muvelet);
            muvelet(p.tart);
        }
    }
}
