using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;
public class StoreManager : MonoBehaviour
{
    private StoreDTO store;
    private StoreBUS storeBUS;
    private UserBUS userBUS;


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

    [SerializeField]
    private TMP_Text coin;
    // Start is called before the first frame update

    public List<TestDTO> TestsUI {
        set {
            if (testTab.transform.childCount != 0) {
                foreach(Transform child in testTab.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < value.Count; i++) {
                GameObject newProduct = Instantiate(prefabProduct);
                TextMeshProUGUI newProductName = newProduct.GetComponentInChildren<TextMeshProUGUI>();
                var imageProduct = newProduct.transform.GetChild(1).GetComponent<Image>();
                imageProduct.sprite = Resources.Load<Sprite>("Test");



                int index = i;

                if (newProductName != null)
                {
                    newProductName.text = value[i].test_name;// Set button text 

                }


                var button = newProduct.GetComponentInChildren<Button>();


                if (button != null)
                {
                    if (value[i].is_purchased)
                    {
                        button.gameObject.SetActive(false);
                        newProduct.transform.GetChild(5).gameObject.SetActive(true);
                        newProduct.GetComponent<Image>().color = new Color32(176,176,176,255);
                        imageProduct.GetComponent<Image>().color = new Color32(255,255,255,146);
                    }
                    else
                    {
                        var price = button.GetComponentInChildren<TextMeshProUGUI>();
                        price.text = value[i].price.ToString();
                        button.onClick.AddListener(() => PaymentPanel("tests", value[index].test_id, value[index].test_name, value[index].price, imageProduct.sprite));
                    }
                }

                newProduct.transform.SetParent(testTab.transform, false);
            }
        }
    }
    public List<FrameDTO> FramesUI {
        set {
            if (frameTab.transform.childCount != 0) {
                foreach(Transform child in frameTab.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < value.Count; i++)
            {
                GameObject newProduct = Instantiate(prefabProduct);
                TextMeshProUGUI newProductName = newProduct.GetComponentInChildren<TextMeshProUGUI>();

                var imageProduct = newProduct.transform.GetChild(1).GetComponent<Image>();

                StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, value[i].image_id));

                int index = i;

                if (newProductName != null)
                {
                    newProductName.text = value[i].frame_name;// Set button text 

                }


                var button = newProduct.GetComponentInChildren<Button>();


                if (button != null)
                {
                    if (value[i].is_purchased)
                    {
                         button.gameObject.SetActive(false);
                        newProduct.transform.GetChild(5).gameObject.SetActive(true);
                         newProduct.GetComponent<Image>().color = new Color32(176,176,176,255);
                         imageProduct.GetComponent<Image>().color = new Color32(255,255,255,146);
                    }
                    else
                    {
                        var price = button.GetComponentInChildren<TextMeshProUGUI>();
                        price.text = value[i].price.ToString();
                        button.onClick.AddListener(() => PaymentPanel("frames", value[index].frame_id, value[index].frame_name, value[index].price, imageProduct.sprite));
                    }
                }

                newProduct.transform.SetParent(frameTab.transform, false);
            }
        }
    }
    public List<SkinDTO> SkinsUI {
        set {
            if (skinTab.transform.childCount != 0) {
                foreach(Transform child in skinTab.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < value.Count; i++) {
                GameObject newProduct = Instantiate(prefabProduct);
                TextMeshProUGUI newProductName = newProduct.GetComponentInChildren<TextMeshProUGUI>();

                var imageProduct = newProduct.transform.GetChild(1).GetComponent<Image>();

                StartCoroutine(LoadImageManager.LoadBinaryImage(imageProduct, value[i].image_id));

                int index = i;

                if (newProductName != null)
                {
                    newProductName.text = value[i].skin_name;// Set button text 

                }

                var button = newProduct.GetComponentInChildren<Button>();

                if (button != null)
                {
                    if (value[i].is_purchased)
                    {
                         button.gameObject.SetActive(false);
                         newProduct.transform.GetChild(5).gameObject.SetActive(true);
                         newProduct.GetComponent<Image>().color = new Color32(176,176,176,255);
                          imageProduct.GetComponent<Image>().color = new Color32(255,255,255,146);
                    }
                    else
                    {
                        var price = button.GetComponentInChildren<TextMeshProUGUI>();
                        price.text = value[i].price.ToString();

                        button.onClick.AddListener(() => PaymentPanel("skins", value[index].skin_id, value[index].skin_name, value[index].price, imageProduct.sprite));
                    }
                }


                newProduct.transform.SetParent(skinTab.transform, false);
            }
        }
    }

    async void Start()
    {
        storeBUS = new StoreBUS();
        userBUS = new UserBUS();
        var response = await storeBUS.GetAll();

        if (response.isSuccessful && response.data != null)
        {
            store = response.data;

            SkinsUI = store.skins;
            FramesUI = store.frames;
            TestsUI = store.tests;
        }
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
            if  (CollectibleManager.GetCoin() >= price) {
                await storeBUS.Purchase(new PurchaseDTO {
                    type = type,
                    id = id
                });

                var response = await storeBUS.GetAll();
                var responseUser = await userBUS.GetMe();
                if (response.isSuccessful && response.data != null) {
                    if (responseUser.isSuccessful) {
                        var me = responseUser.data;
                        coin.text = me.coin.ToString();
                    }
                    SkinsUI = response.data.skins;
                    FramesUI = response.data.frames;
                    TestsUI = response.data.tests;
                    panel.SetActive(false);
                } else {
                    Toast.Show("Không thể kết nối với máy chủ.", .7f,ToastPosition.MiddleCenter);
                }
            } else {
                Toast.Show("Bạn không đủ tiền để mua sản phẩm này.", .7f,ToastPosition.MiddleCenter);
            }
        });

        panel.transform.SetParent(canvas.transform, false);

        panel.SetActive(true);
    }
}
