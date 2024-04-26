using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementList : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private GameObject parent;
    // Start is called before the first frame update
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
                     newAchievement.transform.GetChild(3).gameObject.SetActive(true);
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
}
