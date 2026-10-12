using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int NbVie;
    private PlayerController _player_control;
    private PlayerAnim _player_anim;
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<int> OnNbVieChanged;

    private bool isDead = false;

    public bool grounded {get{return _player_control.grounded;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Récupère les scripts du player
        _player_control = GetComponent<PlayerController>();
        _player_anim = GetComponent<PlayerAnim>();

        OnNbVieChanged?.Invoke(NbVie);

    }

    public void Kill()
    {
        if (isDead) return;
        isDead = true;

        // Joue l'animation de mort
        _player_control.SetFreeze(true);
        _player_anim.SetDeathAnim(true);

        NbVie--;
        OnNbVieChanged?.Invoke(NbVie);

        if (NbVie <= 0)
        {
            // Si le joueur n'a plus de vie, recharge la scène
            GameManager.instance.ReloadScene();
            return;
        }
        else 
        {
            StartCoroutine(Respawn());
        }

    }

    public void AddCoin()
    {
        score += 100;
        OnScoreChanged?.Invoke(score);
    }

    public void Bounce()
    {
        _player_control.ForceJump();
    }

    public void OnPauseInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Level.current_level.PauseLevel();
        }
    }


    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.5f);
        
        GameManager.instance.screen_transition.Show();
        yield return new WaitForSeconds(GameManager.instance.screen_transition.transition_duration); // Attend que l'écran termine son fade in
        
        // Charge le checkpoint
        Level.current_level.LoadCheckpoint();

        // Arrête l'animation de mort
        _player_anim.SetDeathAnim(false);


        GameManager.instance.screen_transition.Hide();
        yield return new WaitForSeconds(GameManager.instance.screen_transition.transition_duration); // Attend que l'écran termine son fade in
        

        // Réactive le contrôle
        _player_control.SetFreeze(false);

        isDead = false;
    }

    public void Teleport(Vector2 position)
    {
        transform.position = position;
    }

}
