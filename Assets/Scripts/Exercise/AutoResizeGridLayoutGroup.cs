using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoResizeGridLayoutGroup : MonoBehaviour
{
    // Start is called before the first frame update
    GridLayoutGroup gridlayout;
    RectTransform container;
    void Start()
    {
        gridlayout= GetComponent<GridLayoutGroup>();
        container = GetComponentInParent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float width= container.rect.width-44;
        float oldHeigth = container.rect.height-44;
        
        gridlayout.cellSize = new Vector2(width, oldHeigth);
    }
    public static void resizeGridLayout(GridLayoutGroup gridlayout)
    {
        
    }
}
