using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace DefaultNamespace
{
	public class PositionSaver : MonoBehaviour
	{
		[Serializable]
		public struct Data
		{
			public Vector3 Position;
			public float Time;
		}
		
		[Tooltip("Для заполнения этого поля нажмите правой кнопкой мыши по полю и выберите \"Create File\" в контекстном меню.")]
		[SerializeField] private TextAsset _json;

		[field:SerializeField, HideInInspector]
		public List<Data> Records { get; private set; }

		private void Awake()
		{
			//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			//без return будет ошибка NullReferenceException при _json=null
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json");
				return;
			}
			
			JsonUtility.FromJsonOverwrite(_json.text, this); //не найден объект _json.text
			//todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
			//если поле Records не инициализировано, создается экземпляр списка Records
			if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
			//todo comment: Зачем нужны эти проверки (что они позволяют избежать)?
			//проверяет наличие коллекции Records, проверяет коллекция пустая или нет.
			
			if (Records == null || Records.Count == 0) return;
			//если убрать проверку:
			var data = Records; //будет NullReferenceException, т.к. не инициализирована коллекция
            var prev = data[0].Position; //будет IndexOutOfRangeException, т.к. коллекция пустая
            Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
			//todo comment: Почему итерация начинается не с нулевого элемента?
			//для цикла требуется предыдущее значение, 
			for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}
		
#if UNITY_EDITOR
		[ContextMenu("Create File")]
		private void CreateFile()
		{
			//todo comment: Что происходит в этой строке?
			//создается файл Path.txt в папке Assets
			var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
			//todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав) 
			//освобождает ресурсы, которые были заняты потоком
			stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
				//todo comment: Для чего нужны эти проверки?
				if(asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
					//todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
					//код ищет только один asset с именем path, задача выполнена
					return;
				}
			}
		}

        private void OnDestroy()
        {
            if (_json == null)
            {
                Debug.LogWarning("TextAsset _json is not assigned. Cannot save data.");
                return;
            }

            if (Records == null)
            {
                Debug.LogWarning("Records list is null. Nothing to save.");
                return;
            }

            string jsonString = JsonUtility.ToJson(this, true);

            string assetPath = AssetDatabase.GetAssetPath(_json);

            if (string.IsNullOrEmpty(assetPath) || !assetPath.StartsWith("Assets/"))
            {
                Debug.LogError("Invalid asset path for _json. Cannot save data.");
                return;
            }

            try
            {
                File.WriteAllText(assetPath, jsonString);

                AssetDatabase.Refresh();

                Debug.Log($"Data successfully saved to {assetPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to {assetPath}. Error: {e.Message}");
            }
        }
#endif
    }
}