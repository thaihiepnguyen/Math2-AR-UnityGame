using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public static event Action<ulong> OnPlayerScoreIncrease;
    public static event Action<(ulong from,ulong to)> OnPlayerHealthDescrease;
    private NetworkVariable<ulong> owner = new(999);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Arrow") == true)
        {
            var networkObject = GetComponent<NetworkObject>();
            var check = QuestionManager.Instance.CheckQuestion(collision.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
            if (check)
            {
                OnPlayerScoreIncrease?.Invoke(networkObject.OwnerClientId);
                
            }
            else
            {
                OnPlayerHealthDescrease?.Invoke(new(owner.Value, networkObject.OwnerClientId));
            }
            if (IsHost)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                GetComponent<ClientObjectDestroyer>().RequestObjectDestructionServerRpc(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
            }
        }


    }
}
