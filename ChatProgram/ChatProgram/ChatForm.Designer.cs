namespace ChatProgram
{
    partial class ChatForm
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
            button1 = new Button();
            txtMessage = new TextBox();
            listView1 = new ListBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 751);
            button1.Name = "button1";
            button1.Size = new Size(1216, 91);
            button1.TabIndex = 0;
            button1.Text = "메시지 보내기";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(12, 706);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(1216, 39);
            txtMessage.TabIndex = 2;
            // 
            // listView1
            // 
            listView1.FormattingEnabled = true;
            listView1.Location = new Point(12, 12);
            listView1.Name = "listView1";
            listView1.Size = new Size(1216, 676);
            listView1.TabIndex = 3;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1240, 854);
            Controls.Add(listView1);
            Controls.Add(txtMessage);
            Controls.Add(button1);
            Name = "ChatForm";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox txtMessage;
        private ListBox listView1;
    }
}
