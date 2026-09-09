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

    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel(string message = "A tároló megtelt.") : base(message) { }
    }

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
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

        public virtual IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < n; i++)
                yield return tarolo[i];
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