using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace SBForm
{
    class ThoiGian
    {
        private int h, p, g;

        public int G
        {
            get { return g; }
            set { g = value; }
        }

        public int P
        {
            get { return p; }
            set { p = value; }
        }

        public int H
        {
            get { return h; }
            set { h = value; }
        }

        public ThoiGian() { }

        //hàm khởi tạo thời gian xuất phát
        public void StartTime(int gio, int phut, int giay)
        {
            this.h = gio;
            this.p = phut;
            this.g = giay;
        }

        //hàm cộng giây vào thời gian
        public void CongThemTime(int giay)
        {
            g = g + giay;
            while (g >= 60)
            {
                g = g - 60;
                p = p + 1;
            }
            while (p >= 60)
            {
                p = p - 60;
                h = h + 1;
            }
        }
        //hàm viết thời gian thành 2 chữ số
        public string PrintTime(string time)
        {
            string[] tam = time.Split(new char[] { ' ', '\t', ':' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tam.Length; i++)
            {
                if (int.Parse(tam[i]) < 10)
                    tam[i] = "0" + tam[i];
            }

            string inra = (tam[0] + ":" + tam[1] + ":" + tam[2]);
            return inra;
        }
    }
}
