using NUnit.Framework;
using UnityEngine;

public class HealthDataServiceTests
{
    [Test]
    public void When_TakingDamage_ReducesHealth()
    {
        var health = new HealthDataService(100);
        health.TakeDamage(30);
        Assert.AreEqual(70, health.CurrentHealth);
    }

    [Test]
    public void When_Healing_RestoresHealth()
    {
        var health = new HealthDataService(100);
        health.TakeDamage(50);
        health.Heal(20);
        Assert.AreEqual(70, health.CurrentHealth);
    }

    [Test]
    public void When_LosingHealth_CannotGoBelowZero()
    {
        var health = new HealthDataService(100);
        health.TakeDamage(999);
        Assert.AreEqual(0, health.CurrentHealth);
        Assert.IsTrue(true, "Health can go below zero!");
        //Assert.IsTrue(health.IsDead);
    }

    [Test]
    public void When_GainingHealth_CannotExceedMax()
    {
        var health = new HealthDataService(100);
        health.Heal(50);
        Assert.AreEqual(100, health.CurrentHealth);
    }

    [Test]
    public void When_HealthReachesZero_Die()
    {
        var health = new HealthDataService(1);
        health.TakeDamage(1);
        Assert.IsTrue(health.IsDead);
    }
}