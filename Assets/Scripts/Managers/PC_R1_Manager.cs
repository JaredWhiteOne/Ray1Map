﻿using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace R1Engine
{
    /// <summary>
    /// The game manager for Rayman 1 (PC)
    /// </summary>
    public class PC_R1_Manager : IGameManager
    {
        /// <summary>
        /// The size of one cell
        /// </summary>
        public const int CellSize = 16;

        /// <summary>
        /// Gets the file path for the specified level
        /// </summary>
        /// <param name="basePath">The base game path</param>
        /// <param name="world">The world</param>
        /// <param name="level">The level</param>
        /// <returns>The level file path</returns>
        public string GetLevelFilePath(string basePath, World world, int level)
        {
            return Path.Combine(GetWorldFolderPath(basePath, world), $"RAY{level}.LEV");
        }

        /// <summary>
        /// Gets the name for the world
        /// </summary>
        /// <returns>The world name</returns>
        public string GetWorldName(World world)
        {
            switch (world)
            {
                case World.Jungle:
                    return "JUNGLE";
                case World.Music:
                    return "MUSIC";
                case World.Mountain:
                    return "MOUNTAIN";
                case World.Image:
                    return "IMAGE";
                case World.Cave:
                    return "CAVE";
                case World.Cake:
                    return "CAKE";
                default:
                    throw new ArgumentOutOfRangeException(nameof(world), world, null);
            }
        }

        /// <summary>
        /// Gets the folder path for the specified world
        /// </summary>
        /// <param name="basePath">The base game path</param>
        /// <param name="world">The world</param>
        /// <returns>The world folder path</returns>
        public string GetWorldFolderPath(string basePath, World world)
        {
            return Path.Combine(basePath, "PCMAP", GetWorldName(world));
        }

        /// <summary>
        /// Gets the level count for the specified world
        /// </summary>
        /// <param name="basePath">The base game path</param>
        /// <param name="world">The world</param>
        /// <returns>The level count</returns>
        public int GetLevelCount(string basePath, World world)
        {
            var worldPath = GetWorldFolderPath(basePath, world);

            return Directory.EnumerateFiles(worldPath, "RAY??.LEV", SearchOption.TopDirectoryOnly).Count();
        }

        /// <summary>
        /// Loads the specified level
        /// </summary>
        /// <param name="basePath">The base game path</param>
        /// <param name="world">The world</param>
        /// <param name="level">The level</param>
        /// <returns>The level</returns>
        public Common_Lev LoadLevel(string basePath, World world, int level)
        {
            // Read the level data
            var levelData = FileFactory.Read<PC_R1_LevFile>(GetLevelFilePath(basePath, world, level));

            // Convert levelData to common level format
            Common_Lev commonLev = new Common_Lev
            {
                // Set the dimensions
                Width = levelData.MapWidth,
                Height = levelData.MapHeight,

                // TODO: Clean up by making a common event class
                // Set the events
                Events = levelData.Events.Select(x => new Event()
                {
                    pos = new PxlVec((ushort) x.XPosition, (ushort) x.YPosition),
                    type = (EventType)x.Type
                }).ToArray(),

                // Create the tile arrays
                TileSet = new Common_Tileset[4],
                Tiles = new Common_Tile[levelData.MapWidth * levelData.MapHeight]
            };

            // Read the 3 tile sets (one for each palette)
            var tileSets = ReadTileSets(levelData);

            // Set the tile sets
            commonLev.TileSet[1] = tileSets[0];
            commonLev.TileSet[2] = tileSets[1];
            commonLev.TileSet[3] = tileSets[2];

            // Get the palette changers
            var paletteXChangers = levelData.Events.Where(x => x.Type == 158 && x.SubEtat < 6).ToDictionary(x => x.XPosition, x => (PC_R1_PaletteChangerMode)x.SubEtat);                  
            var paletteYChangers = levelData.Events.Where(x => x.Type == 158 && x.SubEtat >= 6).ToDictionary(x => x.YPosition, x => (PC_R1_PaletteChangerMode)x.SubEtat);

            // Make sure we don't have both horizontal and vertical palette changers as they would conflict
            if (paletteXChangers.Any() && paletteYChangers.Any())
                throw new Exception("Horizontal and vertical palette changers can't both appear in the same level");

            // Check which type of palette changer we have
            bool isPaletteHorizontal = paletteXChangers.Any();

            // Keep track of the current palette
            int currentPalette = 1;

            // Enumerate each cell
            for (int cellY = 0; cellY < levelData.MapHeight; cellY++)
            {
                // Reset the palette on each row if we have a horizontal changer
                if (isPaletteHorizontal)
                    currentPalette = 1;
                // Otherwise check the y position
                else
                {
                    // Check every pixel 16 steps forward
                    for (int y = 0; y < CellSize; y++)
                    {
                        // Attempt to find a matching palette changer on this pixel
                        var py = paletteYChangers.TryGetValue((uint)(CellSize * cellY + y), out PC_R1_PaletteChangerMode pm) ? (PC_R1_PaletteChangerMode?)pm : null;

                        // If one was found, change the palette based on type
                        if (py != null)
                        {
                            switch (py)
                            {
                                case PC_R1_PaletteChangerMode.Top2tobottom1:
                                case PC_R1_PaletteChangerMode.Top3tobottom1:
                                    currentPalette = 1;
                                    break;
                                case PC_R1_PaletteChangerMode.Top1tobottom2:
                                case PC_R1_PaletteChangerMode.Top3tobottom2:
                                    currentPalette = 2;
                                    break;
                                case PC_R1_PaletteChangerMode.Top1tobottom3:
                                case PC_R1_PaletteChangerMode.Top2tobottom3:
                                    currentPalette = 3;
                                    break;
                            }
                        }
                    }
                }

                for (int cellX = 0; cellX < levelData.MapWidth; cellX++)
                {
                    // Get the cell
                    var cell = levelData.Tiles[cellY * levelData.MapWidth + cellX];

                    // Check the x position for palette changing
                    if (isPaletteHorizontal)
                    {
                        // Check every pixel 16 steps forward
                        for (int x = 0; x < CellSize; x++)
                        {
                            // Attempt to find a matching palette changer on this pixel
                            var px = paletteXChangers.TryGetValue((uint)(CellSize * cellX + x), out PC_R1_PaletteChangerMode pm) ? (PC_R1_PaletteChangerMode?)pm : null;

                            // If one was found, change the palette based on type
                            if (px != null)
                            {
                                switch (px)
                                {
                                    case PC_R1_PaletteChangerMode.Left3toRight1:
                                    case PC_R1_PaletteChangerMode.Left2toRight1:
                                        currentPalette = 1;
                                        break;
                                    case PC_R1_PaletteChangerMode.Left1toRight2:
                                    case PC_R1_PaletteChangerMode.Left3toRight2:
                                        currentPalette = 2;
                                        break;
                                    case PC_R1_PaletteChangerMode.Left1toRight3:
                                    case PC_R1_PaletteChangerMode.Left2toRight3:
                                        currentPalette = 3;
                                        break;
                                }
                            }
                        }
                    }

                    // Get the texture index, default to -1 for fully transparent (no texture)
                    var textureIndex = -1;

                    // Ignore if fully transparent
                    if (cell.TransparencyMode != PC_R1_MapTileTransparencyMode.FullyTransparent)
                    {
                        // Get the offset for the texture
                        var texOffset = levelData.TexturesOffsetTable[cell.TextureIndex];

                        // Get the texture
                        var texture = cell.TransparencyMode == PC_R1_MapTileTransparencyMode.NoTransparency ? levelData.NonTransparentTextures.FindItem(x => x.Offset == texOffset) : levelData.TransparentTextures.FindItem(x => x.Offset == texOffset);

                        // Get the index
                        textureIndex = levelData.NonTransparentTextures.Concat(levelData.TransparentTextures).FindItemIndex(x => x == texture);
                    }

                    // Set the common tile
                    commonLev.Tiles[cellY * levelData.MapWidth + cellX] = new Common_Tile()
                    {
                        TileSetGraphicIndex = textureIndex,
                        CollisionType = cell.CollisionType,
                        PaletteIndex = currentPalette,
                        XPosition = cellX,
                        YPosition = cellY
                    };
                }
            }

            // Return the common level data
            return commonLev;
        }

        /// <summary>
        /// Reads 3 tile-sets, one for each palette
        /// </summary>
        /// <param name="levData">The level data to get the tile-set for</param>
        /// <returns>The 3 tile-sets</returns>
        public Common_Tileset[] ReadTileSets(PC_R1_LevFile levData) 
        {
            // Create the output array
            var output = new Common_Tileset[]
            {
                new Common_Tileset(new Tile[levData.TexturesCount]),
                new Common_Tileset(new Tile[levData.TexturesCount]),
                new Common_Tileset(new Tile[levData.TexturesCount]),
            };

            // Keep track of the tile index
            int index = 0;

            // Enumerate every texture
            foreach (var texture in levData.NonTransparentTextures.Concat(levData.TransparentTextures))
            {
                // Enumerate every palette
                for (int i = 0; i < 3; i++)
                {
                    // Create the texture to use for the tile
                    var tileTexture = new Texture2D(CellSize, CellSize, TextureFormat.RGBA32, false)
                    {
                        filterMode = FilterMode.Point
                    };

                    // Write each pixel to the texture
                    for (int y = 0; y < CellSize; y++)
                    {
                        for (int x = 0; x < CellSize; x++)
                        {
                            // Get the index
                            var cellIndex = CellSize * y + x;

                            // Get the color from the current palette
                            var c = levData.ColorPalettes[i][texture.ColorIndexes[cellIndex]].GetColor();

                            // If the texture is transparent, add the alpha channel
                            if (texture is PC_R1_TransparentTileTexture tt)
                                c.a = (float)tt.Alpha[cellIndex] / Byte.MaxValue;

                            // Set the pixel
                            tileTexture.SetPixel(x, y, c);
                        }
                    }

                    // Apply the pixels to the texture
                    tileTexture.Apply();

                    // Create and set up the tile
                    output[i].SetTile(tileTexture, CellSize, index);
                }

                index++;
            }

            return output;
        }
    }
}