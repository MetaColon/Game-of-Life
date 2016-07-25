using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_game_of_life
{
    public partial class gol : Form
    {
        int sizex = 0, sizey = 0;
        bool[,] Feld;
        int fac = 100;
        public gol()
        {
            InitializeComponent();
        }

        private void gol_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < sizex; i+=fac)
            {
                e.Graphics.DrawLine(Pens.Black, 0, i, sizex, i);
                e.Graphics.DrawLine(Pens.Black, i, 0, i, sizey);
            }
            for (int x = 0; x < Feld.GetLength(1); x++)
            {
                for (int y = 0; y < Feld.GetLength(0); y++)
                {
                    if (Feld[y, x])
                        e.Graphics.FillRectangle(Brushes.Yellow, (float)(fac*x), (float)(fac*y), fac, fac);
                }
            }
        }

        private void gol_MouseDown(object sender, MouseEventArgs e)
        {
            Feld[e.Y / fac, e.X / fac] = !Feld[e.Y / fac, e.X / fac];
            Refresh();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (Start.Text == "Start!") Start.Text = "End!";
            else Start.Text = "Start!";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool[,] newone = new bool[Feld.GetLength(0), Feld.GetLength(1)];
            for (int x = 0; x < Feld.GetLength(1); x++)
            {
                for (int y = 0; y < Feld.GetLength(0); y++)
                {
                    newone[y, x] = getneighbours(x, y, Feld) == 3 || getneighbours(x, y, Feld) == 2 && Feld[y, x];
                }
            }
            Feld = newone;
            Refresh();
        }

        int getneighbours (int x, int y, bool[,] Feld)
        {
            int n = 0;
            if(x > 0)
            {
                if (Feld[y, x - 1]) n++;
                if (y > 0 && Feld[y - 1, x - 1]) n++;
                if (y < Feld.GetLength(0) - 1 && Feld[y + 1, x - 1]) n++;
            }
            if(x < Feld.GetLength(1) -1)
            {
                if (Feld[y, x + 1]) n++;
                if (y > 0 && Feld[y - 1, x + 1]) n++;
                if (y < Feld.GetLength(0) - 1 && Feld[y + 1, x + 1]) n++;
            }
            if (y > 0 && Feld[y - 1, x]) n++;
            if (y < Feld.GetLength(0) - 1 && Feld[y + 1, x]) n++;
            return n;
        }

        private void Speed_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = 550 - Speed.Value * 50;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            fac = 110 - 10 * trackBar1.Value;
            bool[,] temp = clone(Feld);
            gol_Load(new object(), new EventArgs());
            involve(temp);
            Refresh();
        }

        bool[,] clone (bool[,] a)
        {
            bool[,] b = new bool[a.GetLength(0), a.GetLength(1)];
            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    b[x, y] = a[x, y];
                }
            }
            return b;
        }

        void involve(bool[,] a)
        {
            for (int y = 0; y < a.GetLength(0) && y < Feld.GetLength(0); y++)
            {
                for (int x = 0; x < a.GetLength(1) && x < Feld.GetLength(1); x++)
                {
                    Feld[y, x] = a[y, x];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
        }

        private void gol_Load(object sender, EventArgs e)
        {
            sizex = gol.ActiveForm.Size.Width;
            sizey = gol.ActiveForm.Size.Height;
            Feld = new bool[sizey / fac, sizex / fac];
        }
    }
}
