using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;


public class LoadModelManager:MonoBehaviour {
    static public IEnumerator LoadModel( int? id, GameObject self, GameObject parent = null, bool active = false)
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

            
               var model = Instantiate(remoteAssetBundle.LoadAssetAsync(remoteAssetBundle.GetAllAssetNames()[0]).asset) as GameObject;
             
                model.transform.position = self.transform.position;
                model.transform.rotation = self.transform.rotation;
                model.transform.localScale = self.transform.localScale;

                if (parent !=null){
                model.transform.SetParent(parent.transform,false);
                }

                if (active == true){
                    model.SetActive(true);
                }
                else {
                         model.SetActive(false);
                }
               remoteAssetBundle.Unload(false);
            }
        }
    }
    
        static public IEnumerator LoadModelBuffer( int? id, GameObject self, GameObject[] buffer = null, bool active = false, int index = -1)
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

            
               var model = remoteAssetBundle.LoadAssetAsync(remoteAssetBundle.GetAllAssetNames()[0]).asset as GameObject;
             
        

                if (buffer !=null && index!=-1){
                    buffer[index] = model;
                }
                if (active == true){
                    model.SetActive(true);
                }
                else {
                         model.SetActive(false);
                }
               remoteAssetBundle.Unload(false);
            }
        }
    }
  
}