using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatus ps = collision.gameObject.GetComponent<PlayerStatus>();
            if (ps != null)
                ps.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}