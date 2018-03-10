using UnityEngine;
using Svelto.ECS.Example.Survive.Player.Bonus;

namespace Svelto.ECS.Example.Survive.HUD
{
    [DisallowMultipleComponent]
	public class HudEntityDescriptorHolder:GenericEntityDescriptorHolder<GenericEntityDescriptor<HUDEntityView>>
	{}
}