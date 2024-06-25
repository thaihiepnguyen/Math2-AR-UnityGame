using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameNonAR : NetworkBehaviour
{
    [SerializeField] private Button StartHost;
    [SerializeField] private Button StartClient;


    // Start is called before the first frame update
    void Start()
    {
        StartHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        StartClient.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

    }

}