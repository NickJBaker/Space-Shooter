using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] // 0 = TripleShot, 1 = Speed, 2 = Shield, 3 = TripleShot
    private int _powerUpID;
    [SerializeField]
    private AudioClip _audioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_audioClip, this.transform.position);
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0: player.TripleShotActive(); break;
                    case 1: player.SpeedActive(); break;
                    case 2: player.ShieldActive(); break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
