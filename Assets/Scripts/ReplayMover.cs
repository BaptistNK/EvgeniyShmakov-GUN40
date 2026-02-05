using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
            //todo comment: зачем нужны эти проверки?
            //Скрипт зависит от компонента PositionSaver + проверка наличия записей в коллекции
            if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				//прекращение работы скрипта в случае отсутствия компонента PositionSaver
				//или пустой коллекции Records. 
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
			//todo comment: Что проверяет это условие (с какой целью)? 
			//сравнение текущего игрового времени с временем из коллекции _save.Records.
			//проверяет наступила ли очередь конкретной записи
			if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
                //todo comment: Для чего нужна эта проверка?
                //когда пройдут все точки, отключится компонент. без этой проверки выпадет исключение IndexOutOfRangeException
                if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
            //todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            //вычисляется коэффициент прогресса в перемещении (при значении delta 0.4f объект переместился
            //от точки _prev.Position на 40% до точки curr.Position)
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
            //todo comment: Зачем нужна эта проверка?
            //защищает от некорректных математических операций (когда curr.Time - _prev.Time = 0, т.к. деление на 0 дает NaN delta=NaN)
            if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            /*
			1. свойство компонента Transform игрового объекта задает текущую позицию в мировом пространстве
			2. Vector3.Lerp вектор перемещения из точки _prev.Position в точку curr.Position, delta - это % пройденного расстояния
			3. transform.position пересчитывается каждый кадр
			*/
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}