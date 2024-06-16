using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private RectTransform monsterMinimapPrefab;
    [SerializeField] private RectTransform missileMinimapPrefab;
    [SerializeField] private int maxMonstersMinimap = 50;
    [SerializeField] private int maxMissilesMinimap = 5;
    [SerializeField] private float delayUpdateMinimap = 0.1f;
    [SerializeField] private float worldRadiusDimension = 5f;

    RectTransform[] monstersMinimap;
    RectTransform[] missilesMinimap;
    WaitForSeconds waitUpdateMinimap;
    Transform player;

    private void Awake()
    {
        monstersMinimap = new RectTransform[maxMonstersMinimap];
        missilesMinimap = new RectTransform[maxMissilesMinimap];


        InstantiateMinimapIcons(monstersMinimap, monsterMinimapPrefab);
        InstantiateMinimapIcons(missilesMinimap, missileMinimapPrefab);

        waitUpdateMinimap = new WaitForSeconds(delayUpdateMinimap);
    }

    void InstantiateMinimapIcons(RectTransform[] minimapIcons, RectTransform prefab)
    {
        for (int i = 0; i < minimapIcons.Length; i++)
        {
            var m = Instantiate(prefab, circle.transform);
            m.gameObject.SetActive(false);
            minimapIcons[i] = m;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnBattling += BattlingHanlder;
        GameManager.Instance.OnRestart += RestartHandler;
    }    

    private void OnDisable()
    {
        GameManager.Instance.OnBattling -= BattlingHanlder;
        GameManager.Instance.OnRestart -= RestartHandler;
    }
    
    private void BattlingHanlder(int arg2)
    {        
        StartCoroutine(MinimapRoutine());
    }

    private void RestartHandler()
    {
        StopAllCoroutines();
    }

    IEnumerator MinimapRoutine()
    {
        var monsters = GameManager.Instance.Monsters;
        var missiles = GameManager.Instance.Missiles;
        player = GameManager.Instance.Player;

        while (true)
        {
            DeactivateMinimapIcons();
            
            for (int i = 0; i < monsters.Count; i++)
            {
                var monster = monsters[i];                
                ShowMinimapIconFromWorldPosition(monster.transform, monstersMinimap[i], monster.CurrentColor);                
            }

            for (int i = 0; i < missiles.Count; i++)
            {
                var missile = missiles[i];                
                ShowMinimapIconFromWorldPosition(missile.transform, missilesMinimap[i], missile.CurrentColor, true);                
            }

     

            yield return waitUpdateMinimap;
        }
    }


    void ShowMinimapIconFromWorldPosition(Transform entity, RectTransform icon, Color color, bool rotate = false)
    {
        Vector3 position = entity.position;
        var positionRelativeToPlayer = GetPositionRelativeToPlayer(position, worldRadiusDimension);
        var minimapPosition = GetMinimapPosition(positionRelativeToPlayer);
        ActivateMinimapIcon(icon, minimapPosition, color);

        if (rotate)
        {
            var forward = entity.forward;
            forward.y = 0;
            var direction = player.InverseTransformDirection(forward);
            float angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);
            icon.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }

    void DeactivateMinimapIcons()
    {
        for (int i = 0; i < monstersMinimap.Length; i++)
        {
            monstersMinimap[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < missilesMinimap.Length; i++)
        {
            missilesMinimap[i].gameObject.SetActive(false);
        }
    }

    Vector3 GetPositionRelativeToPlayer(Vector3 position, float maxLength)
    {
        var positionRelativeToPlayer = player.InverseTransformPoint(position);
        positionRelativeToPlayer.y = 0;
        positionRelativeToPlayer = Vector3.ClampMagnitude(positionRelativeToPlayer, maxLength);
        return positionRelativeToPlayer;
    }

    Vector2 GetMinimapPosition(Vector3 localPosition)
    {
        float sizeImage = circle.rectTransform.rect.size.x;
        float diameterWorld = worldRadiusDimension * 2f;
        float scaleRatio = sizeImage / diameterWorld;

        var minimapPosition = localPosition * scaleRatio;
        minimapPosition.y = minimapPosition.z;
        minimapPosition.z = 0;

        return minimapPosition;
    }

    void ActivateMinimapIcon(RectTransform minimapIcon, Vector2 position, Color color)
    {
        minimapIcon.anchoredPosition = position; 
        minimapIcon.gameObject.SetActive(true);
        minimapIcon.GetComponent<Image>().color = color;
    }

}
