using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float laserSpeed = 8.0f;
    [SerializeField]
    private GameObject _parent;
    private bool _isEnemyLaser;

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    public void MoveUp()
    {
        transform.Translate(Vector3.up * laserSpeed * Time.deltaTime);

        if (transform.position.y > 8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * laserSpeed * Time.deltaTime);

        if (transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void EnemyLaserActive()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" & _isEnemyLaser == true)
        {
            // Even with tag, program could not reach the Player script
            Player player = GameObject.Find("Player").GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Destroy(GameObject.FindGameObjectWithTag("EnemyLeftAndRightLasers"));
        }
    }
}
