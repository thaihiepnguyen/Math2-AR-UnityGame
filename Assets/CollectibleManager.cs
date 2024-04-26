using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;
    // Start is called before the first frame update
    async void Start() {
        UserBUS userBus = new();

        var response = await userBus.GetMe();

        if (response.isSuccessful) {
            var me = response.data;
            Debug.Log(response.data.coin);
            coinText.text = me.coin.ToString();
        }
    }
}
