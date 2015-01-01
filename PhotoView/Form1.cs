using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoView
{
    public partial class Form1 : Form
    {
        int resHeight;
        int resWidth;

        public Form1(String [] args)
        {
            InitializeComponent();

            //drag and drop setup
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            //we need the resolution of the screen to determine resizing sizes
            resWidth = Screen.PrimaryScreen.Bounds.Width;
            resHeight = Screen.PrimaryScreen.Bounds.Height;

            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            //if a file was loaded from explorer or the command line, show it
            if (args.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(args[0]);
                fileOpen(args[0]);
            }
            //otherwise, show the black screen for dragging.
        }

        
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        //array of files in the folder
        string[] paths;

        //the position in the array where the current file is.
        int currentFile;

        void changeFolder(String path)
        {
            //chop up the path and get the directory from it.
            Boolean chopped = false;
            String dir = "";

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if ((chopped == false) && (path[i] == '/' || path[i] == '\\'))
                {
                    //we cut it off here
                    dir = path.Substring(0, i);
                    chopped = true;
                }
            }

            //get a list of all the files in the folder
            paths = Directory.GetFiles(dir);

            //figure out where this file is in the folder so we can go back and forth through the dir
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].Equals(path)) currentFile = i;
            }
        }

        void fileOpen(String path)
        {
            pictureBox1.Load(path);
            //pictureBox1.Height = pictureBox1.Image.Height;

            if (WindowState != FormWindowState.Maximized)
            {
                //figure out the size and aspect ratio of the image
                double height = (double)pictureBox1.Image.Height;
                double width = (double)pictureBox1.Image.Width;
                double aspect = width / height;

                //keep pictures on the screen, no matter how huge they are via resizing.
                if (height > resHeight)
                {
                    height = (int)(resHeight * .8);
                    width = (int)(height * aspect);

                    if (width > height) this.Size = new Size((int)(width * .95), (int)height);
                    else this.Size = new Size((int)(width), (int)height);
                }
                //if the image isn't taller than the screen, don't worry about resizing anything.
                else
                {
                    this.Size = new Size((int)width, (int)height);
                }
            }
        }

        //drag and drop handler
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            fileOpen(files[0]);
           
        }

        //maximize or restore the window on click
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
