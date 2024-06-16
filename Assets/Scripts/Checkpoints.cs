using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Checkpoints : MonoBehaviour
{

    [SerializeField] private List<Transform> carList = new List<Transform>();
    private List<Checkpoint> allCheckpointsList = new List<Checkpoint>();
    private List<int> nextCheckpointList = new List<int>();
    private List<int> currentLapList = new List<int>();
    int laps = 2;

    // timer
    float timeStart = 0;
    float countdown = 3;

    // only for player (AI doesn't need this)
    private float time = 0.00f;
    bool racing = false;
    private List<float> lapTimesList = new List<float>();
    private Checkpoint lastCheckpointPassed;

    // Missed checkpoint text
    [SerializeField] private TextMeshProUGUI missedText, timerText, lapText, finishText, placeText, bestLapText, positionText, countdownText;
    [SerializeField] private Button garageButton;

    public Checkpoint GetLastCheckpointPassed()
    {
        return this.lastCheckpointPassed;
    }


    private void Awake()
    {
        Transform allCheckpoints = transform.Find("Checkpoints");

        foreach(Transform checkpoint in allCheckpoints)
        {
            Checkpoint cp = checkpoint.GetComponent<Checkpoint>();
            cp.setCheckpoints(this);

            allCheckpointsList.Add(cp);
        }

        foreach(Transform car in carList)
        {
            nextCheckpointList.Add(0);
            currentLapList.Add(0);
            if (!car.gameObject.GetComponent<CarController>().IsAI())
            {
                car.gameObject.GetComponent<CarController>().SetCheckpoints(this.gameObject.GetComponent<Checkpoints>());
            }

        }
    }

    private void Update()
    {
        if (timeStart == 0)
        {
            timeStart = Time.realtimeSinceStartup;
            Debug.Log(timeStart);
        }
        countdownText.text = countdown.ToString();

        if (countdown >= 0)
        {
            if (countdown == 0)
            {
                Time.timeScale = 1;
            } else
            {
                Time.timeScale = 0;
            }

            if (Time.realtimeSinceStartup - timeStart > 2.5f)
            {
                countdown--;
                Debug.Log(countdown);
                timeStart += 2.5f;
            }

        } else
        {
            countdownText.gameObject.SetActive(false);
        }

        if (racing)
        {
            time += Time.deltaTime;
            timerText.text = time.ToString("0.00") + "s";

        }
    }

    public void ResetCheckpoints(Transform car)
    {
        nextCheckpointList[carList.IndexOf(car)] = 0;
        currentLapList[carList.IndexOf(car)] = 0;
    }

    public Checkpoint GetNextCheckpoint(Transform car)
    {
        int nextCheckpoint = nextCheckpointList[carList.IndexOf(car)];
        return allCheckpointsList[nextCheckpoint];
    }

    public void onCarThroughCheckpoint(Checkpoint checkpoint, Transform car)
    {
        int nextCheckpoint = nextCheckpointList[carList.IndexOf(car)];
        int currentCheckpoint = nextCheckpoint - 1;
        Checkpoint correctCheckpoint = allCheckpointsList[nextCheckpoint];

        if (currentCheckpoint >= 0 && !car.gameObject.GetComponent<CarController>().IsAI())
        {
            lastCheckpointPassed = allCheckpointsList[currentCheckpoint];
        }

        /*        if (nextCheckpointList.Count() > 0 && !car.gameObject.GetComponent<CarController>().IsAI())
                {
                    lastCheckpointPassed = allCheckpointsList[nextCheckpointList[carList.IndexOf(car)]-1];
                }*/


        if (allCheckpointsList.IndexOf(checkpoint) == nextCheckpoint)
        {
            nextCheckpointList[carList.IndexOf(car)] = (nextCheckpoint + 1) % allCheckpointsList.Count;  // (next checkpoint < count  -->  remainder = next checkpoint
            int currentLap = currentLapList[carList.IndexOf(car)];

            if (!car.gameObject.GetComponent<CarController>().IsAI())
            {
                missedText.gameObject.SetActive(false);
                correctCheckpoint.toggleView(false);

                // Werkt alleen bij 1V1
                if (currentLapList.Min() < currentLap)
                {
                    positionText.text = "position: 1/2";
                }
                else
                {
                    if (nextCheckpointList.Max() == nextCheckpoint+1)
                    {
                        positionText.text = "position: 1/2";
                    }
                    else
                    {
                        positionText.text = "position: 2/2";
                    }
                }
            } else
            {
                car.GetComponent<CarAgent>().CorrectCheckpoint();
            }

            if (allCheckpointsList.IndexOf(checkpoint) == 0)
            {
                if (!car.gameObject.GetComponent<CarController>().IsAI())
                {
                    if (currentLap == 0)
                    {
                        if (!car.gameObject.GetComponent<CarController>().IsAI())
                        {
                            lapText.gameObject.SetActive(true);
                        }
                        racing = true;
                        lapTimesList.Add(0.00f);
                    }
                    else
                    {
                        lapTimesList.Add(time - lapTimesList[lapTimesList.Count - 1]);
                    }
                }

                if (currentLap != laps)
                {
                    currentLap++;
                    currentLapList[carList.IndexOf(car)] = currentLap;

                    if (!car.gameObject.GetComponent<CarController>().IsAI())
                    {
                        lapText.text = "LAP " + currentLap + " / " + laps;
                    }
                } else
                {
                    racing = false;
                    finished(car);
                }

            }
        }
        else
        {
            if (!racing) return;
            if (!car.gameObject.GetComponent<CarController>().IsAI())
            {
                missedText.gameObject.SetActive(true);
                correctCheckpoint.toggleView(true);
                
            } else
            {
                car.GetComponent<CarAgent>().WrongCheckpoint();
            }
        }
    }

    private void finished(Transform car)
    {
        lapTimesList.RemoveAt(0);

        if (!car.gameObject.GetComponent<CarController>().IsAI())
        {
            finishText.gameObject.SetActive(true);
            placeText.gameObject.SetActive(true);
            bestLapText.gameObject.SetActive(true);
            garageButton.gameObject.SetActive(true);

            placeText.text = "1st place";
            bestLapText.text = "Best lap: " + lapTimesList.Min().ToString("0.00") + "s";
        }
    }
}
