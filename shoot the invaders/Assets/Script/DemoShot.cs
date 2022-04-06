using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ選択画面の機体の機関銃を発射するクラス
/// </summary>
public class DemoShot : MonoBehaviour
{
    [Header("弾のオブジェクト")]public GameObject bullet;
    [Header("弾の発射位置")]public Transform shotPoint;
    [Header("弾の発射間隔")] public float shotInterval;
    [Header("発射音")] public AudioClip audioClip;
    //AudioSource
    private AudioSource audioSource;
    //経過時間
    private float time;

    private void Start()
    {
        //インスタンス初期化
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //左クリックで弾を発射する
        if (Input.GetMouseButton(0))
        {
            //経過時間が発射間隔以上だったら
            //弾のインスタンスを起こす
            time += Time.deltaTime;
            if (time >= shotInterval)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                Instantiate(bullet, shotPoint.position, Quaternion.LookRotation(transform.forward * 2));
                time = 0f;
            }
        }
    }
}
