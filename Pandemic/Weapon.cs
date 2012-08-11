using System;
using System.Collections.Generic;
using System.Linq;
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
    class Weapon
    {
        int range;
        int[,] atkAreaUp;
        int[,] atkAreaDown;
        int[,] atkAreaRight;
        int[,] atkAreaLeft;
        Vector2 position;

        float atkCooldown;

        Texture2D bulletTex;
        Texture2D effectTex;
        Texture2D tileTex;

        int damage;
        float effectTimeOut;

        bool alive;

        public Weapon(string name)
        {
            range = Stage.stageInstance.WeaponSpec[name].range;

            atkAreaUp = Stage.stageInstance.WeaponSpec[name].AttackAreaUp;
            atkAreaDown = Stage.stageInstance.WeaponSpec[name].AttackAreaDown;
            atkAreaRight = Stage.stageInstance.WeaponSpec[name].AttackAreaRight;
            atkAreaLeft = Stage.stageInstance.WeaponSpec[name].AttackAreaLeft;

            atkCooldown = Stage.stageInstance.WeaponSpec[name].Cooldown;
            effectTimeOut = Stage.stageInstance.WeaponSpec[name].EffectTimeOut;
            damage = 34;
        }

        public void Initialize()
        {
        }

        public void Spawn(Vector2 pos)
        {
            position = pos;
            alive = true;
        }

        public void LoadContent(ContentManager Content)
        {
            bulletTex = Content.Load<Texture2D>(Stage.stageInstance.NonUnits.Bullet["basic"].DefaultTexture);
            effectTex = Content.Load<Texture2D>(Stage.stageInstance.NonUnits.Effect["basic"][0]);
            // load tileTex
        }

        public float GetCooldown()
        {
            return atkCooldown;
        }

        public int GetDamage()
        {
            return damage;
        }

        public float GetEffectTimeOut()
        {
            return effectTimeOut;
        }

        public Texture2D GetBulletTex()
        {
            return bulletTex;
        }

        public Texture2D GetEffectTex()
        {
            return effectTex;
        }

        public int[,] GetAreaUp()
        {
            return atkAreaUp;
        }

        public int[,] GetAreaDown()
        {
            return atkAreaDown;
        }

        public int[,] GetAreaRight()
        {
            return atkAreaRight;
        }

        public int[,] GetAreaLeft()
        {
            return atkAreaLeft;
        }

        public int GetRange()
        {
            return range;
        }

        public void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            if (alive)
            {
                spriteBatch.Draw(tileTex, position, Color.White);
            }
        }
    }
}
