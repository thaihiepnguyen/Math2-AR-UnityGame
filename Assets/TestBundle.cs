using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestBundle : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        // string url = $"{GlobalVariable.server_url}/images/download/"; // Replace with your server and image id
        int id = 167;
       string url = $"{GlobalVariable.server_url}/images/download/{id}";
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
                Debug.Log(remoteAssetBundle.GetAllAssetNames()[0]);
               Instantiate(remoteAssetBundle.LoadAsset(remoteAssetBundle.GetAllAssetNames()[0]));
               remoteAssetBundle.Unload(false);
            }
        }
    }

    // Update is called once per frame
}
