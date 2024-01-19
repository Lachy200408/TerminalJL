using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Terminal
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = ColorTranslator.FromHtml("#101018");
            this.richTextBox1.ForeColor = Color.White;
            this.richTextBox1.BorderStyle = BorderStyle.None;
            this.richTextBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            // this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            //Esta talla era la impedia el funcionamiento del layout
            //asi que mejor dejarlo asi
            this.richTextBox1.Location = new Point(0, 30);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(1099, 455);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            //
            // Buttons 
            //
            this.closeButton = new MyButton("close",this);
            this.minButton = new MyButton("min",this);

            this.closeButton.Location = new Point(1059 , 0);
            this.minButton.Location = new Point(1019 , 0);

            this.closeButton.BackgroundImage = Image.FromFile("./close.png");
            this.minButton.BackgroundImage = Image.FromFile("./minimize.png");
            //
            // Icono
            //
            this.icono = new PictureBox();
            this.icono.Image = Image.FromFile("./Recurso 2.png");
            this.icono.Location = new Point(5,5);
            this.icono.Size = new Size(30,20);
            this.icono.SizeMode = PictureBoxSizeMode.StretchImage;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = ColorTranslator.FromHtml("#151523");
            this.ClientSize = new Size(1099, 485);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.minButton);
            this.Controls.Add(this.icono);
            this.ForeColor = Color.White;
            this.Name = "Jela";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            //Poner el titulo en la pantalla
            this.Paint += new PaintEventHandler((object sender, PaintEventArgs e)=>{
                e.Graphics.DrawString("Jela Bash", new Font("Arial", 12), Brushes.White, 540, 5);
            });
        }

        #endregion
        private RichTextBox richTextBox1;
        private MyButton closeButton, minButton;
        private PictureBox icono;

        #region Manejador de clicks de los botones de la ventana
            public EventHandler buttonClick = (object sender, EventArgs e)=>{
                var btn = (MyButton)sender;
                var ventana = (Form1)btn.Parent;

                switch(btn.Name){
                    case "close":
                        ventana.Close();
                        break;
                    case "min":
                        ventana.WindowState = FormWindowState.Minimized;
                        break;
                }
            };
        #endregion
    }

    //Rounded corners
    public class RoundForm : Form
    {
        #region Region de hacer las esquinas redondeadas
            
            public RoundForm(){
                this.FormBorderStyle = FormBorderStyle.None;
                this.DoubleBuffered =  true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                using (var path = new System.Drawing.Drawing2D.GraphicsPath()){
                    int radius = 30;
                    
                    path.AddArc(0,0,radius,radius,180,90);
                    path.AddArc(this.Width-radius,0,radius,radius,270,90);
                    path.AddArc(this.Width-radius,this.Height-radius,radius,radius,0,90);
                    path.AddArc(0,this.Height-radius,radius,radius,90,90);

                    this.Region = new Region(path);
                }
            }

        #endregion

        #region Region de habilitar mover la ventana con clicks
            
            private bool mouseDown;
            private Point lastLocation;

            private void DraggableForm_MouseDown(object sender, MouseEventArgs e)
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void DraggableForm_MouseMove(object sender, MouseEventArgs e)
            {
                if (mouseDown)
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                    this.Update();
                }
            }

            private void DraggableForm_MouseUp(object sender, MouseEventArgs e)
            {
                mouseDown = false;
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                this.MouseDown += DraggableForm_MouseDown;
                this.MouseMove += DraggableForm_MouseMove;
                this.MouseUp += DraggableForm_MouseUp;
            }

        #endregion
    }

    public class MyButton : Button
    {
        public MyButton(string _name, Form1 form){
            this.Size = new Size(40,30);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.Name = _name;

            this.MouseEnter += this.mouseOver;
            this.MouseLeave += this.mouseLeave;
            this.Click += form.buttonClick;
        }

        //Funciones de manejo de eventos
        #region mouseOver
            private EventHandler mouseOver = (object sender, EventArgs e)=>{
                var btn = (MyButton)sender;

                switch(btn.Name){
                    case "close":
                        btn.BackColor = ColorTranslator.FromHtml("#f33");
                        break;
                    case "min":
                        btn.BackColor = ColorTranslator.FromHtml("#181826");
                        break;
                }
            };
        #endregion

        #region mouseLeave
            private EventHandler mouseLeave = (object sender, EventArgs e)=>{
                var btn = (MyButton)sender;

                switch(btn.Name){
                    case "close":
                        btn.BackColor = Color.Transparent;
                        break;
                    case "min":
                        btn.BackColor = Color.Transparent;
                        break;
                }
            };
        #endregion
    }
}