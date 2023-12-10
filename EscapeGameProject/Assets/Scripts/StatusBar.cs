using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum BarType
{
    Hp,
    Hunger
}
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerStatus player;
    public BarType barType;
    private float smoothness = 5f;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI hungerText;
    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        FindPlayerStatus();

        if (slider != null && player != null)
        {
            if(barType == BarType.Hp)
            {
            slider.maxValue = player.playerMaxHp;

            }
            else if(barType == BarType.Hunger) 
            {
            slider.maxValue = player.theMaxHunger;
            
            }
            UpdateHealthBar();
        }

    }

    private void Update()
    {
        if(barType == BarType.Hp) 
        {
        UpdateHealthBar();
        }
        else if (barType == BarType.Hunger)
        {
            UpdateHungerBar();
        }
        if(player == null)
        {
            FindPlayerStatus();
        }
    }

    private void UpdateHealthBar()
    {
        if (slider != null && player != null)
        {
            float targetValue = player.playerCurrentHp;
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothness);
            if(hpText != null)
            {
            hpText.text = $"{player.playerCurrentHp} / {player.playerMaxHp}";

            }
        }
    }

    private void UpdateHungerBar()
    {
        if (slider != null && player != null)
        {
            float targetValue = player.theCurrentStateOfHunger;
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothness);

            if(hungerText != null)
            {
            hungerText.text = $"{player.theCurrentStateOfHunger} / {player.theMaxHunger}";

            }
        }
    }


    private void FindPlayerStatus()
    {
        player = FindObjectOfType<PlayerStatus>();

    }
}