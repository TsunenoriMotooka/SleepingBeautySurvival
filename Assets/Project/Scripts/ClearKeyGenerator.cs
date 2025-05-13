using System.Collections.Generic;
using UnityEngine;

public class ClearKeyGenerator : MonoBehaviour
{
    public GameObject clearKeyPrefab;
    
    Dictionary<Vector2, int> clearKeyPosDict = new Dictionary<Vector2, int>();
    Dictionary<(int, int), List<GameObject>> clearKeyDict = new Dictionary<(int, int), List<GameObject>>();

    GameObject clearKeys;

    public void Init()
    {
        clearKeys = new GameObject("ClearKeys");

        clearKeyPosDict.Clear();
        for (int i = 0; i < ClearKeyManager.GetInstance().Count(); i++) {
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

    public void InitOneChunk()
    {
        clearKeys = new GameObject("ClearKeys");

        clearKeyPosDict.Clear();
        while(true) {
            int x = Random.Range(-Const.chunkSizeX / 2, Const.chunkSizeX / 2);
            int y = Random.Range(-Const.chunkSizeY / 2, Const.chunkSizeY / 2);

            Vector2 position = new Vector2(x, y);
            if (clearKeyPosDict.ContainsKey(position)) continue;

            if (!ExistPositionManager.GetInstance().Contains(x, y)) {
                clearKeyPosDict[position] = 1;
                break;
            }
        }
    }

    public void GenerateClearKeys(int chunkX, int chunkY)
    {        
        int wx = chunkX % Const.fieldMatrixX;
        int wy = chunkY % Const.fieldMatrixY;

        List<GameObject> clearKeyList = new List<GameObject>();

        foreach (Vector2 clearKeyPos in clearKeyPosDict.Keys) {
            (int clearKeyChunkX, int clearKeyChunkY) = Utils.PositionToChunkMatrix(clearKeyPos);
            if (clearKeyChunkX == wx && clearKeyChunkY == wy) {
                Vector2 position = new Vector2(chunkX * Const.fieldSizeX, chunkY * Const.fieldSizeY) + clearKeyPos;
                GameObject clearKey = Instantiate(
                    clearKeyPrefab,
                    position,
                    Quaternion.identity);
                clearKey.transform.parent = clearKeys.transform;
                ClearKey script = clearKey.GetComponent<ClearKey>();
                if (script != null) script.destoryDelegate = () => {
                    if (clearKeyPosDict.ContainsKey(clearKeyPos)) {
                        clearKeyPosDict.Remove(clearKeyPos);
                        ClearKeyManager.GetInstance().Found();
                    }
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
