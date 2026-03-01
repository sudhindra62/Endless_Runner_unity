using UnityEngine;

/// <summary>
/// The Ultra Combo fusion power-up. This is a combination of the Score Multiplier and Fever Mode.
/// It temporarily increases the multiplier cap, doubles the style bonus, and extends the combo timeout.
/// </summary>
public class UltraComboPowerUp : PowerUpEffect
{
    private ScoreManager scoreManager;
    private StyleManager styleManager;
    private FlowComboManager comboManager;

    private readonly int multiplierCapIncrease;
    private readonly float styleBonusMultiplier;
    private readonly float comboTimeoutExtension;

    private int originalMultiplierCap;
    private float originalStyleBonus;
    private float originalComboTimeout;

    public UltraComboPowerUp(float duration, int capIncrease, float styleMultiplier, float timeoutExtension) : base(duration)
    {
        this.multiplierCapIncrease = capIncrease;
        this.styleBonusMultiplier = styleMultiplier;
        this.comboTimeoutExtension = timeoutExtension;

        scoreManager = ServiceLocator.Get<ScoreManager>();
        styleManager = ServiceLocator.Get<StyleManager>();
        comboManager = ServiceLocator.Get<FlowComboManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (scoreManager != null)
        {
            originalMultiplierCap = scoreManager.GetMultiplierCap();
            scoreManager.SetMultiplierCap(originalMultiplierCap + multiplierCapIncrease);
        }

        if (styleManager != null)
        {
            originalStyleBonus = styleManager.GetBonusMultiplier();
            styleManager.SetBonusMultiplier(originalStyleBonus * styleBonusMultiplier);
        }

        if (comboManager != null)
        {
            originalComboTimeout = comboManager.GetTimeout();
            comboManager.SetTimeout(originalComboTimeout + comboTimeoutExtension);
        }

        Debug.Log("Ultra Combo Activated!");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (scoreManager != null)
        {
            scoreManager.SetMultiplierCap(originalMultiplierCap);
        }

        if (styleManager != null)
        {
            styleManager.SetBonusMultiplier(originalStyleBonus);
        }

        if (comboManager != null)
        {
            comboManager.SetTimeout(originalComboTimeout);
        }

        Debug.Log("Ultra Combo Deactivated.");
    }
}
