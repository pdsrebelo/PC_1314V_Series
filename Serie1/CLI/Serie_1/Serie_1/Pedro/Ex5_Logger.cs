using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_1.Pedro
{
    /// <summary>
    /// Implemente em C# a classe Logger que fornece funcionalidade de registo de relatórios suportada 
    /// por uma thread de baixa prioridade criada para o efeito (logger thread). As mensagens com os 
    /// relatórios são passadas por threads que chamam a operação LogMessage(string msg) e são escritas 
    /// pela logger thread na instância de TextWriter especificada como argumento no construtor de Logger. 
    /// A classe inclui ainda as operações Start e Shutdown que promovem o início e a terminação do 
    /// registo de relatórios, respectivamente. A operação Shutdown bloqueia a thread invocante até que 
    /// todos os relatórios submetidos até ao momento sejam efectivamente escritos; relatórios submetidos 
    /// posteriormente são rejeitados (i.e. chamadas a LogMessage produzem excepção).
    /// 
    /// Note que a preocupação principal da solução a apresentar deverá ser o de minimizar o custo do 
    /// registo de relatórios por threads cuja prioridade se admite maior que a prioridade da logger thread. 
    /// Pretende-se, por isso, que seja usado um mecanismo de comunicação que minimize o tempo de 
    /// bloqueio das threads produtoras, admitindo-se inclusivamente a possibilidade de ignorar relatórios.
    /// </summary>
    class Ex5Logger
    {
        public Ex5Logger ()
        {
            
        }

        public void LogMessage(string msg)
        {
            
        }

        public void Start()
        {
            
        }

        public void Shutdown()
        {
            
        }

    }
}
