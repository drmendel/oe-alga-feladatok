using System;

namespace OE.ALGA.Optimalizalas
{
    // 8. heti labor feladat - Tesztek: 08_DinamikusProgramozasTesztek.cs

    public class DinamikusHatizsakPakolas
    {
        private HatizsakProblema problema;

        public int LepesSzam { get; private set; }

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        private float[,] TablazatFeltoltes()
        {
            float[,] F = new float[problema.n + 1, problema.Wmax + 1];
            LepesSzam = 0;

            for (int t = 1; t <= problema.n; t++)
                for (int h = 1; h <= problema.Wmax; h++)
                {
                    LepesSzam++;
                    if (h < problema.w[t - 1])
                        F[t, h] = F[t - 1, h];
                    else
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - problema.w[t - 1]] + problema.p[t - 1]);
                }

            return F;
        }

        public float OptimalisErtek()
        {
            float[,] F = TablazatFeltoltes();
            return F[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            float[,] F = TablazatFeltoltes();
            bool[] pakolas = new bool[problema.n];
            int h = problema.Wmax;

            for (int t = problema.n; t > 0; t--)
                if (F[t, h] != F[t - 1, h])
                {
                    pakolas[t - 1] = true;
                    h -= problema.w[t - 1];
                }

            return pakolas;
        }
    }
}
