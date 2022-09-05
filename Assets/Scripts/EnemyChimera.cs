using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChimera : MonoBehaviour
{
    // Start is called before the first frame update

    
    public int currentHealth = 40;
    public int health { get { return currentHealth; } }
    Rigidbody2D rigidbody2D;
    int maxParts = 4;
    public float speed = 5f;

    public int numChunk=0;
    public int numSpeed = 0;
    public int numSplitter = 0;

    void Awake()
    {
        CreateConnectors();
        CreateBody();
        rigidbody2D = GetComponent<Rigidbody2D>();

        currentHealth = currentHealth + numChunk * 10;
        speed = speed + numSpeed;

        //define special behavior based on the body parts chosen, armed and splitter will require seperate functions
    }

    // Update is called once per frame
    void Update()
    {
        //basic movement behavior
        Transform[] transformlist= GetComponentsInChildren<Transform>();
        Rigidbody2D[] bodyList= GetComponentsInChildren<Rigidbody2D>();
        GameObject player = GameObject.Find("Player");
        Rigidbody2D playerbody = player.GetComponent<Rigidbody2D>();
        Vector3 target = playerbody.position;
        Vector3 currentPosition = rigidbody2D.position;

        float x_transformed = target.x - rigidbody2D.position.x;
        float y_transformed = target.y - rigidbody2D.position.y;

        Vector3 target_direction = new Vector3(x_transformed, y_transformed, 0);

        currentPosition = currentPosition + target_direction.normalized * speed * Time.deltaTime;
        rigidbody2D.MovePosition(currentPosition);

        foreach (Rigidbody2D r in bodyList)
        {
            Vector3 partPosition = r.position;
            partPosition = partPosition + target_direction.normalized * speed * Time.deltaTime;

            r.MovePosition(partPosition);
        }




        //rotate core
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, target_direction);
        rigidbody2D.MoveRotation(targetRotation);

        if (currentHealth <= 0)
        {
            //pull player controller to access gold change functions

            PlayerController playerController = player.GetComponent<PlayerController>();
            //add function to check for unique behavior, and if those controllers exist, execute death functions

            if (numSplitter != 0)
            {
                spawnOnDeath(numSplitter*4, rigidbody2D);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            playerController.changeGold(100);
            

        }
    }


    void spawnOnDeath(int num, Rigidbody2D rigidbody2D)
    {
        for (int i=0; i < num; i++)
        {
            Object[] enemyList = Resources.LoadAll("Prefabs/Enemies", typeof(GameObject));
            int randomSelected = Random.Range(0, enemyList.Length);

            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);

            if (randomSelected == 3)
            {
                randomSelected = 0;
            }
            GameObject selected = enemyList[randomSelected] as GameObject;
            Vector2 position = new Vector2(x, y);

            GameObject spawned = Instantiate(selected,rigidbody2D.position+position,Quaternion.identity);
        }
    }

    void spawnConnectors(GameObject connector,GameObject chimera, Vector3 direction, Quaternion rotation)
    {
        GameObject connectorPart = Instantiate(connector,chimera.GetComponent<Transform>().position+direction*1.25f , rotation);
        connectorPart.transform.SetParent(chimera.transform);

        Physics2D.IgnoreCollision(connectorPart.GetComponent<BoxCollider2D>(), chimera.GetComponent<CircleCollider2D>());
        connectorPart.AddComponent<DistanceJoint2D>();

        DistanceJoint2D joint = connectorPart.GetComponent<DistanceJoint2D>();

        joint.connectedBody = chimera.GetComponent<Rigidbody2D>();
        joint.distance = 1.25f;

    }

    void spawnBodyParts(GameObject part, GameObject chimera, Vector3 direction, Quaternion rotation)
    {
        GameObject bodyPart = Instantiate(part,chimera.GetComponent<Transform>().position+direction*2f,rotation);

        bodyPart.transform.SetParent(chimera.transform);

        GameObject[] connectors= GameObject.FindGameObjectsWithTag("Connector");

        foreach(GameObject c in connectors)
        {
            Physics2D.IgnoreCollision(c.GetComponent<BoxCollider2D>(), bodyPart.GetComponent<CircleCollider2D>());
        }

        //Physics2D.IgnoreCollision(bodyPart.GetComponent<CircleCollider2D>(), chimera.GetComponent<CircleCollider2D>());

        bodyPart.AddComponent<DistanceJoint2D>();
        DistanceJoint2D joint = bodyPart.GetComponent<DistanceJoint2D>();

        joint.connectedBody = chimera.GetComponent<Rigidbody2D>();



    }


    void CreateConnectors()//Instantiates Connectors at predefined positions. 
    {
        Object[] connectorList = Resources.LoadAll("Prefabs/ChimeraConnectors", typeof(GameObject));//pull all possible connector variants

        int connectorSelected;

        if (connectorList.Length == 1) { connectorSelected = 0; }//select the only one that is available, otherwise randomly choose between them
        else { connectorSelected = Random.Range(0, connectorList.Length); }

        GameObject connector = connectorList[connectorSelected] as GameObject;

        spawnConnectors(connector, gameObject, Vector3.up,Quaternion.Euler(0,0,90));
        spawnConnectors(connector, gameObject, Vector3.down, Quaternion.Euler(0, 0, 90));
        spawnConnectors(connector, gameObject, Vector3.left, Quaternion.Euler(0, 0, 0));
        spawnConnectors(connector, gameObject, Vector3.right, Quaternion.Euler(0, 0, 0));

        Transform[] activeParts = GetComponentsInChildren<Transform>();
        List<GameObject> connectors = new List<GameObject>();
        foreach (Transform g in activeParts)
        {

            if (g.tag == "Connector")
            {
                connectors.Add(g.gameObject);
                //Debug.Log(g);
            }
        }

        AttachEmptyJoints(connectors[0], connectors[3].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(connectors[1], connectors[2].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(connectors[2], connectors[0].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(connectors[3], connectors[1].GetComponent<Rigidbody2D>());

    }


    void CreateBody()//instantiates bodies from the parts folder randomly depending on how many connectors there are.
    {
        Object[] partList = Resources.LoadAll("Prefabs/ChimeraParts", typeof(GameObject));//pull all possible connector variants

        //foreach (Object p in partList)
        //{
        //    Debug.Log(p);
        //}

        int partSelected;

        partSelected = selectPart(partList);
        GameObject part1 = partList[partSelected] as GameObject;
        spawnBodyParts(part1, gameObject, Vector3.up, Quaternion.identity);

        partSelected = selectPart(partList);
        GameObject part2 = partList[partSelected] as GameObject;
        spawnBodyParts(part2, gameObject, Vector3.down, Quaternion.identity);

        partSelected = selectPart(partList);
        GameObject part3 = partList[partSelected] as GameObject;
        spawnBodyParts(part3, gameObject, Vector3.left, Quaternion.identity);

        partSelected = selectPart(partList);
        GameObject part4 = partList[partSelected] as GameObject;
        spawnBodyParts(part4, gameObject, Vector3.right, Quaternion.identity);

        Transform[] activeParts = GetComponentsInChildren<Transform>();
        List<GameObject> chimeraParts = new List<GameObject>();
        foreach (Transform g in activeParts)
        {

            if (g.tag == "Chimera Parts")
            {
                chimeraParts.Add(g.gameObject);
                //Debug.Log(g);
            }
        }

        Transform[] activeConnectors = GetComponentsInChildren<Transform>();
        List<GameObject> connectors = new List<GameObject>();
        foreach (Transform g in activeConnectors)
        {

            if (g.tag == "Connector")
            {
                connectors.Add(g.gameObject);
                //Debug.Log(g);
            }
        }

        AttachEmptyJoints(chimeraParts[0], chimeraParts[3].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(chimeraParts[1], chimeraParts[2].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(chimeraParts[2], chimeraParts[0].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(chimeraParts[3], chimeraParts[1].GetComponent<Rigidbody2D>());

        AttachEmptyJoints(chimeraParts[0], connectors[0].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(chimeraParts[1], connectors[1].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(chimeraParts[2], connectors[2].GetComponent<Rigidbody2D>());
        AttachEmptyJoints(chimeraParts[3], connectors[3].GetComponent<Rigidbody2D>());


    }

    void AttachEmptyJoints(GameObject part, Rigidbody2D attachTo)
    {
        part.AddComponent<FixedJoint2D>();
        FixedJoint2D[] joints = part.GetComponents<FixedJoint2D>();

        foreach(FixedJoint2D j in joints)
        {
            if (j.connectedBody == null)
            {
                j.connectedBody = attachTo;
            }
        }

    }

    int selectPart(Object[] partList)
    {
        int partSelected = Random.Range(0, partList.Length);

        if (partSelected == 1)
        {
            numChunk += 1;
        }

        if (partSelected == 2)
        {
            numSpeed += 1;
        }

        if (partSelected == 3)
        {
            numSplitter += 1;
        }

        return partSelected;

    }

    void takeDamage()
    {
        currentHealth -= 1;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.changeHealth(-1);
        }

        


    }
}

