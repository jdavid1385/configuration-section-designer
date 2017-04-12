namespace Samples
{
    partial class MainTestForm
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
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.ClearConsoleButton = new System.Windows.Forms.Button();
            this.DemoSelectComboBox = new System.Windows.Forms.ComboBox();
            this.RunDemoButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Location = new System.Drawing.Point(12, 65);
            this.ConsoleTextBox.Multiline = true;
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleTextBox.Size = new System.Drawing.Size(645, 372);
            this.ConsoleTextBox.TabIndex = 0;
            // 
            // ClearConsoleButton
            // 
            this.ClearConsoleButton.Location = new System.Drawing.Point(597, 443);
            this.ClearConsoleButton.Name = "ClearConsoleButton";
            this.ClearConsoleButton.Size = new System.Drawing.Size(60, 41);
            this.ClearConsoleButton.TabIndex = 3;
            this.ClearConsoleButton.Text = "Clear";
            this.ClearConsoleButton.UseVisualStyleBackColor = true;
            this.ClearConsoleButton.Click += new System.EventHandler(this.ClearConsoleButton_Click);
            // 
            // DemoSelectComboBox
            // 
            this.DemoSelectComboBox.FormattingEnabled = true;
            this.DemoSelectComboBox.Items.AddRange(new object[] {
            "Sample Demo",
            "School Demo",
            "Basic Demo - Config File",
            "Sample Demo - Config File",
            "School Demo - Config File"});
            this.DemoSelectComboBox.Location = new System.Drawing.Point(13, 463);
            this.DemoSelectComboBox.Name = "DemoSelectComboBox";
            this.DemoSelectComboBox.Size = new System.Drawing.Size(215, 21);
            this.DemoSelectComboBox.TabIndex = 4;
            // 
            // RunDemoButton
            // 
            this.RunDemoButton.Location = new System.Drawing.Point(234, 446);
            this.RunDemoButton.Name = "RunDemoButton";
            this.RunDemoButton.Size = new System.Drawing.Size(137, 41);
            this.RunDemoButton.TabIndex = 5;
            this.RunDemoButton.Text = "Run Demo";
            this.RunDemoButton.UseVisualStyleBackColor = true;
            this.RunDemoButton.Click += new System.EventHandler(this.RunDemoButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 444);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select a demo to run:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Console";
            // 
            // MainTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 499);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RunDemoButton);
            this.Controls.Add(this.DemoSelectComboBox);
            this.Controls.Add(this.ClearConsoleButton);
            this.Controls.Add(this.ConsoleTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainTestForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainTestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.Button ClearConsoleButton;
        private System.Windows.Forms.ComboBox DemoSelectComboBox;
        private System.Windows.Forms.Button RunDemoButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

