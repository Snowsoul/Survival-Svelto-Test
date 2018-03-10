using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player.Bonus
{
	public interface IPlayerAmmoBoxComponent : IComponent
	{
		bool colided { get; set; }
		void DestroyBox();
	}

	public interface IPlayerMedkitComponent : IComponent
	{
		bool colided { get; set; }
	}
}
