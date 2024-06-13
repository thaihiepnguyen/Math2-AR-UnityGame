using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberRunnerManager : MonoBehaviour
{
    List<int> numbers = new List<int>();

    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();

    private int currentNumber = 0;
    private int currentIndex = 0;

    
    void Awake()
    {
        for (int i = 0; i< 10; i++){
            numbers.Add(i);
        }

        numbers.Sort();

        currentNumber = numbers[currentIndex];

        for (int i = 0; i < numbers.Count; i++){
            var obj = GetRandomObject();

            obj.GetComponent<NumberCollectiblesManager>().SetOverlayText(numbers[i].ToString());
            spawnObjects.Remove(obj);

        
        }

        
    }

    // Update is called once per frame
    void Update()
    {
          
    }





     GameObject GetRandomObject()
    {
        return spawnObjects[Random.Range(0, spawnObjects.Count)];
    }


    public void ChangeCurrent(){
        if (currentIndex < spawnObjects.Count){
            currentIndex++;
            currentNumber = numbers[currentIndex];
        }
    }

    public bool CheckCurrent(int index, DragonController controller){

          if(controller!=null)
            {
        if (index != currentNumber){
            

                controller.ChangeHealth(-1);
            return false;
            
        }

        controller.ChangeHealth(1);
        ChangeCurrent();


        return true;
            }
        return false;
        
    }

    
}
