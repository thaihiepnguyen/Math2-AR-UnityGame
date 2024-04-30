using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    private StoreDTO store;
    private StoreBUS storeBUS;



    [SerializeField]
    private GameObject prefabProduct;


    [SerializeField]
    private GameObject paymentPanel;


    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject skinTab;

    [SerializeField]
    private GameObject frameTab;

    [SerializeField]
    private GameObject testTab;
    // Start is called before the first frame update

    private List<SkinDTO> skins;
    private List<FrameDTO> frames;
    private List<TestDTO> tests;
    async void Start()
    {
        storeBUS = new StoreBUS();
        var response = await storeBUS.GetAll();

        if (response.isSuccessful && response.data != null)
        {
            store = response.data;

            skins = store.skins;

            //skin

            for (int i = 0; i < skins.Count; i++)
            {

                GameObject newProduct = Instantiate(prefabProduct);
                TextMeshProUGUI newProductName = newProduct.GetComponentInChildren<TextMeshProUGUI>();

                // if (skins[i].image_url != ""){
                var imageProduct = newProduct.transform.GetChild(1).GetComponent<Image>();

                StartCoroutine(LoadImage(imageProduct, skins[i].image_url));
                // }

                int index = i;

                if (newProductName != null)
                {
                    newProductName.text = skins[i].skin_name;// Set button text 

                }

                var button = newProduct.GetComponentInChildren<Button>();


                if (button != null)
                {
                    if (skins[i].is_purchased)
                    {
                         button.gameObject.SetActive(false);
                         newProduct.transform.GetChild(5).gameObject.SetActive(true);
                         newProduct.GetComponent<Image>().color = new Color32(176,176,176,255);
                          imageProduct.GetComponent<Image>().color = new Color32(255,255,255,146);
                    }
                    else
                    {
                        var price = button.GetComponentInChildren<TextMeshProUGUI>();
                        price.text = skins[i].price.ToString();

                        button.onClick.AddListener(() => PaymentPanel("skins", skins[index].skin_id, skins[index].skin_name, skins[index].price, imageProduct.sprite));
                    }
                }


                newProduct.transform.SetParent(skinTab.transform, false);
            }

            frames = store.frames;
            for (int i = 0; i < frames.Count; i++)
            {
                GameObject newProduct = Instantiate(prefabProduct);
                TextMeshProUGUI newProductName = newProduct.GetComponentInChildren<TextMeshProUGUI>();

                //  if (frames[i].image_url != ""){
                var imageProduct = newProduct.transform.GetChild(1).GetComponent<Image>();

                StartCoroutine(LoadImage(imageProduct, frames[i].image_url));
                //  }

                int index = i;

                if (newProductName != null)
                {
                    newProductName.text = frames[i].frame_name;// Set button text 

                }


                var button = newProduct.GetComponentInChildren<Button>();


                if (button != null)
                {
                    if (frames[i].is_purchased)
                    {
                         button.gameObject.SetActive(false);
                        newProduct.transform.GetChild(5).gameObject.SetActive(true);
                         newProduct.GetComponent<Image>().color = new Color32(176,176,176,255);
                         imageProduct.GetComponent<Image>().color = new Color32(255,255,255,146);
                    }
                    else
                    {
                        var price = button.GetComponentInChildren<TextMeshProUGUI>();
                        price.text = frames[i].price.ToString();
                        button.onClick.AddListener(() => PaymentPanel("frames", frames[index].frame_id, frames[index].frame_name, frames[index].price, imageProduct.sprite));
                    }
                }




                newProduct.transform.SetParent(frameTab.transform, false);
            }
        }

        tests = store.tests;
        for (int i = 0; i < tests.Count; i++)
        {
            GameObject newProduct = Instantiate(prefabProduct);
            TextMeshProUGUI newProductName = newProduct.GetComponentInChildren<TextMeshProUGUI>();
            var imageProduct = newProduct.transform.GetChild(1).GetComponent<Image>();
            imageProduct.sprite = Resources.Load<Sprite>("Test");



            int index = i;

            if (newProductName != null)
            {
                newProductName.text = tests[i].test_name;// Set button text 

            }


            var button = newProduct.GetComponentInChildren<Button>();


            if (button != null)
            {
                if (tests[i].is_purchased)
                {
                   button.gameObject.SetActive(false);
                   newProduct.transform.GetChild(5).gameObject.SetActive(true);
                    newProduct.GetComponent<Image>().color = new Color32(176,176,176,255);
                     imageProduct.GetComponent<Image>().color = new Color32(255,255,255,146);
                }
                else
                {
                    var price = button.GetComponentInChildren<TextMeshProUGUI>();
                    price.text = tests[i].price.ToString();
                    button.onClick.AddListener(() => PaymentPanel("tests", tests[index].test_id, tests[index].test_name, tests[index].price, imageProduct.sprite));
                }
            }




            newProduct.transform.SetParent(testTab.transform, false);
        }

    }




    private IEnumerator LoadImage(Image image, string url)
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

    // Update is called once per frame
    void Update()
    {

    }


    void PaymentPanel(string type, int id, string name, int price, Sprite image)
    {

        GameObject panel = Instantiate(paymentPanel);

        var img = panel.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        img.sprite = image;

        var title = panel.transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        title.text = name;

        var priceTag = panel.transform.GetChild(0).transform.GetChild(2).transform.GetComponentInChildren<TextMeshProUGUI>();

        priceTag.text = price.ToString();

        var button = panel.transform.GetChild(0).transform.GetChild(3).transform.GetComponentInChildren<Button>();

        button.onClick.AddListener(async () => {
            await storeBUS.Purchase(new PurchaseDTO {
              type = type,
              id = id
            });

            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        });

        panel.transform.SetParent(canvas.transform, false);

        panel.SetActive(true);


    }
}
