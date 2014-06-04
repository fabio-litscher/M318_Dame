﻿using System;
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
        int[, ,] fields = new int[8, 8, 5];     // 3 Dimensionales Array: X, Y, Spielstein (0=kein Stein, 1=weiss, 2=rot), zulässiges Feld (0=ja, 1=nein), Dame (0=nein, 1=ja)
        List<int> possibleHits = new List<int>();
        int hitCounter = 0;

        int winner = 0;
        int schlagzwang = 0;
        int lastColor;
        int firstRound = 1;
        int lastFieldX = -1, lastFieldY = -1;
        int lastPositionX = -1, lastPositionY = -1;
        int spieler = 1;   // 1 = weiss, 2 = rot    Spieler weiss beginnz immer
        int schongeschlagen = 0;
        int schlagFeldX = -1, schlagFeldY = -1;

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
                    fields[i, k, 4] = 0;
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
                        else if (lineCounter >= 5) fields[k, i, 2] = 2;
                        else fields[k, i, 2] = 0;
                    }
                }
                if (n == 1) n = 0;
                else n = 1;
                lineCounter++;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Damit die Steine und welcher Spieler dass dran ist angezeigt werden.
            // Ohne das würden die Spielsteine nach minimieren, maximieren nicht angezeigt werden
            zeichneSteine();
            zeichneSpieler();

            // Ein Zusatz könnte hier eingebaut werden, damit die Grösse des Spielfelds automatisch angepasst wird etc.
                // berechneGroesse();
                // berechneKoordinaten();
        }

        private void btn_newRound_Click(object sender, EventArgs e)
        {
            // neues spiel starten
            newRound();
        }

        private void pic_Spielfeld_MouseClick(object sender, MouseEventArgs e) // Nur für Maus
        {
            int eineEinheit = pic_Spielfeld.Width / 10;
            int feldX = fieldX(e.X, eineEinheit);
            int feldY = fieldY(e.Y, eineEinheit);

            txt_console.Text = "";
            if (rdb_schlagzwangEin.Checked) schlagzwang = 1;
            else if (rdb_schlagzwangAus.Checked) schlagzwang = 0;

            if(fields[feldX, feldY, 2] != spieler && lastColor != spieler)
            {
                txt_console.Text = "Spieler " + spieler + " ist am Zug!";
            }
            else if (schongeschlagen == 1 && (feldX != schlagFeldX || feldY != schlagFeldY))      // es muss bei zweitem schlagen mit jenem Stein fahren, der geschlagen hat
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nEs muss mit dem Stein gefahren werden, der schlagen kann";             
            }
            else if (feldGesperrt(feldX, feldY) == 0)
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nAuf weisse Felder darf nicht gesetzt werden!"; 
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
            }

            else if (gueltigeFahrt(feldX, feldY) == 1)
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nNur diagonal fahren ist erlaubt!";
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
            }

            else if (feldBesetzt(feldX, feldY) == 0)
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nAuf ein bereits besetztes Feld kann nicht gesetzt werden!";
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
            }

            else if (fahrtrichtung(feldX, feldY) == 1)
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nMit einem normalen Stein können Sie nicht rückwärts fahren!";
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
            }
            else if (diagonaleDistanzDame(feldX, feldY) == 1)
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nSie dürfen nur beim Schlagen weiter als ein Feld springen!";
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
            }
            else if (diagonaleDistanzNormal(feldX, feldY) == 1)
            {
                txt_console.Text = "Spieler " + spieler + ":\r\nSie dürfen nur beim Schlagen weiter als ein Feld springen!";
                fields[lastFieldX, lastFieldY, 2] = lastColor;
                lastColor = 0;
                zeichneSteine();
            }

            else if (schlagzwang == 1 && schlagenMoeglich(feldX, feldY) == 0)
            {
                if (checkHitFields(feldX, feldY) == 0)
                {
                    txt_console.Text = "Spieler " + spieler + ":\r\nEs muss mit dem Stein gefahren werden, der schlagen kann!";
                    fields[lastFieldX, lastFieldY, 2] = lastColor;
                    lastColor = 0;
                    zeichneSteine();
                }
            }

            else
            {
                if (weiterSchlagenMoeglich(feldX, feldY) == 1)    // wenn kein weiteres schlagen mehr möglich
                {
                    // spielerwechsel, wenn kein weiteres schlagen mehr möglich ist
                    if (lastColor == 1) spieler = 2;
                    else if (lastColor == 2) spieler = 1;
                    schongeschlagen = 0;
                    zeichneSpieler();
                }

                switch (fields[feldX, feldY, 2])
                {
                    case 0:
                        fields[feldX, feldY, 2] = lastColor;
                        if (firstRound == 1) firstRound = 0;
                        if (fields[lastPositionX, lastPositionY, 4] == 1) fields[feldX, feldY, 4] = 1;
                        fields[lastPositionX, lastPositionY, 4] = 0;
                        lastColor = 0;
                        lastFieldY = -1;
                        break;
                    case 1:
                        fields[feldX, feldY, 2] = 0;
                        lastColor = 1;
                        lastFieldX = feldX;
                        lastFieldY = feldY;
                        lastPositionX = feldX;
                        lastPositionY = feldY;
                        break;
                    case 2:
                        fields[feldX, feldY, 2] = 0;
                        lastColor = 2;
                        lastFieldX = feldX;
                        lastFieldY = feldY;
                        lastPositionX = feldX;
                        lastPositionY = feldY;
                        break;
                    default:
                        return;
                }
            }

            // überprüfung ob zu Dame wird
            if (fields[feldX, feldY, 2] == 1)
            {
                if (feldY == 7) fields[feldX, feldY, 4] = 1;
            }
            else if (fields[feldX, feldY, 2] == 2)
            {
                if (feldY == 0) fields[feldX, feldY, 4] = 1;
            }

            zeichneSteine();

            // falls das Spiel fertig ist, soll eine Messagebox kommen
            winner = spielende();
            if (winner == 1 && lastColor == 0)
            {
                MessageBox.Show("Spieler 1 hat gewonnen!", "Spielende", MessageBoxButtons.OK);               // weiss (Spieler 1) hat gewonnen
                newRound();
            }
            else if (winner == 2 && lastColor == 0)
            {
                MessageBox.Show("Spieler 2 hat gewonnen!", "Spielende", MessageBoxButtons.OK);              // rot (Spieler 2) hat gewonnen
                newRound();
            }

        }

        public int spielende()
        {
            int redCounter = 0;
            int whiteCounter = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (fields[i, k, 2] == 1) whiteCounter++;
                    else if (fields[i, k, 2] == 2) redCounter++;
                }
            }

            if (whiteCounter == 0) return 2;
            else if (redCounter == 0) return 1;
            else return 0;
        }

        public void newRound()
        {
            spieler = 1;
            setFixFields();
            zeichneSteine();
            zeichneSpieler();
            rdb_schlagzwangAus.Checked = true;
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
            fields[x, y, 4] = 0;        // Dame = nein
        }

        public void zeichneSpieler()
        {
            pnl_whichPlayer.Refresh();

            if (spieler == 1)
            {
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
                System.Drawing.Graphics formGraphics = pnl_whichPlayer.CreateGraphics();
                formGraphics.FillEllipse(myBrush, new Rectangle(17, 38, 15, 15));
                myBrush.Dispose();
                formGraphics.Dispose();
            }
            else if (spieler == 2)
            {
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
                System.Drawing.Graphics formGraphics = pnl_whichPlayer.CreateGraphics();
                formGraphics.FillEllipse(myBrush, new Rectangle(17, 67, 15, 15));
                myBrush.Dispose();
                formGraphics.Dispose();
            }
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
                    if (fields[x, y, 4] == 1) zeichneDame(wertX, wertY);
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
            if (lastColor != 0 && fields[lastPositionX, lastPositionY, 4] == 1) return 0;
            
            // Y-Achse, überprüfung, dass nur vw gefahren wird
            if (lastFieldY == -1 && lastFieldY != feldY)   // wenn erster Zug, immer vorwärts
            {
                return 0;
            }
            if (lastColor == 1) // farbe: weiss
            {
                if (feldY < lastFieldY && lastFieldY != feldY)
                {
                    return 1;
                }
                else return 0;
            }
            else if (lastColor == 2) // farbe: rot
            {
                if (feldY > lastFieldY && lastFieldY != feldY)
                {
                    return 1;
                }
                else return 0;
            }
            else return 0;
        }
        
        public int gueltigeFahrt(int feldX, int feldY)
        {
            if (feldX == lastFieldX && lastColor != 0)
            {
                return 1;   // fahrt ungültig
            }

            if (feldY == lastFieldY && lastColor != 0)
            {
                return 1;   // fahrt ungültig
            }
            else return 0;
        }

        public int diagonaleDistanzNormal(int feldX, int feldY)
        {
            if (lastColor != 0 && fields[lastPositionX, lastPositionY, 4] == 1) return 0;

            if (lastColor == 1)                                                         // weiss
            {
                if (feldY > lastPositionY + 2)                                          // wenn zu grosse Distanz
                {
                    return 1;                                                               // ungültig
                }
                else if (feldY == lastPositionY + 2)                                    // wenn zwei Felder diagonal
                {
                    if (feldX < lastPositionX)
                    {
                        if (fields[feldX + 1, feldY - 1, 2] == 2)
                        {
                            if (fields[feldX + 1, feldY - 1, 4] == 1)   // muss noch überall angepasst werden!!                        // wenn damestein übersprungen, muss Dame sein zum schlagen
                            {
                                if (fields[feldX, feldY, 4] == 1)
                                {
                                    schlagen(feldX + 1, feldY - 1, feldX, feldY);
                                    return 0;
                                }
                                else return 1;
                            }
                            else
                            {
                                schlagen(feldX + 1, feldY - 1, feldX, feldY);                                         // wenn ja, schlage diesen, gültig zurückgeben
                                return 0;
                            }
                        }
                        else return 1;
                    }
                    else if (feldX > lastPositionX)
                    {
                        if (fields[feldX - 1, feldY - 1, 2] == 2)
                        {
                            if (fields[feldX - 1, feldY - 1, 4] == 1)
                            {
                                if (fields[feldX, feldY, 4] == 1)
                                {
                                    schlagen(feldX - 1, feldY - 1, feldX, feldY);
                                    return 0;
                                }
                                else return 1;
                            }
                            else
                            {
                                schlagen(feldX - 1, feldY - 1, feldX, feldY);                                         // wenn ja, schlage diesen, gültig zurückgeben
                                return 0;
                            }
                        }
                        else return 1;
                    }
                    else return 1;  // ungültig
                }
                else return 0;
            }
            else if (lastColor == 2) // rot
            {
                if (feldY < lastPositionY - 2)
                {
                    return 1;   // ungültig
                }
                else if (feldY == lastPositionY - 2)
                {
                    if (feldX < lastPositionX)
                    {
                        if (fields[feldX + 1, feldY + 1, 2] == 1)
                        {
                            if (fields[feldX + 1, feldY + 1, 4] == 1)
                            {
                                if (fields[feldX, feldY, 4] == 1)
                                {
                                    schlagen(feldX + 1, feldY + 1, feldX, feldY);
                                    return 0;
                                }
                                else return 1;
                            }
                            else
                            {
                                schlagen(feldX + 1, feldY + 1, feldX, feldY);
                                return 0;
                            }
                        }
                        else return 1;
                    }
                    else if (feldX > lastPositionX)
                    {
                        if (fields[feldX - 1, feldY + 1, 2] == 1)
                        {
                            if (fields[feldX - 1, feldY + 1, 4] == 1)
                            {
                                if (fields[feldX, feldY, 4] == 1)
                                {
                                    schlagen(feldX - 1, feldY + 1, feldX, feldY);
                                    return 0;
                                }
                                else return 1;
                            }
                            else
                            {
                                schlagen(feldX - 1, feldY + 1, feldX, feldY);
                                return 0;
                            }
                        }
                        else return 1;
                    }
                    else return 1;      // ungültig
                }
                else return 0;
            }
            else return 0;
        }

        public void schlagen(int zuschlagenX, int zuschlagenY, int feldX, int feldY)
        {
            fields[zuschlagenX, zuschlagenY, 2] = 0;
            schongeschlagen = 1;
            schlagFeldX = feldX;
            schlagFeldY = feldY;
        }

        public int weiterSchlagenMoeglich(int feldX, int feldY)
        {
            int farbeGegner = 0;
            if (lastColor == 1) farbeGegner = 2;
            else if (lastColor == 2) farbeGegner = 1;

            if (firstRound != 1 && fields[lastPositionX, lastPositionY, 4] == 1)
            {
                if (feldX < 2)              // wenn am linken zu nahe Rand
                {
                    if (feldY > 5)
                    {
                        if (fields[feldX + 1, feldY - 1, 2] == farbeGegner && fields[feldX + 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                        else return 1;
                    }
                    else if (feldY < 2)
                    {
                        if (fields[feldX + 1, feldY + 1, 2] == farbeGegner && fields[feldX + 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                        else return 1;
                    }
                    else if (fields[feldX + 1, feldY + 1, 2] == farbeGegner && fields[feldX + 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else if (fields[feldX + 1, feldY - 1, 2] == farbeGegner && fields[feldX + 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (feldX > 5)              // wenn am linken zu nahe Rand
                {
                    if (feldY > 5)
                    {
                        if (fields[feldX - 1, feldY - 1, 2] == farbeGegner && fields[feldX - 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                        else return 1;
                    }
                    else if (feldY < 2)
                    {
                        if (fields[feldX - 1, feldY + 1, 2] == farbeGegner && fields[feldX - 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                        else return 1;
                    }
                    if (fields[feldX - 1, feldY + 1, 2] == farbeGegner && fields[feldX - 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else if (fields[feldX - 1, feldY - 1, 2] == farbeGegner && fields[feldX - 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (feldY > 5)
                {
                    if (fields[feldX + 1, feldY - 1, 2] == farbeGegner && fields[feldX + 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else if (fields[feldX - 1, feldY - 1, 2] == farbeGegner && fields[feldX - 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (feldY < 2)
                {
                    if (fields[feldX + 1, feldY + 1, 2] == farbeGegner && fields[feldX + 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else if (fields[feldX - 1, feldY + 1, 2] == farbeGegner && fields[feldX - 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (fields[feldX + 1, feldY + 1, 2] == farbeGegner && fields[feldX + 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                else if (fields[feldX + 1, feldY - 1, 2] == farbeGegner && fields[feldX + 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                else if (fields[feldX - 1, feldY + 1, 2] == farbeGegner && fields[feldX - 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                else if (fields[feldX - 1, feldY - 1, 2] == farbeGegner && fields[feldX - 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                else return 1;
            }

            else if (lastColor == 1)  // wenn spieler weiss
            {
                if (feldY > 5) return 1;    // spielfeldrand unten zu nahe
                else if (feldX < 2)              // wenn am linken zu nahe Rand
                {
                    if (fields[feldX + 1, feldY + 1, 2] == 2 && fields[feldX + 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (feldX > 5)              // wenn am linken zu nahe Rand
                {
                    if (fields[feldX - 1, feldY + 1, 2] == 2 && fields[feldX - 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (fields[feldX - 1, feldY + 1, 2] == 2 && fields[feldX - 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                else if (fields[feldX + 1, feldY + 1, 2] == 2 && fields[feldX + 2, feldY + 2, 2] == 0 && schongeschlagen != 0) return 0;
                else return 1;
            }

            else if (lastColor == 2)    // wenn spieler rot
            {
                if (feldY < 2) return 1;    // spielfeldrand unten zu nahe
                else if (feldX < 2)              // wenn am linken zu nahe Rand
                {
                    if (fields[feldX + 1, feldY - 1, 2] == 1 && fields[feldX + 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (feldX > 5)              // wenn am linken zu nahe Rand
                {
                    if (fields[feldX - 1, feldY - 1, 2] == 1 && fields[feldX - 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                    else return 1;
                }
                else if (fields[feldX - 1, feldY - 1, 2] == 1 && fields[feldX - 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                else if (fields[feldX + 1, feldY - 1, 2] == 1 && fields[feldX + 2, feldY - 2, 2] == 0 && schongeschlagen != 0) return 0;
                else return 1;
            }
            
            else return 1;   // weiteres schlagen nicht möglich
        }

        public int schlagenMoeglich(int feldX, int feldY)   // überprüfung ob irgendwo geschlagen werden kann, nur diese Varianten sollen dann möglich sein
        {
            int farbeGegner = 0;
            //if (firstRound == 1) return 1;
            if (lastColor == 0) return 1;
            if (lastColor == 1) farbeGegner = 2;
            else if (lastColor == 2) farbeGegner = 1;
            hitCounter = 0;
            possibleHits.Clear();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if(fields[x, y, 4] == 1 || (lastPositionX == x && lastPositionY == y && fields[feldX, feldY, 4] == 1))        // Wenn Damestein
                    {
                        if (x < 2)                 // wenn am linken zu nahe Rand
                        {
                            if(y < 2)
                            {
                                if (fields[x + 1, y + 1, 2] == farbeGegner && fields[x + 2, y + 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x + 2);
                                    possibleHits.Add(y + 2);
                                }
                            }
                            else if (y > 5)
                            {
                                if (fields[x + 1, y - 1, 2] == farbeGegner && fields[x + 2, y - 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x + 2);
                                    possibleHits.Add(y - 2);
                                }
                            }
                            else
                            {
                                if (fields[x + 1, y + 1, 2] == farbeGegner && fields[x + 2, y + 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x + 2);
                                    possibleHits.Add(y + 2);
                                }
                                if (fields[x + 1, y - 1, 2] == farbeGegner && fields[x + 2, y - 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x + 2);
                                    possibleHits.Add(y - 2);
                                }
                            }
                        }
                        else if (x > 5)                 // wenn am linken zu nahe Rand
                        {
                            if (y < 2)
                            {
                                if (fields[x - 1, y + 1, 2] == farbeGegner && fields[x - 2, y + 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x - 2);
                                    possibleHits.Add(y + 2);
                                }
                            }
                            else if (y > 5)
                            {
                                if (fields[x - 1, y - 1, 2] == farbeGegner && fields[x - 2, y - 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x - 2);
                                    possibleHits.Add(y - 2);
                                }
                            }
                            else
                            {
                                if (fields[x - 1, y + 1, 2] == farbeGegner && fields[x - 2, y + 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x - 2);
                                    possibleHits.Add(y + 2);
                                }
                                if (fields[x - 1, y - 1, 2] == farbeGegner && fields[x - 2, y - 2, 2] == 0)
                                {
                                    hitCounter++;
                                    possibleHits.Add(x - 2);
                                    possibleHits.Add(y - 2);
                                }
                            }
                        }
                        else if (y < 2)
                        {
                            if (fields[x + 1, y + 1, 2] == farbeGegner && fields[x + 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y + 2);
                            }
                            if (fields[x - 1, y + 1, 2] == farbeGegner && fields[x - 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y + 2);
                            }
                        }
                        else if (y > 5)
                        {
                            if (fields[x + 1, y - 1, 2] == farbeGegner && fields[x + 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y - 2);
                            }
                            if (fields[x - 1, y - 1, 2] == farbeGegner && fields[x - 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y - 2);
                            }
                        }
                        else
                        {
                            if (fields[x + 1, y + 1, 2] == farbeGegner && fields[x + 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y + 2);
                            }
                            if (fields[x - 1, y + 1, 2] == farbeGegner && fields[x - 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y + 2);
                            }
                            if (fields[x + 1, y - 1, 2] == farbeGegner && fields[x + 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y - 2);
                            }
                            if (fields[x - 1, y - 1, 2] == farbeGegner && fields[x - 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y - 2);
                            }
                        }
                    }

                    else if (fields[x, y, 2] == 1 || (lastPositionX == x && lastPositionY == y && lastColor == 1))       // Wenn weisser Stein
                    {
                        if (y > 5) continue;            // spielfeldrand unten zu nahe
                        else if (x < 2)                 // wenn am linken zu nahe Rand
                        {
                            if (fields[x + 1, y + 1, 2] == 2 && fields[x + 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y + 2);
                            }
                        }
                        else if (x > 5)                 // wenn am linken zu nahe Rand
                        {
                            if (fields[x - 1, y + 1, 2] == 2 && fields[x - 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y + 2);
                            }
                        }
                        else
                        {
                            if (fields[x + 1, y + 1, 2] == 2 && fields[x + 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y + 2);
                            }
                            if (fields[x - 1, y + 1, 2] == 2 && fields[x - 2, y + 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y + 2);
                            }
                        }
                    }

                    else if (fields[x, y, 2] == 2 || (lastPositionX == x && lastPositionY == y && lastColor == 2))       // Wenn roter Stein
                    {
                        if (y < 5) continue;            // spielfeldrand oben zu nahe
                        else if (x < 2)                 // wenn am linken zu nahe Rand
                        {
                            if (fields[x + 1, y - 1, 2] == 1 && fields[x + 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y - 2);
                            }
                        }
                        else if (x > 5)                 // wenn am linken zu nahe Rand
                        {
                            if (fields[x - 1, y - 1, 2] == 1 && fields[x - 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y - 2);
                            }
                        }
                        else
                        {
                            if (fields[x + 1, y - 1, 2] == 1 && fields[x + 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x + 2);
                                possibleHits.Add(y - 2);
                            }
                            if (fields[x - 1, y - 1, 2] == 1 && fields[x - 2, y - 2, 2] == 0)
                            {
                                hitCounter++;
                                possibleHits.Add(x - 2);
                                possibleHits.Add(y - 2);
                            }
                        }
                    }
                }
            }
            if (hitCounter == 0) return 1;                  // nirgens schlagen möglich, darf mit beliebigem Stein fahren.
            else return 0;                                  // kann irgendwo schlagen
        }

        public int checkHitFields(int feldX, int feldY)
        {
            int k = 0;
            if (hitCounter == 0) return 1;

            for (int i = 0; i < possibleHits.Count; i = i + 2)
            {
                k = i + 1;
                if (feldX == possibleHits[i] && feldY == possibleHits[k]) return 1;
            }
            return 0;
        }




        ///////////////////
        /// Regeln Dame ///
        ///////////////////

        public void zeichneDame(int x, int y)
        {
            int halbeEinheit = pic_Spielfeld.Width / 20;

            System.Drawing.Graphics formGraphics = pic_Spielfeld.CreateGraphics();
            string drawString = "D";
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 25, FontStyle.Bold);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x + 8, y + 6, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }

        public int diagonaleDistanzDame(int feldX, int feldY)
        {
            if (lastColor != 0 && fields[lastPositionX, lastPositionY, 4] == 1)
            {
                int farbeGegner = -1;
                if (lastColor == 1) farbeGegner = 2;
                else if (lastColor == 2) farbeGegner = 1;

                if (feldY > lastPositionY + 2 || feldY < lastPositionY - 2) return 1;        // ungültig

                else if (feldY == lastPositionY + 2)                                    // wenn zwei Felder diagonal
                {
                    if (feldX < 2)                                                          // wenn links am Rand
                    {
                        if (fields[feldX + 1, feldY - 1, 2] == farbeGegner)                               // übersprungenes Feld ein gegnerischer Stein?
                        {
                            schlagen(feldX + 1, feldY - 1, feldX, feldY);                                         // wenn ja, schlage diesen, gültig zurückgeben
                            return 0;
                        }
                        else return 1;                                                                     // sonst ungültig zurückgeben
                    }
                    else if (feldX > 5)                                                     // wenn rechts am Rand
                    {
                        if (fields[feldX - 1, feldY - 1, 2] == farbeGegner)                               // übersprungenes Feld ein gegnerischer Stein?
                        {
                            schlagen(feldX - 1, feldY - 1, feldX, feldY);                                         // wenn ja, schlage diesen, gültig zurückgeben
                            return 0;
                        }
                        else return 1;                                                              // sonst ungültig zurückgeben
                    }
                    else if (feldX < lastPositionX)
                    {
                        if (fields[feldX + 1, feldY - 1, 2] == farbeGegner)
                        {
                            schlagen(feldX + 1, feldY - 1, feldX, feldY);                                         // die selben abfragen nochmals, einfach wenn nicht am Rand
                            return 0;
                        }
                        else return 1;
                    }
                    else if (feldX > lastPositionX)
                    {
                        if (fields[feldX - 1, feldY - 1, 2] == farbeGegner)
                        {
                            schlagen(feldX - 1, feldY - 1, feldX, feldY);
                            return 0;
                        }
                        else return 1;
                    }
                    else return 1;  // ungültig
                }


                else if (feldY == lastPositionY - 2)
                {
                    if (feldX < 2)
                    {
                        if (fields[feldX + 1, feldY + 1, 2] == farbeGegner)
                        {
                            schlagen(feldX + 1, feldY + 1, feldX, feldY);
                            return 0;
                        }
                        else return 1;
                    }
                    else if (feldX > 5)     // rand rechts
                    {
                        if (fields[feldX - 1, feldY + 1, 2] == farbeGegner)
                        {
                            schlagen(feldX - 1, feldY + 1, feldX, feldY);
                            return 0;
                        }
                        else return 1;
                    }
                    else if (feldX < lastPositionX)
                    {
                        if (fields[feldX + 1, feldY + 1, 2] == farbeGegner)
                        {
                            schlagen(feldX + 1, feldY + 1, feldX, feldY);
                            return 0;
                        }
                        else return 1;
                    }
                    else if (feldX > lastPositionX)
                    {
                        if (fields[feldX - 1, feldY + 1, 2] == farbeGegner)
                        {
                            schlagen(feldX - 1, feldY + 1, feldX, feldY);
                            return 0;
                        }
                        else return 1;
                    }
                    else return 1;                                              // ungültig
                }
                else return 0;
            }
            else return 0;
        }

    }
}



        ///////////////////
        ///  Some Code  ///
        ///////////////////


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