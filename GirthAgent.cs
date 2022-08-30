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
        private float y;
        private float x;
        private Agent target;
        private Agent previousTarget;

        private bool hasProcreated = false;

        private int screenSizeX = 992;
        private int screenSizeY = 559;


        public GirthAgent(IPropertyStorage propertyStorage) : base(propertyStorage)
        {
            rand = new Random();

            //MovementSpeed = 35;
            //Strength = 30;
            //Health = 70;
            //Eyesight = 35;
            //Endurance = 45;
            //Dodge = 35;

            MovementSpeed = 60;
            Strength = 50;
            Health = 50;
            Eyesight = 45;
            Endurance = 45;
            Dodge = 0;


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


            //meet other Girth if both are able to multiply
            if (randAgent != null && randAgent.GetType() == typeof(GirthAgent) &&
              randAgent != this && ProcreationCountDown == 0 && randAgent.ProcreationCountDown == 0)
            {

                float agentX = randAgent.Position.X - Position.X;
                float agentY = randAgent.Position.Y - Position.Y;
                x = agentX;
                y = agentY;
                hasProcreated = true;
                return new Procreate(randAgent);
            }


            //if it has multiplied get new direction
            if (hasProcreated && ProcreationCountDown != 0)
            {
                y = rand.Next(-1, 2);
                x = rand.Next(-1, 2);
                MovementDirection(ref x, ref y);
                hasProcreated = false;
            }

            //follow enemy
            if (target != null && target.Health != 0 && target != previousTarget)
            {
                float targetX = target.Position.X - Position.X;
                float targetY = target.Position.Y - Position.Y;
                x = targetX;
                y = targetY;
            }

            //change direction once killed
            if (target != null && target.Hitpoints <= 0)
            {
                target = previousTarget;
                y = rand.Next(-1, 2);
                x = rand.Next(-1, 2);
                MovementDirection(ref x, ref y);
            }

            //sets a target and attacks
            if (nearEnemies.Count > 0 && nearEnemies.GetType() != typeof(GirthAgent))
            {
                target = (Agent)nearEnemies[0];
                return new Attack((Agent)nearEnemies[0]);
            }

            //moves toward plant
            if (plants.Count > 0)
            {
                float plantX = plants[0].Position.X - Position.X;
                float plantY = plants[0].Position.Y - Position.Y;
                x = plantX;
                y = plantY;
                return new Feed((Plant)plants[0]);
            }


            //change direction if standing still or at the edges
            MovementDirection(ref x, ref y);
            return new Move(new AIVector(x, y));



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
