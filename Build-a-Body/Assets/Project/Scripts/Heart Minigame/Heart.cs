using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    [Header("Properties")]
    public int timeForWinCondition = 5;
    public Vector2Int[] roundRanges;

    [Header("Bones")]
    public Transform upperBone;
    public Transform lowerBone;

    [Header("UI References")]
    public QuicktimeEvent stageOne;
    public QuicktimeEvent stageTwo;
    public Image checkmark;
    public TextMeshProUGUI currentBpmText;
    public TextMeshProUGUI targetBpmText;
    public Animator cardicArrestAlarm;

    private int targetBPM;
    private bool heartIn;
    private bool playerIsInactive;
    private int currentBpm;
    private float timeAtLastPump;
    private Coroutine winConditionTimer;
    private Coroutine pumpingRoutine;
    private int roundIndex;
    private List<float> bpmTimes = new List<float>();


    private const float PUMP_TIME = 0.25f;
    private const int BPM_RANGE = 10;
    private const int BPM_LIST_RANGE = 10;
    private const int MAX_INACTIVITY_TIME = 3;

    private void Awake()
    {
        stageOne.OnEventSuccess += PumpIn;
        stageTwo.OnEventSuccess += PumpOut;

        stageOne.Enable();
        stageTwo.Disable();
        stageTwo.OnEventFailed += StartCardiacArrest;
    }

    private void Start()
    {
        playerIsInactive = true;
        ChooseNewTargetBPM(roundRanges[roundIndex]);
        ResetProgression();
    }

    public void Update()
    {
        CheckInactivity();

        //DEBUG ONLY REMOVE LATER
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            stageOne.DebugPress();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            stageTwo.DebugPress();
        }
    }


    public void PumpIn()
    {
        if (!heartIn)
        {
            if (pumpingRoutine != null)
                StopCoroutine(pumpingRoutine);
            StartCoroutine(PumpingRoutine(true, 0.9f, 1.2f));
            heartIn = true;

            stageTwo.EnableWithQuicktimeEvent(0.4f);
        }
    }

    public void PumpOut()
    {
        if (heartIn)
        {
            playerIsInactive = false;

            if (pumpingRoutine != null)
                StopCoroutine(pumpingRoutine);
            StartCoroutine(PumpingRoutine(false, 1.1f, 0.8f));
            heartIn = false;

            stageOne.Enable();

            if (bpmTimes.Count > 1)
            {
                currentBpm = CalculateBPM();
                currentBpmText.text = currentBpm.ToString();
            }

            timeAtLastPump = Time.time;

            bpmTimes.Add(timeAtLastPump);
            if (bpmTimes.Count > 10)
            {
                bpmTimes.RemoveAt(0);
            }

            if (WithinBPMRange())
            {
                if (winConditionTimer == null)
                    winConditionTimer = StartCoroutine(WinConditionTimer());
            }
            else
            {
                ResetProgression();
            }
        }
    }

    private void ResetProgression()
    {
        if (winConditionTimer != null)
            StopCoroutine(winConditionTimer);
        winConditionTimer = null;
        checkmark.fillAmount = 0;
    }

    private IEnumerator WinConditionTimer()
    {
        float startTime = Time.time;

        while (Time.time - startTime < timeForWinCondition)
        {
            float diffPercentage = (Time.time - startTime) / timeForWinCondition;
            checkmark.fillAmount = diffPercentage;
            yield return new WaitForEndOfFrame();
        }

        checkmark.fillAmount = 1;
        //play win sound effect

        yield return new WaitForSeconds(1);

        if (!MinigameWon())
        {
            ChooseNewTargetBPM(roundRanges[roundIndex]);
        }
        else
        {
            Debug.Log("WON!");
        }
    }

    private void ChooseNewTargetBPM(Vector2Int ranges)
    {
        targetBPM = Random.Range(ranges.x, ranges.y);
        targetBpmText.text = targetBPM.ToString();
        roundIndex++;
    }

    private bool MinigameWon()
    {
        return roundIndex >= roundRanges.Length;
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

        float averageBpm = 60 / average;


        return Mathf.RoundToInt(averageBpm);
    }

    private bool WithinBPMRange()
    {
        return currentBpm >= targetBPM - BPM_RANGE && currentBpm <= targetBPM + BPM_RANGE;
    }

    private void CheckInactivity()
    {
        if (!playerIsInactive)
        {
            if (Time.time - timeAtLastPump > MAX_INACTIVITY_TIME)
            {
                StartCardiacArrest();
            }
            else
            {
                cardicArrestAlarm.Play("Empty");
                //TODO: stop alarm audio
            }
        }
    }

    private void StartCardiacArrest()
    {
        ResetProgression();
        playerIsInactive = true;
        bpmTimes.Clear();

        cardicArrestAlarm.Play("Cardiac Arrest Alarm");
        //TODO: play alarm audio

        currentBpm = 0;
        currentBpmText.text = currentBpm.ToString();
    }

}
