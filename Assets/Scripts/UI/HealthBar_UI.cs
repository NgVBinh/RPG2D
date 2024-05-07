using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;

    private RectTransform myTransform;

    private CharacterStats characterStats;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        characterStats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;

        slider.maxValue = characterStats.GetMaxHealthValue();
        slider.value = characterStats.GetMaxHealthValue();
        characterStats.onHealthChanged += UpdateHealth;

    }

    private void UpdateHealth()
    {
        slider.value = characterStats.currentHealth;
    }

    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        characterStats.onHealthChanged -= UpdateHealth;
    }
}
