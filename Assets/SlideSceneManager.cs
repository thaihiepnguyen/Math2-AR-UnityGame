using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    Button next;
    [SerializeField]
    Button previous;

    Image[] images;

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        images = parent.GetComponentsInChildren<Image>(true);

        next.onClick.AddListener(OnClickNext);

        previous.onClick.AddListener(OnClickPrevious);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickNext() {
        if (index + 2 == images.Length) {
            return;
        }

        Image thisImage = images[index];
        if (thisImage != null) {
            thisImage.gameObject.SetActive(false);
        }

        index += 2;
        Image nextImage = images[index];

        if (nextImage != null) {
            nextImage.gameObject.SetActive(true);
        }
    }

    void OnClickPrevious() {
        if (index == 0) {
            return;
        }

        Image thisImage = images[index];
        if (thisImage != null) {
            thisImage.gameObject.SetActive(false);
        }

        index -= 2;
        Image previousImage = images[index];

        if (previousImage != null) {
            previousImage.gameObject.SetActive(true);
        }
    }
}
