using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_generator : MonoBehaviour
{
    public int map_width;
    public int map_height;
    
    public GameObject floor_cube;
    public GameObject underground_cube;

    private float noise_scale = 0.05f;
    private float square_scale = 0.16f;
    private float num_noise_layers = 3;
    private float[] frequencies = { 0.2f, 1.0f, 2.0f };
    private float[] amplitudes = { 1.0f, 0.5f, 0.25f };


    private int[,] map;
    private int[] max_height;

    // Start is called before the first frame update
    void Start() {
        map = new int[map_width, map_height];
        max_height = new int[map_width];
        GenerateMap();
        RenderMap();
    }

    // Update is called once per frame
    void Update() {
    }
    void GenerateMap() {
        GenerateFloor();
    }
    void GenerateFloor() {
        // Generate a map with random values
        for (int x = 0; x < map_width; x++) {
            float noise_sum = 0;
            for (int i = 0; i < num_noise_layers; i++) {
                noise_sum += amplitudes[i] * Mathf.PerlinNoise(x * frequencies[i] * noise_scale, 0);
            }
            int height = (int)(noise_sum * map_height);
            max_height[x] = height;
        }
    }
    void RenderMap() {
        for (int x = 0; x < map_width; x++) {
            for (int y = -20; y < map_height; y++) {
                if (y < max_height[x]) {
                    // Instantiate(Square, new Vector3(x, y, 0), Quaternion.identity);
                    Instantiate(underground_cube, new Vector3(x * square_scale, y * square_scale, 0), Quaternion.identity);
                }
                if (y == max_height[x] ) {
                    // Instantiate(Triangle, new Vector3(x, y, 0), Quaternion.identity);
                    Instantiate(floor_cube, new Vector3(x * square_scale, y * square_scale, 0), Quaternion.identity);
                }
            }
        }
    }
}
