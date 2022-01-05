namespace Sas.GraphicRepresentation
{
    partial class RootWindow
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
            this.menu = new System.Windows.Forms.Panel();
            this.scaleValue = new System.Windows.Forms.Label();
            this.scaleMinus = new System.Windows.Forms.Button();
            this.scalePlus = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.board = new System.Windows.Forms.Panel();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menu.Controls.Add(this.scaleValue);
            this.menu.Controls.Add(this.scaleMinus);
            this.menu.Controls.Add(this.scalePlus);
            this.menu.Controls.Add(this.clearButton);
            this.menu.Controls.Add(this.button1);
            this.menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(147, 480);
            this.menu.TabIndex = 0;
            // 
            // scaleValue
            // 
            this.scaleValue.AutoSize = true;
            this.scaleValue.Location = new System.Drawing.Point(19, 200);
            this.scaleValue.Name = "scaleValue";
            this.scaleValue.Size = new System.Drawing.Size(61, 15);
            this.scaleValue.TabIndex = 3;
            this.scaleValue.Text = "0";
            // 
            // scaleMinus
            // 
            this.scaleMinus.Location = new System.Drawing.Point(16, 145);
            this.scaleMinus.Name = "scaleMinus";
            this.scaleMinus.Size = new System.Drawing.Size(68, 36);
            this.scaleMinus.TabIndex = 2;
            this.scaleMinus.Text = "Scale -";
            this.scaleMinus.UseVisualStyleBackColor = true;
            this.scaleMinus.Click += new System.EventHandler(this.scaleMinus_Click);
            // 
            // scalePlus
            // 
            this.scalePlus.Location = new System.Drawing.Point(15, 102);
            this.scalePlus.Name = "scalePlus";
            this.scalePlus.Size = new System.Drawing.Size(69, 37);
            this.scalePlus.TabIndex = 1;
            this.scalePlus.Text = "Scale +";
            this.scalePlus.UseVisualStyleBackColor = true;
            this.scalePlus.Click += new System.EventHandler(this.scalePlus_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(15, 62);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(69, 34);
            this.clearButton.TabIndex = 0;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
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
            // board
            // 
            this.board.Dock = System.Windows.Forms.DockStyle.Right;
            this.board.Location = new System.Drawing.Point(144, 0);
            this.board.Name = "board";
            this.board.Size = new System.Drawing.Size(640, 480);
            this.board.TabIndex = 1;
            this.board.Paint += new System.Windows.Forms.PaintEventHandler(this.board_Paint);
            // 
            // RootWindow
            // 
            this.ClientSize = new System.Drawing.Size(784, 480);
            this.Controls.Add(this.board);
            this.Controls.Add(this.menu);
            this.Name = "RootWindow";
            this.Load += new System.EventHandler(this.RootWindow_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel menu;
        private Panel board;
        private Button button1;
        private Button clearButton;
        private Button scalePlus;
        private Label scaleValue;
        private Button scaleMinus;

    }
}