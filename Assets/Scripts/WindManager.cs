using System;
using UnityEngine;

public static class WindManager
{
	[Range(-10f, 10f)] private static float windValue;

	public static float WindValue
	{
		set
		{
			windValue = Mathf.Clamp(value, -10f, 10f);
			try
			{
				OnWindValueChanged?.Invoke(windValue);
			}
			catch (Exception e)
			{
				OnWindValueChanged = null;
			}
		}
		get => windValue;
	}

	public static event Action<float> OnWindValueChanged;
}
