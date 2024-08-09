using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private Animator ani;
    private Transform playerTr;
    private Transform enemyTr;
    private Transform firePos;
    private float nextFire = 0.0f;
    private const float fireRate = 0.1f;
    private const float damping = 10.0f;
    public bool isFire = false;
    
    private const int maxBullet = 10;
    private int curBullet = 0;
    private bool isReload = false;
    public MeshRenderer muzzFlash;


    void Start()
    {
        ani = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        firePos = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Transform>();
        curBullet = maxBullet;
        muzzFlash = firePos.GetChild(0).GetComponent<MeshRenderer>();
        muzzFlash.enabled = false;
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFire)
            {
                StartCoroutine(Fire());
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            Vector3 playerLooknormal = playerTr.position - enemyTr.position;
            Quaternion rot = Quaternion.LookRotation(playerLooknormal);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, damping * Time.deltaTime);
        }    
    }
    
    IEnumerator Fire()
    {
        if (isReload)
        {
            isReload = false;
            StartCoroutine(Reload());
        }
        else if (curBullet > 0 && !isReload)
        {
            isReload = --curBullet % maxBullet == 0;

            RaycastHit hit;
            if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20.0f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerDamage.Hp(10.0f);
                }
            }

            StartCoroutine(ShowMuzzleFlash());
            yield return new WaitForSeconds(0.1f);

        }
    }
    IEnumerator Reload()
    {
        ani.SetTrigger("ReloadTrigger");
        yield return new WaitForSeconds(3.0f);
        curBullet = maxBullet;
    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzFlash.enabled = true;

        muzzFlash.transform.localScale = Vector3.one * Random.Range(0.8f, 1.5f);
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        muzzFlash.transform.localRotation = rot;

        yield return new WaitForSeconds(0.02f);
        muzzFlash.enabled = false;
    }

}

