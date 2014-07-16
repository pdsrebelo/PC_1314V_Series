using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * INSTITUTO SUPERIOR DE ENGENHARIA DE LISBOA
 * Licenciatura em Engenharia Informática e de Computadores
 *
 * Programação Concorrente - Inverno de 2009-2010
 * João Trindade
 *
 * Código base para a 3ª Série de Exercícios.
 *
 */
using System.IO;
using System.Threading;
using System.Web.UI.Design;
using System.Windows.Forms;


namespace Serie_3_Cat
{
    // Logger single-threaded.
    public class Logger
    {
        private TextWriter writer;
        public DateTime start_time { get; private set; }
        private volatile int num_requests;
        private string fileName;//TODO
        private Control _txtBox;

        public Logger() : this(Console.Out) { }
        public Logger(string logfile) : this(new StreamWriter(new FileStream(logfile, FileMode.Append, FileAccess.Write))) { }
        public Logger(TextWriter awriter)
        {
            num_requests = 0;
            writer = awriter;
        }

        public void SetLoggerTextBox(TextBox ctrl)
        {
            //TODO
            //Control _txtWriterControl = new Control();
            //ControlPersister.PersistControl(writer, _txtWriterControl);
            
            _txtBox = ctrl;
            writer = new TextBoxStreamWriter(ctrl);
        }

        public void Start()
        {
            start_time = DateTime.Now;
            writer.WriteLine();
            writer.WriteLine(String.Format("::- LOG STARTED @ {0} -::", DateTime.Now));
            writer.WriteLine();
        }

        public void LogMessage(string msg)
        {
            if (_txtBox!=null && _txtBox.InvokeRequired) // Se o writer pertence a um elemento Control que está noutra thread
            {
                _txtBox.BeginInvoke(new Action(() => LogMessage(msg)));
                return;
            }
            writer.WriteLine(String.Format("{0}: {1}", DateTime.Now, msg));
        }

        public void IncrementRequests()
        {
            Interlocked.Increment(ref num_requests); //++num_requests;
        }

        public void Stop()
        {
            long elapsed = DateTime.Now.Ticks - start_time.Ticks;
            writer.WriteLine();
            LogMessage(String.Format("Running for {0} second(s)", elapsed / 10000000L));
            LogMessage(String.Format("Number of request(s): {0}", num_requests));
            writer.WriteLine();
            writer.WriteLine(String.Format("::- LOG STOPPED @ {0} -::", DateTime.Now));
            writer.Close();
        }
    }
}