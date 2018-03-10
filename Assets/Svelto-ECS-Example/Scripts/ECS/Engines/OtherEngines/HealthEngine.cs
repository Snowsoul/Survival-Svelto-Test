namespace Svelto.ECS.Example.Survive
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<HealInfo>
    {
        public void Ready()
        { }

        public HealthEngine(ISequencer damageSequence)
        {
            _damageSequence = damageSequence;
        }

        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Step(ref DamageInfo damage, int condition)
        {
            var entityView      = entityViewsDB.QueryEntityView<HealthEntityView>(damage.entityDamagedID);
            var healthComponent = entityView.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            //the HealthEngine can branch the sequencer flow triggering two different
            //conditions
            if (healthComponent.currentHealth <= 0)
                _damageSequence.Next(this, ref damage, DamageCondition.Dead);
            else
                _damageSequence.Next(this, ref damage, DamageCondition.Damage);
        }

		public void Step(ref HealInfo heal, int condition)
		{
			// Heal Sequence Step
			var entityView = entityViewsDB.QueryEntityView<HealthEntityView>(heal.entityHealID);
			var healthComponent = entityView.healthComponent;

			healthComponent.currentHealth += heal.healAmmount;
		}

		readonly ISequencer  _damageSequence;
    }
}
