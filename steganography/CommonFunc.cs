using System;
using System.Collections.Generic;
using System.Drawing;

namespace steganography.Functions
{
    public static class CommonFunc
    {
        //функция для получения размера длины (для определения числа байтов)
        public static byte Size(int len)
        {
            if (len > 0 && len < 256)
                return 1; //2^8 (каждая цифра занимает 1 байт)
            else if (len < 65536)
                return 2; //2^16 (каждая цифра занимает 2 байта)
            else
                return 4; //2^32
        }

        //функция для получения байтов из длины в соотвествии с размером текста 
        public static byte[] LenInBytes(int len, int sizeLen)
        {
            if (sizeLen == 1) // преобразуем длину в массив из одного байта
            {
                var arr = new byte[1] { (byte)len };
                return arr;
            }
            else if (sizeLen == 2) // преобразуем длину в массив из двух байтов
                return BitConverter.GetBytes((short)len);
            else
                return BitConverter.GetBytes(len); // преобразуем длину в массив из четырех байтов
        }

        // функция для перевода байта в булевый массив
        public static bool[] ByteBoolArr(byte b)
        {
            bool[] arr = new bool[8]; // размер массива соотвествует 8 битам в одном байте
            for (int i = 0; i < 8; i++)
                arr[i] = (b & (1 << i)) != 0 ? true : false; // побитово проходимся по байту, заполняем в соответсвии со значением бита  
            Array.Reverse(arr); //переворачиваем массив, чтобы булевые перемнные стояли в праивльном порядке соответствующем порядке битов
            return arr;
        }

        // функция для перевода булевого массива в байт 
        public static byte BoolArrByte(bool[] arr)
        {
            byte res = 0;
            int ind = 8 - arr.Length;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                    res |= (byte)(1 << (7 - ind)); // побитово увеличиваем байт, в случае true значения элемента булевого массива
                ind++;
            }
            return res;
        }

        // функция для перевода байтового списка в целочисленнное значение (до size, чтобы получить именно значение длины сообщения)
        public static int IntBytes(List<byte> lenBytes)
        {
            int ans = 0;
            for (int i = 0; i < lenBytes.Count; i++)
                ans |= lenBytes[i] << i * 8; // побайтово увеличиваем целочисленное значение
            return ans;
        }

        //обрезаем изображение в случае неделения поровну на сегменты
        public static void Cut(ref Bitmap img, int sizeSegm)
        {
            int x = img.Width % sizeSegm; //остаток от разбиения на сегменты по горизонтали
            int y = img.Height % sizeSegm; //остаток от разбиения на сегменты по вертикали
            var newImg = new Bitmap(img.Width - x, img.Height - y);
            for (int i = 0; i < newImg.Width; i++)
            {
                for (int j = 0; j < newImg.Height; j++)
                    newImg.SetPixel(i, j, img.GetPixel(i, j));
            }
            img = newImg;
        }
    }
}
