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
    GameObject hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    
    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        hearts = GameObject.Find("Hearts");
        _mainCamera = GameObject.FindObjectOfType<Camera>();
        AllPlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnPlayerHealthChangedServerRpc;
        InstanceOnOnPlayerHealthChangedServerRpc(GetComponentInParent<NetworkObject>().OwnerClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    private void InstanceOnOnPlayerHealthChangedServerRpc(ulong id)
    {
        if (GetComponentInParent<NetworkObject>().OwnerClientId == id)
        {
            SetHealthTextClientRpc(id);
            UpdateHeartsUIClientRpc(id);
        }
    }

    private void Update()
    {
        if (_mainCamera)
        {
            HealthText.transform.LookAt(_mainCamera.transform);
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
    private void UpdateHeartsUIClientRpc(ulong id)
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