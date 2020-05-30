using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using steganography.Functions;

namespace steganography.Methods
{
    class KochZhao // метод, суть которого состоит в относительной замене величин коэффициентов дискретного косинусного преобразования (каждый блок - сегмент - предназначен для скрытия одного бита данных) 
    {
        static Point p1 = new Point(6, 3); // точки для работы с сегментами коэффициентов ДКП
        static Point p2 = new Point(3, 6); // точки для работы с сегментами коэффициентов ДКП\
        private const int SizeSegment = 8; // размер сегмента

        // функция сокрытия текста в изображении
        public static StegoBitmap Hide(StegoBitmap stgbmap, string txt, Colours colour, int CoefDif)
        {
            var bmap = stgbmap.GetImage();
            if ((bmap.Width % SizeSegment) != 0 || (bmap.Height % SizeSegment) != 0) //если изображение не делится на сегменты ровно 
                CommonFunc.Cut(ref bmap, SizeSegment);
            
            var arrForHiding = new byte[bmap.Width, bmap.Height]; // массив байтов, где будет хранится текст в соотвествии с выбранным цветом
            //выбираем из изображения пиксели в соответствии с переданным цветом
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    if (colour == Colours.Red)
                        arrForHiding[i, j] = bmap.GetPixel(i, j).R;
                    else if (colour == Colours.Green)
                        arrForHiding[i, j] = bmap.GetPixel(i, j).G;
                    else if (colour == Colours.Blue)
                        arrForHiding[i, j] = bmap.GetPixel(i, j).B;
                    else
                        throw new NullReferenceException();
                }
            }
            int numSegm = bmap.Width * bmap.Height / (SizeSegment * SizeSegment); //общее число сегментов
            var stgByte = Encoding.GetEncoding(1251).GetBytes(txt); // перевод строки в массив байтов
            byte[] len = CommonFunc.LenInBytes(txt.Length, CommonFunc.Size(txt.Length)); // массив байтов из длины текста в соответвии с размером его длины            
            byte[] txtByte = new byte[2 + len.Length + stgByte.Length]; //массив для скрытого текста размером в длину текста + размер длины + два дополнительных байта
            txtByte[0] = Convert.ToByte('Z'); // первый элемент массива содержит пометку, что в изображении скрыт текст 
            txtByte[1] = CommonFunc.Size(txt.Length); // второй элемент массива размер длины
            int ind = 0;
            for (int i = 0; i < len.Length; i++)
                txtByte[i + 2] = len[ind++]; // сохраняем в массив байты из длины текста в соответвии с размером его длины   
            ind = 0;
            for (int i = 0; i < stgByte.Length; i++)
                txtByte[i + 2 + len.Length] = stgByte[ind++]; // сохраняем в массив скрываемый текст 
            var segm = new List<byte[,]>();
            Separate(arrForHiding, segm, bmap.Width, bmap.Height, SizeSegment);// разбиваем массив на сегменты
            // дискретное косинусное преобразование
            var dctList = new List<double[,]>();
            foreach (var b in segm)
                dctList.Add(DCT(b)); // список из коэффициентов ДКП
            SetText(txtByte, ref dctList, CoefDif); // внедрение текста 
            // обратное дискретное косинусное преобразование
            var idctList = new List<double[,]>();
            foreach (var d in dctList)
                idctList.Add(IDCT(d));
            var newArr = new double[bmap.Width, bmap.Height]; // новый массив значений
            Join(ref newArr, idctList, bmap.Width, bmap.Height, SizeSegment); //соединяем сегменты
            Normalize(ref newArr); // модификация коэффициентов ДКП иногда приводит к выходу значений интенсивностей пикселей изображения за пределы допустимого диапазона [0,255], проводим нормирование указанных значений
            return new StegoBitmap(bmap, newArr, colour); 
        }



        // разбиение массива на сегменты
        private static void Separate(byte[,] arr, List<byte[,]> segmList, int width, int height, int sizeSegm)
        {
            int numSW = width / sizeSegm; // количество сегментов по горизонтали
            int numSH = height / sizeSegm; // количество сегментов по вертикали
            for (int i = 0; i < numSW; i++)
            {
                int firstWPoint = i * sizeSegm; //начало каждого сегмента по горизонтали
                int lastWPoint = firstWPoint + sizeSegm - 1; //конец каждого сегмента по горизонтали
                for (int j = 0; j < numSH; j++)
                {
                    int firstHPoint = j * sizeSegm; //начало каждого сегмента по вертикали
                    int lastHPoint = firstHPoint + sizeSegm - 1; //конец каждого сегмента по вертикали
                    segmList.Add(SegmBytes(arr, firstWPoint, lastWPoint, firstHPoint, lastHPoint)); // добавляем в список элементы относительно сегментов
                }
            }
        }
        
        // получение элементов сегмента
        private static byte[,] SegmBytes(byte[,] arr, int a, int b, int c, int d)
        {
            var sg = new byte[b - a + 1, d - c + 1];
            for (int i = a, x = 0; i <= b; i++, x++)
                for (int j = c, y = 0; j <= d; j++, y++)
                    sg[x, y] = arr[i, j];
            return sg;
        }

        // определение значений коэффициентов для текущего значения аргументов
        private static double GetCoefficient(int arg)
        {
            if (arg == 0)
                return 1.0 / Math.Sqrt(2);
            return 1;
        }
       
        // прямое дискретное косинусное преобразование
        private static double[,] DCT(byte[,] b)
        {
            int len = b.GetLength(0); // получаем размер сегмента
            double[,] arrDCT = new double[len, len]; // новый массив после дискретного косинусного преобразования
            double temp = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    temp = 0;
                    for (int x = 0; x < len; x++)
                    {
                        for (int y = 0; y < len; y++)
                            temp += b[x, y] * Math.Cos(Math.PI * i * (2 * x + 1) / (2 * len)) * Math.Cos(Math.PI * j * (2 * y + 1) / (2 * len)); // вычисление спектральных коэффициентов ДКП для каждого сегмента (без умножения на значения коэффициентов для текущего значения аргумента) 
                    }
                    arrDCT[i, j] = GetCoefficient(j) * GetCoefficient(i) * temp / Math.Sqrt(2 * len); // вычисление спектральных коэффициентов ДКП для каждого сегмента (домножаем на значения коэффициентов для текущего значения аргумента)
                }
            }
            return arrDCT;
        }

        // внедрение текста
        private static void SetText(byte[] txt, ref List<double[,]> DCT, int coefDif)
        {
            List<int> freePos = new List<int>(); // свободные позиции в соответствии с размером списка коэффициентов ДКП
            for (int i = 0; i < DCT.Count; i++)
                freePos.Add(i);
            for (int i = 0; i < txt.Length; i++)
            {
                bool[] bitsSymb = CommonFunc.ByteBoolArr(txt[i]); //перевод байтого символа текста в булевый массив
                for (int j = 0; j < 8; j++)
                {
                    bool currentBit = bitsSymb[j];
                    int pos = freePos[0]; // позиция
                    freePos.RemoveAt(0);
                    // берем значения коэффициентов ДКП по модулю
                    double AbsP1 = Math.Abs(DCT[pos][p1.X, p1.Y]);
                    double AbsP2 = Math.Abs(DCT[pos][p2.X, p2.Y]);
                    int z1 = 1, z2 = 1; // переменные для сохранения знака первичных значений коэффициентов ДКП по модулю
                    if (DCT[pos][p1.X, p1.Y] < 0)
                        z1 = -1;
                    if (DCT[pos][p2.X, p2.Y] < 0)
                        z2 = -1;
                    if (currentBit) //для передачи бита "1" стремяться, чтобы разница абсолютных значений коэффициентов ДКП была меньше по сравнению с некоторой отрицательной величиной
                    {
                        if (AbsP1 - AbsP2 >= -coefDif)
                            AbsP2 = coefDif + AbsP1 + 1;
                    }
                   else //для передачи бита "0" стремятся, чтобы разница абсолютных значений коэффициентов ДКП превышала некоторую положительную величину
                    {
                        if (AbsP1 - AbsP2 <= coefDif)
                            AbsP1 = coefDif + AbsP2 + 1;
                    }
                    // присваиваем коэффициентам ДКП новые значения
                    DCT[pos][p1.X, p1.Y] = z1 * AbsP1;
                    DCT[pos][p2.X, p2.Y] = z2 * AbsP2;
                }
            }
        }
                
        // обратное дискретное косинусное преобразование
        private static double[,] IDCT(double[,] dct)
        {
            int len = dct.GetLength(0); // получаем размер сегмента ДКП
            double[,] result = new double[len, len]; // новый массив после обратного дискретного косинусного преобразования
            double temp = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    temp = 0;
                    for (int x = 0; x < len; x++)
                    {
                        for (int y = 0; y < len; y++)
                            temp += GetCoefficient(x) * GetCoefficient(y) * dct[x, y] * Math.Cos(Math.PI * x * (2 * i + 1) / (2 * len)) * Math.Cos(Math.PI * y * (2 * j + 1) / (2 * len));
                    }
                    result[i, j] = temp / (Math.Sqrt(2 * len));
                }
            }
            return result;
        }

        // соединяем сегменты
        private static void Join(ref double[,] arr, List<double[,]> Idct, int width, int height, int sizeSegm)
        {
            var temp = Idct.ToArray();
            int numSW = width / sizeSegm; // количество сегментов по горизонтали
            int numSH = height / sizeSegm; // количество сегментов по вертикали
            int k = 0;
            for (int i = 0; i < numSW; i++)
            {
                int firstWPoint = i * sizeSegm; //начало каждого сегмента по горизонтали
                int lastWPoint = firstWPoint + sizeSegm - 1; //конец каждого сегмента по горизонтали
                for (int j = 0; j < numSH; j++)
                {
                    int firstHPoint = j * sizeSegm;//начало каждого сегмента по вертикали
                    int lastHPoint = firstHPoint + sizeSegm - 1;//конец каждого сегмента по вертикали
                    Insert(ref arr, temp[k], firstWPoint, lastWPoint, firstHPoint, lastHPoint);
                    k++;
                }
            }
        }

        // вставка сегмент в массив
        private static void Insert(ref double[,] arr, double[,] temp, int firstWPoint, int lastWPoint, int firstHPoint, int lastHPoint)
        {            
            for (int i = firstWPoint, u = 0; i < lastWPoint + 1; i++, u++)
            {               
                for (int j = firstHPoint, v = 0; j < lastHPoint + 1; j++, v++)
                    arr[i, j] = temp[u, v];     
            }
        }

        // нормировка
        private static void Normalize(ref double[,] Idct)
        {
            double min = double.MaxValue, max = double.MinValue;
            for (int i = 0; i < Idct.GetLength(0); i++)
            {
                for (int j = 0; j < Idct.GetLength(1); j++)
                {
                    if (Idct[i, j] > max)
                        max = Idct[i, j]; // находим максиальный элемент
                    if (Idct[i, j] < min)
                        min = Idct[i, j];// находим минимальный элемент
                }
            }
            for (int i = 0; i < Idct.GetLength(0); i++)
            {
                for (int j = 0; j < Idct.GetLength(1); j++)
                    Idct[i, j] = 255 * (Idct[i, j] + Math.Abs(min)) / (max + Math.Abs(min)); // записываем результат нормировки в массив
            }
        }

        // проверка на наличие скрытого текста
        public static bool IsHiddenText(StegoBitmap stgbmap, Colours c)
        {
            var bmap = stgbmap.GetImage();
            int width = bmap.Width;
            int height = bmap.Height;
            var arrWhereHide = new byte[bmap.Width, bmap.Height]; // массив байтов, где будет хранится текст в соотвествии с выбранным цветом
            //выбираем из изображения пиксели в соответствии с переданным цветом
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    if (c == Colours.Red)
                        arrWhereHide[i, j] = bmap.GetPixel(i, j).R;
                    else if (c == Colours.Green)
                        arrWhereHide[i, j] = bmap.GetPixel(i, j).G;
                    else if (c == Colours.Blue)
                        arrWhereHide[i, j] = bmap.GetPixel(i, j).B;
                    else
                        throw new NullReferenceException();
                }
            }
            int numSegm = bmap.Width * bmap.Height / (SizeSegment * SizeSegment); //общее число сегментов
            var segm = new List<byte[,]>();
            Separate(arrWhereHide, segm, bmap.Width, bmap.Height, SizeSegment);// разбиваем массив на сегменты
            // дискретное косинусное преобразование
            var dctList = new List<double[,]>();
            foreach (var b in segm)
                dctList.Add(DCT(b)); // список из коэффициентов ДКП
            var txtByte = new List<byte>();
            List<int> possibPos = new List<int>(); // возможные позиции в соответствии с размером списка коэффициентов ДКП
            for (int i = 0; i < dctList.Count; i++)
                possibPos.Add(i);
            var bits = new bool[8]; // булевый массив битов символа
            for (int j = 0; j < 8; j++)
            {
                    int pos = possibPos[0]; // позиция 
                    possibPos.RemoveAt(0);
                    double AbsPoint1 = Math.Abs(dctList[pos][p1.X, p1.Y]);
                    double AbsPoint2 = Math.Abs(dctList[pos][p2.X, p2.Y]);
                    // бит выделяем в соответсвии с тем, какое абслютное значение больше
                    if (AbsPoint1 > AbsPoint2)
                        bits[j] = false;
                    else if (AbsPoint1 < AbsPoint2)
                        bits[j] = true;
            }
            txtByte.Add(CommonFunc.BoolArrByte(bits));
            if (txtByte.ToArray()[0] != Convert.ToByte('Z')) // в случае отсутвия метки возвращаем пустую строку
                return false;
            return true;
        }

        // функция находит скрытый текст
        public static string GetHiddenText(StegoBitmap stgbmap, Colours c)
        {
            var bmap = stgbmap.GetImage();
            int width = bmap.Width;
            int height = bmap.Height;
            var arrWhereHide = new byte[bmap.Width, bmap.Height]; // массив байтов, где будет хранится текст в соотвествии с выбранным цветом
            //выбираем из изображения пиксели в соответствии с переданным цветом
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    if (c == Colours.Red)
                        arrWhereHide[i, j] = bmap.GetPixel(i, j).R;
                    else if (c == Colours.Green)
                        arrWhereHide[i, j] = bmap.GetPixel(i, j).G;
                    else if (c == Colours.Blue)
                        arrWhereHide[i, j] = bmap.GetPixel(i, j).B;
                    else
                        throw new NullReferenceException();
                }
            }
            int numSegm = bmap.Width * bmap.Height / (SizeSegment * SizeSegment); //общее число сегментов
            var segm = new List<byte[,]>();
            Separate(arrWhereHide, segm, bmap.Width, bmap.Height, SizeSegment);// разбиваем массив на сегменты
            // дискретное косинусное преобразование
            var dctList = new List<double[,]>();
            foreach (var b in segm)
                dctList.Add(DCT(b)); // список из коэффициентов ДКП
            var txtByte = new List<byte>();  
            List<int> possibPos = new List<int>(); // возможные позиции в соответствии с размером списка коэффициентов ДКП
            for (int i = 0; i < dctList.Count; i++)
                possibPos.Add(i);
            int end = 2; //конец прохода
            bool LenDone = true;
            for (int i = 0; i < end; i++)
                {
                    var bits = new bool[8]; // булевый массив битов символа
                    for (int j = 0; j < 8; j++)
                    {
                        int pos = possibPos[0]; // позиция 
                        possibPos.RemoveAt(0);
                        double AbsPoint1 = Math.Abs(dctList[pos][p1.X, p1.Y]);
                        double AbsPoint2 = Math.Abs(dctList[pos][p2.X, p2.Y]);
                        // бит выделяем в соответсвии с тем, какое абслютное значение больше
                        if (AbsPoint1 > AbsPoint2)
                            bits[j] = false;
                        else if (AbsPoint1 < AbsPoint2)
                            bits[j] = true;
                    }
                    txtByte.Add(CommonFunc.BoolArrByte(bits));
                    if (i == 0 && txtByte.ToArray()[0] != Convert.ToByte('Z')) // в случае отсутвия метки возвращаем пустую строку
                        return "";
                    else if (i == 1) //увеличиваем конец прохода, узнав размер длины
                    {
                        end = txtByte.ToArray()[1] + 2;
                        txtByte.Clear();
                    }
                    else if (LenDone && i+1 == end) //увеличиваем конец прохода, узнав длину
                    {
                        end += CommonFunc.IntBytes(txtByte);
                        txtByte.Clear();
                        LenDone = false;
                    }
                }     
            return Encoding.GetEncoding(1251).GetString(txtByte.ToArray()); //преобразуем массив 
        }
    }
}