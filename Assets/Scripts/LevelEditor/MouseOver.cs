﻿using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Input;

namespace R1Engine
{
    public class MouseOver : MonoBehaviour {
        public Text textGraphic, textCollision;

        // Reference to level's tilemap controller
        public LevelTilemapController tilemapController;

        void Update() {
            transform.position = mousePosition;
            Vector3 mousePositionTile = tilemapController.MouseToTileCoords(mousePosition);

            //Physics2D.Raycast(Camera.main.ScreenPointToRay(mousePosition), out var hit, 30);

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);

            var e = hit.collider?.GetComponentInParent<Common_Event>();
            // Mouse over event
            if (e != null) {
                textCollision.text = $"{e.DisplayName(Settings.World)}";
                textGraphic.text = $"Type: {e.EventInfoData?.Type}{Environment.NewLine}" +
                                   $"Pos: {e.XPosition}, {e.YPosition}{Environment.NewLine}" +
                                   $"AnimSpeed: {e.Speed}{Environment.NewLine}" +
                                   $"Offsets: {e.EventInfoData?.OffsetBX} x {e.EventInfoData?.OffsetBY} x {e.EventInfoData?.OffsetHY}";
            }
            // Else Mouse over type
            else {

                Common_Tile t = tilemapController.GetTileAtPos((int)mousePositionTile.x, -(int)mousePositionTile.y);

                if (t != null) {
                    //Debug.Log("Tile here x:" + t.XPosition + " y:" + t.YPosition + " col:" + t.CollisionType);
                    textCollision.text = $"Collision: {t.CollisionType}";
                    textGraphic.text = $"Graphic tile: {t.TileSetGraphicIndex}";
                }
            }
        }
    }
}