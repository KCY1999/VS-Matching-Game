using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace 重試
{
    public partial class Form1 : Form
    {
        int[] n = new int[17];//圖片的值
        PictureBox[] p = new PictureBox[17];//圖片控制陣列
        PictureBox hp1, hp2; //宣告第一次跟第二次翻的圖
        string s1, s2; //存放翻牌取得的值的字串
        bool fg = true;//按下圖片的旗標
        int t1, t2; //timer1,2執行的次數
        int tot; //答對組數 8組為過
        int lev; //難度等級
        int score;
        SoundPlayer sound = new SoundPlayer(); //音樂
        Random rd = new Random(); //亂數
       
        public Form1()
        {
            InitializeComponent();
        }
        private void Set()  //利用亂數來將n洗牌
        {
            int[] ary = new int[] { 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };
            int max = n.GetUpperBound(0);
            int rdN;
            for (int i = 1; i <= n.GetUpperBound(0); i++)
            {
                rdN = rd.Next(1, max + 1);
                n[i] = ary[rdN];
                ary[rdN] = ary[max];
                max--;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.autoStart = true;//播放背景音樂
            axWindowsMediaPlayer1.URL = "Epic1.mp3";//播放之音樂檔
            MessageBox.Show("玩家可自行決定是否輸入名稱，輸入後則有上榜機會，規則請詳見'關於'。");
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;//自動調整控制項大小
            label1.Text = "";                                                    //label1為遊戲進行時間
            label2.Text = "請點選'遊戲'並選擇難度開始遊戲";//label2為提示剩餘時間
            timer1.Interval = 1000;  //指定timer1每1秒執行一次
            timer2.Interval = 1000; //指定timer2每1秒執行一次
            p[1] = pictureBox1;//表示p[1]~p[16]可以操作picturebox控制項
            p[2] = pictureBox2;
            p[3] = pictureBox3;
            p[4] = pictureBox4;
            p[5] = pictureBox5;
            p[6] = pictureBox6;
            p[7] = pictureBox7;
            p[8] = pictureBox8;
            p[9] = pictureBox9;
            p[10] = pictureBox10;
            p[11] = pictureBox11;
            p[12] = pictureBox12;
            p[13] = pictureBox13;
            p[14] = pictureBox14;
            p[15] = pictureBox15;
            p[16] = pictureBox16;

            for (int i = 1; i <= n.GetUpperBound(0); i++)
            {
                p[i].Image = new Bitmap("KKK.png");//使picturebox顯示KKK
                p[i].SizeMode = PictureBoxSizeMode.StretchImage;//使圖片隨picturebox大小做縮放
                p[i].BorderStyle = BorderStyle.Fixed3D;//picturebox變為3D呈現
                p[i].Enabled = false;//picture失效
                p[i].Click += new EventHandler(PicClick);//委派picturebox執行自訂函數PicClick的動作
            }
        }
        private void PicClick(object sender, EventArgs e)  //定義PicClick事件，提供給p[1]~p[16]的Click事件使用
        {
            if (fg)  //第一次翻牌
            {
                hp1 = (PictureBox)sender;//第一次翻牌的圖片指定給hp1
                s1 = Convert.ToString(hp1.Tag);//將目前翻牌圖片的值指定給s1
                hp1.Image = new Bitmap("KK" + s1 + ".png");//顯示翻牌圖示
                fg = false;//將fg設為false表示目前結束第二次翻牌
            }
            else  //第二次翻牌
            {
                hp2 = (PictureBox)sender;//將第二次翻牌的圖片指定給hp2
                s2 = Convert.ToString(hp2.Tag);//將目前翻牌圖片的值指定給s2
                hp2.Image = new Bitmap("KK" + s2 + ".png"); //顯示翻牌圖示
                 fg = true;//將fg設為false表示目前結束第二次翻牌

                if (s1 == s2)//如果s1 = s2，表示所翻的兩張圖Tag屬性相同，即兩者圖示相同
                {
                    hp1.Enabled = false;//使目前翻的兩個圖片失效
                    hp2.Enabled = false;
                    tot += 1;//答對組數+1
                    score += 15;
                    label19.Text = score.ToString();
                    SoundPlayer sound = new SoundPlayer("Success.wav");//播放答對時音效
                    sound.Play();
                }
                if (s1 != s2)//如果s1 ≠ s2，表示所翻的兩張圖Tag屬性不同，即兩者圖示不同
                {
                    score -= 5;
                    label19.Text = score.ToString();
                    delay(1);//延遲0.5秒
                    hp1.Image = new Bitmap("KKK.png");//將牌翻回背圖
                    hp2.Image = new Bitmap("KKK.png");
                    SoundPlayer sound = new SoundPlayer("Fail.wav");//播放錯誤時音效
                    sound.Play();
                }
                void delay(int t)  //定義delay
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(t * 500);
                }
                if (tot == 8)  //如果答對組數為8則過關
                {
                    高級提示3秒ToolStripMenuItem.Enabled = true;//選單之難度選項啟用
                    中級ToolStripMenuItem.Enabled = true;
                    低級ToolStripMenuItem.Enabled = true;
                    timer1.Enabled = false;//timer1停止
                    timer2.Enabled = false;//timer2停止
                    //各個等級過關的讚美語
                    if (lev == 3)//困難
                    {
                        MessageBox.Show("記憶力很好欸!");
                    }
                    else if (lev == 5)//普通
                    {
                        MessageBox.Show("記憶力不錯喔!");
                    }
                    else if (lev == 10)//簡單
                    {
                        MessageBox.Show("記憶力還行唷!");
                    }
                }
            }
        }
        private void GameStart()  //定義遊戲進行的GameStart()事件處理函式
        {
            lev = t1;//等級=timer1執行次數
            高級提示3秒ToolStripMenuItem.Enabled = false;//難度選擇鈕失效
            中級ToolStripMenuItem.Enabled = false;
            低級ToolStripMenuItem.Enabled = false;
            timer1.Enabled = true;//啟動timer1計時器
            t2 = 0;  //timer2計時遊戲時間
            s1 = ""; //將第一次翻牌所取得的值設為空白
            s2 = "";//將第二次翻牌所取得的值設為空白
            tot = 0;//將答對組數設為0，若達8表示過關
            hp1 = null;//將hp1第一次翻的圖設為null(空值)
            hp2 = null;//將hp2第一次翻的圖設為null(空值)
            label1.Text = "";
            label2.Text = "提示時間剩餘" + Convert.ToString(t1) + "秒";
            Set(); //呼叫亂數程序對n陣列重新洗牌

            for (int i = 1; i <= n.GetUpperBound(0); i++)//使p[1]~p[16]顯示KK1~KK8.png8個圖
            {
                p[i].Tag = n[i];//p[1]~p[16]的Tag屬性皆設為n[1]~n[16]表示圖示狀態
                p[i].Image = new Bitmap("KK" + Convert.ToString(n[i]) + ".png");
            }
        }
        private void 高級提示3秒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t1 = 3;
            GameStart();
        }
        private void 中級ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t1 = 5;
            GameStart();
        }
        private void 低級ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t1 = 10;
            GameStart();
        }
        private void 關於ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("遊戲分為三種難度,高級將給予3秒提示、中級5秒、初級10秒,遊戲時間有30秒,時間到後則遊戲將會結束，請玩家好好把握時間。");
        }
        private void 繼續ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
            pictureBox1.Enabled = true;
            pictureBox2.Enabled = true;
            pictureBox3.Enabled = true;
            pictureBox4.Enabled = true;
            pictureBox5.Enabled = true;
            pictureBox6.Enabled = true;
            pictureBox7.Enabled = true;
            pictureBox8.Enabled = true;
            pictureBox9.Enabled = true;
            pictureBox10.Enabled = true;
            pictureBox11.Enabled = true;
            pictureBox12.Enabled = true;
            pictureBox13.Enabled = true;
            pictureBox14.Enabled = true;
            pictureBox15.Enabled = true;
            pictureBox16.Enabled = true;
        }
        private void 暫停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            pictureBox1.Enabled = false;
            pictureBox2.Enabled = false;
            pictureBox3.Enabled = false;
            pictureBox4.Enabled = false;
            pictureBox5.Enabled = false;
            pictureBox6.Enabled = false;
            pictureBox7.Enabled = false;
            pictureBox8.Enabled = false;
            pictureBox9.Enabled = false;
            pictureBox10.Enabled = false;
            pictureBox11.Enabled = false;
            pictureBox12.Enabled = false;
            pictureBox13.Enabled = false;
            pictureBox14.Enabled = false;
            pictureBox15.Enabled = false;
            pictureBox16.Enabled = false;
        }
        private void 結束ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("遊戲開發者 10733216 如遊玩上遇問題請至 22050 新北市板橋區文化路1段313號致理科技大學圖書館6F詢問。");
        }
        private void 輸入玩家名稱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.Top = 100;
            panel1.Left = 200;
            高級提示3秒ToolStripMenuItem.Enabled = false;
            中級ToolStripMenuItem.Enabled = false ;
            低級ToolStripMenuItem.Enabled = false;
            繼續ToolStripMenuItem.Enabled = false;
            暫停ToolStripMenuItem.Enabled = false;
        }
        private void 開啟音效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.autoStart = true;
            axWindowsMediaPlayer1.URL = "Epic1.mp3";
        }
        private void 關閉音效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.autoStart = false;
            axWindowsMediaPlayer1.URL = "Epic1.mp3";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string pn;    
            pn = Convert.ToString(textBox1.Text);
            label4.Text = pn.ToString();
            label5.Text = pn.ToString();
            panel1.Visible = false;
            高級提示3秒ToolStripMenuItem.Enabled = true;
            中級ToolStripMenuItem.Enabled = true;
            低級ToolStripMenuItem.Enabled = true;
            繼續ToolStripMenuItem.Enabled = true;
            暫停ToolStripMenuItem.Enabled = true;
            輸入玩家名稱ToolStripMenuItem.Enabled = false;
        }
        private void timer1_Tick(object sender, EventArgs e) //timer1計時器啟動觸發timer1_Tick
        {
            t1 -= 1;//t1減1即倒數秒數
            label2.Text = "提示時間剩餘" + Convert.ToString(t1) + "秒";

            if (t1 == 0)//若倒數秒數為0
            {
                timer1.Enabled = false;//timer1失效
                label2.Text = "";
                timer2.Enabled = true;//timer2啟動

                for (int i = 1; i <= n.GetUpperBound(0); i++)
                {
                    p[i].Image = new Bitmap("KKK.png");
                    p[i].Enabled = true;//所有圖片啟用
                }
            }
        }
        private void timer2_Tick(object sender, EventArgs e)//timer2計時器啟動觸發timer2_Tick
        {
            t2 += 1;//timer2加1即遊戲時間加1
            label1.Text = "遊戲時間:" + Convert.ToString(t2) + "秒";

            if (t2 == 30)//若秒數達到30，則執行下列敘述停止遊戲
            {
                timer2.Enabled = false;
                高級提示3秒ToolStripMenuItem.Enabled = true;//按鈕重新啟動
                中級ToolStripMenuItem.Enabled = true;
                低級ToolStripMenuItem.Enabled = true;
                MessageBox.Show("時間到!!!");
                label1.Text = "";
                label2.Text = "請點選'遊戲'並選擇難度開始遊戲";

                for (int i = 1; i <= n.GetUpperBound(0); i++)
                {
                    p[i].Image = new Bitmap("KKK.png");
                    p[i].Enabled = false;//所有圖片失效
                }
            }
        }
    }
}