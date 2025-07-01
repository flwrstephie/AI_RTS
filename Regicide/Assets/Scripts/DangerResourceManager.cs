using UnityEngine;
using TMPro;

public class DangerResourceManager : MonoBehaviour
{
    public int DangerLevel { get; set; }
    public int HungerLevel { get; set; }
    public int RawFood { get; set; }
    public int RawMaterial { get; set; }
    public int ProcessedMaterial { get; set; }
    public int CannonCompletion { get; set; }

    public TextMeshProUGUI dangerText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI raw_materialText;
    public TextMeshProUGUI processed_materialText;
    public TextMeshProUGUI cannonText;

    private const int HungerMax = 100;

    private void Awake()
    {
        DangerLevel = 0;
        HungerLevel = 50;
        RawFood = 10;
        RawMaterial = 0;
        ProcessedMaterial = 0;
        CannonCompletion = 0;

        StartCoroutine(HungerDrainRoutine());
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
    }

    private void UpdateUI()
    {
        if (dangerText != null) dangerText.text = $"{DangerLevel}";
        if (hungerText != null) hungerText.text = $"{HungerLevel}";
        if (foodText != null) foodText.text = $"{RawFood}";
        if (raw_materialText != null) raw_materialText.text = $"{RawMaterial}";
        if (processed_materialText != null) processed_materialText.text = $"{ProcessedMaterial}";
        if (cannonText != null) cannonText.text = $"{CannonCompletion}%";
    }

    private System.Collections.IEnumerator HungerDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            HungerLevel -= 1;
        }
    }
}
