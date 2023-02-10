using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // When private, variable name = _variableName
    [SerializeField]
    private float _speed = 7.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _firingRate = .15f;
    private float _canFire = -1f;
    public int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;

    private bool _isTripleShotEnabled = false;
    private bool _isSpeedEnabled = false;
    private bool _isShieldEnabled = false;

    // Create a variable referance for shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    public int _score;

    private UIManager _UIManager;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private AudioSource _laserAudioSource;
    [SerializeField]
    private AudioClip _laserAudioClip;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _laserAudioSource = GetComponent<AudioSource>();
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
        

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_laserAudioSource == null)
        {
            Debug.LogError("Laser audio source is NULL");
        }
        else
        {
            _laserAudioSource.clip = _laserAudioClip;
        }

        if ( _gameManager == null)
        {
            Debug.LogError("Game Manager script is NULL");
        }
    }
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        if (_isSpeedEnabled == true)
        {
            transform.Translate(direction * (_speed + 4.0f) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        // Creating the player bounds and map wrapping
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.0f, 0), 0);

        if (transform.position.x < -11.26f)
        {
            transform.position = new Vector3(11.26f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.26f)
        {
            transform.position = new Vector3(-11.26f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        _canFire = Time.time + _firingRate;
        if (_isTripleShotEnabled == true)
        {
            GameObject newTripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            if (_tripleShotPrefab.transform.position.y > 8.0f)
            {
                Destroy(newTripleShot.gameObject);
            }
        }
        else 
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        }
        _laserAudioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldEnabled == true)
        {
            _isShieldEnabled = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _UIManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _UIManager.CheckForBestScore();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotEnabled = false;
    }

    public void SpeedActive()
    {
        _isSpeedEnabled = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedEnabled = false;
    }

    public void ShieldActive()
    {
        _isShieldEnabled = true;
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isShieldEnabled = false;
    }

    public int AddToScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
        return _score;
    }
}
