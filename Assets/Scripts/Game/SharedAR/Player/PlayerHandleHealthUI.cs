using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandleHealthUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private Image triangle;
    GameObject hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    
    private Camera _mainCamera;
    void Start()
    {
        // Check if this instance is the server
       
    }
    public override void OnNetworkSpawn()
    {
        hearts = GameObject.Find("Hearts");
        _mainCamera = GameObject.FindObjectOfType<Camera>();
        if (IsServer)
        {
            HealthText.text = "P1";
        }
        // Check if this instance is a client
        else if (IsClient)
        {
            HealthText.text = "P2";
        }
        AllPlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnPlayerHealthChangedServerRpc;
        InstanceOnOnPlayerHealthChangedServerRpc(GetComponentInParent<NetworkObject>().OwnerClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    private void InstanceOnOnPlayerHealthChangedServerRpc(ulong id)
    {
        if (GetComponentInParent<NetworkObject>().OwnerClientId == id)
        {
            
            UpdateHeartsUIClientRpc(id);
        }
    }

    private void Update()
    {
        if (_mainCamera)
        {
            HealthText.transform.LookAt(_mainCamera.transform);
            triangle.transform.LookAt(_mainCamera.transform);
        }
    }

    [ClientRpc]
    void SetHealthTextClientRpc(ulong id)
    {
        HealthText.text = AllPlayerDataManager.Instance.GetPlayerHealth(id).ToString();
    }

    public override void OnNetworkDespawn()
    {
        AllPlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnPlayerHealthChangedServerRpc;
    }
    [ClientRpc]
    private void SetPlayerLabelClientRpc(string label)
    {
        HealthText.text = label;
    }
    [ClientRpc]
    private void UpdateHeartsUIClientRpc(ulong id)
    {
        if (id == NetworkManager.Singleton.LocalClientId)
        {
            if (hearts != null)
            {
                var heartImages = hearts.GetComponentsInChildren<Image>();
                if (heartImages != null)
                {
                    for (int i = 0; i < heartImages.Length; i++)
                    {
                        heartImages[i].sprite = i < AllPlayerDataManager.Instance.GetPlayerHealth(id) ? fullHeart : emptyHeart;
                    }

                }
            }
        }
        
        
    }
}