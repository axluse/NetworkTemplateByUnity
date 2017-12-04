using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BookSleeve;

namespace UnityEngine.Networking {
    public class RedisDriver : MonoBehaviour {
        [SerializeField]
        private NetworkMasterSettings networkMasterSettings;
        private RedisConnection redisConnection;
        private bool protectionRedisConnectControllCD;

        void Awake() {
            OnEngineConnection(networkMasterSettings.ipAddress);
        }

        protected void OnEngineConnection(string ipAddress = "") {
            if (ipAddress == "") {
                Debug.LogError("Non Type ipAddress Entry");
            } else {
                StartCoroutine(EngineConnectionByRedis(ipAddress));
            }
        }

        private IEnumerator EngineConnectionByRedis(string ip) {
            yield return RedisConnection(ip);
        }

        private async Task<bool> RedisConnection(string ip) {
            redisConnection = new RedisConnection(ip);

            await redisConnection.Open();
            protectionRedisConnectControllCD = true;
            return true;
        }

        public async Task<bool> Insert (List<string> keys,List<string> datas) {
            if(protectionRedisConnectControllCD) {
                if(keys.Count == datas.Count) {
                    for (int i = 0; i < keys.Count; ++i) {
                        await redisConnection.Strings.Set(db: 0, key: keys[i], value: datas[i]);
                    }
                } else {
                    Debug.LogError($"RedisInsertError:配列のkeyの数が{keys.Count}個に対し、datasは{datas.Count}個です！これらは同じ値でなくてはなりません。個数に相違のあるListは必ず同数で管理します。");
                    return false;
                }
            } else {
                Debug.LogError("RedisInsertError:起動してません。");
                return false;
            }
            return true;
        }

        public async Task<string> Get(string key) {
            if (protectionRedisConnectControllCD) {
                byte[] data =  await redisConnection.Strings.Get(db: 0, key: key);
                string parthData = "";
                for(int i = 0; i < data.Length; ++i) {
                    parthData += data[i].ToString();
                }
                return parthData;
            } else {
                Debug.LogError("RedisInsertError:起動してません。");
                return "Error";
            }
        }
    }

    [CreateAssetMenu(menuName = "GameSettings/NetworkMasterSettings")]
    public class NetworkMasterSettings : ScriptableObject {
        public string ipAddress;
    }
}