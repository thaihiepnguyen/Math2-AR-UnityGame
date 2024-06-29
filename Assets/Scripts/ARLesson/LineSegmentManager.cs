using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegmentManager : MonoBehaviour
{
     [SerializeField] private GameObject point;

    [SerializeField] private GameObject container;

private GameObject pointA;
private GameObject pointB;
    
    
    [SerializeField]
    private InfoBehavior[] placedObjects;

    [SerializeField] private float animationDuration = 2f ;

   [SerializeField] LineRenderer lineRenderer ;
    private  LineRenderer line;
    private Vector2 touchPosition = default;
   private Vector3[] linePoints ;
        [SerializeField]
    private bool displayOverlay = false;



    void Start()
    {

          Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 2f));
        pointA = Instantiate(point, spawnPos, Quaternion.identity,container.transform);
     
        // pointA.transform.position = spawnPos;
        pointA.GetComponent<InfoBehavior>().SetInfo("A");
        pointA.SetActive(false);

        pointB = Instantiate(point, pointA.transform.position + new Vector3(3f,0,1.5f), Quaternion.identity, container.transform);
        
        // pointB.transform.position = pointA.transform.position + new Vector3(3f,0,1.5f);
        pointB.GetComponent<InfoBehavior>().SetInfo("B");
        pointB.SetActive(false);
   

        line = Instantiate(lineRenderer);
        line.transform.SetParent(container.transform);

            StartCoroutine(LessonInit());
   
    }

    // Update is called once per frame

     

   
    void Update()
    {
        // if(Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);
            
        //     touchPosition = touch.position;

        //     if(touch.phase == TouchPhase.Began)
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //         RaycastHit hitObject;
        //         if(Physics.Raycast(ray, out hitObject))
        //         {
        //             InfoBehavior placementObject = hitObject.transform.GetComponent<InfoBehavior>();
        //             if(placementObject != null)
        //             {
        //                 ChangeSelectedObject(placementObject);
        //             }
        //         }
        //     }
        // }
    }

     void ChangeSelectedObject(InfoBehavior selected)
    {
        foreach (InfoBehavior current in placedObjects)
        {   
          
            if(selected != current) 
            {
                current.Selected = false;
             
            }
            else 
            {
                current.Selected = true;
           
            }
            
            if(displayOverlay)
                current.OpenInfo();
        }
    }

    IEnumerator LessonInit(){
      
        pointA.SetActive(true);
          yield return new WaitForSeconds(5f);
        pointB.SetActive(true);
            line.positionCount=2;
           yield return new WaitForSeconds(5f);

        linePoints = new Vector3[2] ;
    
         linePoints [0] = pointA.transform.position;
         linePoints[1] = pointB.transform.position;
      

      StartCoroutine (AnimateLine ()) ;

    }


       private IEnumerator AnimateLine () {
      float segmentDuration = animationDuration / 2 ;

      for (int i = 0; i < 2 - 1; i++) {
         float startTime = Time.time ;

         Vector3 startPosition = linePoints [ i ] ;
         Vector3 endPosition = linePoints [ i + 1 ] ;

         Debug.Log(startPosition);
         Debug.Log(endPosition);

         Vector3 pos = startPosition ;
         line.SetPosition(i,pos);
         while (pos != endPosition) {
            float t = (Time.time - startTime) / segmentDuration ;
            pos = Vector3.Lerp (startPosition, endPosition, t) ;

            // animate all other points except point at index i
            for (int j = i + 1; j < 2; j++)
               line.SetPosition (j, pos) ;

            yield return null ;
         }
      }
   }
}
