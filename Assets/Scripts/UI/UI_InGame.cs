using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider helthSlider;

    //skill image
    [SerializeField] private Image dashCooldownImage;
    [SerializeField] private Image crystalCooldownImage;
    [SerializeField] private Image blackholdCooldownImage;
    [SerializeField] private Image parryCooldownImage;
    [SerializeField] private Image swordCooldownImage;
    [SerializeField] private Image flaskCooldownImage;

    [SerializeField] private TextMeshProUGUI currencyText;
    private SkillManager skill;
    // Start is called before the first frame update
    void Start()
    {
        skill = SkillManager.instance;

        if (playerStats != null)
        {

            helthSlider.maxValue = playerStats.GetMaxHealthValue();
            helthSlider.value = playerStats.GetMaxHealthValue();
            playerStats.onHealthChanged += UpdateHealth;
        }


    }

    private void Update()
    {
        currencyText.text = PlayerManager.instance.currency.ToString();
        if (skill.dashSkill.dashUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                SetCoolDownOf(dashCooldownImage);

            CheckCoolDownOf(dashCooldownImage, skill.dashSkill.coolDown);
        }
        else
        {
            dashCooldownImage.fillAmount = 1;
        }

        if (skill.crystalSkill.crystalUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SetCoolDownOf(crystalCooldownImage);

            CheckCoolDownOf(crystalCooldownImage, skill.crystalSkill.coolDown);
        }
        else
        {
            crystalCooldownImage.fillAmount = 1;
        }

        if (skill.parrySkill.parryUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.E))
                SetCoolDownOf(parryCooldownImage);
            CheckCoolDownOf(parryCooldownImage, skill.parrySkill.coolDown);

        }
        else
        {
            parryCooldownImage.fillAmount = 1;
        }
        if (skill.blackholeSkill.blackholdUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.R))
                SetCoolDownOf(blackholdCooldownImage);

            CheckCoolDownOf(blackholdCooldownImage, skill.blackholeSkill.coolDown);
        }
        else
        {
            blackholdCooldownImage.fillAmount = 1;
        }

        if (skill.swordSkill.swordUnlocked)
        {
            if (Input.GetMouseButtonUp(1))
                SetCoolDownOf(swordCooldownImage);

            CheckCoolDownOf(swordCooldownImage, skill.swordSkill.coolDown);
        }
        else
        {
            swordCooldownImage.fillAmount = 1;
        }
        if (Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetCoolDownOf(flaskCooldownImage);

            CheckCoolDownOf(flaskCooldownImage, Inventory.instance.flaskCooldown);
        }
        else
        {
            flaskCooldownImage.fillAmount = 1;
        }
    }

    private void UpdateHealth()
    {
        helthSlider.value = playerStats.currentHealth;
    }

    private void OnDisable()
    {
        playerStats.onHealthChanged -= UpdateHealth;
    }

    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCoolDownOf(Image _image, float cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= (1 / cooldown) * Time.deltaTime;
        }
    }
}
