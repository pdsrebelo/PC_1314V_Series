using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie_1.Catia
{
    /*
     * Implemente em Java e em C# o sincronizador rendezvous channel, com base na classe genérica 
        RendezvousChannel<S,R>. 
     * 
     * O rendezvous channel serve para sincronizar a comunicação entre 
        threads cliente e threads servidoras, com a seguinte semântica: 
       
     * • As threads cliente realizam pedidos de serviço invocando o método bool Request(S 
        service, int timeout, out R response). 
     *  O objecto, do tipo S, passado através do 
        parâmetro service descreve o pedido de serviço. Se o serviço for executado com sucesso por 
        uma thread servidora, este método devolve true e a resposta ao pedido de serviço (um objecto do 
        tipo R) é passada através do parâmetro response; se o pedido de serviço não for aceite de 
        imediato, por não existir nenhuma thread servidora disponível, a thread cliente fica bloqueada até 
        que o pedido de serviço seja aceite, a thread cliente seja interrompida, ou expire o limite de tempo 
        especificado através do parâmetro timeout. 
     * 
     * Note que quando existe desistência o método 
        Request devolve false. Dado que não está prevista nenhuma forma de interromper o 
        processamento de um pedido de serviço já aceite por uma thread servidora, as threads cliente não 
        poderão desistir, devido a interrupção ou timeout, após terem iniciado o rendezvous com uma 
        thread servidora, devendo esperar incondicionalmente que o serviço seja concluído, isto é, a 
        thread servidora invoque o método Reply, com o respectivo rendezvous token. 
     * 
     * 
        • Sempre que uma thread servidora estiver em condições de processar pedidos de serviço, invoca o 
        método object Accept (int timeout, out S service). Quando um pedido de serviço é 
        aceite, a descrição do pedido de serviço é passado através do parâmetro de saída service e o 
        método Accept devolve também um rendezvous token (i.e., um objecto opaco, cujo tipo é 
        definido pela implementação) para identificar um rendezvous particular. 
     * 
     * Quando não existe 
        nenhum pedido de serviço pendente, a thread servidora fica bloqueada até que seja solicitado um 
        pedido de serviço, seja interrompida ou expire o limite de tempo especificado através do 
        parâmetro timeout. (Este método deve devolver null como rendezvous token para indicar que a 
        thread servidora retornou por desistência.) 
       
     * 
     * • Quando uma thread servidora quer indicar a conclusão de um serviço particular (definido pelo 
        respectivo rendezvous token) e devolver o respectivo resultado, invoca o método void 
        Reply(object rendezVousToken, R response). Através do primeiro parâmetro é passada 
        a identificação do rendezvous e através do parâmetro response o objecto do tipo R, que contém a 
        resposta ao pedido de serviço. 
     * 
        Na resolução deste exercício procure minimizar as comutações de thread, usando as técnicas que 
        foram discutidas nas aulas teóricas. 
        Note que a implementação em Java exigirá alterações à interface pública da classe. 
        */

    public class Ex6RendezvousChannel<S,R>
    {

        private class ClientRequest // Objectos sobre os quais se vai fazer pulse e wait!!
        {
            public S serviceRequested { get; set; }
            public bool isBeingProcessed { get; set; }
            public R response { get; set; }

            public ClientRequest(S service)
            {
                serviceRequested = service;
                isBeingProcessed = false;
                response = default(R);
            }
        }

        private LinkedList<ClientRequest> clientServiceRequests; 

        // Threads cliente chamam Request para fazer pedidos de serviço - é bloqueante e tem desistência por timeout ou interrupção
        public bool Request(S service, int timeout, out R response)
        {
            bool success = false;
            response = default(R);
            lock (clientServiceRequests)
            {
                ClientRequest request = new ClientRequest(service);
                
                clientServiceRequests.AddLast(request);
                
                // Notificar thread servidora (se houver) que a lista de pedidos foi modificada
                Monitor.Pulse(clientServiceRequests);

                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
                do
                {
                    // Verificar estado do pedido
                    if (request.isBeingProcessed && request.response!=null)
                    {
                        // Pedido já foi atendido e já há resposta!
                        response = request.response;
                        success = true;
                        break;
                    }
                    
                    try
                    {
                        Monitor.Wait(request);
                    }
                    catch (ThreadInterruptedException iex)
                    {
                        // Verificar se o pedido foi concluído, apesar da interrupção
                        if (request.isBeingProcessed && request.response != null)
                        {
                            // Se já foi atendido e já há resposta, retornar como sucesso
                            response = request.response;
                            return true;
                        }
                        // Senão, remover
                        clientServiceRequests.Remove(request);
                        throw;
                    }
                } while ((timeout = SyncUtils.AdjustTimeout(ref lastTime, ref timeout))>0);
            }
            return success;
        }

        // A invocar por uma thread servidora sempre que esteja em condições de satisfazer um pedido
        /*
         * Quando não existe 
        nenhum pedido de serviço pendente, a thread servidora fica bloqueada até que seja solicitado um 
        pedido de serviço, seja interrompida ou expire o limite de tempo especificado através do 
        parâmetro timeout.*/
        public object Accept(int timeout, out S service)
        {
            ClientRequest clientRequest = null;
            lock (clientServiceRequests)
            {
                service = default(S);
                
                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0; // Get the current time
                do
                {
                    // Se há pedidos para atender.. Vai buscar o 1º, para passar como parâmetro de out a sua descrição
                    foreach (ClientRequest req in clientServiceRequests)
                    {
                        if (req.isBeingProcessed) continue;
                        // Se foi encontrado um pedido que ainda não está associado a nenhuma thread servidora...
                        req.isBeingProcessed = true;
                        clientRequest = req;
                        break;
                    }
                    if (clientRequest != null)
                    {
                        Monitor.Pulse(clientRequest); // Notificar o objecto (o pedido de um cliente sofreu alterações)
                        service = clientRequest.serviceRequested;
                        break;
                    }
                    try
                    {
                        // Esperar que haja notificação que indique que a lista de pedidos foi modificada
                        Monitor.Wait(clientServiceRequests);
                    }
                    catch (ThreadInterruptedException iex)
                    {
                        Monitor.PulseAll(clientServiceRequests);
                        throw;
                    }
                } while ((timeout = SyncUtils.AdjustTimeout(ref lastTime, ref timeout))> 0);
            }
            return clientRequest; // Devolve o rendezvous token
        }

        public void Reply(object rendezVousToken, R response)
        {
            lock (rendezVousToken)
            {
                ((ClientRequest) rendezVousToken).response = response;
                Monitor.Pulse(rendezVousToken);
            }
        }

        // Usado para notificação específica
        // Serve para adquirir o lock de um objecto.
        // "informa" se a thread foi interrompida, ao tentar adquirir o lock
        private static void EnterUninterruptibly(object mlock, out bool interrupted)
        {
            interrupted = false;
            do
            {
                try
                {
                    Monitor.Enter(mlock);
                    break;
                }
                catch (ThreadInterruptedException iex)
                {
                    interrupted = true;
                }
            } while (true);
        }


    }
}
