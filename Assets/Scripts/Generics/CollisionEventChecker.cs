using System;
using UnityEngine;

namespace Generics
{
	[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
	public class CollisionEventChecker : MonoBehaviour
	{
		public event Action<Collision, CollisionEventChecker> OnCollisionEnterEvent;
		public event Action<Collision, CollisionEventChecker> OnCollisionExitEvent;
		public event Action<Collision, CollisionEventChecker> OnCollisionStayEvent;
		
		public event Action<Collider, CollisionEventChecker> OnTriggerEnterEvent;
		public event Action<Collider, CollisionEventChecker> OnTriggerExitEvent;
		public event Action<Collider, CollisionEventChecker> OnTriggerStayEvent;

		private void OnCollisionEnter(Collision collision) => OnCollisionEnterEvent?.Invoke(collision, this);

		private void OnCollisionExit(Collision collision) => OnCollisionExitEvent?.Invoke(collision, this);

		private void OnCollisionStay(Collision collision) => OnCollisionStayEvent?.Invoke(collision, this);

		private void OnTriggerEnter(Collider other) => OnTriggerEnterEvent?.Invoke(other, this);

		private void OnTriggerExit(Collider other) => OnTriggerExitEvent?.Invoke(other, this);

		private void OnTriggerStay(Collider other) => OnTriggerStayEvent?.Invoke(other, this);
	}
}