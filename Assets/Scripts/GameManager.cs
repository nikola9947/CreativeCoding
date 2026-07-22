using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    [System.Serializable]
    public class DifficultySettings
    {
        public int targetPallets = 10;
        public float timeLimit = 60f;

        public float workerDrain = 0.020f;
        public float machineDrain = 0.015f;
        public float robotDrain = 0.010f;
        public float palletDrain = 0.025f;
    }

    [Header("Difficulty")]
    public DifficultySettings easySettings;
    public DifficultySettings mediumSettings;
    public DifficultySettings hardSettings;

    private DifficultySettings currentDifficulty;
    

    [Header("Spawner")]
    public WorkerSpawner workerSpawner;

    [Header("Conveyors")]
    public ConveyorBelt[] conveyors;

    [Header("Outro")]
    public OutroManager outroManager;

    [Header("Minigames")]
    public WorkerMiniGame workerMiniGame;
    public MachineColorMiniGame machineMiniGame;
    public RobotCalibrationMiniGame robotMiniGame;
    public BalanceMiniGame palletMiniGame;


    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI palletText;
    public TextMeshProUGUI[] extraTimerTexts;

    [Header("Vitality")]
    public Slider workerSlider;
    public Slider machineSlider;
    public Slider robotSlider;
    public Slider palletSlider;



    private int targetPallets;
    private int deliveredPallets;

    private float timer;

    private float workerDrain;
    private float machineDrain;
    private float robotDrain;
    private float palletDrain;

    private float workerHealth;
    private float machineHealth;
    private float robotHealth;
    private float palletHealth;

    private bool gameStarted;
    private bool gameEnded;
    private bool stationBroken;




    private const string INTRO_REASON = "INTRO";
    private const string MINIGAME_REASON = "MINIGAME";
    private const string GAME_END_REASON = "GAME_END";



    private enum Station
    {
        Worker,
        Machine,
        Robot,
        Pallet
    }

    private Station brokenStation;

    public static bool SkipIntro = false;


    private void Start()
    {
        ApplyDifficulty(mediumSettings);

        ResetGameState();

        StopProduction(INTRO_REASON);

        UpdateVitalityUI();
        UpdateGameUI();
    }


    private void Update()
    {
        if (!gameStarted || gameEnded)
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


    public void ApplyEasyDifficulty()
    {
        ApplyDifficulty(easySettings);
    }

    public void ApplyMediumDifficulty()
    {
        ApplyDifficulty(mediumSettings);
    }

    public void ApplyHardDifficulty()
    {
        ApplyDifficulty(hardSettings);
    }

    private void ApplyDifficulty(DifficultySettings settings)
    {
        currentDifficulty = settings;

        targetPallets = settings.targetPallets;
        timer = settings.timeLimit;

        workerDrain = settings.workerDrain;
        machineDrain = settings.machineDrain;
        robotDrain = settings.robotDrain;
        palletDrain = settings.palletDrain;
    }


    private void ResetGameState()
    {
        deliveredPallets = 0;

        workerHealth = 1f;
        machineHealth = 1f;
        robotHealth = 1f;
        palletHealth = 1f;

        timer = currentDifficulty.timeLimit;

        gameStarted = false;
        gameEnded = false;
        stationBroken = false;

        workerMiniGame?.ResetMiniGame();
        machineMiniGame?.ResetMiniGame();
        robotMiniGame?.ResetMiniGame();
        palletMiniGame?.ResetMiniGame();
    }

    public void StartGame()
    {
        ResetGameState();

        if (workerSpawner != null)
            workerSpawner.StartSpawner();

        gameStarted = true;
        gameEnded = false;

        ResumeProduction(INTRO_REASON);
        ResumeProduction(MINIGAME_REASON);
        ResumeProduction(GAME_END_REASON);

        UpdateVitalityUI();
        UpdateGameUI();

        Debug.Log("GAME STARTED");
    }

    public void ResetForNewGame()
    {
        ResetGameState();

        ResumeProduction(INTRO_REASON);
        ResumeProduction(MINIGAME_REASON);
        ResumeProduction(GAME_END_REASON);

        UpdateVitalityUI();
        UpdateGameUI();
    }


    public bool IsMiniGameRunning()
    {
        return stationBroken;
    }

    public bool CanUseWorldInteraction()
    {
        return gameStarted && !gameEnded && !stationBroken;
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

        StopProduction(MINIGAME_REASON);

        switch (station)
        {
            case Station.Worker:
                workerHealth = 0f;
                workerMiniGame?.StartMiniGame();
                break;

            case Station.Machine:
                machineHealth = 0f;
                machineMiniGame?.StartMiniGame();
                break;

            case Station.Robot:
                robotHealth = 0f;
                robotMiniGame?.StartMiniGame();
                break;

            case Station.Pallet:
                palletHealth = 0f;
                palletMiniGame?.StartMiniGame();
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

        ResumeProduction(MINIGAME_REASON);

        UpdateVitalityUI();
    }


    public void PalletDelivered()
    {
        if (!gameStarted || gameEnded)
            return;

        deliveredPallets++;

        UpdateGameUI();

        if (deliveredPallets >= targetPallets)
            WinGame();
    }


    private void WinGame()
    {
        gameStarted = false;
        gameEnded = true;

        StopProduction(GAME_END_REASON);

        Debug.Log("YOU WIN");

        outroManager?.ShowWin();
    }

    private void LoseGame()
    {
        gameStarted = false;
        gameEnded = true;

        StopProduction(GAME_END_REASON);

        Debug.Log("GAME OVER");

        outroManager?.ShowGameOver();
    }



    private void StopProduction(string reason)
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.AddStopReason(reason);
        }
    }

    private void ResumeProduction(string reason)
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
                conveyor.RemoveStopReason(reason);
        }
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
        string time = Mathf.CeilToInt(timer).ToString();

        if (timerText != null)
            timerText.text = time;

        if (extraTimerTexts != null)
        {
            foreach (TextMeshProUGUI txt in extraTimerTexts)
            {
                if (txt != null)
                    txt.text = time;
            }
        }

        if (palletText != null)
            palletText.text = deliveredPallets + " / " + targetPallets;
    }


    public void RestartScene()
    {
        SkipIntro = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}