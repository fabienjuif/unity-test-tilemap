using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public float speed;
    public PlayerController playerController;
    private Vector2 move;

    private void Update()
    {
        this.move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        this.playerController.Move(this.move * this.speed * Time.fixedDeltaTime);
    }
}
