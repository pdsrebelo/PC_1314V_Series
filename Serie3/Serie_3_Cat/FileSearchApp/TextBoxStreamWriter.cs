using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Serie_3_Cat
{
    public class TextBoxStreamWriter : TextWriter
    {
        public TextBox TextBox { get; private set; }
        private Boolean _enabled;

        public TextBoxStreamWriter(TextBox textBox)
        {
            _enabled = true;
            TextBox = textBox;
        }

        public override void Write(char value)
        {
            if (!_enabled) return;      
            base.Write(value);
            if (TextBox.InvokeRequired)
            {
                //TODO verificar se ainda da erro!!
                TextBox.BeginInvoke(new Action(() => TextBox.AppendText(value.ToString())));//LogMessage(msg)));
                return;
            }
            TextBox.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }

        public override void Close()
        {
            base.Close();
            _enabled = false;
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
