using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RobotUI : MonoBehaviour
{
    [Header("Target Robot")]
    public RobotStats targetRobot;

    [Header("UI Element")]
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public TextMeshProUGUI energyText;

    private void OnEnable()
    {
        if (targetRobot != null)
        {
            targetRobot.OnHPChanged += UpdateHPBar;
            targetRobot.OnEnergyChanged += UpdateEnergyText;
        }
    }

    private void OnDisable()
    {
        if (targetRobot != null)
        {
            targetRobot.OnHPChanged -= UpdateHPBar;
            targetRobot.OnEnergyChanged -= UpdateEnergyText;
        }
    }

    private void Start()
    {
        if (targetRobot != null && targetRobot.baseData != null)
        {
            if (nameText != null) nameText.text = targetRobot.baseData.characterName;
        }
    }

    private void UpdateHPBar(int currentHP, int maxHP)
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    private void UpdateEnergyText(int currentEnergy)
    {
        if (energyText != null) energyText.text = currentEnergy.ToString();
    }
}
