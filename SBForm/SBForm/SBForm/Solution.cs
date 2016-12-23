using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace SBForm
{
    class Solution
    {
        int[] idBus, chongoiBus, hs;
        int[,] kc, tg;
        string[] kinhDo, viDo;
        int thoigian = 0, time = 0, ok = 0;
        List<int> cacTram = new List<int>();            //các trạm 1 xe bus đi qua
        List<int> hslen = new List<int>();              //số học sinh lên xe
        List<string> TimeDen = new List<string>();      //thời gian xe bus đến từng trạm, bắt đầu từ Start Time
        ThoiGian tictac = new ThoiGian();                 //tạo biến ThoiGian, dùng để in tg đến từng trạm
        StudentInStation AllStationHS = new StudentInStation();

        public int demCombo = 0;


        public Solution(int[] idBus, int[] chongoiBus, int[] hs, int[,] kc, int[,] tg, string[] kinhDo, string[] viDo)
        {
            this.idBus = idBus;
            this.chongoiBus = chongoiBus;
            this.hs = hs;
            this.kc = kc;
            this.tg = tg;
            this.kinhDo = kinhDo;
            this.viDo = viDo;
        }

        //ghi tọa độ vào file txt, map đọc tọa độ
        private void GhiToaDo(int tram, string tenfile)
        {
            using (StreamWriter sw = new StreamWriter(tenfile, true))    //true là ghi thêm
            {
                sw.WriteLine(kinhDo[tram]);
                sw.WriteLine(viDo[tram]);
            }
        }

        //ghi lộ trình vào file
        private void GhiRoute(int tram,string tenfile)
        {
            using (StreamWriter sw = new StreamWriter(tenfile, true))    //true là ghi thêm
            {
                sw.Write(tram + " => ");
            }
        }

        private int TongHSlen(List<int> hslen)
        {
            int s = 0;
            for(int i=0;i<hslen.Count;i++)
                s += hslen[i];
            return s;
        }



 //các hàm thuật toán

        private int TramXaTruongNhat()
        {
            int max = 0, tramxanhat = 0;
            for (int i = 1; i < 42; i++)
            {
                if (max < kc[0, i] && hs[i] != 0)
                {
                    max = kc[0, i];
                    tramxanhat = i;
                }
            }
            return tramxanhat;
        }

        /*trạm gần nhất theo thời gian*/
        private int TramNhanhNhat(int tram)
        {
            int tramg = 9999999, dem = 0;
            for (int i = 1; i < 42; i++)
            {
                if (tramg >= tg[tram, i] && tg[tram, i] != 0 && hs[i] != 0)
                {
                    tramg = tg[tram, i];
                    dem = i;
                }
            }
            return dem;
        }
        
        /*trạm gần nhất theo khoảng cách*/
        private int TramGanNhat(int tram)
        {
            int tramg = 9999999, dem = 0;
            for (int i = 1; i < 42; i++)
            {
                if (tramg >= kc[tram, i] && kc[tram, i] != 0 && hs[i] != 0)
                {
                    tramg = kc[tram, i];
                    dem = i;
                }
            }
            return dem;
        }
        

//hàm chạy đệ quy mỗi chiếc xe Bus qua các trạm <hàm quan trọng nhất>
        private void run(int tram, int id, int d, int kieu)
        {
            if (kieu == 0)  //run theo khoảng cách
            {
                while (thoigian <= 2400 && chongoiBus[id] != 0)
                {
                    //Reset hs trạm và chỗ ngồi Bus
                    if (hs[tram] <= chongoiBus[id])
                    {
                        hslen.Add(hs[tram]);
                        chongoiBus[id] -= hs[tram];
                        hs[tram] = 0;

                        //biến đi qua trạm lấy hết hs thì + thêm (cờ)
                        ok++;
                    }
                    else
                    {
                        hslen.Add(chongoiBus[id]);
                        hs[tram] -= chongoiBus[id];
                        chongoiBus[id] = 0;
                    }

                    //Add trạm vào List 
                    cacTram.Add(tram);

                    //trường hợp xe đi qua trạm cuối cùng, dừng luôn
                    if (TramGanNhat(tram) == 0 && ok == d)
                    {
                        /*nếu xe cuối cùng chỉ đi qua 1 trạm time bằng tg trạm đó về trường
                         ngược lại đi nhiều trạm thì time = thoigian*/
                        if (thoigian == 0)
                        {
                            time = tg[tram, 0];
                        }
                        else
                        {
                            time = thoigian;
                        }

                        tictac.CongThemTime(tg[TramGanNhat(tram), tram] + tg[TramGanNhat(tram), 0]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);

                        //cộng cờ trạm cuối cùng trước khi dừng vòng lặp, ok lúc này bằng 41 theo file input
                        ok++;

                        //dừng vòng lặp
                        thoigian = 2500;
                        chongoiBus[id] = 0;
                        break;
                    }

                    //xe hết chỗ ngồi thì dừng, về trường
                    if (chongoiBus[id] == 0)
                    {
                        time = thoigian - time;
                        time += tg[tram, 0];
                        tictac.CongThemTime(tg[tram, 0]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);
                        break;
                    }

                    /*xử lý thời gian, lưu ý: time là biến thời gian chính xác dùng để in ra
                     thoigian là biến đệm tính time (test time)*/
                    thoigian = thoigian - time;
                    thoigian += tg[TramGanNhat(tram), tram] + tg[TramGanNhat(tram), 0];
                    time = tg[TramGanNhat(tram), 0];

                    //tg vượt quá quy định 40p, về trường, tg trừ xuống trạm đi quá quy định
                    if (thoigian > 2400)
                    {
                        time = thoigian - tg[TramGanNhat(tram), tram] - tg[TramGanNhat(tram), 0] + tg[tram, 0];
                        tictac.CongThemTime(tg[tram, 0]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);
                    }
                    else
                    {
                        tictac.CongThemTime(tg[TramGanNhat(tram), tram]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);

                        //đệ quy với trạm là trạm gần nhất
                        run(TramGanNhat(tram), id, d, 0);
                    }
                }
            }
            else //kieu==1 run theo tg
            {
                while (thoigian <= 2400 && chongoiBus[id] != 0)
                {
                    if (hs[tram] <= chongoiBus[id])
                    {
                        hslen.Add(hs[tram]);
                        chongoiBus[id] -= hs[tram];
                        hs[tram] = 0;
                        ok++;
                    }
                    else
                    {
                        hslen.Add(chongoiBus[id]);
                        hs[tram] -= chongoiBus[id];
                        chongoiBus[id] = 0;
                    }
                    cacTram.Add(tram);
                    if (TramNhanhNhat(tram) == 0 && ok == d)
                    {
                        if (thoigian == 0) time = tg[tram, 0];
                        else time = thoigian;
                        tictac.CongThemTime(tg[TramNhanhNhat(tram), tram] + tg[TramNhanhNhat(tram), 0]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);
                        ok++;
                        thoigian = 2500;
                        chongoiBus[id] = 0;
                        break;
                    }
                    if (chongoiBus[id] == 0)
                    {
                        time = thoigian - time;
                        time += tg[tram, 0];
                        tictac.CongThemTime(tg[tram, 0]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);
                        break;
                    }
                    thoigian = thoigian - time;
                    thoigian += tg[TramNhanhNhat(tram), tram] + tg[TramNhanhNhat(tram), 0];
                    time = tg[TramNhanhNhat(tram), 0];
                    if (thoigian > 2400)
                    {
                        time = thoigian - tg[TramNhanhNhat(tram), tram] - tg[TramNhanhNhat(tram), 0] + tg[tram, 0];
                        tictac.CongThemTime(tg[tram, 0]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);
                    }
                    else
                    {
                        tictac.CongThemTime(tg[TramNhanhNhat(tram), tram]);
                        TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);
                        run(TramNhanhNhat(tram), id, d, 1);
                    }
                }
            }
        }




        private void HamGhiFile(int id, int kieu)
        {
            if (kieu == 0)
            {
                //ghi Route
                string aaa = "Bus" + idBus[id] + "RouteByDistance" + ".txt";
                File.Delete(aaa);
                for (int i = 0; i < cacTram.Count; i++)
                    GhiRoute(cacTram[i], aaa);
                using (StreamWriter sw = new StreamWriter(aaa, true))
                    sw.Write(" School" + " <Student: " + TongHSlen(hslen) + ">");


                //ghi tọa độ
                string tenfile = "Bus" + idBus[id] + "ByDistance" + ".txt";
                //xóa dữ liệu cũ
                File.Delete(tenfile);
                //ghi vào file
                for (int i = 0; i < cacTram.Count; i++)
                    GhiToaDo(cacTram[i], tenfile);
                using (StreamWriter sw = new StreamWriter(tenfile, true))
                {
                    sw.WriteLine(kinhDo[0].ToString());
                    sw.WriteLine(viDo[0].ToString());
                    sw.Write("//");
                }
            }
            else
            {
                string aaa = "Bus" + idBus[id] + "RouteByTime" + ".txt";
                File.Delete(aaa);
                for (int i = 0; i < cacTram.Count; i++)
                    GhiRoute(cacTram[i], aaa);
                using (StreamWriter sw = new StreamWriter(aaa, true))
                    sw.Write(" School" + " <Student: " + TongHSlen(hslen) + ">");
                

                string tenfile = "Bus" + idBus[id] + "ByTime" + ".txt";
                File.Delete(tenfile);
                for (int i = 0; i < cacTram.Count; i++)
                    GhiToaDo(cacTram[i], tenfile);
                using (StreamWriter sw = new StreamWriter(tenfile, true))
                {
                    sw.WriteLine(kinhDo[0].ToString());
                    sw.WriteLine(viDo[0].ToString());
                    sw.Write("//");
                }
            }
        }


        //hàm in ra các bộ test lộ trình các xe Bus
        public void show(TextBox txtShow, TextBox txtShow1,int a, int b, int c, int test)
        {
            int d = AllStationHS.TongcacTram();         //trả về 41 trạm (trừ trường ra)
            int id = 0;         //id xe bus

            while (ok < d)
            {
                //gán thời gian xuất phát
                tictac.StartTime(a, b, c);

                //trạm xa nhất là trạm xuất phát, Add vào List
                int tram = TramXaTruongNhat();
                TimeDen.Add(tictac.H + ":" + tictac.P + ":" + tictac.G);

                if (test == 0)
                {
                    run(tram, id, d, test);
                    //in lộ trình từng xe
                    txtShow.Text = txtShow.Text + ("Bus " + idBus[id] + " (Total Time: " + time + ") : ");
                    for (int i = 0; i < cacTram.Count; i++)
                        txtShow.Text = txtShow.Text + (cacTram[i].ToString() + "(" + hslen[i].ToString() + ")" + " (Time: " + tictac.PrintTime(TimeDen[i]) + ") => ");
                    txtShow.Text = txtShow.Text + (" School" + " (Time: " + tictac.PrintTime(TimeDen[TimeDen.Count - 1]) + ")" + "\r\n\r\n");
                    
                    HamGhiFile(id, test);

                    //lộ trình xe tiếp theo
                    id++;

                    //xóa các bộ dữ liệu của xe cũ, tiếp tục lặp xe mới với bộ dữ liệu trống
                    time = 0;
                    thoigian = 0;
                    TimeDen.Clear();
                    cacTram.Clear();
                    hslen.Clear();

                    demCombo++;
                }

                else
                {
                    run(tram, id, d, test);
                    //in lộ trình từng xe
                    txtShow1.Text = txtShow1.Text + ("Bus " + idBus[id] + " (Total Time: " + time + ") : ");
                    for (int i = 0; i < cacTram.Count; i++)
                        txtShow1.Text = txtShow1.Text + (cacTram[i].ToString() + "(" + hslen[i].ToString() + ")" + " (Time: " + tictac.PrintTime(TimeDen[i]) + ") => ");
                    txtShow1.Text = txtShow1.Text + (" School" + " (Time: " + tictac.PrintTime(TimeDen[TimeDen.Count - 1]) + ")" + "\r\n\r\n");
                    
                    HamGhiFile(id, test);
                    
                    //lộ trình xe tiếp theo
                    id++;

                    //xóa các bộ dữ liệu của xe cũ, tiếp tục lặp xe mới với bộ dữ liệu trống
                    time = 0;
                    thoigian = 0;
                    TimeDen.Clear();
                    cacTram.Clear();
                    hslen.Clear();

                    demCombo++;
                }
            }
        }
    }
}
