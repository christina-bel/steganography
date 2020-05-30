using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using steganography.stegoBitmap;

namespace steganography.Methods
{
    public class LSB
    {
        // перевод строки в массив байтов 
        public static byte[] StringByteArr(string text)
        { 
            return Encoding.GetEncoding(1251).GetBytes(text);
        }

        // перевод массива байтов в строку 
        public static string ByteArrString(byte[] arr)
        {
            return Encoding.GetEncoding(1251).GetString(arr);
        }

        //функция для получения размера длины (для определения числа байтов)
        //private static byte Size(int len)
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

        //статическая функция для замены НЗБ (внедрения текста в изображение путем замены битов)
        public static StegoBitmap Hide(StegoBitmap bitmap, string text, Colours colour)
        {
            byte[] sourceColour = bitmap.GetColour(colour); // массив байтов указанного цвета для данного StegoBitmap
            byte[] stgText = StringByteArr(text); // массив байтов введеных символов
            byte[] len = LenInBytes(text.Length, Size(text.Length)); // массив байтов из длины текста в соответвии с размером его длины            
            byte[] stgBytes = new byte[2 + len.Length + stgText.Length]; //массив для скрытого текста размером в длину текста + размер длины + два дополнительных байта
            stgBytes[0] = Convert.ToByte('L'); // первый элемент массива содержит пометку, что в изображении скрыт текст 
            stgBytes[1] = Size(text.Length); // второй элемент массива размер длины
            int ind = 0;
            for (int i = 0; i < len.Length; i++)
                stgBytes[i + 2] = len[ind++]; // сохраняем в массив байты из длины текста в соответвии с размером его длины   
            ind = 0;
            for (int i = 0; i < stgText.Length; i++)
                stgBytes[i + 2 + len.Length] = stgText[ind++]; // сохраняем в массив скрываемый текст   
            // булевский массив из массива байтов
            bool[] stgBoolBits = stgBytes.SelectMany(e => ByteBoolArr(e)).ToArray(); // SelectMany используется для создания выходной последовательности с проекцией "один ко многим" из входной последовательности. Select вернет один выходной элемент для каждого входного элемента, SelectMany - ноль или более выходных элементов для каждого входного
            // проходим по элементам булевского массива текста. если бит текста true и байт цвета четен, то увеличиваем его на 1, в обратной ситуации - уменьшаем на 1, в остальный случаях байт цвета остается не изменным  (0 и 0 => 0, 1 и 1 => 1, 0 и 1 => 1, 1 и 0 => 0)  
            for (int i = 0; i < stgBoolBits.Length; i++)
               if ((sourceColour[i] % 2 == 0) && stgBoolBits[i]) 
                    sourceColour[i]++;
                else if ((sourceColour[i] % 2 == 1) && !stgBoolBits[i]) 
                    sourceColour[i]--;
            return new StegoBitmap(bitmap, sourceColour, colour);
        }

        // функция для перевода байта в булевый массив
        private static bool[] ByteBoolArr(byte b)
        {
            bool[] arr = new bool[8]; // размер массива соотвествует 8 битам в одном байте
            for (int i = 0; i < 8; i++)
                arr[i] = (b & (1 << i)) != 0 ? true : false; // побитово проходимся по байту, заполняем в соответсвии со значением бита  
            Array.Reverse(arr); //переворачиваем массив, чтобы булевые перемнные стояли в праивльном порядке соответствующем порядке битов
            return arr;
        }

        //функция проверяет наличие скрытого текста, в случае его нахождения, возвращает true и цвет байтов со скрытым текстом
        public static bool IsHiddenText(StegoBitmap stg, out Colours c)
        {
            if (Mark(stg.RedColour))
                c = Colours.Red;
            else if (Mark(stg.GreenColour))
                c = Colours.Green;
            else if (Mark(stg.BlueColour))
                c = Colours.Blue;
            else
            {
                c = 0;
                return false;
            }
            return true;
        }

        //функция проверяет наличие метки
        private static bool Mark(byte[] b)
        {
            var arr = b.Take(8).SelectMany(e => LastBit(e, 1)).ToArray(); // для проверки метки используются только первые 8 байтов, в каждом последнем бите которого может находится бит метки 'L'
            return BoolArrByte(arr) == Convert.ToByte('L'); //переводим булев массив в байты и проверяем совпадение с байтовым представлением метки 
        }

        // функция превращает последний бит байта в булевую переменную
        private static bool[] LastBit(byte b, int num)
        {
            var arr = ByteBoolArr(b); // переводим массив байтов в булев
            return arr.Skip(8 - num).Take(num).ToArray();  // берем только последний элемент массива (отвечающий за последний бит)
        }

        // функция для перевода булевого массива в байт 
        public static byte BoolArrByte(bool[] arr)
        {
            byte res = 0;
            int ind = 8 - arr.Length;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                    res |= (byte)(1 << (7 - ind)); // побитово увеличиваем байт, в случае true начения элмента булевого массива
                ind++;
            }
            return res;
        }

        // функция возвращает скрытый текст
        public static string GetHiddenText(StegoBitmap bitmap, Colours colour)
        {
            byte[] txt = bitmap.GetColour(colour); // массив байтов указанного цвета для данного StegoBitmap
            byte mark = BoolArrByte(txt.Take(8).SelectMany(e => LastBit(e, 1)).ToArray()); // байт метки
            byte sizeLen = BoolArrByte(txt.Skip(8).Take(8).SelectMany(e => LastBit(e, 1)).ToArray()); // байт размера длины
            bool[] lenBool = txt.Skip(16).Take(sizeLen * 8).SelectMany(e => LastBit(e, 1)).ToArray(); // массив из булевых элементов длины скрытого текста (каждый элемент в отдельном байте)
            var lenBytes = new List<byte>();
            for (int i = 0; i < lenBool.Length; i += 8)
                lenBytes.Add(BoolArrByte(lenBool.Skip(i).Take(8).ToArray())); // список байтов длины (использовуется список, т.к. длина текста может быть разной), добавляем элементы путем прохода по 8 битам, а затем берем следующие восемь
            bool[] textBool = txt.Skip(16 + sizeLen * 8).Take(IntBytes(lenBytes) * 8).SelectMany(x => LastBit(x, 1)).ToArray(); // массив из булевых элементов скрытого текста (каждый элемент в отдельном байте)
            var textBytes = new List<byte>();
            for (int i = 0; i < textBool.Length; i += 8)
                textBytes.Add(BoolArrByte(textBool.Skip(i).Take(8).ToArray())); // список байтов текста, добавляем элементы путем прохода по 8 битам, а затем берем следующие восемь
            return ByteArrString(textBytes.ToArray());
        }

        // функция для перевода байтового списка в целочисленнное значение
        public static int IntBytes(List<byte> lenBytes)
        {
            int ans = 0;
            for (int i = 0; i < lenBytes.Count; i++)
                ans |= lenBytes[i] << i * 8; // побайтово увеличиваем целочисленное значение
            return ans;
        }
    }
}