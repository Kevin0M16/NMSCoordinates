using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NMSCoordinates
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.pictureBox24 = new System.Windows.Forms.PictureBox();
            this.pictureBox23 = new System.Windows.Forms.PictureBox();
            this.pictureBox21 = new System.Windows.Forms.PictureBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.pictureBox16 = new System.Windows.Forms.PictureBox();
            this.pictureBox17 = new System.Windows.Forms.PictureBox();
            this.pictureBox18 = new System.Windows.Forms.PictureBox();
            this.pictureBox19 = new System.Windows.Forms.PictureBox();
            this.pictureBox20 = new System.Windows.Forms.PictureBox();
            this.pictureBox22 = new System.Windows.Forms.PictureBox();
            this.pictureBox25 = new System.Windows.Forms.PictureBox();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.textBox26 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox27 = new System.Windows.Forms.TextBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox24 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectSaveFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appDataDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manuallySelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discoveriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupALLSaveFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotPageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotPageToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox25)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(876, 936);
            this.tabControl1.TabIndex = 57;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage1.Controls.Add(this.progressBar2);
            this.tabPage1.Controls.Add(this.groupBox15);
            this.tabPage1.Controls.Add(this.textBox17);
            this.tabPage1.Controls.Add(this.groupBox13);
            this.tabPage1.Controls.Add(this.groupBox12);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(868, 908);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Save Files";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(145, 301);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(538, 25);
            this.progressBar2.TabIndex = 70;
            this.progressBar2.Visible = false;
            // 
            // groupBox15
            // 
            this.groupBox15.BackColor = System.Drawing.Color.Transparent;
            this.groupBox15.Controls.Add(this.groupBox11);
            this.groupBox15.Controls.Add(this.groupBox14);
            this.groupBox15.Controls.Add(this.pictureBox25);
            this.groupBox15.ForeColor = System.Drawing.Color.White;
            this.groupBox15.Location = new System.Drawing.Point(15, 332);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(837, 420);
            this.groupBox15.TabIndex = 69;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Current Player Location";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label21);
            this.groupBox11.Controls.Add(this.textBox23);
            this.groupBox11.Controls.Add(this.textBox21);
            this.groupBox11.Controls.Add(this.label19);
            this.groupBox11.Controls.Add(this.label20);
            this.groupBox11.Controls.Add(this.textBox22);
            this.groupBox11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox11.Location = new System.Drawing.Point(23, 144);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(208, 195);
            this.groupBox11.TabIndex = 67;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Location Coordinates";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(118, 155);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(81, 15);
            this.label21.TabIndex = 32;
            this.label21.Text = "Galaxy Name";
            // 
            // textBox23
            // 
            this.textBox23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox23.ForeColor = System.Drawing.Color.Azure;
            this.textBox23.Location = new System.Drawing.Point(19, 150);
            this.textBox23.Multiline = true;
            this.textBox23.Name = "textBox23";
            this.textBox23.ReadOnly = true;
            this.textBox23.Size = new System.Drawing.Size(93, 24);
            this.textBox23.TabIndex = 31;
            this.textBox23.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox21
            // 
            this.textBox21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox21.ForeColor = System.Drawing.Color.Azure;
            this.textBox21.Location = new System.Drawing.Point(20, 99);
            this.textBox21.Multiline = true;
            this.textBox21.Name = "textBox21";
            this.textBox21.ReadOnly = true;
            this.textBox21.Size = new System.Drawing.Size(93, 24);
            this.textBox21.TabIndex = 29;
            this.textBox21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(119, 104);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 15);
            this.label19.TabIndex = 30;
            this.label19.Text = "Portal Code";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(35, 31);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(120, 15);
            this.label20.TabIndex = 22;
            this.label20.Text = "Galactic Coordinates";
            // 
            // textBox22
            // 
            this.textBox22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox22.ForeColor = System.Drawing.Color.Azure;
            this.textBox22.Location = new System.Drawing.Point(19, 52);
            this.textBox22.Multiline = true;
            this.textBox22.Name = "textBox22";
            this.textBox22.ReadOnly = true;
            this.textBox22.Size = new System.Drawing.Size(134, 24);
            this.textBox22.TabIndex = 1;
            this.textBox22.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.pictureBox13);
            this.groupBox14.Controls.Add(this.pictureBox14);
            this.groupBox14.Controls.Add(this.pictureBox24);
            this.groupBox14.Controls.Add(this.pictureBox23);
            this.groupBox14.Controls.Add(this.pictureBox21);
            this.groupBox14.Controls.Add(this.pictureBox15);
            this.groupBox14.Controls.Add(this.pictureBox16);
            this.groupBox14.Controls.Add(this.pictureBox17);
            this.groupBox14.Controls.Add(this.pictureBox18);
            this.groupBox14.Controls.Add(this.pictureBox19);
            this.groupBox14.Controls.Add(this.pictureBox20);
            this.groupBox14.Controls.Add(this.pictureBox22);
            this.groupBox14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox14.Location = new System.Drawing.Point(13, 20);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(805, 103);
            this.groupBox14.TabIndex = 68;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Glyph Coordinates";
            // 
            // pictureBox13
            // 
            this.pictureBox13.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox13.Location = new System.Drawing.Point(10, 20);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(65, 70);
            this.pictureBox13.TabIndex = 43;
            this.pictureBox13.TabStop = false;
            // 
            // pictureBox14
            // 
            this.pictureBox14.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox14.InitialImage = null;
            this.pictureBox14.Location = new System.Drawing.Point(75, 20);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(65, 70);
            this.pictureBox14.TabIndex = 32;
            this.pictureBox14.TabStop = false;
            // 
            // pictureBox24
            // 
            this.pictureBox24.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox24.Location = new System.Drawing.Point(725, 20);
            this.pictureBox24.Name = "pictureBox24";
            this.pictureBox24.Size = new System.Drawing.Size(65, 70);
            this.pictureBox24.TabIndex = 40;
            this.pictureBox24.TabStop = false;
            // 
            // pictureBox23
            // 
            this.pictureBox23.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox23.Location = new System.Drawing.Point(660, 20);
            this.pictureBox23.Name = "pictureBox23";
            this.pictureBox23.Size = new System.Drawing.Size(65, 70);
            this.pictureBox23.TabIndex = 39;
            this.pictureBox23.TabStop = false;
            // 
            // pictureBox21
            // 
            this.pictureBox21.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox21.Location = new System.Drawing.Point(530, 20);
            this.pictureBox21.Name = "pictureBox21";
            this.pictureBox21.Size = new System.Drawing.Size(65, 70);
            this.pictureBox21.TabIndex = 38;
            this.pictureBox21.TabStop = false;
            // 
            // pictureBox15
            // 
            this.pictureBox15.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox15.Location = new System.Drawing.Point(140, 20);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(65, 70);
            this.pictureBox15.TabIndex = 33;
            this.pictureBox15.TabStop = false;
            // 
            // pictureBox16
            // 
            this.pictureBox16.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox16.Location = new System.Drawing.Point(205, 20);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(65, 70);
            this.pictureBox16.TabIndex = 34;
            this.pictureBox16.TabStop = false;
            // 
            // pictureBox17
            // 
            this.pictureBox17.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox17.Location = new System.Drawing.Point(270, 20);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(65, 70);
            this.pictureBox17.TabIndex = 35;
            this.pictureBox17.TabStop = false;
            // 
            // pictureBox18
            // 
            this.pictureBox18.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox18.Location = new System.Drawing.Point(335, 20);
            this.pictureBox18.Name = "pictureBox18";
            this.pictureBox18.Size = new System.Drawing.Size(65, 70);
            this.pictureBox18.TabIndex = 36;
            this.pictureBox18.TabStop = false;
            // 
            // pictureBox19
            // 
            this.pictureBox19.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox19.Location = new System.Drawing.Point(400, 20);
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.Size = new System.Drawing.Size(65, 70);
            this.pictureBox19.TabIndex = 37;
            this.pictureBox19.TabStop = false;
            // 
            // pictureBox20
            // 
            this.pictureBox20.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox20.Location = new System.Drawing.Point(465, 20);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(65, 70);
            this.pictureBox20.TabIndex = 42;
            this.pictureBox20.TabStop = false;
            // 
            // pictureBox22
            // 
            this.pictureBox22.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox22.Location = new System.Drawing.Point(595, 20);
            this.pictureBox22.Name = "pictureBox22";
            this.pictureBox22.Size = new System.Drawing.Size(65, 70);
            this.pictureBox22.TabIndex = 41;
            this.pictureBox22.TabStop = false;
            // 
            // pictureBox25
            // 
            this.pictureBox25.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox25.Location = new System.Drawing.Point(274, 129);
            this.pictureBox25.Name = "pictureBox25";
            this.pictureBox25.Size = new System.Drawing.Size(479, 285);
            this.pictureBox25.TabIndex = 69;
            this.pictureBox25.TabStop = false;
            // 
            // textBox17
            // 
            this.textBox17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox17.ForeColor = System.Drawing.Color.Azure;
            this.textBox17.Location = new System.Drawing.Point(78, 758);
            this.textBox17.Multiline = true;
            this.textBox17.Name = "textBox17";
            this.textBox17.ReadOnly = true;
            this.textBox17.Size = new System.Drawing.Size(690, 86);
            this.textBox17.TabIndex = 2;
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.Color.Transparent;
            this.groupBox13.Controls.Add(this.textBox16);
            this.groupBox13.ForeColor = System.Drawing.Color.White;
            this.groupBox13.Location = new System.Drawing.Point(145, 230);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(538, 65);
            this.groupBox13.TabIndex = 2;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Current Save Path";
            // 
            // textBox16
            // 
            this.textBox16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox16.ForeColor = System.Drawing.Color.Azure;
            this.textBox16.Location = new System.Drawing.Point(28, 25);
            this.textBox16.Multiline = true;
            this.textBox16.Name = "textBox16";
            this.textBox16.ReadOnly = true;
            this.textBox16.Size = new System.Drawing.Size(490, 24);
            this.textBox16.TabIndex = 1;
            // 
            // groupBox12
            // 
            this.groupBox12.BackColor = System.Drawing.Color.Transparent;
            this.groupBox12.Controls.Add(this.textBox26);
            this.groupBox12.Controls.Add(this.label24);
            this.groupBox12.Controls.Add(this.comboBox2);
            this.groupBox12.Controls.Add(this.label18);
            this.groupBox12.Controls.Add(this.button4);
            this.groupBox12.Controls.Add(this.comboBox1);
            this.groupBox12.ForeColor = System.Drawing.Color.White;
            this.groupBox12.Location = new System.Drawing.Point(215, 10);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(393, 214);
            this.groupBox12.TabIndex = 1;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Load Save File";
            // 
            // textBox26
            // 
            this.textBox26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox26.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox26.Location = new System.Drawing.Point(90, 127);
            this.textBox26.MinimumSize = new System.Drawing.Size(190, 18);
            this.textBox26.Name = "textBox26";
            this.textBox26.ReadOnly = true;
            this.textBox26.Size = new System.Drawing.Size(190, 14);
            this.textBox26.TabIndex = 33;
            this.textBox26.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(91, 25);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(58, 15);
            this.label24.TabIndex = 6;
            this.label24.Text = "Save Slot";
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.ForeColor = System.Drawing.Color.Azure;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(91, 42);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(188, 23);
            this.comboBox2.Sorted = true;
            this.comboBox2.TabIndex = 5;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2_SelectedIndexChanged);
            this.comboBox2.SelectionChangeCommitted += new System.EventHandler(this.ComboBox2_SelectionChangeCommitted);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(90, 78);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(137, 15);
            this.label18.TabIndex = 4;
            this.label18.Text = "Please select a save file";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button4.Location = new System.Drawing.Point(188, 149);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 31);
            this.button4.TabIndex = 3;
            this.button4.Text = "Reload Save";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.ForeColor = System.Drawing.Color.Azure;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "(Select a save file)"});
            this.comboBox1.Location = new System.Drawing.Point(91, 98);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(188, 23);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.ComboBox1_SelectionChangeCommitted);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage2.Controls.Add(this.progressBar1);
            this.tabPage2.Controls.Add(this.textBox27);
            this.tabPage2.Controls.Add(this.groupBox17);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.label23);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(868, 908);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Base and Space Station";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(539, 552);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(322, 25);
            this.progressBar1.TabIndex = 73;
            this.progressBar1.Visible = false;
            // 
            // textBox27
            // 
            this.textBox27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox27.ForeColor = System.Drawing.Color.Azure;
            this.textBox27.Location = new System.Drawing.Point(382, 707);
            this.textBox27.Multiline = true;
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new System.Drawing.Size(469, 86);
            this.textBox27.TabIndex = 32;
            // 
            // groupBox17
            // 
            this.groupBox17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox17.Controls.Add(this.button5);
            this.groupBox17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox17.Location = new System.Drawing.Point(333, 411);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(200, 61);
            this.groupBox17.TabIndex = 72;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Fast Travel";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(30, 22);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(134, 25);
            this.button5.TabIndex = 0;
            this.button5.Text = "Move Player Here";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.Button5_ClickAsync);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox7.Controls.Add(this.textBox12);
            this.groupBox7.Controls.Add(this.button3);
            this.groupBox7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox7.Location = new System.Drawing.Point(334, 479);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(199, 98);
            this.groupBox7.TabIndex = 67;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Portal Interference Status";
            // 
            // textBox12
            // 
            this.textBox12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox12.ForeColor = System.Drawing.Color.Azure;
            this.textBox12.Location = new System.Drawing.Point(60, 27);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(75, 24);
            this.textBox12.TabIndex = 25;
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(30, 59);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(134, 27);
            this.button3.TabIndex = 27;
            this.button3.Text = "Clear interference";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label23.Location = new System.Drawing.Point(91, 561);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(183, 15);
            this.label23.TabIndex = 70;
            this.label23.Text = "(B) = Base  (SS) = Space Station";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox6.Controls.Add(this.textBox2);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.textBox1);
            this.groupBox6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox6.Location = new System.Drawing.Point(334, 265);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(199, 140);
            this.groupBox6.TabIndex = 66;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Location Coordinates";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox2.ForeColor = System.Drawing.Color.Azure;
            this.textBox2.Location = new System.Drawing.Point(30, 95);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(134, 24);
            this.textBox2.TabIndex = 29;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(62, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 15);
            this.label10.TabIndex = 30;
            this.label10.Text = "Portal Code";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(45, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "Galactic Coordinates";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox1.ForeColor = System.Drawing.Color.Azure;
            this.textBox1.Location = new System.Drawing.Point(30, 42);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(134, 24);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox5.Controls.Add(this.textBox3);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox5.Location = new System.Drawing.Point(6, 697);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(360, 196);
            this.groupBox5.TabIndex = 65;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Calculations Log";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox3.ForeColor = System.Drawing.Color.Azure;
            this.textBox3.Location = new System.Drawing.Point(18, 20);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(326, 161);
            this.textBox3.TabIndex = 31;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox4.Controls.Add(this.pictureBox12);
            this.groupBox4.Controls.Add(this.pictureBox1);
            this.groupBox4.Controls.Add(this.pictureBox2);
            this.groupBox4.Controls.Add(this.pictureBox3);
            this.groupBox4.Controls.Add(this.pictureBox4);
            this.groupBox4.Controls.Add(this.pictureBox5);
            this.groupBox4.Controls.Add(this.pictureBox6);
            this.groupBox4.Controls.Add(this.pictureBox11);
            this.groupBox4.Controls.Add(this.pictureBox7);
            this.groupBox4.Controls.Add(this.pictureBox10);
            this.groupBox4.Controls.Add(this.pictureBox8);
            this.groupBox4.Controls.Add(this.pictureBox9);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox4.Location = new System.Drawing.Point(6, 584);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(855, 108);
            this.groupBox4.TabIndex = 64;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Glyph Coordinates";
            // 
            // pictureBox12
            // 
            this.pictureBox12.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox12.Location = new System.Drawing.Point(750, 20);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(65, 70);
            this.pictureBox12.TabIndex = 43;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(35, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(65, 70);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox2.Location = new System.Drawing.Point(100, 20);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(65, 70);
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox3.Location = new System.Drawing.Point(165, 20);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(65, 70);
            this.pictureBox3.TabIndex = 34;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox4.Location = new System.Drawing.Point(230, 20);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(65, 70);
            this.pictureBox4.TabIndex = 35;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox5.Location = new System.Drawing.Point(295, 20);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(65, 70);
            this.pictureBox5.TabIndex = 36;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox6.Location = new System.Drawing.Point(360, 20);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(65, 70);
            this.pictureBox6.TabIndex = 37;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox11.Location = new System.Drawing.Point(685, 20);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(65, 70);
            this.pictureBox11.TabIndex = 42;
            this.pictureBox11.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox7.Location = new System.Drawing.Point(425, 20);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(65, 70);
            this.pictureBox7.TabIndex = 38;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox10.Location = new System.Drawing.Point(620, 20);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(65, 70);
            this.pictureBox10.TabIndex = 41;
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox8.Location = new System.Drawing.Point(490, 20);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(65, 70);
            this.pictureBox8.TabIndex = 39;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox9.Location = new System.Drawing.Point(555, 20);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(65, 70);
            this.pictureBox9.TabIndex = 40;
            this.pictureBox9.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.textBox24);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.textBox19);
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Location = new System.Drawing.Point(6, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(322, 541);
            this.groupBox3.TabIndex = 63;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " All Available Locations";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.Azure;
            this.label25.Location = new System.Drawing.Point(32, 506);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(125, 15);
            this.label25.TabIndex = 70;
            this.label25.Text = "Click a location above";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(219, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 69;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // textBox24
            // 
            this.textBox24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox24.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox24.ForeColor = System.Drawing.Color.Azure;
            this.textBox24.Location = new System.Drawing.Point(18, 22);
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new System.Drawing.Size(195, 22);
            this.textBox24.TabIndex = 68;
            this.textBox24.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox24_KeyUp);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(205, 506);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 15);
            this.label17.TabIndex = 59;
            this.label17.Text = "Total Discoveries";
            // 
            // textBox19
            // 
            this.textBox19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox19.ForeColor = System.Drawing.Color.Azure;
            this.textBox19.Location = new System.Drawing.Point(169, 502);
            this.textBox19.Multiline = true;
            this.textBox19.Name = "textBox19";
            this.textBox19.ReadOnly = true;
            this.textBox19.Size = new System.Drawing.Size(30, 21);
            this.textBox19.TabIndex = 51;
            this.textBox19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.listBox1.ForeColor = System.Drawing.Color.Azure;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(18, 51);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(277, 424);
            this.listBox1.TabIndex = 6;
            this.listBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseClick);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.textBox10);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.textBox9);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(334, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 252);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Voxel Coordinates";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 222);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 15);
            this.label8.TabIndex = 23;
            this.label8.Text = "Galaxy Name";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox5.ForeColor = System.Drawing.Color.Azure;
            this.textBox5.Location = new System.Drawing.Point(25, 60);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(66, 24);
            this.textBox5.TabIndex = 9;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox10.ForeColor = System.Drawing.Color.Azure;
            this.textBox10.Location = new System.Drawing.Point(25, 216);
            this.textBox10.Multiline = true;
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(86, 24);
            this.textBox10.TabIndex = 21;
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox6.ForeColor = System.Drawing.Color.Azure;
            this.textBox6.Location = new System.Drawing.Point(25, 92);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(66, 24);
            this.textBox6.TabIndex = 10;
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox4.ForeColor = System.Drawing.Color.Azure;
            this.textBox4.Location = new System.Drawing.Point(25, 29);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(66, 24);
            this.textBox4.TabIndex = 8;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox7.ForeColor = System.Drawing.Color.Azure;
            this.textBox7.Location = new System.Drawing.Point(25, 123);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(66, 24);
            this.textBox7.TabIndex = 11;
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(97, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "Planet Index";
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox8.ForeColor = System.Drawing.Color.Azure;
            this.textBox8.Location = new System.Drawing.Point(25, 154);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(66, 24);
            this.textBox8.TabIndex = 12;
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox9.ForeColor = System.Drawing.Color.Azure;
            this.textBox9.Location = new System.Drawing.Point(25, 185);
            this.textBox9.Multiline = true;
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(66, 24);
            this.textBox9.TabIndex = 18;
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Galaxy";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(97, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "SSI";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "Z";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Y";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textBox25);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBox20);
            this.groupBox1.Controls.Add(this.listBox2);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(539, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 541);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Space Station Locations";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(224, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 27);
            this.button2.TabIndex = 71;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // textBox25
            // 
            this.textBox25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox25.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox25.ForeColor = System.Drawing.Color.Azure;
            this.textBox25.Location = new System.Drawing.Point(23, 24);
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new System.Drawing.Size(195, 22);
            this.textBox25.TabIndex = 70;
            this.textBox25.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox25_KeyUp);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(193, 505);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(119, 15);
            this.label16.TabIndex = 58;
            this.label16.Text = "Total Space Stations";
            // 
            // textBox20
            // 
            this.textBox20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox20.ForeColor = System.Drawing.Color.Azure;
            this.textBox20.Location = new System.Drawing.Point(157, 501);
            this.textBox20.Multiline = true;
            this.textBox20.Name = "textBox20";
            this.textBox20.ReadOnly = true;
            this.textBox20.Size = new System.Drawing.Size(30, 21);
            this.textBox20.TabIndex = 52;
            this.textBox20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.listBox2.ForeColor = System.Drawing.Color.Azure;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(22, 51);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(277, 424);
            this.listBox2.TabIndex = 20;
            this.listBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListBox2_MouseClick);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage3.Controls.Add(this.button7);
            this.tabPage3.Controls.Add(this.listBox3);
            this.tabPage3.Controls.Add(this.button6);
            this.tabPage3.Controls.Add(this.textBox11);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(868, 908);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Other";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(344, 730);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(142, 42);
            this.button7.TabIndex = 36;
            this.button7.Text = "Coordinate Calculator";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // listBox3
            // 
            this.listBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.listBox3.ForeColor = System.Drawing.Color.Azure;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 15;
            this.listBox3.Location = new System.Drawing.Point(19, 16);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(824, 454);
            this.listBox3.TabIndex = 35;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.ListBox3_SelectedIndexChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(344, 476);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(142, 42);
            this.button6.TabIndex = 34;
            this.button6.Text = "Load locbackup.txt";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox11.ForeColor = System.Drawing.Color.Azure;
            this.textBox11.Location = new System.Drawing.Point(247, 544);
            this.textBox11.Multiline = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(339, 142);
            this.textBox11.TabIndex = 33;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(898, 24);
            this.menuStrip1.TabIndex = 58;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectSaveFolderToolStripMenuItem,
            this.backupToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectSaveFolderToolStripMenuItem
            // 
            this.selectSaveFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appDataDefaultToolStripMenuItem,
            this.manuallySelectToolStripMenuItem});
            this.selectSaveFolderToolStripMenuItem.Name = "selectSaveFolderToolStripMenuItem";
            this.selectSaveFolderToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selectSaveFolderToolStripMenuItem.Text = "Select Save Folder";
            // 
            // appDataDefaultToolStripMenuItem
            // 
            this.appDataDefaultToolStripMenuItem.Name = "appDataDefaultToolStripMenuItem";
            this.appDataDefaultToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.appDataDefaultToolStripMenuItem.Text = "AppData (Default)";
            this.appDataDefaultToolStripMenuItem.Click += new System.EventHandler(this.AppDataDefaultToolStripMenuItem_Click);
            // 
            // manuallySelectToolStripMenuItem
            // 
            this.manuallySelectToolStripMenuItem.Name = "manuallySelectToolStripMenuItem";
            this.manuallySelectToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.manuallySelectToolStripMenuItem.Text = "Manually Select";
            this.manuallySelectToolStripMenuItem.Click += new System.EventHandler(this.ManuallySelectToolStripMenuItem_Click);
            // 
            // backupToolStripMenuItem
            // 
            this.backupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.discoveriesToolStripMenuItem,
            this.backupALLSaveFilesToolStripMenuItem});
            this.backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            this.backupToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.backupToolStripMenuItem.Text = "Backups...";
            // 
            // discoveriesToolStripMenuItem
            // 
            this.discoveriesToolStripMenuItem.Name = "discoveriesToolStripMenuItem";
            this.discoveriesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.discoveriesToolStripMenuItem.Text = "Backup Discoveries to *.txt";
            this.discoveriesToolStripMenuItem.Click += new System.EventHandler(this.DiscoveriesToolStripMenuItem_Click);
            // 
            // backupALLSaveFilesToolStripMenuItem
            // 
            this.backupALLSaveFilesToolStripMenuItem.Name = "backupALLSaveFilesToolStripMenuItem";
            this.backupALLSaveFilesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.backupALLSaveFilesToolStripMenuItem.Text = "Backup ALL Save Files";
            this.backupALLSaveFilesToolStripMenuItem.Click += new System.EventHandler(this.BackupALLSaveFilesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.screenshotPageToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.aboutToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.helpToolStripMenuItem.Text = "About";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            // 
            // screenshotPageToolStripMenuItem
            // 
            this.screenshotPageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.screenshotPageToolStripMenuItem1,
            this.screenshotPageToolStripMenuItem2});
            this.screenshotPageToolStripMenuItem.Name = "screenshotPageToolStripMenuItem";
            this.screenshotPageToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.screenshotPageToolStripMenuItem.Text = "Screenshots...";
            // 
            // screenshotPageToolStripMenuItem1
            // 
            this.screenshotPageToolStripMenuItem1.Name = "screenshotPageToolStripMenuItem1";
            this.screenshotPageToolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.screenshotPageToolStripMenuItem1.Text = "Screen Shot Page";
            this.screenshotPageToolStripMenuItem1.Click += new System.EventHandler(this.ScreenshotPageToolStripMenuItem1_Click);
            // 
            // screenshotPageToolStripMenuItem2
            // 
            this.screenshotPageToolStripMenuItem2.Name = "screenshotPageToolStripMenuItem2";
            this.screenshotPageToolStripMenuItem2.Size = new System.Drawing.Size(178, 22);
            this.screenshotPageToolStripMenuItem2.Text = "Set Screenshot Path";
            this.screenshotPageToolStripMenuItem2.Click += new System.EventHandler(this.ScreenshotPageToolStripMenuItem2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.ClientSize = new System.Drawing.Size(898, 980);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NMSCoordinates";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox25)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        public string savePath;
        public string hgFilePath;
        public string hgFileDir;
        public string nmsPath;
        public string hgfile;
        public string stmPath;
        public string ssdPath;
        public string ssPath;

        public string galaxy;
        public string X;
        public string Y;
        public string Z;
        public string SSI;
        public string GalacticCoord;
        public string GalacticCoord2;
        public string PortalCode;
        public string PI;
        public string voxel;

        public int igalaxy;
        public int iX;
        public int iY;
        public int iZ;
        public int iSSI;
        public int iGalacticCoord;
        public int iPortalCoord;
        public int iPI;

        public int saveslot;

        public int ipgalaxy;
        public int ipX;
        public int ipY;
        public int ipZ;
        public int ipSSI;
        public int ipGalacticCoord;
        public int ipPortalCoord;
        public int ipPI;

        public string json;
        public dynamic jsonObj;
        public dynamic jsonArray;

        private string decrypt;
        private string encrypt;

        public string rxPatternP;
        public string rxValP;
        public string rxPatternSt;
        public string rxValSt;
        public string rxPatternPs;
        public string rxValPs;
        public string rxPatternPrtl;
        public string rxValPrtl;

        public string rxPatternG;
        public string rxValG;
        public string rxPatternX;
        public string rxValX;
        public string rxPatternY;
        public string rxValY;
        public string rxPatternZ;
        public string rxValZ;
        public string rxPatternSSI;
        public string rxValSSI;
        public string rxPatternPI;
        public string rxValPI;

        public IDictionary<string, string> galaxyDict;
        public IDictionary<string, string> gameMode;
        public Dictionary<char, Bitmap> glyphDict;

        public Dictionary<int, string> sn1;
        public Dictionary<int, string> sn2;
        public Dictionary<int, string> sn3;
        public Dictionary<int, string> sn4;
        public Dictionary<int, string> sn5;

        public List<string> DiscList { get; private set; }
        public List<string> Backuplist = new List<string>();

        public List<string> BaseList { get; private set; }
        //public List<Bitmap> glyphlist { get; private set; }

        private char _gl1;
        private char _gl2;
        private char _gl3;
        private char _gl4;
        private char _gl5;
        private char _gl6;
        private char _gl7;
        private char _gl8;
        private char _gl9;
        private char _gl10;
        private char _gl11;
        private char _gl12;

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ToolStripMenuItem selectSaveFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appDataDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manuallySelectToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox22;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox24;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem backupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discoveriesToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox textBox26;
        private System.Windows.Forms.TextBox textBox27;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ToolStripMenuItem backupALLSaveFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenshotPageToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox25;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.PictureBox pictureBox14;
        private System.Windows.Forms.PictureBox pictureBox24;
        private System.Windows.Forms.PictureBox pictureBox23;
        private System.Windows.Forms.PictureBox pictureBox21;
        private System.Windows.Forms.PictureBox pictureBox15;
        private System.Windows.Forms.PictureBox pictureBox16;
        private System.Windows.Forms.PictureBox pictureBox17;
        private System.Windows.Forms.PictureBox pictureBox18;
        private System.Windows.Forms.PictureBox pictureBox19;
        private System.Windows.Forms.PictureBox pictureBox20;
        private System.Windows.Forms.PictureBox pictureBox22;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.ToolStripMenuItem screenshotPageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem screenshotPageToolStripMenuItem2;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.Button button7;
    }
}

