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
        int[,] atkArea;
        Vector2 position;

        float atkCooldown;

        Texture2D bulletTex;
        Texture2D effectTex;
        Texture2D tileTex;

        int damage;

        bool alive;

        public Weapon(int weaponRange, int[,] attackArea, float cooldown, int dmg)
        {
            range = weaponRange;
            atkArea = attackArea;
            atkCooldown = cooldown;
            damage = dmg;
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
            // Load Tile Tex
        }

        public float GetCooldown()
        {
            return atkCooldown;
        }

        public int GetDamage()
        {
            return damage;
        }


        public Texture2D GetBulletTex()
        {
            return bulletTex;
        }

        public Texture2D GetEffectTex()
        {
            return effectTex;
        }

        public int[,] GetArea()
        {
            return atkArea;
        }

        public int GetRange()
        {
            return range;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(tileTex, position, Color.White);
            }
        }
    }
}
