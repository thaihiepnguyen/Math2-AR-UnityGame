using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem monsterDeadParticleColorsPrefab;
    [SerializeField] private ParticleSystem damageParticleColorsPrefab;
    [SerializeField] private ParticleSystem missileExplosionParticleColorsPrefab;
    [SerializeField] private ParticleSystem attackParticleColorsPrefab;
    [SerializeField] private ParticleSystem portalParticleColorsPrefab;
    [SerializeField] private float delayDamageMaterial = 0.1f;

    ParticleSystem monsterDeadParticleColorsInstance;
    ParticleSystem damageParticleColorsInstance;
    ParticleSystem missileExplosionParticleColorsInstance;
    ParticleSystem attackParticleColorsInstance;
    ParticleSystem portalParticleColorsInstance;


    private void OnEnable()
    {
        GameManager.Instance.OnMonsterDead += MonsterDeadHandler;
   
        GameManager.Instance.OnMonsterDamage += MonsterDamageHandler;
        GameManager.Instance.OnMissileDead += MissileDeadHandler;
        GameManager.Instance.OnMonsterAttacking += MonsterAttackingHandler;
        GameManager.Instance.OnPortalCreated += PortalCreatedHandler;
        GameManager.Instance.OnRestart += RestartHandler;
        GameManager.Instance.OnMonsterCreated += MonsterCreatedHandler;

    }

    private void OnDisable()
    {
        GameManager.Instance.OnMonsterDead -= MonsterDeadHandler;
        GameManager.Instance.OnMonsterDamage -= MonsterDamageHandler;
        GameManager.Instance.OnMissileDead -= MissileDeadHandler;
        GameManager.Instance.OnMonsterAttacking -= MonsterAttackingHandler;
        GameManager.Instance.OnPortalCreated -= PortalCreatedHandler;
        GameManager.Instance.OnRestart -= RestartHandler;
        GameManager.Instance.OnMonsterCreated -= MonsterCreatedHandler;
    }

    private void Start()
    {
        monsterDeadParticleColorsInstance = Instantiate(monsterDeadParticleColorsPrefab);
        damageParticleColorsInstance = Instantiate(damageParticleColorsPrefab);
        missileExplosionParticleColorsInstance = Instantiate(missileExplosionParticleColorsPrefab);
        attackParticleColorsInstance = Instantiate(attackParticleColorsPrefab);
        portalParticleColorsInstance = Instantiate(portalParticleColorsPrefab);
    }

    private void MonsterCreatedHandler(IVFXEntity monster)
    {
        if (monster.NormalMaterial)
            UseMaterialOnVFXEntity(monster.NormalMaterial, monster);
    }


    private void RestartHandler()
    {
        portalParticleColorsInstance.Stop();
    }

    private void PortalCreatedHandler()
    {
        var position = GameManager.Instance.Portal.transform.position;
        portalParticleColorsInstance.transform.position = position;
        portalParticleColorsInstance.Play();
    }

    private void MonsterAttackingHandler(IVFXEntity monster)
    {
        PlayParticleColorsInstance(attackParticleColorsInstance, monster.CurrentColor, monster.ExplosionPosition);
        
        if (monster.AttackMaterial)
            UseMaterialOnVFXEntity(monster.AttackMaterial, monster);
    }

    private void MonsterDamageHandler(IVFXEntity monster)
    {
        PlayParticleColorsInstance(damageParticleColorsInstance, monster.CurrentColor, monster.ExplosionPosition);
    }

 


    private void MissileDeadHandler(IVFXEntity missil)
    {
        PlayParticleColorsInstance(missileExplosionParticleColorsInstance, missil.CurrentColor, missil.ExplosionPosition);
    }

   

    IEnumerator UseMaterialOnVFXEntityAndRevertRoutine(Material material, IVFXEntity vfxEntity, float delay)
    {        
        UseMaterialOnVFXEntity(material, vfxEntity);
        yield return new WaitForSeconds(delay);
       
        if (vfxEntity.Renderers[0] != null)
            UseMaterialOnVFXEntity(vfxEntity.NormalMaterial, vfxEntity);               
    }

    private void MonsterDeadHandler(IVFXEntity monsterDead)
    {
        PlayParticleColorsInstance(monsterDeadParticleColorsInstance, monsterDead.CurrentColor, monsterDead.ExplosionPosition);
    }

    void PlayParticleColorsInstance(ParticleSystem particleColorsInstance, Color color, Vector3 position)
    {                
        var main = particleColorsInstance.main;
        var gradientColor = main.startColor;
        gradientColor.color = color;
        main.startColor = gradientColor;

        particleColorsInstance.transform.position = position;
        particleColorsInstance.Play();
    }

    void UseMaterialOnVFXEntity(Material material, IVFXEntity vfxEntity)
    {
        for (int i = 0; i < vfxEntity.Renderers.Length; i++)
        {
            var rend = vfxEntity.Renderers[i];
            rend.material = material;
        }
    }

}
