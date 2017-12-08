using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookSleeve.Handle {
    [CreateAssetMenu(menuName = "NetworkConfig", fileName = "NetworkConf")]
    public class NetworkConfMaker : ScriptableObject {
        public string gameName = "sample";
        public string ipAddress = "127.0.0.1";
        public int port = 6379;
        public int useDB = 0;
        public float interval = 0.25f;
    }
}
