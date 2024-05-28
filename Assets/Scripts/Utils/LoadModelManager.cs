using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;


public class LoadModelManager:MonoBehaviour {
    static public IEnumerator LoadModel( int? id, GameObject self, GameObject parent = null)
    {
        if (id == null)
        {
            id = 0; // skin default
        }
        string url = $"{GlobalVariable.server_url}/3ds/download/{id}"; // Replace with your server and image id
    
        using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
               AssetBundle  remoteAssetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);

               if (remoteAssetBundle == null){
                yield break;
               }

                Debug.Log(remoteAssetBundle.LoadAsset(remoteAssetBundle.GetAllAssetNames()[0]));
               var model = Instantiate(remoteAssetBundle.LoadAsset(remoteAssetBundle.GetAllAssetNames()[0])) as GameObject;
             
                model.transform.position = self.transform.position;
                model.transform.rotation = self.transform.rotation;
                model.transform.localScale = self.transform.localScale;

                if (parent !=null){
                model.transform.SetParent(parent.transform,false);
                }
               remoteAssetBundle.Unload(false);
            }
        }
    }
    
    
  
}