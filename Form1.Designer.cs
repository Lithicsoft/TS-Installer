namespace Lithicsoft_Trainer_Studio_Installer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            button1 = new Button();
            button3 = new Button();
            progressBar1 = new ProgressBar();
            label3 = new Label();
            richTextBox1 = new RichTextBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Light", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Highlight;
            label1.Location = new Point(218, 12);
            label1.Name = "label1";
            label1.Size = new Size(482, 47);
            label1.TabIndex = 0;
            label1.Text = "Lithicsoft Trainer Studio Installer";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Traner_Studio;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(200, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Light", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.Highlight;
            label2.Location = new Point(218, 59);
            label2.Name = "label2";
            label2.Size = new Size(380, 30);
            label2.TabIndex = 2;
            label2.Text = "Copyright © 2024 Lithicsoft Organization.";
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.ForeColor = SystemColors.Highlight;
            button1.Location = new Point(713, 425);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Update";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button3
            // 
            button3.Enabled = false;
            button3.ForeColor = SystemColors.Highlight;
            button3.Location = new Point(580, 425);
            button3.Name = "button3";
            button3.Size = new Size(127, 23);
            button3.TabIndex = 6;
            button3.Text = "Install Trainer Studio";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 425);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(562, 23);
            progressBar1.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.Highlight;
            label3.Location = new Point(218, 197);
            label3.Name = "label3";
            label3.Size = new Size(16, 15);
            label3.TabIndex = 8;
            label3.Text = "...";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 218);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(776, 201);
            richTextBox1.TabIndex = 9;
            richTextBox1.Text = "";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.Highlight;
            label4.Location = new Point(717, 197);
            label4.Name = "label4";
            label4.Size = new Size(71, 15);
            label4.TabIndex = 10;
            label4.Text = "Change Log";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(800, 460);
            Controls.Add(label4);
            Controls.Add(richTextBox1);
            Controls.Add(label3);
            Controls.Add(progressBar1);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private Button button1;
        private Button button3;
        private ProgressBar progressBar1;
        private Label label3;
        private RichTextBox richTextBox1;
        private Label label4;
    }
}
