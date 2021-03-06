﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pandemic
{
    class NPCManager
    {
        UInt32 numberOfRestGeneratableNPC;
        UInt16 NPCGenerationRate;
        float NPCBasicGenerationRate;

        float accumulateElapsedTime;
        float playTimer;
        const float generateInterval = 0.5f;

        List<NPC> NPCList;

        public Map map;

        public List<NPC> NPCs { get { return NPCList; } }

        Random randomGenerator;

        public float playTime { get { return playTimer; } }
        public UInt32 kill { get { return (UInt32)(2147483647 - numberOfRestGeneratableNPC - NPCList.Count + 5); } }
        public UInt32 survivor { get { return numberOfRestGeneratableNPC;  } }

        protected NPCManager()
        {
            NPCList = new List<NPC>();
            Initialize();
        }

        public void Initialize()
        {
            NPCList.Clear();
            numberOfRestGeneratableNPC = 2147483647;
            NPCGenerationRate = 5;
            NPCBasicGenerationRate = 5;
            accumulateElapsedTime = 0;
            randomGenerator = new Random();
            for (int i = 0; i < 5; i++)
            {
                NPCList.Add(createNPC(new Vector2(200, 200)));
            }
            playTimer = 0;
        }

        protected NPC createNPC(Vector2 playerPosition)
        {
            NPC newNPC = new NPC();
            newNPC.Spawn(playerPosition + new Vector2((randomGenerator.Next(2) == 1 ? -1 : 1) * (float)(randomGenerator.NextDouble() * 300 + 200), (randomGenerator.Next(2) == 1 ? -1 : 1) * (float)(randomGenerator.NextDouble() * 300 + 200)));
            return newNPC;
        }

        public void Update(float elapsedTime, Vector2 playerPosition, Bullet[] playerBullets)
        {
            accumulateElapsedTime += elapsedTime;
            NPCBasicGenerationRate += elapsedTime * 0.1f;
            //NPCBasicGenerationRate += elapsedTime * 0.03f;
            playTimer += elapsedTime;

            for (; accumulateElapsedTime > generateInterval; accumulateElapsedTime -= generateInterval)
            {
                NPCGenerationRate = (UInt16)(NPCBasicGenerationRate / 5 + 4 * Math.Sqrt(Math.Pow(NPCList.Count, 2.15) / Math.Sqrt(numberOfRestGeneratableNPC)));
                //NPCGenerationRate = (UInt16)(NPCBasicGenerationRate + 3 * Math.Pow(NPCList.Count, 1) + 2.1);
                for (int i = NPCGenerationRate; i > 0 && numberOfRestGeneratableNPC > 0; numberOfRestGeneratableNPC--, i--)
                {
                    NPC newNPC = createNPC(playerPosition);
                    NPCList.Add(newNPC);
                }
            }

            foreach (NPC npc in NPCList)
            {
                npc.SetDestination(playerPosition);
                npc.Update(elapsedTime);
                npc.CheckBulletCollision(playerBullets);
            }

            NPCList.RemoveAll(npc => !npc.IsAlive());
        }

        public void Move()
        {
            foreach (NPC npc in NPCList)
            {
                npc.Move(map.CalcRealDirection(npc));
            }
        }

        public void PostUpdate()
        {
            foreach (NPC npc in NPCList)
            {
                npc.PostUpdate();
            }
            NPCList.RemoveAll(npc => !npc.IsAlive());
        }

        public void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            foreach (NPC npc in NPCList)
            {
                npc.Draw(spriteBatch, screen);
            }
        }

        public List<Rectangle> GetNPCRectList()
        {
            List<Rectangle> rectList = new List<Rectangle>();

            foreach (NPC npc in NPCList)
            {
                rectList.Add(npc.GetRectangle());
            }

            return rectList;
        }

        private static NPCManager sharedManager = new NPCManager();
        public static NPCManager SharedManager
        {
            get { return sharedManager; }
        }
    }
}
