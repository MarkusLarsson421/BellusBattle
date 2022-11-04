using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

[ExecuteAlways]
public class ContentX : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject spawnPoint;
    private static int amountOfObjectToSpawn;
    private static List<float> xPos = new List<float>();
    private static List<float> yPos = new List<float>();
    private static List<string> prefabName = new List<string>();
    private static List<Transform> tranformList = new List<Transform>();
    private string textMesh;


    private void Start()
    {
        textMesh = prefab.name;
        OnSceneGUI();
    }
    void OnSceneGUI()
    {
        if (!Application.isPlayer)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
            for (int i = 0; i < amountOfObjectToSpawn; i++) 
            {
                if (prefabName.ElementAt(i).Equals(textMesh + "(Clone)")) 
                {
                    GameObject p = Instantiate(prefab, tranformList.ElementAt(i));
                    p.transform.position = new Vector2(xPos.ElementAt(i), yPos.ElementAt(i));
                }
            }
        }
    }
    public void SpawnObject()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 10;
        pos = Camera.main.ScreenToWorldPoint(pos);
        spawnPoint.transform.position = pos;
        GameObject p = Instantiate(prefab, spawnPoint.transform);
        p.transform.parent = null;
        xPos.Add(p.transform.position.x);
        yPos.Add(p.transform.position.y);
        prefabName.Add(p.name);
        tranformList.Add(p.transform);
        amountOfObjectToSpawn++;
    }
}
