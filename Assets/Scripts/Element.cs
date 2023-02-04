using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum eElements
{
    LIGHT,
    WATER,
    NUTRIENT,
    AIR,
    ARMOR,
    COUNT
}
public class Element : MonoBehaviour
{
    [SerializeField] eElements element;
    [SerializeField] float Movementspeed;
    [SerializeField] float SpeedY;
    [SerializeField] float timeToMove;

    float timer;

    Vector3 direction;
    Vector3 desiredDirection;

    void Start()
    {
        timer = 0f;
        direction = new Vector3(0, SpeedY, 0);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToMove)
        {
            var xPos = Random.Range(-20.5f, 20.5f);
            desiredDirection = new Vector3(xPos,
                SpeedY, 
                0);
            desiredDirection = desiredDirection.normalized * Movementspeed;
            timer = 0; 
        }
        
        direction = Vector3.RotateTowards(direction, desiredDirection, Mathf.Deg2Rad * 0.75f, 1);
        transform.position += direction * Time.deltaTime;
    }
    private void OnDrawGizmos()
    {

        //Direction vector
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + direction);

        //desired direction vector
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + desiredDirection);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ParticleAllowed"))
        {
            Destroy(gameObject);
        }
    }
}
