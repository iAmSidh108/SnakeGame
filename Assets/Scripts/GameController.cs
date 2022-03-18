using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    const float width = 3.7f;
    const float height = 7f;

    public float snakeSpeed = 1;

    public BodyPart bodyPrefab = null;
    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldenEggPrefab = null;

    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;
    public bool alive = true;

    public bool waitingToPlay = true;

    List<Egg> eggs = new List< Egg >();
    void Start()
    {
        instance = this;
        Debug.Log("Starting the Snake Game");
        CreateWalls();
        CreateEgg();
        
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartGameplay();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                StartGameplay();
            }
        }
    }

    

    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;
    }

    void StartGameplay()
    {
        waitingToPlay = false;
        alive = true;
        KillOldEggs();
        snakeHead.ResetSnake();
    }

    public void EggEaten(Egg egg)
    {
        Destroy(egg.gameObject);
    }
    void CreateWalls()
    {
        Vector3 start = new Vector3(-width, -height, 0);   //left wall
        Vector3 finish = new Vector3(-width, +height, 0);
        CreateWall(start, finish);

         start = new Vector3(+width, -height, 0);         // right wall
         finish = new Vector3(+width, +height, 0);
         CreateWall(start, finish);

         start = new Vector3(-width, +height, 0);         //  Up wall
         finish = new Vector3(+width, +height, 0);
         CreateWall(start, finish);

         start = new Vector3(-width, -height, 0);         //down wall
         finish = new Vector3(+width, -height, 0);
         CreateWall(start, finish);

    }
    void CreateWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish - start) / noOfRocks;

        Vector3 position = start;
        for(int rock = 0; rock <= noOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale = Random.Range(1.6f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale,float rotation)
    {
        GameObject rock = Instantiate(rockPrefab,position,Quaternion.Euler(0,0,rotation));
        rock.transform.localScale = new Vector3(scale, scale, 0);
    }

    void CreateEgg(bool golden = false)
    {
        Vector3 position;
        position.x = -width + Random.Range(1f, (width * 2) - 2f);
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = 0;

        Egg egg = null;
        if(golden)
             egg=Instantiate(goldenEggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        else
           egg= Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        eggs.Add(egg);
    }

    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
}
