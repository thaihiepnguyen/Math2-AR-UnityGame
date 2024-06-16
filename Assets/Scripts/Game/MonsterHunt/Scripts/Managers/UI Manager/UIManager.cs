using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{


    [Header("UI Game Panels")]
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject portalCreationPanel;
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject vfxPanel;
    [SerializeField] private GameObject winLevelPanel;
    [SerializeField] private GameObject minimapPanel;

    [Space(10)]
    [Header("UI Elements")]
    [SerializeField] private Image backgroundImage;    
    [SerializeField] private GameObject pointAtFloorMessage;
    [SerializeField] private GameObject tapToPlacePortalMessage;
    [SerializeField] private GameObject goMessage;
    [SerializeField] private Image playerHealthBarImage;
    [SerializeField] private Image splatImage;
    [SerializeField] private TMP_Text scoreTextGameOver;
    [SerializeField] private TMP_Text scoreTextWinLevel;



    [Space(10)]
    [Header("Settings")]
    [SerializeField] private Sprite[] splatSprites;
    [SerializeField] private float secondsToDeactivateGOMessage = 1;
    [SerializeField] private float timeToFadeBackground = 2f;
    [SerializeField] private float timeToFadeSplat = 1f;
    [SerializeField] private int splatImageRandomOffset = 200;
    [SerializeField] private float durationScoreIncrement = 5f;
    [SerializeField] private float minAlphaBackground = 0.5f;


    GameObject[] messagesPanelCenter;
    int score;
    int scorePreviousLevel;
    Coroutine uiRoutine;

    private void Awake()
    {
        messagesPanelCenter = new GameObject[] { backgroundPanel, mainPanel, portalCreationPanel, HUDPanel, battlePanel, 
            gameOverPanel, vfxPanel, winLevelPanel, minimapPanel };
        HideAllMessages();
    }
    
    public void Close() => GameManager.Instance.Close();
    
    public void StartGame() => GameManager.Instance.GameStarted();
    
    public void RestartGame() => GameManager.Instance.GameRestarted();
    
    private void OnEnable()
    {
        GameManager.Instance.OnMainMenuActivating += MainMenuHandler;
        GameManager.Instance.OnPortalCreating += PortalCreatingHandler;
        GameManager.Instance.OnStatusPortalChanged += StatusPortalHandler;
        GameManager.Instance.OnPortalCreated += PortalCreatedHandler;
        GameManager.Instance.OnBattling += BattlingHandler;
        GameManager.Instance.OnPlayerDamage += PlayerDamageHandler;
        GameManager.Instance.OnPlayerDead += PlayerDeadHandler;
        GameManager.Instance.OnGameOver += GameOverHandler;
        GameManager.Instance.OnScoreUpdated += ScoreUpdatedHandler;
   
        GameManager.Instance.OnWinLevel += WinLevelHandler;
        GameManager.Instance.OnRestart += RestartHandler;
    }    

    private void OnDisable()
    {
        GameManager.Instance.OnMainMenuActivating -= MainMenuHandler;
        GameManager.Instance.OnPortalCreating -= PortalCreatingHandler;
        GameManager.Instance.OnStatusPortalChanged -= StatusPortalHandler;
        GameManager.Instance.OnPortalCreated -= PortalCreatedHandler;
        GameManager.Instance.OnBattling -= BattlingHandler;
        GameManager.Instance.OnPlayerDamage -= PlayerDamageHandler;
        GameManager.Instance.OnPlayerDead -= PlayerDeadHandler;
        GameManager.Instance.OnGameOver -= GameOverHandler;
        GameManager.Instance.OnScoreUpdated -= ScoreUpdatedHandler;
   
        GameManager.Instance.OnWinLevel -= WinLevelHandler;
   
        GameManager.Instance.OnRestart -= RestartHandler;
    }

    private void RestartHandler() => scorePreviousLevel = 0;
    

    private void WinLevelHandler() => uiRoutine = StartCoroutine(WinLevelHandlerRoutine());
    
    IEnumerator WinLevelHandlerRoutine()
    {       
        DeactivatePanels(battlePanel);
        ActivatePanels(winLevelPanel);
        
        GameManager.Instance.InitIncrementScore();

        yield return StartCoroutine(SimpleTween.TweenRoutine(scorePreviousLevel, score, durationScoreIncrement,
            tween: (value) => scoreTextWinLevel.text = Mathf.CeilToInt(value).ToString(), 
            postTween: () => scoreTextWinLevel.text = score.ToString()));

        GameManager.Instance.EndIncrementScore();
    }
    
  
  

    private void ScoreUpdatedHandler(int score) => this.score = score;
    
    private void GameOverHandler(float delay)
    {
        StopCoroutine(uiRoutine);
        uiRoutine = StartCoroutine(GameOverHandlerRoutine(delay));
    }

    IEnumerator GameOverHandlerRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideAllMessages();        
        ActivatePanels(minimapPanel, backgroundPanel, HUDPanel, gameOverPanel);
        backgroundImage.canvasRenderer.SetAlpha(minAlphaBackground);
        scoreTextGameOver.text = score.ToString();
    }

    private void PlayerDeadHandler()
    {        
        DeactivatePanels(battlePanel);
        ActivatePanels(backgroundPanel);
        playerHealthBarImage.fillAmount = 0;
        ShowSplatBlood();
        FadeGraphic(backgroundImage, 1f, minAlphaBackground, timeToFadeBackground);
    }

    private void PlayerDamageHandler(float currentHealthPercentage)
    {
        playerHealthBarImage.fillAmount = currentHealthPercentage;
        ShowSplatBlood();
    }

    void ShowSplatBlood()
    {
        int x = Random.Range(splatImageRandomOffset, Screen.width - splatImageRandomOffset);
        int y = Random.Range(splatImageRandomOffset, Screen.height - splatImageRandomOffset);
        splatImage.transform.position = new Vector3(x, y, 0);

        int idx = Random.Range(0, splatSprites.Length);
        splatImage.sprite = splatSprites[idx];

        FadeGraphic(splatImage, 1f, 0f, timeToFadeSplat);       
    }

    void FadeGraphic(Graphic graphic, float fromAlpha, float toAlpha, float duration)
    {
        graphic.canvasRenderer.SetAlpha(fromAlpha);
        graphic.CrossFadeAlpha(toAlpha, duration, true);
    }

    private void BattlingHandler(int level) => uiRoutine = StartCoroutine(BattlingRoutine(level));
    
    IEnumerator BattlingRoutine(int level)
    {
        HideAllMessages();
       
        ActivatePanels(minimapPanel, HUDPanel, battlePanel, vfxPanel, goMessage);
      
        playerHealthBarImage.fillAmount = 1f; 
        splatImage.canvasRenderer.SetAlpha(0f);
        
        yield return new WaitForSeconds(secondsToDeactivateGOMessage);
        DeactivatePanels(goMessage);        
    }

    private void PortalCreatedHandler() => portalCreationPanel.SetActive(false);
    
    private void StatusPortalHandler(bool status)
    {
        pointAtFloorMessage.SetActive(!status);
        tapToPlacePortalMessage.SetActive(status);
    }

    private void MainMenuHandler()
    {        
        HideAllMessages();
        ActivatePanels(backgroundPanel, mainPanel);
        FadeGraphic(backgroundImage, 1f, 0f, timeToFadeBackground);
    }

    private void PortalCreatingHandler()
    {        
        HideAllMessages();
        ActivatePanels(portalCreationPanel);        
    }   

    void HideAllMessages()
    {
        for (int i = 0; i < messagesPanelCenter.Length; i++)
            messagesPanelCenter[i].SetActive(false);
    }

    void ActivatePanels(params GameObject[] panels)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(true);
    }

    void DeactivatePanels(params GameObject[] panels)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(false);
    }
}
