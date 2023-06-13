using HybridWebSocket;
using rpc_csharp.transport;
using System;

namespace RPC.Transports
{
    public class WebSocketClientTransport : ITransport
    {
        private readonly WebSocket webSocket;

        public event Action OnCloseEvent;
        public event Action<string> OnErrorEvent;
        public event Action<byte[]> OnMessageEvent;
        public event Action OnConnectEvent;

        public WebSocketClientTransport(string url)
        {
            webSocket = WebSocketFactory.CreateInstance(url);

            webSocket.OnMessage += this.HandleMessage;
            webSocket.OnError += this.HandleError;
            webSocket.OnClose += this.HandleClose;
            webSocket.OnOpen += this.HandleOpen;
        }

        public void Connect() =>
            webSocket.Connect();

        public void Close() =>
            webSocket.Close();

        private void HandleMessage(byte[] data)
        {
            OnMessageEvent?.Invoke(data);
        }

        private void HandleError(string error)
        {
            OnErrorEvent?.Invoke(error);
        }

        private void HandleClose(WebSocketCloseCode closeCode)
        {
            OnCloseEvent?.Invoke();
        }

        private void HandleOpen()
        {
            OnConnectEvent?.Invoke();
        }

        public void SendMessage(byte[] data) =>
            webSocket.Send(data);

        public void Dispose()
        {
            OnCloseEvent = null;
            OnErrorEvent = null;
            OnMessageEvent = null;
            OnConnectEvent = null;

            webSocket.OnMessage -= this.HandleMessage;
            webSocket.OnError -= this.HandleError;
            webSocket.OnClose -= this.HandleClose;
            webSocket.OnOpen -= this.HandleOpen;
        }
    }
}
