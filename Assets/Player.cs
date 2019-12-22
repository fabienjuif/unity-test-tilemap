using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public new BoxCollider2D collider;
    public float speed;
    public Tilemap tilemap;
    private Vector2 move;
    private bool blockDown = false;
    private bool blockUp = false;
    private bool blockLeft = false;
    private bool blockRight = false;
    private ContactPoint2D[] contacts = new ContactPoint2D[10];

    private void Update()
    {
        this.move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        Vector2 velocity = this.move * this.speed * Time.fixedDeltaTime;
        if (this.blockDown && this.move.y < 0) velocity.y = 0;
        if (this.blockUp && this.move.y > 0) velocity.y = 0;
        if (this.blockLeft && this.move.x < 0) velocity.x = 0;
        if (this.blockRight && this.move.x > 0) velocity.x = 0;
        this.rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the collision is not the one set for tile collisions we exit
        if (collision.otherCollider != this.collider && collision.collider != this.collider) return;

        Vector3 position = this.transform.position;

        bool wasBlockDown = this.blockDown;
        bool wasBlockUp = this.blockUp;
        bool wasBlockLeft = this.blockLeft;
        bool wasBlockRight = this.blockRight;

        this.blockDown = false;
        this.blockUp = false;
        this.blockLeft = false;
        this.blockRight = false;

        Debug.Log("Position avant: " + position);
        int contactsSize = collision.GetContacts(this.contacts);
        for (int i = 0; i < contactsSize; i += 1)
        {
            ContactPoint2D contact = this.contacts[i];
            Debug.DrawLine(collision.transform.position, contact.point, Color.red);
            Debug.DrawLine(contact.point, contact.point + contact.normal, new Color(1, 0, 1));
            Debug.Log("Point: " + contact.point);
            Debug.Log("Normal: " + contact.normal);
            // it does not work every time :(
            // cell position is sometime bad!
            // edit: it seems to work... I just move the position too far sometimes!
            // Vector2 correctedContactPoint = contact.point;
            // if (contact.normal.y < 0.5f) correctedContactPoint.y += 1;
            // if (contact.normal.x < 0.5f) correctedContactPoint.x += 1;
            // Vector2 cellPosition = this.tilemap.CellToWorld(this.tilemap.WorldToCell(correctedContactPoint));
            Vector2 cellPosition = contact.point;

            if (contact.normal.y > 0.5f)
            {
                if (!wasBlockDown && !this.blockDown) position.y = cellPosition.y + this.collider.size.y / 2 - this.collider.offset.y;
                this.blockDown = true;
            } else if (contact.normal.y < -0.5f)
            {
                if (!wasBlockUp && !this.blockUp) position.y = cellPosition.y - this.collider.size.y / 2 - this.collider.offset.y;
                this.blockUp = true;
            }
            else if (contact.normal.x < -0.5f)
            {
                if (!wasBlockRight && !this.blockRight) position.x = cellPosition.x - this.collider.size.x / 2 - this.collider.offset.x;
                this.blockRight = true;
            }
            else if (contact.normal.x > 0.5f)
            {
                if (!wasBlockLeft && !this.blockLeft) position.x = cellPosition.x + this.collider.size.x / 2 - this.collider.offset.x;
                this.blockLeft = true;
            }
        }

        Debug.Log("Position apres: " + position);

        this.transform.position = position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // TODO: look at normals
        this.OnCollisionEnter2D(collision);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exit collision with: " + collision.gameObject.name);
        this.blockDown = false;
        this.blockLeft = false;
        this.blockRight = false;
        this.blockUp = false;
    }
}
