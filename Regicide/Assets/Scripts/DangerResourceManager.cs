using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DangerResourceManager : MonoBehaviour
{
    public int DangerLevel { get; set; }
    public int HungerLevel { get; set; }
    public int RawFood { get; set; }
    public int RawMaterial { get; set; }
    public int ProcessedMaterial { get; set; }
    public int CannonCompletion { get; set; }
    public int VassalNumber { get; set; }

    public TextMeshProUGUI dangerText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI raw_materialText;
    public TextMeshProUGUI processed_materialText;
    public TextMeshProUGUI cannonText;
    public TextMeshProUGUI vassalText;

    private const int HungerMax = 100;
    private const int DangerMax = 100;

    public Slider hungerSlider;
    public Slider dangerSlider;


    private void Awake()
    {
        DangerLevel = 100;
        HungerLevel = 50;
        RawFood = 10;
        RawMaterial = 0;
        ProcessedMaterial = 0;
        CannonCompletion = 0;
        VassalNumber = 30;

        StartCoroutine(HungerDrainRoutine());
        StartCoroutine(DangerBasedDrainRoutine());
        UpdateUI();
    }

    private void Update()
    {
        ClampValues();
        UpdateUI();
    }

    private void ClampValues()
    {
        HungerLevel = Mathf.Clamp(HungerLevel, 0, HungerMax);
        DangerLevel = Mathf.Clamp(DangerLevel, 0, DangerMax);
        CannonCompletion = Mathf.Clamp(CannonCompletion, 0, 100);
        RawFood = Mathf.Clamp(RawFood, 0, 600);
        RawMaterial = Mathf.Clamp(RawMaterial, 0, 450);
        ProcessedMaterial = Mathf.Clamp(ProcessedMaterial, 0, 300);
    }

    private void UpdateUI()
    {
        if (dangerText != null) dangerText.text = $"{DangerLevel}";
        if (hungerText != null) hungerText.text = $"{HungerLevel}";
        if (foodText != null) foodText.text = $"{RawFood}";
        if (raw_materialText != null) raw_materialText.text = $"{RawMaterial}";
        if (processed_materialText != null) processed_materialText.text = $"{ProcessedMaterial}";
        if (cannonText != null) cannonText.text = $"{CannonCompletion}%";
        if (vassalText != null) vassalText.text = $"{VassalNumber}";

        if (dangerSlider != null) dangerSlider.value = DangerLevel / (float)DangerMax;
        if (hungerSlider != null) hungerSlider.value = HungerLevel / (float)HungerMax;
    }


    private System.Collections.IEnumerator HungerDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            HungerLevel -= 1;
        }
    }

    private System.Collections.IEnumerator DangerBasedDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);

            if (DangerLevel <= 60)
            {
                bool drainFood = Random.value < 0.5f;

                if (drainFood && RawFood > 0)
                {
                    RawFood -= 1;
                    Debug.Log("[DangerDrain] -1 Raw Food (Danger ≤ 60)");
                }
                else if (!drainFood && RawMaterial > 0)
                {
                    RawMaterial -= 1;
                    Debug.Log("[DangerDrain] -1 Raw Material (Danger ≤ 60)");
                }
            }

            if (DangerLevel <= 40 && ProcessedMaterial > 0)
            {
                ProcessedMaterial -= 1;
                Debug.Log("[DangerDrain] -1 Processed Material (Danger ≤ 40)");
            }

            if (DangerLevel <= 20 && CannonCompletion > 0)
            {
                CannonCompletion -= 1;
                Debug.Log("[DangerDrain] -1% Cannon Progress (Danger ≤ 20)");
            }
        }
    }
}
