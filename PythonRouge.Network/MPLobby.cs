using System;
using System.Collections.Generic;
using RLNET;
using Lidgren.Network;
using System.Net;
using System.Timers;

namespace PythonRouge.Network
{
    public class MPLobby
    {
        private RLRootConsole _rootConsole;

        private NetClient Client;
        private string name;
        public Dictionary<string, IPEndPoint> servers = new Dictionary<string, IPEndPoint>();
        public List<string> sellist = new List<string>();
        public int curIndex = 0;
        private Timer aTimer;

        public MPLobby(RLRootConsole _rootConsole, string name)
        {
            this._rootConsole = _rootConsole;
            this.name = name;
            var config = new NetPeerConfiguration("PythonRouge");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            Client = new NetClient(config);
            Client.Start();
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTick;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            Client.DiscoverLocalPeers(32078);
        }

        public int Update(RLKeyPress keypress)
        {
            var msg = Client.ReadMessage();
            if(msg != null)
            {
                switch(msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        var name = msg.ReadString();
                        var endpoint = msg.SenderEndPoint;
                        if (!servers.ContainsKey(name)) servers.Add(name, endpoint);
                        if (!sellist.Contains(name)) sellist.Add(name);
                        break;
                }
            }
            
            if (keypress != null)
            {
                switch (keypress.Key)
                {
                    case RLKey.Up:
                        if (curIndex == 0)
                        {
                            break;
                        }
                        else
                        {
                            curIndex--;
                            break;
                        }
                    case RLKey.Down:
                        if (curIndex == sellist.Count - 1)
                        {
                            break;
                        }
                        else
                        {
                            curIndex++;
                        }
                        break;
                    case RLKey.Enter:
                        if(servers.Count > 0) return 1;
                        break;
                }
            }
            return 0;
        }
        
        public void Render()
        {
            _rootConsole.Print(4, 3, "Servers:", RLColor.White);
            var counter = 0;
            foreach(KeyValuePair<string, IPEndPoint> kvp in servers)
            {
                if(kvp.Key == sellist[curIndex])
                {
                    _rootConsole.Print(4, 4 + counter, kvp.Key + "," + kvp.Value.Address + ":" + kvp.Value.Port, RLColor.Black, RLColor.White);
                }
                else
                {
                    _rootConsole.Print(4, 4 + counter, kvp.Key + "," + kvp.Value.Address + ":" + kvp.Value.Port, RLColor.White);
                }
                counter++;
            }
        }


    }
}
