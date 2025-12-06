using System;
using UnityEngine;

namespace UnityBase.Base.Controller
{
    public class BehaviourController : MonoBehaviour
    {
        public float timePerform = 0.3f;
        public float timeClose = 0.02f;
        public LeanTweenType leanTweenType = LeanTweenType.linear;
        public LeanTweenType leanTweenCloseType = LeanTweenType.linear;

        private int id;
        
        public void MoveToPos(Vector3 posMoveTo, Action callBackComplete = null)
        {
            LeanTween.move(gameObject, posMoveTo, timePerform).setEase(leanTweenType).setOnComplete(callBackComplete);
        }

        public void MoveToPosLocal(Vector3 posMoveTo, Action callBackComplete = null)
        {
            LeanTween.moveLocal(gameObject, posMoveTo, timePerform).setEase(leanTweenType)
                .setOnComplete(callBackComplete);
        }

        protected void MoveUpdate(Vector3 posMoveTo, Action callBackComplete = null)
        {
            LeanTween.cancel(id);
            id = LeanTween.move(gameObject, posMoveTo, timePerform).setEase(leanTweenType)
                .setOnComplete(callBackComplete).id;
        }

        public void MoveUpdateLocal(Vector3 posMoveTo, Action callBackComplete = null)
        {
            LeanTween.cancel(id);
            id = LeanTween.moveLocal(gameObject, posMoveTo, timePerform).setOnComplete(callBackComplete)
                .setEase(leanTweenType).id;
        }

        public void RotationTo(Vector3 rotationTo, Action callBackComplete = null)
        {
            LeanTween.rotate(gameObject, rotationTo, timePerform).setEase(leanTweenType)
                .setOnComplete(callBackComplete);
        }

        protected void RotationUpdate(Vector3 rotationTo)
        {
            LeanTween.cancel(id);
            id = LeanTween.rotate(gameObject, rotationTo, timePerform).setEase(leanTweenType).id;
        }

        protected void UpdateValue(float firstValue, float lastValue, Action<float> updateValue = null,
            Action onComplete = null)
        {
            LeanTween.cancel(id);
            id = LeanTween.value(gameObject, updateValue, firstValue, lastValue, timePerform)
                .setEase(LeanTweenType.linear).setOnComplete(onComplete).id;
        }

        protected void UpdateValue(Vector2 firstValue, Vector2 lastValue, Action<Vector2> updateValue = null,
            Action onComplete = null)
        {
            LeanTween.cancel(id);
            id = LeanTween.value(gameObject, updateValue, firstValue, lastValue, timePerform)
                .setEase(LeanTweenType.linear).setOnComplete(onComplete).id;
        }

        public void ScaleUpdate(Vector3 ScaleValue)
        {
            LeanTween.cancel(id);
            id = LeanTween.scale(gameObject, ScaleValue, timePerform).setIgnoreTimeScale(true).setEase(leanTweenType)
                .id;
        }

        public void ScaleTo(Vector3 ScaleValue, Action callBackComplete = null)
        {
            LeanTween.scale(gameObject, ScaleValue, timePerform).setEase(leanTweenType).setOnComplete(callBackComplete)
                .setIgnoreTimeScale(true);
        }

        protected void ScaleToClose(Vector3 ScaleValue, Action callBackComplete = null)
        {
            LeanTween.scale(gameObject, ScaleValue, timeClose).setEase(leanTweenCloseType)
                .setOnComplete(callBackComplete);
        }

        protected void UpdateColor(Color firstColor, Color lastColor, Action<Color> updateValue = null,
            Action onComplete = null)
        {
            LeanTween.cancel(id);
            id = LeanTween.value(gameObject, updateValue, firstColor, lastColor, timePerform).setEase(leanTweenType)
                .setOnComplete(onComplete).id;
        }
    }
}