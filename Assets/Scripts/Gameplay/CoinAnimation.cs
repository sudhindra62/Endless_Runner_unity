
using System.Collections;
using UnityEngine;

    /// <summary>
    /// Handles the continuous rotation animation for a coin.
    /// </summary>
    public class CoinAnimation : MonoBehaviour
    {
        [Tooltip("The speed at which the coin rotates around its vertical axis.")]
        [SerializeField] private float rotationSpeed = 100f;

        private void OnEnable()
        {
            StartCoroutine(Spin());
        }

        /// <summary>
        /// A coroutine that continuously rotates the coin.
        /// </summary>
        private IEnumerator Spin()
        {
            while (true)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

