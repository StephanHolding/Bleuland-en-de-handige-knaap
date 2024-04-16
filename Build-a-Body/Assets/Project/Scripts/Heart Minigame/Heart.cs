using System.Collections;
using System.Collections.Generic;
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
    public Image checkmark;
    public Animator cardicArrestAlarm;

    private RangeBar rangeBar;
    private int targetBPM;
    private bool heartIn;
    private bool playerIsInactive;
    private int currentBpm;
    private float timeAtLastPump;
    private float inactivityTime;
    private float winTimer;
    private Coroutine winConditionTimer;
    private Coroutine pumpingRoutine;
    private Coroutine waitForSecondPump;
    private int roundIndex;
    public List<float> bpmTimes = new List<float>();
    private bool won;


    private const float PUMP_TIME = 0.25f;
    private const int BPM_RANGE = 5;
    private const int BPM_LIST_RANGE = 20;
    private const float MAX_INACTIVITY_TIME = 2;
    private const float SECOND_PUMP_TIME = 1f;
    private const int BAR_MIN = 40;
    private const int BAR_MAX = 130;

    private void Start()
    {
        rangeBar = FindObjectOfType<RangeBar>();
        playerIsInactive = true;
        ChooseNewTargetBPM(roundRanges[roundIndex]);
        rangeBar.ToggleShowArrowOnScreen(true);
        ResetProgression();

        /*        for (int i = 0; i < BPM_LIST_RANGE; i++)
                {
                    bpmTimes.Add(i * 10);
                }*/
    }

    public void Update()
    {
        if (!won)
            CheckInactivity();

        WinProgressionCheckmark();

        rangeBar.PlaceArrow(currentBpm);

        //DEBUG ONLY REMOVE LATER
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PumpIn();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PumpOut();
        }
    }


    public void HeartPressed()
    {
        if (!heartIn)
        {
            PumpIn();
        }
        else
        {
            PumpOut();
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

            inactivityTime = Time.time;

            waitForSecondPump = StartCoroutine(WaitForSecondPump());
        }
    }

    public void PumpOut()
    {
        if (heartIn)
        {
            if (waitForSecondPump != null)
                StopCoroutine(waitForSecondPump);

            playerIsInactive = false;

            if (pumpingRoutine != null)
                StopCoroutine(pumpingRoutine);
            StartCoroutine(PumpingRoutine(false, 1.1f, 0.8f));
            heartIn = false;

            timeAtLastPump = Time.time;
            inactivityTime = Time.time;

            bpmTimes.Add(timeAtLastPump);
            if (bpmTimes.Count > BPM_LIST_RANGE)
            {
                bpmTimes.RemoveAt(0);
            }

            if (bpmTimes.Count > 1)
            {
                currentBpm = CalculateBPM();
            }
        }
    }

    private void ResetProgression()
    {
        if (winConditionTimer != null)
            StopCoroutine(winConditionTimer);

        winConditionTimer = null;
        checkmark.fillAmount = 0;
        winTimer = 0;
    }

    private void WinProgressionCheckmark()
    {
        if (won) return;

        if (WithinBPMRange() && winTimer < timeForWinCondition)
        {
            winTimer += Time.deltaTime;
            checkmark.fillAmount = winTimer / timeForWinCondition;
            return;
        }
        else if (!WithinBPMRange() && winTimer > 0)
        {
            winTimer -= Time.deltaTime;
            checkmark.fillAmount = winTimer / timeForWinCondition;
            return;
        }

        if (WithinBPMRange() && winTimer >= timeForWinCondition)
        {
            StartCoroutine(WinRound());
        }
    }

    private IEnumerator WinRound()
    {
        won = true;
        checkmark.fillAmount = 1;
        //play win sound effect

        yield return new WaitForSeconds(1);

        if (!MinigameWon())
        {
            ChooseNewTargetBPM(roundRanges[roundIndex]);
        }
        else
        {
            PlayerWon();
        }
    }

    private void ChooseNewTargetBPM(Vector2Int ranges)
    {
        targetBPM = Random.Range(ranges.x, ranges.y);
        rangeBar.SetGreenBarRange(BAR_MIN, BAR_MAX, targetBPM - BPM_RANGE, targetBPM + BPM_RANGE);
        ResetProgression();
        won = false;
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

    private IEnumerator WaitForSecondPump()
    {
        yield return new WaitForSeconds(SECOND_PUMP_TIME);
        heartIn = false;
        StartCoroutine(PumpingRoutine(false, 1.1f, 0.8f));
    }

    private int CalculateBPM()
    {
        float totalDiff = 0;

        for (int i = 0; i < bpmTimes.Count; i++)
        {
            if (i != 0)
            {
                float diff = bpmTimes[i] - bpmTimes[i - 1];
                totalDiff += diff;
            }
        }


        float average = totalDiff / (bpmTimes.Count - 1);

        print(average);
        float averageBpm;

        if (average > 0)
            averageBpm = 60 / average;
        else
            averageBpm = 0;


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
            if (Time.time - inactivityTime > MAX_INACTIVITY_TIME)
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
        if (won) return;

        ResetProgression();
        playerIsInactive = true;
        bpmTimes.Clear();

        cardicArrestAlarm.Play("Cardiac Arrest Alarm");
        //TODO: play alarm audio

        currentBpm = 0;
    }

    [ContextMenu("Player Win")]
    private void PlayerWon()
    {
        won = true;
        if (GameStateManager.instance.IsGamestate<HeartMinigameState>())
        {
            GameStateManager.instance.PlayerCompletedTask();
        }
    }

}
