﻿using Cysharp.Threading.Tasks;
using R1Engine.Serialize;
using System;
using System.IO;
using System.Linq;
using ImageMagick;
using UnityEngine;

namespace R1Engine
{
    public class SNES_Prototype_Manager : IGameManager
    {
        public GameInfo_Volume[] GetLevels(GameSettings settings) => GameInfo_Volume.SingleVolume(new GameInfo_World[]
        {
            new GameInfo_World(0, new int[]
            {
                0
            })
        });

        public virtual string GetROMFilePath => $"ROM.sfc";

        public GameAction[] GetGameActions(GameSettings settings) => new GameAction[]
        {
            new GameAction("Export Sprites", false, true, (input, output) => ExportSpritesAsync(settings, output)),
            new GameAction("Export Animation Frames", false, true, (input, output) => ExportAnimFramesAsync(settings, output, false)),
            new GameAction("Export Animations as GIF", false, true, (input, output) => ExportAnimFramesAsync(settings, output, true)),
        };

        public async UniTask ExportSpritesAsync(GameSettings settings, string outputDir)
        {
            using (var context = new Context(settings))
            {
                // Load rom
                await LoadFilesAsync(context);
                var rom = FileFactory.Read<SNES_Proto_ROM>(GetROMFilePath, context);

                var sprites = GetSprites(rom);

                // Export every sprite
                for (int i = 0; i < sprites.Length; i++)
                {
                    var spriteIndex = i % rom.ImageDescriptors.Length;
                    var vramConfig = i / rom.ImageDescriptors.Length;
                    var imgDescriptor = rom.ImageDescriptors[spriteIndex];
                    var sprite = sprites[i];
                    if(sprite == null) continue;

                    var xPos = imgDescriptor.TileIndex % 16;
                    var yPos = (imgDescriptor.TileIndex - xPos) / 16;
                    
                    var width = (int)sprite.rect.width;
                    var height = (int)sprite.rect.height;

                    var newTex = TextureHelpers.CreateTexture2D(width, height);

                    var flipX = imgDescriptor.FlipX;
                    var flipY = !imgDescriptor.FlipY;
                    
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            newTex.SetPixel(flipX ? width - x - 1 : x, (!flipY) ? height - y - 1 : y, sprite.texture.GetPixel((int)sprite.rect.x + x, (int)sprite.rect.y + y));
                        }
                    }

                    newTex.Apply();

                    Util.ByteArrayToFile(Path.Combine(outputDir, $"{spriteIndex} - {vramConfig}.png"), newTex.EncodeToPNG());
                }
            }
        }

        public async UniTask ExportAnimFramesAsync(GameSettings settings, string outputDir, bool saveAsGif)
        {
            using (var context = new Context(settings))
            {
                // Load rom
                await LoadFilesAsync(context);
                var rom = FileFactory.Read<SNES_Proto_ROM>(GetROMFilePath, context);

                var sprites = GetSprites(rom);

                var animIndex = 0;

                foreach (var stateGroup in rom.States.GroupBy(x => x.Animation))
                {
                    // Get the animation
                    var anim = stateGroup.Key;
                    var layersPerFrame = anim.LayersPerFrame;
                    var frameCount = anim.FrameCount;
                    string animPointer = String.Format("{0:X4}", (stateGroup.Key.Offset.FileOffset + 4) % 0x8000 + 0x8000);
                    int vramConfig = stateGroup.First().VRAMConfigIndex;
                    var spriteOffset = rom.ImageDescriptors.Length * vramConfig;

                    // Calculate frame size
                    int minX = anim.Layers.Where(x => sprites[x.ImageIndex] != null).Min(x => x.XPosition);
                    int minY = anim.Layers.Where(x => sprites[x.ImageIndex] != null).Min(x => x.YPosition);
                    int frameWidth = (int)anim.Layers.Where(x => sprites[x.ImageIndex] != null).Max(x => sprites[x.ImageIndex].rect.width + x.XPosition);
                    int frameHeight = (int)anim.Layers.Where(x => sprites[x.ImageIndex] != null).Max(x => sprites[x.ImageIndex].rect.height + x.YPosition);

                    // Create frame textures
                    var frames = new Texture2D[frameCount];

                    // Create each animation frame
                    for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                    {
                        var tex = TextureHelpers.CreateTexture2D(frameWidth - minX, frameHeight - minY, clear: true);

                        // Write each layer
                        for (var layerIndex = 0; layerIndex < layersPerFrame; layerIndex++)
                        {
                            var animationLayer = anim.Layers[frameIndex * layersPerFrame + layerIndex];

                            if ((spriteOffset + animationLayer.ImageIndex) >= sprites.Length)
                                continue;

                            // Get the sprite
                            var sprite = sprites[spriteOffset + animationLayer.ImageIndex];

                            if (sprite == null)
                                continue;

                            // Set every pixel
                            for (int y = 0; y < sprite.rect.height; y++)
                            {
                                for (int x = 0; x < sprite.rect.width; x++)
                                {
                                    var c = sprite.texture.GetPixel((int)sprite.rect.x + x, (int)sprite.rect.y + y);

                                    var xPosition = (animationLayer.IsFlippedHorizontally ? (sprite.rect.width - 1 - x) : x) + animationLayer.XPosition;
                                    var yPosition = (!animationLayer.IsFlippedVertically ? (sprite.rect.height - 1 - y) : y) + animationLayer.YPosition;

                                    xPosition -= minX;
                                    yPosition -= minY;

                                    if (c.a != 0)
                                        tex.SetPixel((int)xPosition, (int)(tex.height - yPosition - 1), c);
                                }
                            }
                        }

                        tex.Apply();

                        frames[frameIndex] = tex;
                    }

                    // Export animation
                    if (saveAsGif)
                    {
                        var speeds = stateGroup.Select(x => x.AnimSpeed).Distinct();

                        foreach (var speed in speeds)
                        {
                            using (MagickImageCollection collection = new MagickImageCollection())
                            {
                                int index = 0;

                                foreach (var tex in frames)
                                {
                                    var img = tex.ToMagickImage();
                                    collection.Add(img);
                                    collection[index].AnimationDelay = speed;
                                    collection[index].AnimationTicksPerSecond = 60;
                                    collection[index].Trim();

                                    collection[index].GifDisposeMethod = GifDisposeMethod.Background;
                                    index++;
                                }

                                // Save gif
                                collection.Write(Path.Combine(outputDir, $"{animIndex} ({speed}) - {animPointer}.gif"));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < frames.Length; i++)
                            Util.ByteArrayToFile(Path.Combine(outputDir, $"{animIndex}", $"{i}.png"), frames[i].EncodeToPNG());
                    }

                    animIndex++;
                }
            }
        }

        public async UniTask<Unity_Level> LoadAsync(Context context, bool loadTextures)
        {
            Controller.DetailedState = $"Loading data";
            await Controller.WaitIfNecessary();

            // Read the rom
            var rom = FileFactory.Read<SNES_Proto_ROM>(GetROMFilePath, context);

            Controller.DetailedState = $"Loading maps";
            await Controller.WaitIfNecessary();

            // Get the tilesets
            var tileSet = GetTileSet(rom);
            //var genericTileSet = GetGenericTileSet(rom);

            var maps = new Unity_Map[]
            {
                // TODO: Background is 2bpp and uses different tileset
                // Background
                //new Unity_Map()
                //{
                //    Type = Unity_Map.MapType.Graphics,
                //    Width = 32,
                //    Height = 32,
                //    TileSet = new Unity_TileSet[]
                //    {
                //        genericTileSet
                //    },
                //    MapTiles = rom.BackgroundTiles.Select(x => new Unity_Tile(new MapTile()
                //    {
                //        TileMapY = (ushort)(x.TileIndex + 1024),
                //        HorizontalFlip = x.FlipX,
                //        VerticalFlip = x.FlipY
                //    })).ToArray(),
                //},
                // Map
                new Unity_Map()
                {
                    Type = Unity_Map.MapType.Graphics | Unity_Map.MapType.Collision,

                    // Set the dimensions
                    Width = rom.MapData.Width,
                    Height = rom.MapData.Height,

                    // Create the tile arrays
                    TileSet = new Unity_TileSet[]
                    {
                        tileSet
                    },
                    MapTiles = rom.MapData.Tiles.Select(x => new Unity_Tile(x)).ToArray(),
                },
                // TODO: Uncomment after we make primary map 8px instead
                // Foreground
                //new Unity_Map()
                //{
                //    Type = Unity_Map.MapType.Graphics,
                //    Width = 32,
                //    Height = 32,
                //    TileSet = new Unity_TileSet[]
                //    {
                //        genericTileSet
                //    },
                //    MapTiles = rom.ForegroundTiles.Select(x => new Unity_Tile(new MapTile()
                //    {
                //        TileMapY = (ushort)x.TileIndex,
                //        HorizontalFlip = x.FlipX,
                //        VerticalFlip = x.FlipY
                //    })).ToArray(),
                //},
            };

            Controller.DetailedState = $"Loading sprites";
            await Controller.WaitIfNecessary();

            // Load sprites
            var sprites = GetSprites(rom);

            var objManager = new Unity_ObjectManager_SNES(context,
                rom.States.Select(x => new Unity_ObjectManager_SNES.State(x,
                x.Animation.ToCommonAnimation(baseSpriteIndex: x.VRAMConfigIndex * rom.ImageDescriptors.Length))).ToArray(), sprites);

            // Create Rayman
            var rayman = new Unity_Object_SNES(objManager);

            // Convert levelData to common level format
            Unity_Level level = new Unity_Level(
                maps: maps, 
                objManager: new Unity_ObjectManager(context),
                getCollisionTypeNameFunc: x => ((R1Jaguar_TileCollisionType)x).ToString(),
                getCollisionTypeGraphicFunc: x => ((R1Jaguar_TileCollisionType)x).GetCollisionTypeGraphic(),
                rayman: rayman);

            return level;
        }

        public Sprite[] GetSprites(SNES_Proto_ROM rom) {
            var sprites = new Sprite[rom.ImageDescriptors.Length * 3];

            var pal = Util.ConvertAndSplitGBAPalette(rom.SpritePalette);
            var buffer = new byte[rom.SpriteTileSet.Length];
            for (int addBlock = 0; addBlock < 3; addBlock++) {
                Array.Copy(rom.SpriteTileSet, buffer, buffer.Length);
                switch (addBlock) {
                    case 0:
                        Array.Copy(rom.SpriteTileSetAdd0, 0, buffer, 0xC00, 0x400);
                        Array.Copy(rom.SpriteTileSetAdd0, 0x400, buffer, 0x1000, 0x100);
                        Array.Copy(rom.SpriteTileSetAdd0, 0x500, buffer, 0x1200, 0x100);
                        break;
                    case 1:
                        Array.Copy(rom.SpriteTileSetAdd1, 0, buffer, 0xC00, 0x400);
                        Array.Copy(rom.SpriteTileSetAdd1, 0x400, buffer, 0x1000, 0x100);
                        Array.Copy(rom.SpriteTileSetAdd1, 0x500, buffer, 0x1200, 0x100);
                        break;
                    case 2:
                        Array.Copy(rom.SpriteTileSetAdd2, 0, buffer, 0xC00, 0x400);
                        break;
                }



                var tileSets = pal.Select(x => Util.ToTileSetTexture(buffer, x, Util.TileEncoding.Planar_4bpp, 8, true, wrap: 16, flipTileX: true)).ToArray();

                for (int i = 0; i < rom.ImageDescriptors.Length; i++) {
                    if (i == 0) {
                        sprites[i] = null;
                        continue;
                    }

                    var imgDescriptor = rom.ImageDescriptors[i];

                    var xPos = imgDescriptor.TileIndex % 16;
                    var yPos = (imgDescriptor.TileIndex - xPos) / 16;
                    var size = imgDescriptor.IsLarge ? 16 : 8;

                    sprites[addBlock * rom.ImageDescriptors.Length + i] = tileSets[imgDescriptor.Palette].CreateSprite(new Rect(xPos * 8, tileSets[imgDescriptor.Palette].height - yPos * 8 - size, size, size));
                }
            }
            return sprites;
        }

        public virtual Unity_TileSet GetTileSet(SNES_Proto_ROM rom)
        {
            // Read the tiles
            const int block_size = 0x20;

            uint length = (uint)rom.TileDescriptors.Length * 8 * 8;

            // Get the tile-set texture
            var tex = TextureHelpers.CreateTexture2D(256, Mathf.CeilToInt(length / 256f / Settings.CellSize) * Settings.CellSize, clear: true);

            var pal = Util.ConvertAndSplitGBAPalette(rom.TilePalette);

            for (int i = 0; i < rom.TileDescriptors.Length; i++)
            {
                var descriptor = rom.TileDescriptors[i];

                var x = ((i / 4) * 2) % (256 / 8) + ((i % 2) == 0 ? 0 : 1);
                var y = (((i / 4) * 2) / (256 / 8)) * 2 + ((i % 4) < 2 ? 0 : 1);

                var curOff = block_size * descriptor.TileIndex;

                tex.FillInTile(
                    imgData: rom.TileSet_0000, 
                    imgDataOffset: curOff, 
                    pal: pal[descriptor.Palette], 
                    encoding: Util.TileEncoding.Planar_4bpp, 
                    tileWidth: 8, 
                    flipTextureY: false,
                    tileX: x * 8,
                    tileY: y * 8,
                    flipTileX: !descriptor.FlipX,
                    flipTileY: descriptor.FlipY,
                    ignoreTransparent: true);
            }

            tex.Apply();

            return new Unity_TileSet(tex, Settings.CellSize);
        }
        public virtual Unity_TileSet GetGenericTileSet(SNES_Proto_ROM rom)
        {
            var tileDescriptors = rom.TileDescriptors.Concat(rom.BackgroundTiles).Concat(rom.ForegroundTiles).ToArray();
            var pal = Util.ConvertAndSplitGBAPalette(rom.TilePalette);

            var tex = Util.ToTileSetTexture(
                imgData: rom.TileSet_0000, 
                pal: pal.First(), 
                encoding: Util.TileEncoding.Planar_4bpp, 
                tileWidth: 8, 
                flipY: false, 
                flipTileX: true,
                getPalFunc: x => pal[tileDescriptors.FirstOrDefault(t => t.TileIndex == x)?.Palette ?? 0]);

            return new Unity_TileSet(tex, 8);
        }

        public UniTask SaveLevelAsync(Context context, Unity_Level level) => throw new NotImplementedException();

        public async UniTask LoadFilesAsync(Context context) => await context.AddLinearSerializedFileAsync(GetROMFilePath);
    }
}