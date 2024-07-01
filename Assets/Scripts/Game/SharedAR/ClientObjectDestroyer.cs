using Unity.Netcode;
using UnityEngine;

public class ClientObjectDestroyer : MonoBehaviour
{
    public ulong objectNetworkId;


    [ServerRpc(RequireOwnership = false)]
    public void RequestObjectDestructionServerRpc(ulong networkObjectId, ServerRpcParams rpcParams = default)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkObject networkObject;
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out networkObject))
            {
                networkObject.Despawn();
            }
        }
    }
}
