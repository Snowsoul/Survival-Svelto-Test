using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
	public class WaveAnnouncerImplementor : MonoBehaviour, IImplementor, IWaveWaitingTimeComponent
	{
		public int secondsRemaining {
			get { return _secondsRemaining; }
			set {
				_secondsRemaining = value;
				_text.text = "Wave comming in " + _secondsRemaining + " seconds";
			}
		}

		void Awake()
		{
			// Set up the reference.
			_text = GetComponent<Text>();

			// Reset the score.
			_secondsRemaining = _initialSecondsRemaining;
		}

		int _secondsRemaining;
		int _initialSecondsRemaining = 3;

		Text _text;
	}
}
