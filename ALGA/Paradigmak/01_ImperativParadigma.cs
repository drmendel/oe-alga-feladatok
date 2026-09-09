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

    // where - any interface can be used after this
    public class FeladatTarolo<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        int n;
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

        public void MindentVegrehajt()
        {
            for (int i = 0; i < tarolo.Length; i++)
                tarolo[i].Vegrehajtas();
        }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int size) : base(size) { }

        override public void MindentVegrehajt()
        {
            for (int i = 0; i < tarolo.Length; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
    }
}