using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_1.Pedro
{
    /// <summary>
    /// Implemente em C# a classe Logger que fornece funcionalidade de registo de relatórios suportada 
    /// por uma thread de baixa prioridade criada para o efeito (logger thread). 
    /// 
    /// Note que a preocupação principal da solução a apresentar deverá ser o de minimizar o custo do 
    /// registo de relatórios por threads cuja prioridade se admite maior que a prioridade da logger thread. 
    /// Pretende-se, por isso, que seja usado um mecanismo de comunicação que minimize o tempo de 
    /// bloqueio das threads produtoras, admitindo-se inclusivamente a possibilidade de ignorar relatórios.
    /// </summary>
    class Ex5Logger
    {
        private Thread _loggerThread;

        public Ex5Logger (TextWriter tw)
        {
//            _loggerThread = new Thread();
//            tw = File.Create(@"C:\Users\Pedro Rebelo\Desktop\Logger.txt");
        }

        /// <summary>
        /// As mensagens com os relatórios são passadas por threads que chamam a operação LogMessage(string msg) e 
        /// são escritas pela logger thread na instância de TextWriter especificada como argumento no construtor de Logger. 
        /// </summary>
        /// <param name="msg"></param>
        public void LogMessage(string msg)
        {
            
        }

        /// <summary>
        /// A classe inclui ainda as operações Start e Shutdown que promovem o início e a terminação do 
        /// registo de relatórios, respectivamente.
        /// </summary>
        public void Start()
        {
            
        }

        /// <summary>
        /// A operação Shutdown bloqueia a thread invocante até que todos os relatórios submetidos até ao momento sejam efectivamente 
        /// escritos; relatórios submetidos posteriormente são rejeitados (i.e. chamadas a LogMessage produzem excepção).
        /// </summary>
        public void Shutdown()
        {
            
        }

    }
}
