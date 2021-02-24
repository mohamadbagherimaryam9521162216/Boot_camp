using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;



namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> names = new List<string>();
            names.Add("John Smith");
            names.Add("Maryam mohammadbagheri");
            names.Add("sara rezaii");
            names.Add("Jane Doe");
            names.Add("chi c");

            string ImagePathFile = @"C:\cer.bmp";//read path
            string path = @"C:\images\";  //store path
            int h = 0 +2-2;
            foreach (var full_name in names)
            { 
                System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile(ImagePathFile); // set image 
   
                Graphics graphicsImage = Graphics.FromImage(bitmap);
                int k = 0;
                StringFormat stringformat = new StringFormat();
                stringformat.Alignment = StringAlignment.Far;
                stringformat.LineAlignment = StringAlignment.Far;
         

                Color StringColor = System.Drawing.ColorTranslator.FromHtml("#000000");//direct color adding
                string file_name = string.Concat(path, full_name, ".Jpeg");
               
                int y = 570;
                int x = full_name.Length/2;

  
                if (full_name.Length < 20 && full_name.Length > 14 )
                    x *= 37;
                else
                    x *= 42; 
                graphicsImage.DrawString(full_name, new Font("OTAMENDI", 24,
                FontStyle.Regular), new SolidBrush(StringColor), new Point(1103 + x+k, y+h),
                stringformat);
                bitmap.Save(file_name, ImageFormat.Jpeg);//store 

            }





        }

    }
}
