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
        Point position;

        float atkCooldown;

        Texture2D bulletTex;
        Texture2D effectTex;

        public Weapon(int weaponRange, int[,] attackArea, float cooldown)
        {
            range = weaponRange;
            atkArea = attackArea;
            atkCooldown = cooldown;
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager Content)
        {
            bulletTex = Content.Load<Texture2D>("bullet");
            effectTex = Content.Load<Texture2D>("effect");
        }

        public float GetCooldown()
        {
            return atkCooldown;
        }

        public Texture2D GetBulletTex()
        {
            return bulletTex;
        }

        public Texture2D GetEffectTex()
        {
            return effectTex;
        }

        public void Spawn(Point pos)
        {
            position = pos;
        }

        public int[,] GetArea()
        {
            return atkArea;
        }

        public int GetRange()
        {
            return range;
        }
    }
}
