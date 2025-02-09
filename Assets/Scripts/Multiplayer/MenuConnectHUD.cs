using UnityEngine;
using UnityEngine.UI;
using Mirror;
using kcp2k;
using System;

public class MenuConnectHUD : NetworkManager
{
    public static MenuConnectHUD instance;
    
    [SerializeField] private InputField IPInputField;

    private NetworkManager manager;
    private KcpTransport kcpTransport;
    private NetworkManagerHUD networkManagerHud;

    public override void Awake()
    {
        instance = this;
        
        manager = GetComponent<NetworkManager>();
        kcpTransport = GetComponent<KcpTransport>();
        networkManagerHud = GetComponent<NetworkManagerHUD>();
    }

    public void CreateServerButton()
    {
        if (IPInputField.text != "")
        {
            manager.StartHost();

            manager.networkAddress = IPInputField.text;

            if (Transport.active is PortTransport portTransport)
            {
                portTransport.Port = kcpTransport.port;
            }

            networkManagerHud.enabled = true;
        }
    }

    public void ConnectServerButton()
    {
        if (IPInputField.text != "")
        {
            manager.StartClient();
            
            manager.networkAddress = IPInputField.text;
            
            if (Transport.active is PortTransport portTransport)
            {
                portTransport.Port = kcpTransport.port;
            }

            if ((NetworkClient.isConnected))
            {
                networkManagerHud.enabled = true;
            }
        }
    }
}
