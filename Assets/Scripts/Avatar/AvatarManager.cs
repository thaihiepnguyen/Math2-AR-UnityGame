using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyUI.Toast;
using Unity.XR.CoreUtils;
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

        [SerializeField] private GameObject prototype;
    [SerializeField] private GameObject container;

    [SerializeField] private GameObject frameContainer;

    [SerializeField] private GameObject frame;

    int? skinId = null;
    int? frameId = null;

    async void Start()
    {
        userBUS = new UserBUS();
        var response = await userBUS.GetProfile();

        if (response.isSuccessful && response.data != null)
        {
            personal = response.data;

            skins = personal.skinsPurchased;
            skinId =  personal.imageSkinId;
            frameId = personal.imageFrameId;
            //skin

            for (int i = 0; i < skins.Length; i++)
            {

                GameObject newProduct = Instantiate(prefabProduct);
          
                var imageProduct = newProduct.transform.GetChild(0).GetComponent<Image>();

                StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, skins[i].imageSkinId));
                // }
               
               

                 int index = i;
                newProduct.transform.SetParent(skinTab.transform, false);
                if (personal.imageSkinId == skins[i].imageSkinId){
                    newProduct.GetComponent<Image>().color = new Color32(209,165,165,255);
                    frameContainer.GetComponent<Image>().sprite = imageProduct.sprite;

                     StartCoroutine(LoadModelManager.LoadModel(skins[i].threeDimensionId,prototype,container,true,i));
                }
                else {
                       StartCoroutine(LoadModelManager.LoadModel(skins[i].threeDimensionId,prototype,container));
                }

               
                newProduct.GetComponent<Button>().onClick.AddListener(() => CustomAvatar(index,skins[i].imageSkinId,"skin", skinTab));
            }

            frames = personal.framesPurchased;
            for (int i = 0; i < frames.Length; i++)
            {
                GameObject newProduct = Instantiate(prefabProduct);
          
                var imageProduct = newProduct.transform.GetChild(0).GetComponent<Image>();

                StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, frames[i].imageFrameId));
                //  }
                int index = i;
                newProduct.transform.SetParent(frameTab.transform, false);

                 if (personal.imageFrameId == frames[i].imageFrameId){
                    newProduct.GetComponent<Image>().color = new Color32(209,165,165,255);
                    frame.GetComponent<Image>().sprite = imageProduct.sprite;
                    frame.SetActive(true);
                }
                   newProduct.GetComponent<Button>().onClick.AddListener(() => CustomAvatar(index,frames[i].imageFrameId,"frame", frameTab,imageProduct.sprite));
            }
        }

      
        

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Tab>().GetComponent<Tab>().GetAvatarTab == 0){
            container.SetActive(true);
            frameContainer.SetActive(false);
        }
        else if (FindObjectOfType<Tab>().GetComponent<Tab>().GetAvatarTab  == 1){
            container.SetActive(false);
            frameContainer.SetActive(true);
        }
    }

     void CustomAvatar(int index, int? id, string type, GameObject tab, Sprite image = null)
    {
        if (type == "skin"){
            skinId = id;
            for (int i = 0; i < skins.Length; i++){
                if (i == index){
                    container.transform.GetChild(i+1).gameObject.SetActive(true);
                    tab.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(209,165,165,255);
                   
                }
                else {
                     container.transform.GetChild(i+1).gameObject.SetActive(false);
                      tab.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(255,255,255,255);
                }
            }
        }
        else if (type == "frame"){
            frameId = id;
            frame.SetActive(true);
            frame.GetComponent<Image>().sprite = image;
            for (int i = 0; i < frames.Length; i++){
                 if (i == index){
                    tab.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(209,165,165,255);
                   
                }
                else {
                      tab.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(255,255,255,255);
                }
            }
        }
    }
}
