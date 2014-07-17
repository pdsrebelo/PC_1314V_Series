using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ServerClientUtils
{
    public class TextBoxWriter : TextWriter
    {
        public Control TextBox { get; private set; }

        public TextBoxWriter(Control textBox)
        {
            TextBox = textBox;
        }

        public override void Write(char value)
        {
            TextBox.Text += value;
        }

        public override void Write(string value)
        {
            TextBox.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}