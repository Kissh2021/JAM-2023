using System;
using System.Collections;
using UnityEngine;

namespace Generics
{
	public class FunctionsDelayer : SingletonStaticPersistent<FunctionsDelayer>
	{
		private sealed class NonPersistantCoroutine : MonoBehaviour
		{
			private void Start() => enabled = false;
		}

		private static Transform folder;

		//TODO mettre à disposition un moyen d'annuler un DelayedCall
		public static void DelayedCall(Action callback, float startsInSeconds, bool isPersistant = false)
		{
			if (callback == null) throw new ArgumentNullException($"{nameof(callback)} must not be null");
			if (startsInSeconds < 0) throw new ArgumentOutOfRangeException($"{nameof(startsInSeconds)} must be >= 0");
			
			if (isPersistant)
			{
				Instance.StartCoroutine(DelayedCallCoroutine(callback, startsInSeconds));
			}
			else
			{
				if (folder == null) folder = new GameObject("#DelayedCoroutines").transform;
				MonoBehaviour go = new GameObject(
					callback.Target + "." + callback.Method.Name
				).AddComponent<NonPersistantCoroutine>();
				go.transform.parent = folder;
				go.StartCoroutine(DelayedCallCoroutine(callback + (() => Destroy(go.gameObject)), startsInSeconds));
			}
		}

		private static IEnumerator DelayedCallCoroutine(Action callback, float startsInSeconds)
		{
			yield return new WaitForSeconds(startsInSeconds);

			callback?.Invoke();
		}

		protected override void OnDestroy()
		{
			StopAllCoroutines();
			base.OnDestroy();
		}

		/// <summary>
		/// Work In Progress, use at your own risks
		/// </summary>
		/// <param name="callback"></param>
		/// <param name="condition"></param>
		/// <param name="conditionParameter"></param>
		/// <param name="eachTimeInSeconds"></param>
		/// <typeparam name="T"></typeparam>
		[Obsolete]
		public static void DelayedCallWhile<T>(Action callback, Predicate<T> condition, T conditionParameter,
			float eachTimeInSeconds)
		{
			Instance.StartCoroutine(DelayedCallWhileCoroutine(callback, condition, conditionParameter,
				eachTimeInSeconds));
			//TODO DelayedCallWhile
		}

		private static IEnumerator DelayedCallWhileCoroutine<T>(Action callback, Predicate<T> condition,
			T conditionParameter, float eachTimeInSeconds)
		{
			while (condition != null && callback != null && condition.Invoke(conditionParameter))
			{
				callback.Invoke();
				yield return new WaitForSeconds(eachTimeInSeconds);
			}
		}
	}
}