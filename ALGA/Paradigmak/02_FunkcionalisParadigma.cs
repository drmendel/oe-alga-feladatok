using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 2. heti labor feladat - Tesztek: 02_FunkcionálisParadigmaTesztek.cs

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

        public override IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < n; i++)
            {
                if (BejaroFeltetel(tarolo[i]))
                {
                    yield return tarolo[i];
                }
            }
        }
    }
}
