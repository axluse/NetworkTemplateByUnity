using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BookSleeve.Handle;

public class Sample : MonoBehaviour {

    [SerializeField] private NetworkConfMaker configure;
    public KVS data;

    private void Awake() {
        StartCoroutine(EntryServer());
    }

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(IObserver());
        }
	}

    private IEnumerator IObserver() {
        Debug.Log($"データセット開始:{System.DateTime.Now}");
        // データセット
        yield return Connecter.Set(data).Result;
        Debug.Log($"データセット完了:{System.DateTime.Now}");
        // データゲット
        Debug.Log($"データゲット開始:{System.DateTime.Now}");
        var result =  Connecter.Get(data.key).Result;
        yield return result;
        Debug.Log($"データゲット完了[結果]=>{result}:{System.DateTime.Now}");
    }

    private IEnumerator EntryServer() {
        Debug.Log($"サーバー接続開始:{System.DateTime.Now}");
        // 設定ファイルをアタッチ
        Connecter.SetUp(configure);
        // Redis起動
        yield return Connecter.Open();
        Debug.Log($"サーバー接続完了:{System.DateTime.Now}");
    }
	
}
