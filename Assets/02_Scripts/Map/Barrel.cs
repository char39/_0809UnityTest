using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public int hp = 5;
    public bool isExplode = false;
    public Transform mainCam_tr;
    public Camera mainCamera;

    public GameObject exlosionEff;

    void Start()
    {
        exlosionEff = Resources.Load<GameObject>("Explosion");
        mainCamera = Camera.main;
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet") && !isExplode)
            hp--;

        if (hp == 0 && !isExplode)
        {
            isExplode = true;
            Instantiate(exlosionEff, other.transform.position, Quaternion.identity);
            StartCoroutine(CameraShake());
        }
    }

    public IEnumerator CameraShake()
    {
        mainCam_tr = mainCamera.transform;
        for (int i = 30; i > 0; i--)
        {
            float ran = Random.Range(-0.1f, 0.1f) * i * 0.1f;
            Vector3 changePos = new Vector3(ran, ran, ran);
            mainCamera.transform.position += changePos;

            float angle = Random.Range(-2f, 2f) * i * 0.1f;
            Quaternion changeRot = Quaternion.Euler(angle, angle, angle);
            mainCamera.transform.rotation *= changeRot;

            yield return new WaitForSeconds(0.002f);
        }
        mainCamera.transform.position = mainCam_tr.position;
        mainCamera.transform.rotation = mainCam_tr.rotation;
    }
}
