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
    };
    class Stage
    {
        public int MapWidth, MapHeight;
        public int ScreenWidth, ScreenHeight;
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
            PlayerInitialPosition = new Vector2(30, 20),
            weapons = new string[] { "basic", "advanced", "extreme" },
            obstacles = new string[] { "house1", "house2", "house3", "house4", "tree1", "tree2", "stone" },
            Units = new UnitTextures()
            {
                PlayerArmed = new Dictionary<string, RedirectiveTextures>() {
                    {"basic", new RedirectiveTextures() { DefaultTexture = "player" } }
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
                    {"basic", new List<string>() { "effect" } }
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
                    Cooldown = 1.0f,
                    range = 1,
                    EffectTimeOut = 0.1f
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
                    Cooldown = 0.5f,
                    range = 400,
                    EffectTimeOut = 0.1f
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
                    Cooldown = 1.5f,
                    range = 1,
                    EffectTimeOut = 0.1f
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
                    Cooldown = 0.2f,
                    range = 400,
                    EffectTimeOut = 0.1f
                }},
                {"RPG", new WeaponSpec() {
                    AttackAreaUp = new int[,] {
                    {1,1,1},
                    {1,2,1},
                    {1,1,1}},
                    AttackAreaDown = new int[,] {
                    {1,1,1},
                    {1,2,1},
                    {1,1,1}},
                    AttackAreaRight = new int[,] {
                    {1,1,1},
                    {1,2,1},
                    {1,1,1}},
                    AttackAreaLeft = new int[,] {
                    {1,1,1},
                    {1,2,1},
                    {1,1,1}},
                    Cooldown = 0.5f,
                    range = 400,
                    EffectTimeOut = 0.1f
                }}
            }
        };
    }
}
