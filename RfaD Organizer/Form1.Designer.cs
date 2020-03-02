namespace RfaD_Organizer
{
    partial class ROrganizer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ROrganizer));
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.modpackBrowseBox = new System.Windows.Forms.TextBox();
            this.moBrowseBox = new System.Windows.Forms.TextBox();
            this.modpackLabel = new System.Windows.Forms.Label();
            this.moLabel = new System.Windows.Forms.Label();
            this.modpackBrowseButton = new System.Windows.Forms.Button();
            this.moBrowseButton = new System.Windows.Forms.Button();
            this.doAllButton = new System.Windows.Forms.Button();
            this.verLabel = new System.Windows.Forms.Label();
            this.byLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cancelButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.skyrimLabel = new System.Windows.Forms.Label();
            this.skyrimBrowseBox = new System.Windows.Forms.TextBox();
            this.skyrimBrowseButton = new System.Windows.Forms.Button();
            this.addExecutablesButton = new System.Windows.Forms.Button();
            this.createProfileButton = new System.Windows.Forms.Button();
            this.organizeModsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // logoBox
            // 
            this.logoBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoBox.Image = global::RfaD_Organizer.Properties.Resources.logo_black;
            this.logoBox.Location = new System.Drawing.Point(0, 0);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(484, 160);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoBox.TabIndex = 0;
            this.logoBox.TabStop = false;
            // 
            // modpackBrowseBox
            // 
            this.modpackBrowseBox.BackColor = System.Drawing.SystemColors.Window;
            this.modpackBrowseBox.Location = new System.Drawing.Point(12, 179);
            this.modpackBrowseBox.Name = "modpackBrowseBox";
            this.modpackBrowseBox.Size = new System.Drawing.Size(379, 20);
            this.modpackBrowseBox.TabIndex = 1;
            // 
            // moBrowseBox
            // 
            this.moBrowseBox.BackColor = System.Drawing.SystemColors.Window;
            this.moBrowseBox.Location = new System.Drawing.Point(12, 257);
            this.moBrowseBox.Name = "moBrowseBox";
            this.moBrowseBox.Size = new System.Drawing.Size(379, 20);
            this.moBrowseBox.TabIndex = 2;
            this.moBrowseBox.TextChanged += new System.EventHandler(this.moBrowseBox_TextChanged);
            // 
            // modpackLabel
            // 
            this.modpackLabel.AutoSize = true;
            this.modpackLabel.Location = new System.Drawing.Point(9, 163);
            this.modpackLabel.Name = "modpackLabel";
            this.modpackLabel.Size = new System.Drawing.Size(234, 13);
            this.modpackLabel.TabIndex = 3;
            this.modpackLabel.Text = "Путь к папке с разархивированной сборкой:";
            // 
            // moLabel
            // 
            this.moLabel.AutoSize = true;
            this.moLabel.Location = new System.Drawing.Point(9, 241);
            this.moLabel.Name = "moLabel";
            this.moLabel.Size = new System.Drawing.Size(230, 13);
            this.moLabel.TabIndex = 4;
            this.moLabel.Text = "Путь к основному каталогу Mod Organizer 2:";
            // 
            // modpackBrowseButton
            // 
            this.modpackBrowseButton.BackColor = System.Drawing.SystemColors.Control;
            this.modpackBrowseButton.Location = new System.Drawing.Point(397, 179);
            this.modpackBrowseButton.Name = "modpackBrowseButton";
            this.modpackBrowseButton.Size = new System.Drawing.Size(75, 20);
            this.modpackBrowseButton.TabIndex = 5;
            this.modpackBrowseButton.Text = "Обзор";
            this.modpackBrowseButton.UseVisualStyleBackColor = false;
            this.modpackBrowseButton.Click += new System.EventHandler(this.modpackBrowseButton_Click);
            // 
            // moBrowseButton
            // 
            this.moBrowseButton.Location = new System.Drawing.Point(397, 256);
            this.moBrowseButton.Name = "moBrowseButton";
            this.moBrowseButton.Size = new System.Drawing.Size(75, 20);
            this.moBrowseButton.TabIndex = 6;
            this.moBrowseButton.Text = "Обзор";
            this.moBrowseButton.UseVisualStyleBackColor = true;
            this.moBrowseButton.Click += new System.EventHandler(this.moBrowseButton_Click);
            // 
            // doAllButton
            // 
            this.doAllButton.Location = new System.Drawing.Point(12, 283);
            this.doAllButton.Name = "doAllButton";
            this.doAllButton.Size = new System.Drawing.Size(460, 23);
            this.doAllButton.TabIndex = 7;
            this.doAllButton.Text = "Полная установка";
            this.doAllButton.UseVisualStyleBackColor = true;
            this.doAllButton.Click += new System.EventHandler(this.doAllButton_Click);
            // 
            // verLabel
            // 
            this.verLabel.AutoSize = true;
            this.verLabel.Location = new System.Drawing.Point(9, 339);
            this.verLabel.Name = "verLabel";
            this.verLabel.Size = new System.Drawing.Size(49, 13);
            this.verLabel.TabIndex = 8;
            this.verLabel.Text = "ver 6.0.1";
            // 
            // byLabel
            // 
            this.byLabel.AutoSize = true;
            this.byLabel.Location = new System.Drawing.Point(346, 339);
            this.byLabel.Name = "byLabel";
            this.byLabel.Size = new System.Drawing.Size(123, 13);
            this.byLabel.TabIndex = 9;
            this.byLabel.Text = "RfaD Organizer by Fozar";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.Cursor = System.Windows.Forms.Cursors.No;
            this.progressBar1.Location = new System.Drawing.Point(12, 283);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(379, 52);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(398, 283);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(74, 52);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // skyrimLabel
            // 
            this.skyrimLabel.AutoSize = true;
            this.skyrimLabel.Location = new System.Drawing.Point(9, 202);
            this.skyrimLabel.Name = "skyrimLabel";
            this.skyrimLabel.Size = new System.Drawing.Size(110, 13);
            this.skyrimLabel.TabIndex = 12;
            this.skyrimLabel.Text = "Путь к папке Skyrim:";
            // 
            // skyrimBrowseBox
            // 
            this.skyrimBrowseBox.BackColor = System.Drawing.SystemColors.Window;
            this.skyrimBrowseBox.Location = new System.Drawing.Point(12, 218);
            this.skyrimBrowseBox.Name = "skyrimBrowseBox";
            this.skyrimBrowseBox.Size = new System.Drawing.Size(379, 20);
            this.skyrimBrowseBox.TabIndex = 13;
            // 
            // skyrimBrowseButton
            // 
            this.skyrimBrowseButton.BackColor = System.Drawing.SystemColors.Control;
            this.skyrimBrowseButton.Location = new System.Drawing.Point(397, 217);
            this.skyrimBrowseButton.Name = "skyrimBrowseButton";
            this.skyrimBrowseButton.Size = new System.Drawing.Size(75, 20);
            this.skyrimBrowseButton.TabIndex = 14;
            this.skyrimBrowseButton.Text = "Обзор";
            this.skyrimBrowseButton.UseVisualStyleBackColor = false;
            this.skyrimBrowseButton.Click += new System.EventHandler(this.skyrimBrowseButton_Click);
            // 
            // addExecutablesButton
            // 
            this.addExecutablesButton.Location = new System.Drawing.Point(138, 312);
            this.addExecutablesButton.Name = "addExecutablesButton";
            this.addExecutablesButton.Size = new System.Drawing.Size(208, 23);
            this.addExecutablesButton.TabIndex = 15;
            this.addExecutablesButton.Text = "Добавить исполняемые файлы";
            this.addExecutablesButton.UseVisualStyleBackColor = true;
            this.addExecutablesButton.Click += new System.EventHandler(this.addExecutablesButton_Click);
            // 
            // createProfileButton
            // 
            this.createProfileButton.Location = new System.Drawing.Point(352, 312);
            this.createProfileButton.Name = "createProfileButton";
            this.createProfileButton.Size = new System.Drawing.Size(120, 23);
            this.createProfileButton.TabIndex = 16;
            this.createProfileButton.Text = "Создать профиль";
            this.createProfileButton.UseVisualStyleBackColor = true;
            this.createProfileButton.Click += new System.EventHandler(this.createProfileButton_Click);
            // 
            // organizeModsButton
            // 
            this.organizeModsButton.Location = new System.Drawing.Point(12, 312);
            this.organizeModsButton.Name = "organizeModsButton";
            this.organizeModsButton.Size = new System.Drawing.Size(120, 23);
            this.organizeModsButton.TabIndex = 17;
            this.organizeModsButton.Text = "Организовать моды";
            this.organizeModsButton.UseVisualStyleBackColor = true;
            this.organizeModsButton.Click += new System.EventHandler(this.organizeModsButton_Click);
            // 
            // ROrganizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.organizeModsButton);
            this.Controls.Add(this.createProfileButton);
            this.Controls.Add(this.addExecutablesButton);
            this.Controls.Add(this.skyrimBrowseButton);
            this.Controls.Add(this.skyrimBrowseBox);
            this.Controls.Add(this.skyrimLabel);
            this.Controls.Add(this.byLabel);
            this.Controls.Add(this.verLabel);
            this.Controls.Add(this.doAllButton);
            this.Controls.Add(this.moBrowseButton);
            this.Controls.Add(this.modpackBrowseButton);
            this.Controls.Add(this.moLabel);
            this.Controls.Add(this.modpackLabel);
            this.Controls.Add(this.moBrowseBox);
            this.Controls.Add(this.modpackBrowseBox);
            this.Controls.Add(this.logoBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "ROrganizer";
            this.Text = "Requiem for a Dream Organizer";
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoBox;
        private System.Windows.Forms.TextBox modpackBrowseBox;
        private System.Windows.Forms.TextBox moBrowseBox;
        private System.Windows.Forms.Label modpackLabel;
        private System.Windows.Forms.Label moLabel;
        private System.Windows.Forms.Button modpackBrowseButton;
        private System.Windows.Forms.Button moBrowseButton;
        private System.Windows.Forms.Button doAllButton;
        private System.Windows.Forms.Label verLabel;
        private System.Windows.Forms.Label byLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button cancelButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label skyrimLabel;
        private System.Windows.Forms.TextBox skyrimBrowseBox;
        private System.Windows.Forms.Button skyrimBrowseButton;
        private System.Windows.Forms.Button addExecutablesButton;
        private System.Windows.Forms.Button createProfileButton;
        private System.Windows.Forms.Button organizeModsButton;
    }
}

