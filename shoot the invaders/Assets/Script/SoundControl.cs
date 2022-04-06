using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Soundを操作するクラス
/// </summary>
public class SoundControl : MonoBehaviour
{
    [Header("音楽")] public AudioClip audioClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //インスタンスを初期化
        //音楽を再生する
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
