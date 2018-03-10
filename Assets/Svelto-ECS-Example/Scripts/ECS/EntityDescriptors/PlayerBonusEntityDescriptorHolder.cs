using Svelto.ECS.Example.Survive.Player.Bonus;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
	[DisallowMultipleComponent]
	public class PlayerBonusEntityDescriptorHolder : GenericEntityDescriptorHolder<GenericEntityDescriptor<PlayerBonusEntitityView>>
	{ }
}
