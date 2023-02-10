using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;
    private Animator _enemyDestroyedAnim;
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _fireRate;
    private float _canFire;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _enemyDestroyedAnim = GetComponent<Animator>();

        if (_enemyDestroyedAnim == null)
        {
            Debug.LogError("The enemy destroyed animation is NULL");
        }

        _explosionAudio = GetComponent<AudioSource>();

        if (_explosionAudio == null)
        {
            Debug.LogError("Explosion Audio Source is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject newEnemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(-1.3f, -1.8f, 0), Quaternion.identity);
            Laser[] lasers = newEnemyLaser.GetComponentsInChildren<Laser>();

            if (lasers != null)
            {
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].EnemyLaserActive();
                }
            }
            else
            {
                Debug.LogError("Laser Array for enemy laser is NULL");
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        // If enemy goes off screen, reuse again from top
        if (transform.position.y < -9f)
        {
            transform.position = RandomSpawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _enemyDestroyedAnim.SetTrigger("OnEnemyDeath");
            _explosionAudio.Play();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            _enemyDestroyedAnim.SetTrigger("OnEnemyDeath");
            _explosionAudio.Play();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this.gameObject, 2.8f);
        }
    }

    private Vector3 RandomSpawn()
    {
        Vector3 _randomSpawn = new Vector3(Random.Range(-9f, 9f), 10, 0);
        return _randomSpawn;
    }
}
