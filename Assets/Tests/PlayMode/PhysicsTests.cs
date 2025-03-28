using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PhysicsTests
{
    private GameObject _ball;
    private GameObject _ground;

    [SetUp]
    public void Setup()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);

        // Create a ball
        _ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _ball.transform.position = new Vector3(0, 5, 0);
        var rb = _ball.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.mass = 1f;

        // Create a ground
        _ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _ground.transform.position = new Vector3(0, 0, 0);
        _ground.transform.localScale = new Vector3(10, 1, 10);
        _ground.AddComponent<BoxCollider>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_ball);
        Object.DestroyImmediate(_ground);
    }

    [UnityTest]
    public IEnumerator BallFallsDueToGravity()
    {
        float initialY = _ball.transform.position.y;

        yield return new WaitForSeconds(0.5f);

        float newY = _ball.transform.position.y;
        Assert.Less(newY, initialY, "Ball should have fallen due to gravity.");
    }

    [UnityTest]
    public IEnumerator BallHitsGroundAndStops()
    {
        Rigidbody rb = _ball.GetComponent<Rigidbody>();

        yield return new WaitForSeconds(2f);

        Assert.Less(Mathf.Abs(rb.velocity.y), 0.1f, "Ball should have stopped after hitting the ground.");
    }

    [UnityTest]
    public IEnumerator RigidbodyMassAffectsFallSpeed_Fail()
    {
        // This will fail because in Unity, all objects fall at the same rate regardless of mass
        _ball.GetComponent<Rigidbody>().mass = 1000;

        float initialY = _ball.transform.position.y;
        yield return new WaitForSeconds(1f);
        float newY = _ball.transform.position.y;

        // WRONG: this is intentionally false
        Assert.Greater(newY, initialY, "Heavy objects shouldn't fall slower. This test will fail.");
    }

    [UnityTest]
    public IEnumerator AddingForceMovesBall()
    {
        Rigidbody rb = _ball.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.right * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        Assert.Greater(_ball.transform.position.x, 0, "Ball should have moved to the right.");
    }

    [UnityTest]
    public IEnumerator BallHasColliderAttached()
    {
        Collider col = _ball.GetComponent<Collider>();
        Assert.IsNotNull(col, "Ball should have a Collider.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator BallHasNoCollider_Fail()
    {
        Collider col = _ball.GetComponent<Collider>();

        // INTENTIONALLY WRONG: We assert it's null, but it exists
        Assert.IsNull(col, "This test will fail because the ball has a Collider.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator GravityIsWorking()
    {
        Assert.AreEqual(new Vector3(0, -9.81f, 0), Physics.gravity, "Gravity should be the default.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator GravityIsSetToZero_Fail()
    {
        Assert.AreEqual(Vector3.zero, Physics.gravity, "This test will fail unless gravity is zero.");
        yield return null;
    }
}
