using UnityEditor;
using UnityEngine;

namespace Navigation
{
	[CreateAssetMenu(menuName = "TrainingBuddy/Scene Reference", fileName = "SceneReference")]
	public class SceneReference : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField] private string _sceneName;
		public string SceneName => _sceneName;

#if UNITY_EDITOR
		[SerializeField] private SceneAsset _scene;
#endif

		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			if (_scene != null && _sceneName != _scene.name)
			{
				_sceneName = _scene.name;
				EditorUtility.SetDirty(this);
				AssetDatabase.SaveAssetIfDirty(this);
			}
#endif
		}

		public void OnAfterDeserialize()
		{
			throw new System.NotImplementedException();
		}
	}
}