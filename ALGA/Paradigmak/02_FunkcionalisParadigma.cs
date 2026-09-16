using System;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 2. heti labor feladat - Tesztek: 02_FunkcionálisParadigmaTesztek.cs

    public class FeltetelesFeladatTaroloBejaro<T> : FeladatTaroloBejaro<T>
    {
        private readonly Func<T, bool> bejaroFeltetel;

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> bejaroFeltetel) : base(tarolo, n)
        {
            this.bejaroFeltetel = bejaroFeltetel;
        }

        public override bool Kovetkezo()
        {
            bool van;
            do
            {
                van = base.Kovetkezo();
            } while (van && !bejaroFeltetel(Aktualis));
            return van;
        }
    }

    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel { get; set; } = _ => true;

        public FeltetelesFeladatTarolo(int size) : base(size) { }

        public void FeltetelesVegrehajtas(Func<T, bool> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }

        public override FeltetelesFeladatTaroloBejaro<T> BejaroLetrehozas() =>
            new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
    }
}
