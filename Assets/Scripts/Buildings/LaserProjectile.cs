using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 15f;
    private Transform target;
    private int damage;

    public void Init(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            Meteor meteor = target.GetComponent<Meteor>();
            if (meteor != null)
                meteor.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
