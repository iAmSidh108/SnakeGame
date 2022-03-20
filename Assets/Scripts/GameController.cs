using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject spikePrefab = null;

    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;
    public bool alive = true;

    public bool waitingToPlay = true;

    List<Egg> eggs = new List< Egg >();
    List<Spike> spikes = new List<Spike>();

    public int  level = 0;
    int noOfEggsForNExtLevel = 0;
    int noOfSpikesForNextLevel = 0;

    public int score=0;
    public int highScore = 0;

    public Text LevelText = null;

    public Text scoreText=null;
    public Text highScoreText = null;

    public Text tapToPlayText = null;
    public Text gameOverText = null;

    


    void Start()
    {
        instance = this;
        Debug.Log("Starting the Snake Game");
        CreateWalls();
       
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

        scoreText.text = "Score :- " + score;
        highScoreText.text = "High Score :- " + highScore;
        LevelText.text = "Level: " + level;
        //speedText.text = "Speed: " + snakeSpeed;

    }

    

    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;
        tapToPlayText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);

    }

    void StartGameplay()
    {
        tapToPlayText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        score = 0;
        waitingToPlay = false;
        alive = true;
        KillOldEggs();
        DestroyAllSpikes();

        LevelUp();
    }

    void LevelUp()
    {

        
        level++;

        noOfEggsForNExtLevel = 4 + (level * 2);
        noOfSpikesForNextLevel = level;

        

        snakeSpeed = 2f * (level / 4f);
        if (snakeSpeed > 6) snakeSpeed = 6;

        snakeHead.ResetSnake();
        CreateEgg();
        DestroyAllSpikes();
        CreateSpike();
    }
    

    public void EggEaten(Egg egg)
    {

        score++;
        noOfEggsForNExtLevel--;
        if (noOfEggsForNExtLevel == 0)
        {
            score += 10;
            LevelUp();
        }
        else if (noOfEggsForNExtLevel == 1)
        {
            CreateEgg(true);
        }
        else
            CreateEgg(false);

        if (score > highScore)
            highScore = score;

        eggs.Remove(egg);
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
    void CreateSpike()
    {
        Spike spike = null;

        for(int i = 1; i <= level; i++)
        {
            Vector3 position;
            position.x = -width + Random.Range(1f, (width * 2) - 2f);
            position.y = -height + Random.Range(1f, (height * 2) - 2f);
            position.z = 0;


            spike = Instantiate(spikePrefab, position, Quaternion.identity).GetComponent<Spike>();
            spikes.Add(spike);
        }
        
    }

    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
    void DestroyAllSpikes()
    {
        foreach(Spike spike in spikes)
        {
            Destroy(spike.gameObject);
        }
        spikes.Clear();
    }
}
