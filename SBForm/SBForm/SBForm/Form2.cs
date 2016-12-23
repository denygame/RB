using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SBForm
{
    public partial class Form2 : Form
    {
        int[] idBus = new int[22];
        int[] chongoiBus = new int[22];
        int[] hs = new int[42];
        string[] kinhDo = new string[42];
        string[] viDo = new string[42];
        int[,] kc = new int[42, 42];
        int[,] tg = new int[42, 42];

        string dcBus, dcStation, dcDistance;
        int dem;

        private SendData obj;
        public Form2(SendData obj)
        {
            InitializeComponent();

            // Lưu lại thông tin của đối tượng truyền qua
            this.obj = obj;

            // Hiển thị dữ liệu
            /*if (objSV != null)
            {
                lblMaSo.Text = objSV.MaSo.ToString();
                lblHoTen.Text = objSV.HoTen;
                lblQueQuan.Text = objSV.QueQuan;
            }*/

            dcBus = obj.DcBus;
            dcStation = obj.DcStation;
            dcDistance = obj.DcDistance;
            dem = obj.DemCombo;

            webBrowser1.DocumentText = File.ReadAllText(@"MapOK.html");
        }


        private void maps_webbrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.InvokeScript("initialize");
        }


        private void DocFile(ref int a, int i)
        {
            TongBus bus = new TongBus(idBus, chongoiBus);
            bus.DocfileBuses(dcBus);
            StudentInStation st = new StudentInStation(hs, kinhDo, viDo);
            st.DocfileStation(dcStation);
            KCTG dis = new KCTG(kc, tg);
            dis.DocfileDistance(dcDistance);
            Solution solution = new Solution(idBus, chongoiBus, hs, kc, tg, kinhDo, viDo);
            a = dem;
        }

        private void btnDistance_Click(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 0);

            comboBox1.Items.Clear();
            for(int i=1;i<=a;i++)
                comboBox1.Items.Add("Bus " + i);

            txtRouteTime.Enabled = false;
            comboBox2.Enabled = false;
            txtRouteDis.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 1);

            comboBox2.Items.Clear();
            for (int i = 1; i <= a; i++)
                comboBox2.Items.Add("Bus " + i);

            txtRouteDis.Enabled = false;
            comboBox1.Enabled = false;
            txtRouteTime.Enabled = true;
            comboBox2.Enabled = true;
        }


        //viết lại hàm show, bỏ đi một số thứ cho Form 2, dùng để load combobox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 0);

            string[] tenfile = new string[a];
            for(int i=0;i<a;i++)
                tenfile[i]="Bus" + (i+1) + "ByDistance" + ".txt";

            string[] aaa = new string[a];
            for (int i = 0; i < a; i++)
                aaa[i] = "Bus" + (i + 1) + "RouteByDistance" + ".txt";

            for (int i = 0; i < a; i++)
            {
                if (comboBox1.SelectedIndex == i)
                {
                    string line = "";

                    //đọc file tọa độ, truyền vào javascript để polyline
                    using (StreamReader sr = new StreamReader(tenfile[i]))
                    {
                        txtShow.Text = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            txtShow.Text = txtShow.Text + line + "\r\n";
                        }
                    }
                    object o = txtShow.Text;
                    webBrowser1.Document.InvokeScript("add", new object[] { o });

                    //đọc file Route
                    using (StreamReader sr = new StreamReader(aaa[i]))
                    {
                        txtRouteDis.Text = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            txtRouteDis.Text = line ;
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 1);

            string[] tenfile = new string[a];
            for(int i=0;i<a;i++)
                tenfile[i]="Bus" + (i+1) + "ByTime" + ".txt";

            string[] aaa = new string[a];
            for (int i = 0; i < a; i++)
                aaa[i] = "Bus" + (i + 1) + "RouteByTime" + ".txt";

            for (int i = 0; i < a; i++)
            {
                if (comboBox2.SelectedIndex == i)
                {
                    string line = "";

                    //đọc file tọa độ, truyền vào javascript để polyline
                    using (StreamReader sr = new StreamReader(tenfile[i]))
                    {
                        txtShow.Text = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            txtShow.Text = txtShow.Text + line + "\r\n";
                        }
                    }
                    object o = txtShow.Text;
                    webBrowser1.Document.InvokeScript("add", new object[] { o });

                    //đọc file Route
                    using (StreamReader sr = new StreamReader(aaa[i]))
                    {
                        txtRouteTime.Text = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            txtRouteTime.Text = line;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtShow.Text = "";
            string l = "Đây là trang web html Google Map API tự viết, có javascript, từ C# chúng ta có thể sử dụng webBrowser Control để load file html và gọi hàm javascript trong code để vẽ những đường Polyline, Polygon hay chấm điểm, làm Marker."
                +"\r\n \tLưu ý: Chỉ là đường chim bay!";
            txtShow.Text = l;
        }
    }
}
