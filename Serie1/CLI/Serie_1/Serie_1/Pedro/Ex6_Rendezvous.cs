using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_1.Pedro
{
    /// <summary>
    /// Implemente em Java e em C# o sincronizador rendezvous channel, com base na classe genérica RendezvousChannel<S,R>.
    /// O rendezvous channel serve para sincronizar a comunicação entre threads cliente e threads servidoras.
    /// 
    /// Na resolução deste exercício procure minimizar as comutações de thread, usando as técnicas que foram discutidas nas aulas teóricas. 
    /// Note que a implementação em Java exigirá alterações à interface pública da classe.
    /// </summary>
    class Ex6_Rendezvous<S,R>
    {

        LinkedList<S> _clientRequests = new LinkedList<S>(); 

        /// <summary>
        /// As threads cliente realizam pedidos de serviço invocando o método bool Request(S service, int timeout, out R response). 
        /// O objecto, do tipo S, passado através do parâmetro service descreve o pedido de serviço. Se o serviço for executado com
        /// sucesso por uma thread servidora, este método devolve true e a resposta ao pedido de serviço (um objecto do tipo R) 
        /// é passada através do parâmetro response; se o pedido de serviço não for aceite de imediato, por não existir nenhuma thread 
        /// servidora disponível, a thread cliente fica bloqueada até que o pedido de serviço seja aceite, a thread cliente seja interrompida,
        /// ou expire o limite de tempo especificado através do parâmetro timeout. Note que quando existe desistência o método Request 
        /// devolve false. Dado que não está prevista nenhuma forma de interromper o processamento de um pedido de serviço já aceite
        /// por uma thread servidora, as threads cliente não poderão desistir, devido a interrupção ou timeout, após terem iniciado o 
        /// rendezvous com uma thread servidora, devendo esperar incondicionalmente que o serviço seja concluído, isto é, a thread servidora 
        /// invoque o método Reply, com o respectivo rendezvous token.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="timeout"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool Request(S service, int timeout, out R response)
        {




            response = default(R);
            return false;
        }

        /// <summary>
        /// Sempre que uma thread servidora estiver em condições de processar pedidos de serviço, invoca o método 
        /// object Accept (int timeout, out S service). Quando um pedido de serviço é aceite, a descrição do pedido de serviço é 
        /// passado através do parâmetro de saída service e o método Accept devolve também um rendezvous token (i.e., um objecto opaco,
        /// cujo tipo é definido pela implementação) para identificar um rendezvous particular. Quando não existe nenhum pedido de 
        /// serviço pendente, a thread servidora fica bloqueada até que seja solicitado um pedido de serviço, seja interrompida ou expire
        /// o limite de tempo especificado através do parâmetro timeout. (Este método deve devolver null como rendezvous token para indicar
        /// que a thread servidora retornou por desistência.)
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public object Accept(int timeout, out S service)
        {
            service = default(S);
            return null;
        }

        /// <summary>
        /// Quando uma thread servidora quer indicar a conclusão de um serviço particular (definido pelo respectivo rendezvous token) e 
        /// devolver o respectivo resultado, invoca o método void Reply(object rendezVousToken, R response). Através do primeiro parâmetro é 
        /// passada a identificação do rendezvous e através do parâmetro responseo objecto do tipo R, que contém a resposta ao pedido de serviço.
        /// </summary>
        /// <param name="rendezvousToken"></param>
        /// <param name="response"></param>
        public void Reply(object rendezvousToken, R response)
        {
            
        }
    }
}
