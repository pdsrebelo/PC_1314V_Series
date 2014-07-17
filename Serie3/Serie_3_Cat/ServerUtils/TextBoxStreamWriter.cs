using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Serie_3_Cat
{
    public class TextBoxStreamWriter : TextWriter
    {
        private readonly TextBox _output;
        private Boolean _enabled;

        public TextBoxStreamWriter(TextBox output)
        {
            _enabled = true;
            _output = output;
        }

        public override void Write(char value)
        {
            if (!_enabled) return;
            base.Write(value);
            _output.AppendText(value.ToString()); // When character data is written, append it to the text box.
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
