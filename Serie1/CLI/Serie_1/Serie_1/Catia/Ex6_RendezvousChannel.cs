using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Serie_1.Catia
{
    public class Ex6RendezvousChannel<TS,TR>
    {
        private class ClientRequest
        {
            public TS ServiceRequested { get; set; }
            public bool IsBeingProcessed { get; set; }
            public TR Response { get; set; }

            public ClientRequest(TS service)
            {
                ServiceRequested = service;
                IsBeingProcessed = false;
                Response = default(TR);
            }
        }

        private readonly LinkedList<ClientRequest> _clientServiceRequests;

        public Ex6RendezvousChannel()
        {
            _clientServiceRequests = new LinkedList<ClientRequest>();
        }

        // Threads cliente chamam Request para fazer pedidos de serviço - é bloqueante e tem desistência por timeout ou interrupção
        public bool Request(TS service, int timeout, out TR response)
        {
            var success = false;
            response = default(TR);
            lock (this)
            {
                var request = new ClientRequest(service);
                _clientServiceRequests.AddLast(request);
                
                // Notificar threads servidoras (podem existir ou não) que a lista de pedidos foi modificada
                SyncUtils.Broadcast(this, _clientServiceRequests);

                var lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
                do
                {
                    // Verificar estado do pedido
                    if (request.IsBeingProcessed && !request.Response.Equals(default(TR)))//TODO Se tem lá alguma coisa
                    {
                        // Pedido já foi atendido e já há resposta!
                        response = request.Response;
                        success = true;
                        break;
                    }
                    
                    try
                    {
                        SyncUtils.Wait(this, request); // Em vez de "Monitor.Wait(request);"
                    }
                    catch (ThreadInterruptedException)
                    {
                        // Verificar se o pedido foi concluído, apesar da interrupção
                        if (request.IsBeingProcessed && !request.Response.Equals(default(TR))) // TODO - Ver se "request" tem alguma coisa
                        {
                            // Se já foi atendido e já há resposta, retornar como sucesso
                            response = request.Response;
                            return true;
                        }
                        // Senão, remover
                        _clientServiceRequests.Remove(request);
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
        public object Accept(int timeout, out TS service)
        {
            ClientRequest clientRequest = null;
            lock (_clientServiceRequests)
            {
                service = default(TS);
                
                var lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0; // Get the current time
                do
                {
                    // Se há pedidos para atender.. Vai buscar o 1º, para passar como parâmetro de out a sua descrição
                    foreach (var req in _clientServiceRequests.Where(req => !req.IsBeingProcessed))
                    {
                        // Se foi encontrado um pedido que ainda não está associado a nenhuma thread servidora...
                        req.IsBeingProcessed = true;
                        clientRequest = req;
                        break;
                    }
                    if (clientRequest != null)
                    {
                        SyncUtils.Notify(this, clientRequest); // Notificar o objecto (pedido do cliente) sofreu alterações
                        service = clientRequest.ServiceRequested;
                        break;
                    }
                    try
                    {
                        // Esperar que haja notificação que indique que a lista de pedidos foi modificada
                        SyncUtils.Wait(this, _clientServiceRequests, timeout);
                    }
                    catch (ThreadInterruptedException)
                    {
                        SyncUtils.Broadcast(this, _clientServiceRequests);
                        throw;
                    }
                } while ((timeout = SyncUtils.AdjustTimeout(ref lastTime, ref timeout))> 0);
            }
            return clientRequest; // Devolve o rendezvous token
        }

        public void Reply(object rendezVousToken, TR response)
        {
            lock (this)
            {
                ((ClientRequest) rendezVousToken).Response = response;
                SyncUtils.Notify(this, rendezVousToken);
            }
        }
    }
}
