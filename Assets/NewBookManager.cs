using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewBookManager : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    Button next;
    [SerializeField]
    Button previous;

    Button[] buttons;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttons = parent.GetComponentsInChildren<Button>();

        next.onClick.AddListener(OnClickNext);

        previous.onClick.AddListener(OnClickPrevious);
    }

    void OnClickNext() {
        if (index + 1 == buttons.Length) {
            return;
        }

        Button currentButton = buttons[index];
        Image thisImage = currentButton.GetComponentInChildren<Image>(true);
        if (thisImage != null) {
            thisImage.gameObject.SetActive(false);
        }

        index++;
        Button nextButton = buttons[index];
        Image nextImage = nextButton.GetComponentInChildren<Image>(true);

        if (nextImage != null) {
            nextImage.gameObject.SetActive(true);
        }
    }

    void OnClickPrevious() {
        if (index == 0) {
            return;
        }

        Button currentButton = buttons[index];
        Image thisImage = currentButton.GetComponentInChildren<Image>(true);
        if (thisImage != null) {
            thisImage.gameObject.SetActive(false);
        }

        index--;
        Button previousButton = buttons[index];
        Image previousImage = previousButton.GetComponentInChildren<Image>(true);

        if (previousImage != null) {
            previousImage.gameObject.SetActive(true);
        }
    }
}
