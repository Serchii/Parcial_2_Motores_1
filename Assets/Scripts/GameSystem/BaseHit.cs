using UnityEngine;

public abstract class BaseHit : MonoBehaviour
{
    [Header("TagsColliders")]
    [SerializeField] protected string target;
    [SerializeField] protected float damage = 10;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(target))
        {
            ApplyEffect(other.gameObject);
        }
    }

    protected virtual void ApplyEffect(GameObject target)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage);
    }
}