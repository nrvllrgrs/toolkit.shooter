using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ToolkitEngine.Shooter
{
	public abstract class BaseShooterModifier : MonoBehaviour
	{
		#region Fields

		[SerializeField, HideInInspector]
		protected ModifiablePropertyFilters m_filters = new();

		#endregion

		#region Properties

		internal abstract string[] propertyNames { get; }

		#endregion

		#region  Editor Only
#if UNITY_EDITOR

		protected virtual void OnValidate()
		{
			foreach (var name in propertyNames)
			{
				if (!m_filters.ContainsKey(name))
				{
					m_filters.Add(name, new UnityEvaluator());
				}
			}
		}

#endif
		#endregion

		#region Structures

		[System.Serializable]
		public class ModifiablePropertyFilters : SerializableDictionary<string, UnityEvaluator>
		{ }

		#endregion
	}

	public abstract class BaseShooterModifier<T> : BaseShooterModifier
		where T : Component
	{
		#region Fields

		private T m_target;
		private Dictionary<string, ModifierData> m_modifiers = new();
		private Dictionary<string, PropertyData> m_properties = new();

		#endregion

		#region Properties

		public T target
		{
			get => m_target;
			set
			{
				// No change, skip
				if (m_target == value)
					return;

				if (m_target != null)
				{
					ResetAll();
				}

				m_target = value;

				if (m_target != null)
				{
					Setup();
					UpdateAll();
				}
			}
		}

		#endregion

		#region Methods

		public void SetTarget(GameObject obj)
		{
			target = obj?.GetComponent<T>();
		}

		protected virtual void Awake()
		{
			foreach (var name in propertyNames)
			{
				m_modifiers.Add(name, new ModifierData()
				{
					bonus = 0f,
					factor = 1f
				});
			}

			m_target = m_target ?? GetComponent<T>();
			Setup();
		}

		private void Setup()
		{
			if (m_target == null)
				return;

			// Get default values and members
			var type = m_target.GetType();
			foreach (var name in propertyNames)
			{
				var info = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
				if (info != null)
				{
					m_properties.Add(name, new PropertyData()
					{
						defaultValue = info.GetValue(m_target),
						info = info,
					});
				}
			}
		}

		private void ResetAll()
		{
			foreach (var v in m_properties.Values)
			{
				v.info.SetValue(m_target, v.defaultValue);
			}
			m_properties.Clear();
		}

		private void UpdateAll()
		{
			foreach (var k in m_properties.Keys)
			{
				UpdateValue(k);
			}
		}

		private void UpdateValue(string key)
		{
			if (!m_properties.TryGetValue(key, out var propertyData))
				return;

			if (!m_modifiers.TryGetValue(key, out var modifierData))
				return;

			if (!m_filters.TryGetValue(key, out var evaluator) || evaluator.Evaluate(gameObject, target.gameObject) == 0)
				return;

			if (propertyData.info.PropertyType == typeof(float))
			{
				float value = ((float)propertyData.defaultValue * modifierData.factor) + modifierData.bonus;
				propertyData.info.SetValue(m_target, value);
			}
			else if (propertyData.info.PropertyType == typeof(int))
			{
				float value = ((int)propertyData.defaultValue * modifierData.factor) + modifierData.bonus;
				propertyData.info.SetValue(m_target, (int)value);
			}
		}

		#endregion

		#region Modifier Methods

		public void ModifyByValue(string key, int value)
		{
			if (!m_modifiers.TryGetValue(key, out var modifierData))
				return;

			modifierData.bonus += value;
			UpdateValue(key);
		}

		public void ModifyByValue(string key, float value)
		{
			if (!m_modifiers.TryGetValue(key, out var modifierData))
				return;

			modifierData.bonus += value;
			UpdateValue(key);
		}

		public void ModifyByFactor(string key, float value)
		{
			if (!m_modifiers.TryGetValue(key, out var modifierData))
				return;

			modifierData.factor += value;
			UpdateValue(key);
		}

		#endregion

		#region Structures

		public class ModifierData
		{
			public float bonus;
			public float factor;
		}

		public class PropertyData
		{
			public object defaultValue;
			public PropertyInfo info;
		}

		#endregion
	}
}