using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        // Массив объектов прямоугольников для хранения сегментов картинки.
        private PictureBox[] pbSegments;

        // Длина стороны в прямоугольниках.
        private int numRect = 3;

        // Объект хранения картинки.
        private Bitmap Picture;

        

        public Form1()
        {
            InitializeComponent();
        }

        private void ToolStripButtonLoadPicture_Click(object sender, EventArgs e)
        {
            LoadPicture();
        }

        private void LoadPicture()
        {
            var ofDlg = new OpenFileDialog();
            ofDlg.Filter = "файлы картинок (*.bmp;*.jpg;*.jpeg;*.gif;)|";
            ofDlg.Filter += "*.bmp;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";
            ofDlg.FilterIndex = 1;
            ofDlg.RestoreDirectory = true;

    if (ofDlg.ShowDialog() == DialogResult.OK)
    {
                Picture = new Bitmap(ofDlg.FileName);

                
                CreatePictureSegments();
            }
        }

        private void CreatePictureSegments()
        {
           
            if (pbSegments != null)
            {
                for (int i = 0; i < pbSegments.Length; i++)
                {
                    pbSegments[i].Dispose();
                }
                pbSegments = null;
            }


          
            pbSegments = new PictureBox[numRect * numRect];

          
            int w = ClientSize.Width / numRect;
            int h = ClientSize.Height / numRect;

           
            int countX = 0;
            int countY = 0;

            for (int i = 0; i < pbSegments.Length; i++)
            {
               
                pbSegments[i] = new PictureBox
                {
                    Width = w,
                    Height = h,
                    Left = countX * w,
                    Top = countY * h
                };


               
                Point pt = new Point();
                pt.X = pbSegments[i].Left;
                pt.Y = pbSegments[i].Top;

               
                pbSegments[i].Tag = pt;

               
                countX++;
                if (countX == numRect)
                {
                    countX = 0;
                    countY++;
                }


                pbSegments[i].Parent = this;
                pbSegments[i].BorderStyle = BorderStyle.None;
                pbSegments[i].SizeMode = PictureBoxSizeMode.StretchImage;

               
                pbSegments[i].Show();


              
                pbSegments[i].Click += new EventHandler(PB_Click);

            }

            DrawPicture();
        }

        private void DrawPicture()
        {
            if (Picture == null) return;

            int countX = 0;
            int countY = 0;

            for (int i = 0; i < pbSegments.Length; i++)
            {
                int w = Picture.Width / numRect;
                int h = Picture.Height / numRect;
                pbSegments[i].Image =
                    Picture.Clone(new RectangleF(countX * w, countY * h, w, h),
                        Picture.PixelFormat);
                countX++;
                if (countX == numRect)
                {
                    countX = 0;
                    countY++;
                }
            }
        }
        private void CorrectSizeSegments()
        {
            if (pbSegments == null) return;

            // Предыдущие размеры сегментов
            int oldwidth = pbSegments[0].Width;
            int oldheight = pbSegments[0].Height;


            // Новые размеры прямоугольников.
            int w = ClientSize.Width / numRect;
            int h = ClientSize.Height / numRect;



            //int countX = 0; // счетчик прямоугольников по координате X в одном ряду
            //int countY = 0; // счетчик прямоугольников по координате Y в одном столбце
            for (int i = 0; i < pbSegments.Length; i++)
            {
                pbSegments[i].Width = w;
                pbSegments[i].Height = h;

                // Получим порядковый номер сегмента по координате Х
                int countX = pbSegments[i].Left /= oldwidth;

                // Получим порядковый номер сегмента по координате Y
                int countY = pbSegments[i].Top /= oldheight;

                pbSegments[i].Left = countX * w;
                pbSegments[i].Top = countY * h;
            }
        }
        private void MixedSegments()
        {
            if (Picture == null) return;

            Random rand = new Random(Environment.TickCount);
            for (int i = 0; i < pbSegments.Length; i++)
            {
                pbSegments[i].Visible = true;
                int temp = rand.Next(0, pbSegments.Length);
                Point ptR = pbSegments[temp].Location;
                Point ptI = pbSegments[i].Location;
                pbSegments[i].Location = ptR;
                pbSegments[temp].Location = ptI;

                pbSegments[i].BorderStyle = BorderStyle.Fixed3D;
            }

          
            int r = rand.Next(0, pbSegments.Length);
            pbSegments[r].Visible = false;

        }
        private void RestorePicture()
        {
            
            for (int i = 0; i < pbSegments.Length; i++)
            {
                Point point = (Point)pbSegments[i].Tag;
                if (pbSegments[i].Location != point)
                {
                    return;
                }
                for (int m = 0; m < pbSegments.Length; m++)
                {

                    pbSegments[m].Visible = true;
                    pbSegments[m].BorderStyle = BorderStyle.None;
                }

            }
        }
        


    private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PB_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
