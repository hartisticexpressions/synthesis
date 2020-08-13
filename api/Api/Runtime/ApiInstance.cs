using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.Utilities;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using Component = SynthesisAPI.EnvironmentManager.Component;

namespace SynthesisAPI.Runtime
{
	internal abstract class ApiInstance : MarshalByRefObject
	{
		public abstract void AddEntityToScene(Entity entity);

		public abstract void RemoveEntityFromScene(Entity entity);

		#nullable enable

		public abstract Component? AddComponentToScene(Entity entity, Type t);

		public abstract void AddComponentToScene(Entity entity, Component component);

		public abstract void RemoveComponentFromScene(Entity entity, Type t);

		public abstract T CreateUnityType<T>(params object[] args) where T : class;

		public abstract VisualTreeAsset GetDefaultUIAsset(string assetName);

		public abstract UnityEngine.UIElements.VisualElement GetRootVisualElement();
	}
}
