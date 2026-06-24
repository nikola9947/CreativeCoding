using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Goal")]
    public int targetPallets = 10;
    public float timeLimit = 60f;

    [Header("UI Texts")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI palletText;
    public TextMeshProUGUI[] extraTimerTexts;

    [Header("Result Images")]
    public GameObject winImage;
    public GameObject gameOverImage;

    [Header("Vitality Sliders")]
    public Slider workerSlider;
    public Slider machineSlider;
    public Slider robotSlider;
    public Slider palletSlider;

    [Header("Minigames")]
    public WorkerMiniGame workerMiniGame;
    public MachineColorMiniGame machineMiniGame;
    public RobotCalibrationMiniGame robotMiniGame;
    public BalanceMiniGame palletMiniGame;

    [Header("Production Stop")]
    public ConveyorBelt[] conveyors;

    [Header("Vitality Drain")]
    public float workerDrain = 0.020f;
    public float machineDrain = 0.015f;
    public float robotDrain = 0.010f;
    public float palletDrain = 0.025f;

    private int deliveredPallets = 0;
    private float timer;

    private float workerHealth = 1f;
    private float machineHealth = 1f;
    private float robotHealth = 1f;
    private float palletHealth = 1f;

    private bool gameStarted = false;
    private bool gameEnded = false;
    private bool stationBroken = false;

    private const string MINIGAME_REASON = "MINIGAME";
    private const string GAME_END_REASON = "GAME_END";
    private const string INTRO_REASON = "INTRO";

    private enum Station
    {
        Worker,
        Machine,
        Robot,
        Pallet
    }

    private Station brokenStation;

    private void Start()
    {
        timer = timeLimit;

        workerHealth = 1f;
        machineHealth = 1f;
        robotHealth = 1f;
        palletHealth = 1f;

        if (winImage != null)
            winImage.SetActive(false);

        if (gameOverImage != null)
            gameOverImage.SetActive(false);

        StopProductionIntro();

        UpdateVitalityUI();
        UpdateGameUI();
    }

    private void Update()
    {
        if (!gameStarted)
            return;

        if (gameEnded)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            LoseGame();
            return;
        }

        if (!stationBroken)
        {
            DrainVitality();
            CheckForBreakdown();
        }

        UpdateVitalityUI();
        UpdateGameUI();
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;
        ResumeProductionIntro();

        Debug.Log("GAME STARTED");
    }

    private void DrainVitality()
    {
        workerHealth = Mathf.Clamp01(workerHealth - workerDrain * Time.deltaTime);
        machineHealth = Mathf.Clamp01(machineHealth - machineDrain * Time.deltaTime);
        robotHealth = Mathf.Clamp01(robotHealth - robotDrain * Time.deltaTime);
        palletHealth = Mathf.Clamp01(palletHealth - palletDrain * Time.deltaTime);
    }

    private void CheckForBreakdown()
    {
        if (workerHealth <= 0f)
        {
            TriggerBreakdown(Station.Worker);
            return;
        }

        if (machineHealth <= 0f)
        {
            TriggerBreakdown(Station.Machine);
            return;
        }

        if (robotHealth <= 0f)
        {
            TriggerBreakdown(Station.Robot);
            return;
        }

        if (palletHealth <= 0f)
        {
            TriggerBreakdown(Station.Pallet);
            return;
        }
    }

    private void TriggerBreakdown(Station station)
    {
        stationBroken = true;
        brokenStation = station;

        StopProductionForMinigame();

        switch (station)
        {
            case Station.Worker:
                workerHealth = 0f;
                if (workerMiniGame != null)
                    workerMiniGame.StartMiniGame();
                break;

            case Station.Machine:
                machineHealth = 0f;
                if (machineMiniGame != null)
                    machineMiniGame.StartMiniGame();
                break;

            case Station.Robot:
                robotHealth = 0f;
                if (robotMiniGame != null)
                    robotMiniGame.StartMiniGame();
                break;

            case Station.Pallet:
                palletHealth = 0f;
                if (palletMiniGame != null)
                    palletMiniGame.StartMiniGame();
                break;
        }

        UpdateVitalityUI();
    }

    public void RepairCurrentStation()
    {
        if (gameEnded)
            return;

        switch (brokenStation)
        {
            case Station.Worker:
                workerHealth = 1f;
                break;

            case Station.Machine:
                machineHealth = 1f;
                break;

            case Station.Robot:
                robotHealth = 1f;
                break;

            case Station.Pallet:
                palletHealth = 1f;
                break;
        }

        stationBroken = false;

        ResumeProductionAfterMinigame();

        UpdateVitalityUI();
    }

    private void StopProductionIntro()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.AddStopReason(INTRO_REASON);
        }
    }

    private void ResumeProductionIntro()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.RemoveStopReason(INTRO_REASON);
        }
    }

    private void StopProductionForMinigame()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.AddStopReason(MINIGAME_REASON);
        }
    }

    private void ResumeProductionAfterMinigame()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.RemoveStopReason(MINIGAME_REASON);
        }
    }

    private void StopProductionForever()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.AddStopReason(GAME_END_REASON);
        }
    }

    public void PalletDelivered()
    {
        if (gameEnded || !gameStarted)
            return;

        deliveredPallets++;

        if (deliveredPallets >= targetPallets)
        {
            WinGame();
        }

        UpdateGameUI();
    }

    private void WinGame()
    {
        gameEnded = true;
        StopProductionForever();

        if (winImage != null)
            winImage.SetActive(true);

        if (gameOverImage != null)
            gameOverImage.SetActive(false);
    }

    private void LoseGame()
    {
        gameEnded = true;
        StopProductionForever();

        if (gameOverImage != null)
            gameOverImage.SetActive(true);

        if (winImage != null)
            winImage.SetActive(false);
    }

    private void UpdateVitalityUI()
    {
        if (workerSlider != null)
            workerSlider.value = workerHealth;

        if (machineSlider != null)
            machineSlider.value = machineHealth;

        if (robotSlider != null)
            robotSlider.value = robotHealth;

        if (palletSlider != null)
            palletSlider.value = palletHealth;
    }

    private void UpdateGameUI()
{
    string timeString = Mathf.CeilToInt(timer).ToString();

    if (timerText != null)
        timerText.text = timeString;

    foreach (TextMeshProUGUI text in extraTimerTexts)
    {
        if (text != null)
            text.text = timeString;
    }

    if (palletText != null)
        palletText.text = deliveredPallets + " / " + targetPallets;
}}