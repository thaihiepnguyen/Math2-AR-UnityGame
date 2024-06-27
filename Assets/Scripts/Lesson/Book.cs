using EasyUI.Progress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] float pageSpeed = 2f;
    //[SerializeField] List<Transform> pages;
    int index = 0;
    bool rotate = false;
    [SerializeField] Button backButton;
    [SerializeField] Button forwardButton;
    [SerializeField] GameObject pagePrefab;

    NoteBUS noteBus = new NoteBUS();
    List<NoteDTO> notes;
    
    NoteDTO baseNote = new NoteDTO(0,0,"Hãy tạo ra hân trời sáng tạo của riêng mình bằng cách học hỏi chăm chỉ !!","Ghi chú của em");

    private void Start()
    {
        InitialState();
    }

    public async void InitialState()
    {
        Progress.Show("Đang xử lý...", ProgressColor.Orange);
        var noteResponse = await noteBus.GetNotesByUserId();
        Progress.Hide();
        if (noteResponse.isSuccessful)
        {
            notes = noteResponse.data;
            notes.Insert(0, baseNote);
        }
        else
        {
            SceneHistory.GetInstance().PreviousScene();
        }
    }


    public void GoToNextNote()
    {
        if (index < notes.Count - 1)
        {
            backButton.interactable = true;
            StartCoroutine(RotateSequence());
            index++;
            pagePrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = notes[index].lesson_name;
            pagePrefab.transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = notes[index].note;
            pagePrefab.transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Left;
            if (index + 1 >= notes.Count) forwardButton.interactable = false;
        }
        else
        {
            forwardButton.interactable = false;
        }
    }
    public void GoToPrevNote()
    {
        if (index > 0)
        {
            forwardButton.interactable = true;
            StartCoroutine(RotateSequence());
            index--;
            pagePrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = notes[index].lesson_name;
            pagePrefab.transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = notes[index].note;
            if (index == 0)
            {
                pagePrefab.transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
            }
            else
            {
                pagePrefab.transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Left;

            }
            if (index - 1 < 0) backButton.interactable = false;
        }
        else
        {
            backButton.interactable = false;
        }
    }
    IEnumerator RotateSequence()
    {
        yield return Rotate(180);
        yield return Rotate(180);
    }
    IEnumerator Rotate(float angle)
    {
        float value = 0f;
        Quaternion startRotation = pagePrefab.transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        while (value < 1.0f)
        {
            value += Time.deltaTime * pageSpeed;
            pagePrefab.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, value);
            yield return null;
        }

        pagePrefab.transform.rotation = targetRotation; // Ensure final rotation is set
    }
}