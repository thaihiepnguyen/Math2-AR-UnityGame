using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoadImageManager {
    static public IEnumerator LoadImage(Image image, string url) 
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
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            image.sprite = newSprite;
        }
    }
}