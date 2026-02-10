using System;
using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
		private PositionSaver _save;
		private float _currentDelay;

        //todo comment: Что произойдёт, если _delay > _duration?
        //_duration - общее время жизни скрипта
        //_delay - интервал между записями
        //при уменьшении этих значений на одинаковое число каждый кадр, _duration раньше выполнит условие (_duration <= 0f)
        //и выключит скрипт, так и не выполнив условие (_currentDelay <= 0f), где производится запись в коллекцию
        [Range(0.2f, 1.0f)]
		[SerializeField]private float _delay = 0.5f;
		[Min(0.2f)]
		[SerializeField]private float _duration = 5f;
		
		
		private void Start()
		{
			if(_duration < _delay)
			{
				_duration = _delay * 5f;
			}
			//todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
			//GetComponent достаточно вызвать в Start, чтобы не выполнять поиск каждый кадр, это сильно дорого
			_save = GetComponent<PositionSaver>();
			_save.Records.Clear();
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}

            //todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
            //_delay в роли константы.
            //_currentDelay уменьшается каждый кадр и обновляется на _delay при выполнении условия (_currentDelay <= 0f)
            _currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.Records.Add(new PositionSaver.Data
				{
					Position = transform.position,
					//todo comment: Для чего сохраняется значение игрового времени?
					//для хронологической привязки позиции Vector3, чтобы в дальнейшем воспроизвести траекторию перемещения объекта 
					Time = Time.time,
				});
			}
		}
	}
}