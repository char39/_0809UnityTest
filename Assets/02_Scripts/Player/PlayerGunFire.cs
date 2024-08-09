using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    private Animator ani;
    public Transform firePos;
    public int bulletStack = 10;
    public int currentBullets = 0;
    public bool isNowReloading = false;
    public bool canFire = true;
    public MeshRenderer muzzFlash;

    void Start()
    {
        ani = GetComponent<Animator>();
        firePos = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Transform>();
        currentBullets = bulletStack;
        muzzFlash = firePos.GetChild(0).GetComponent<MeshRenderer>();
        muzzFlash.enabled = false;
    }

    void Update()
    {
        StartCoroutine(FireCtrl());
    }

    IEnumerator Reload()
    {
        if (isNowReloading)
        {
            currentBullets = bulletStack;
            ani.SetTrigger("ReloadTrigger");
            yield return new WaitForSeconds(3.0f);
            isNowReloading = false;
            canFire = true;
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator FireCtrl()
    {
        if (Input.GetMouseButtonDown(0) && currentBullets > 0 && canFire && !isNowReloading)
        {
            canFire = false;
            currentBullets--;
            StartCoroutine(FireOn());
            StartCoroutine(ShowMuzzleFlash());
        }
        if (currentBullets == 0 && !isNowReloading)
        {
            canFire = false;
            isNowReloading = true;
            StartCoroutine(Reload());
        }
        yield return new WaitForSeconds(0.01f);
    }

    IEnumerator FireOn()
    {
        var bullets = PlayerBulletPooling.instance.GetBulletPool();
        if (bullets != null)
        {
            bullets.transform.position = firePos.position;
            bullets.transform.rotation = firePos.rotation;
            bullets.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        canFire = true;
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
