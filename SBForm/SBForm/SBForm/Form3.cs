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
using System.Diagnostics;

namespace SBForm
{
    public partial class Form3 : Form
    {
        int[] idBus = new int[22];
        int[] chongoiBus = new int[22];
        int[] hs = new int[42];
        string[] kinhDo = new string[42];
        string[] viDo = new string[42];
        int[,] kc = new int[42, 42];
        int[,] tg = new int[42, 42];
        string toado = "";
        int demCb;
        string dcBus, dcStation, dcDistance;
        private void DocFile(ref int a, int i)
        {
            TongBus bus = new TongBus(idBus, chongoiBus);
            bus.DocfileBuses(dcBus);
            StudentInStation st = new StudentInStation(hs, kinhDo, viDo);
            st.DocfileStation(dcStation);
            KCTG dis = new KCTG(kc, tg);
            dis.DocfileDistance(dcDistance);
            Solution solution = new Solution(idBus, chongoiBus, hs, kc, tg, kinhDo, viDo);
            a = demCb;
        }

        private void btnDistance_Click(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 0);

            comboBox1.Items.Clear();
            for (int i = 1; i <= a; i++)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 0);

            string[] tenfile = new string[a];
            for (int i = 0; i < a; i++)
                tenfile[i] = "Bus" + (i + 1) + "ByDistance" + ".txt";

            string[] aaa = new string[a];
            for (int i = 0; i < a; i++)
                aaa[i] = "Bus" + (i + 1) + "RouteByDistance" + ".txt";

            //reset toado
            toado = "";

            for (int i = 0; i < a; i++)
            {
                if (comboBox1.SelectedIndex == i)
                {
                    string line = "";
                    int dem = 0;

                    //đọc file tọa độ
                    using (StreamReader sr = new StreamReader(tenfile[i]))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == "//") break;
                            else
                            {
                                if (dem % 2 == 0) toado += "'" + line;
                                else toado += "," + line + "'/";
                                dem++;
                            }
                        }
                    }

                    //đọc file Route
                    using (StreamReader sr = new StreamReader(aaa[i]))
                    {
                        txtRouteDis.Text = "";
                        while ((line = sr.ReadLine()) != null)
                            txtRouteDis.Text = line;
                    }
                }
            }
            button1.Enabled = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = 0;
            DocFile(ref a, 1);

            string[] tenfile = new string[a];
            for (int i = 0; i < a; i++)
                tenfile[i] = "Bus" + (i + 1) + "ByTime" + ".txt";

            string[] aaa = new string[a];
            for (int i = 0; i < a; i++)
                aaa[i] = "Bus" + (i + 1) + "RouteByTime" + ".txt";

            //reset toado
            toado = "";

            for (int i = 0; i < a; i++)
            {
                if (comboBox2.SelectedIndex == i)
                {
                    string line = "";
                    int dem = 0;

                    //đọc file tọa độ
                    using (StreamReader sr = new StreamReader(tenfile[i]))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == "//") break;
                            else
                            {
                                if (dem % 2 == 0) toado += "'" + line;
                                else toado += "," + line + "'/";
                                dem++;
                            }
                        }
                    }

                    //đọc file Route
                    using (StreamReader sr = new StreamReader(aaa[i]))
                    {
                        txtRouteTime.Text = "";
                        while ((line = sr.ReadLine()) != null)
                            txtRouteTime.Text = line;
                    }
                }
            }
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/maps/dir/" + toado);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/maps/dir/" + toado);
        }

        private SendData obj;
        public Form3(SendData obj)
        {
            InitializeComponent();
            this.obj = obj;
            
            dcBus = obj.DcBus;
            dcStation = obj.DcStation;
            dcDistance = obj.DcDistance;
            demCb = obj.DemCombo;
        }

    }
}
