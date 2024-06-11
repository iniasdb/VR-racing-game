using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTimer : MonoBehaviour
{

    private float time;
    private bool racing = false;
    private int laps = 1;
    private int currentLap = 0;
    private bool justPassedFinish = false;
    private float timeSincePassedFinish = 0;

    [SerializeField] private TextMeshProUGUI timerText;

    private void OnTriggerEnter(Collider other)
    {
        
        if (!justPassedFinish)
        {
            if (currentLap == laps)
            {
                racing = false;
            }
            else
            {
                racing = true;
            }

            if (racing)
            {
                currentLap++;
            }
        }

        justPassedFinish = true;
        timeSincePassedFinish = time;


    }

    // Update is called once per frame
    void Update()
    {
        if (racing)
        {
            if (time >= timeSincePassedFinish+1f)
            {
                justPassedFinish = false;
            }
            time += Time.deltaTime;
            timerText.text = time.ToString("0.00") + "s";
            
        }
    }
}
