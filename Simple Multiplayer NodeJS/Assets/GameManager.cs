using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake() => Instance = this;
    public float Movespeed;
    public Button mShootBullet;

    private void Start()
    {
       // mShootBullet.gameObject.SetActive(false);
    }
}
