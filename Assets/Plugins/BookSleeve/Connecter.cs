using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BookSleeve.Handle {
    public class Connecter {

        public static NetworkConfMaker networkConfig { get; set; }
        public static RedisConnection conn { get; private set; }
        public static bool connected { get; private set; }

        #region SetUP
        public static RedisConnection SetUp(NetworkConfMaker config = null) {
            if(config != null) {
                networkConfig = config;
            }
            if(CheckConfig()) {
                string ipAddress = networkConfig.ipAddress;
                int port = networkConfig.port;
                conn = new RedisConnection(ipAddress, port);
                connected = true;
                return conn;
            } else {
                connected = false;
                return null;
            }
        }
        #endregion

        #region Open
        public static async Task<bool>  Open() {
            if(CheckConfig()) {
                await conn.Open();
                return true;
            }
            return false;
        }
        #endregion

        #region Set
        public static async Task<bool> Set (KVS kvs) {
            if(CheckConfig()) {
                await conn.Strings.Set(db: networkConfig.useDB, key: kvs.key, value: kvs.value);
                return true;
            }
            return false;
        }
        #endregion

        #region Get
        public static async Task<String> Get(string Key) {
            if (CheckConfig()) {
                byte[] bytes = await conn.Strings.Get(db: networkConfig.useDB, key: Key);
                return new String(Array.ConvertAll(bytes, x => (char)x));
            }
            return "";
        }
        #endregion

        #region Ping
        public static async Task<int> Ping () {
            if(CheckConfig()) {
                 return (int) await conn.Ping();
            }
            return -1;
        }
        #endregion

        #region Behaviour
        public static void ConfigAttach(NetworkConfMaker config) {
            networkConfig = config;
        }

        public static bool CheckConfig() {
            if(networkConfig != null) {
                return true;
            }
            return false;
        }
        #endregion

    }

    [Serializable]
    public class KVS {
        public string key;
        public string value;
    }
}
