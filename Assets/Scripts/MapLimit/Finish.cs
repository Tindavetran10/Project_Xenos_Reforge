using UnityEngine;

namespace MapLimit
{
    public class Finish : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.PlayerStateMachine.Player>() != null) 
                GameObject.Find("Canvas").GetComponent<UI.MenuManager>().SwitchOnWinScreen();
        }
    }
}
