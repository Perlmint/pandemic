using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pandemic
{
    class NPCManager
    {
        UInt64 numberOfRestGeneratableNPC;
        UInt16 NPCGenerateRate;

        float accumulateElapsedTime;
        const float generateInterval = 3;

        List<NPC> NPCList;

        Random randomGenerator;

        protected NPCManager()
        {
            NPCList = new List<NPC>();
            Initialize();
        }

        public void Initialize()
        {
            NPCList.Clear();
            numberOfRestGeneratableNPC = 18446744073709551615;
            NPCGenerateRate = 1;
            accumulateElapsedTime = 0;
            randomGenerator = new Random();
        }

        public void Update(float elapsedTime, Vector2 playerPosition, Bullet[] playerBullets)
        {
            accumulateElapsedTime += elapsedTime;

            for (; accumulateElapsedTime > generateInterval; accumulateElapsedTime -= generateInterval)
            {
                for (int i = NPCGenerateRate; i > 0 && numberOfRestGeneratableNPC > 0; numberOfRestGeneratableNPC--, i--)
                {
                    NPC newNPC = new NPC();
                    newNPC.Spawn(playerPosition + new Vector2((float)(randomGenerator.NextDouble() * 300 + 200), (float)(randomGenerator.NextDouble() * 300 + 200)));
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

            NPCGenerateRate += (UInt16)Math.Sqrt(NPCList.Count);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (NPC npc in NPCList)
            {
                npc.Draw(spriteBatch);
            }
        }

        private static NPCManager sharedManager = new NPCManager();
        public static NPCManager SharedManager
        {
            get { return sharedManager; }
        }
    }
}
