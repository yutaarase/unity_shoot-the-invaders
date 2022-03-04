using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの生成を管理するクラス
/// </summary>
public class SpawnControl : MonoBehaviour
{
    [Header("最大生成数")] public float maxSpawn;
    [Header("生成間隔")] public float spawnSpan;
    [Header("生成オブジェクト")] public GameObject Object;
    [Header("生成場所")]　public Transform Trans;

    private float currentSpawn;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        //変数初期化
        currentSpawn = 0;
        Object.SetActive(false);
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        //現在のスポーン数が最大値と同じだったら
        //自身を削除する
        if (currentSpawn == maxSpawn)
        {
            Destroy(this.gameObject);
        }

        //現在の経過時間がスポーンスパン以上だったら
        //オブジェクトのインスタンスを起こす
        //インスタンスを表示させる(スポナーにアタッチしているオブジェクトが非表示なため)
        if(currentTime >= spawnSpan)
        {
            var Instance=Instantiate(Object,Trans.position,Trans.rotation);
            Instance.SetActive(true);
            ++currentSpawn;
            currentTime = 0;
        }
        else
        {
            //超えてなかったら
            //経過時間を加算
            currentTime += Time.fixedDeltaTime;
        }
            
    }
}
