using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerEntityView : EntityView
    {
        public IPlayerInputComponent inputComponent;
        
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
        public ITransformComponent     transformComponent;
    }

    public interface IPlayerInputComponent
    {
        Vector3 input { get; set; }
        Ray camRay { get; set; }
        bool fire { get; set; }
		bool rmb { get; set; }
    }

    public class PlayerTargetEntityView : EntityView
    {
        public IPlayerTargetComponent     playerTargetComponent;
    }
}

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class GunEntityView : EntityView
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
		public IGrenadeSpawnerComponent  grenadeSpawnerComponent;
    }

	public class GrenadeEntityView : EntityView
	{
		public IGrenadeComponent grenadeComponent;
	}
}

namespace Svelto.ECS.Example.Survive.Player.Bonus
{
	public class PlayerAmmoboxEntityView : EntityView
	{
		public IPlayerAmmoBoxComponent playerAmmoBoxComponent;
	}

	public class PlayerMedkitEntityView : EntityView
	{
		public IPlayerMedkitComponent  playerMedkitComponent;
	}
}
