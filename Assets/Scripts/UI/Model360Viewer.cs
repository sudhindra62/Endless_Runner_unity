
using UnityEngine;
using UnityEngine.EventSystems;

    public class Model360Viewer : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public Transform target;
        public float rotationSpeed = 200f;

        private Vector2 lastInputPosition;
        private bool isRotating = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (target != null)
            {
                isRotating = true;
                lastInputPosition = eventData.position;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isRotating || target == null) return;

            Vector2 delta = eventData.position - lastInputPosition;
            lastInputPosition = eventData.position;

            float rotationAmount = -delta.x * rotationSpeed * Time.deltaTime;
            target.Rotate(Vector3.up, rotationAmount, Space.World);
        }
    }
