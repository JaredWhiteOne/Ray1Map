﻿using System.Collections.Generic;
using System.Linq;

namespace R1Engine
{
    public class GBAIsometric_RHR_ROM : GBA_ROMBase
    {
        public GBAIsometric_RHR_Localization Localization { get; set; }

        public GBAIsometric_RHR_PaletteAnimationTable[] PaletteAnimations { get; set; }
        public GBAIsometric_RHR_LevelInfo[] LevelInfos { get; set; }
        public GBAIsometric_ObjectType[] ObjectTypes { get; set; }
        public GBAIsometric_ObjectType[] AdditionalObjectTypes { get; set; }
        public GBAIsometric_RHR_AnimSet[] AdditionalAnimSets { get; set; }
        public GBAIsometric_RHR_SpriteSet[] SpriteSets { get; set; }
        public GBAIsometric_RHR_Sprite[] Sprites { get; set; }
        public Pointer[] SpriteIconPointers { get; set; }
        public GBAIsometric_RHR_Sprite[] SpriteIcons { get; set; }

        public GBAIsometric_RHR_Font Font0 { get; set; }
        public GBAIsometric_RHR_Font Font1 { get; set; }
        public GBAIsometric_RHR_Font Font2 { get; set; }

        public Pointer[] PortraitPointers { get; set; }
        public GBAIsometric_RHR_AnimSet[] Portraits { get; set; }

        public GBAIsometric_RHR_MapLayer[] MenuMaps { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize ROM header
            base.SerializeImpl(s);

            var pointerTable = PointerTables.GBAIsometric_RHR_PointerTable(s.GameSettings.GameModeSelection, Offset.file);

            // Serialize localization
            Localization = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.Localization], () => s.SerializeObject<GBAIsometric_RHR_Localization>(Localization, name: nameof(Localization)));

            if (s.GameSettings.World == 0)
            {
                // Serialize level infos
                s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.Levels], () =>
                {
                    if (LevelInfos == null)
                        LevelInfos = new GBAIsometric_RHR_LevelInfo[20];

                    for (int i = 0; i < LevelInfos.Length; i++)
                        LevelInfos[i] = s.SerializeObject(LevelInfos[i], x => x.SerializeData = i == s.GameSettings.Level, name: $"{nameof(LevelInfos)}[{i}]");
                });

                PaletteAnimations = new GBAIsometric_RHR_PaletteAnimationTable[3];
                s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.PaletteAnimations0], () => {
                    PaletteAnimations[0] = s.SerializeObject<GBAIsometric_RHR_PaletteAnimationTable>(PaletteAnimations[0], name: $"{nameof(PaletteAnimations)}[0]");
                });
                s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.PaletteAnimations1], () => {
                    PaletteAnimations[1] = s.SerializeObject<GBAIsometric_RHR_PaletteAnimationTable>(PaletteAnimations[1], name: $"{nameof(PaletteAnimations)}[1]");
                });
                s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.PaletteAnimations2], () => {
                    PaletteAnimations[2] = s.SerializeObject<GBAIsometric_RHR_PaletteAnimationTable>(PaletteAnimations[2], name: $"{nameof(PaletteAnimations)}[2]");
                });

                // Serialize object types
                ObjectTypes = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.ObjTypes], () => s.SerializeObjectArray<GBAIsometric_ObjectType>(ObjectTypes, 105, name: nameof(ObjectTypes)));

                // Serialize the crab type and add to the array (the crab entry points to memory)
                ObjectTypes[13].Data = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.CrabObjType], () => s.SerializeObject<GBAIsometric_ObjectTypeData>(ObjectTypes[13].Data, name: $"CrabObjectTypeData"));

                // Serialize additional object types
                var additionalObjTypePointers = ObjTypePointers[s.GameSettings.GameModeSelection];
                if (AdditionalObjectTypes == null)
                    AdditionalObjectTypes = new GBAIsometric_ObjectType[additionalObjTypePointers.Length];

                for (int i = 0; i < AdditionalObjectTypes.Length; i++)
                    AdditionalObjectTypes[i] = s.DoAt(new Pointer(additionalObjTypePointers[i], Offset.file), () => s.SerializeObject<GBAIsometric_ObjectType>(AdditionalObjectTypes[i], name: $"{nameof(AdditionalObjectTypes)}[{i}]"));

                // Serialize additional animation sets
                var additionalAnimSetPointers = AnimSetPointers[s.GameSettings.GameModeSelection];
                if (AdditionalAnimSets == null)
                    AdditionalAnimSets = new GBAIsometric_RHR_AnimSet[additionalAnimSetPointers.Length];

                for (int i = 0; i < AdditionalAnimSets.Length; i++)
                    AdditionalAnimSets[i] = s.DoAt(new Pointer(additionalAnimSetPointers[i], Offset.file), () => s.SerializeObject<GBAIsometric_RHR_AnimSet>(AdditionalAnimSets[i], name: $"{nameof(AdditionalAnimSets)}[{i}]"));

                // Serialize sprite sets
                var spriteSetPointers = SpriteSetPointers[s.GameSettings.GameModeSelection];
                if (SpriteSets == null)
                    SpriteSets = new GBAIsometric_RHR_SpriteSet[spriteSetPointers.Length];

                for (int i = 0; i < SpriteSets.Length; i++)
                    SpriteSets[i] = s.DoAt(new Pointer(spriteSetPointers[i], Offset.file), () => s.SerializeObject<GBAIsometric_RHR_SpriteSet>(SpriteSets[i], name: $"{nameof(SpriteSets)}[{i}]"));

                // Serialize sprites
                var spritePointers = SpritePointers[s.GameSettings.GameModeSelection];
                if (Sprites == null)
                    Sprites = new GBAIsometric_RHR_Sprite[spritePointers.Length];

                for (int i = 0; i < Sprites.Length; i++)
                    Sprites[i] = s.DoAt(new Pointer(spritePointers[i], Offset.file), () => s.SerializeObject<GBAIsometric_RHR_Sprite>(Sprites[i], name: $"{nameof(Sprites)}[{i}]"));

                // Serialize sprite icons
                SpriteIconPointers = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.SpriteIcons], () => s.SerializePointerArray(SpriteIconPointers, 84, name: nameof(SpriteIconPointers)));

                if (SpriteIcons == null)
                    SpriteIcons = new GBAIsometric_RHR_Sprite[SpriteIconPointers.Length];

                for (int i = 0; i < SpriteIcons.Length; i++)
                    SpriteIcons[i] = s.DoAt(SpriteIconPointers[i], () => s.SerializeObject<GBAIsometric_RHR_Sprite>(SpriteIcons[i], name: $"{nameof(SpriteIcons)}[{i}]"));

                // Serialize font
                Font0 = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.Font0], () => s.SerializeObject<GBAIsometric_RHR_Font>(Font0, name: nameof(Font0)));
                Font1 = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.Font1], () => s.SerializeObject<GBAIsometric_RHR_Font>(Font1, name: nameof(Font1)));
                Font2 = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.Font2], () => s.SerializeObject<GBAIsometric_RHR_Font>(Font2, name: nameof(Font2)));

                // Serialize portraits
                PortraitPointers = s.DoAt(pointerTable[GBAIsometric_RHR_Pointer.Portraits], () => s.SerializePointerArray(PortraitPointers, 10, name: nameof(PortraitPointers)));

                if (Portraits == null)
                    Portraits = new GBAIsometric_RHR_AnimSet[PortraitPointers.Length];

                for (int i = 0; i < Portraits.Length; i++)
                    Portraits[i] = s.DoAt(PortraitPointers[i], () => s.SerializeObject<GBAIsometric_RHR_AnimSet>(Portraits[i], name: $"{nameof(Portraits)}[{i}]"));
            }
            else
            {
                var maps = ((GBAIsometric_RHR_Manager)s.GameSettings.GetGameManager).GetMenuMaps(s.GameSettings.Level);
                MenuMaps = new GBAIsometric_RHR_MapLayer[maps.Length];

                for (int i = 0; i < MenuMaps.Length; i++)
                    MenuMaps[i] = s.DoAt(pointerTable[maps[i]], () => s.SerializeObject<GBAIsometric_RHR_MapLayer>(default, name: $"{maps[i]}"));

                /*
                 * Palette shift for Digital Eclipse logo (StartIndex,EndIndex,speed)
                 *  ShiftPaletteDigitalEclipse(0x12,0x20,1);
                    ShiftPaletteDigitalEclipse(0x22,0x30,1);
                    ShiftPaletteDigitalEclipse(0x32,0x40,1);
                    ShiftPaletteDigitalEclipse(0x42,0xd0,1);
                    ShiftPaletteDigitalEclipse(0xd1,0xf0,1);
                 * */
            }
        }

        public IEnumerable<GBAIsometric_RHR_AnimSet> GetAllAnimSets() => ObjectTypes.Concat(AdditionalObjectTypes).Select(x => x.Data?.AnimSetPointer.Value).Concat(AdditionalAnimSets).Concat(Portraits).Where(x => x != null).Distinct();

        public Dictionary<GameModeSelection, uint[]> ObjTypePointers => new Dictionary<GameModeSelection, uint[]>()
        {
            [GameModeSelection.RaymanHoodlumsRevengeUS] = new uint[]
            {
                0x080e6b68, // waterSplashAnimSet
                0x080e6af8, // cutsceneTeensieAnimSet
                0x080e6b14, // murfyAnimSet
                0x080e6b30, // sheftAnimSet
                0x080e6b4c, // lavaSteamAnimSet
                0x080e6b84, // begoniaxSmokeAnimSet
                0x080e6abc, // crabStarAnimSet
                0x080e7734, // plumTumblingAnimSet
                0x080e7750, // barrel1AnimSet
                0x080e83cc, // raymanFistAnimSet
                0x080e83b0, // raymanFistAnimSet
                0x080e84e4, // refluxOrbAnimSet
                0x080e851c, // {null}
                0x080e84c8, // cloneBarrelAnimSet
                0x080e8500, // refluxOrbAnimSet
                0x080e8640, // sparkAnimSet
                0x080e865c, // gemSparkAnimSet
                0x080e8740, // teensieCageAnimSet
                0x080e887c, // raymanToadAnimSet
                0x080e8898, // globoxToadAnimSet
                0x080e88b4, // begoToadAnimSet
            }
        };
        public Dictionary<GameModeSelection, uint[]> AnimSetPointers => new Dictionary<GameModeSelection, uint[]>()
        {
            [GameModeSelection.RaymanHoodlumsRevengeUS] = new uint[]
            {
                0x081e49bc, // shadowAnimSet
                0x081e4ce0, // targetAnimSet
                0x080f0968, // portraitTeensie_3low
                0x080f0a2c, // portraitTeensie_4low
                0x08549ed0, // fireMonsterFlamesAnimSet
                0x084214f4, // gearBlockAnimSet
                0x08449598, // raftShadowAnimSet
                0x08481e58, // dialogFrame
                0x08482234, // eyes
                0x08481f04, // piston
                0x080effd4, // bezel
                0x084826dc, // mapIcon

                // Unused
                0x0810f1d8, // raymanPafAnimSet
                0x081fc28c, // greenPowerupAnimSet
                0x081e6f7c, // spikeAnimSet
            }
        };
        public Dictionary<GameModeSelection, uint[]> SpriteSetPointers => new Dictionary<GameModeSelection, uint[]>()
        {
            [GameModeSelection.RaymanHoodlumsRevengeUS] = new uint[]
            {
                0x080f004c, // meterSpriteSet
                0x080f0098, // bossMeterSpriteSet
                0x084e6f38, // soundMeterTopSpriteSet
                0x084e6f80, // soundMeterBottomSpriteSet
            }
        };
        public Dictionary<GameModeSelection, uint[]> SpritePointers => new Dictionary<GameModeSelection, uint[]>()
        {
            [GameModeSelection.RaymanHoodlumsRevengeUS] = new uint[]
            {
                0x080eb3d0, // aButton
                0x080eb3f0, // bButton
                0x080eb410, // dPadUp
                0x080eb430, // dPadDown
                0x080eb454, // dPadLeft
                0x080eb478, // dPadRight
                0x080eb49c, // selector
                0x080eb4c8, // selector_yn
                0x080eb4ec, // cursor
                0x080eb50c, // dlgAButton
                0x080eb530, // dlgBButton
                0x080eb554, // dlgRButton
                0x080eb578, // dlgLButton
                0x080eb59c, // dlgStart
                0x080eb5c0, // dlgSelect
                0x080eb5e4, // dlgDpadUp
                0x080eb608, // dlgDpadDown
                0x080eb62c, // dlgDpadLeft
                0x080eb650, // dlgDpadRight
                0x080eb678, // dlgDpad
                0x080ef81c, // scoreCounterFrame
                0x080ef880, // scoreComboFrame
                0x080ef8b0, // comboText1
                0x080ef8d0, // comboText2
                0x080ef8f4, // teensyIcon
                0x080ef918, // lumIcon
                0x080ef938, // singleCounterFrame
                0x080ef96c, // doubleCounterFrame
                0x080ef998, // runeIcon1
                0x080ef9bc, // runeIcon2
                0x080ef9e0, // runeIcon3
                0x080efa04, // runeIcon4
                0x080efa28, // runeIcon5
                0x080efa4c, // currentIconNE
                0x080efa74, // currentIconNW
                0x080efa9c, // currentIconSE
                0x080efac4, // currentIconSW
                0x080efb04, // fireResistanceIcon
                0x080efb48, // copterIcon
                0x080efb84, // metalFistIcon
                0x080efbc4, // plumIcon
                0x080efc00, // frogIcon
                0x080efc24, // frameOverrunIcon
                0x080efc50, // murfyIconSmall
                0x080efc90, // murfyStamp
                0x080efcec, // stampFrame1
                0x080efd28, // stampFrame2
                0x080f0014, // meterLeftCap
                0x080f0ab8, // parchmentLeft
                0x080f0af8, // parchmentRight
                0x080f0b38, // parchmentCenter
                0x080f0bf8, // ingameDialogFrame
                0x08481fc0, // bottleHighlight0
                0x08482064, // bottleHighlight1
                0x08482108, // bottleHighlight2
                0x08482290, // englishFlag
                0x084822cc, // ukFlag
                0x08482304, // frenchFlag
                0x08482340, // germanFlag
                0x0848237c, // spanishFlag
                0x084823b8, // italianFlag
                0x084823f4, // dutchFlag
                0x08482638, // mapIconComplete
                0x0848265c, // mapIconBetween
                0x084e6e68, // RLArrow
                0x084e6e88, // leftButton
                0x084e6eac, // rightButton
                0x084e6fe4, // mapIconRayman
                0x084e7014, // cartouche
            }
        };
    }
}