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
        int lastFieldX = -1, lastFieldY = -1;
        int lastPositionX = -1, lastPositionY = -1;

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
            int eineEinheit = pic_Spielfeld.Width / 10;
            int feldX = fieldX(e.X, eineEinheit);
            int feldY = fieldY(e.Y, eineEinheit);
            lastPositionX = feldX;
            lastPositionY = feldY;

            if (feldGesperrt(feldX, feldY) == 0)
            {
                Console.WriteLine("Feld gesperrt!");
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
                return;
            }

            if (feldBesetzt(feldX, feldY) == 0)
            {
                Console.WriteLine("Feld besetzt");
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
                return;
            }

            if (fahrtrichtung(feldX, feldY) == 1)
            {
                Console.WriteLine("Rückwärts!");
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
                return;
            }

            switch (fields[feldX, feldY, 2])
            {
                case 0:
                    fields[feldX, feldY, 2] = lastColor;
                    lastColor = 0;
                    lastFieldY = -1;
                    break;
                case 1:
                    fields[feldX, feldY, 2] = 0;
                    lastColor = 1;
                    lastFieldX = feldX;
                    lastFieldY = feldY;
                    break;
                case 2:
                    fields[feldX, feldY, 2] = 0;
                    lastColor = 2;
                    lastFieldX = feldX;
                    lastFieldY = feldY;
                    break;
                default:
                    return;
            }
            fressen(feldX, feldY);
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

        public int fieldX(int wertX, int eineEinheit)
        {
            int feldX = -1, x = 0, exit = 0;

            while (exit != 1 && x < 8)
            {
                if (wertX >= fields[x, 0, 0] && wertX < fields[x, 0, 0] + eineEinheit)
                {
                    feldX = x;
                    exit = 1;
                }
                x++;
            }
            return feldX;
        }

        public int fieldY(int wertY, int eineEinheit)
        {
            int feldY = -1, y = 0, exit = 0;

            while (exit != 1 && y < 8)
            {
                if (wertY >= fields[0, y, 1] && wertY < fields[0, y, 1] + eineEinheit)
                {
                    feldY = y;
                    exit = 1;
                }
                y++;
            }
            return feldY;
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


        //////////////////////
        // Regelen / Logik  //
        //////////////////////

        public int feldGesperrt(int feldX, int feldY)
        {
            if (fields[feldX, feldY, 3] == 1)   // wenn feld gesperrt, return 0
            {
                return 0;
            }
            else return 1;
        }

        public int feldBesetzt(int feldX, int feldY)
        {
            if (fields[feldX, feldY, 2] != 0 && lastColor != 0) // wenn feld besetzt, return 0
            {
                return 0;
            }
            else return 1;
        }

        public int fahrtrichtung(int feldX, int feldY)
        {
            // Y-Achse, überprüfung, dass nur vw gefahren wird
            if (lastFieldY == -1 && lastFieldY != feldY)   // wenn erster Zug, immer vorwärts
            {
                return 0;
            }
            if (lastColor == 1) // farbe: weiss
            {
                if (feldY <= lastFieldY && lastFieldY != feldY)
                {
                    return 1;
                }
                else return 0;
            }
            else // farbe: rot
            {
                if (feldY >= lastFieldY && lastFieldY != feldY)
                {
                    return 1;
                }
                else return 0;
            }

        }

        public void fressen(int feldX, int feldY)
        {
            if (lastColor == 1) // farbe: weiss
            {
                if (lastPositionY + 2 == feldY)
                {
                    if (fields[feldX - 1, feldY - 1, 2] == 2) fields[feldX - 1, feldY - 1, 2] = 0;
                    if (fields[feldX + 1, feldY - 1, 2] == 2) fields[feldX - 1, feldY + 1, 2] = 0;
                }
            }
            else // farbe: rot
            {
                if (lastPositionY - 2 == feldY)
                {
                    if (fields[feldX - 1, feldY + 1, 2] == 1) fields[feldX - 1, feldY + 1, 2] = 0;
                    if (fields[feldX + 1, feldY + 1, 2] == 1) fields[feldX + 1, feldY + 1, 2] = 0;
                }

            }
        }

    }
}
