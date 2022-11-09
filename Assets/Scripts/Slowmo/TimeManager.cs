using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float slowdownAmount = 0.05f;
    [SerializeField] private bool isSlowMo;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        Time.timeScale += 1f * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void FixedUpdate()
    {
        if (isSlowMo)
        {
            DoSlowMotion();
        }
    }

    private void DoSlowMotion()
    {
        Time.timeScale = slowdownAmount;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // The settings button for slow mo
    public void OnButtonPressed()
    {
        // invert the bool flag
        isSlowMo = !isSlowMo;
        DoSlowMotion();
    }
}
