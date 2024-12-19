namespace SecureChat
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox chatBox;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Button sendButton;

        private void InitializeComponent()
        {
            this.chatBox = new System.Windows.Forms.TextBox();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();

            // chatBox
            this.chatBox.Multiline = true;
            this.chatBox.Location = new System.Drawing.Point(12, 12);
            this.chatBox.Size = new System.Drawing.Size(360, 200);
            this.chatBox.ReadOnly = true;

            // inputBox
            this.inputBox.Location = new System.Drawing.Point(12, 220);
            this.inputBox.Size = new System.Drawing.Size(270, 23);

            // sendButton
            this.sendButton.Location = new System.Drawing.Point(290, 220);
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
           
        }
    }
}
