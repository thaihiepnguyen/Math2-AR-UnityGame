using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridAlignment : MonoBehaviour
{
    public GameObject container;
    public int col = 3;
    public  int row = 2;
    void Start()
    
    {
        var paddingHorizontal = (int) container.GetComponent<RectTransform>().rect.width * 1/20;
        GetComponent<GridLayoutGroup>().padding.left = paddingHorizontal;
        GetComponent<GridLayoutGroup>().padding.right = paddingHorizontal;
        var paddingVertical = (int) container.GetComponent<RectTransform>().rect.height * 1/20;

        GetComponent<GridLayoutGroup>().padding.top = paddingVertical;
        GetComponent<GridLayoutGroup>().padding.bottom = paddingVertical;
        float width = container.GetComponent<RectTransform>().rect.width - 2*paddingHorizontal;
        float height = container.GetComponent<RectTransform>().rect.height  - 2*paddingVertical; 
        Debug.Log(height);
        Vector2 newSize = new Vector2(width/col,height/row);

        GetComponent<GridLayoutGroup>().cellSize = newSize;
    }


}
