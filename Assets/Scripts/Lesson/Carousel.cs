using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Carousel : MonoBehaviour
{

    public GameObject scrollbar;
    public float scroll_pos = 0;
    float[]pos;
    float distance;

    LessonList manager;
    public bool checkMove = false;
    // Start is called before the first frame update

    void Start(){
        manager = GameObject.Find("LessonListManager").GetComponent<LessonList>();
    }
    void Created(){
        pos = new float [transform.childCount];
       distance = 1f/ (pos.Length - 1);
       for (int i = 0 ; i < pos.Length; i++){
            pos[i] = distance * i;
       }
    }
    // Update is called once per frame
    void Update()
    {
         if (!manager.isDone) return;
         else Created();
         if (checkMove) return;

         if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //   scroll_pos += touch.deltaPosition.x * Time.deltaTime;
               scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
        }
       else {
        //  scroll_pos = Mathf.Clamp01(scroll_pos);
        
        for (int i = 0; i< pos.Length; i ++){
            if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos [i] - (distance/2)){
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                 scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
                 break;
                 
            }
        }
       }
       

       for (int i = 0; i< pos.Length; i ++){
            float targetScale = (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) ? 1f : 0.8f;
            transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(targetScale, targetScale), 0.1f);
        }
    }

    public void MoveLeft(){
       
       checkMove = true;

        for (int i = 0; i< pos.Length; i ++){
            if (i!=0 && scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)){
                // scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i-1], 0.1f);
                  
                  scrollbar.GetComponent<Scrollbar>().value = pos[i-1];
                  scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
                 break;
             
            }
        }

        checkMove = false;
    }

     public void MoveRight(){
         checkMove = true;
        for (int i = 0; i< pos.Length; i ++){
            if (i!=pos.Length-1 && scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)){
                // scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i+1], 0.1f);
                    scrollbar.GetComponent<Scrollbar>().value = pos[i+1];
                  scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
                 break;
         
            }
        }

         checkMove = false;
    }
}
