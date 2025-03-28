using UnityEngine;

public static class ProjectilePhysics
{
    /// <summary>
    /// Calculates the 2D position of a projectile at time `t`.
    /// </summary>
    /// <param name="initialVelocity">Initial velocity (m/s)</param>
    /// <param name="angleInDegrees">Launch angle (degrees)</param>
    /// <param name="time">Time in seconds</param>
    /// <param name="gravity">Gravity (positive value)</param>
    /// <returns>Position in 2D space</returns>
    public static Vector2 CalculatePosition(float initialVelocity, float angleInDegrees, float time, float gravity = 9.81f)
    {
        float angleRad = angleInDegrees * Mathf.Deg2Rad;

        float vx = initialVelocity * Mathf.Cos(angleRad);
        float vy = initialVelocity * Mathf.Sin(angleRad);

        float x = vx * time;
        float y = vy * time - 0.5f * gravity * time * time;

        return new Vector2(x, y);
    }
}