using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPooling : MonoBehaviour
{
    public static PlayerBulletPooling instance;
    private int playerMaxBulletCount = 20;
    private Transform tr;
    public List<GameObject> bulletPool;
    private GameObject bullet;

    void Awake()
    {
        instance = this;
        tr = transform;
        bulletPool = new List<GameObject>();
        bullet = Resources.Load<GameObject>("Bullet_Player");
        SetBulletPool();
    }

    private void SetBulletPool()
    {
        for (int i = 0; i <= playerMaxBulletCount; i++)
        {
            var bullets = Instantiate(bullet, tr);
            bullets.name = $"bullet_{i}";
            bullets.SetActive(false);
            bulletPool.Add(bullets);
        }
    }

    public GameObject GetBulletPool()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeSelf)
                return bulletPool[i];
        }
        return null;
    }



}
