namespace WindowsFormsApp1
{
    partial class AddForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.labelNameProd = new System.Windows.Forms.Label();
            this.labelProiz = new System.Windows.Forms.Label();
            this.labelCat = new System.Windows.Forms.Label();
            this.labelPodcat = new System.Windows.Forms.Label();
            this.labelOpis = new System.Windows.Forms.Label();
            this.labelSostav = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(185, 145);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(178)))), ((int)(((byte)(175)))));
            this.button1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(610, 418);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 31);
            this.button1.TabIndex = 8;
            this.button1.Text = "Назад";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(178)))), ((int)(((byte)(175)))));
            this.button2.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(308, 418);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 31);
            this.button2.TabIndex = 9;
            this.button2.Text = "Добавить в корзинку";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelNameProd
            // 
            this.labelNameProd.AutoSize = true;
            this.labelNameProd.Location = new System.Drawing.Point(203, 23);
            this.labelNameProd.Name = "labelNameProd";
            this.labelNameProd.Size = new System.Drawing.Size(92, 15);
            this.labelNameProd.TabIndex = 10;
            this.labelNameProd.Text = "labelNameProd";
            // 
            // labelProiz
            // 
            this.labelProiz.AutoSize = true;
            this.labelProiz.Location = new System.Drawing.Point(203, 68);
            this.labelProiz.Name = "labelProiz";
            this.labelProiz.Size = new System.Drawing.Size(53, 15);
            this.labelProiz.TabIndex = 11;
            this.labelProiz.Text = "labelCat";
            // 
            // labelCat
            // 
            this.labelCat.AutoSize = true;
            this.labelCat.Location = new System.Drawing.Point(203, 115);
            this.labelCat.Name = "labelCat";
            this.labelCat.Size = new System.Drawing.Size(42, 15);
            this.labelCat.TabIndex = 12;
            this.labelCat.Text = "label3";
            // 
            // labelPodcat
            // 
            this.labelPodcat.AutoSize = true;
            this.labelPodcat.Location = new System.Drawing.Point(203, 166);
            this.labelPodcat.Name = "labelPodcat";
            this.labelPodcat.Size = new System.Drawing.Size(42, 15);
            this.labelPodcat.TabIndex = 13;
            this.labelPodcat.Text = "label4";
            // 
            // labelOpis
            // 
            this.labelOpis.Location = new System.Drawing.Point(203, 199);
            this.labelOpis.Name = "labelOpis";
            this.labelOpis.Size = new System.Drawing.Size(299, 60);
            this.labelOpis.TabIndex = 14;
            this.labelOpis.Text = "label5";
            // 
            // labelSostav
            // 
            this.labelSostav.Location = new System.Drawing.Point(203, 270);
            this.labelSostav.Name = "labelSostav";
            this.labelSostav.Size = new System.Drawing.Size(302, 85);
            this.labelSostav.TabIndex = 15;
            this.labelSostav.Text = "label1";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(203, 355);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(42, 15);
            this.labelPrice.TabIndex = 16;
            this.labelPrice.Text = "label4";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(308, 374);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(461, 374);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(44, 23);
            this.numericUpDown1.TabIndex = 18;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Выберите размер";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(511, 377);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "шт.";
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(442, 356);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(106, 15);
            this.labelWarning.TabIndex = 21;
            this.labelWarning.Text = "Выберите размер";
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(718, 461);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelSostav);
            this.Controls.Add(this.labelOpis);
            this.Controls.Add(this.labelPodcat);
            this.Controls.Add(this.labelCat);
            this.Controls.Add(this.labelProiz);
            this.Controls.Add(this.labelNameProd);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddForm";
            this.Text = "ООО \"Обувь\" \\ Просмотр товара";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelNameProd;
        private System.Windows.Forms.Label labelProiz;
        private System.Windows.Forms.Label labelCat;
        private System.Windows.Forms.Label labelPodcat;
        private System.Windows.Forms.Label labelOpis;
        private System.Windows.Forms.Label labelSostav;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelWarning;
    }
}