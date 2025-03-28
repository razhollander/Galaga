public class HealthDataService
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    public HealthDataService(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }
}