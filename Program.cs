using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Smash64BackgroundResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            using (FileStream imgStream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                Bitmap bitmap1 = new Bitmap(imgStream);
                Bitmap bitmap2;

                //Image 2 is a 300x220 copy of the image if image 1 is (300, 263) 
                if (bitmap1.Width == 300 && bitmap1.Height == 263)
                {
                    bitmap2 = ResizeBitmap(bitmap1, 300, 263);
                }
                //If image1 is (300, 220), then make image1 (300, 263)
                else if (bitmap1.Height == 220)
                {
                    bitmap2 = (Bitmap)bitmap1.Clone();
                    bitmap1 = ResizeBitmap(bitmap2, 300, 263);
                }
                else
                {
                    Console.WriteLine("Invalid image size! Supported sizes are 300x220 and 300x263");
                    return;
                }

                //bitmap1.Save("output.png", System.Drawing.Imaging.ImageFormat.Png);
                //bitmap2.Save("output2.png", System.Drawing.Imaging.ImageFormat.Png);
                //return;

                for (int i = 0; i < 6; i++)
                {
                    CopyRow(bitmap1, bitmap2, i, i);
                }
                CopyRow(bitmap1, bitmap2, 5, 6);

                int count = 0;
                int row = 0;

                //From row 8, every 5th row is a duplicate
                for (int i = 7; i < 221; i++)
                {
                    if (i + row > 263)
                    {
                        break;
                    }
                    CopyRow(bitmap1, bitmap2, i - 1, i + row);
                    count++;
                    if (count == 5 && i < 263)
                    {
                        row++;
                        CopyRow(bitmap1, bitmap2, i - 1, i + row);
                        count = 0;
                    }
                }

                //Save the imagge
                bitmap1.Save("output.png", System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("Success! Saved as output.png!");
            }
        }

        //Resizes a bitmap
        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
            return result;
        }

        //Copies a row of pixels from one image to another
        public static void CopyRow(Bitmap map1, Bitmap map2, int rowSource, int rowDest)
        {
            for (int i = 0; i < 300; i++)
            {
                map1.SetPixel(i, rowDest, map2.GetPixel(i, rowSource));
            }
        }
    }
}
