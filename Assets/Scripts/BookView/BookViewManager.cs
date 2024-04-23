using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public Button backBtn;
    public Button rotateBtn;
    private bool _isVertical = false;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        backBtn.onClick.AddListener(OnClickBackBtn);
        rotateBtn.onClick.AddListener(() => OnClickRotateBtn(!this._isVertical));
    }

    public void OnClickBackBtn()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        this._isVertical = false;
    }

    public void OnClickRotateBtn(bool isVertical)
    {
        if (isVertical)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        } else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        this._isVertical = isVertical;
    }
}
