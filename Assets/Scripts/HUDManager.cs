using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Vitality Sliders")]
    public Slider workerSlider;
    public Slider machineSlider;
    public Slider robotSlider;
    public Slider palletSlider;

    [Header("Texts")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI palletText;

    [Header("Game Manager")]
    public GameManager gameManager;

    public void SetWorkerVitality(float value)
    {
        workerSlider.value = Mathf.Clamp01(value);
    }

    public void SetMachineVitality(float value)
    {
        machineSlider.value = Mathf.Clamp01(value);
    }

    public void SetRobotVitality(float value)
    {
        robotSlider.value = Mathf.Clamp01(value);
    }

    public void SetPalletVitality(float value)
    {
        palletSlider.value = Mathf.Clamp01(value);
    }

    public void UpdateTimer(float time)
    {
        timerText.text = "Time: " + Mathf.CeilToInt(time);
    }

    public void UpdatePallets(int delivered, int target)
    {
        palletText.text = "Pallets: " + delivered + "/" + target;
    }
}