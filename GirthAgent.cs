using AIFramework.Actions;
using AIFramework.Entities;
using AIFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GirthAgent
{
    public class GirthAgent : Agent
    {
        Random rand;
        private AIVector previousPos;
        private float y;
        private float x;

        private int screenSizeX = 992;
        private int screenSizeY = 559;


        public GirthAgent(IPropertyStorage propertyStorage) : base(propertyStorage)
        {
            rand = new Random();

            MovementSpeed = 35;
            Strength = 35;
            Health = 70;
            Eyesight = 30;
            Endurance = 25;
            Dodge = 55;

            y = rand.Next(-1, 2);
            x = rand.Next(-1, 2);

        }
        public override void ActionResultCallback(bool success)
        {

        }

        //update metode
        public override IAction GetNextAction(List<IEntity> otherEntities)
        {
            List<IEntity> nearEnemies = otherEntities.FindAll(x => x.GetType() != typeof(GirthAgent) &&
            x is Agent && AIVector.Distance(Position, x.Position) < AIModifiers.maxMeleeAttackRange);

            List<IEntity> plants = otherEntities.FindAll(x => x is Plant &&
           AIVector.Distance(Position, x.Position) < AIModifiers.maxFeedingRange);

            List<Agent> agents = otherEntities.FindAll(a => a is GirthAgent).ConvertAll<Agent>(a => (GirthAgent)a);

            Agent randAgent = null;
            randAgent = agents[rand.Next(agents.Count)];





            if (randAgent != null && randAgent.GetType() == typeof(GirthAgent) && randAgent != this)
            {
                return new Procreate(randAgent);
            }

            
           

            if (nearEnemies.Count > 0 && nearEnemies.GetType() != typeof(GirthAgent))
            {
                return new Attack((Agent)nearEnemies[0]);
            }


            if (plants.Count > 0)
            {
                return new Feed((Plant)plants[0]);
            }


            MovementDirection(ref x, ref y);

            return new Move(new AIVector(x, y));





            //  MovementDirection(ref x, ref y);

            //  return new Move(new AIVector(x, y));









        }

        private void MovementDirection(ref float x, ref float y)
        {
            if (Position.X == screenSizeX || Position.Y == screenSizeY
                || Position.X == 0 || Position.Y == 0)
            {
                x = rand.Next(-1, 2);
                y = rand.Next(-1, 2);

            }
            if (x == 0 && y == 0)
            {
                x = rand.Next(-1, 2);
                y = rand.Next(-1, 2);
            }
        }
    }
}
