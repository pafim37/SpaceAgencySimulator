namespace Sas.GraphicRepresentation
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ScaleValue = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.ScaleValue);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.ClearButton);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 441);
            this.panel1.TabIndex = 0;
            // 
            // ScaleValue
            // 
            this.ScaleValue.AutoSize = true;
            this.ScaleValue.Location = new System.Drawing.Point(19, 200);
            this.ScaleValue.Name = "ScaleValue";
            this.ScaleValue.Size = new System.Drawing.Size(61, 15);
            this.ScaleValue.TabIndex = 3;
            this.ScaleValue.Text = "100000000";
            this.ScaleValue.Click += new System.EventHandler(this.label1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(16, 145);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 36);
            this.button3.TabIndex = 2;
            this.button3.Text = "Scale -";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 102);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "Scale +";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(15, 62);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(69, 34);
            this.ClearButton.TabIndex = 0;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Draw";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Right;
            this.canvas.Location = new System.Drawing.Point(97, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(527, 441);
            this.canvas.TabIndex = 1;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel canvas;
        private Button button1;
        private Button ClearButton;
        private Button button2;
        private Label ScaleValue;
        private Button button3;
    }
}