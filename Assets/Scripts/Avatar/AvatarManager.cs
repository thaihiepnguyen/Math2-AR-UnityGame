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

    private int indexChoice = 0;


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
    GameObject[] bufferObject;

    async void Start()
    {
        userBUS = new UserBUS();
        var response = await userBUS.GetProfile();

        if (response.isSuccessful && response.data != null)
        {
            personal = response.data;

            skins = personal.skinsPurchased;
          
            //skin
            bufferObject = new GameObject[skins.Length];
            for (int i = 0; i < skins.Length; i++)
            {
                
                GameObject newProduct = Instantiate(prefabProduct);
          
                var imageProduct = newProduct.transform.GetChild(0).GetComponent<Image>();

                int index = i;
                newProduct.transform.SetParent(skinTab.transform, false);
                if (personal.imageSkinId == skins[i].imageSkinId){
                    skinId = skins[i].skinId;
                    StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, skins[i].imageSkinId,500,200, frameContainer.GetComponent<Image>()));
                    newProduct.GetComponent<Image>().color = new Color32(209,165,165,255);
                    this.indexChoice = i;

                    StartCoroutine(LoadModelManager.LoadModelBuffer(skins[i].threeDimensionId,prototype,bufferObject,true, index));
                }
                else {
                    StartCoroutine(LoadModelManager.LoadModelBuffer(skins[i].threeDimensionId,prototype,bufferObject,false, index));
                    StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, skins[i].imageSkinId));
                }

               
                newProduct.GetComponent<Button>().onClick.AddListener(() => CustomAvatar(index,skins[index].skinId,"skin", skinTab));
            }

            frames = personal.framesPurchased;
            for (int i = 0; i < frames.Length; i++)
            {
                GameObject newProduct = Instantiate(prefabProduct);
          
                var imageProduct = newProduct.transform.GetChild(0).GetComponent<Image>();

                
       
                int index = i;
                newProduct.transform.SetParent(frameTab.transform, false);

                if (personal.imageFrameId == frames[i].imageFrameId){
                    frameId = frames[i].frameId;
                    StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, frames[i].imageFrameId, 500, 200, frame.GetComponent<Image>()));
                    newProduct.GetComponent<Image>().color = new Color32(209,165,165,255);
                    frame.SetActive(true);
                } else {
                    StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, frames[i].imageFrameId));
                }
                newProduct.GetComponent<Button>().onClick.AddListener(() => CustomAvatar(index,frames[index].frameId,"frame", frameTab,imageProduct.sprite));
            }
        }
    }
    bool isOK = false;
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


        if (isOK == false && bufferObject != null && CheckBufferObject()) {
             for (int i = 0; i < bufferObject.Length; i++) {
                if (bufferObject[i] != null) {
                    bufferObject[i].transform.localScale = prototype.transform.localScale;
                    Instantiate(bufferObject[i],prototype.transform.position,prototype.transform.rotation,container.transform);
                 
                }
            }
            isOK = true;
        }
    }

    bool CheckBufferObject() {
        for (int i = 0; i < bufferObject.Length; i++) {
            if (bufferObject[i] == null) {
                return false;
            }
        }

        return true;
    }

     void CustomAvatar(int index, int? id, string type, GameObject tab, Sprite image = null)
    {
        if (type == "skin"){
            Debug.Log(id);
            skinId = id;
            Debug.Log(index);
            indexChoice = index;
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

     public async void SaveChange(){
         var response = await userBUS.UpdateProfile(new UpdateProfileDTO {
                    skinId = skinId,
                    frameId = frameId
                });

        if (response.isSuccessful){
            Debug.Log(indexChoice);
            frameContainer.GetComponent<Image>().sprite = skinTab.transform.GetChild(indexChoice).GetChild(0).gameObject.GetComponent<Image>().sprite;


            Toast.Show("Cập nhật avatar thành công", 1f, ToastPosition.MiddleCenter);
        }
        else {
            Toast.Show("Cập nhật avatar thất bại", 1f, ToastPosition.MiddleCenter);
        }
     }

}
