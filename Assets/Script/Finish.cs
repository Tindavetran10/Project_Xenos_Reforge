using UnityEngine;

namespace Script
{
    public class Finish : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collison)
        {
            if (collison.GetComponent<Player.PlayerStateMachine.Player>() != null) 
                GameObject.Find("Canvas").GetComponent<UI.UI>().SwitchOnWinScreen();
        }
    }
}
