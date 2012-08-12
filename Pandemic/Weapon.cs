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
    class Weapon : GameObject
    {
        int range;
        int[,] atkAreaUp;
        int[,] atkAreaDown;
        int[,] atkAreaRight;
        int[,] atkAreaLeft;

        float atkCooldown;

        static Texture2D bulletTex;
        static Texture2D effectTex;
        static Texture2D swordEffTex;
        static Texture2D daggerEffTex;
        static Dictionary<string, Texture2D> effTexDic = new Dictionary<string,Texture2D>();
        Texture2D tileTex;

        string name;

        protected override int rectSize { get { return 30; } }

        int damage;
        float effectTimeOut;

        bool alive;

        public Weapon(string weaponName)
        {
            name = weaponName;

            range = Stage.stageInstance.WeaponSpec[name].range;

            atkAreaUp = Stage.stageInstance.WeaponSpec[name].AttackAreaUp;
            atkAreaDown = Stage.stageInstance.WeaponSpec[name].AttackAreaDown;
            atkAreaRight = Stage.stageInstance.WeaponSpec[name].AttackAreaRight;
            atkAreaLeft = Stage.stageInstance.WeaponSpec[name].AttackAreaLeft;

            atkCooldown = Stage.stageInstance.WeaponSpec[name].Cooldown;
            effectTimeOut = Stage.stageInstance.WeaponSpec[name].EffectTimeOut;
            damage = Stage.stageInstance.WeaponSpec[name].damage;

            //effTexDic = new Dictionary<string, Texture2D>();
        }

        public string GetName()
        {
            return name;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Spawn(Vector2 pos)
        {
            base.Spawn(pos);
            position = pos;
            alive = true;
        }

        public override void Update(float elapsedGameTime)
        {
        }

        public override void PostUpdate()
        {
        }

        public void KillObject()
        {
            isAlive = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            tileTex = Content.Load<Texture2D>("box");
        }

        public static void LoadCommonContent(ContentManager Content)
        {
            bulletTex = Content.Load<Texture2D>(Stage.stageInstance.NonUnits.Bullet["basic"].DefaultTexture);
            effectTex = Content.Load<Texture2D>(Stage.stageInstance.NonUnits.Effect["basic"][0]);
            swordEffTex = Content.Load<Texture2D>(Stage.stageInstance.NonUnits.Effect["sword"][0]);
            daggerEffTex = Content.Load<Texture2D>(Stage.stageInstance.NonUnits.Effect["dagger"][0]);

            effTexDic.Add("handgun", effectTex);
            effTexDic.Add("rpg", effectTex);
            effTexDic.Add("gatling", effectTex);
            effTexDic.Add("sword", swordEffTex);
            effTexDic.Add("dagger", daggerEffTex);
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
            return effTexDic[name];
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

        public override void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            if (alive)
            {
                spriteBatch.Draw(tileTex, screen.translateWorldToScreen(GetRectangle()), Color.White);
            }
        }
    }
}
