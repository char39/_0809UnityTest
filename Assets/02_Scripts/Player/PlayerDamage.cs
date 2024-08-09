using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static float hp = 100.0f;
    public static bool isDie = false;

    public static void Hp(float value)
    {
        hp -= value;
    }

    void Update()
    {
        if (hp <= 0.0f)
        {
            isDie = true;
            Time.timeScale = 0.0f; // 걍 게임 멈추기
        }
    }
}
