using UnityEngine;

public abstract class BaseHit : MonoBehaviour
{
    [Header("TagsColliders")]
    [SerializeField] protected string target;
    //[SerializeField] protected string ally;
    //[SerializeField] protected string[] ignoredTags;
    [SerializeField] protected float damage = 10;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //if ((System.Array.Exists(ignoredTags, tag => other.CompareTag(tag))) || other.CompareTag(ally)) return;

        if (other.CompareTag(target))
        {
            ApplyEffect(other.gameObject);
        }

        Debug.Log("Choque con algo");
    }

    protected virtual void ApplyEffect(GameObject target)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage);
    }
}