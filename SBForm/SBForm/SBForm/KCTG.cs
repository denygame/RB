using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SBForm
{
    class KCTG
    {
        private int[,] kc, tg;

        public int[,] Tg
        {
            get { return tg; }
            set { tg = value; }
        }

        public int[,] Kc
        {
            get { return kc; }
            set { kc = value; }
        }

        public KCTG(int[,] kc, int[,] tg)
        {
            this.kc = kc;
            this.tg = tg;
        }

        //ma trận 2 chiều KC, TG. VD: kc[5,9] (là khoảng cách từ trạm 5 đến trạm 9) == kc[9,5]
        public void DocfileDistance(string dcDistance)
        {
            string[] lines = File.ReadAllLines(@dcDistance);
            int t = 0;
            for (int i = 0; i < 42; i++)
            {
                for (int j = 0; j < 42; j++)
                {
                    string[] tam = lines[t].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    kc[i, j] = int.Parse(tam[0].ToString());
                    tg[i, j] = int.Parse(tam[1].ToString());
                    t++;
                }
            }
        }
    }
}
