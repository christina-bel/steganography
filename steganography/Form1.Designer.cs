namespace steganography
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.methodButton1 = new System.Windows.Forms.RadioButton();
            this.methodButton2 = new System.Windows.Forms.RadioButton();
            this.methodButton3 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ColourBox1 = new System.Windows.Forms.ComboBox();
            this.labelAttention = new System.Windows.Forms.Label();
            this.ColourBox2 = new System.Windows.Forms.ComboBox();
            this.CoefDiffBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ColourBox3 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(614, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "OPEN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(614, 206);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "SAVE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(614, 249);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 37);
            this.button3.TabIndex = 2;
            this.button3.Text = "ENCODE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(614, 292);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 36);
            this.button4.TabIndex = 3;
            this.button4.Text = "DECODE";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(279, 323);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(302, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(279, 323);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(302, 331);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(279, 99);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 331);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Размер файла (в пикселях):";
            // 
            // methodButton1
            // 
            this.methodButton1.AutoSize = true;
            this.methodButton1.Location = new System.Drawing.Point(584, 5);
            this.methodButton1.Name = "methodButton1";
            this.methodButton1.Size = new System.Drawing.Size(80, 17);
            this.methodButton1.TabIndex = 8;
            this.methodButton1.TabStop = true;
            this.methodButton1.Text = "Метод LSB";
            this.methodButton1.UseVisualStyleBackColor = true;
            this.methodButton1.CheckedChanged += new System.EventHandler(this.methodButton1_CheckedChanged);
            // 
            // methodButton2
            // 
            this.methodButton2.AutoSize = true;
            this.methodButton2.Location = new System.Drawing.Point(584, 55);
            this.methodButton2.Name = "methodButton2";
            this.methodButton2.Size = new System.Drawing.Size(110, 17);
            this.methodButton2.TabIndex = 9;
            this.methodButton2.TabStop = true;
            this.methodButton2.Text = "Метод Коха-Жао";
            this.methodButton2.UseVisualStyleBackColor = true;
            this.methodButton2.CheckedChanged += new System.EventHandler(this.methodButton2_CheckedChanged);
            // 
            // methodButton3
            // 
            this.methodButton3.AutoSize = true;
            this.methodButton3.Location = new System.Drawing.Point(584, 133);
            this.methodButton3.Name = "methodButton3";
            this.methodButton3.Size = new System.Drawing.Size(104, 17);
            this.methodButton3.TabIndex = 10;
            this.methodButton3.TabStop = true;
            this.methodButton3.Text = "Метод Бенгама";
            this.methodButton3.UseVisualStyleBackColor = true;
            this.methodButton3.CheckedChanged += new System.EventHandler(this.methodButton3_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 331);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 354);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Максимальное число символов:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(181, 354);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 377);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Введено символов:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 377);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 400);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Осталось символов:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(181, 400);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 17;
            // 
            // ColourBox1
            // 
            this.ColourBox1.FormattingEnabled = true;
            this.ColourBox1.Items.AddRange(new object[] {
            "Красный",
            "Зеленый",
            "Синий"});
            this.ColourBox1.Location = new System.Drawing.Point(584, 28);
            this.ColourBox1.Name = "ColourBox1";
            this.ColourBox1.Size = new System.Drawing.Size(178, 21);
            this.ColourBox1.TabIndex = 18;
            // 
            // labelAttention
            // 
            this.labelAttention.Location = new System.Drawing.Point(581, 340);
            this.labelAttention.Name = "labelAttention";
            this.labelAttention.Size = new System.Drawing.Size(181, 99);
            this.labelAttention.TabIndex = 19;
            this.labelAttention.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColourBox2
            // 
            this.ColourBox2.FormattingEnabled = true;
            this.ColourBox2.Items.AddRange(new object[] {
            "Красный",
            "Зеленый",
            "Синий"});
            this.ColourBox2.Location = new System.Drawing.Point(584, 78);
            this.ColourBox2.Name = "ColourBox2";
            this.ColourBox2.Size = new System.Drawing.Size(178, 21);
            this.ColourBox2.TabIndex = 20;
            // 
            // CoefDiffBox
            // 
            this.CoefDiffBox.FormattingEnabled = true;
            this.CoefDiffBox.Items.AddRange(new object[] {
            "25",
            "40",
            "55",
            "70",
            "85",
            "100"});
            this.CoefDiffBox.Location = new System.Drawing.Point(684, 106);
            this.CoefDiffBox.Name = "CoefDiffBox";
            this.CoefDiffBox.Size = new System.Drawing.Size(78, 21);
            this.CoefDiffBox.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(584, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 28);
            this.label9.TabIndex = 23;
            this.label9.Text = "Разность коэффициентов:";
            // 
            // ColourBox3
            // 
            this.ColourBox3.FormattingEnabled = true;
            this.ColourBox3.Items.AddRange(new object[] {
            "Красный",
            "Зеленый",
            "Синий"});
            this.ColourBox3.Location = new System.Drawing.Point(684, 133);
            this.ColourBox3.Name = "ColourBox3";
            this.ColourBox3.Size = new System.Drawing.Size(78, 21);
            this.ColourBox3.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 449);
            this.Controls.Add(this.ColourBox3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.CoefDiffBox);
            this.Controls.Add(this.ColourBox2);
            this.Controls.Add(this.labelAttention);
            this.Controls.Add(this.ColourBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.methodButton3);
            this.Controls.Add(this.methodButton2);
            this.Controls.Add(this.methodButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton methodButton1;
        private System.Windows.Forms.RadioButton methodButton2;
        private System.Windows.Forms.RadioButton methodButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ColourBox1;
        private System.Windows.Forms.Label labelAttention;
        private System.Windows.Forms.ComboBox ColourBox2;
        private System.Windows.Forms.ComboBox CoefDiffBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ColourBox3;
    }
}

