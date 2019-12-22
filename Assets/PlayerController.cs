using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public BoxCollider2D moveCollider;
    private bool blockDown = false;
    private bool blockUp = false;
    private bool blockLeft = false;
    private bool blockRight = false;
    private ContactPoint2D[] contacts = new ContactPoint2D[10];

    public void Move(Vector2 direction)
    {
        Vector2 velocity = direction;

        if (this.blockDown && velocity.y < 0) velocity.y = 0;
        if (this.blockUp && velocity.y > 0) velocity.y = 0;
        if (this.blockLeft && velocity.x < 0) velocity.x = 0;
        if (this.blockRight && velocity.x > 0) velocity.x = 0;
        
        this.rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the collision is not the one set for tile collisions we exit
        if (collision.otherCollider != this.moveCollider && collision.collider != this.moveCollider) return;

        Vector3 position = this.transform.position;

        bool wasBlockDown = this.blockDown;
        bool wasBlockUp = this.blockUp;
        bool wasBlockLeft = this.blockLeft;
        bool wasBlockRight = this.blockRight;

        this.blockDown = false;
        this.blockUp = false;
        this.blockLeft = false;
        this.blockRight = false;

        int contactsSize = collision.GetContacts(this.contacts);
        for (int i = 0; i < contactsSize; i += 1)
        {
            ContactPoint2D contact = this.contacts[i];

            Debug.DrawLine(collision.transform.position, contact.point, Color.red);
            Debug.DrawLine(contact.point, contact.point + contact.normal, new Color(1, 0, 1));

            if (contact.normal.y > 0.5f)
            {
                if (!wasBlockDown && !this.blockDown) position.y = contact.point.y + this.moveCollider.size.y / 2 - this.moveCollider.offset.y;
                this.blockDown = true;
            }
            else if (contact.normal.y < -0.5f)
            {
                if (!wasBlockUp && !this.blockUp) position.y = contact.point.y - this.moveCollider.size.y / 2 - this.moveCollider.offset.y;
                this.blockUp = true;
            }
            else if (contact.normal.x < -0.5f)
            {
                if (!wasBlockRight && !this.blockRight) position.x = contact.point.x - this.moveCollider.size.x / 2 - this.moveCollider.offset.x;
                this.blockRight = true;
            }
            else if (contact.normal.x > 0.5f)
            {
                if (!wasBlockLeft && !this.blockLeft) position.x = contact.point.x + this.moveCollider.size.x / 2 - this.moveCollider.offset.x;
                this.blockLeft = true;
            }
        }

        this.transform.position = position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        this.OnCollisionEnter2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.blockDown = false;
        this.blockLeft = false;
        this.blockRight = false;
        this.blockUp = false;
    }
}
