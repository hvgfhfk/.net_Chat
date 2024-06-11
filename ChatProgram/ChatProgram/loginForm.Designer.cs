namespace ChatProgram
{
    partial class loginForm
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
            emailText = new TextBox();
            passwordText = new TextBox();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // emailText
            // 
            emailText.Location = new Point(29, 22);
            emailText.Name = "emailText";
            emailText.Size = new Size(397, 39);
            emailText.TabIndex = 0;
            // 
            // passwordText
            // 
            passwordText.Location = new Point(29, 67);
            passwordText.Name = "passwordText";
            passwordText.Size = new Size(397, 39);
            passwordText.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(443, 22);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(150, 84);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // loginForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(606, 113);
            Controls.Add(btnLogin);
            Controls.Add(passwordText);
            Controls.Add(emailText);
            Name = "loginForm";
            Text = "loginForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox emailText;
        private TextBox passwordText;
        private Button btnLogin;
    }
}