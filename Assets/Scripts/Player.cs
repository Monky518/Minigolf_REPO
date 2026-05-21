using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int rotateSpeed;
    
    void Start()
    {
        
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime);
        }

        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    ChangePosition(Vector2.up);
        //}

        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    ChangePosition(Vector2.down);
        //}
    }

//    void ChangePosition(Vector2 d)
//    {
//        //checks if you did not click the same direction again
//        if (d != direction)
//        {
//            nextDirection = d;
//        }

//        if (currentNode != null)
//        {
//            Node moveToNode = CanMove(d);

//            if (moveToNode != null)
//            {
//                //actively moving
//                direction = d;
//                targetNode = moveToNode;
//                previousNode = currentNode;
//                currentNode = null;
//            }
//        }
//    }

//    void Move()
//    {
//        if (targetNode != currentNode && targetNode != null)
//        {
//            //turning around between nodes
//            if (nextDirection == direction * -1)
//            {
//                direction *= -1;

//                Node tempNode = targetNode;
//                targetNode = previousNode;
//                previousNode = tempNode;
//            }

//            if (OverShotTarget())
//            {
//                //sets the game's boundaries
//                currentNode = targetNode;
//                transform.localPosition = currentNode.transform.position;

//                GameObject otherPortal = GetPortal(currentNode.transform.position);
//                if (otherPortal != null)
//                {
//                    transform.localPosition = otherPortal.transform.position;
//                    currentNode = otherPortal.GetComponent<Node>();
//                }

//                Node moveToNode = CanMove(nextDirection);
//                if (moveToNode != null)
//                {
//                    direction = nextDirection;
//                }
//                if (moveToNode == null)
//                {
//                    moveToNode = CanMove(direction);
//                }
//                if (moveToNode != null)
//                {
//                    targetNode = moveToNode;
//                    previousNode = currentNode;
//                    currentNode = null;
//                }
//                else
//                {
//                    //hits wall
//                    direction = Vector2.zero;
//                }
//            }
//            else
//            {
//                transform.position += (Vector3)(direction * speed) * Time.deltaTime;
//            }
//        }
//    }
}
