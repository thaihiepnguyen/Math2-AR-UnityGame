using UnityEngine;

public abstract  class BaseShooterController : MonoBehaviour
{
   
    [SerializeField] protected float damage = 1f;

    protected BulletFactory bulletFactory;
    
    protected virtual void Awake()
    {
        bulletFactory = GetComponentInChildren<BulletFactory>();
    }


    public virtual int FireBullet()
    {
        int gunIndex = bulletFactory.Fire();
        return gunIndex;
    }

    public virtual void DoDamage(GameObject opponent)
    {
        IDamageable damageable = opponent.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.Damage(damage, DamageMode.Shooting);
    }

}
