using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Colliderを反転させるクラス
/// </summary>
public class ReverseCollider : MonoBehaviour
{
    //お借りしたコードのサイト
    //https://raspberly.hateblo.jp/entry/2018/09/13/000000

    /// <summary>
    /// 既存のコライダーを削除するかどうか
    /// </summary>
    public bool removeExistingColliders = true;

    private void Start()
    {
        //メソッド呼び出し
        CreateInvertedMeshCollider();
    }

    /// <summary>
    /// 反転したメッシュコライダーを作成するメソッド
    /// </summary>
    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    /// <summary>
    /// 既存のコライダーを削除するメソッド
    /// </summary>
    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    /// <summary>
    /// Meshを反転させるメソッド
    /// </summary>
    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}