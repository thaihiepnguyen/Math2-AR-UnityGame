using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject personal;

    [SerializeField]
    private Button optionBtn;
    // Start is called before the first frame update
    void Start()
    {
        optionBtn.onClick.AddListener(OnClickButton);
    }

    void OnClickButton() {
       personal.SetActive(true);
    }
}
