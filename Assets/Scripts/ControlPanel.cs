using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [Header("Conveyors")]
    public ConveyorBelt conveyor1;
    public ConveyorBelt conveyor2;
    public ConveyorBelt conveyor3;

    [Header("Worker / Machine / Robot")]
    public WorkerSpawner workerSpawner;
    public ProcessingMachine processingMachine;
    public RobotArmTransfer robotArm;

    [Header("Sliders 0 - 100")]
    public Slider conveyor1Slider;
    public Slider conveyor2Slider;
    public Slider conveyor3Slider;
    public Slider workerSlider;
    public Slider machineSlider;
    public Slider robotSlider;

    [Header("Default Values")]
    public float defaultConveyorSpeed = 50f;
    public float defaultWorkerSpeed = 50f;
    public float defaultMachineSpeed = 50f;
    public float defaultRobotSpeed = 50f;

    [Header("Speed Ranges")]
    public float minConveyorSpeed = 0.5f;
    public float maxConveyorSpeed = 8f;

    [Header("Worker Intervals")]
    public float slowWorkerInterval = 4f;
    public float fastWorkerInterval = 0.4f;

    [Header("Machine Delays")]
    public float slowMachineDelay = 4f;
    public float fastMachineDelay = 0.3f;

    [Header("Robot Durations")]
    public float slowRobotDuration = 2f;
    public float fastRobotDuration = 0.2f;

    private void Start()
    {
        SetupSlider(conveyor1Slider);
        SetupSlider(conveyor2Slider);
        SetupSlider(conveyor3Slider);
        SetupSlider(workerSlider);
        SetupSlider(machineSlider);
        SetupSlider(robotSlider);

        conveyor1Slider.value = defaultConveyorSpeed;
        conveyor2Slider.value = defaultConveyorSpeed;
        conveyor3Slider.value = defaultConveyorSpeed;

        workerSlider.value = defaultWorkerSpeed;
        machineSlider.value = defaultMachineSpeed;
        robotSlider.value = defaultRobotSpeed;

        conveyor1Slider.onValueChanged.AddListener(SetConveyor1);
        conveyor2Slider.onValueChanged.AddListener(SetConveyor2);
        conveyor3Slider.onValueChanged.AddListener(SetConveyor3);

        workerSlider.onValueChanged.AddListener(SetWorker);
        machineSlider.onValueChanged.AddListener(SetMachine);
        robotSlider.onValueChanged.AddListener(SetRobot);

        ApplyAllValues();
    }

    private void SetupSlider(Slider slider)
    {
        if (slider == null)
            return;

        slider.minValue = 0f;
        slider.maxValue = 100f;
    }

    private float Slider01(float value)
    {
        return Mathf.Clamp01(value / 100f);
    }

    private void SetConveyor1(float value)
    {
        if (conveyor1 != null)
        {
            conveyor1.speed =
                Mathf.Lerp(
                    minConveyorSpeed,
                    maxConveyorSpeed,
                    Slider01(value)
                );
        }
    }

    private void SetConveyor2(float value)
    {
        if (conveyor2 != null)
        {
            conveyor2.speed =
                Mathf.Lerp(
                    minConveyorSpeed,
                    maxConveyorSpeed,
                    Slider01(value)
                );
        }
    }

    private void SetConveyor3(float value)
    {
        if (conveyor3 != null)
        {
            conveyor3.speed =
                Mathf.Lerp(
                    minConveyorSpeed,
                    maxConveyorSpeed,
                    Slider01(value)
                );
        }
    }

    private void SetWorker(float value)
    {
        if (workerSpawner != null)
        {
            workerSpawner.workInterval =
                Mathf.Lerp(
                    slowWorkerInterval,
                    fastWorkerInterval,
                    Slider01(value)
                );
        }
    }

    private void SetMachine(float value)
    {
        if (processingMachine != null)
        {
            processingMachine.processingDelay =
                Mathf.Lerp(
                    slowMachineDelay,
                    fastMachineDelay,
                    Slider01(value)
                );
        }
    }

    private void SetRobot(float value)
    {
        if (robotArm != null)
        {
            robotArm.moveDuration =
                Mathf.Lerp(
                    slowRobotDuration,
                    fastRobotDuration,
                    Slider01(value)
                );
        }
    }

    private void ApplyAllValues()
    {
        SetConveyor1(conveyor1Slider.value);
        SetConveyor2(conveyor2Slider.value);
        SetConveyor3(conveyor3Slider.value);

        SetWorker(workerSlider.value);
        SetMachine(machineSlider.value);
        SetRobot(robotSlider.value);
    }
}