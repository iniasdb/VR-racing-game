using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Checkpoints : MonoBehaviour
{

    [SerializeField] private List<Transform> carList = new List<Transform>();
    private List<Checkpoint> allCheckpointsList = new List<Checkpoint>();
    private List<int> nextCheckpointList = new List<int>();
    private List<int> currentLapList = new List<int>();
    int laps = 2;

    // only for player (AI doesn't need this)
    private float time = 0.00f;
    bool racing = false;
    private List<float> lapTimesList = new List<float>();

    // Missed checkpoint text
    [SerializeField] private TextMeshProUGUI missedText, timerText, lapText, finishText, placeText, bestLapText, positionText;
    [SerializeField] private Button garageButton;


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
        }

    }

    private void Update()
    {
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
        Checkpoint correctCheckpoint = allCheckpointsList[nextCheckpoint];

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
                    lapTimesList.Add(time- lapTimesList[lapTimesList.Count - 1]);
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
        Debug.Log("finished race");
        lapTimesList.RemoveAt(0);
        foreach (float lapTime in lapTimesList)
        {
            Debug.Log(lapTime);
        }

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
