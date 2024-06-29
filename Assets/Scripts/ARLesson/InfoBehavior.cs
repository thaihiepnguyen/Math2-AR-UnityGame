using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBehavior : MonoBehaviour
{
    const float SPEED = 3f;
    [SerializeField] Transform SectionInfo;

    [SerializeField] TextMeshPro pointName;
    [SerializeField] TextMeshPro pointInfo;
    Vector3 desiredScale = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        // SectionInfo.gameObject.SetActive(false);
    }

    
    [SerializeField]
    private bool IsSelected;


    public bool Selected 
    { 
        get 
        {
            return this.IsSelected;
        }
        set 
        {
            IsSelected = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // SectionInfo.gameObject.SetActive(IsSelected);
        SectionInfo.localScale = Vector3.Lerp(SectionInfo.localScale, desiredScale,Time.deltaTime * SPEED);
    }

    public void OpenInfo(){
        
        desiredScale = Vector3.one;
    }

    public void CloseInfo(){
        desiredScale = Vector3.zero;
    }

    public void SetInfo(string point){
        pointName.SetText(point);
        pointInfo.SetText("Điểm "+ point);
    }



  

  



}
