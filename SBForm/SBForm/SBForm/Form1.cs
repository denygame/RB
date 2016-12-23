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

    public partial class Form1 : Form
    {
        int[] idBus = new int[22];
        int[] chongoiBus = new int[22];
        int[] hs = new int[42];
        string[] kinhDo = new string[42];
        string[] viDo = new string[42];
        int[,] kc = new int[42, 42];
        int[,] tg = new int[42, 42];
        int dem = 0, demresult1 = 0, demresult2 = 0;
        string KQdis, KQtime;
        int demCb = 0;
        
        public Form1()
        {
            InitializeComponent();
        }

        //FormClosing là sự kiện nút X
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XoaFile();
        }

        #region functions

        //các hàm dùng để gọi nhiều lần
        private void DocFile(int i)
        {
            btnOK.Enabled = false;
            btnCancle.Enabled = true;

            if (i == 0) txtShow.Text = ""; else txtShow1.Text = "";
            
            int a = Convert.ToInt32(txtH.SelectedItem.ToString());
            int b = Convert.ToInt32(txtP.SelectedItem.ToString());
            int c = Convert.ToInt32(txtG.SelectedItem.ToString());
            
            TongBus bus = new TongBus(idBus, chongoiBus);
            bus.DocfileBuses(txtBus.Text.ToString());
            StudentInStation st = new StudentInStation(hs, kinhDo, viDo);
            st.DocfileStation(txtStation.Text.ToString());
            KCTG dis = new KCTG(kc, tg);
            dis.DocfileDistance(txtDistance.Text.ToString());
            Solution solution = new Solution(idBus, chongoiBus, hs, kc, tg, kinhDo, viDo);
            solution.show(txtShow, txtShow1, a, b, c, i);
            demCb = solution.demCombo;
            solution.demCombo = 0;
            if (i == 0) KQdis = txtShow.Text; else KQtime = txtShow1.Text;
        }
        
        private void openEnable()
        {
            if (txtBus.Text != "" && txtDistance.Text != "" && txtStation.Text != "")
            {
                txtShow.Text = "";
                txtShow.Text = "FILES READY\r\n\r\nINPUT START TIME";
                txtShow1.Text = "";
                txtShow1.Text = "FILES READY\r\n\r\nINPUT START TIME";

                txtG.Enabled = true;
                txtH.Enabled = true;
                txtP.Enabled = true;
                btnOK.Enabled = true;
            }
            else
            {
                btnShow.Enabled = false;
                txtShow.Text = "";
                btnShow1.Enabled = false;
                txtShow1.Text = "";
                txtH.SelectedIndex = 0;
                txtP.SelectedIndex = 0;
                txtG.SelectedIndex = 0;
                btnOK.Enabled = false;
                txtG.Enabled = false;
                txtH.Enabled = false;
                txtP.Enabled = false;
            }

        }
        
        private void BusStationDistance(Button btnTest, TextBox txtTest, int i)
        {
            string ten = "";
            if (i == 1) ten = "buses.txt";
            if (i == 2) ten = "station.txt";
            if (i == 3) ten = "distance.txt";
            
            //hàm xử lý
            btnCancle.Enabled = false;
            btnMap.Enabled = false;
            btnRealMap.Enabled = false;
            txtShow.Text = "";
            txtShow1.Text = "";
            txtTest.Text = "";
            OpenFileDialog a = new OpenFileDialog();
            a.ShowDialog();
            a.Filter = "txt file|*.txt";
            txtTest.Text = a.FileName;

            string tam = System.IO.Path.GetFileName(a.FileName);

            while (tam != ten && tam != "")
            {
                DialogResult chon = MessageBox.Show("Bạn không chọn đúng file "+ ten +"!\r\n Vui lòng chọn lại!", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                txtShow.Text = "";
                if (chon == DialogResult.OK)
                {
                    tam = "";
                    DialogResult show = a.ShowDialog();
                    a.Filter = "txt file|*.txt";
                    txtTest.Text = a.FileName;
                    tam = System.IO.Path.GetFileName(a.FileName);

                    if (show == DialogResult.Cancel)
                    {
                        txtTest.Text = "";
                        break;
                    }

                    if (tam == "buses.txt")
                    {
                        txtTest.Text = a.FileName;
                        break;
                    }
                }

                if (chon == DialogResult.Cancel)
                {
                    txtTest.Text = "";
                    break;
                }
            }
            openEnable();
        }
        
        private void Save(string KQ)
        {
            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "TXT file|*.txt";
            DialogResult test = a.ShowDialog();

            if (test == DialogResult.OK)
                using (StreamWriter sw = new StreamWriter(a.FileName))
                    sw.WriteLine(KQ);
        }

        private void openMapAPI()
        {
            if (demresult1 != 0 && demresult2 != 0)
            {
                btnMap.Enabled = true;
                btnRealMap.Enabled = true;
            }
        }


        //Xóa vĩnh viễn các file
        private void XoaFile()
        {
            for (int i = 0; i < demCb; i++)
            {
                string aaa = "Bus" + (i + 1) + "RouteByDistance" + ".txt";
                FileInfo fileRoute = new FileInfo(aaa);
                fileRoute.Delete();

                string tenfile = "Bus" + (i + 1) + "ByDistance" + ".txt";
                FileInfo fileTD = new FileInfo(tenfile);
                fileTD.Delete();

                string aaa1 = "Bus" + (i + 1) + "RouteByTime" + ".txt";
                FileInfo fileRoute1 = new FileInfo(aaa1);
                fileRoute1.Delete();

                string tenfile1 = "Bus" + (i + 1) + "ByTime" + ".txt";
                FileInfo fileTD1 = new FileInfo(tenfile1);
                fileTD1.Delete();
            }
        }


        #endregion

        #region menu
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtShow.Text = "";
            txtShow1.Text = "";
            string line = "";
            using (StreamReader sr = new StreamReader("description.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    txtShow.Text = txtShow.Text + line + "\r\n";
                    txtShow1.Text = txtShow1.Text + line + "\r\n";
                }
            }
        }

        private void instructionsForUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tam = "Bước 1: Chọn 3 file buses.txt, station.txt, distance.txt.\r\n\r\n"
                + "Bước 2: Chọn tab By Distance hay By Time => READY.\r\n\r\n" + "Bước 3: Chọn thời gian xe bus bắt đầu đi (Start Time).\r\n\r\n" + "Bước 4: Chọn OK.\r\n\r\n" + "Bước 5: Chọn Result.\r\n\r\n" + "Bonus:\r\n\r\n" + "\t + Có thể chọn Cancel để chọn lại Start Time." + "\r\n\r\n\t + Có thể chọn FAST để chọn nhanh file mặc định." + "\r\n\r\n\t + Khi click RESULT của cả By Time và By Distance, có thể click MAP hay MAP API để hiển thị Form Map." + "\r\n\r\n\t + Khi click RESET sẽ mặc định xóa các file txt đệm dùng để load Map.";

            txtShow.Text = "";
            txtShow.Text = tam;
            txtShow1.Text = "";
            txtShow1.Text = tam;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tam = "\t\t Nguyễn Thanh Huy\r\n\r\n\r\n" + "\tLiên Hệ:\t thanhhuy96.gtvt@gmail.com";
            txtShow.Text = "";
            txtShow.Text = tam;
            txtShow1.Text = "";
            txtShow1.Text = tam;
        }

        private void attentionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phải truyền vào các file đúng với định dạng <xem Description>",
                "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #endregion

        #region btnBus,btnStation,btnDistance
        private void btnBus_Click(object sender, EventArgs e)
        {
            BusStationDistance(btnBus, txtBus, 1);
        }

        private void btnStation_Click(object sender, EventArgs e)
        {
            BusStationDistance(btnStation, txtStation, 2);
        }

        private void btnDistance_Click(object sender, EventArgs e)
        {
            BusStationDistance(btnDistance, txtDistance, 3);
        }

        #endregion

        #region Result
        private void btnShow_Click(object sender, EventArgs e)
        {
            demresult1++;
            DocFile(0);
            btnSave.Enabled = true;
            openMapAPI();
        }

        private void btnShow1_Click(object sender, EventArgs e)
        {
            demresult2++;
            DocFile(1);
            btnSave1.Enabled = true;
            openMapAPI();
        }
        #endregion

        #region StartTime
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dem >= 3 && txtH.SelectedItem.ToString() != "" && txtP.SelectedItem.ToString() != "" && txtG.SelectedItem.ToString() != "")
            {
                btnOK.Enabled = false;
                btnShow.Enabled = true;
                btnShow1.Enabled = true;
                txtG.Enabled = false;
                txtP.Enabled = false;
                txtH.Enabled = false;
                dem = 0;
                txtShow.Text = "INPUT START TIME SUCCESS\r\n\r\nCLICK RESULT";
                txtShow1.Text = "INPUT START TIME SUCCESS\r\n\r\nCLICK RESULT";
            }
            else
            {
                txtShow.Text = "INPUT WRONG";
                txtShow1.Text = "INPUT WRONG";
            }
        }

        //chọn start time rồi thì dem++;
        private void txtH_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCancle.Enabled = false;
            if (txtH.SelectedItem.ToString() != "") dem++;
        }

        private void txtP_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCancle.Enabled = false;
            if (txtP.SelectedItem.ToString() != "") dem++;
        }

        private void txtG_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCancle.Enabled = false;
            if (txtG.SelectedItem.ToString() != "") dem++;
        }
        
        private void btnCancle_Click(object sender, EventArgs e)
        {
            txtG.Enabled = true;
            txtP.Enabled = true;
            txtH.Enabled = true;
            txtP.SelectedIndex = 0;
            txtH.SelectedIndex = 0;
            txtG.SelectedIndex = 0;
            btnOK.Enabled = true;
            btnShow.Enabled = false;
            btnShow1.Enabled = false;
            txtShow.Text = "";
            txtShow.Text = "FILES READY\r\n\r\nINPUT START TIME";
            txtShow1.Text = "";
            txtShow1.Text = "FILES READY\r\n\r\nINPUT START TIME";
            btnMap.Enabled = false;
            btnRealMap.Enabled = false;
            demresult1 = 0;
            demresult2 = 0;
            btnSave.Enabled = false;
            btnSave1.Enabled = false;
        }

        #endregion

        #region Map
        private void btnMap_Click(object sender, EventArgs e)
        {
            //mở form khác
            SendData obj = new SendData();
            obj.DcBus = txtBus.Text;
            obj.DcStation = txtStation.Text;
            obj.DcDistance = txtDistance.Text;
            obj.DemCombo = demCb;

            Form2 f2 = new Form2(obj);  //chú ý
            this.Visible = false;
            f2.ShowDialog();
            this.Visible = true;    //đóng form 2 mở lại form 1

            demresult1 = 0;
            demresult2 = 0;
        }

        private void btnRealMap_Click(object sender, EventArgs e)
        {
            //mở form khác
            SendData obj = new SendData();
            obj.DcBus = txtBus.Text;
            obj.DcStation = txtStation.Text;
            obj.DcDistance = txtDistance.Text;
            obj.DemCombo = demCb;

            Form3 f3 = new Form3(obj);
            this.Visible = false;
            f3.ShowDialog();
            this.Visible = true;
        }

        #endregion

        #region some btn
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtShow.Text = "";
        }

        private void btnClear1_Click(object sender, EventArgs e)
        {
            txtShow1.Text = "";
        }
        
        private void btnFast_Click(object sender, EventArgs e)
        {
            txtBus.Text = "buses.txt";
            txtDistance.Text = "distance.txt";
            txtStation.Text = "station.txt";
            openEnable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnMap.Enabled = false;
            btnRealMap.Enabled = false;
            btnSave.Enabled = false;
            btnSave1.Enabled = false;
            KQdis = "";
            KQtime = "";
            txtBus.Text = "";
            txtStation.Text = "";
            txtDistance.Text = "";
            txtH.SelectedIndex = 0;
            txtP.SelectedIndex = 0;
            txtG.SelectedIndex = 0;
            btnShow.Enabled = false;
            btnShow1.Enabled = false;
            btnCancle.Enabled = false;
            txtShow.Text = "";
            txtShow1.Text = "";
            btnOK.Enabled = false;
            txtG.Enabled = false;
            txtH.Enabled = false;
            txtP.Enabled = false;
            demresult1 = 0;
            demresult2 = 0;
            XoaFile();
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phần mềm xác định lộ trình xe đưa đón học sinh",
                "Bus Route", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save(KQdis);
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            Save(KQtime);
        }

        #endregion


    }
}
