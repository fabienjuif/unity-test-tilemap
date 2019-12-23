using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public float speed;
    public PlayerController playerController;
    public Tilemap tilemap;
    public TileBase constructTile;
    private Vector2 move;
    private int constructed = 0;
    private bool canConstruct = false;

    private void Update()
    {
        this.move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButton("Fire1"))
        {
            if (this.canConstruct)
            {
                this.canConstruct = false;
                if (this.constructed == 0)
                {
                    this.constructed += 1;

                    this.tilemap.SetTile(new Vector3Int(-2, 7, 0), this.constructTile);
                    this.tilemap.SetTile(new Vector3Int(-3, 7, 0), this.constructTile);
                    this.tilemap.SetTile(new Vector3Int(-4, 7, 0), this.constructTile);
                    this.tilemap.SetTile(new Vector3Int(-3, 8, 0), this.constructTile);
                    this.tilemap.SetTile(new Vector3Int(-4, 8, 0), this.constructTile);
                    this.tilemap.SetTile(new Vector3Int(-3, 9, 0), this.constructTile);
                }
                else if (this.constructed == 1)
                {
                    this.constructed += 1;

                    this.tilemap.SetTile(new Vector3Int(-2, 9, 0), this.constructTile);
                    this.tilemap.SetTile(new Vector3Int(-2, 8, 0), this.constructTile);
                }
                else if (this.constructed == 2)
                {
                    this.constructed = 1;

                    this.tilemap.SetTile(new Vector3Int(-2, 8, 0), null);
                }
            }
        } else
        {
            this.canConstruct = true;
        }
    }

    private void FixedUpdate()
    {
        this.playerController.Move(this.move * this.speed * Time.fixedDeltaTime);
    }
}
