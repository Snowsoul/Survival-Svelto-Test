using Svelto.ECS.Example.Survive.Player.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class GrenadeHUDImplementor: MonoBehaviour, IImplementor, IGrenadeHUDComponent
    {
		public float sliderValue {
			get { return _sliderValue; }
			set {
				_sliderValue = value;
				GetComponent<Image>().fillAmount = sliderValue;
			}
		}

		float _sliderValue = 1;
    }
}