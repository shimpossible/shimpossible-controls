namespace Shimpossible.Controls
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.led1 = new Shimpossible.Controls.LED();
            this.circularGuage1 = new Shimpossible.Controls.Guage.CircularGuage();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 67);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Location = new System.Drawing.Point(12, 320);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(155, 54);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // led1
            // 
            this.led1.Location = new System.Drawing.Point(394, 247);
            this.led1.Name = "led1";
            this.led1.Size = new System.Drawing.Size(128, 128);
            this.led1.TabIndex = 6;
            this.led1.Text = "led1";
            // 
            // circularGuage1
            // 
            this.circularGuage1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.circularGuage1.Location = new System.Drawing.Point(12, 12);
            this.circularGuage1.Max = 100;
            this.circularGuage1.Min = 0;
            this.circularGuage1.Name = "circularGuage1";
            this.circularGuage1.Radius = 84F;
            this.circularGuage1.Size = new System.Drawing.Size(255, 229);
            this.circularGuage1.StartAngle = 135F;
            this.circularGuage1.SweepAngle = 270F;
            this.circularGuage1.TabIndex = 0;
            this.circularGuage1.Text = "circularGuage1";
            this.circularGuage1.Value = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(556, 417);
            this.Controls.Add(this.led1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.circularGuage1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Shimpossible.Controls.Guage.CircularGuage circularGuage1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private LED led1;
        private System.Windows.Forms.Timer timer1;
    }
}

