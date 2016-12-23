using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace SBForm
{
    class TongBus
    {
        private int[] idBus, chongoiBus;
        public int[] ChongoiBus
        {
            get { return chongoiBus; }
            set { chongoiBus = value; }
        }

        public int[] IdBus
        {
            get { return idBus; }
            set { idBus = value; }
        }

        public TongBus(int[] idBus, int[] chongoiBus)
        {
            this.idBus = idBus;
            this.chongoiBus = chongoiBus;
        }



        public void DocfileBuses(string dcBus)
        {
            string[] lines = File.ReadAllLines(@dcBus);
            int j = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tam = lines[i].Split(new char[] { ' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                idBus[j] = int.Parse(tam[0].ToString());
                chongoiBus[j] = int.Parse(tam[1].ToString());
                j++;
            }
        }
    }
}
