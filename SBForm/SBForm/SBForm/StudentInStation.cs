using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace SBForm
{
    class StudentInStation
    {
        private int[] hs;
        private string[] kinhDo, viDo;

        public string[] ViDo
        {
            get { return viDo; }
            set { viDo = value; }
        }

        public string[] KinhDo
        {
            get { return kinhDo; }
            set { kinhDo = value; }
        }

        public int[] Hs
        {
            get { return hs; }
            set { hs = value; }
        }
        public StudentInStation() { }     //cách khai báo khác
        public StudentInStation(int[] hs,string[] kinhDo,string[] viDo)
        {
            this.hs = hs;
            this.kinhDo = kinhDo;
            this.viDo = viDo;
        }

        public void DocfileStation(string dcStation)
        {
            string[] lines = File.ReadAllLines(@dcStation);

            //lưu lại tọa độ của trường
            string[] truong = lines[0].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            kinhDo[0] = truong[1];
            ViDo[0] = truong[2];

            int j = 1;                                  // mảng bắt đầu từ 1 do ở trạm 0(trường học không có sinh viên chờ bus)
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tam = lines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                kinhDo[j] = tam[1];
                viDo[j] = tam[2];
                hs[j] = int.Parse(tam[3].ToString());
                j++;
            }
        }
        public int TongcacTram()        //tổng các trạm có học sinh
        {
            int dem = 0;
            string[] lines = File.ReadAllLines(@"station.txt");
            for (int i = 1; i < lines.Length; i++)
                dem++;
            return dem;
        }
    }
}
