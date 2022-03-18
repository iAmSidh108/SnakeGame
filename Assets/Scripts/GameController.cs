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
    void Start()
    {
        instance = this;
        Debug.Log("Starting the Snake Game");
        CreateWalls();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        snakeHead.ResetSnake();
        CreateEgg();
    }

    public void GameOver()
    {
        alive = false;
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

        if(golden)
             Instantiate(goldenEggPrefab, position, Quaternion.identity);

        else
            Instantiate(eggPrefab, position, Quaternion.identity);
    }
}
