using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateWater : MonoBehaviour {
    public Tilemap tilemap;
    public TileBase[] tiles;
    public int tilemapWidth;
    public int tilemapHeight;
    public Transform player;
    public float tileRenderCutoffDistance;

    private float[] rotations = new float[] { 0f, 90f, 180f, 270f };

    private void Awake() {
        StartCoroutine(GenerateTiles(() => {
            StartCoroutine(UpdateTilesBasedOnPlayerProximity());
        }));
    }

    private void SpawnTile(int x, int y) {
        Vector3Int pos = TilePosition(x, y);
        float randomRotation = rotations[UnityEngine.Random.Range(0, 4)];
        TileBase randomTile = tiles[UnityEngine.Random.Range(0, tiles.Length)];

        tilemap.SetTile(pos, randomTile);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, randomRotation), Vector3.one);
        tilemap.SetTransformMatrix(pos, matrix);
    }

    private Vector3Int TilePosition(int x, int y) {
        return new Vector3Int(x, y, 0);
    }

    private IEnumerator GenerateTiles(Action callback) {
        Vector2 playerPosition = player.position;

        for (int x = 0; x < tilemapWidth; x++) {
            for (int y = 0; y < tilemapHeight; y++) {
                Vector3 tileWorldPos = tilemap.CellToWorld(TilePosition(x, y));
                float distance = Vector3.Distance(tileWorldPos, playerPosition);
                if (distance <= tileRenderCutoffDistance) {
                    SpawnTile(x, y);
                }
            }
        }

        callback();
        yield return null;
    }

    private IEnumerator UpdateTilesBasedOnPlayerProximity() {
        while (true) {
            Vector2 playerPosition = player.position;
            for (int x = 0; x < tilemapWidth; x++) {
                for (int y = 0; y < tilemapHeight; y++) {
                    Vector3Int tilePosition = TilePosition(x, y);
                    TileBase currentTile = tilemap.GetTile(tilePosition);

                    Vector3 tileWorldPos = tilemap.CellToWorld(tilePosition);
                    float distance = Vector3.Distance(tileWorldPos, playerPosition);

                    // my thinking is that in most cases, a tile will be too far away and already null, so
                    // make a shortcut for this case that only requires one comparison to null
                    if (distance > tileRenderCutoffDistance && currentTile == null) continue;

                    if (distance > tileRenderCutoffDistance && currentTile != null) {
                        tilemap.SetTile(tilePosition, null);
                    } else if (distance <= tileRenderCutoffDistance && currentTile == null) {
                        SpawnTile(x, y);
                    }
                }
            }

            yield return new WaitForSeconds(1);
        }
    }
}
