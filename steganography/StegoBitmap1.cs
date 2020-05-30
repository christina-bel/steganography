using System;
using System.Drawing;
using System.IO;

namespace steganography
{
    public enum Colours { Red, Green, Blue } //перечисление цветов 

    public class StegoBitmap
    {
        readonly private Bitmap sourceBitmap; // Bitmap исходного файла (доступ только для чтения)

        public string FileSize { get; } // размер файла в пикселях
        public byte[] RedColour { get; } // байты красного цвета
        public byte[] GreenColour { get; } // байты зеленого цвета
        public byte[] BlueColour { get; } // байты голубого цвета

        // конструктор
        public StegoBitmap(Bitmap bitmap)
        {
            sourceBitmap = bitmap;  
            FileSize = bitmap.Height.ToString()+" x "+ bitmap.Width.ToString();
            RedColour = ReadColour(bitmap, Colours.Red);
            GreenColour = ReadColour(bitmap, Colours.Green);
            BlueColour = ReadColour(bitmap, Colours.Blue);
        }

        // конструктор
        public StegoBitmap(StegoBitmap stgBitmap, byte[] changedColour, Colours c)
        {
            RedColour = stgBitmap.RedColour;
            GreenColour = stgBitmap.GreenColour;
            BlueColour = stgBitmap.BlueColour;
            if (c == Colours.Red)
                RedColour = changedColour;
            if (c == Colours.Green)
                GreenColour = changedColour;
            if (c == Colours.Blue)
                BlueColour = changedColour;

            sourceBitmap = new Bitmap(stgBitmap.sourceBitmap.Width, stgBitmap.sourceBitmap.Height);

            int ind = 0;
            for (int i = 0; i < sourceBitmap.Width; i++)
            {
                for (int j = 0; j < sourceBitmap.Height; j++)
                {
                    sourceBitmap.SetPixel(i, j, Color.FromArgb(RedColour[ind], GreenColour[ind], BlueColour[ind])); // устанавливает пиксель в нужном месте в соответвии с переданным цветом 
                    ind++;
                }
            }
        }
               
        // конструктор
        public StegoBitmap(Bitmap bmap, double[,] newArr, Colours c)
        {
            sourceBitmap = new Bitmap(bmap.Width, bmap.Height);
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    if (c == Colours.Red)
                        sourceBitmap.SetPixel(i, j, Color.FromArgb((byte)Math.Round(newArr[i, j]), bmap.GetPixel(i, j).G, bmap.GetPixel(i, j).B));
                    else if (c == Colours.Green)
                        sourceBitmap.SetPixel(i, j, Color.FromArgb(bmap.GetPixel(i, j).R, (byte)Math.Round(newArr[i, j]), bmap.GetPixel(i, j).B));
                    else
                        sourceBitmap.SetPixel(i, j, Color.FromArgb(bmap.GetPixel(i, j).R, bmap.GetPixel(i, j).G, (byte)Math.Round(newArr[i, j])));                   
                }
            }
            RedColour = ReadColour(sourceBitmap, Colours.Red);
            GreenColour = ReadColour(sourceBitmap, Colours.Green);
            BlueColour = ReadColour(sourceBitmap, Colours.Blue);
        }

        // конструктор
        public StegoBitmap(Bitmap bmap, byte[,] newArr, Colours c)
        {
            sourceBitmap = new Bitmap(bmap.Width, bmap.Height);
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    if (c == Colours.Red)
                        sourceBitmap.SetPixel(i, j, Color.FromArgb(newArr[i, j], bmap.GetPixel(i, j).G, bmap.GetPixel(i, j).B));
                    else if (c == Colours.Green)
                        sourceBitmap.SetPixel(i, j, Color.FromArgb(bmap.GetPixel(i, j).R, newArr[i, j], bmap.GetPixel(i, j).B));
                    else
                        sourceBitmap.SetPixel(i, j, Color.FromArgb(bmap.GetPixel(i, j).R, bmap.GetPixel(i, j).G, newArr[i, j]));
                }
            }
            RedColour = ReadColour(sourceBitmap, Colours.Red);
            GreenColour = ReadColour(sourceBitmap, Colours.Green);
            BlueColour = ReadColour(sourceBitmap, Colours.Blue);
        }

        // функция возвращает Bitmap исходного файла (для задачи изображения в pictureBox)
        public Bitmap GetImage()
        {
            return sourceBitmap;
        }

        // функция возвращает максимально возможно число вместимых символов для метода 1 (т.к. для одного символа из одного байта при замене одного наименьшего значащего бит пикселя тербуется 8 пикселей)
        public int GetMaxCapacityMethod1()
        {
            return (sourceBitmap.Width * sourceBitmap.Height / 8) - 2 - (sourceBitmap.Width * sourceBitmap.Height / 8).ToString().Length;//отнимаем с учетом символа метки, размера текста и максимальной длины, т.к. эта информация также внедряется в сообщение 
        }


    // функция возвращает максимально возможно число вместимых символов для метода 2 (т.к. для одного символа из одного байта при замене одного наименьшего значащего бит пикселя тербуется 8 пикселей)
    public int GetMaxCapacityMethod2()
        {
            int wid = sourceBitmap.Width;
            int height = sourceBitmap.Height;
            if ((wid % 8) != 0 || (wid % 8) != 0) //если изображение не делится на сегменты ровно (изображение обрежется в будущем)  
            {
                wid -= wid % 8; //остаток от разбиения на сегменты по горизонтали
                height -= height % 8; //остаток от разбиения на сегменты по вертикали
            }
            int numSeg = (wid * height) / (8 * 8);
            return (numSeg / 8) - 2 - (numSeg / 8).ToString().Length; //отнимаем с учетом символа метки, размера текста и максимальной длины, т.к. эта информация также внедряется в сообщение 
        }

        // чтение цвета каждого пикселя в соответсвии с RGB
        private byte[] ReadColour(Bitmap bitmap, Colours c)
        {
            byte[] colArr = new byte[bitmap.Height * bitmap.Width]; // массив байтов, количество элементов которого равно количеству пикселей 
            int ind = 0; // счетчик для прохода по элементам массива байтов
            // проходим по каждому пикселю и выбираем нужный в соответствии с цветом
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                    if (c == Colours.Red)
                        colArr[ind++] = bitmap.GetPixel(i, j).R;
                    else if (c == Colours.Green)
                        colArr[ind++] = bitmap.GetPixel(i, j).G;
                    else if (c == Colours.Blue)
                        colArr[ind++] = bitmap.GetPixel(i, j).B;           
            return colArr;
        }

        // функция возвращает массив байтов указанного цвета для данного StegoBitmap
        public byte[] GetColour(Colours channel)
        {
            if (channel == Colours.Red)
                return RedColour;
            else if (channel == Colours.Green)
                return GreenColour;
            else if (channel == Colours.Blue)
                return BlueColour;
            else
                return null;
        }

        // сохранение изображения
        public void SaveBitmap(string fileName)
        {
            //если файл уже существует, то он удаляется
            if (File.Exists(fileName))
                File.Delete(fileName);
            sourceBitmap.Save(fileName); //сохраняем изображение с выбранным именем файла
        }

    }
}