using UnityEngine;

namespace Svelto.ECS.Example.Survive.HUD
{
    public interface IDamageHUDComponent: IComponent
    {
        float speed { get; }
        Color flashColor { get; }
        Color imageColor { set; get;  }
    }

    public interface IHealthSliderComponent: IComponent
    {
        int value { set; }
    }

    public interface IScoreComponent: IComponent
    {
        int score { set; get; }
    }

	public interface IWaveWaitingTimeComponent: IComponent
	{
		int secondsRemaining { set; get; }
	}

	public interface IEnemiesLeftComponent : IComponent
	{
		bool isEnabled { set; get; }
		int enemies { set; get; }
	}
	public interface IBulletsManagerComponent : IComponent
	{
		int currentBullets { set; get; }
		int totalBullets { set; get; }
		bool HasEnoughBullets();
		void RemoveBullet();
		void ResetBullets();
	}
}
