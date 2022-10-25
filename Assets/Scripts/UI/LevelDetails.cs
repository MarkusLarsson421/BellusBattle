using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelDetails : MonoBehaviour
{
    [SerializeField] private string scene;
    private Toggle toggle;
    private LevelManager levelManager;
    private Image image;
    private TextMeshProUGUI textMesh;
    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        toggle = GetComponent<Toggle>();
        image = GetComponent<Image>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = scene;
    }
    public bool GetToggle()
    {
        return toggle.isOn;
    }
    public string GetName()
    {
        return scene;
    }

    public void SetName(string scene)
    {
        this.scene = scene;
    }
    public void ToggleValueChanged()
    {
        levelManager.ChangeScenesToChooseFrom(this);
        if(toggle.isOn)image.color = Color.white;
        else image.color = Color.black;
    }
}
