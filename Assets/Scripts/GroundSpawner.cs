using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject groundTile;
    [SerializeField] GameObject Player;

    [SerializeField] GameObject normalTile;
    [SerializeField] GameObject emptyTile;
    [SerializeField] GameObject burningTile;
    [SerializeField] GameObject suppliesTile;
    [SerializeField] GameObject stickyTile;
    [SerializeField] GameObject boostTile;
    [SerializeField] GameObject obstacle;

    GameObject instantiatedTile;
    private Vector3 nextSpawnPoint;

    private List<GameObject> activeTiles = new List<GameObject>();

    void SpawnTile(int id)
    {
        nextSpawnPoint.x = groundTile.transform.position.x;

        switch (id)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                instantiatedTile = Instantiate(emptyTile, nextSpawnPoint, Quaternion.identity);
                break;
            case 5:
            case 6:
            case 7:
                instantiatedTile = Instantiate(burningTile, nextSpawnPoint, Quaternion.identity);
                break;
            case 8:
            case 9:
                instantiatedTile = Instantiate(suppliesTile, nextSpawnPoint, Quaternion.identity);
                break;
            case 11:
            case 12:
                instantiatedTile = Instantiate(stickyTile, nextSpawnPoint, Quaternion.identity);
                break;
            case 13:
            case 14:
                instantiatedTile = Instantiate(boostTile, nextSpawnPoint, Quaternion.identity);
                break;
            case 15:
            case 16:
                instantiatedTile = Instantiate(obstacle, nextSpawnPoint, Quaternion.identity);
                break;
            default:
                instantiatedTile = Instantiate(normalTile, nextSpawnPoint, Quaternion.identity);
                break;

        }

        activeTiles.Add(instantiatedTile);

        // Update the next spawn point to the position of the child 'NextSpawnPoint'
        if (instantiatedTile.transform.childCount > 0)
        {
            nextSpawnPoint = instantiatedTile.transform.GetChild(0).position;
        }
    }

    private void Start()
    {
        PauseMenu.isPaused = false;

        for (int i = 0; i < 5; i++)
        {
            SpawnTile(0);
        }
        for(int i = 0; i < 15; i++)
        {
            int random_number = Random.Range(1, 32);
            SpawnTile(random_number);
        }
    }

    private void Update()
    {
        if (!PauseMenu.isPaused)
        {
            int random_number = Random.Range(1, 32); 

            if (activeTiles.Count > 0)
            {
                // Get the last tile
                GameObject lastTile = activeTiles[0];
                float tileEndZ = lastTile.transform.position.z + lastTile.transform.localScale.z;

                // Check if the player's z position is greater than the tile's end position
                if (Player.transform.position.z > tileEndZ + 2)
                {
                    Destroy(activeTiles[0]); // Destroy the first tile that the player has passed
                    activeTiles.RemoveAt(0); // Remove it from the list

                    // Spawn a new tile at the end of the path
                    SpawnTile(random_number);
                }
            }

        }
    }
}
