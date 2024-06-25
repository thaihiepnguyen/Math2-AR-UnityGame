using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(ARPlaneManager))]
public class BallARPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    [SerializeField]
    private GameObject mainUI;

    [SerializeField]
    private GameObject waitTracking;

    private GameObject[] rims;
    private BallGame ballGameInstance;
    private readonly int quantity = 3;
    private Vector3 spacing = new (0.7f, 0, 0);
    private bool flag = false;
    private List<ExerciseDTO> exerciseList;
    void Awake() 
    {
        rims = new GameObject[quantity];
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;
    }

    void Start() {
        ballGameInstance = GameObject.FindGameObjectWithTag("BallGameManager").GetComponent<BallGame>();
        exerciseList = ballGameInstance.exerciseList;
    }

    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if(args.added != null && !flag)
        {
            mainUI.SetActive(true);
            waitTracking.SetActive(false);
            ARPlane arPlane = args.added[0];
            for (int i = 0; i < quantity; i++) {
                rims[i] = Instantiate(placedPrefab, arPlane.transform.position + spacing*(i - 1), Quaternion.identity);
            }
        
            flag = true;
            UpdateRims(exerciseList[0]);
        }
    }

    public void UpdateRims(ExerciseDTO exerciseDTO) {
        string[] answers = exerciseDTO.answer.Split(",");
        for (int i = 0; i < answers.Length; i++) {
            rims[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
        }
    }
}
