using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleApplication7
{
    // Versie 18:31 16-01 (Inleverversie)
    class Program
    {
        static void Main()
        {
            Application.Run(new ReversiForm());
        }
    }

    class ReversiForm : Form
    {
        speelbord reversi;
        public Label aantal_wit, score_wit, score_zwart, aantal_zwart, label1;
        public TextBox grote, snelheid_bot;
        public Button start_bots;
        ListBox speler_zwart, speler_wit;
        public int keuze_wit, keuze_zwart;

        public ReversiForm()
        {
            this.Text = "Reversi";
            this.Size = new Size(630, 800);
            this.BackColor = Color.Gray;
            this.Controls_methode();
            this.Paint += this.paint_methode;
            keuze_wit = 1;
            keuze_zwart = 1;
        }

        public void Controls_methode()
        {
            reversi = new speelbord(this);
            reversi.Size = new Size(501, 501);
            reversi.Location = new Point(58, 225);
            Controls.Add(reversi);

            Button nieuwspel = new Button();
            nieuwspel.Location = new Point(58, 18);
            nieuwspel.Size = new Size(75, 37);
            nieuwspel.Text = "Nieuw Spel";
            nieuwspel.Click += this.nieuwspel;
            Controls.Add(nieuwspel);

            Button help = new Button();
            help.Location = new Point(150, 18);
            help.Size = new Size(75, 37);
            help.Text = "Help";
            help.Click += this.help_click;
            Controls.Add(help);

            Button plus_grote = new Button();
            plus_grote.Location = new Point(400, 35);
            plus_grote.Size = new Size(25, 25);
            plus_grote.Text = "+";
            plus_grote.Font = new Font("Arial", 16);
            plus_grote.Click += this.plus_grote_click;
            Controls.Add(plus_grote);

            Button min_grote = new Button();
            min_grote.Location = new Point(260, 35);
            min_grote.Size = new Size(25, 25);
            min_grote.Text = "-";
            min_grote.Font = new Font("Arial", 16);
            min_grote.Click += this.min_grote_click;
            Controls.Add(min_grote);

            grote = new TextBox();
            grote.Location = new Point(290, 37);
            grote.Size = new Size(104, 25);
            grote.Text = "8";
            grote.TextChanged += grote_changed;
            Controls.Add(grote);

            label1 = new Label();
            label1.Location = new Point(58, 190);
            label1.Size = new Size(200, 30);
            label1.Font = new Font("Times New Roman", 16);
            label1.Text = reversi.label_aanzet();
            Controls.Add(label1);

            aantal_zwart = new Label();
            aantal_zwart.Location = new Point(110, 85);
            aantal_zwart.Size = new Size(135, 20);
            aantal_zwart.Font = new Font("Times New Roman", 16, FontStyle.Bold);
            aantal_zwart.Text = ControleBieb.aantal_zwart(reversi.grid, reversi.bord_formaat) + " stenen";
            Controls.Add(aantal_zwart);

            aantal_wit = new Label();
            aantal_wit.Location = new Point(110, 140);
            aantal_wit.Size = new Size(135, 20);
            aantal_wit.Font = new Font("Times New Roman", 16, FontStyle.Bold);
            aantal_wit.Text = ControleBieb.aantal_wit(reversi.grid, reversi.bord_formaat) + " stenen";
            Controls.Add(aantal_wit);

            speler_wit = new ListBox();
            speler_wit.Size = new Size(90, 60);
            speler_wit.Location = new Point(364, 145);
            speler_wit.Items.Add("Mens");
            speler_wit.Items.Add("Beginner Bot");
            speler_wit.Items.Add("Intermediate Bot");
            speler_wit.Items.Add("Expert Bot");
            speler_wit.SelectedItem = "Mens";
            speler_wit.SelectedIndexChanged += listbox_changed;
            Controls.Add(speler_wit);

            Label speler_wit_label = new Label();
            speler_wit_label.Size = new Size(70, 20);
            speler_wit_label.Location = new Point(377, 80);
            speler_wit_label.Text = "Speler Wit";
            speler_wit_label.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            Controls.Add(speler_wit_label);

            speler_zwart = new ListBox();
            speler_zwart.Size = new Size(90, 60);
            speler_zwart.Location = new Point(464, 145);
            speler_zwart.Items.Add("Mens");
            speler_zwart.Items.Add("Beginner Bot");
            speler_zwart.Items.Add("Intermediate Bot");
            speler_zwart.SelectedItem = "Mens";
            speler_zwart.Items.Add("Expert Bot");
            speler_zwart.SelectedIndexChanged += listbox_changed;
            Controls.Add(speler_zwart);

            Label speler_zwart_label = new Label();
            speler_zwart_label.Size = new Size(87, 20);
            speler_zwart_label.Location = new Point(470, 80);
            speler_zwart_label.Text = "Speler Zwart";
            speler_zwart_label.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            Controls.Add(speler_zwart_label);

            snelheid_bot = new TextBox();
            snelheid_bot.Size = new Size(100, 40);
            snelheid_bot.Location = new Point(440, 37);
            snelheid_bot.Text = "200";
            Controls.Add(snelheid_bot);

            start_bots = new Button();
            start_bots.Location = new Point(250, 75);
            start_bots.Size = new Size(80, 30);
            start_bots.Text = "Start";
            start_bots.Click += start_bots_click;
            Controls.Add(start_bots);

            Label grote_bord = new Label();
            grote_bord.Location = new Point(310, 18);
            grote_bord.Size = new Size(100, 25);
            grote_bord.Text = "Bordgrote";
            grote_bord.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            Controls.Add(grote_bord);

            Label snelheid_botlabel = new Label();
            snelheid_botlabel.Location = new Point(450, 18);
            snelheid_botlabel.Size = new Size(100, 25);
            snelheid_botlabel.Text = "Snelheid Bot";
            snelheid_botlabel.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            Controls.Add(snelheid_botlabel);

            score_zwart = new Label();
            score_wit = new Label();
            score_zwart.Text = "0";
            score_wit.Text = "0";

            Button reset_score = new Button();
            reset_score.Location = new Point(250, 115);
            reset_score.Size = new Size(80, 30);
            reset_score.Text = "Reset Score";
            reset_score.Click += reset_score_click;
            Controls.Add(reset_score);

            Button handleiding = new Button();
            handleiding.Location = new Point(250, 155);
            handleiding.Size = new Size(80, 30);
            handleiding.Text = "Handleiding";
            handleiding.Click += handleiding_click;
            Controls.Add(handleiding);

            score_tellers();
        }

        public void score_tellers() // De size en positie van de teller verandert naarmate het aantal cijfers veranderd, zodat ze altijd mooi in het midden blijven.
        {

            if (Convert.ToInt32(score_zwart.Text) >= 10 && Convert.ToInt32(score_zwart.Text) < 100)
            {
                score_zwart.Size = new Size(70, 50);
                score_zwart.Location = new Point(480, 100);
            }
            else if (Convert.ToInt32(score_zwart.Text) >= 100 && Convert.ToInt32(score_zwart.Text) < 1000)
            {
                score_zwart.Size = new Size(80, 50);
                score_zwart.Location = new Point(470, 100);
            }
            else if (Convert.ToInt32(score_zwart.Text) > 1000) // Tot een maximum van duizend.
                score_zwart.Text = "0";
            else
            {
                score_zwart.Size = new Size(50, 50);
                score_zwart.Location = new Point(493, 100);
            }
            score_zwart.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            Controls.Add(score_zwart);


            if (Convert.ToInt32(score_wit.Text) >= 10 && Convert.ToInt32(score_wit.Text) < 100)
            {
                score_wit.Size = new Size(70, 50);
                score_wit.Location = new Point(380, 100);
            }
            else if (Convert.ToInt32(score_wit.Text) >= 100 && Convert.ToInt32(score_wit.Text) < 1000)
            {
                score_wit.Size = new Size(80, 50);
                score_wit.Location = new Point(370, 100);
            }
            else if ((Convert.ToInt32(score_wit.Text) > 1000))
                score_wit.Text = "0";
            else
            {
                score_wit.Size = new Size(50, 50);
                score_wit.Location = new Point(393, 100);
            }
            score_wit.Font = new Font("Times New Roman", 30, FontStyle.Bold);
            Controls.Add(score_wit);
        }


        void listbox_changed(object o, EventArgs ea)
        {
            string wit_gekozen = speler_wit.SelectedItem.ToString();
            string zwart_gekozen = speler_zwart.SelectedItem.ToString();

            if (wit_gekozen == "Mens")
            {
                keuze_wit = 1;
                reversi.bots_spel = false; // Als mens wordt aangeklikt tijdens bot vs bot, dan zal de animatie stoppen. Je kan wel tussen bots wisselen tijdens dat het spel gespeeld wordt tussen bots.
                start_bots.Text = "Start";
            }
            if (wit_gekozen == "Beginner Bot")
                keuze_wit = 2;
            if (wit_gekozen == "Intermediate Bot")
                keuze_wit = 3;
            if (wit_gekozen == "Expert Bot")
                keuze_wit = 4;



            if (zwart_gekozen == "Mens")
            {
                keuze_zwart = 1;
                reversi.bots_spel = false;
                start_bots.Text = "Start";
            }
            if (zwart_gekozen == "Beginner Bot")
                keuze_zwart = 2;
            if (zwart_gekozen == "Intermediate Bot")
                keuze_zwart = 3;
            if (zwart_gekozen == "Expert Bot")
                keuze_zwart = 4;

            if (keuze_wit == 1 && keuze_zwart == 2 && reversi.counter % 2 != 0) // Als men mens tegen mens speelt en besluit te wisselen tijdens het spel naar een bot, dan, mits de bot aan zet is, doet hij de zet zodra er naar de bot gewisseld wordt.
            {
                Thread.Sleep(300);
                reversi.beginnerbot();
            }
            if (keuze_wit == 1 && keuze_zwart == 3 && reversi.counter % 2 != 0)
            {
                Thread.Sleep(300);
                reversi.intermediatebot();
            }
            if (keuze_wit == 1 && keuze_zwart == 4 && reversi.counter % 2 != 0)
            {
                Thread.Sleep(300);
                reversi.expertbot();
            }
            if (keuze_zwart == 1 && keuze_wit == 2 && reversi.counter % 2 == 0) // Als zwart mens speelt, dan moet de bot beginnen dus krijgt hij hier de eerste zet. Maar alleen als wit aan de beurt is, anders blijft hij zetten als je van bots veranderd.
            {
                Thread.Sleep(300);
                reversi.beginnerbot();
            }
            if (keuze_zwart == 1 && keuze_wit == 3 && reversi.counter % 2 == 0)
            {
                Thread.Sleep(300);
                reversi.intermediatebot();
            }
            if (keuze_zwart == 1 && keuze_wit == 4 && reversi.counter % 2 == 0)
            {
                Thread.Sleep(300);
                reversi.expertbot();
            }

            reversi.speelbord1.Invalidate();
        }
        private void start_bots_click(object o, EventArgs ea)
        {
            reversi.start_knop_bots();
        }

        private void grote_changed(object o, EventArgs ea)
        {
            try
            {
                if (Convert.ToInt32(grote.Text) % 2 == 0 && Convert.ToInt32(grote.Text) >= 4 && Convert.ToInt32(grote.Text) <= 20)
                {
                    reversi.bord_formaat = Convert.ToInt32(grote.Text);
                    reversi.afmeting = 500 / reversi.bord_formaat;
                    reversi.speelbord1.Size = new Size(reversi.afmeting * reversi.bord_formaat + 1, reversi.afmeting * reversi.bord_formaat);
                    reversi.nieuwspel();
                    reversi.speelbord1.Invalidate();
                }
            }
            catch
            {
                return;
            }
        }

        private void handleiding_click(object o, EventArgs ea)
        {
            MessageBox.Show("Het doel van het spel is de meeste stenen te hebben aan het eind. Je krijgt stenen door die van de tegenstander in te sluiten, dit kan diagonaal, verticaal en horizontaal. Ook kun je tegen bots spelen, dit doe je door je gewenste kleur te kiezen in het schermpje rechtsboven en als tegenstander een bot aan te klikken. Als je wilt dat bots tegen elkaar spelen, selecteer je dat en druk je op de knop 'start'.", "Handleiding", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void reset_score_click(object o, EventArgs ea)
        {
            score_wit.Text = "0";
            score_zwart.Text = "0";
        }

        public void min_grote_click(object o, EventArgs ea)
        {
            if (reversi.bord_formaat > 4)
            {
                reversi.bord_formaat -= 2;
                reversi.afmeting = 500 / reversi.bord_formaat;
                reversi.speelbord1.Size = new Size(reversi.afmeting * reversi.bord_formaat + 1, reversi.afmeting * reversi.bord_formaat);
                reversi.nieuwspel();
                reversi.speelbord1.Invalidate();
            }
            else
                MessageBox.Show("Dit is de minimale grootte", "Minimale grote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void plus_grote_click(object o, EventArgs ea)
        {
            if (reversi.bord_formaat < 20)
            {
                reversi.bord_formaat += 2;
                reversi.afmeting = 500 / reversi.bord_formaat;
                reversi.speelbord1.Size = new Size(reversi.afmeting * reversi.bord_formaat + 1, reversi.afmeting * reversi.bord_formaat);
                reversi.nieuwspel();
                reversi.speelbord1.Invalidate();
            }
            else
                MessageBox.Show("Dit is de maximale grootte", "Minimale grote", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void nieuwspel(object o, EventArgs ea)
        {
            reversi.nieuwspel();
        }

        public void help_click(object o, EventArgs ea)
        {
            reversi.help_click();
        }

        public void paint_methode(object o, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            gr.FillEllipse(Brushes.Black, 58, 70, 50, 50);
            gr.FillEllipse(Brushes.White, 58, 126, 50, 50);
            gr.DrawLine(Pens.Black, 359, 75, 359, 210);
            gr.DrawLine(Pens.Black, 559, 75, 559, 210);
            gr.DrawLine(Pens.Black, 359, 75, 559, 75);
            gr.DrawLine(Pens.Black, 359, 210, 559, 210);
            gr.DrawLine(Pens.Black, 459, 210, 459, 75);
        }
    }

    class speelbord : UserControl
    {
        public int[,] grid;
        public int bord_formaat, afmeting, counter = 0;
        public Panel speelbord1;
        bool helpteken_clicked = false;
        ReversiForm form;
        public bool bots_spel = false, spel_afgelopen = false;
        int sterkte_zet = 0;


        public speelbord(ReversiForm form)
        {
            this.form = form;

            bord_formaat = 8;
            afmeting = 500 / bord_formaat;

            speelbord1 = new Panel();
            speelbord1.Location = new Point(0, 0);
            speelbord1.Size = new Size(afmeting * bord_formaat + 1, afmeting * bord_formaat + 1); // +1 zodat de recher en onderste lijn ook te zien zijn.
            speelbord1.BackColor = Color.DarkGreen;
            speelbord1.MouseClick += this.speelbord_click;
            Controls.Add(speelbord1);
            arraybeginwaarde();
            speelbord1.Paint += this.speelbord_paint;
        }

        private void set_labels()
        {
            try
            {
                this.form.aantal_zwart.Text = ControleBieb.aantal_zwart(grid, bord_formaat) + " stenen";
                this.form.aantal_wit.Text = ControleBieb.aantal_wit(grid, bord_formaat) + " stenen";
                this.form.label1.Text = label_aanzet();
                this.form.grote.Text = bord_formaat.ToString();
            }
            catch
            {
                return;
            }
        }

        public void arraybeginwaarde()
        {
            counter = 0;
            grid = new int[bord_formaat, bord_formaat];
            for (int i = 0; i < bord_formaat; i++)
            {
                for (int j = 0; j < bord_formaat; j++)
                {
                    grid[i, j] = 0;
                }
            }
            grid[bord_formaat / 2 - 1, bord_formaat / 2 - 1] = -1;
            grid[bord_formaat / 2, bord_formaat / 2 - 1] = 1;
            grid[bord_formaat / 2 - 1, bord_formaat / 2] = 1;
            grid[bord_formaat / 2, bord_formaat / 2] = -1;
        }

        public void nieuwspel()
        {
            arraybeginwaarde();
            bots_spel = false;
            form.start_bots.Text = "Start";
            spel_afgelopen = false;
            if (form.keuze_zwart == 1 && form.keuze_wit == 2 && counter % 2 == 0) // Als zwart mens speelt, dan moet de bot beginnen dus krijgt hij hier de eerste zet. Maar alleen als wit aan de beurt is, anders blijft hij zetten als je van bots veranderd.
            {
                Thread.Sleep(300);
                beginnerbot();
            }
            if (form.keuze_zwart == 1 && form.keuze_wit == 3 && counter % 2 == 0)
            {
                Thread.Sleep(300);
                intermediatebot();
            }
            if (form.keuze_zwart == 1 && form.keuze_wit == 4 && counter % 2 == 0)
            {
                Thread.Sleep(300);
                expertbot();
            }
            speelbord1.Invalidate();
        }

        public void start_knop_bots()
        {
            Thread spel_bots;
            if (bots_spel) // Als het bord vol is dan blijft de knop start, want dat is logischer dan stop. Ook als het spel is gestopt, wordt de knop tekst weer start.
            {

                bots_spel = false;
                form.start_bots.Text = "Start";
                if (ControleBieb.bord_vol(grid, bord_formaat) || ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter)) // Als het spel afgelopen is, dan start het spel opnieuw met de knop "start" zonder dat "nieuw spel" in gedrukt hoeft te worden.
                {
                    nieuwspel();
                    start_knop_bots();
                }
            }
            else
            {
                spel_bots = new Thread(this.bot_vs_bot);
                spel_bots.Start();
                if (form.keuze_wit != 1 && form.keuze_zwart != 1)
                {
                    form.start_bots.Text = "Stop";
                    bots_spel = true;
                }
                if (ControleBieb.bord_vol(grid, bord_formaat) || ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter))
                {
                    nieuwspel();
                    start_knop_bots();
                }
            }
        }

        void speelbord_paint(object o, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            Color c;

            for (int i = 0; i < bord_formaat; i++)
            {
                for (int j = 0; j < bord_formaat; j++)
                {
                    if (grid[i, j] == -1)
                        c = Color.White;
                    else if (grid[i, j] == 1)
                        c = Color.Black;
                    else
                        c = Color.DarkGreen;
                    gr.DrawRectangle(Pens.Black, afmeting * i, afmeting * j, afmeting, afmeting);
                    gr.FillEllipse(new SolidBrush(c), afmeting * i + 5, afmeting * j + 5, (afmeting - 10), (afmeting - 10));
                }
            }
            // Als help aanstaat wordt deze ook getekent
            if (helpteken_clicked)
                teken_help(gr);
            if (!spel_afgelopen)
            {
                set_labels();
                winnaar_bepaler();
            }
        }

        private void teken_help(Graphics gr)
        {
            Pen pen = new Pen(Color.Gray, 3);
            int kleur1 = kleurbepaler1();
            int kleur2 = kleurbepaler2();

            for (int i = 0; i < bord_formaat; i++)
            {
                for (int j = 0; j < bord_formaat; j++)
                {
                    if (ControleBieb.legitiemcheck(i, j, kleur1, kleur2, grid, bord_formaat) && grid[i, j] == 0)
                        gr.DrawEllipse(pen, afmeting * i + 5, afmeting * j + 5, (afmeting - 10), (afmeting - 10));
                }
            }
        }

        private void winnaar_bepaler()
        {
            try
            {
                if ((ControleBieb.bord_vol(grid, bord_formaat) || ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter))) // Als het bord vol is of er is geen zet meer mogelijk worden de stenen getelt en een winnaar gekozen, ook veranderd de knop weer naar "Start".
                {
                    if (ControleBieb.aantal_wit(grid, bord_formaat) > ControleBieb.aantal_zwart(grid, bord_formaat))
                    {
                        form.label1.Text = "Wit is de winnaar";
                        form.score_wit.Text = (Convert.ToInt32(form.score_wit.Text) + 1).ToString();
                        form.score_tellers();
                        spel_afgelopen = true; // Zodat als de winnaar is bepaald het spel afgelopen is.
                    }
                    if (ControleBieb.aantal_zwart(grid, bord_formaat) > ControleBieb.aantal_wit(grid, bord_formaat))
                    {
                        form.label1.Text = "Zwart is de winnaar";
                        form.score_zwart.Text = (Convert.ToInt32(form.score_zwart.Text) + 1).ToString();
                        form.score_tellers();
                        spel_afgelopen = true;
                    }
                    if (ControleBieb.aantal_zwart(grid, bord_formaat) == ControleBieb.aantal_wit(grid, bord_formaat))
                        form.label1.Text = "Het is gelijkspel!";
                    form.start_bots.Text = "Start";
                }
            }
            catch
            {
                nieuwspel();
            }
        }

        void speelbord_click(object o, MouseEventArgs mea)
        {
            int x_array = x_waarde_muis(mea.X);
            int y_array = y_waarde_muis(mea.Y);
            int kleur1 = kleurbepaler1();
            int kleur2 = kleurbepaler2();
            bool legitiem = ControleBieb.legitiemcheck(x_array, y_array, kleur1, kleur2, grid, bord_formaat);

            if (form.keuze_wit == 1 || form.keuze_zwart == 1)
            {
                if (legitiem && grid[x_array, y_array] == 0) // Test of de zet legitiem is.
                {
                    if (counter % 2 == 0)
                        grid[x_array, y_array] = -1;
                    else
                        grid[x_array, y_array] = 1;
                    inkleur_methode(x_array, y_array, kleur1, kleur2, true);
                    counter++; // Telt elke keer als er een legitieme zet wordt gedaan
                    speelbord1.Refresh();
                    zet_overslaan();

                    if ((form.keuze_zwart == 2 && counter % 2 != 0) || (form.keuze_wit == 2 && counter % 2 == 0)) // Als de bot zwart is dan gaat hij alleen als zwart aan de beurt is, 
                                                                                                                  // anders zet hij ook als "zet_ overslaan()" wordt geactiveerd, want die doet de counter omhoog. En vice versa voor wit.
                    {
                        Thread.Sleep(Convert.ToInt32(form.snelheid_bot.Text));
                        beginnerbot();
                    }
                    if ((form.keuze_zwart == 3 && counter % 2 != 0) || (form.keuze_wit == 3 && counter % 2 == 0))
                    {
                        Thread.Sleep(Convert.ToInt32(form.snelheid_bot.Text));
                        intermediatebot();
                    }
                    if ((form.keuze_zwart == 4 && counter % 2 != 0) || (form.keuze_wit == 4 && counter % 2 == 0))
                    {
                        Thread.Sleep(Convert.ToInt32(form.snelheid_bot.Text));
                        expertbot();
                    }
                }
            }
        }

        public void bot_vs_bot()
        {
            int snelheid = 500;
            try
            {
                snelheid = Convert.ToInt32(form.snelheid_bot.Text);
            }
            catch
            {
                MessageBox.Show("Alleen hele getallen", "Bot Snelheid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bots_spel = false;
            }
            if (form.keuze_wit != 1 && form.keuze_zwart != 1)
            {
                while (!ControleBieb.bord_vol(grid, bord_formaat) && ControleBieb.zet_mogelijk(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat) && bots_spel) // Animatie gaat door zolang het bord niet vol is, er een zet mogelijk is en de bots spelen.
                {
                    if (form.keuze_wit == 2)
                    {
                        Thread.Sleep(snelheid);
                        beginnerbot();
                    }
                    if (form.keuze_wit == 3)
                    {
                        Thread.Sleep(snelheid);
                        intermediatebot();
                    }
                    if (form.keuze_wit == 4)
                    {
                        Thread.Sleep(snelheid);
                        expertbot();
                    }
                    if (form.keuze_zwart == 2)
                    {
                        Thread.Sleep(snelheid);
                        beginnerbot();
                    }
                    if (form.keuze_zwart == 3)
                    {
                        Thread.Sleep(snelheid);
                        intermediatebot();
                    }
                    if (form.keuze_zwart == 4)
                    {
                        Thread.Sleep(snelheid);
                        expertbot();
                    }
                }
            }
        }

        public void expertbot()
        {
            bool zet_gedaan = false;
            int hoogste_sterkte = 0;
            int kleur1 = kleurbepaler1();
            int kleur2 = kleurbepaler2();
            List<int> lijst2 = new List<int>();
            List<int> lijst1 = new List<int>();

            for (int i = 0; i < bord_formaat; i++)
            {
                for (int j = 0; j < bord_formaat; j++)
                {
                    if (ControleBieb.legitiemcheck(i, j, kleur1, kleur2, grid, bord_formaat) && grid[i, j] == 0) // Kijkt welke zetten legitiem zijn en slaat de x en y waarde van dat hokje in 2 lijsten op dezelfde plaats in de lijsten.
                    {
                        lijst1.Add(i);
                        lijst2.Add(j);
                        zet_gedaan = true;
                    }
                }
            }

            List<int> lijst_sterktes = new List<int>();

            for (int k = 0; k < lijst1.Count; k++) // Voor elke legitieme zet in de lijst wordt de sterkte bepaald (de hoeveelheid blokjes die worden geslagen) en aan een lijst toegevoegt.
            {
                inkleur_methode(lijst1[k], lijst2[k], kleur1, kleur2, false); // False wordt meegegeven zodat de waardes niet veranderen van de grids.
                if (ControleBieb.vakje_ligging(lijst1[k], lijst2[k], bord_formaat) == 1) // De hoeken
                    sterkte_zet += 100;
                else if (ControleBieb.vakje_ligging(lijst1[k], lijst2[k], bord_formaat) == 2) // Om de hoeken.
                    sterkte_zet += 44;
                else if (ControleBieb.vakje_ligging(lijst1[k], lijst2[k], bord_formaat) == 3) // Naast de zijkanten.
                    sterkte_zet += 48;
                else if (ControleBieb.vakje_ligging(lijst1[k], lijst2[k], bord_formaat) == 4) // De zijkanten.
                    sterkte_zet += 53;
                else // De rest.
                    sterkte_zet += 50;
                lijst_sterktes.Add(sterkte_zet);
            }

            for (int l = 0; l < lijst_sterktes.Count; l++) // Uit de lijst wordt bepaald welke sterkte het hoogst is.
            {
                if (lijst_sterktes[l] > hoogste_sterkte)
                {
                    hoogste_sterkte = lijst_sterktes[l]; // Die sterkte wordt nu de hoogste sterke.
                }
            }

            List<int> sterkste_plek = new List<int>();
            for (int m = 0; m < lijst_sterktes.Count; m++)
            {
                if (lijst_sterktes[m] == hoogste_sterkte) // Een nieuw lijst slaat alle plekken op waar in de lijst de sterkste waardes qua zet aan verbonden is.
                {
                    sterkste_plek.Add(m);
                }
            }

            if (zet_gedaan)
            {
                Random r = new Random();
                int rnd = r.Next(0, sterkste_plek.Count); // Vervolgens pakt hij random 1 van de sterkste plekken.
                int x = lijst1[sterkste_plek[rnd]]; // Die sterkste plek correspondeert met een sterkste zet en die wordt hier gedaan.
                int y = lijst2[sterkste_plek[rnd]];
                if (counter % 2 == 0)
                    grid[x, y] = -1;
                else
                    grid[x, y] = 1;
                inkleur_methode(x, y, kleur1, kleur2, true);
                lijst1.Clear();
                lijst2.Clear();
                counter++;
                speelbord1.Invalidate();
            }
            zet_overslaan();
        }

        public void intermediatebot()
        {
            bool zet_gedaan = false;
            int hoogste_sterkte = 0;
            int kleur1 = kleurbepaler1();
            int kleur2 = kleurbepaler2();
            List<int> lijst2 = new List<int>();
            List<int> lijst1 = new List<int>();

            for (int i = 0; i < bord_formaat; i++)
            {
                for (int j = 0; j < bord_formaat; j++)
                {
                    if (ControleBieb.legitiemcheck(i, j, kleur1, kleur2, grid, bord_formaat) && grid[i, j] == 0) // Kijkt welke zetten legitiem zijn en slaat de x en y waarde van dat hokje in 2 lijsten op dezelfde plaats in de lijsten.
                    {
                        lijst1.Add(i);
                        lijst2.Add(j);
                        zet_gedaan = true;
                    }
                }
            }

            List<int> lijst_sterktes = new List<int>();

            for (int k = 0; k < lijst1.Count; k++) // Voor elke legitieme zet in de lijst wordt de sterkte bepaald (de hoeveelheid blokjes die worden geslagen) en aan een lijst toegevoegt.
            {
                inkleur_methode(lijst1[k], lijst2[k], kleur1, kleur2, false); // False wordt meegegeven zodat de waardes niet veranderen van de grids.
                lijst_sterktes.Add(sterkte_zet);
            }

            for (int l = 0; l < lijst_sterktes.Count; l++) // Uit de lijst wordt bepaald welke sterkte het hoogst is.
            {
                if (lijst_sterktes[l] > hoogste_sterkte)
                {
                    hoogste_sterkte = lijst_sterktes[l]; // Die sterkte wordt nu de hoogste sterke.
                }
            }

            List<int> sterkste_plek = new List<int>();
            for (int m = 0; m < lijst_sterktes.Count; m++)
            {
                if (lijst_sterktes[m] == hoogste_sterkte) // Een nieuw lijst slaat alle plekken op waar in de lijst de sterkste waardes qua zet aan verbonden is.
                {
                    sterkste_plek.Add(m);
                }
            }

            if (zet_gedaan)
            {
                Random r = new Random();
                int rnd = r.Next(0, sterkste_plek.Count); // Vervolgens pakt hij random 1 van de sterkste plekken.
                int x = lijst1[sterkste_plek[rnd]]; // Die sterkste plek correspondeert met een sterkste zet en die wordt hier gedaan.
                int y = lijst2[sterkste_plek[rnd]];
                if (counter % 2 == 0)
                    grid[x, y] = -1;
                else
                    grid[x, y] = 1;
                inkleur_methode(x, y, kleur1, kleur2, true);
                lijst1.Clear();
                lijst2.Clear();
                counter++;
                speelbord1.Invalidate();
            }
            zet_overslaan();
        }

        public void beginnerbot()
        {
            bool zet_gedaan = false;
            int kleur1 = kleurbepaler1();
            int kleur2 = kleurbepaler2();
            List<int> lijst2 = new List<int>();
            List<int> lijst1 = new List<int>();

            for (int i = 0; i < bord_formaat; i++)
            {
                for (int j = 0; j < bord_formaat; j++)
                {
                    if (ControleBieb.legitiemcheck(i, j, kleur1, kleur2, grid, bord_formaat) && grid[i, j] == 0) // Kijkt welke zetten legitiem zijn en slaat de x en y waarde van dat hokje in 2 lijsten op dezelfde plaats in de lijsten.
                    {
                        lijst1.Add(i);
                        lijst2.Add(j);
                        zet_gedaan = true;
                    }
                }
            }
            if (zet_gedaan)
            {
                Random r = new Random();
                int rnd = r.Next(0, lijst1.Count()); // Vervolgens wordt er een willekeurige plaats in die lijsten uitgekozen en getekent.
                int x = lijst1[rnd];
                int y = lijst2[rnd];

                if (counter % 2 == 0)
                    grid[x, y] = -1;
                else
                    grid[x, y] = 1;
                inkleur_methode(x, y, kleur1, kleur2, true);
                lijst1.Clear();
                lijst2.Clear();
                counter++;
                speelbord1.Invalidate();
            }
            zet_overslaan();
        }

        private void zet_overslaan()
        {
            if (!ControleBieb.zet_mogelijk(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat) && !ControleBieb.bord_vol(grid, bord_formaat)) // Als er geen zet mogelijk is voor de volgende die aan de beurt is, dan slaat hij een beurt over en komt er een gepaste messagebox.
            {
                if (ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter) && !bottegenbot_check()) // Als een bot tegen een bot speelt, dan worden de meldingen niet getoont.
                {
                    MessageBox.Show("Geen zet mogelijk voor beide partijen, spel is voorbij");
                }
                if (counter % 2 == 0 && !ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter) && !bottegenbot_check())
                    MessageBox.Show("Geen zet mogelijk voor wit");
                if (counter % 2 != 0 && !ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter) && !bottegenbot_check())
                    MessageBox.Show("Geen zet mogelijk voor zwart");
                counter++;
                if (menstegenbot_check() == 1 && counter % 2 != 0 && !ControleBieb.beide_geenzet(kleurbepaler1(), kleurbepaler2(), grid, bord_formaat, counter)) // Als een mens tegen een bot speelt en zwart aan de beurt is, wordt de bot aangeroepen.
                    beginnerbot();
                speelbord1.Invalidate();
            }
        }

        private bool bottegenbot_check() // Controleert of een bot tegen een bot speelt.
        {
            if (form.keuze_wit != 1 && form.keuze_zwart != 1)
                return true;
            else
                return false;
        }

        public string label_aanzet()
        {
            string label;
            if (counter % 2 == 0)
                label = "Wit is aan de beurt";
            else
                label = "Zwart is aan de beurt";
            return label;
        }

        public void help_click()
        {
            {
                if (!helpteken_clicked)
                    helpteken_clicked = true;
                else
                    helpteken_clicked = false;
            }
            speelbord1.Invalidate();
        }

        private int menstegenbot_check()
        {
            if (form.keuze_wit == 1 && (form.keuze_zwart == 2 || form.keuze_zwart == 3 || form.keuze_zwart == 4)) // Deze methode om te kunnen onderscheiden of speler wit tegen een bot speelt,
            {
                return 1;
            }

            if (form.keuze_zwart == 1 && (form.keuze_wit == 2 || form.keuze_wit == 3 || form.keuze_wit == 4)) // of speler zwart.
            {
                return 2;
            }
            return 0;
        }

        public int x_waarde_muis(int x) // Bij elke muiswaardew hoort een bepaald veldje, een array waarde. 
        {
            int i = 0;

            for (int k = 0; k < bord_formaat; k++)
            {
                if (x < (k + 1) * afmeting && x > (k * afmeting))
                {
                    i = k;
                    break;
                }
            }
            return i;
        }

        public int y_waarde_muis(int y)
        {
            int j = 0;

            for (int h = 0; h < bord_formaat; h++)
            {
                if (y < afmeting * (h + 1) && y > h * afmeting)
                {
                    j = h;
                    break;
                }
            }
            return j;
        }

        private void inkleur_methode(int i, int j, int kleur1, int kleur2, bool b)
        {
            sterkte_zet = 0;
            teken_middenboven(i, j, kleur1, kleur2, b);
            teken_middenonder(i, j, kleur1, kleur2, b);
            teken_middenlinks(i, j, kleur1, kleur2, b);
            teken_middenrechts(i, j, kleur1, kleur2, b);
            teken_linksboven(i, j, kleur1, kleur2, b);
            teken_rechtsonder(i, j, kleur1, kleur2, b);
            teken_rechtsboven(i, j, kleur1, kleur2, b);
            teken_linksonder(i, j, kleur1, kleur2, b);
        }

        private int kleurbepaler1()
        {
            int kleur1;
            if (counter % 2 == 0)
                kleur1 = 1; // Zwart
            else
                kleur1 = -1; // Wit
            return kleur1;
        }

        private int kleurbepaler2()
        {
            int kleur2;
            if (counter % 2 == 0)
                kleur2 = -1; // Wit
            else
                kleur2 = 1; // Zwart
            return kleur2;
        }

        #region 8 inkleur methodes
        private void teken_middenboven(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k <= j; k++)
            {
                if (grid[i, (j - k)] == 0) // Als er bij de k-waarde een leeg vakje hoort, dan gaat hij niet verder.
                    return;
                if (grid[i, (j - k)] == kleur2) // Bij geen leeg vakje controleert hij bij welke k-waarde dezelfde kleur als de gene die aan de beurt is gevonden wordt.
                {
                    for (int h = 1; h < k; h++) // Elke tussen waar de persoon klikt, en de k-waarde (dus waar dezelfde kleur is gevonden) wordt getekent.
                    {
                        if (b)
                            grid[i, j - h] = kleur2;
                        sterkte_zet++;
                    }
                    break; // Zodra er een zelfde kleur is gevonden in de rij, dan stopt hij met zoeken om te voorkomen dat de kleuren door veranderen als er weer meerdere kleuren zijn.
                }
            }
        }

        private void teken_middenonder(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k < (bord_formaat - j); k++)
            {
                if (grid[i, (j + k)] == 0)
                    return;
                if (grid[i, (j + k)] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i, j + h] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }

        private void teken_middenlinks(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k <= i; k++)
            {
                if (grid[(i - k), j] == 0)
                    return;
                if (grid[(i - k), j] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i - h, j] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }

        private void teken_middenrechts(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k < (bord_formaat - i); k++)
            {
                if (grid[(i + k), j] == 0)
                    return;
                if (grid[(i + k), j] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i + h, j] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }

        private void teken_linksboven(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k <= Math.Min(i, j); k++)
            {
                if (grid[(i - k), (j - k)] == 0)
                    return;
                if (grid[(i - k), (j - k)] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i - h, j - h] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }

        private void teken_rechtsonder(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k < Math.Min((bord_formaat - i), (bord_formaat - j)); k++)
            {
                if (grid[(i + k), (j + k)] == 0)
                    return;
                if (grid[(i + k), (j + k)] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i + h, j + h] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }

        private void teken_rechtsboven(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k <= Math.Min((bord_formaat - i - 1), (j)); k++)
            {
                if (grid[(i + k), (j - k)] == 0)
                    return;
                if (grid[(i + k), (j - k)] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i + h, j - h] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }

        private void teken_linksonder(int i, int j, int kleur1, int kleur2, bool b)
        {
            for (int k = 1; k <= Math.Min(i, (bord_formaat - j - 1)); k++)
            {
                if (grid[(i - k), (j + k)] == 0)
                    return;
                if (grid[(i - k), (j + k)] == kleur2)
                {
                    for (int h = 1; h < k; h++)
                    {
                        if (b)
                            grid[i - h, j + h] = kleur2;
                        sterkte_zet++;
                    }
                    break;
                }
            }
        }


        #endregion       
    }
}

public class ControleBieb
{
    public static int vakje_ligging(int x, int y, int bord_formaat)
    {
        if ((x == 0 && y == 0) || (x == (bord_formaat - 1) && y == 0) || (x == 0 && y == (bord_formaat - 1)) || (x == (bord_formaat - 1) && y == (bord_formaat - 1))) // De hoekjes zijn sterke plekken, die krijgen altijd voorkeur.
            return 1;
        else if ((x == 1 && y == 1) || (x == 0 && y == 1) || (x == 1 && y == 0) || (x == 1 && y == (bord_formaat - 2)) || (x == 0 && y == (bord_formaat - 2)) || (x == 1 && y == (bord_formaat - 1)) || (x == (bord_formaat - 1) && y == 1) || (x == (bord_formaat - 2) && y == 1) || (x == (bord_formaat - 2) && y == 0) || (x == (bord_formaat - 1) && y == (bord_formaat - 2)) || (x == (bord_formaat - 2) && y == (bord_formaat - 2)) || (x == (bord_formaat - 2) && y == (bord_formaat - 1)))
            return 2; // Alle vakjes die naast een hoekje liggen extra zwak, dus die krijgen weinig sterkte erbij.
        else if (x == 1 || y == 1 || x == (bord_formaat - 2) || y == (bord_formaat - 2))
            return 3; // De vakken voor de zijkanten zijn iets zwakker dan normale vakjes.
        else if (x == 0 || x == (bord_formaat - 1) || y == 0 || y == (bord_formaat - 1)) // Zijkanten zijn sterker dan normale vakjes.
            return 4;
        else return 0;
    }

    public static int aantal_wit(int[,] grid, int bord_formaat) // Kijkt hoeveel vakjes er wit zijn.
    {
        int aantal_wit = 0;
        for (int i = 0; i < bord_formaat; i++)
        {
            for (int j = 0; j < bord_formaat; j++)
            {
                if (grid[i, j] == -1)
                    aantal_wit++;
            }
        }
        return aantal_wit;
    }

    public static int aantal_zwart(int[,] grid, int bord_formaat) // Kijkt hoeveel vakjes er zwart zijn.
    {
        int aantal_zwart = 0;
        for (int i = 0; i < bord_formaat; i++)
        {
            for (int j = 0; j < bord_formaat; j++)
            {
                if (grid[i, j] == 1)
                    aantal_zwart++;
            }
        }
        return aantal_zwart;
    }

    public static bool beide_geenzet(int kleur1, int kleur2, int[,] grid, int bord_formaat, int counter) // Controleert of beide spelers geen zet kunnen doen.
    {
        if (!zet_mogelijk(kleur1, kleur2, grid, bord_formaat) && !bord_vol(grid, bord_formaat))
        {
            if (!zet_mogelijk(kleur2, kleur1, grid, bord_formaat) && !bord_vol(grid, bord_formaat)) // Kleuren worden omgedraaid om te testen of de tegenstander ook geen zet heeft.
            {

                return true;
            }
            return false;
        }
        else
            return false;
    }

    public static bool zet_mogelijk(int kleur1, int kleur2, int[,] grid, int bord_formaat) // Controleert of de speler aan de beurt, een zet kan doen.
    {
        for (int i = 0; i < bord_formaat; i++)
        {
            for (int j = 0; j < bord_formaat; j++)
            {
                if (legitiemcheck(i, j, kleur1, kleur2, grid, bord_formaat) && grid[i, j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool legitiemcheck(int i, int j, int kleur1, int kleur2, int[,] grid, int bord_formaat) // Controleert of er een zet mogelijk is op het vakje dat hij als parameter krijgt.
    {
        int teken_controle = 0;
        bool tekencontrole = false;
        if (grid[i, j] == 0)
        {
            teken_controle = middenboven(i, j, kleur1, kleur2, grid) + middenonder(i, j, kleur1, kleur2, grid, bord_formaat) +
            middenlinks(i, j, kleur1, kleur2, grid) + middenrechts(i, j, kleur1, kleur2, grid, bord_formaat) +
            rechtsonder(i, j, kleur1, kleur2, grid, bord_formaat) + rechtsboven(i, j, kleur1, kleur2, grid, bord_formaat) +
            linksonder(i, j, kleur1, kleur2, grid, bord_formaat) + linksboven(i, j, kleur1, kleur2, grid);

            if (teken_controle >= 1)
                tekencontrole = true;
        }
        return tekencontrole;
    }

    public static bool bord_vol(int[,] grid, int bord_formaat) // Controleert of het bord vol is.
    {
        for (int i = 0; i < bord_formaat; i++)
        {
            for (int j = 0; j < bord_formaat; j++)
            {
                if (grid[i, j] == 0)
                    return false;
            }
        }
        return true;
    }

    #region 8 methode checks
    public static int middenboven(int i, int j, int kleur1, int kleur2, int[,] grid)
    {

        int middenboven = 0;
        int k = 0;

        for (k = 1; k <= j; k++) // k <= j omdat de waardes anders buiten de array vallen
        {
            if (grid[i, (j - k)] == 0) // Controleert of er geen lege vakjes tussen staat.
                return 0;
            if (grid[i, (j - 1)] == kleur1 && grid[i, (j - k)] == kleur2) // Controleert of het vakje links een andere kleur heeft en of daarna weer een vakje van dezelfde kleur komt.
            {
                middenboven = 1;
                break; // Zodra er een vakje van dezelfde kleur als de kleur die aan de beurt is wordt gevonden stopt de loop. Zodat er achter het gevonden vakje wel weer een leeg vakje mag staan.
            }
        }
        return middenboven;
    }

    public static int middenonder(int i, int j, int kleur1, int kleur2, int[,] grid, int bord_formaat)
    {
        int middenonder = 0;
        int k = 0;

        for (k = 1; k < (bord_formaat - j); k++)
        {
            if (grid[i, (j + k)] == 0)
                return 0;
            if (grid[i, (j + 1)] == kleur1 && grid[i, (j + k)] == kleur2)
            {
                middenonder = 1;
                break;
            }
        }
        return middenonder;
    }

    public static int middenlinks(int i, int j, int kleur1, int kleur2, int[,] grid)
    {
        int middenlinks = 0;
        int k = 0;

        for (k = 1; k <= i; k++)
        {
            if (grid[(i - k), j] == 0)
                return 0;
            if (grid[(i - 1), j] == kleur1 && grid[(i - k), j] == kleur2)
            {
                middenlinks = 1;
                break;
            }
        }
        return middenlinks;
    }

    public static int middenrechts(int i, int j, int kleur1, int kleur2, int[,] grid, int bord_formaat)
    {
        int middenrechts = 0;
        int k = 0;

        for (k = 1; k < (bord_formaat - i); k++)
        {
            if (grid[(i + k), j] == 0)
                return 0;
            if (grid[(i + 1), j] == kleur1 && grid[(i + k), j] == kleur2)
            {
                middenrechts = 1;
                break;
            }
        }
        return middenrechts;
    }

    public static int linksboven(int i, int j, int kleur1, int kleur2, int[,] grid)
    {
        int linksboven = 0;
        int k = 0;

        // Neem minimale van i en j, zodat k nooit groter wordt dan i of j en dus de waarde i-k of j-k niet buiten de array komt.      
        for (k = 1; k <= Math.Min(i, j); k++)
        {
            if (grid[(i - k), (j - k)] == 0)
                return 0;
            if (grid[(i - 1), (j - 1)] == kleur1 && grid[(i - k), (j - k)] == kleur2)
            {
                linksboven = 1;
                break;
            }
        }
        return linksboven;
    }

    public static int rechtsonder(int i, int j, int kleur1, int kleur2, int[,] grid, int bord_formaat)
    {
        int rechtsonder = 0;
        int k = 0;

        for (k = 1; k < Math.Min((bord_formaat - i), (bord_formaat - j)); k++)
        {
            if (grid[(i + k), (j + k)] == 0)
                return 0;
            if (grid[(i + 1), (j + 1)] == kleur1 && grid[(i + k), (j + k)] == kleur2)
            {
                rechtsonder = 1;
                break;
            }
        }
        return rechtsonder;
    }

    public static int rechtsboven(int i, int j, int kleur1, int kleur2, int[,] grid, int bord_formaat)
    {
        int rechtsboven = 0;
        int k = 0;

        for (k = 1; k <= Math.Min((bord_formaat - i - 1), (j)); k++)
        {
            if (grid[(i + k), (j - k)] == 0)
                return 0;
            if (grid[(i + 1), (j - 1)] == kleur1 && grid[(i + k), (j - k)] == kleur2)
            {
                rechtsboven = 1;
                break;
            }
        }
        return rechtsboven;
    }

    public static int linksonder(int i, int j, int kleur1, int kleur2, int[,] grid, int bord_formaat)
    {
        int linksonder = 0;
        int k = 0;

        for (k = 1; k <= Math.Min(i, (bord_formaat - j - 1)); k++)
        {
            if (grid[(i - k), (j + k)] == 0)
                return 0;
            if (grid[(i - 1), (j + 1)] == kleur1 && grid[(i - k), (j + k)] == kleur2)
            {
                linksonder = 1;
                break;
            }
        }
        return linksonder;
    }
    #endregion
}
