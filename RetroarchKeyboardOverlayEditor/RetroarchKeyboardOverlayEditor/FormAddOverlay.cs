using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RetroarchKeyboardOverlayEditor
{
    class FormAddOverlay : Form
    {
        public string returnText { get; private set; }

        IContainer components = null;
        Button buttonOK;
        TextBox textBox;
        Label label;

        public FormAddOverlay(string defaultText = "")
        {
            returnText = defaultText;
            buttonOK = new Button();
            textBox = new TextBox();
            label = new Label();
            SuspendLayout();

            buttonOK.Location = new Point(137, 26);
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 1;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += new EventHandler(buttonOK_Click);

            textBox.Location = new Point(12, 28);
            textBox.Size = new Size(119, 20);
            textBox.Text = returnText;
            textBox.TabIndex = 0;
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);

            label.AutoSize = true;
            label.Location = new Point(10, 9);
            label.Size = new Size(95, 13);
            label.TabIndex = 2;
            label.Text = "New overlay name";

            ClientSize = new Size(224, 62);
            Controls.Add(label);
            Controls.Add(textBox);
            Controls.Add(buttonOK);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "FormAddOverlay";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add overlay";
            ResumeLayout(false);
            PerformLayout();
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonOK.PerformClick();
        }

        void buttonOK_Click(object sender, EventArgs e)
        {
            returnText = textBox.Text;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}
