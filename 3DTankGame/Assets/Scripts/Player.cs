using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Movement))]
[RequireComponent (typeof(Tank))]


public class Player : MonoBehaviour
{
    Tank tank;
    Movement movement;
    SoundManager soundManager;
    FadeImage fadeImage;

    [SerializeField] private Image blackout;
    void Start()
    {
        movement = GetComponent<Movement>();
        tank = GetComponent<Tank>();
        fadeImage = gameObject.AddComponent<FadeImage>();
        fadeImage.imageToFade = blackout;
        soundManager = FindAnyObjectByType<SoundManager>();
        soundManager.startEngine();

    }

    void Update()
    {
        if (tank.justDied)
        {
            tank.justDied = false;
            fadeImage.Startfading();
        }

        movement.dirX = Input.GetAxisRaw("Horizontal");
        movement.dirZ = Input.GetAxisRaw("Vertical");

        if(movement.dirZ != 0){soundManager.revEngine();}
        else{soundManager.stopRevEngine();}

        float lookX = 0;
        float lookY = 0;
        float gunRotation = tank.gun.localEulerAngles.x;
        if (gunRotation > 180){gunRotation -= 360;}

        lookX = Input.GetAxis("RightStickHorizontal");
        lookY = Input.GetAxis("RightStickVertical");
        if (Input.GetKey(KeyCode.J)){ lookX = -1; }
        if (Input.GetKey(KeyCode.L)){ lookX =  1; }
        if (Input.GetKey(KeyCode.I)){ lookY =  1; }
        if (Input.GetKey(KeyCode.K)){ lookY = -1; }

        if (gunRotation < -35 && lookY > 0){lookY = 0;}
        if (gunRotation > 10 && lookY < 0){lookY = 0;}


        Vector3 rollDirection = new Vector3(lookX, 0, 0);
        if (rollDirection != Vector3.zero)
        {
            Quaternion targetTurretRotation = Quaternion.LookRotation(rollDirection);
            tank.turret.localRotation = Quaternion.Lerp(tank.turret.localRotation, targetTurretRotation, Time.deltaTime * 1f);
        }

        Vector3 pitchDirection = new Vector3(0,lookY, 0);
        if (pitchDirection != Vector3.zero)
        {
            Quaternion targetGunRotation = Quaternion.LookRotation(pitchDirection);
            float angle = targetGunRotation.eulerAngles.x * Mathf.Rad2Deg;
            tank.gun.localRotation = Quaternion.Lerp(tank.gun.localRotation, targetGunRotation, Time.deltaTime * 0.5f);
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) || Input.GetAxis("TankFire") < 0)
        {
            soundManager.playShot();
            tank.Shoot();
        }else{soundManager.stopShot();
        }
    }

}
