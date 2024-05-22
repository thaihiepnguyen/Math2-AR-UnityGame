using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class LoadImageManager {
    static public IEnumerator LoadBinaryImage(Image image, int? id, int width = 500, int height = 200)
    {
        if (id == null)
        {
            id = 0; // skin default
        }
        string url = $"{GlobalVariable.server_url}/images/download/{id}"; // Replace with your server and image id
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                // Get downloaded data
                byte[] imageData = webRequest.downloadHandler.data;

                // Use imageData as needed, for example, convert to Texture2D
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                texture = ResizeTexture(texture, width, height);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
    
    
    static public IEnumerator LoadImage(Image image, string url, int width = 500, int height = 200) 
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) 
        {
            Debug.Log(request.error);
        }
        else 
        {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            myTexture = ResizeTexture(myTexture, width, height);
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            image.sprite = newSprite;
        }
    }
    static private Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Bilinear;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(newWidth, newHeight);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }
}