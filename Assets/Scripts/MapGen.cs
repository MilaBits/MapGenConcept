using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGen : MonoBehaviour {

    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> gridTiles = new List<GameObject>();

    public List<GameObject> Tiles = new List<GameObject>();
    public int positionSpacing = 10;
    public int rows = 10;
    public int columns = 10;

    // Use this for initialization
    void Start() {
        GenerateGrid();
        PopulateGrid();
    }

    // Update is called once per frame
    void Update() {

    }

    private void GenerateGrid() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                gridPositions.Add(new Vector3(i * positionSpacing, 0, j * positionSpacing));
            }
        }
    }

    private void PopulateGrid() {
        foreach (var gridPosition in gridPositions) {
            GameObject tile = Instantiate(Tiles[Random.Range(0, Tiles.Count)]);
            tile.transform.position = gridPosition;
            tile.transform.rotation = Quaternion.Euler(tile.transform.rotation.eulerAngles.x, tile.transform.rotation.eulerAngles.y, tile.transform.rotation.eulerAngles.z + Random.Range(0, 4) * 90);

            if (gridTiles.Count < 1) {
                gridTiles.Add(tile);
                continue;
            }
            foreach (var collider in tile.GetComponentsInChildren<Collider>()) {

                List<Bounds> boundsToCheck = new List<Bounds>();
                foreach (var existingCollider in gridTiles.Last().GetComponentsInChildren<Collider>()) {
                    boundsToCheck.Add(existingCollider.bounds);
                }
                if (gridTiles.Count > 10) {
                    foreach (var existingCollider in gridTiles[gridTiles.Count - 11].GetComponentsInChildren<Collider>()) {
                        boundsToCheck.Add(existingCollider.bounds);
                    }
                }

                foreach (var existingBounds in boundsToCheck) {
                    if (!collider.bounds.Intersects(existingBounds)) {
                        tile.transform.rotation = Quaternion.Euler(tile.transform.rotation.eulerAngles.x, tile.transform.rotation.eulerAngles.y, tile.transform.rotation.eulerAngles.z + 90);
                    }
                }
            }

            gridTiles.Add(tile);
        }
    }
}
