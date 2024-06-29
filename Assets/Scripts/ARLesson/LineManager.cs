using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
public class LineManager : MonoBehaviour
{
   
    public LineRenderer lineRenderer;
    public ARPlacementInteractable placementInteractable;
    LineRenderer line;
    public TextMeshPro mText;

    private int pointCount = 0;

    public TextMeshProUGUI buttonText;

    public bool continuous;

    public void ToggleBetweenDiscreteAndContinuous(){
        continuous = !continuous;
        if(continuous){
            buttonText.text = "Discrete";
        }
        else{
            buttonText.text = "Continuous";
        }
    }

    void Start()
    {
        placementInteractable.objectPlaced.AddListener(DrawLine);
    }

    void DrawLine(ARObjectPlacementEventArgs args){
        
        pointCount++;

        if (pointCount < 2)
        {
            line = Instantiate(lineRenderer);
            line.positionCount = 1;
        }
        else {
            line.positionCount = pointCount;
            if(!continuous)
            pointCount = 0;
        }


        // //1. increase the point count
        // lineRenderer.positionCount++;

        // // let the points location in the line renderer

        // lineRenderer.SetPosition(lineRenderer.positionCount-1,args.placementObject.transform.position);
        // if (lineRenderer.positionCount > 1){
        //     Vector3 pointA = lineRenderer.GetPosition(lineRenderer.positionCount-1);
        //     Vector3 pointB = lineRenderer.GetPosition(lineRenderer.positionCount-2);
        //     float dis = Vector3.Distance(pointA, pointB);
        //     TextMeshPro distText = Instantiate(mText);
        //     distText.text = ""+dis;

        //     Vector3 directionVector = (pointB - pointA);
        //     Vector3 normal = args.placementObject.transform.up;
        //     Vector3 upd = Vector3.Cross(directionVector,normal).normalized;

        //     Quaternion rotation = Quaternion.LookRotation(-normal,upd);

        //     distText.transform.rotation = rotation;
        //     distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.05f;

        // }


        // let the points location in the line renderer

        line.SetPosition(line.positionCount-1,args.placementObject.transform.position);
        if (line.positionCount > 1){
            Vector3 pointA = line.GetPosition(line.positionCount-1);
            Vector3 pointB = line.GetPosition(line.positionCount-2);
            float dis = Vector3.Distance(pointA, pointB);
            TextMeshPro distText = Instantiate(mText);
            distText.text = ""+dis;

            Vector3 directionVector = (pointB - pointA);
            Vector3 normal = args.placementObject.transform.up;
            Vector3 upd = Vector3.Cross(directionVector,normal).normalized;

            Quaternion rotation = Quaternion.LookRotation(-normal,upd);

            distText.transform.rotation = rotation;
            distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.05f;

        }
   
    }
   
}
