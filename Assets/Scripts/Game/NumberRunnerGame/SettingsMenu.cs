using UnityEngine ;
using UnityEngine.UI ;

public class SettingsMenu : MonoBehaviour {
   [Header ("space between menu items")]
   [SerializeField] Vector3 spacing ;

     [Space]
   [Header ("Main button rotation")]
   [SerializeField] float rotationDuration ;



   [Space]
   [Header ("Animation")]
   [SerializeField] float expandDuration ;
   [SerializeField] float collapseDuration ;

   [Space]
   [Header ("Fading")]
   [SerializeField] float expandFadeDuration ;
   [SerializeField] float collapseFadeDuration ;

   [SerializeField] GameObject welcomePanel;
  
   Button mainButton ;
   SettingsMenuItem[] menuItems ;

   //is menu opened or not
   bool isExpanded = false ;

   Vector3 mainButtonPosition ;
   int itemsCount ;

   void Start () {
      //add all the items to the menuItems array
      LeanTween.reset();
      itemsCount = transform.childCount - 1 ;
      menuItems = new SettingsMenuItem[itemsCount] ;
      for (int i = 0; i < itemsCount; i++) {
         // +1 to ignore the main button
         menuItems [ i ] = transform.GetChild (i + 1).GetComponent <SettingsMenuItem> () ;
      }

      mainButton = transform.GetChild (0).GetComponent <Button> () ;
      mainButton.onClick.AddListener (ToggleMenu) ;
      //SetAsLastSibling () to make sure that the main button will be always at the top layer
      mainButton.transform.SetAsLastSibling () ;

      mainButtonPosition = mainButton.GetComponent <RectTransform> ().anchoredPosition ;

      //set all menu items position to mainButtonPosition
      ResetPositions () ;
   }

   void ResetPositions () {
      for (int i = 0; i < itemsCount; i++) {
         menuItems [ i ].rectTrans.anchoredPosition = mainButtonPosition ;
      }
   }

   void ToggleMenu () {
      
      Debug.Log(mainButtonPosition.y);
      isExpanded = !isExpanded ;
      
    

      if (isExpanded) {
         //menu opened
         for (int i = 0; i < itemsCount; i++) {
         
            LeanTween.moveLocalY(menuItems[i].gameObject,  mainButtonPosition.y+ spacing.y*(i+1), .7f).setDelay(expandDuration).setEase(LeanTweenType.easeOutElastic);
            LeanTween.alpha(menuItems[i].gameObject,0f, .5f).setDelay(expandFadeDuration);

          

         }
      } else {
         //menu closed
         for (int i = 0; i < itemsCount; i++) {

       
            LeanTween.moveLocalY(menuItems[i].gameObject, mainButtonPosition.y,.7f).setDelay(collapseDuration).setEase(LeanTweenType.easeInElastic);
            LeanTween.alpha(menuItems[i].gameObject,0f, .5f).setDelay(collapseFadeDuration);

      
         }
        
      }

    //   //rotate main button arround Z axis by 180 degree starting from 0
      LeanTween.rotate(mainButton.gameObject, Vector3.forward * 180f, rotationDuration).setEase(LeanTweenType.linear);
   }

   public bool open = true;
   public void OnItemClick (int index) {
      //here you can add you logic 
      switch (index) {
         case 0: 
				//first button
            
              Debug.Log("Hello");
            welcomePanel.SetActive(open);
              open = !open;
          
            break ;
         case 1: 
				//second button
           SceneHistory.GetInstance().PreviousScene();
            break ;
        
      }
   }

   void OnDestroy () {
      //remove click listener to avoid memory leaks
      mainButton.onClick.RemoveListener (ToggleMenu) ;
   }
}