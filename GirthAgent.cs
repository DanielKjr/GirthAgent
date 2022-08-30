using AIFramework.Actions;
using AIFramework.Entities;
using AIFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GirthAgent
{
    public class GirthAgent : Agent
    {
        Random rand;
        private AIVector previousPos;
        private float y;
        private float x;

        public GirthAgent(IPropertyStorage propertyStorage) : base(propertyStorage)
        {
            rand = new Random();

            MovementSpeed = 45;
            Strength = 65;
            Health = 30;
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

            List<Agent> agents = otherEntities.FindAll(a => a is Agent).ConvertAll<Agent>(a => (Agent)a);

            Agent randAgent = null;
            randAgent = agents[rand.Next(agents.Count)];



            switch (rand.Next(5))
            {
                case 1://Procreate
                    if (randAgent != null && randAgent.GetType() == typeof(GirthAgent))
                    {
                        return new Procreate(randAgent);
                    }
                    break;

                case 2: //attack
                    if (nearEnemies.Count > 0 && nearEnemies.GetType() != typeof(GirthAgent))
                    {
                        return new Attack((Agent)nearEnemies[0]);
                    }
                    break;
                case 3: //eat
                    if (plants.Count > 0)
                    {
                        return new Feed((Plant)plants[0]);
                    }
                    break;
                case 4: //move

                    MovementDirection(previousPos, ref x, ref y);
                    previousPos = Position;
                    return new Move(new AIVector(x, y));

                default:
                    return new Defend();

            }





            //  MovementDirection(previousPos, ref x, ref y);
            previousPos = Position;
            return new Move(new AIVector(x, y));



        }

        private void MovementDirection(AIVector position, ref float x, ref float y)
        {
            if (position != null && Position == position)
            {



                x = rand.Next(-1, 2);
                y = rand.Next(-1, 2);






            }
        }
    }
}
