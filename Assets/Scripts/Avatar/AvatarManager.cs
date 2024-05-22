using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyUI.Toast;
public class AvatarManager : MonoBehaviour
{
   private PersonalDTO personal;
    private UserBUS userBUS;



    [SerializeField]
    private GameObject prefabProduct;




    [SerializeField]
    private GameObject skinTab;

    [SerializeField]
    private GameObject frameTab;

   

    private SkinsDTO[] skins;
    private FramesDTO[] frames;
    async void Start()
    {
        userBUS = new UserBUS();
        var response = await userBUS.GetProfile();

        if (response.isSuccessful && response.data != null)
        {
            personal = response.data;

            skins = personal.skinsPurchased;
            //skin

            for (int i = 0; i < skins.Length; i++)
            {

                GameObject newProduct = Instantiate(prefabProduct);
          
                var imageProduct = newProduct.transform.GetChild(0).GetComponent<Image>();

                StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, skins[i].imageSkinId));
                // }


                newProduct.transform.SetParent(skinTab.transform, false);
            }

            frames = personal.framesPurchased;
            for (int i = 0; i < frames.Length; i++)
            {
                GameObject newProduct = Instantiate(prefabProduct);
          
                var imageProduct = newProduct.transform.GetChild(0).GetComponent<Image>();

                StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, frames[i].imageFrameId));
                //  }

                newProduct.transform.SetParent(frameTab.transform, false);
            }
        }

      
        

    }

    // Update is called once per frame
    void Update()
    {

    }


}
