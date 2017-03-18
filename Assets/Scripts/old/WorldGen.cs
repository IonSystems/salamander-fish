using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{


    public GameObject platformPrefab;
    public ArrayList platforms;
    // Use this for initialization
    void Start()
    {
        platforms = new ArrayList();
        for (int y = -10; y < 10; y += 5)
        {
            for (int x = -10; x < 10; x += 5)
            {
                GameObject platform = Instantiate(platformPrefab);
                platform.AddComponent<BoxCollider2D>();
                platform.transform.position = new Vector3(x, y, 0);

                platforms.Add(platform);
            }
        }

        //Add Walls
        Vector3 raxis = new Vector3(0, 0, 1);
        int xp;
        int world_x = 50;
        int wall_len = 8;
        int world_width = 100;
        for (xp = 0; xp < world_x; xp += wall_len)
        {
            GameObject left = Instantiate(platformPrefab);
            left.AddComponent<BoxCollider2D>();
            left.transform.position = new Vector3(-world_width / 2, xp - 5, 0);
            left.transform.rotation = Quaternion.AngleAxis(90, raxis);
            platforms.Add(left);

            GameObject right = Instantiate(platformPrefab);
            right.AddComponent<BoxCollider2D>();
            right.transform.position = new Vector3(world_width / 2, xp - 5, 0);
            right.transform.rotation = Quaternion.AngleAxis(90, raxis);
            platforms.Add(left);
        }

        int yp;
        int world_y = 100;
        int world_height = 50;
        for (yp = 0; yp < world_y; yp += wall_len)
        {
            GameObject top = Instantiate(platformPrefab);
            top.AddComponent<BoxCollider2D>();
            top.transform.position = new Vector3(yp - 50, -(world_height / 2) + 20, 0);
            platforms.Add(top);

            GameObject bottom = Instantiate(platformPrefab);
            bottom.AddComponent<BoxCollider2D>();
            bottom.transform.position = new Vector3(yp - 50, (world_height / 2) + 20, 0);
            platforms.Add(bottom);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
