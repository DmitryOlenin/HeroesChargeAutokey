namespace HeroesChargeAutokey
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.cb_start = new System.Windows.Forms.CheckBox();
            this.cb_type = new System.Windows.Forms.ComboBox();
            this.cb_ratio = new System.Windows.Forms.ComboBox();
            this.lb_type = new System.Windows.Forms.Label();
            this.lb_ratio = new System.Windows.Forms.Label();
            this.b_test = new System.Windows.Forms.Button();
            this.chb_debug = new System.Windows.Forms.CheckBox();
            this.lb_current = new System.Windows.Forms.Label();
            this.chb_skip1 = new System.Windows.Forms.CheckBox();
            this.lb_skip = new System.Windows.Forms.Label();
            this.chb_skip2 = new System.Windows.Forms.CheckBox();
            this.chb_skip3 = new System.Windows.Forms.CheckBox();
            this.chb_skip4 = new System.Windows.Forms.CheckBox();
            this.chb_skip5 = new System.Windows.Forms.CheckBox();
            this.chb_skip6 = new System.Windows.Forms.CheckBox();
            this.lb_start = new System.Windows.Forms.Label();
            this.lb_prev = new System.Windows.Forms.Label();
            this.lb_average = new System.Windows.Forms.Label();
            this.chb_skip8 = new System.Windows.Forms.CheckBox();
            this.chb_skip7 = new System.Windows.Forms.CheckBox();
            this.lb_restart = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cb_start
            // 
            this.cb_start.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_start.Location = new System.Drawing.Point(168, 6);
            this.cb_start.Name = "cb_start";
            this.cb_start.Size = new System.Drawing.Size(154, 84);
            this.cb_start.TabIndex = 13;
            this.cb_start.Text = "Start";
            this.cb_start.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_start.UseVisualStyleBackColor = true;
            this.cb_start.CheckedChanged += new System.EventHandler(this.cb_start_CheckedChanged);
            // 
            // cb_type
            // 
            this.cb_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_type.FormattingEnabled = true;
            this.cb_type.Items.AddRange(new object[] {
            "Warhall",
            "Campaign"});
            this.cb_type.Location = new System.Drawing.Point(310, 188);
            this.cb_type.Name = "cb_type";
            this.cb_type.Size = new System.Drawing.Size(113, 21);
            this.cb_type.TabIndex = 14;
            this.cb_type.Visible = false;
            this.cb_type.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // cb_ratio
            // 
            this.cb_ratio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ratio.FormattingEnabled = true;
            this.cb_ratio.Items.AddRange(new object[] {
            "16:9",
            "_test"});
            this.cb_ratio.Location = new System.Drawing.Point(26, 22);
            this.cb_ratio.Name = "cb_ratio";
            this.cb_ratio.Size = new System.Drawing.Size(113, 21);
            this.cb_ratio.TabIndex = 15;
            this.cb_ratio.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // lb_type
            // 
            this.lb_type.AutoSize = true;
            this.lb_type.Location = new System.Drawing.Point(308, 172);
            this.lb_type.Name = "lb_type";
            this.lb_type.Size = new System.Drawing.Size(66, 13);
            this.lb_type.TabIndex = 16;
            this.lb_type.Text = "Тип работы";
            this.lb_type.Visible = false;
            // 
            // lb_ratio
            // 
            this.lb_ratio.AutoSize = true;
            this.lb_ratio.Location = new System.Drawing.Point(26, 6);
            this.lb_ratio.Name = "lb_ratio";
            this.lb_ratio.Size = new System.Drawing.Size(113, 13);
            this.lb_ratio.TabIndex = 17;
            this.lb_ratio.Text = "Соотношение сторон";
            // 
            // b_test
            // 
            this.b_test.Location = new System.Drawing.Point(220, 203);
            this.b_test.Name = "b_test";
            this.b_test.Size = new System.Drawing.Size(75, 23);
            this.b_test.TabIndex = 18;
            this.b_test.Text = "Test";
            this.b_test.UseVisualStyleBackColor = true;
            this.b_test.Click += new System.EventHandler(this.button1_Click);
            // 
            // chb_debug
            // 
            this.chb_debug.AutoSize = true;
            this.chb_debug.Location = new System.Drawing.Point(156, 207);
            this.chb_debug.Name = "chb_debug";
            this.chb_debug.Size = new System.Drawing.Size(58, 17);
            this.chb_debug.TabIndex = 19;
            this.chb_debug.Text = "Debug";
            this.chb_debug.UseVisualStyleBackColor = true;
            this.chb_debug.CheckedChanged += new System.EventHandler(this.chb_debug_CheckedChanged);
            // 
            // lb_current
            // 
            this.lb_current.AutoSize = true;
            this.lb_current.Location = new System.Drawing.Point(165, 176);
            this.lb_current.Name = "lb_current";
            this.lb_current.Size = new System.Drawing.Size(78, 13);
            this.lb_current.TabIndex = 20;
            this.lb_current.Text = "Текущий этап";
            // 
            // chb_skip1
            // 
            this.chb_skip1.AutoSize = true;
            this.chb_skip1.Checked = true;
            this.chb_skip1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip1.Location = new System.Drawing.Point(33, 70);
            this.chb_skip1.Name = "chb_skip1";
            this.chb_skip1.Size = new System.Drawing.Size(63, 17);
            this.chb_skip1.TabIndex = 21;
            this.chb_skip1.Text = "Кролик";
            this.chb_skip1.UseVisualStyleBackColor = true;
            // 
            // lb_skip
            // 
            this.lb_skip.AutoSize = true;
            this.lb_skip.Location = new System.Drawing.Point(27, 50);
            this.lb_skip.Name = "lb_skip";
            this.lb_skip.Size = new System.Drawing.Size(98, 13);
            this.lb_skip.TabIndex = 22;
            this.lb_skip.Text = "Кого пропускаем:";
            // 
            // chb_skip2
            // 
            this.chb_skip2.AutoSize = true;
            this.chb_skip2.Checked = true;
            this.chb_skip2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip2.Location = new System.Drawing.Point(33, 90);
            this.chb_skip2.Name = "chb_skip2";
            this.chb_skip2.Size = new System.Drawing.Size(56, 17);
            this.chb_skip2.TabIndex = 23;
            this.chb_skip2.Text = "Ветер";
            this.chb_skip2.UseVisualStyleBackColor = true;
            // 
            // chb_skip3
            // 
            this.chb_skip3.AutoSize = true;
            this.chb_skip3.Checked = true;
            this.chb_skip3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip3.Location = new System.Drawing.Point(33, 110);
            this.chb_skip3.Name = "chb_skip3";
            this.chb_skip3.Size = new System.Drawing.Size(72, 17);
            this.chb_skip3.TabIndex = 24;
            this.chb_skip3.Text = "Несущий";
            this.chb_skip3.UseVisualStyleBackColor = true;
            // 
            // chb_skip4
            // 
            this.chb_skip4.AutoSize = true;
            this.chb_skip4.Checked = true;
            this.chb_skip4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip4.Location = new System.Drawing.Point(33, 130);
            this.chb_skip4.Name = "chb_skip4";
            this.chb_skip4.Size = new System.Drawing.Size(51, 17);
            this.chb_skip4.TabIndex = 25;
            this.chb_skip4.Text = "Таро";
            this.chb_skip4.UseVisualStyleBackColor = true;
            // 
            // chb_skip5
            // 
            this.chb_skip5.AutoSize = true;
            this.chb_skip5.Checked = true;
            this.chb_skip5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip5.Location = new System.Drawing.Point(33, 150);
            this.chb_skip5.Name = "chb_skip5";
            this.chb_skip5.Size = new System.Drawing.Size(64, 17);
            this.chb_skip5.TabIndex = 26;
            this.chb_skip5.Text = "Медуза";
            this.chb_skip5.UseVisualStyleBackColor = true;
            // 
            // chb_skip6
            // 
            this.chb_skip6.AutoSize = true;
            this.chb_skip6.Checked = true;
            this.chb_skip6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip6.Location = new System.Drawing.Point(33, 170);
            this.chb_skip6.Name = "chb_skip6";
            this.chb_skip6.Size = new System.Drawing.Size(51, 17);
            this.chb_skip6.TabIndex = 27;
            this.chb_skip6.Text = "Игла";
            this.chb_skip6.UseVisualStyleBackColor = true;
            // 
            // lb_start
            // 
            this.lb_start.AutoSize = true;
            this.lb_start.Location = new System.Drawing.Point(165, 96);
            this.lb_start.Name = "lb_start";
            this.lb_start.Size = new System.Drawing.Size(144, 13);
            this.lb_start.TabIndex = 28;
            this.lb_start.Text = "Время запуска (до сброса)";
            // 
            // lb_prev
            // 
            this.lb_prev.AutoSize = true;
            this.lb_prev.Location = new System.Drawing.Point(165, 116);
            this.lb_prev.Name = "lb_prev";
            this.lb_prev.Size = new System.Drawing.Size(82, 13);
            this.lb_prev.TabIndex = 29;
            this.lb_prev.Text = "Прошлый цикл";
            // 
            // lb_average
            // 
            this.lb_average.AutoSize = true;
            this.lb_average.Location = new System.Drawing.Point(165, 136);
            this.lb_average.Name = "lb_average";
            this.lb_average.Size = new System.Drawing.Size(85, 13);
            this.lb_average.TabIndex = 30;
            this.lb_average.Text = "Среднее время";
            // 
            // chb_skip8
            // 
            this.chb_skip8.AutoSize = true;
            this.chb_skip8.Checked = true;
            this.chb_skip8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip8.Location = new System.Drawing.Point(33, 210);
            this.chb_skip8.Name = "chb_skip8";
            this.chb_skip8.Size = new System.Drawing.Size(59, 17);
            this.chb_skip8.TabIndex = 31;
            this.chb_skip8.Text = "Диван";
            this.chb_skip8.UseVisualStyleBackColor = true;
            // 
            // chb_skip7
            // 
            this.chb_skip7.AutoSize = true;
            this.chb_skip7.Checked = true;
            this.chb_skip7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_skip7.Location = new System.Drawing.Point(33, 190);
            this.chb_skip7.Name = "chb_skip7";
            this.chb_skip7.Size = new System.Drawing.Size(59, 17);
            this.chb_skip7.TabIndex = 32;
            this.chb_skip7.Text = "Кошка";
            this.chb_skip7.UseVisualStyleBackColor = true;
            // 
            // lb_restart
            // 
            this.lb_restart.AutoSize = true;
            this.lb_restart.Location = new System.Drawing.Point(165, 156);
            this.lb_restart.Name = "lb_restart";
            this.lb_restart.Size = new System.Drawing.Size(68, 13);
            this.lb_restart.TabIndex = 33;
            this.lb_restart.Text = "Перезапуск";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 231);
            this.Controls.Add(this.lb_restart);
            this.Controls.Add(this.chb_skip7);
            this.Controls.Add(this.chb_skip8);
            this.Controls.Add(this.lb_average);
            this.Controls.Add(this.lb_prev);
            this.Controls.Add(this.lb_start);
            this.Controls.Add(this.chb_skip6);
            this.Controls.Add(this.chb_skip5);
            this.Controls.Add(this.chb_skip4);
            this.Controls.Add(this.chb_skip3);
            this.Controls.Add(this.chb_skip2);
            this.Controls.Add(this.lb_skip);
            this.Controls.Add(this.chb_skip1);
            this.Controls.Add(this.lb_current);
            this.Controls.Add(this.chb_debug);
            this.Controls.Add(this.b_test);
            this.Controls.Add(this.lb_ratio);
            this.Controls.Add(this.lb_type);
            this.Controls.Add(this.cb_ratio);
            this.Controls.Add(this.cb_type);
            this.Controls.Add(this.cb_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Автофарм ресурсов v1.1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_start;
        private System.Windows.Forms.ComboBox cb_type;
        private System.Windows.Forms.ComboBox cb_ratio;
        private System.Windows.Forms.Label lb_type;
        private System.Windows.Forms.Label lb_ratio;
        private System.Windows.Forms.Button b_test;
        private System.Windows.Forms.CheckBox chb_debug;
        private System.Windows.Forms.Label lb_current;
        private System.Windows.Forms.CheckBox chb_skip1;
        private System.Windows.Forms.Label lb_skip;
        private System.Windows.Forms.CheckBox chb_skip2;
        private System.Windows.Forms.CheckBox chb_skip3;
        private System.Windows.Forms.CheckBox chb_skip4;
        private System.Windows.Forms.CheckBox chb_skip5;
        private System.Windows.Forms.CheckBox chb_skip6;
        private System.Windows.Forms.Label lb_start;
        private System.Windows.Forms.Label lb_prev;
        private System.Windows.Forms.Label lb_average;
        private System.Windows.Forms.CheckBox chb_skip8;
        private System.Windows.Forms.CheckBox chb_skip7;
        private System.Windows.Forms.Label lb_restart;
    }
}

