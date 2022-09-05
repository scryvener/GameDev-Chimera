using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPosition : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyChimera EnemyChimera = GetComponentInParent<EnemyChimera>();
        Rigidbody2D parentRigid = EnemyChimera.GetComponent<Rigidbody2D>();

        Vector2 Distance = parentRigid.position - rigidbody2D.position;

        Vector2 formation=new Vector2(0,0);
        Vector2 newPosition = new Vector2(0, 0);

        formation=FindPosition(parentRigid, rigidbody2D);
        Debug.Log(formation);
        if (Distance.magnitude != 2)
        {
            newPosition = parentRigid.position + formation;
            rigidbody2D.MovePosition(newPosition);
        }
    }

    Vector2 FindPosition(Rigidbody2D parentRigid, Rigidbody2D rigidbody2D)
    {
        Vector2 Distance = parentRigid.position - rigidbody2D.position;

        Vector2 formation = new Vector2(0, 0);
        Vector2 newPosition = new Vector2(0, 0);

        if (parentRigid.position.x - rigidbody2D.position.x > 0 && Mathf.Abs(parentRigid.position.y - rigidbody2D.position.y)<.25)
        {
            Debug.Log(1);
            formation = Vector2.left * 2;
            return formation;
        }

        else if (parentRigid.position.x - rigidbody2D.position.x < 0 && Mathf.Abs(parentRigid.position.y - rigidbody2D.position.y) < .25)
        {
            Debug.Log(2);
            formation = Vector2.right * 2;
            return formation;
        }

        else if (parentRigid.position.y - rigidbody2D.position.y < 0 && Mathf.Abs(parentRigid.position.x - rigidbody2D.position.x) < .25)
        {
            Debug.Log(3);
            formation = Vector2.up * 2;
            return formation;
        }

        else if (parentRigid.position.y - rigidbody2D.position.y > 0 && Mathf.Abs(parentRigid.position.x - rigidbody2D.position.x) < .25)
        {
            Debug.Log(4);
            formation = Vector2.down * 2;
            return formation;
        }
        else
        {
            return formation;
        }
    }
}
