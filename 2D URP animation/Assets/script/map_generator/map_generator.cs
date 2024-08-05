using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_generator : MonoBehaviour
{
    public int map_width;
    public int map_height;
    public GameObject[] terrainPrefabs;
    public int num_prefabs;
    private float square_scale = 0.16f;
    private float air_island_gap = 0.5f;

    // Start is called before the first frame update
    void Start() {
        // map = new int[map_width, map_height];
        // max_height = new int[map_width];
        // GenerateMap();
        RenderMap();
    }

    // Update is called once per frame
    void Update() {
    }
    void GenerateMap() {
        GenerateFloor();
    }
    void GenerateFloor() {
        // // Generate a map with random values
        // for (int x = 0; x < map_width; x++) {
        //     float noise_sum = 0;
        //     for (int i = 0; i < num_noise_layers; i++) {
        //         noise_sum += amplitudes[i] * Mathf.PerlinNoise(x * frequencies[i] * noise_scale, 0);
        //     }
        //     int height = (int)(noise_sum * map_height);
        //     max_height[x] = height;
        // }
    }
    void RenderMap() {
        float sum_width = 0;
        float last_height = 0;
        for(int i = 0; i < num_prefabs; i++) {
            int terrain_index = Random.Range(0, terrainPrefabs.Length);
            while(terrain_index <= 1) {
                terrain_index = Random.Range(0, terrainPrefabs.Length);
            }

            // terrain_index = 4;

            // Debug.Log("terrain_index: " + terrain_index, "i: " + i, "sum_width: " + sum_width);
            // Debug.Log("terrain_index: " + terrain_index);
            // Debug.Log("sum_width: " + sum_width);

            Debug.Log("i " + i);
            Debug.Log("sum_width " + sum_width);
            GameObject prefab = terrainPrefabs[terrain_index];
            Vector3 prefabSize = GetPrefabSize(prefab);
            Debug.Log("prefabSize.x: " + prefabSize.x);
            if(terrain_index <= 1) { // air island
                sum_width += air_island_gap;
            }
            if(terrain_index > 1)  {
                Instantiate(prefab, new Vector3(sum_width + prefabSize.x / 2.0f, 0, 0), Quaternion.identity);
                Debug.Log("render center: " + (sum_width + prefabSize.x / 2.0f));
            }
            else {
                Instantiate(prefab, new Vector3(sum_width + prefabSize.x / 2.0f, 2, 0), Quaternion.identity);
            }
            sum_width += prefabSize.x;
            if(terrain_index <= 1) { // air island
                sum_width += air_island_gap;
            }
        }

    }
    Vector3 GetPrefabSize(GameObject prefab) {
        EdgeCollider2D edgeCollider = prefab.GetComponent<EdgeCollider2D>();
        // Transform transform = prefab.GetComponent<Transform>();
        // Vector3 scale = transform.localScale;
        if (edgeCollider != null) {
            Vector2 min = edgeCollider.points[0];
            Vector2 max = edgeCollider.points[0];
            foreach (Vector2 point in edgeCollider.points) {
                if (point.x < min.x) min.x = point.x;
                if (point.y < min.y) min.y = point.y;
                if (point.x > max.x) max.x = point.x;
                if (point.y > max.y) max.y = point.y;
            }
            Vector2 size = max - min;
            return new Vector3(size.x, size.y, 1);
        }
        return Vector3.one;
    }

}
