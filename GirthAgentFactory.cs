using AIFramework;
using AIFramework.Entities;
using System.Collections.Generic;
using System;

namespace GirthAgent
{
    public class GirthAgentFactory : AgentFactory
    {
        public override Agent CreateAgent(IPropertyStorage propertyStorage)
        {
            return new GirthAgent(propertyStorage);
        }

        public override Agent CreateAgent(Agent parent1, Agent parent2, IPropertyStorage propertyStorage)
        {
            return new GirthAgent(propertyStorage);
        }

        public override void RegisterWinners(List<Agent> sortedAfterDeathTime)
        {

        }

        public override Type ProvidedAgentType
        {
            get { return typeof(GirthAgent); }
        }

        public override string Creators
        {
            get { return "Daniel, Rolf, H.C og Frederik"; }
        }
    }
}
