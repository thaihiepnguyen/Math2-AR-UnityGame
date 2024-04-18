using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public Button backBtn;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        backBtn.onClick.AddListener(OnClickBackBtn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBackBtn()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
