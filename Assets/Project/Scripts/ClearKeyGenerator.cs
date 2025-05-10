using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Collections;

using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ClearKeyGenerator : MonoBehaviour
{
    public GameObject clearKeyPrefab;
    
    Dictionary<Vector2, int> clearKeyPosDict = new Dictionary<Vector2, int>();
    Dictionary<(int, int), List<GameObject>> clearKeyDict = new Dictionary<(int, int), List<GameObject>>();

    SemaphoreSlim semaphore = new SemaphoreSlim(1);

    GameObject clearKeys;

    public void Init()
    {
        clearKeys = new GameObject("ClearKeys");

        clearKeyPosDict.Clear();
        for (int i = 0; i < Const.clearKeyCount; i++) {
            while(true) {
                int x = Random.Range(-Const.fieldSizeX / 2, Const.fieldSizeX / 2);
                int y = Random.Range(-Const.fieldSizeY / 2, Const.fieldSizeY / 2);

                (int chunkX, int chunkY) = Utils.PositionToChunkMatrix(x, y);
                if (chunkX == 0 && chunkY == 0) continue;

                Vector2 position = new Vector2(x, y);
                if (clearKeyPosDict.ContainsKey(position)) continue;

                if (!ExistPositionManager.GetInstance().Contains(x, y)) {
                    clearKeyPosDict[position] = 1;
                    break;
                }
            }
        }
    }

    public void GenerateClearKeys(int chunkX, int chunkY)
    {        
        Vector2 chunkPos = Utils.chunkMatrixToPosition(chunkX, chunkY);

        List<GameObject> clearKeyList = new List<GameObject>();

        foreach (Vector2 clearKeyPos in clearKeyPosDict.Keys) {
            (int clearKeyChunkX, int clearKeyChunkY) = Utils.PositionToChunkMatrix(clearKeyPos);

            if (clearKeyChunkX == (chunkX + Const.chunkSizeX / 2) % Const.chunkSizeX - Const.chunkSizeX / 2 &&
                clearKeyChunkY == (chunkY + Const.chunkSizeY / 2) % Const.chunkSizeY - Const.chunkSizeX / 2 ) {

                int filedX = (int)chunkPos.x / Const.fieldSizeX;
                int filedY = (int)chunkPos.y / Const.fieldSizeX;

                Vector2 position = clearKeyPos + new Vector2(filedX * Const.fieldSizeX, filedY * Const.fieldSizeY);
                GameObject clearKey = Instantiate(
                    clearKeyPrefab,
                    position,
                    Quaternion.identity);
                clearKey.transform.parent = clearKeys.transform;
                ClearKey script = clearKey.GetComponent<ClearKey>();
                if (script != null) script.destoryDelegate = () => {
                    clearKeyPosDict.Remove(clearKeyPos);
                };

                clearKeyList.Add(clearKey);
            }
        }

        ClearClearKeys(chunkX, chunkY);
        if (clearKeyList.Count > 0) {
            clearKeyDict[(chunkX, chunkY)] = clearKeyList;
        }
    }

    public void ClearClearKeys(int chunkX, int chunkY)
    {
        if (!clearKeyDict.ContainsKey((chunkX, chunkY))) return;

        List<GameObject> clearKeyList = clearKeyDict[(chunkX, chunkY)];
        foreach(GameObject clearKey in clearKeyList) {
            ClearKey script = clearKey.GetComponent<ClearKey>();
            if (script != null) script.destoryDelegate = null;
            Destroy(clearKey);
        }
        clearKeyDict.Remove((chunkX, chunkY));
    }
}
