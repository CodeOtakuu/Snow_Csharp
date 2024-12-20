using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snow_Csharp
{
    public partial class SnowForm : Form
    {
        struct Snow
        {
            public float Drift { get; set; }
            public float Speed { get; set; }
            public float WindowOfSet { get; set; }
            public PointF pos;

            private static Random rd = new Random();

           
            public Snow(PointF startPosition)
            {
                Drift = ((float)rd.NextDouble()) * 2.0f * 3.1f;
                Speed = 1.0f + ((float)rd.NextDouble()) * 2.0f;
                WindowOfSet = ((float)rd.NextDouble()) - 0.5f;

                pos = startPosition;
            }

            
            public void Draw(Graphics g)
            {
                using (Pen pen = new Pen(Color.Snow))
                {
                    g.DrawEllipse(pen, pos.X, pos.Y, 6, 6);
                }
            }

            
            public void UpdatePosition(int windowHeight, int windowWidth)
            {
                pos.Y += Speed; 
                Drift += 0.05f;
                pos.X += MathF.Sin(Drift) * 0.5f + WindowOfSet;

                
                if (pos.Y > windowHeight)
                {
                    pos.Y = 0;
                    pos.X = rd.Next(0, windowWidth);
                }
            }
        }

        private Snow[] flakes = new Snow[1200]; 
        private System.Windows.Forms.Timer timer; 
      
     

        public SnowForm()
        {
            InitializeComponent();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("C:\\Users\\user\\Downloads\\p1.wav");
            _ = Task.Run(() => player.Play());

            Random rand = new Random();
            for (int i = 0; i < flakes.Length; i++)
            {
                flakes[i] = new Snow(new PointF(rand.Next(0, this.Width), rand.Next(0, this.Height)));
            }

            
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10; 
            timer.Tick += (s, e) => { UpdateSnow(); };
            timer.Start();

            this.DoubleBuffered = true;
            this.BackColor = Color.Black; 
        }

        
        private void UpdateSnow()
        {
            for (int i = 0; i < flakes.Length; i++)
            {
                flakes[i].UpdatePosition(this.ClientSize.Height, this.ClientSize.Width);
            }

            this.Invalidate(); 
        }

     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int i = 0; i < flakes.Length; i++)
            {
                flakes[i].Draw(g);
            }
        }
    }
}

