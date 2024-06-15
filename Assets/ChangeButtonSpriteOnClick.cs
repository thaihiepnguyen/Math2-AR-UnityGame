using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonSpriteOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Sprite _startSprite;
    [SerializeField] private Sprite _changeSprite;
    Image _image;
    private void Start()
    {
        _image= GetComponent<Image>();
    }
    public void changeSprite()
    {
        _image.sprite = _changeSprite;
        StartCoroutine(BackToStartSpriteAfterSecond(0.5f));
    }
    IEnumerator BackToStartSpriteAfterSecond(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _image.sprite = _startSprite;
    }
}
