using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private int _healQuantity = 25;  // Cantidad de vida que otorga el objeto

    private void OnCollisionEnter2D(Collision2D other)
    {
        var healReceiver = other.collider.GetComponent<IHealReceiver>();  // Verifica si el objeto puede recibir curación
        if (healReceiver != null)
        {
            ApplyHeal(healReceiver);  // Aplica la curación
        }
    }

    /// <summary>
    /// Aplica la curación al jugador.
    /// </summary>
    /// <param name="player">El receptor de la curación</param>
    private void ApplyHeal(IHealReceiver player)
    {
        player.Heal(_healQuantity);
        Destroy(gameObject);  // Destruye el objeto de curación una vez utilizado
    }
}
