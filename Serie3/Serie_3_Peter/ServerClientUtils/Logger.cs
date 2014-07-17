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

using System;
using System.IO;

namespace ServerClientUtils
{
	// Logger single-threaded.
	public class Logger
	{
		private readonly TextWriter _writer;
		public DateTime StartTime { get; private set; }
		private int _numRequests;

		public Logger() : this(Console.Out) {}
		
        public Logger(string logfile) : this(new StreamWriter(new FileStream(logfile, FileMode.Append, FileAccess.Write))) {}
		
        public Logger(TextWriter awriter)
		{
		    _numRequests = 0;
		    _writer = awriter;
		}

	    public void Start()
		{
			StartTime = DateTime.Now;
			_writer.WriteLine();
			_writer.WriteLine("::- LOG STARTED @ {0} -::", DateTime.Now);
			_writer.WriteLine();
		}

		public void LogMessage(string msg)
		{
		    if (_writer is TextBoxWriter)
		    {
                if ((_writer as TextBoxWriter).TextBox.InvokeRequired)
                    // This delegate enables asynchronous calls for setting the text property on a TextBox control. 
                    (_writer as TextBoxWriter).TextBox.BeginInvoke(new Action(() => LogMessage(msg)));
		        else
		            _writer.WriteLine("{0}: {1}", DateTime.Now, msg);
		    }
		    else
                _writer.WriteLine("{0}: {1}", DateTime.Now, msg);
		}

		public void IncrementRequests()
		{
			++_numRequests;
		}

		public void Stop()
		{
			long elapsed = DateTime.Now.Ticks - StartTime.Ticks;
			_writer.WriteLine();
			LogMessage(String.Format("Running for {0} second(s)", elapsed / 10000000L));
			LogMessage(String.Format("Number of request(s): {0}", _numRequests));
			_writer.WriteLine();
			_writer.WriteLine("::- LOG STOPPED @ {0} -::", DateTime.Now);
			_writer.Close();
		}
	}
}