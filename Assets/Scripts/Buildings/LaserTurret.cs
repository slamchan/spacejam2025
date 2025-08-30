using UnityEngine;

public class LaserTurret : Building
{
    [Header("Turret Stats")]
    public float range = 10f;
    public float fireRate = 1f;
    public int damage = 10;

    [Header("Resource Costs")]
    public int powerRequired = 1;
    public int manpowerRequired = 1;

    [Header("References")]
    public Transform firePoint;
    public GameObject laserPrefab;

    private float fireCooldown = 0f;

    protected override void NewUpdate()
    {
        base.NewAwake();
        if (!CanShoot() | currentLevel < 1)
            return;

        fireCooldown -= Time.deltaTime;

        // Find nearest meteor in range
        Meteor target = FindNearestMeteor();
        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = 1f / fireRate;
        }
    }

    private bool CanShoot()
    {
        {
            return ResourceManager.Instance.HasEnough(ownerPlayerId, "power", powerRequired) &&
                   ResourceManager.Instance.HasEnough(ownerPlayerId, "pop", manpowerRequired);
        }
    }

    private Meteor FindNearestMeteor()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        Meteor closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            Meteor m = hit.GetComponent<Meteor>();
            if (m != null)
            {
                float dist = Vector2.Distance(transform.position, m.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = m;
                }
            }
        }
        return closest;
    }

    private void Shoot(Meteor target)
    {
        if (laserPrefab == null || firePoint == null) return;

        // make a new instance (clone)
        GameObject laserInstance = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);

        // get projectile component
        LaserProjectile proj = laserInstance.GetComponent<LaserProjectile>();
        if (proj != null)
        {
            proj.Init(target.transform, damage);
        }
    }

    protected override void ApplyUpgradeEffects(int upg)
    {
        range = 10 + currentLevel * 2;
        damage = 2 + currentLevel * 2;
        fireRate = 0.5f * currentLevel;

        owner.powerExpense += 1 * upg;
    }
}
