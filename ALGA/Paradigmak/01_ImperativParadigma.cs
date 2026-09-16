using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OE.ALGA.Paradigmak
{
    // 1. heti labor feladat - Tesztek: 01_ImperativParadigmaTesztek.cs

    public interface IVegrehajthato
    {
        public void Vegrehajtas();
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    public interface IBejaro<T>
    {
        T Aktualis { get; }
        void Alaphelyzet();
        bool Kovetkezo();
    }

    public interface IBejarhato<T>
    {
        IBejaro<T> BejaroLetrehozas();
    }

    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel(string message = "A tároló megtelt.") : base(message) { }
    }

    public class FeladatTaroloBejaro<T> : IBejaro<T>
    {
        private readonly T[] tarolo;
        private readonly int n;
        private int aktualisIndex;

        public T Aktualis => tarolo[aktualisIndex];

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
            Alaphelyzet();
        }

        public void Alaphelyzet()
        {
            aktualisIndex = -1;
        }

        public virtual bool Kovetkezo()
        {
            aktualisIndex++;
            return aktualisIndex < n;
        }
    }

    public class FeladatTarolo<T> : IBejarhato<T>, IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n;
        public FeladatTarolo(int size)
        {
            tarolo = new T[size];
            n = 0;
        }

        public void Felvesz(T feladat)
        {
            if (n >= tarolo.Length)
                throw new TaroloMegteltKivetel();
            tarolo[n++] = feladat;
        }

        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
                tarolo[i].Vegrehajtas();
        }

        public virtual FeladatTaroloBejaro<T> BejaroLetrehozas() => new FeladatTaroloBejaro<T>(tarolo, n);

        IBejaro<T> IBejarhato<T>.BejaroLetrehozas() => BejaroLetrehozas();

        public IEnumerator<T> GetEnumerator()
        {
            IBejaro<T> bejaro = BejaroLetrehozas();
            while (bejaro.Kovetkezo())
                yield return bejaro.Aktualis;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int size) : base(size) { }

        override public void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
    }
}