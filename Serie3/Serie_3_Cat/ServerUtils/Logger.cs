using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Serie_3_Cat
{
    #pragma warning disable 420
    public class Logger
    {
        public DateTime StartTime { get; private set; }
        public volatile TextWriter Writer;
        private volatile int _numRequests;
        private volatile Control _txtBox;
        private readonly string _logFileName;//TODO

        public Logger() : this(Console.Out) { }
        public Logger(string logfile) : this(new StreamWriter(new FileStream(logfile, FileMode.Append, FileAccess.Write))) { }
        public Logger(TextWriter awriter)
        {
            _numRequests = 0;
            Writer = awriter;
        }

        public void SetLoggerTextBox(TextBox txtBox)
        {
            //TODO
            //Control _txtWriterControl = new Control();
            //ControlPersister.PersistControl(writer, _txtWriterControl);

            Interlocked.Exchange(ref _txtBox, txtBox);
            var txbxWriter = new TextBoxStreamWriter(txtBox);
            Interlocked.Exchange(ref Writer, txbxWriter);
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            Writer.WriteLine();
            Writer.WriteLine(String.Format("::- LOG STARTED @ {0} -::", StartTime));
            Writer.WriteLine();   
        }

        public void LogMessage(string msg)
        {
            if (_txtBox!=null && _txtBox.InvokeRequired) // Se o writer pertence a um elemento Control que está noutra thread
            {
                _txtBox.BeginInvoke(new Action(() => LogMessage(msg)));
                return;
            }
            Writer.WriteLine(String.Format("{0}: {1}", DateTime.Now, msg));
        }

        public void IncrementRequests()
        {
            Interlocked.Increment(ref _numRequests);
        }

        public void Stop()
        {
            long elapsed = DateTime.Now.Ticks - StartTime.Ticks;
            Writer.WriteLine();
            LogMessage(String.Format("Running for {0} second(s)", elapsed / 10000000L));
            LogMessage(String.Format("Number of request(s): {0}", _numRequests));
            Writer.WriteLine();
            Writer.WriteLine(String.Format("::- LOG STOPPED @ {0} -::", DateTime.Now));
            Writer.Close();
        }
    }
}