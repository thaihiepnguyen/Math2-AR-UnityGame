using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;
    static protected int coin;
    public static int GetCoin(){
        return coin;
    }
    // Start is called before the first frame update
    async void Start() {
        UserBUS userBus = new();

        var response = await userBus.GetMe();

        if (response.isSuccessful) {
            var me = response.data;
            coinText.text = me.coin.ToString();
            coin = me.coin;
        }
    }
}
