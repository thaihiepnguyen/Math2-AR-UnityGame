using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private  GameObject tabHeaderList;
    [SerializeField] private  GameObject tabContentList;
    void Start()
    {

            // tabHeaderList = GameObject.Find("Tab Header");
            // tabContentList = GameObject.Find("Tab Content");
            for (int i = 0; i < tabHeaderList.transform.childCount; i++)
            {
                 GameObject tabHeader = tabHeaderList.transform.GetChild(i).gameObject;
                 var image = tabHeader.GetComponent<Image>();
                 image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                 var text = tabHeader.GetComponentInChildren<TextMeshProUGUI>();
                 text.color = Color.white;
                 Button button = tabHeader.GetComponent<Button>();
                    

                int index = i;

                button.onClick.AddListener(() => {

                            TabOnClick(index);
                        });
                if (i == 0)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 255f);
                     text.color = Color.black;
                  
                }

               

            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TabOnClick(int index){
            for (int i = 0; i < tabHeaderList.transform.childCount; i++)
            {

                 GameObject tabHeader = tabHeaderList.transform.GetChild(i).gameObject;
                 var image = tabHeader.GetComponent<Image>();
                 image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                 var text = tabHeader.GetComponentInChildren<TextMeshProUGUI>();
                 text.color = Color.white;
                 Button button = tabHeader.GetComponent<Button>();

                 tabContentList.transform.GetChild(i).gameObject.SetActive(false);

                if (i == index)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 255f);
                     text.color = Color.black;
                    tabContentList.transform.GetChild(i).gameObject.SetActive(true);
                  
                }
               

               

            }
    }
}
