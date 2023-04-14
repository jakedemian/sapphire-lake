using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapProximityManager : MonoBehaviour {
    public Transform player;
    public Tilemap tilemap;
    public TileBase baseTile;

    private void Update() {
        // foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin) {
        //     Vector3 tileWorldPos = tilemap.CellToWorld(position);
        //     float distance = Vector3.Distance(tileWorldPos, player.position);

        //     // expensive to do a null check every frame?
        //     if (distance > 10f && tilemap.GetTile(position) != null) {
        //         tilemap.SetTile(position, null);
        //     } else if (distance <= 10f && tilemap.GetTile(position) == null){
        //         tilemap.SetTile(position, "");
        //     }
        // }
    }
}
