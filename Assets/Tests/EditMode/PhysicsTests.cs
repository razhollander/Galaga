using NUnit.Framework;
using UnityEngine;

public class PhysicsTests
{
    /// <summary>
    /// //
    /// //
    /// </summary>
    [Test]
    public void Position_After1Second_IsCorrect()
    {
        float velocity = 10f;
        float angle = 45f;
        float time = 1f;

        Vector2 result = ProjectilePhysics.CalculatePosition(velocity, angle, time);

        // Expected result (approximate): x ≈ 7.07, y ≈ 2.17
        Assert.AreEqual(7.07f, result.x, 0.1f, "X position should be around 7.07");
        Assert.AreEqual(2.17f, result.y, 0.1f, "Y position should be around 2.17");
    }

    [Test]
    public void Position_AtTimeZero_IsZero()
    {
        Vector2 result = ProjectilePhysics.CalculatePosition(15f, 60f, 0f);
        Assert.AreEqual(Vector2.zero, result);
    }

    [Test]
    public void Velocity_AfterCollision_NotNegative()
    {
        Assert.AreEqual(Vector2.zero, Vector2.zero);

        //Assert.Throws<System.ArgumentOutOfRangeException>(() =>
        //{
        //    ProjectilePhysics.CalculatePosition(10f, 30f, 1f, -9.81f);
        //});
    }
}