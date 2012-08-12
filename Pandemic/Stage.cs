using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Pandemic
{
    public enum PlayerDirection
    {
        up, right, down, left
    };
    public class RedirectiveTextures : Dictionary<PlayerDirection, string>
    {
        public RedirectiveTextures()
        {
        }
        public RedirectiveTextures(string def)
        {
            DefaultTexture = def;
        }
        public string DefaultTexture;
        public string get(PlayerDirection key)
        {
            string texture;
            if (this.TryGetValue(key, out texture))
                return texture;
            else
                return DefaultTexture;
        }
        public bool ETryGetValue(PlayerDirection key, out string value)
        {
            string evalue;
            if (TryGetValue(key, out evalue))
                value = evalue;
            else
                value = DefaultTexture;
            return true;
        }
    }
    public class UnitTextures
    {
        public Dictionary<string, RedirectiveTextures> PlayerArmed;
        public string Player_Death;
        public string Enemy;
        public string Normal;
        public string Dead;
    };
    public class NonUnitTextures
    {
        public Dictionary<string, RedirectiveTextures> WeaponOnly;
        public Dictionary<string, RedirectiveTextures> Bullet;
        public Dictionary<string, List<string>> Effect;
        public List<string> Ground;
        public Dictionary<string, string> Obstacle;
    };

    public class WeaponSpec
    {
        public int[,] AttackAreaUp;
        public int[,] AttackAreaDown;
        public int[,] AttackAreaLeft;
        public int[,] AttackAreaRight;
        public float Cooldown;
        public int range;
        public float EffectTimeOut;
        public int damage;
    };
    class Stage
    {
        public int MapWidth, MapHeight;
        public int ScreenWidth, ScreenHeight;
        public int TileWidth, TileHeight;
        public Vector2 PlayerInitialPosition;
        public string[] weapons;
        public string[] obstacles;
        public UnitTextures Units;
        public NonUnitTextures NonUnits;
        public Dictionary<int /*appear_uptime_in_map*/, int /*num_of_enemies*/> NPCsAppearSpec;
        public Dictionary<int /*appear_uptime_in_map*/, string /*name_of_weapon*/> WeaponsAppearSpec;
        public Dictionary<Vector2 /*position_in_map*/, string /*name_of_obstacle*/> ObstaclesArrangeSpec;
        public Dictionary<string, WeaponSpec> WeaponSpec;

        public static Stage stageInstance = new Stage()
        {
            MapWidth = 2400,
            MapHeight = 1800,
            ScreenWidth = 800,
            ScreenHeight = 600,
            TileWidth = 50,
            TileHeight = 50,
            PlayerInitialPosition = new Vector2(0, 0),
            weapons = new string[] { "basic", "advanced", "extreme" },
            obstacles = new string[] { "house1", "house2", "house3", "house4", "tree1", "tree2", "stone" },
            Units = new UnitTextures()
            {
                PlayerArmed = new Dictionary<string, RedirectiveTextures>() {
                    {"dagger", new RedirectiveTextures("player") { 
                    {PlayerDirection.up, "Character\\dagger\\dagger-up"},
                    {PlayerDirection.down, "Character\\dagger\\dagger-down"},
                    {PlayerDirection.right, "Character\\dagger\\dagger-right"},
                    {PlayerDirection.left, "Character\\dagger\\dagger-left"}}},
                    
                    {"gatling", new RedirectiveTextures("player") {
                    {PlayerDirection.up, "Character\\gatling\\machine gun-up"},
                    {PlayerDirection.down, "Character\\gatling\\machine gun-down"},
                    {PlayerDirection.right, "Character\\gatling\\machine gun-right"},
                    {PlayerDirection.left, "Character\\gatling\\machine gun-left"}}},

                    {"rpg", new RedirectiveTextures("player") {
                    {PlayerDirection.up, "Character\\rpg\\rpg-up"},
                    {PlayerDirection.down, "Character\\rpg\\rpg-down"},
                    {PlayerDirection.right, "Character\\rpg\\rpg-right"},
                    {PlayerDirection.left, "Character\\rpg\\rpg-left"}}},

                    {"sword", new RedirectiveTextures("player") {
                    {PlayerDirection.up, "Character\\sword\\sword-up"},
                    {PlayerDirection.down, "Character\\sword\\sword-down"},
                    {PlayerDirection.right, "Character\\sword\\sword-right"},
                    {PlayerDirection.left, "Character\\sword\\sword-left"}}},

                    {"handgun", new RedirectiveTextures("player") {
                    {PlayerDirection.up, "Character\\handgun\\gun-up"},
                    {PlayerDirection.down, "Character\\handgun\\gun-down"},
                    {PlayerDirection.right, "Character\\handgun\\gun-right"},
                    {PlayerDirection.left, "Character\\handgun\\gun-left"}
                    } }
                },
                Player_Death = "player_death",
                Enemy = "enemy",
                Normal = "normal",
                Dead = "blood"
            },
            NonUnits = new NonUnitTextures()
            {
                WeaponOnly = new Dictionary<string, RedirectiveTextures>() {
                    {"advanced", new RedirectiveTextures() { DefaultTexture = "weapon" } }
                },
                Bullet = new Dictionary<string, RedirectiveTextures>() {
                    {"basic", new RedirectiveTextures() { DefaultTexture = "bullet" } }
                },
                Effect = new Dictionary<string, List<string>>() {
                    {"basic", new List<string>() { "effect" } },
                    {"dagger", new List<string>() { "Character\\dagger\\daggereff"}},
                    {"sword", new List<string>() { "Character\\sword\\swordeff"}}
                },
                Ground = new List<string> {
                    "ground1", "ground2"
                },
                Obstacle = new Dictionary<string, string>() {
                    {"house1", "house1"},
                    {"house2", "house2"},
                    {"house3", "house3"},
                    {"house4", "house4"},
                    {"stone", "stone"},
                    {"tree1", "tree1"},
                    {"tree2", "tree2"}
                        
                }
            },
            NPCsAppearSpec = new Dictionary<int, int>()
            {
                {0, 2},
                {20, 5},
                {60, 50}
            },
            WeaponsAppearSpec = new Dictionary<int, string>()
            {
                {0, "basic"},
                {10, "advanced"},
                {30, "extreme"}
            },
            ObstaclesArrangeSpec = new Dictionary<Vector2, string>()
            {
                {new Vector2(-10, 1), "house1"},
                {new Vector2(-10, 2), "house3"},
                {new Vector2(-9, 1), "house2"},
                {new Vector2(-9, 2), "house4"},
                {new Vector2(-8, 1), "house1"},
                {new Vector2(-8, 2), "house3"},
                {new Vector2(-7, 1), "house2"},
                {new Vector2(-7, 2), "house4"},
                {new Vector2(-10, -1), "house1"},
                {new Vector2(-10, 0), "house3"},
                {new Vector2(-9, -1), "house2"},
                {new Vector2(-9, 0), "house4"},
                {new Vector2(-8, -1), "house1"},
                {new Vector2(-8, 0), "house3"},
                {new Vector2(-7, -1), "house2"},
                {new Vector2(-7, 0), "house4"},
                {new Vector2(-3, -2), "tree1"},
                {new Vector2(-3, -1), "tree2"},
                {new Vector2(6, 1), "tree1"},
                {new Vector2(6, 2), "tree2"},
                {new Vector2(7, 5), "tree1"},
                {new Vector2(7, 6), "tree2"},
                {new Vector2(11, 1), "tree1"},
                {new Vector2(11, 2), "tree2"},
                {new Vector2(1, -3), "tree1"},
                {new Vector2(1, -2), "tree2"},
                {new Vector2(5, 1), "stone"},
                {new Vector2(7, 4), "stone"},
                {new Vector2(9, 6), "stone"},
                {new Vector2(10, 6), "stone"},
                {new Vector2(11, 6), "stone"},
                {new Vector2(12, 6), "stone"}

            },

            WeaponSpec = new Dictionary<string,WeaponSpec>()
            {
                {"dagger", new WeaponSpec() {
                    AttackAreaLeft = new int[,] {
                    {0,1,0},
                    {1,1,1},
                    {1,2,1}},
                    AttackAreaRight = new int[,] {
                    {1,2,1},
                    {1,1,1},
                    {0,1,0}},
                    AttackAreaDown = new int[,] {
                    {1,1,0},
                    {2,1,1},
                    {1,1,0}},
                    AttackAreaUp = new int[,] {
                    {0,1,1},
                    {1,1,2},
                    {0,1,1}},
                    Cooldown = 0.15f,
                    range = 1,
                    EffectTimeOut = 0.1f,
                    damage = 50
                }},
                {"handgun", new WeaponSpec() {
                    AttackAreaUp = new int[,] {
                    {2}},
                    AttackAreaDown = new int[,] {
                    {2}},
                    AttackAreaRight = new int[,] {
                    {2}},
                    AttackAreaLeft = new int[,] {
                    {2}},
                    Cooldown = 0.3f,
                    range = 400,
                    EffectTimeOut = 0.1f,
                    damage = 100
                }},
                {"sword", new WeaponSpec() {
                    AttackAreaLeft = new int[,] {
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,1,1,1,0},
                    {0,1,1,1,0},
                    {1,1,2,1,1}},
                    AttackAreaRight = new int[,] {
                    {1,1,2,1,1},
                    {0,1,1,1,0},
                    {0,1,1,1,0},
                    {0,0,1,0,0},
                    {0,0,0,0,0}},
                    AttackAreaDown = new int[,] {
                    {1,0,0,0,0},
                    {1,1,1,1,0},
                    {2,1,1,1,1},
                    {1,1,1,1,0},
                    {1,0,0,0,0}},
                    AttackAreaUp = new int[,] {
                    {0,0,0,0,1},
                    {0,1,1,1,1},
                    {1,1,1,1,2},
                    {0,1,1,1,1},
                    {0,0,0,0,1}},
                    Cooldown = 0.4f,
                    range = 1,
                    EffectTimeOut = 0.1f,
                    damage = 100
                }},
                {"gatling", new WeaponSpec() {
                    AttackAreaUp = new int[,] {
                    {2}},
                    AttackAreaDown = new int[,] {
                    {2}},
                    AttackAreaRight = new int[,] {
                    {2}},
                    AttackAreaLeft = new int[,] {
                    {2}},
                    Cooldown = 0.1f,
                    range = 400,
                    EffectTimeOut = 0.1f,
                    damage = 50
                }},
                {"rpg", new WeaponSpec() {
                    AttackAreaUp = new int[,] {
                    {0,1,1,1,0},
                    {1,1,1,1,1},
                    {1,1,2,1,1},
                    {1,1,1,1,1},
                    {0,1,1,1,0}},
                    AttackAreaDown = new int[,] {
                    {0,1,1,1,0},
                    {1,1,1,1,1},
                    {1,1,2,1,1},
                    {1,1,1,1,1},
                    {0,1,1,1,0}},
                    AttackAreaRight = new int[,] {
                    {0,1,1,1,0},
                    {1,1,1,1,1},
                    {1,1,2,1,1},
                    {1,1,1,1,1},
                    {0,1,1,1,0}},
                    AttackAreaLeft = new int[,] {
                    {0,1,1,1,0},
                    {1,1,1,1,1},
                    {1,1,2,1,1},
                    {1,1,1,1,1},
                    {0,1,1,1,0}},
                    Cooldown = 0.4f,
                    range = 200,
                    EffectTimeOut = 0.5f,
                    damage = 100
                }}
            }
        };
    }
}
