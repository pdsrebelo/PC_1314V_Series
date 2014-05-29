package Cat;

import java.util.LinkedList;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import Utils.SyncUtils;

//TODO NOT FINISHED YET
public class Ex6_RendezvousChannel<S,R> {
	private final Lock lock;
	
	private static class ClientRequest<S,R>{
		protected Condition waitCondition;
		protected S serviceRequested;
		private R response;
		protected boolean isBeingProcessed;
		
		public ClientRequest(Condition wcond, S service){
			waitCondition = wcond;
			isBeingProcessed = false;
			setResponse(null);
			serviceRequested = service;
		}

		public R getResponse() {
			return response;
		}

		public void setResponse(R response) {
			this.response = response;
		}
	}
	
	private final LinkedList<ClientRequest<S,R>> clientRequests = new LinkedList<ClientRequest<S,R>>();
	private final Condition requestsQueueCondition;
	// .CTOR
	public Ex6_RendezvousChannel(){
		lock = new ReentrantLock();
		requestsQueueCondition = lock.newCondition();
	}

	// A invocar por uma thread servidora sempre que esteja em condições de satisfazer um pedido
    /*
     * Quando não existe 
    nenhum pedido de serviço pendente, a thread servidora fica bloqueada até que seja solicitado um 
    pedido de serviço, seja interrompida ou expire o limite de tempo especificado através do 
    parâmetro timeout.*/
    public Object Accept(int timeout, /*out*/ S service){
    	//TODO
		return service;
    }
	
    // Threads cliente chamam Request para fazer pedidos de serviço
    // é bloqueante e tem desistência por timeout ou interrupção
    public boolean Request(S service, long timeout, /*out*/ R response) throws InterruptedException
    {
    	lock.lock();
        boolean success = false;
    
        ClientRequest<S, R> request = new ClientRequest<S,R>(lock.newCondition(), service);
        clientRequests.add(request);
            
        // Notificar threads servidoras (podem existir ou não) que a lista de pedidos foi modificada
        requestsQueueCondition.signalAll(); // Broadcast

        long lastTime = System.currentTimeMillis();
        do
        {
            // Verificar estado do pedido
            if (request.isBeingProcessed && request!=null)
            {
                // Pedido já foi atendido e já há resposta!
                response = request.response;
                success = true;
                break;
            }
            try
            {
                request.waitCondition.wait(timeout);
            }
            catch (InterruptedException iex)
            {
                // Verificar se o pedido foi concluído, apesar da interrupção
                if (request.isBeingProcessed && request.response!=null)
                {
                    // Se já foi atendido e já há resposta, retornar como sucesso
                    response = request.response;
                    return success=true;
                }
                // Senão, remover
                clientRequests.remove(request);
                throw iex;
            }
        } while ((timeout = SyncUtils.adjustTimeout(lastTime, timeout))>0);
        lock.unlock();
        return success;
    }
    
    public void Reply(Object rendezVousToken, R response)
    {
    	lock.lock();
    	ClientRequest<S, R> clientReq = (ClientRequest<S, R>) rendezVousToken;
    	clientReq.setResponse(response);	// Afectar objecto com a resposta
    	clientReq.waitCondition.signal(); 	// Notificar, pois o objecto sofreu alterações
        lock.unlock();
    }
}
