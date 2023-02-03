using UnityEngine;

namespace Generics
{
	/// <summary>
	/// Singleton used the same way as a static class,
	/// which means you can't directly manipulate
	/// the instance from the outside of the class.
	/// Will be destroyed on load.
	/// </summary>
	/// <typeparam name="T">The type of the class inheriting from this Singleton</typeparam>
	public abstract class SingletonStatic<T> : MonoBehaviour where T : SingletonStatic<T>
	{
		protected static T Instance
		{
			get
			{
				if (!IsInit && !HasToBeInitWithFunction)
				{
					CreateInstance();
				}

				return _instance;
			}
		}

		private static T _instance;

		public static bool IsInit => _instance != null;

		public static bool HasToBeInitWithFunction = false;

		protected virtual void Awake()
		{
			if (!IsInit)
			{
				_instance = this as T;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		private static void CreateInstance()
		{
			new GameObject(typeof(T).ToString()).AddComponent<T>();
		}

		public static void Init()
		{
			if (!IsInit)
			{
				CreateInstance();
			}
			else
			{
				Debug.LogWarning($"An instance of {typeof(T)} already exists, aborting initialization");
			}
		}

		public static void ResetManager()
		{
			DestroyManager();
			Init();
		}

		public static void DestroyManager()
		{
			if (IsInit)
			{
				DestroyImmediate(_instance.gameObject);
				_instance = null;
			}
		}
	}

	/// <summary>
	/// Singleton used the same way as a static class,
	/// which means you can't directly manipulate
	/// the instance from the outside of the class.
	/// Persistant means it won't be destroyed on load
	/// once created.
	/// </summary>
	/// <typeparam name="T">The type of the class inheriting from this Singleton</typeparam>
	public abstract class SingletonStaticPersistent<T> : SingletonStatic<T> where T : SingletonStaticPersistent<T>
	{
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
	}

	/// <summary>
	/// Usual Singleton.
	/// Will be destroyed on load.
	/// </summary>
	/// <typeparam name="T">The type of the class inheriting from this Singleton</typeparam>
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		public static T Instance
		{
			get
			{
				if (!IsInit && !HasToBeInitWithFunction)
				{
					CreateInstance();
				}

				return _instance;
			}
		}

		protected static T _instance;

		public static bool IsInit => _instance != null;

		public static bool HasToBeInitWithFunction = false;

		protected virtual void Awake()
		{
			if (!IsInit)
			{
				_instance = this as T;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		private static void CreateInstance()
		{
			new GameObject(typeof(T).ToString()).AddComponent<T>();
		}

		public static void Init()
		{
			if (!IsInit)
			{
				CreateInstance();
			}
			else
			{
				Debug.LogWarning($"An instance of {typeof(T)} already exists, aborting initialization");
			}
		}

		public static void ResetManager()
		{
			DestroyManager();
			Init();
		}

		public static void DestroyManager()
		{
			if (IsInit)
			{
				Destroy(_instance.gameObject);
				_instance = null;
			}
		}
	}

	/// <summary>
	/// Usual Singleton.
	/// Persistant means it won't be destroyed on load
	/// once created.
	/// </summary>
	/// <typeparam name="T">The type of the class inheriting from this Singleton</typeparam>
	public abstract class SingletonPersistent<T> : Singleton<T> where T : SingletonPersistent<T>
	{
		protected new void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
	}
}