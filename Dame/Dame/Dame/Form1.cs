using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dame
{
    public partial class Form1 : Form
    {
        Array Ar = new Array();
        Visualize Vs = new Visualize();
        Helper He = new Helper();

        int[, ,] fields = new int[8, 8, 4];     // 3 Dimensionales Array: X, Y, Spielstein (0=kein Stein, 1=Spieler1, 2=Spieler2), zulässiges Feld (0=ja, 1=nein)
        int lastColor;

        public Form1()
        {
            InitializeComponent();

            setFixFields();            
            berechneKoordinaten();
            zeichneSteine();
        }

        public void setFixFields()
        {
            int lineCounter = 0;
            int n = 1;
            int z;
            for (int i = 0; i < 8; i++)
            {
                if (n == 1) z = 1;
                else z = 0;
                for (int k = 0; k < 8; k++)
                {
                    fields[i, k, 3] = z; // 1 = nicht zulässig
                    if (z == 1) // gesperrt
                    {
                        z = 0;
                        fields[i, k, 2] = 0;
                    }
                    else // zulässig
                    {
                        z = 1;
                        if (lineCounter < 3) fields[k, i, 2] = 1;
                        if (lineCounter >= 5) fields[k, i, 2] = 2;
                    }
                }
                if (n == 1) n = 0;
                else n = 1;
                lineCounter++;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // berechneGroesse();
            // berechneKoordinaten();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            zeichneSteine();
        }

        private void pic_Spielfeld_MouseClick(object sender, MouseEventArgs e) // Nur für Maus
        {
            int wertX = e.X;
            int wertY = e.Y;
            int eineEinheit = pic_Spielfeld.Width / 10;

            welchesFeld(wertX, wertY, eineEinheit);
            zeichneSteine();


            /*      //Ausgabe ganzes array
            for (int i =0; i<8; i++)
            {
                for (int k = 0; k<8; k++)
                {
                    Console.WriteLine(fields[i, k, 0]);
                    Console.WriteLine(fields[i, k, 1]);
                }
            }
            */
        }

        public void welchesFeld(int wertX, int wertY, int eineEinheit)
        {
            int feldX = -1, feldY = -1;
            int x = 0, y = 0, exit1 = 0, exit2 = 0;

            while (exit1 != 1 && x < 8)
            {
                if (wertX >= fields[x, 0, 0] && wertX < fields[x, 0, 0] + eineEinheit)
                {
                    feldX = x;
                    exit1 = 1;
                }
                x++;
            }

            while (exit2 != 1 && y < 8)
            {
                if (wertY >= fields[0, y, 1] && wertY < fields[0, y, 1] + eineEinheit)
                {
                    feldY = y;
                    exit2 = 1;
                }
                y++;
            }

            switch(fields[feldX, feldY, 2])
            {
                case 0:
                    fields[feldX, feldY, 2] = lastColor;
                    lastColor = 0;
                    break;
                case 1:
                    fields[feldX, feldY, 2] = 0;
                    lastColor = 1;
                    break;
                case 2:
                    fields[feldX, feldY, 2] = 0;
                    lastColor = 2;
                    break;
                default: 
                    return;
            }
          
        }

 /*       private void berechneGroesse()
        {
            int hoeheFenster = Form1.ActiveForm.Size.Height;
            int breiteFenster = Form1.ActiveForm.Size.Width;

            if (hoeheFenster < breiteFenster)
            {
                pic_Spielfeld.Height = hoeheFenster - 100;
                pic_Spielfeld.Width = hoeheFenster - 100;
            }
            else
            {
                pic_Spielfeld.Height = breiteFenster - 100;
                pic_Spielfeld.Width = breiteFenster - 100;
            }
            pic_Spielfeld.Location = new Point(breiteFenster / 4, 30);
        } */

        private void berechneKoordinaten()
        {
            int eineEinheit = pic_Spielfeld.Width / 10;

            int wertX = eineEinheit;
            int wertY = eineEinheit;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    fuelleArray(x, y, wertX, wertY);
                    wertX = wertX + eineEinheit;
                }
                wertX = eineEinheit;
                wertY = wertY + eineEinheit;
            }
        }

        private void fuelleArray(int x, int y, int wertX, int wertY)
        {
            fields[x, y, 0] = wertX;    // x Koordinate
            fields[x, y, 1] = wertY;    // y Koordinate
        }

        public void zeichneSteine()
        {
            pic_Spielfeld.Refresh();

            int wertX = 0, wertY = 0, color = 0;
            int y = 0, x = 0;
            for (y = 0; y < 8; y++)
            {
                for (x = 0; x < 8; x++)
                {
                    wertX = fields[x, y, 0];
                    wertY = fields[x, y, 1];
                    color = fields[x, y, 2];
                    zeichneKreis(wertX, wertY, color);
                }
            }
        }

        private void zeichneKreis(int x, int y, int color)
        {
            int d = 40;
            int eineEinheit = pic_Spielfeld.Width / 10;
            if (color == 1) 
            {
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                System.Drawing.Graphics formGraphics = pic_Spielfeld.CreateGraphics();
                formGraphics.FillEllipse(myBrush, new Rectangle(x + 4, y + 4, d, d));
                myBrush.Dispose();
                formGraphics.Dispose();
            }
            else if (color == 2)
            {
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                System.Drawing.Graphics formGraphics = pic_Spielfeld.CreateGraphics();
                formGraphics.FillEllipse(myBrush, new Rectangle(x + 4, y + 4, d, d));
                myBrush.Dispose();
                formGraphics.Dispose();
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            //berechneKoordinaten();
        }

    }
}
