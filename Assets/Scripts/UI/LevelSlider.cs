using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI textS;
    [SerializeField] private ScoreManager scoreManager;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 1;
        slider.maxValue = levelManager.GetScencesList().Count;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textS.text = slider.value.ToString();

    }
    public void OnPlay()
    {
        scoreManager.SetPointsToWin((int)slider.value);
        levelManager.LoadNextScene();
    }
}
