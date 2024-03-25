using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    public TextMeshProUGUI bpmText;
    public TextMeshProUGUI targetBpmText;
    public Image checkmark;
    public Transform upperBone;
    public Transform lowerBone;
    public Image aButton;
    public Image bButton;
    public int targetBPM;

    private const float PUMP_TIME = 0.25f;
    private const int BPM_RANGE = 10;
    private const int BPM_LIST_RANGE = 10;
    private bool heartIn;
    private float timer;
    private float lastBeat = -1;
    private int currentBpm;

    private List<float> bpmTimes = new List<float>();

    private void Start()
    {
        targetBPM = Random.Range(50, 151);
        targetBpmText.text = targetBPM.ToString();
        aButton.gameObject.SetActive(false);
        bButton.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            aButton.gameObject.SetActive(true);
            PumpIn();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            bButton.gameObject.SetActive(true);
            PumpOut();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            aButton.gameObject.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            bButton.gameObject.SetActive(false);
        }

        timer += Time.deltaTime;
        WithinBPMRange();
    }


    private void PumpIn()
    {
        if (!heartIn)
        {
            StopAllCoroutines();
            StartCoroutine(PumpingRoutine(true, 0.9f, 1.2f));
            heartIn = true;
        }

    }

    private void PumpOut()
    {
        if (heartIn)
        {
            StopAllCoroutines();
            StartCoroutine(PumpingRoutine(false, 1.1f, 0.8f));
            heartIn = false;

            if (bpmTimes.Count > 0)
            {
                currentBpm = CalculateBPM();
                bpmText.text = currentBpm.ToString();
            }

            bpmTimes.Add(timer);
            if (bpmTimes.Count > 10)
            {
                bpmTimes.RemoveAt(0);
            }
        }
    }

    private IEnumerator PumpingRoutine(bool upDown, float smallScaleTarget, float bigScaleTarget)
    {
        if (upDown)
        {
            while (lowerBone.localScale.x < bigScaleTarget)
            {
                lowerBone.localScale += new Vector3(Time.deltaTime, 0, Time.deltaTime) / PUMP_TIME;
                yield return null;
            }

            while (upperBone.localScale.x > smallScaleTarget)
            {
                upperBone.localScale -= new Vector3(Time.deltaTime, 0, Time.deltaTime) / PUMP_TIME;
                yield return null;
            }
        }
        else if (!upDown)
        {
            while (lowerBone.localScale.x > bigScaleTarget)
            {
                lowerBone.localScale -= new Vector3(Time.deltaTime, 0, Time.deltaTime) / PUMP_TIME;
                yield return null;
            }

            while (upperBone.localScale.x < smallScaleTarget)
            {
                upperBone.localScale += new Vector3(Time.deltaTime, 0, Time.deltaTime) / PUMP_TIME;
                yield return null;
            }
        }

        upperBone.transform.localScale = new Vector3(smallScaleTarget, upperBone.transform.localScale.y, smallScaleTarget);
        lowerBone.transform.localScale = new Vector3(bigScaleTarget, lowerBone.transform.localScale.y, bigScaleTarget);
    }

    private int CalculateBPM()
    {
        /*        bpmTime += Time.deltaTime;

                float multiplyBy = 60 / bpmTime;

                return Mathf.RoundToInt(bpmTracker * multiplyBy);*/

        /*        var beatSpeed = timer - lastBeat;
                return Mathf.RoundToInt(60 / beatSpeed);*/

        float totalDiff = 0;

        for (int i = 0; i < bpmTimes.Count; i++)
        {
            if (i != 0)
            {
                float diff = (bpmTimes[i] - bpmTimes[i - 1]);
                totalDiff += diff;
            }
        }

        float average = totalDiff / bpmTimes.Count;
        print(average);

        float averageBpm = 60 / average;


        return Mathf.RoundToInt(averageBpm);
    }

    private void WithinBPMRange()
    {
        if (currentBpm >= targetBPM - BPM_RANGE && currentBpm <= targetBPM + BPM_RANGE)
        {
            checkmark.gameObject.SetActive(true);
        }
        else
        {
            checkmark.gameObject.SetActive(false);
        }
    }

}
