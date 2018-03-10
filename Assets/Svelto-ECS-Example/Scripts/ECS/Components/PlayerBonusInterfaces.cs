namespace Svelto.ECS.Example.Survive.Player.Bonus
{
	public interface IPlayerAmmoBoxComponent : IComponent
	{
		bool colided { get; set; }
	}

	public interface IPlayerMedkitComponent : IComponent
	{
		bool colided { get; set; }
	}
}
