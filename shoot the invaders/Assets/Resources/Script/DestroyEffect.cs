using UnityEngine;
using System.Collections;

/// <summary>
/// エフェクトを削除するクラス
/// </summary>
public class DestroyEffect : MonoBehaviour {

    //パーティクル
    public ParticleSystem ex;
    //流す音楽
    public AudioClip clip;
    //AudioSource
    private AudioSource audioSource;

    private void Start()
    {
        //各インスタンス初期化
        //再生
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }
    private void Update()
    {
        //エフェクトの再生が終了したら自身を削除
        if (ex.isStopped)
        {
            Destroy(this.gameObject);
        }
		
    }
	
}

