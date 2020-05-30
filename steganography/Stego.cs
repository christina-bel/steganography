using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using steganography.Functions; 

namespace steganography.Methods 
{
    public class LSB
    {
        //статическая функция для замены НЗБ (внедрения текста в изображение путем замены битов)
        public static StegoBitmap Hide(StegoBitmap bitmap, string text, Colours colour)
        {
            byte[] sourceColour = bitmap.GetColour(colour); // массив байтов указанного цвета для данного StegoBitmap
            byte[] stgText = Encoding.GetEncoding(1251).GetBytes(text); // массив байтов введеных символов
            byte[] len = CommonFunc.LenInBytes(text.Length, CommonFunc.Size(text.Length)); // массив байтов из длины текста в соответвии с размером его длины            
            byte[] stgBytes = new byte[2 + len.Length + stgText.Length]; //массив для скрытого текста размером в длину текста + размер длины + два дополнительных байта
            stgBytes[0] = Convert.ToByte('L'); // первый элемент массива содержит пометку, что в изображении скрыт текст 
            stgBytes[1] = CommonFunc.Size(text.Length); // второй элемент массива размер длины
            int ind = 0;
            for (int i = 0; i < len.Length; i++)
                stgBytes[i + 2] = len[ind++]; // сохраняем в массив байты из длины текста в соответвии с размером его длины   
            ind = 0;
            for (int i = 0; i < stgText.Length; i++)
                stgBytes[i + 2 + len.Length] = stgText[ind++]; // сохраняем в массив скрываемый текст   
            // булевский массив из массива байтов
            bool[] stgBoolBits = stgBytes.SelectMany(e => CommonFunc.ByteBoolArr(e)).ToArray(); // SelectMany используется для создания выходной последовательности с проекцией "один ко многим" из входной последовательности. Select вернет один выходной элемент для каждого входного элемента, SelectMany - ноль или более выходных элементов для каждого входного
            // проходим по элементам булевского массива текста. если бит текста true и байт цвета четен, то увеличиваем его на 1, в обратной ситуации - уменьшаем на 1, в остальный случаях байт цвета остается не изменным  (0 и 0 => 0, 1 и 1 => 1, 0 и 1 => 1, 1 и 0 => 0)  
            for (int i = 0; i < stgBoolBits.Length; i++)
               if ((sourceColour[i] % 2 == 0) && stgBoolBits[i]) 
                    sourceColour[i]++;
                else if ((sourceColour[i] % 2 == 1) && !stgBoolBits[i]) 
                    sourceColour[i]--;
            return new StegoBitmap(bitmap, sourceColour, colour);
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
            return CommonFunc.BoolArrByte(arr) == Convert.ToByte('L'); //переводим булев массив в байты и проверяем совпадение с байтовым представлением метки 
        }

        // функция превращает последний бит байта в булевую переменную
        private static bool[] LastBit(byte b, int num)
        {
            var arr = CommonFunc.ByteBoolArr(b); // переводим массив байтов в булев
            return arr.Skip(8 - num).Take(num).ToArray();  // берем только последний элемент массива (отвечающий за последний бит)
        }

        // функция возвращает скрытый текст
        public static string GetHiddenText(StegoBitmap bitmap, Colours colour)
        {
            byte[] txt = bitmap.GetColour(colour); // массив байтов указанного цвета для данного StegoBitmap
            byte mark = CommonFunc.BoolArrByte(txt.Take(8).SelectMany(e => LastBit(e, 1)).ToArray()); // байт метки
            byte sizeLen = CommonFunc.BoolArrByte(txt.Skip(8).Take(8).SelectMany(e => LastBit(e, 1)).ToArray()); // байт размера длины
            bool[] lenBool = txt.Skip(16).Take(sizeLen * 8).SelectMany(e => LastBit(e, 1)).ToArray(); // массив из булевых элементов длины скрытого текста (каждый элемент в отдельном байте)
            var lenBytes = new List<byte>();
            for (int i = 0; i < lenBool.Length; i += 8)
                lenBytes.Add(CommonFunc.BoolArrByte(lenBool.Skip(i).Take(8).ToArray())); // список байтов длины (использовуется список, т.к. длина текста может быть разной), добавляем элементы путем прохода по 8 битам, а затем берем следующие восемь
            bool[] textBool = txt.Skip(16 + sizeLen * 8).Take(CommonFunc.IntBytes(lenBytes) * 8).SelectMany(x => LastBit(x, 1)).ToArray(); // массив из булевых элементов скрытого текста (каждый элемент в отдельном байте)
            var textBytes = new List<byte>();
            for (int i = 0; i < textBool.Length; i += 8)
                textBytes.Add(CommonFunc.BoolArrByte(textBool.Skip(i).Take(8).ToArray())); // список байтов текста, добавляем элементы путем прохода по 8 битам, а затем берем следующие восемь
            return Encoding.GetEncoding(1251).GetString(textBytes.ToArray());
        }

    }
}