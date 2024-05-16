using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class SceneManager : MonoBehaviour
    {
        BattleSceneManager battleSceneManager;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
        }

        public void SelectBattleType(string e)
        {
            StartCoroutine(LoadBattle(e));
        }
        public IEnumerator LoadBattle(string e)
        {
            Cursor.lockState = CursorLockMode.Locked;
            yield return new WaitForSeconds(1);

            battleSceneManager.StartFight();

            yield return new WaitForSeconds(1);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
