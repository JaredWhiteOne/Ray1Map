﻿namespace R1Engine
{
    public class Jade_BGE_HD_Manager : Jade_BGE_Manager 
    {
        // Levels
        public override LevelInfo[] LevelInfos => new LevelInfo[] {
            new LevelInfo(0x00006334, "ROOT/EngineDatas/06 Levels/_main/_main_credits", "_main_credits.wol"),
            new LevelInfo(0x000084DF, "ROOT/EngineDatas/06 Levels/_main/_main_fix", "_main_fix.wol"),
            new LevelInfo(0x0000631B, "ROOT/EngineDatas/06 Levels/_main/_main_logo", "_main_logo.wol"),
            new LevelInfo(0x0000631A, "ROOT/EngineDatas/06 Levels/_main/_main_menu", "_main_menu.wol"),
            new LevelInfo(0x0000631F, "ROOT/EngineDatas/06 Levels/_main/_main_newgame", "_main_newgame.wol"),
            new LevelInfo(0x00001D26, "ROOT/EngineDatas/06 Levels/_main/_Main_VideoE3", "_Main_VideoE3.wol"),
            new LevelInfo(0x00003103, "ROOT/EngineDatas/06 Levels/_mdisk/Mdisk_00_01_notes_vaisseau", "Mdisk_00_01_notes_vaisseau.wol"),
            new LevelInfo(0x000093DE, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_00_02_Mr_de_Castellac", "MDisk_00_02_Mr_de_Castellac.wol"),
            new LevelInfo(0x00006111, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_01_05_Pour_Sally", "MDisk_01_05_Pour_Sally.wol"),
            new LevelInfo(0x0000A1A3, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_01_08_Cine", "MDisk_01_08_Cine.wol"),
            new LevelInfo(0x00002B5B, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_01_10_databank_army", "MDisk_01_10_databank_army.wol"),
            new LevelInfo(0x00001848, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_02_03_brief_entrepot", "MDisk_02_03_brief_entrepot.wol"),
            new LevelInfo(0x000030A8, "ROOT/EngineDatas/06 Levels/_mdisk/Mdisk_06_10_animaux_rares", "Mdisk_06_10_animaux_rares.wol"),
            new LevelInfo(0x00004764, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_06_12_catalog_mamago", "MDisk_06_12_catalog_mamago.wol"),
            new LevelInfo(0x00022627, "ROOT/EngineDatas/06 Levels/_mdisk/MDisk_JeuDuPalet", "Mdisk_JeuDuPalet.wol"),
            new LevelInfo(0x000030DA, "ROOT/EngineDatas/06 Levels/_mdisk/Mdisk_SPOON_511", "Mdisk_SPOON_511.wol"),
            new LevelInfo(0x000030E8, "ROOT/EngineDatas/06 Levels/_mdisk/Mdisk_SPOON_512", "Mdisk_SPOON_512.wol"),
            new LevelInfo(0x000030F1, "ROOT/EngineDatas/06 Levels/_mdisk/Mdisk_SPOON_513_entrepot", "Mdisk_SPOON_513_entrepot.wol"),
            new LevelInfo(0x000030FA, "ROOT/EngineDatas/06 Levels/_mdisk/Mdisk_SPOON_514_egouts", "Mdisk_SPOON_514_egouts.wol"),
            new LevelInfo(0x000057FD, "ROOT/EngineDatas/06 Levels/00_home/00_01_home_sas_hangar", "00_01_home_sas_hangar.wol"),
            new LevelInfo(0x00005833, "ROOT/EngineDatas/06 Levels/00_home/00_02_home_decryptage", "00_02_home_decryptage.wol"),
            new LevelInfo(0x000045A2, "ROOT/EngineDatas/06 Levels/00_home/00_03_dehors_maison_Intro", "00_03_dehors_maison_Intro.wol"),
            new LevelInfo(0x000046A6, "ROOT/EngineDatas/06 Levels/00_home/00_03_dehors_maison_Phare_detruit", "00_03_dehors_maison_Phare_detruit.wol"),
            new LevelInfo(0x000045A9, "ROOT/EngineDatas/06 Levels/00_home/00_03_dehors_maison_Waf", "00_03_dehors_maison_Waf.wol"),
            new LevelInfo(0x0000791B, "ROOT/EngineDatas/06 Levels/01_entrepot/01_00_entrepot_sas_entree", "01_00_entrepot_sas_entree.wol"),
            new LevelInfo(0x00006B96, "ROOT/EngineDatas/06 Levels/01_entrepot/01_01_entrepot_ascenseur_central", "01_01_entrepot_ascenseur_central.wol"),
            new LevelInfo(0x00006BFC, "ROOT/EngineDatas/06 Levels/01_entrepot/01_02_entrepot_salle_fusibles", "01_02_entrepot_salle_fusibles.wol"),
            new LevelInfo(0x000054D1, "ROOT/EngineDatas/06 Levels/01_entrepot/01_03_entrepot_2caisses_crans_vie", "01_03_entrepot_2caisses_crans_vie.wol"),
            new LevelInfo(0x00001FBE, "ROOT/EngineDatas/06 Levels/01_entrepot/01_04_entrepot_liberation_mili", "01_04_entrepot_liberation_mili.wol"),
            new LevelInfo(0x00005520, "ROOT/EngineDatas/06 Levels/01_entrepot/01_05_entrepot_enlevement_peyj", "01_05_entrepot_enlevement_peyj.wol"),
            new LevelInfo(0x00001814, "ROOT/EngineDatas/06 Levels/01_entrepot/01_06_entrepot_defile_cercueils", "01_06_entrepot_defile_cercueils.wol"),
            new LevelInfo(0x00007685, "ROOT/EngineDatas/06 Levels/01_entrepot/01_07_entrepot_debarras_caisses", "01_07_entrepot_debarras_caisses.wol"),
            new LevelInfo(0x000004F4, "ROOT/EngineDatas/06 Levels/01_entrepot/01_08_entrepot_cachot_peyj", "01_08_entrepot_cachot_peyj.wol"),
            new LevelInfo(0x00000352, "ROOT/EngineDatas/06 Levels/01_entrepot/01_09_entrepot_boss", "01_09_entrepot_boss.wol"),
            new LevelInfo(0x00007821, "ROOT/EngineDatas/06 Levels/01_entrepot/01_10_entrepot_ramener_cyclope", "01_10_entrepot_ramener_cyclope.wol"),
            new LevelInfo(0x00009E0A, "ROOT/EngineDatas/06 Levels/01_entrepot/01_11_entrepot_tuyau", "01_11_entrepot_tuyau.wol"),
            new LevelInfo(0x00005B59, "ROOT/EngineDatas/06 Levels/01_entrepot/01_12_Entrepot_tutorial_stealth_01", "01_12_Entrepot_tutorial_stealth_01.wol"),
            new LevelInfo(0x000054F1, "ROOT/EngineDatas/06 Levels/01_entrepot/01_13_entrepot_tutorial_stealth_02", "01_13_entrepot_tutorial_stealth_02.wol"),
            new LevelInfo(0x000056DB, "ROOT/EngineDatas/06 Levels/01_entrepot/01_14_entrepot_tutorial_stealth_00", "01_14_entrepot_tutorial_stealth_00.wol"),
            new LevelInfo(0x00000BDD, "ROOT/EngineDatas/06 Levels/03_egouts/03_00_egouts_circuit_start", "03_00_egouts_circuit_start.wol"),
            new LevelInfo(0x0000443C, "ROOT/EngineDatas/06 Levels/03_egouts/03_01_Ville_circuit_start", "03_01_Ville_circuit_start.wol"),
            new LevelInfo(0x0000201A, "ROOT/EngineDatas/06 Levels/03_egouts/03_04_egouts_ouverture_grille", "03_04_egouts_ouverture_grille.wol"),
            new LevelInfo(0x00000569, "ROOT/EngineDatas/06 Levels/03_egouts/03_05_egouts_cyclope_derriere", "03_05_egouts_cyclope_derriere.wol"),
            new LevelInfo(0x00001006, "ROOT/EngineDatas/06 Levels/03_egouts/03_06_egouts_abattoir_exterieur", "03_06_egouts_abattoir_exterieur.wol"),
            new LevelInfo(0x0000056C, "ROOT/EngineDatas/06 Levels/03_egouts/03_07_egouts_abattoir_interieur", "03_07_egouts_abattoir_interieur.wol"),
            new LevelInfo(0x00009415, "ROOT/EngineDatas/06 Levels/03_egouts/03_08_egouts_bonus_grille", "03_08_egouts_bonus_grille.wol"),
            new LevelInfo(0x00006147, "ROOT/EngineDatas/06 Levels/03_egouts/03_09_egouts_rencontre_passeur", "03_09_egouts_rencontre_passeur.wol"),
            new LevelInfo(0x000097D5, "ROOT/EngineDatas/06 Levels/03_egouts/03_10_egouts_acces_usine", "03_10_egouts_acces_usine.wol"),
            new LevelInfo(0x00002301, "ROOT/EngineDatas/06 Levels/03_egouts/03_11_Egouts_Porte_Usine", "03_11_Egouts_Porte_Usine.wol"),
            new LevelInfo(0x0000141D, "ROOT/EngineDatas/06 Levels/03_egouts/03_12_egouts_caisses_mines", "03_12_egouts_caisses_mines.wol"),
            new LevelInfo(0x0000686C, "ROOT/EngineDatas/06 Levels/03_egouts/03_14_egouts_stealth_01", "03_14_egouts_stealth_01.wol"),
            new LevelInfo(0x0000686D, "ROOT/EngineDatas/06 Levels/03_egouts/03_15_egouts_stealth_02", "03_15_egouts_stealth_02.wol"),
            new LevelInfo(0x00000988, "ROOT/EngineDatas/06 Levels/04_vaisseau/04_00_vaisseau_hyllis_planete", "04_00_vaisseau_hyllis_planete.wol"),
            new LevelInfo(0x00000B30, "ROOT/EngineDatas/06 Levels/04_vaisseau/04_01_vaisseau_hyllis_espace", "04_01_vaisseau_hyllis_espace.wol"),
            new LevelInfo(0x00007061, "ROOT/EngineDatas/06 Levels/04_vaisseau/04_02_vaisseau_lune", "04_02_vaisseau_lune.wol"),
            new LevelInfo(0x0000948D, "ROOT/EngineDatas/06 Levels/05_ilot/05_00_ilot_sas_accueil", "05_00_ilot_sas_accueil.wol"),
            new LevelInfo(0x00005EFB, "ROOT/EngineDatas/06 Levels/05_ilot/05_01_ilot_plaques", "05_01_ilot_plaques.wol"),
            new LevelInfo(0x0000112D, "ROOT/EngineDatas/06 Levels/05_ilot/05_02_ilot_boss", "05_02_ilot_boss.wol"),
            new LevelInfo(0x00002009, "ROOT/EngineDatas/06 Levels/05_ilot/05_03_Ilot_Medusa", "05_03_Ilot_Medusa.wol"),
            new LevelInfo(0x0000257A, "ROOT/EngineDatas/06 Levels/06_Animaux/06_00_Animaux_Canaux", "06_00_Animaux_Canaux.wol"),
            new LevelInfo(0x000066FD, "ROOT/EngineDatas/06 Levels/06_Animaux/06_01_animaux_quartier_pietons", "06_01_animaux_quartier_pietons.wol"),
            new LevelInfo(0x00009BE5, "ROOT/EngineDatas/06 Levels/06_Animaux/06_02_animaux_bar_paradise", "06_02_animaux_bar_paradise.wol"),
            new LevelInfo(0x00009C0B, "ROOT/EngineDatas/06 Levels/06_Animaux/06_03_animaux_boutique_electro", "06_03_animaux_boutique_electro.wol"),
            new LevelInfo(0x000046B0, "ROOT/EngineDatas/06 Levels/06_Animaux/06_04_Animaux_quartier_pietons_revolution", "06_00_Animaux_quartier_pietons_revolution.wol"),
            new LevelInfo(0x0000E127, "ROOT/EngineDatas/06 Levels/06_Animaux/06_08_animaux_Repaire_Spoon", "06_08_animaux_Repaire_Spoon.wol"),
            new LevelInfo(0x00002C8D, "ROOT/EngineDatas/06 Levels/06_Animaux/06_12_animaux_garage", "06_12_animaux_garage.wol"),
            new LevelInfo(0x00013E65, "ROOT/EngineDatas/06 Levels/06_Animaux/06_13_animaux_secret_02", "06_13_animaux_secret_02.wol"),
            new LevelInfo(0x0000E439, "ROOT/EngineDatas/06 Levels/06_Animaux/06_18_animaux_minimap_matrix", "06_18_animaux_minimap_matrix.wol"),
            new LevelInfo(0x0000FB16, "ROOT/EngineDatas/06 Levels/07_courses/07_00_course_00", "07_00_course_00.wol"),
            new LevelInfo(0x00008CD8, "ROOT/EngineDatas/06 Levels/07_courses/07_02_course_02", "07_02_course_02.wol"),
            new LevelInfo(0x000004C1, "ROOT/EngineDatas/06 Levels/07_courses/07_03_course_03", "07_03_course_03.wol"),
            new LevelInfo(0x000024EA, "ROOT/EngineDatas/06 Levels/07_courses/07_04_course_04", "07_04_course_04.wol"),
            new LevelInfo(0x0000ABB4, "ROOT/EngineDatas/06 Levels/08_Satellite/08_00_satellite_entree", "08_00_satellite_entree.wol"),
            new LevelInfo(0x00006F33, "ROOT/EngineDatas/06 Levels/08_Satellite/08_03_satellite_emetteur", "08_03_satellite_emetteur.wol"),
            new LevelInfo(0x0000AE8A, "ROOT/EngineDatas/06 Levels/09_Nazh/09_00_nazh_liberation_peyj", "09_00_nazh_liberation_peyj.wol"),
            new LevelInfo(0x0000B7AD, "ROOT/EngineDatas/06 Levels/09_Nazh/09_01_nazh_boss", "09_01_nazh_boss.wol"),
            new LevelInfo(0x0000EA33, "ROOT/EngineDatas/06 Levels/09_Nazh/09_01_nazh_boss_cine_finale", "09_01_nazh_boss_cine_finale.wol"),
            new LevelInfo(0x00015021, "ROOT/EngineDatas/06 Levels/09_Nazh/09_02_nazh_boss_ascenseur", "09_02_nazh_boss_ascenseur.wol"),
            new LevelInfo(0x0000738E, "ROOT/EngineDatas/06 Levels/10_Lune/10_02_lune_faisceaux", "10_02_lune_faisceaux.wol"),
            new LevelInfo(0x000033CB, "ROOT/EngineDatas/06 Levels/11_minimaps/11_00_vieux_fou_01", "11_00_vieux_fou_01.wol"),
            new LevelInfo(0x00000C5B, "ROOT/EngineDatas/06 Levels/11_minimaps/11_01_vieux_fou_02", "11_01_vieux_fou_02.wol"),
            new LevelInfo(0x00003501, "ROOT/EngineDatas/06 Levels/11_minimaps/11_02_vieux_fou_03", "11_02_vieux_fou_03.wol"),
            new LevelInfo(0x00001D41, "ROOT/EngineDatas/06 Levels/11_minimaps/11_03_vieux_fou_04", "11_03_vieux_fou_04.wol"),
            new LevelInfo(0x00000FC8, "ROOT/EngineDatas/06 Levels/11_minimaps/11_04_tresor_alpha_01", "11_04_tresor_alpha_01.wol"),
            new LevelInfo(0x000011B4, "ROOT/EngineDatas/06 Levels/11_minimaps/11_05_tresor_alpha_02", "11_05_tresor_alpha_02.wol"),
            new LevelInfo(0x0000DD6B, "ROOT/EngineDatas/06 Levels/11_minimaps/11_06_tresor_alpha_03", "11_06_tresor_alpha_03.wol"),
            new LevelInfo(0x0000F3F8, "ROOT/EngineDatas/06 Levels/11_minimaps/11_07_tapis_roulant", "11_07_tapis_roulant.wol"),
            new LevelInfo(0x00001FBA, "ROOT/EngineDatas/06 Levels/11_minimaps/11_08_Combat_01", "11_08_Combat_01.wol"),
            new LevelInfo(0x000013A3, "ROOT/EngineDatas/06 Levels/11_minimaps/11_10_Combat_03", "11_10_Combat_03.wol"),
            new LevelInfo(0x00003E1B, "ROOT/EngineDatas/06 Levels/11_minimaps/11_13_Combat_06", "11_13_Combat_06.wol"),
            new LevelInfo(0x00029588, "ROOT/EngineDatas/06 Levels/50_Gplay/YO_PEARL", "YO_pearl.wol"),

        };

        // Version properties
        public override string[] BFFiles => new string[] {
            "Data/Sally_PC_POLISH.bf"
        };
        public override string JadeSpePath => "jade.spe";
    }
}