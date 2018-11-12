using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Controls
{
    public class CharacterMovement : MoveableObject
    {
        protected override float GetMovement()
        {
            if(Input.GetKeyDown("d") || Input.GetKey("d"))
            {
                return _speedStat.GetCurrentValue();
            }
            else if(Input.GetKeyDown("a") || Input.GetKey("a"))
            {
                return _speedStat.GetCurrentValue() * -1;
            }

            return 0;
        }

        protected override void Update()
        {
            if(_body.position.y < -100)
            {
                EventManager.Instance.ExecuteEvent(EventType.PLAYER_FELL_OFF_MAP);

                return;
                //_body.position = new Vector2(-352.8f, -55.0f);
            }

            base.Update();
        }

        /*
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.otherCollider is CircleCollider2D && collision.collider.isTrigger == false)
            {
                var circleCollider = collision.otherCollider as CircleCollider2D;

                ContactPoint2D[] contacts = new ContactPoint2D[1];
                int numberOfCollisions = collision.GetContacts(contacts);

                if (numberOfCollisions > 0)
                {
                    var collisionPoint = contacts[0].point.y;

                    var colliderCenter = circleCollider.bounds.center.y;
                    var colliderRadius = circleCollider.radius;

                    var collisionHorisontial =
                        collisionPoint < Mathf.Sin(Mathf.PI / 4) * colliderRadius + colliderCenter &&
                        collisionPoint > Mathf.Sin((7 * Mathf.PI) / 4) * colliderRadius + colliderCenter;

                    if (collisionHorisontial)
                    {
                        _canContinue = false;
                    }
                    else
                    {
                        _canContinue = true;
                    }
                }
                else
                {
                    Debug.Log("There was no collision point for some reason. . .");
                }
            }
        }
        */
    }
}
