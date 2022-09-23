namespace App
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ApplyStation = new System.Windows.Forms.Button();
            this.StationSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TeamNumber = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EnableButton = new System.Windows.Forms.Button();
            this.EStopButton = new System.Windows.Forms.Button();
            this.DisableButton = new System.Windows.Forms.Button();
            this.ModeSelect = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.StatusDSComms = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.StatusBatt = new System.Windows.Forms.Label();
            this.StatusMode = new System.Windows.Forms.Label();
            this.StatusEStopped = new System.Windows.Forms.Label();
            this.StatusEnabled = new System.Windows.Forms.Label();
            this.StatusComms = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Team Number:";
            this.label1.UseWaitCursor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ApplyStation);
            this.groupBox1.Controls.Add(this.StationSelect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TeamNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Station Options";
            this.groupBox1.UseWaitCursor = true;
            // 
            // ApplyStation
            // 
            this.ApplyStation.Location = new System.Drawing.Point(248, 103);
            this.ApplyStation.Name = "ApplyStation";
            this.ApplyStation.Size = new System.Drawing.Size(75, 23);
            this.ApplyStation.TabIndex = 4;
            this.ApplyStation.Text = "Apply";
            this.ApplyStation.UseVisualStyleBackColor = true;
            this.ApplyStation.UseWaitCursor = true;
            this.ApplyStation.Click += new System.EventHandler(this.ApplyStation_Click);
            // 
            // StationSelect
            // 
            this.StationSelect.FormattingEnabled = true;
            this.StationSelect.Items.AddRange(new object[] {
            "Red 1",
            "Red 2",
            "Red 3",
            "Blue 1",
            "Blue 2",
            "Blue 3"});
            this.StationSelect.Location = new System.Drawing.Point(97, 45);
            this.StationSelect.Name = "StationSelect";
            this.StationSelect.Size = new System.Drawing.Size(121, 23);
            this.StationSelect.TabIndex = 3;
            this.StationSelect.UseWaitCursor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Station:";
            this.label2.UseWaitCursor = true;
            // 
            // TeamNumber
            // 
            this.TeamNumber.Location = new System.Drawing.Point(97, 16);
            this.TeamNumber.Name = "TeamNumber";
            this.TeamNumber.Size = new System.Drawing.Size(153, 23);
            this.TeamNumber.TabIndex = 1;
            this.TeamNumber.UseWaitCursor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.ModeSelect);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(329, 98);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Control";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.EnableButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.EStopButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.DisableButton, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 51);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(329, 31);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // EnableButton
            // 
            this.EnableButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnableButton.Location = new System.Drawing.Point(3, 3);
            this.EnableButton.Name = "EnableButton";
            this.EnableButton.Size = new System.Drawing.Size(76, 25);
            this.EnableButton.TabIndex = 5;
            this.EnableButton.Text = "Enable";
            this.EnableButton.UseVisualStyleBackColor = true;
            this.EnableButton.UseWaitCursor = true;
            this.EnableButton.Click += new System.EventHandler(this.EnableButton_Click);
            // 
            // EStopButton
            // 
            this.EStopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EStopButton.Location = new System.Drawing.Point(249, 3);
            this.EStopButton.Name = "EStopButton";
            this.EStopButton.Size = new System.Drawing.Size(77, 25);
            this.EStopButton.TabIndex = 7;
            this.EStopButton.Text = "E-Stop";
            this.EStopButton.UseVisualStyleBackColor = true;
            this.EStopButton.UseWaitCursor = true;
            this.EStopButton.Click += new System.EventHandler(this.EStopButton_Click);
            // 
            // DisableButton
            // 
            this.DisableButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisableButton.Location = new System.Drawing.Point(85, 3);
            this.DisableButton.Name = "DisableButton";
            this.DisableButton.Size = new System.Drawing.Size(158, 25);
            this.DisableButton.TabIndex = 8;
            this.DisableButton.Text = "Disable";
            this.DisableButton.UseVisualStyleBackColor = true;
            this.DisableButton.UseWaitCursor = true;
            this.DisableButton.Click += new System.EventHandler(this.DisableButton_Click);
            // 
            // ModeSelect
            // 
            this.ModeSelect.FormattingEnabled = true;
            this.ModeSelect.Items.AddRange(new object[] {
            "Tele",
            "Auto",
            "Test"});
            this.ModeSelect.Location = new System.Drawing.Point(97, 22);
            this.ModeSelect.Name = "ModeSelect";
            this.ModeSelect.Size = new System.Drawing.Size(121, 23);
            this.ModeSelect.TabIndex = 6;
            this.ModeSelect.UseWaitCursor = true;
            this.ModeSelect.SelectedIndexChanged += new System.EventHandler(this.ModeSelect_Changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Mode:";
            this.label3.UseWaitCursor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.StatusDSComms);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.StatusBatt);
            this.groupBox3.Controls.Add(this.StatusMode);
            this.groupBox3.Controls.Add(this.StatusEStopped);
            this.groupBox3.Controls.Add(this.StatusEnabled);
            this.groupBox3.Controls.Add(this.StatusComms);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(347, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 236);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Station Status";
            // 
            // StatusDSComms
            // 
            this.StatusDSComms.AutoSize = true;
            this.StatusDSComms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.StatusDSComms.Location = new System.Drawing.Point(99, 19);
            this.StatusDSComms.Name = "StatusDSComms";
            this.StatusDSComms.Size = new System.Drawing.Size(16, 15);
            this.StatusDSComms.TabIndex = 13;
            this.StatusDSComms.Text = "...";
            this.StatusDSComms.UseWaitCursor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 15);
            this.label9.TabIndex = 12;
            this.label9.Text = "Station Comms:";
            this.label9.UseWaitCursor = true;
            // 
            // StatusBatt
            // 
            this.StatusBatt.AutoSize = true;
            this.StatusBatt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.StatusBatt.Location = new System.Drawing.Point(99, 154);
            this.StatusBatt.Name = "StatusBatt";
            this.StatusBatt.Size = new System.Drawing.Size(16, 15);
            this.StatusBatt.TabIndex = 11;
            this.StatusBatt.Text = "...";
            this.StatusBatt.UseWaitCursor = true;
            // 
            // StatusMode
            // 
            this.StatusMode.AutoSize = true;
            this.StatusMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.StatusMode.Location = new System.Drawing.Point(99, 127);
            this.StatusMode.Name = "StatusMode";
            this.StatusMode.Size = new System.Drawing.Size(16, 15);
            this.StatusMode.TabIndex = 10;
            this.StatusMode.Text = "...";
            this.StatusMode.UseWaitCursor = true;
            // 
            // StatusEStopped
            // 
            this.StatusEStopped.AutoSize = true;
            this.StatusEStopped.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.StatusEStopped.Location = new System.Drawing.Point(99, 100);
            this.StatusEStopped.Name = "StatusEStopped";
            this.StatusEStopped.Size = new System.Drawing.Size(16, 15);
            this.StatusEStopped.TabIndex = 9;
            this.StatusEStopped.Text = "...";
            this.StatusEStopped.UseWaitCursor = true;
            // 
            // StatusEnabled
            // 
            this.StatusEnabled.AutoSize = true;
            this.StatusEnabled.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.StatusEnabled.Location = new System.Drawing.Point(99, 74);
            this.StatusEnabled.Name = "StatusEnabled";
            this.StatusEnabled.Size = new System.Drawing.Size(16, 15);
            this.StatusEnabled.TabIndex = 8;
            this.StatusEnabled.Text = "...";
            this.StatusEnabled.UseWaitCursor = true;
            // 
            // StatusComms
            // 
            this.StatusComms.AutoSize = true;
            this.StatusComms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.StatusComms.Location = new System.Drawing.Point(99, 48);
            this.StatusComms.Name = "StatusComms";
            this.StatusComms.Size = new System.Drawing.Size(16, 15);
            this.StatusComms.TabIndex = 7;
            this.StatusComms.Text = "...";
            this.StatusComms.UseWaitCursor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(49, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 15);
            this.label8.TabIndex = 6;
            this.label8.Text = "Battery";
            this.label8.UseWaitCursor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(55, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Mode";
            this.label7.UseWaitCursor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "E-Stopped";
            this.label6.UseWaitCursor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Enabled:";
            this.label5.UseWaitCursor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Robot Comms:";
            this.label4.UseWaitCursor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 260);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private GroupBox groupBox1;
        private TextBox TeamNumber;
        private Label label2;
        private ComboBox StationSelect;
        private Button ApplyStation;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private ComboBox ModeSelect;
        private Label label3;
        private Button DisableButton;
        private Button EStopButton;
        private Button EnableButton;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label StatusBatt;
        private Label StatusMode;
        private Label StatusEStopped;
        private Label StatusEnabled;
        private Label StatusComms;
        private Label StatusDSComms;
        private Label label9;
    }
}