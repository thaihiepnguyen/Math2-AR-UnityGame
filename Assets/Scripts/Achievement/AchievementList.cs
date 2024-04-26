using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementList : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]

    private GameObject rewardPanel;

    [SerializeField]
    private GameObject parent;
    // Start is called before the first frame update
    
    [SerializeField]
    private GameObject canvas;
    async void Start()
    {
        AchievementBUS _achievementBUS = new();
        var response = await _achievementBUS.GetAchievementsByUserId();
        if (response.isSuccessful && response.data != null)
        {
            for (int i = 0; i < response.data.Count; i++)
            {
                GameObject newAchievement = Instantiate(prefab);
                TextMeshProUGUI newAchievementText = newAchievement.GetComponentInChildren<TextMeshProUGUI>();

                 int index = i;
              
                  

               

                if (newAchievementText != null)
                {
                    newAchievementText.text = response.data[i].name;// Set button text 

                }

                if (response.data[i].status_type == 2)
                {
                    newAchievement.transform.GetChild(2).gameObject.SetActive(true);
                     newAchievement.transform.GetChild(3).gameObject.SetActive(false);

                }
                else if (response.data[i].status_type == 1)
                {
                    var button = newAchievement.transform.GetChild(3).gameObject;
                    button.SetActive(true);

                       Button receiveButton = button.GetComponent<Button>();
                    if (receiveButton != null)
                    {
                  
                        receiveButton.onClick.AddListener(() => {

                            ReceiveReward(response.data[index].price);
                        });

                    }
                     
                       newAchievement.transform.GetChild(2).gameObject.SetActive(false);
                }
                else {
                    newAchievement.transform.GetChild(3).gameObject.SetActive(false);
                       newAchievement.transform.GetChild(2).gameObject.SetActive(false);
                }

                newAchievement.transform.SetParent(parent.transform, false);
            }
        }
    }


    void ReceiveReward(int price){

      
        var panel = Instantiate(rewardPanel);
        panel.transform.SetParent(canvas.transform, false);

        panel.SetActive(true);

        panel.GetComponent<RewardReceivingManager>().GetData(price);


    }
}
