using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridAlignmentPersonal : MonoBehaviour
{
    public GameObject container;
    void Update()
    
    {   
        var paddingHorizontal = (int) container.GetComponent<RectTransform>().rect.width * 1/20;
        container.GetComponent<GridLayoutGroup>().padding.left = paddingHorizontal;
        container.GetComponent<GridLayoutGroup>().padding.right = paddingHorizontal;
        var paddingVertical = (int) container.GetComponent<RectTransform>().rect.height * 1/20;

        container.GetComponent<GridLayoutGroup>().padding.top = paddingVertical;
        container.GetComponent<GridLayoutGroup>().padding.bottom = paddingVertical;
        float width = container.GetComponent<RectTransform>().rect.width - 2*paddingHorizontal;
        float height = container.GetComponent<RectTransform>().rect.height  - 2*paddingVertical; 
        Vector2 newSize = new Vector2(width,height/2);

        container.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}
