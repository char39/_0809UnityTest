using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    private float hp;
    private float maxHp = 100f;
    
    void OnEnable()
    {
        hp = maxHp;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            hp -= 20f;
            hp = Mathf.Clamp(hp, 0f, 100f);
            if (hp <= 0f)
                Die();
        }
    }

    void Die()
    {
        GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
    }
}
