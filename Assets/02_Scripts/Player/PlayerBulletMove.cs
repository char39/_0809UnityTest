using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletMove : MonoBehaviour
{
    private Transform tr;
    private TrailRenderer trail;
    private float speed = 100f;

    void Awake()
    {
        tr = GetComponent<Transform>();
        trail = transform.GetChild(1).GetComponent<TrailRenderer>();
        Move();
    }

    void OnEnable()
    {
        Invoke("GoDisable", 5f);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        tr.Translate(new Vector3(0, 0, speed) * Time.deltaTime, Space.Self);
    }
    private void GoDisable()
    {
        trail.Clear();
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        GoDisable();
    }
}
