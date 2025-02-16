﻿using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Ray1Map.Jade;
using UnityEngine;

namespace Ray1Map
{
    public class Jade_RRR_PC_Manager : Jade_Montpellier_BaseManager {
        // Levels
        public override LevelInfo[] LevelInfos => new LevelInfo[]
        {
            new LevelInfo(0x0002EA59, "ROOT/EngineDatas/06 Levels/_main/_main_bootup", "_main_bootup.wol"),
            new LevelInfo(0x000129BE, "ROOT/EngineDatas/06 Levels/_main/_main_credits", "_main_credits.wol"),
            new LevelInfo(0x0000C456, "ROOT/EngineDatas/06 Levels/_main/_main_logo", "_main_logo.wol"),
            new LevelInfo(0x00000B84, "ROOT/EngineDatas/06 Levels/_main/_main_Menu_Interface", "_main_Menu_Interface.wol"),
            new LevelInfo(0x000053C5, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE01_PulpFiction_Easy/MGDANSE01_PulpFiction_Easy_LD", "MGDANSE01_PulpFiction_Easy_LD.wol"),
            new LevelInfo(0x00005F6A, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE01_PulpFiction_Easy/MGDANSE01_PulpFiction_Easy_MULTI_LD", "MGDANSE01_PulpFiction_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005A85, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE01_PulpFiction_Hard/MGDANSE01_PulpFiction_Hard_LD", "MGDANSE01_PulpFiction_Hard_LD.wol"),
            new LevelInfo(0x00005F7B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE01_PulpFiction_Hard/MGDANSE01_PulpFiction_Hard_MULTI_LD", "MGDANSE01_PulpFiction_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005AFD, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE02_ChicGoodTime_Easy/MGDANSE02_ChicGoodTime_Easy_LD", "MGDANSE02_ChicGoodTime_Easy_LD.wol"),
            new LevelInfo(0x00005F7F, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE02_ChicGoodTime_Easy/MGDANSE02_ChicGoodTime_Easy_MULTI_LD", "MGDANSE02_ChicGoodTime_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005B13, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE02_ChicGoodTime_Hard/MGDANSE02_ChicGoodTime_Hard_LD", "MGDANSE02_ChicGoodTime_Hard_LD.wol"),
            new LevelInfo(0x00005F83, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE02_ChicGoodTime_Hard/MGDANSE02_ChicGoodTime_Hard_MULTI_LD", "MGDANSE02_ChicGoodTime_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005B14, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE03_CindyLauper_Easy/MGDANSE03_CindyLauper_Easy_LD", "MGDANSE03_CindyLauper_Easy_LD.wol"),
            new LevelInfo(0x00005F87, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE03_CindyLauper_Easy/MGDANSE03_CindyLauper_Easy_MULTI_LD", "MGDANSE03_CindyLauper_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005B15, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE03_CindyLauper_Hard/MGDANSE03_CindyLauper_Hard_LD", "MGDANSE03_CindyLauper_Hard_LD.wol"),
            new LevelInfo(0x00005F8B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE03_CindyLauper_Hard/MGDANSE03_CindyLauper_Hard_MULTI_LD", "MGDANSE03_CindyLauper_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005B16, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE04_HipHopHooray_Easy/MGDANSE04_HipHopHooray_Easy_LD", "MGDANSE04_HipHopHooray_Easy_LD.wol"),
            new LevelInfo(0x00005F8F, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE04_HipHopHooray_Easy/MGDANSE04_HipHopHooray_Easy_MULTI_LD", "MGDANSE04_HipHopHooray_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005B17, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE04_HipHopHooray_Hard/MGDANSE04_HipHopHooray_Hard_LD", "MGDANSE04_HipHopHooray_Hard_LD.wol"),
            new LevelInfo(0x00005F93, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE04_HipHopHooray_Hard/MGDANSE04_HipHopHooray_Hard_MULTI_LD", "MGDANSE04_HipHopHooray_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005B18, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE05_LaBamba_Easy/MGDANSE05_LaBamba_Easy_LD", "MGDANSE05_LaBamba_Easy_LD.wol"),
            new LevelInfo(0x00005F97, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE05_LaBamba_Easy/MGDANSE05_LaBamba_Easy_MULTI_LD", "MGDANSE05_LaBamba_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005B19, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE05_LaBamba_Hard/MGDANSE05_LaBamba_Hard_LD", "MGDANSE05_LaBamba_Hard_LD.wol"),
            new LevelInfo(0x00005F9B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE05_LaBamba_Hard/MGDANSE05_LaBamba_Hard_MULTI_LD", "MGDANSE05_LaBamba_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005B1A, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE06_Easy/MGDANSE06_Easy_LD", "MGDANSE06_Easy_LD.wol"),
            new LevelInfo(0x00005F9F, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE06_Easy/MGDANSE06_Easy_MULTI_LD", "MGDANSE06_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005B1B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE06_Hard/MGDANSE06_Hard_LD", "MGDANSE06_Hard_LD.wol"),
            new LevelInfo(0x00005FA3, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE06_Hard/MGDANSE06_Hard_MULTI_LD", "MGDANSE06_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005B1C, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE07_Easy/MGDANSE07_Easy_LD", "MGDANSE07_Easy_LD.wol"),
            new LevelInfo(0x00005FA7, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE07_Easy/MGDANSE07_Easy_MULTI_LD", "MGDANSE07_Easy_MULTI_LD.wol"),
            new LevelInfo(0x00005B1D, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE07_Hard/MGDANSE07_Hard_LD", "MGDANSE07_Hard_LD.wol"),
            new LevelInfo(0x00005FAB, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE07_Hard/MGDANSE07_Hard_MULTI_LD", "MGDANSE07_Hard_MULTI_LD.wol"),
            new LevelInfo(0x00005B1E, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE08/MGDANSE08_LD", "MGDANSE08_LD.wol"),
            new LevelInfo(0x00005FAF, "ROOT/EngineDatas/06 Levels/PRODUCTION_Danses/MGDANSE08/MGDANSE08_MULTI_LD", "MGDANSE08_MULTI_LD.wol"),
            new LevelInfo(0x00011497, "ROOT/EngineDatas/06 Levels/PRODUCTION_GLADIATOR/GLADIATOR_ARENE", "GLADIATOR_ARENE.wol"),
            new LevelInfo(0x00001435, "ROOT/EngineDatas/06 Levels/PRODUCTION_GLADIATOR/GLADIATOR_CACHOT", "GLADIATOR_CACHOT.wol"),
            new LevelInfo(0x00003349, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS00_TUTORIAL/FPS00_TUTORIAL_LD", "FPS00_TUTORIAL_LD.wol"),
            new LevelInfo(0x00007D0B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS01_BEACH_1/FPS01_BEACH_1_LD", "FPS01_BEACH_1.wol"),
            new LevelInfo(0x00002C1D, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS02_TRAIN_WESTERN/FPS02_TRAIN_WESTERN_LD", "FPS02_TRAIN_WESTERN_LD.wol"),
            new LevelInfo(0x0000352B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS03_CIMETIERE_MORT/FPS03_CIMETIERE_MORT_LD", "FPS03_CIMETIERE_MORT_LD.wol"),
            new LevelInfo(0x00002D66, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS04_CHARIOT_MINE/FPS04_CHARIOT_MINE_LD", "FPS04_CHARIOT_MINE_LD.wol"),
            new LevelInfo(0x0000556D, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS05_CANYON_DESERT/FPS05_CANYON_DESERT_LD", "FPS05_CANYON_DESERT_LD.wol"),
            new LevelInfo(0x00013816, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS06_BEACH_02/FPS06_BEACH_02_LD", "FPS06_BEACH_02_LD.wol"),
            new LevelInfo(0x0001261C, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS07_VILLE_WESTERN/FPS07_VILLE_WESTERN_LD", "FPS07_VILLE_WESTERN_LD.wol"),
            new LevelInfo(0x00006B44, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS08_COURSE_MORT/FPS08_COURSE_MORT_LD", "FPS08_COURSE_MORT_LD.wol"),
            new LevelInfo(0x00007E79, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_FPS/FPS09_DOOM_BASE/FPS09_DOOM_BASE_LD", "FPS09_DOOM_BASE_LD.wol"),
            new LevelInfo(0x00000ABB, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_RACES/RACE01_La_course_des_morts", "La_course_des_morts_avec_cine.wol"),
            new LevelInfo(0x00027747, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_RACES/RACE01B_La_course_des_morts_reverse", "RACE01B_La_course_des_morts_reverse.wol"),
            new LevelInfo(0x00027523, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_RACES/RACE04_phaco_atoll", "COURSE03_phaco_atoll.wol"),
            new LevelInfo(0x000277B1, "ROOT/EngineDatas/06 Levels/PRODUCTION_Levels_RACES/RACE06_phaco_western", "RACE06_phaco_western.wol"),
            new LevelInfo(0x000272B4, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG01_Corde_sauter", "RIV_Corde_sauter.wol"),
            new LevelInfo(0x0000E964, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG01_Corde_sauter_variante", "RIV_Corde_sauter_variante.wol"),
            new LevelInfo(0x000272B9, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG02_Course_atoll", "RIV_course_atoll.wol"),
            new LevelInfo(0x000272B5, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG03_Lancer_vache", "RIV_lancer_vache.wol"),
            new LevelInfo(0x00010E83, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG04_Toilette_Zone", "MiniGame_Toilette_Zone.wol"),
            new LevelInfo(0x0000140C, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG04_Toilette_Zone_Variante1", "MG04_Toilette_Zone_Variante1.wol"),
            new LevelInfo(0x00010F0B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG05_Traite_Des_Vaches", "PROTOTYPE_Traite_Des_Vaches.wol"),
            new LevelInfo(0x00005C12, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG05_Traite_Des_Vaches_Variante", "MG05_Traite_Des_Vaches_Variante.wol"),
            new LevelInfo(0x000054E7, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG06_Biere", "MG06_Biere.wol"),
            new LevelInfo(0x00006BB4, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG08_Medailles", "MG08_Medailles.wol"),
            new LevelInfo(0x0000179B, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG08_Medailles_Variante", "MG08_Medailles_Variante.wol"),
            new LevelInfo(0x000031E4, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG09_Ride_Minibat", "PROTOTYPE_MiniGame_Ride_Minibat.wol"),
            new LevelInfo(0x0000E970, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG09_Ride_Minibat_variante", "MG09_Ride_Minibat_variante.wol"),
            new LevelInfo(0x000274D3, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG10_Tirage_Vers", "MG10_Tirage_Vers.wol"),
            new LevelInfo(0x0000D849, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG10_Tirage_Vers_Variante1", "MG10_Tirage_Vers_Variante1.wol"),
            new LevelInfo(0x000054EF, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG11_Chariot_shadock", "MG11_Chariot_shadock.wol"),
            new LevelInfo(0x0000E93F, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG11_Chariot_shadock_Variante", "MG11_Chariot_shadock.wol"),
            new LevelInfo(0x000054F3, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG12_Chute_libre", "MG12_Chute_libre.wol"),
            new LevelInfo(0x0000779C, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG12_Chute_Libre_Variante", "MG12_Chute_Libre_Variante.wol"),
            new LevelInfo(0x000054FB, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG14_Bowling", "MG14_Bowling.wol"),
            new LevelInfo(0x00001469, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG15_Tonte_mouton_Variante1", "MG15_Tonte_mouton_Variante1.wol"),
            new LevelInfo(0x0000D797, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG16_Paparazzi", "MG16_Paparazzi.wol"),
            new LevelInfo(0x00003C8D, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG17_PacMan", "MG17_PacMan.wol"),
            new LevelInfo(0x00007498, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG17_PacMan_Variante", "MG17_PacMan_Variante.wol"),
            new LevelInfo(0x000074AD, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG17_PacMan_Variante2", "MG17_PacMan_Variante2.wol"),
            new LevelInfo(0x0000566A, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG19_Resto_lapins", "MG19_Resto_lapins.wol"),
            new LevelInfo(0x00005A31, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG19_Resto_lapins_Variante1", "MG19_Resto_lapins_Variante1.wol"),
            new LevelInfo(0x0000E8F6, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG21_ReveLapin", "MG21_ReveLapin.wol"),
            new LevelInfo(0x000056F8, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG22_Simon", "MG22_Simon.wol"),
            new LevelInfo(0x000017B4, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG22_Simon_Variante", "MG22_Simon_Variante.wol"),
            new LevelInfo(0x00005701, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG23_Tape_taupes", "MG23_Tape_taupes.wol"),
            new LevelInfo(0x000014C7, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG23_Tape_taupes_Variante1", "MG23_Tape_taupes_Variante1.wol"),
            new LevelInfo(0x0000570A, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG24_Lapin_maillard_LD", "MG24_Lapin_maillard_LD.wol"),
            new LevelInfo(0x00005713, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG25_Lancer_couteaux", "MG25_Lancer_couteaux.wol"),
            new LevelInfo(0x0000571C, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG26_Fight_arene", "MG26_Fight_arene.wol"),
            new LevelInfo(0x00005EF8, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG26_Fight_arene_Variante", "MG26_Fight_arene_Variante.wol"),
            new LevelInfo(0x0000E9A7, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG27_CervauxLapin", "MG27_CervauxLapin.wol"),
            new LevelInfo(0x00019B8D, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG28_MasterMind", "MG28_Mastermind.wol"),
            new LevelInfo(0x000058B2, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG29_123_Soleil", "MG29_123_Soleil.wol"),
            new LevelInfo(0x00001721, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG30_Marteau", "MG30_Marteau.wol"),
            new LevelInfo(0x00001633, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG31_LapinPunaise", "MG31_LapinPunaise.wol"),
            new LevelInfo(0x0000B413, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG32_Curling", "MG32_Curling.wol"),
            new LevelInfo(0x00005EEB, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG32_Curling_Variante", "MG32_Curling_Variante.wol"),
            new LevelInfo(0x0000584D, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG33_Encerclement", "MG33_Encerclement.wol"),
            new LevelInfo(0x00001529, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG34_GrappinSoucoupe", "MG34_GrappinSoucoupe.wol"),
            new LevelInfo(0x000058BB, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG35_LapinFoot", "MG35_LapinFoot.wol"),
            new LevelInfo(0x000017F9, "ROOT/EngineDatas/06 Levels/PRODUCTION_Minigames/MG36_Duel", "MG36_Duel.wol"),
        };

        // Version properties
        public override string[] BFFiles => new string[] {
            "Rayman4.bf"
        };


        // Game actions
        public override GameAction[] GetGameActions(GameSettings settings) {
            return base.GetGameActions(settings).Concat(new GameAction[]
            {
                new GameAction("Import files for reference", true, true, (input, output) => ImportSpecificReference(settings, input, output)),
			}).ToArray();
        }


        public async UniTask ImportSpecificReference(GameSettings settings, string inputDir, string outputDir) {
            // Read key list
            string keyListPath = Path.Combine(inputDir, "keylist.txt");
            string[] lines = File.ReadAllLines(keyListPath);
            Dictionary<uint, string> fileKeys = new Dictionary<uint, string>();
            foreach (var l in lines) {
                var lineSplit = l.Split(',');
                if(lineSplit.Length != 2) continue;
                uint k;
                if(uint.TryParse(lineSplit[0], System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out k))
                    fileKeys[k] = lineSplit[1].Replace('\\','/');

            }

            // Step 1: Load files to be loaded
            uint[] soundbankKeys = new uint[] {
                0x2C0008EF, // Rayman full
                0x2C00170B, // Lums full
                0x870043F8, // Aigle
                0x2C0007D5, // Araignée
                0x2C000B17, // Mana
                0x2C000A9C, // MiniRobot
            };
            Jade_Reference<SND_UnknownBank>[] banks = new Jade_Reference<SND_UnknownBank>[soundbankKeys.Length];
            using (var context = new Ray1MapContext(inputDir, settings)) {
                //await LoadFilesAsync(context);

                // Set up loader
                LOA_Loader actualLoader = new LOA_Loader(new BIG_BigFile[0], context);
                context.StoreObject<LOA_Loader>(LoaderKey, actualLoader);

                // Set up texture list
                TEX_GlobalList texList = new TEX_GlobalList();
                context.StoreObject<TEX_GlobalList>(TextureListKey, texList);

                // Set up sound list
                SND_GlobalList sndList = new SND_GlobalList();
                context.StoreObject<SND_GlobalList>(SoundListKey, sndList);

                BinarySerializer.SerializerObject s = context.Deserializer;

                // Resolve Soundbank
                for (int i = 0; i < banks.Length; i++) {
                    banks[i] = new Jade_Reference<SND_UnknownBank>(context, new Jade_Key(context, soundbankKeys[i]));
                    banks[i].Resolve();
                }
                await actualLoader.LoadLoop_RawFiles(s, fileKeys);

                Dictionary<uint, string> writtenFileKeys = new Dictionary<uint, string>();

                using (var writeContext = new Ray1MapContext(outputDir, settings)) {
                    // Set up loader
                    LOA_Loader loader = new LOA_Loader(actualLoader.BigFiles, writeContext);
                    loader.WrittenFileKeys = writtenFileKeys;
                    writeContext.StoreObject<LOA_Loader>(LoaderKey, loader);
                    var sndListWrite = new SND_GlobalList();
                    writeContext.StoreObject<SND_GlobalList>(SoundListKey, sndListWrite);
                    var texListWrite = new TEX_GlobalList();
                    writeContext.StoreObject<TEX_GlobalList>(TextureListKey, texListWrite);
                    var aiLinks = context.GetStoredObject<AI_Links>(AIKey);
                    writeContext.StoreObject<AI_Links>(AIKey, aiLinks);

                    for (int i = 0; i < banks.Length; i++) {
                        var bankRef = new Jade_Reference<SND_UnknownBank>(writeContext, new Jade_Key(writeContext, soundbankKeys[i])) {
                            Value = banks[i].Value
                        };
                        bankRef?.Resolve();
                    }
                    s = writeContext.Serializer;
                    await loader.LoadLoop(s);
                }


                StringBuilder b = new StringBuilder();
                foreach (var fk in writtenFileKeys) {
                    b.AppendLine($"{fk.Key:X8},{fk.Value}");
                }
                File.WriteAllText(Path.Combine(outputDir, "filekeys.txt"), b.ToString());

                Debug.Log($"Finished export");
            }
        }
    }
}