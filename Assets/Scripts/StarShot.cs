using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class StarShot : Ball
{
    private void Awake()
    {
        Debug.Log("Star shot has awoken!");
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        GameObject newBall = Instantiate(Resources.Load("Prefabs/Ball")) as GameObject;
        newBall.GetComponent<Ball>().maxBounces = 5;
        newBall.GetComponent<Ball>().ballState = BallState.ACTIVE;
        newBall.transform.position = transform.position;
        newBall.transform.localScale = transform.localScale * 0.2f;

        float angle = UnityEngine.Random.Range(-30f, 30f);

        newBall.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, angle) * collision.GetContact(0).normal * GetComponent<Rigidbody2D>().velocity.magnitude * 2.0f;
        newBall.GetComponent<TrailRenderer>().widthMultiplier = 0.2f;
    }
}