using System;
using System.Drawing;
using System.Windows.Forms;
using steganography.Methods;

namespace steganography
{
    public partial class Form1 : Form
    {
        StegoBitmap bMap; // для бит исходного изображения
        StegoBitmap bMapSteg; // для бит нового изображения

        public Form1()
        {
            InitializeComponent();
        }

        // кнопка 'OPEN'
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // выбираем рисунок, в котором будет хранится информация
                var open_dialog = new OpenFileDialog();
                open_dialog.Title = "Открыть изображение";
                // зададим фильтр файлов, чтобы в диалоговом окне можно было их отфильтровать по расширению. Фильтр задается в формате: Название_файлов|*.расширение. 
                open_dialog.Filter = "JPEG файл(*.jpg)|*.jpg|" + "Bitmap файл(*.bmp)|*.bmp|" + "GIF файл(*.gif)|*.gif|" + "PNG файл(*.png)|*.png|" + "TIF файл(*.tif)|*.tif";
                // показать диалоговое окно и определить, совпадает ли возвращемое значение с ОК
                open_dialog.CheckFileExists = true;// проверяет существование файла с указанным именем
                open_dialog.CheckPathExists = true;// проверяет существование пути к файлу с указанным именем
                if (open_dialog.ShowDialog() == DialogResult.OK)
                {
                    if (bMap != null) // если поле с исходным изображением уже занято, то освободить все ресурсы
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                        bMap = null;
                    }
                    if (bMapSteg != null) // если поле с измененным изображением уже занято, то освободить все ресурсы
                    {
                        pictureBox2.Image.Dispose();
                        pictureBox2.Image = null;
                        bMapSteg = null;
                    }
                    textBox1.Text = labelAttention.Text = ""; // освобждаем поля с вводимым текстом и предупреждениями
                    bMap = new StegoBitmap(new Bitmap(open_dialog.FileName)); // создаем новый экземпляр класса StegoBitmap для работы с изображением, путем инициализации из указанного файла нового экмземпляра Bitmap
                    pictureBox1.Image = bMap.GetImage(); // задаем изображение в  pictureBox1
                    label2.Text = bMap.FileSize; // размер изображения
                    label4.Text = label8.Text = "зависит от метода"; // максимальное возможное число вводимых символов и число оставшихся символов 
                    label6.Text = "0"; // число введенных символов
                    methodButton1.Checked = methodButton2.Checked = methodButton3.Checked = false; // никакой метод не выбран
                    ColourBox1.SelectedIndex = ColourBox2.SelectedIndex = ColourBox3.SelectedIndex = CoefDiffBox.SelectedIndex = -1; //обнуляем все combobox
                }
                else
                    labelAttention.Text = "Отменено";  // если работа диалогового окна была приостановлена
            }
            catch (Exception)
            {
                labelAttention.Text = "Что-то пошло не так при открытии файла, попробуйте позже"; //в случае какой-то неполадки
            }

        }

        // кнопка 'SAVE'
        private void button2_Click(object sender, EventArgs e)
        {
            if (bMapSteg == null)
                labelAttention.Text = "Изображение отсутсвует";
            else
            {
                try
                {
                    // сохраняем рисунок с внедренной информацией
                    var save_dialog = new SaveFileDialog();
                    save_dialog.Title = "Сохранить изображение";
                    save_dialog.OverwritePrompt = true; //если указан существующий файл, то отображается сообщение о том, что файл будет перезаписан
                    save_dialog.CheckPathExists = true;// проверяет существование пути к файлу с указанным именем
                    save_dialog.Filter = "Bitmap файл(*.bmp)|*.bmp|" + "JPEG файл(*.jpg)|*.jpg|" + "GIF файл(*.gif)|*.gif|" + "PNG файл(*.png)|*.png|" + "TIF файл(*.tif)|*.tif";
                    // показать диалоговое окно и определить, совпадает ли возвращемое значение с ОК
                    if (save_dialog.ShowDialog() == DialogResult.OK)
                    {
                        bMapSteg.SaveBitmap(save_dialog.FileName); // сохраняем изображение передавая ему имя файла, выбранного пользователем
                        labelAttention.Text = "Изображение сохранено! Для продолжения работы загрузите новое изображение или продолжите работу с данным."; // работа диалогового окна выполнена успешно
                    }
                    else
                        labelAttention.Text = "Отменено"; // если работа диалогового окна была приостановлена
                }
                catch (Exception)
                {
                    labelAttention.Text = "Что-то пошло не так при сохранении, попробуйте позже"; //в случае какой-то неполадки
                }
            }
        }
                          
        // кнопка 'ENCODE' 
        private void button3_Click(object sender, EventArgs e)
        {
            if (methodButton1.Checked) //если выбран первый метод
                Method1_LSB();
            else if (methodButton2.Checked) //если выбран второй метод
                Method2_KochZhao();
            else if (methodButton3.Checked) //если выбран третий метод
                Method3_Benham();
            else
                labelAttention.Text = "Метод не выбран";
        }

        // кнопка 'DECODE'
        private void button4_Click(object sender, EventArgs e)
        {
            labelAttention.Text = "";
            if (bMap == null)
                labelAttention.Text = "Вставьте изображение!"; // проверка на наличие изображения
            else
            {
                if (methodButton1.Checked) // проверка по методу LSB
                {
                    try
                    {
                        if (LSB.IsHiddenText(bMap, out Colours c))
                        {
                            textBox1.Text = LSB.GetHiddenText(bMap, c); // получаем и выводим скрытый текст
                            labelAttention.Text = "Скрытый текст найден";
                        }
                        else
                            labelAttention.Text = "В изображении отсутствует скрытый текст по данному методу";// отсутствие скрытого текста
                    }
                    catch (Exception)
                    {
                        labelAttention.Text = "Что-то пошло не так, попробуйте позже"; //в случае какой-то неполадки
                    }
                }

                else if (methodButton2.Checked) // проверка по методу Коха-Жао
                {
                    try // проверка на выбор цвета
                    {
                        Colours colour = (Colours)ColourBox2.SelectedIndex; // цвет задаем в соответвии с выбранным в comboBox1, приводя к перечислению Colours
                        textBox1.Text = KochZhao.GetHiddenText(bMap, colour); // получаем и выводим скрытый текст
                        if (textBox1.Text!="")
                            labelAttention.Text = "Скрытый текст найден";
                        else
                            labelAttention.Text = "В изображении отсутствует скрытый текст по данному методу";// отсутствие скрытого текста
                    }
                    catch (NullReferenceException)
                    {
                        string message = "Проверка по методу Коха-Жао. Выберите цвет для выявления текста в разделе соответствующего метода";
                        string caption = "Отсутствие цвета";
                        MessageBoxButtons button = MessageBoxButtons.OK;
                        DialogResult diaRes = MessageBox.Show(message, caption, button); //  отобразить MessageBox                            
                    }
                    catch (Exception)
                    {
                        labelAttention.Text = "Что-то пошло не так, попробуйте позже"; //в случае какой-то неполадки
                    }

                }
                else if (methodButton3.Checked)  // проверка по методу Бенгама
                {
                    try // проверка на выбор цвета
                    {
                        Colours colour = (Colours)ColourBox3.SelectedIndex; // цвет задаем в соответвии с выбранным в comboBox3, приводя к перечислению Colour
                        textBox1.Text = Benham.GetHiddenText(bMap, colour); // получаем и выводим скрытый текст
                        if (textBox1.Text != "")
                            labelAttention.Text = "Скрытый текст найден";
                        else
                            labelAttention.Text = "В изображении отсутствует скрытый текст по данному методу";// отсутствие скрытого текста
                    }
                    catch (NullReferenceException)
                    {
                        string message = "Проверка по методу Бенгама. Выберите цвет для выявления текста в разделе соответствующего метода";
                        string caption = "Отсутствие цвета";
                        MessageBoxButtons button = MessageBoxButtons.OK;
                        DialogResult res = MessageBox.Show(message, caption, button); //  отобразить MessageBox
                    }
                    catch (Exception)
                    {
                        labelAttention.Text = "Что-то пошло не так, попробуйте позже"; //в случае какой-то неполадки
                    }
                }
                else
                    labelAttention.Text = "Не выбран метод для поиска текста в изображении";
            }              
        }

        //изменение поля textBox1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (bMap != null)
            {
                try
                {
                    if (textBox1.Text.Length > Convert.ToInt32(label4.Text))
                    {
                        textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                        // переходим в конец текста
                        textBox1.Select(textBox1.Text.Length, 0); // выбирает диапазон текста в текстовом поле.
                        textBox1.ScrollToCaret(); // прокручивает содержимое элемента управления до текущей позиции курсора
                    }
                    CalcLab(); // подсчет символов
                }
                catch (FormatException)
                {
                    labelAttention.Text = "Не выбран метод";
                }
            }
        }

        // функция для посчета символов
        public void CalcLab()
        {
            label6.Text = textBox1.Text.Length.ToString(); // число введенных символов
            if (!methodButton3.Checked)
                label8.Text = (Convert.ToInt32(label4.Text) - textBox1.Text.Length).ToString(); // число оставшихся возможных символов
            if (Convert.ToInt32(label4.Text) - textBox1.Text.Length == 0)
                labelAttention.Text = "Количество дозволенных символов закончено"; // проверка на окончание возможных символов
            else
                labelAttention.Text = "";
        }

        // функция вызова первого метода
        public void Method1_LSB()
        {
            labelAttention.Text = "";
            if (bMap == null)
                labelAttention.Text = "Вставьте изображение!"; // проверка на наличие изображения
            else
            {
                if (textBox1.Text != "") // проверка на наличие текста
                {
                   var result = LSB.IsHiddenText(bMap, out Colours c); // содержится ли скрытый текст
                   if (!result || Attention(result)) // если нет скрытого текста или пользователь все равно хочет заменить его на новый
                   {
                        try
                        {
                            Colours colour = (Colours)ColourBox1.SelectedIndex; // цвет задаем в соответвии с выбранным в comboBox1, приводя к перечислению Colours
                            bMapSteg = LSB.Hide(bMap, textBox1.Text, colour);// создаем новый объект StegoBitmap, хранящий в себе скрытый текст
                            pictureBox2.Image = bMapSteg.GetImage(); // задаем изображение в  pictureBox2
                        }
                        catch (NullReferenceException)
                        {
                            string message = "Не выбран цвет для сокрытия текста";
                            string caption = "Отсутствие цвета";
                            MessageBoxButtons button = MessageBoxButtons.OK;
                            DialogResult res = MessageBox.Show(message, caption, button); //  отобразить MessageBox                            
                        }
                        catch (Exception)
                        {
                            labelAttention.Text = "Что-то пошло не так, попробуйте позже"; //в случае какой-то неполадки
                        }
                    }
                }
                else
                    labelAttention.Text = "Введите текст!"; // при отсутствии текста
            }
        }

        // функция вызова второго метода
        public void Method2_KochZhao()
        {
            labelAttention.Text = "";
            if (bMap == null)
                labelAttention.Text = "Вставьте изображение!"; // проверка на наличие изображения
            else
            {
                if (textBox1.Text != "") // проверка на наличие текста
                {
                    try // проверка на выбор всех параметров
                    {
                        Colours colour = (Colours)ColourBox2.SelectedIndex; // цвет задаем в соответвии с выбранным в comboBox3, приводя к перечислению Colours
                        bool findText = KochZhao.IsHiddenText(bMap, colour); // проверка на наличие уже внедренного текста
                        if (!findText || Attention(findText))
                        {
                            bMapSteg = KochZhao.Hide(bMap, textBox1.Text, colour, Convert.ToInt32(CoefDiffBox.SelectedItem.ToString())); // создаем новый объект StegoBitmap, хранящий в себе скрытый текст
                            pictureBox2.Image = bMapSteg.GetImage(); // задаем изображение в  pictureBox2
                        }
                    }
                    catch (NullReferenceException)
                    {
                        string message = "Не выбраны все параметры для сокрытия текста";
                        string caption = "Отсутствие одного из параметров";
                        MessageBoxButtons button = MessageBoxButtons.OK;
                        DialogResult res = MessageBox.Show(message, caption, button); //  отобразить MessageBox
                    }
                    catch (Exception)
                    {
                        labelAttention.Text = "Что-то пошло не так, попробуйте позже"; //в случае какой-то неполадки
                    }
                }
                else
                    labelAttention.Text = "Введите текст!"; // при отсутствии текста
            }
        }

        // функция вызова третьего метода
        public void Method3_Benham()
        {
            labelAttention.Text = "";
            if (bMap == null)
                labelAttention.Text = "Вставьте изображение!"; // проверка на наличие изображения
            else
            {
                if (textBox1.Text != "") // проверка на наличие текста
                {
                    try // проверка на выбор всех параметров
                    {

                        Colours colour = (Colours)ColourBox3.SelectedIndex; // цвет задаем в соответвии с выбранным в comboBox3, приводя к перечислению Colours
                        var result = Benham.IsHiddenText(bMap, colour); // содержится ли скрытый текст
                        if (!result || Attention(result)) // если нет скрытого текста или пользователь все равно хочет заменить его на новый
                        {
                            bMapSteg = Benham.Hide(bMap, textBox1.Text, colour);// создаем новый объект StegoBitmap, хранящий в себе скрытый текст
                            pictureBox2.Image = bMapSteg.GetImage(); // задаем изображение в  pictureBox2
                        }
                    }
                    catch (NullReferenceException)
                    {
                        string message = "Не выбран цвет для сокрытия текста";
                        string caption = "Отсутствие одного из параметров";
                        MessageBoxButtons button = MessageBoxButtons.OK;
                        DialogResult res = MessageBox.Show(message, caption, button); //  отобразить MessageBox
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        labelAttention.Text = "Число символов больше чем число пригодных для внедрения блоков. Выберете другой цвет/метод или уменьшите размер текста";
                    }
                    catch (Exception)
                    {
                        labelAttention.Text = "Что-то пошло не так, попробуйте позже"; //в случае какой-то неполадки
                    }
                }
                else
                    labelAttention.Text = "Введите текст!"; // при отсутствии текста
            }
        }

        // функция предупреждающая о наличии скрытого текста
        public bool Attention(bool result)
        {
            if (result)
            {
                    string message = "Найден скрытый текст, внедренный данным метом и цветом. Переписать его на новый? Старый будет утерян.";
                    string caption = "Обнаружен скрытый текст";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult res = MessageBox.Show(message, caption, buttons); //  отобразить MessageBox
                    if (res == DialogResult.Yes) // в случае, если пользователь все-таки хочет заменить текст                  
                        return true;                   
                    labelAttention.Text = "Найден cкрытый текст. Нажмите кнопку DECODE, чтобы декодировать сообщение";
                    return false;
            }
            else
                return true;
        }

        private void methodButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (bMap != null)
            {
                if (methodButton1.Checked) //если выбран первый метод 
                {
                    label4.Text = bMap.GetMaxCapacityMethod1().ToString(); // максимальное возможное число вводимых символов и число оставшихся символов
                    label8.Text = (Convert.ToInt32(label4.Text) - textBox1.Text.Length).ToString(); // число оставшихся возможных символов
                    CalcLab();
                }
             }
        }

        private void methodButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (bMap != null)
            {
                if (methodButton2.Checked) //если выбран второй метод 
                {
                    label4.Text = label8.Text = bMap.GetMaxCapacityMethod2().ToString(); // максимальное возможное число вводимых символов и число оставшихся символов
                    label8.Text = (Convert.ToInt32(label4.Text) - textBox1.Text.Length).ToString(); // число оставшихся возможных символов
                    CalcLab();
                }
            }
        }

        private void methodButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (bMap != null)
            {
                if (methodButton3.Checked) //если выбран третий метод 
                {
                    label4.Text = bMap.GetMaxCapacityMethod2().ToString(); // максимальное возможное число вводимых символов 
                    label6.Text = textBox1.Text.Length.ToString(); // число введенных символов
                    label8.Text = "Нельзя предсказать\nчисло пригодных\nдля внедрения блоков"; // число оставшихся символов
                }
            }

        }
    }
}
